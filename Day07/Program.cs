using System.Text;
using System.Text.RegularExpressions;


List<ulong[]> equations = ParseInput("../../../input.txt");

ulong sum = 0;
ulong sumWithConcat = 0;

foreach (ulong[] equation in equations) {
    if (CanBeCalculated(equation, 2)) sum += equation[0];
    if (CanBeCalculated(equation, 3)) sumWithConcat += equation[0];
}

Console.WriteLine("The sum of all equations that can be calculated is {0}", sum);
Console.WriteLine("The sum of all equations that can be calculated including the concatenation is {0}", sumWithConcat);
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

bool CanBeCalculated(ulong[] equation, byte operatorCount, ulong total = 0, byte op = 0, int depth = -1) {
    if (depth + 2 >= equation.Length) return total == equation[0];
    
    total = depth < 0 ? equation[1] : Calculate(total, equation[depth + 2], op);
    if (total > equation[0]) return false;
    
    for (byte i = 0; i < operatorCount; i++) {
        if (CanBeCalculated(equation, operatorCount, total, i, depth + 1)) return true;
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
    int digits = operand2 == 0 ? 1 : (int) Math.Floor(Math.Log10(operand2) + 1);
    return operand1 * (ulong) Math.Pow(10, digits) + operand2;
}