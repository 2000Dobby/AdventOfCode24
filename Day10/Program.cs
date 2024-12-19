int[,] map = ParseInput("../../../input.txt");
List<(int, int)> trailHeads = FindTrailHeads(map);

int scoreTotal = 0;
int ratingTotal = 0;

foreach (var trailHead in trailHeads) {
    List<(int, int)> nines = [];
    GetNinesReachable(map, trailHead, nines);
    ratingTotal += nines.Count;
    scoreTotal += CountUniquePositions(nines);
}

Console.WriteLine("The sum of the scores of all trailheads is {0}", scoreTotal);
Console.WriteLine("The sum of the ratings of all trailheads is {0}", ratingTotal);

return;


int[,] ParseInput(string path) {
    string[] lines = File.ReadAllLines(path);

    int rows = lines.Length;
    int cols = lines[0].Length;
    int[,] grid = new int[rows, cols];

    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            grid[i, j] = lines[i][j] - 48;
        }
    }

    return grid;
}

List<(int, int)> FindTrailHeads(int[,] map) {
    List<(int, int)> trailHeads = [];

    for (int x = 0; x < map.GetLength(0); x++) {
        for (int y = 0; y < map.GetLength(1); y++) {
            if (map[x, y] != 0) continue;
            trailHeads.Add((x, y));
        }
    }

    return trailHeads;
}

void GetNinesReachable(int[,] map, (int, int) current, List<(int, int)> nines) {
    if (map[current.Item1, current.Item2] == 9) {
        nines.Add(current);
        return;
    }

    if (ValidNextStep(map, current, (0, 1))) GetNinesReachable(map, Add(current, (0, 1)), nines);
    if (ValidNextStep(map, current, (1, 0))) GetNinesReachable(map, Add(current, (1, 0)), nines);
    if (ValidNextStep(map, current, (0, -1))) GetNinesReachable(map, Add(current, (0, -1)), nines);
    if (ValidNextStep(map, current, (-1, 0))) GetNinesReachable(map, Add(current, (-1, 0)), nines);
}

bool ValidNextStep(int[,] map, (int, int) from, (int, int) dir) {
    (int, int) to = Add(from, dir);
    int current = map[from.Item1, from.Item2];

    return to.Item1 >= 0 && to.Item2 >= 0 && to.Item1 < map.GetLength(0) 
        && to.Item2 < map.GetLength(1) && map[to.Item1, to.Item2] == current + 1;
}

int CountUniquePositions(List<(int, int)> positions) {
    int uniquePositions = 0;
    for (int i = 0; i < positions.Count; i++) {
        bool isUnique = true;

        for (int j = i + 1; j < positions.Count; j++) {
            if (positions[i] != positions[j]) continue;
            isUnique = false;
            break;
        }

        if (isUnique) uniquePositions++;
    }
    return uniquePositions;
}

(int, int) Add((int, int) a, (int, int) b) {
    return (a.Item1 + b.Item1, a.Item2 + b.Item2);
}