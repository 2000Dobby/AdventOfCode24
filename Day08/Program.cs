char[,] grid = ParseInput("../../../input.txt");
var similarFrequencies = FindSimilarFrequencies(grid);

int antinodes = CountAntinodes(grid, similarFrequencies);
int antinodesHarmonics = CountAntinodes(grid, similarFrequencies, true);

Console.WriteLine("There are {0} unique antinodes", antinodes);
Console.WriteLine("Accounting for resonant harmonics there are {0} unique antinodes", antinodesHarmonics);
return;


char[,] ParseInput(string path) {
    string[] lines = File.ReadAllLines(path);

    int rows = lines.Length;
    int cols = lines[0].Length;

    char[,] grid = new char[rows, cols];

    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            char val = lines[i][j];
            grid[i, j] = val;
        }
    }

    return grid;
}

Dictionary<char, List<(int, int)>> FindSimilarFrequencies(char[,] grid) {
    var similarFrequencies = new Dictionary<char, List<(int, int)>>();

    for (int x = 0; x < grid.GetLength(0); x++) {
        for (int y = 0; y < grid.GetLength(1); y++) {
            char frequency = grid[x, y];
            if (frequency == '.') continue;
            
            if (similarFrequencies.TryGetValue(frequency, out List<(int, int)>? locations)) {
                locations.Add((x, y));
                continue;
            }

            locations = [(x, y)];
            similarFrequencies.Add(frequency, locations);
        }
    }
    
    return similarFrequencies;
}

int CountAntinodes(char[,] grid, Dictionary<char, List<(int, int)>> similarFrequencies, bool harmonics = false) {
    char[,] antinodes = new char[grid.GetLength(0), grid.GetLength(1)];

    foreach (var locations in similarFrequencies.Values) {
        FindAntinodes(locations, antinodes, harmonics);
    }

    int count = 0;
    for (int x = 0; x < antinodes.GetLength(0); x++) 
        for (int y = 0; y < antinodes.GetLength(1); y++) 
            if (antinodes[x, y] == '#') count++;
    
    return count;
}

void FindAntinodes(List<(int, int)> locations, char [,] antinodes, bool harmonics) {
    if (locations.Count < 2) return;

    int w = antinodes.GetLength(0);
    int h = antinodes.GetLength(1);
    
    int max = locations.Count - 1;
    for (int i = 0; i < max; i++) {
        (int, int) loc1 = locations[i];
        
        for (int j = i + 1; j < locations.Count; j++) {
            (int, int) loc2 = locations[j];
            
            int dX = loc2.Item1 - loc1.Item1;
            int dY = loc2.Item2 - loc1.Item2;

            PlaceAntinodes(loc1.Item1, loc1.Item2, antinodes, -dX, -dY, w, h, harmonics);
            PlaceAntinodes(loc2.Item1, loc2.Item2, antinodes, dX, dY, w, h, harmonics);
        }
    }
}

void PlaceAntinodes(int x, int y, char[,] antinodes, int dX, int dY, int w, int h, bool harmoncis) {
    if (!harmoncis) {
        x += dX;
        y += dY;
        
        if (x >= 0 && y >= 0 && x < w && y < h) antinodes[x, y] = '#';
        return;
    }
    
    while (x >= 0 && y >= 0 && x < w && y < h) {
        antinodes[x, y] = '#';
        
        x += dX;
        y += dY;
    }
}