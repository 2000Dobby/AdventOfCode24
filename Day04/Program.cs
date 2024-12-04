using System.Text;


char[,] grid = ParseInput("../../../input.txt");
Console.WriteLine("The number of XMAS in the grid is {0}", CountXmas(grid));
Console.WriteLine("The number of X-MAS in the grid is {0}", CountCrossMas(grid));


char[,] ParseInput(string path) {
    using var file = File.OpenRead(path);
    using var reader = new StreamReader(file, Encoding.UTF8);

    List<char[]> lines = [];
    string? line;

    while((line = reader.ReadLine()) != null) {
        lines.Add(line.ToCharArray());
    }

    char[,] grid = new char[lines.Count, lines[0].Length];
    for (int i = 0; i < lines.Count; i++) {
        for (int j = 0; j < lines[i].Length; j++) {
            grid[i, j] = lines[i][j];
        }
    }

    return grid;
}

int HorizontalXmas(int x, int y, char[,] grid) {
    if (x + 3 >= grid.GetLength(0)) return 0;
    string xmas = new([grid[x, y], grid[x + 1, y], grid[x + 2, y], grid[x + 3, y]]);
    return xmas == "XMAS" || xmas == "SAMX" ? 1 : 0;
}

int VerticalXmas(int x, int y, char[,] grid) {
    if (y + 3 >= grid.GetLength(1)) return 0;
    string xmas = new([grid[x, y], grid[x, y + 1], grid[x, y + 2], grid[x, y + 3]]);
    return xmas == "XMAS" || xmas == "SAMX" ? 1 : 0;
}

int DiagonalXmas(int x, int y, char[,] grid) {
    if (x + 3 >= grid.GetLength(0) || y + 3 >= grid.GetLength(1)) return 0;
    string xmas = new([grid[x, y], grid[x + 1, y + 1], grid[x + 2, y + 2], grid[x + 3, y + 3]]);
    return xmas == "XMAS" || xmas == "SAMX" ? 1 : 0;
}

int DiagonalXmasReverse(int x, int y, char[,] grid) {
    if (x - 3 < 0 || y + 3 >= grid.GetLength(1)) return 0;
    string xmas = new([grid[x, y], grid[x - 1, y + 1], grid[x - 2, y + 2], grid[x - 3, y + 3]]);
    return xmas == "XMAS" || xmas == "SAMX" ? 1 : 0;
}

int CrossMas(int x, int y, char[,] grid) {
    if (x + 2 >= grid.GetLength(0) || y + 2 >= grid.GetLength(1)) return 0;
    string firstMas = new([grid[x, y], grid[x + 1, y + 1], grid[x + 2, y + 2]]);
    string secondMas = new([grid[x + 2, y], grid[x + 1, y + 1], grid[x, y + 2]]);
    return (firstMas == "MAS" || firstMas == "SAM") && (secondMas == "MAS" || secondMas == "SAM") ? 1 : 0;
}

int CountXmas(char[,] grid) {
    int xmasCount = 0;
    for (int x = 0; x < grid.GetLength(0); x++) {
        for (int y = 0; y < grid.GetLength(1); y++) {
            xmasCount += HorizontalXmas(x, y, grid);
            xmasCount += VerticalXmas(x, y, grid);
            xmasCount += DiagonalXmas(x, y, grid);
            xmasCount += DiagonalXmasReverse(x, y, grid);
        }
    }

    return xmasCount;
}

int CountCrossMas(char[,] grid) {
    int crossMasCount = 0;

    for (int x = 0; x < grid.GetLength(0); x++) {
        for (int y = 0; y < grid.GetLength(1); y++) {
            crossMasCount += CrossMas(x, y, grid);
        }
    }

    return crossMasCount;
}