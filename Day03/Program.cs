using System.Text;
using System.Text.RegularExpressions;

Regex regex = new(@"mul\(\d{1,3},\d{1,3}\)");
string content = FileContents("../../../input.txt");

int sum = regex.Matches(content).Select(ins => ParseInstruction(ins.Value)).Aggregate((a, b) => a + b);
Console.WriteLine("The sum of the multiplication instructions is {0}", sum);


Regex regexWithDo = new(@"(mul\(\d{1,3},\d{1,3}\))|(do\(\))|(don't\(\))");

sum = 0;
bool dont = false;
foreach (Match match in regexWithDo.Matches(content)) {
    string val = match.Value;

    switch (val) {
        case "do()":
            dont = false;
            break;
        case "don't()":
            dont = true;
            break;
        default:
            if (!dont) sum += ParseInstruction(val);
            break;
    }
}

Console.WriteLine("The sum of the multiplication instructions with do() and don't() is {0}", sum);


string FileContents(string path) {
    using var file = File.OpenRead(path);
    using var reader = new StreamReader(file, Encoding.UTF8);

    return reader.ReadToEnd();
}

int ParseInstruction(string instruction) {
    string[] parts = instruction.Split(",");
    int a = int.Parse(parts[0].Substring(4));
    int b = int.Parse(parts[1].Substring(0, parts[1].Length - 1));
    return a * b;
}