using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

var sw = Stopwatch.StartNew();
List<ulong[]> equations = ParseInput("../../../input.txt");

ulong sum = 0;
ulong sumWithConcat = 0;

foreach (ulong[] equation in equations) {
    if (CanBeCalculated(equation, 2)) sum += equation[0];
    if (CanBeCalculated(equation, 3)) sumWithConcat += equation[0];
}
sw.Stop();

Console.WriteLine("The sum of all equations that can be calculated is {0}", sum);
Console.WriteLine("The sum of all equations that can be calculated including the concatenation is {0}", sumWithConcat);
Console.WriteLine("The calculation took {0}ms", sw.ElapsedMilliseconds);
return;


List<ulong[]> ParseInput(string path) {
    Regex regex = new Regex(@"\d+");    
    
    using var file = File.OpenRead(path);
    using var reader = new StreamReader(file, Encoding.UTF8);

    List<ulong[]> equations = [];

    while (reader.ReadLine() is { } line) {
        var matches = regex.Matches(line);
        ulong[] equation = new ulong[matches.Count];

        for (int i = 0; i < matches.Count; i++) {
            equation[i] = ulong.Parse(matches[i].Value);
        }
        
        equations.Add(equation);
    }
    
    return equations;
}

bool CanBeCalculated(ulong[] equation, byte operatorCount) {
    var stack = new Stack<(ulong total, int depth)>();
    stack.Push((equation[1], 0));
    
    while (stack.Count > 0) {
        var (total, depth) = stack.Pop();
        int nextIndex = depth + 2;

        if (nextIndex >= equation.Length) {
            if (total == equation[0]) return true;
            continue;
        }
        
        for (byte op = 0; op < operatorCount; op++) {
            ulong nextTotal = Calculate(total, equation[nextIndex], op);
            if (nextTotal <= equation[0]) stack.Push((nextTotal, depth + 1));
        }
    }
    
    return false;
}


ulong Calculate(ulong operand1, ulong operand2, byte op) {
    return op switch {
        0 => operand1 + operand2,
        1 => operand1 * operand2,
        2 => Concat(operand1, operand2),
        _ => throw new ArgumentException("Invalid operator")
    };
}

ulong Concat(ulong operand1, ulong operand2) {
    ulong multiplier = 1;
    while (operand2 >= multiplier) multiplier *= 10;
    
    return operand1 * multiplier + operand2;
}