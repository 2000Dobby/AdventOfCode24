using System.Text;


List<int> left, right;
(left, right) = ReadInput("../../../input.txt");

int totalDist = 0;
for (int i = 0; i < left.Count; i++) {
   totalDist += Dist(left[i], right[i]);
}

int similarityScore = 0;
for (int i = 0; i < left.Count; i++) {
    int value = left[i];
    similarityScore += value * CountOccurrences(value, right);
}

Console.WriteLine("The lists have a total distance of {0}", totalDist);
Console.WriteLine("The lists have a similarity score of {0}", similarityScore);


int Dist(int a, int b) => Math.Abs(a - b);
(List<int>, List<int>) ReadInput(string path) {
    List<int> left = [], right = [];

    using var file = File.OpenRead(path);
    using var reader = new StreamReader(file, Encoding.UTF8);

    string? line;
    while ((line = reader.ReadLine()) != null) {
        string[] parts = line.Split("   ");
        left.Add(int.Parse(parts[0]));
        right.Add(int.Parse(parts[1]));
    }

    left.Sort();
    right.Sort();

    return (left, right);
}
int CountOccurrences(int number, List<int> right) {
    int count = 0;
    for (int i = 0; i < right.Count; i++) {
        if (right[i] > number) break;
        if (right[i] == number) count++;
    }
    return count;
}