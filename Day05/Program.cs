using Day05;
using System.Text;

List<(int, int)> rules;
List<int[]> updates;

(rules, updates) = ParseInput("../../../input.txt");
RuleBasedComparer comparer = new(rules);

int correctSum = 0;
int sortedSum = 0;

foreach (var update in updates) {
    if (AnyRuleViolated(update, rules)) {
        Array.Sort(update, comparer);
        sortedSum += GetCenterPage(update);
        continue;
    } 

    correctSum += GetCenterPage(update);
}

Console.WriteLine("The sum of the center page numbers of all valid updates is {0}", correctSum);
Console.WriteLine("The sum of the center page numbers of all invalid updates is {0}", sortedSum);



(List<(int, int)>, List<int[]>) ParseInput(string path) {
    using var file = File.OpenRead(path);
    using var reader = new StreamReader(file, Encoding.UTF8);

    List<(int, int)> rules = [];
    List<int[]> updates = [];

    bool parsingRules = true;
    string? line;

    while ((line = reader.ReadLine()) != null) {
        if (string.IsNullOrWhiteSpace(line)) {
            parsingRules = false;
            continue;
        }

        if (parsingRules) {
            string[] rule = line.Split("|");
            rules.Add((Convert.ToInt32(rule[0]), Convert.ToInt32(rule[1])));
        } else {
            string[] update = line.Split(",");
            updates.Add(update.Select(up => Convert.ToInt32(up)).ToArray());
        }
    }

    return (rules, updates);
}

bool RuleViolated(int[] update, (int, int) rule) {
    bool bFound = false;

    foreach (int page in update) {
        if (page == rule.Item1) return bFound;
        else if (page == rule.Item2) bFound = true;
    }

    return false;
}

bool AnyRuleViolated(int[] update, List<(int, int)> rules) {
    return rules.Any(rule => RuleViolated(update, rule));
}

int GetCenterPage(int[] update) {
    return update[update.Length / 2];
}