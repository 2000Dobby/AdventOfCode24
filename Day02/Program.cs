using System.Text;

List<List<int>> reports = ReadInput("../../../input.txt");
int safeReportsUndampened = reports.Count(AreLevelsSafe);
int safeReportsDampened = reports.Count(AreLevelsSafeDampened);

Console.WriteLine("Without using the Problem Dampener {0} reports are safe", safeReportsUndampened);
Console.WriteLine("Using the Problem Dampener {0} reports are safe", safeReportsDampened);


List<List<int>> ReadInput(string path) {
    List<List<int>> reports = [];

    using var file = File.OpenRead(path);
    using var reader = new StreamReader(file, Encoding.UTF8);

    string? line;
    while ((line = reader.ReadLine()) != null) {
        reports.Add(line.Split(" ").Select(int.Parse).ToList());
    }

    return reports;
}

bool AreLevelsSafe(List<int> levels) {
    if (levels.Count < 2) return true;

    bool increasing = levels[0] < levels[1];
    int a, b, diff;

    for (int i = 0; i < levels.Count - 1; i++) {
        a = levels[i];
        b = levels[i + 1];

        diff = Math.Abs(a - b);
        if (diff < 1 || diff > 3 || increasing && a > b || !increasing && a < b) return false;
    }

    return true;
}

bool AreLevelsSafeDampened(List<int> levels) {
    List<int>[] permutations = new List<int>[levels.Count];

    for (int i = 0; i < permutations.Length; i++) {
        List<int> permutation = [];

        for (int j = 0; j < levels.Count; j++) {
            if (i == j) continue;
            permutation.Add(levels[j]);
        }

        permutations[i] = permutation;
    }

    return permutations.Any(AreLevelsSafe);
}