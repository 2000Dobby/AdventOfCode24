using System.Text;


char[,] grid = ParseInput("../../../input.txt");
(int, int) startingPosition = FindStartingPosition(grid);

IsRunningInCircle(grid, startingPosition);
Console.WriteLine("The guard visited {0} distinct positions", CountPositionsVisited(grid));
Console.WriteLine("The number of possible loops is {0}", CountObstaclePlacements(grid, startingPosition));


char[,] ParseInput(string path) {
    using var file = File.OpenRead(path);
    using var reader = new StreamReader(file, Encoding.UTF8);

    List<char[]> lines = [];
    string? line;

    while ((line = reader.ReadLine()) != null) {
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

(int, int) FindStartingPosition(char[,] grid) {
    for (int x = 0; x < grid.GetLength(0); x++) {
        for (int y = 0; y < grid.GetLength(1); y++) {
            if (grid[x, y] == '^') return (x, y);
        }
    }
    return (0, 0);
}

(int, int) Rotate90Deg((int, int) direction) {
    return direction switch {
        (-1, 0) => (0, 1),
        (0, 1) => (1, 0),
        (1, 0) => (0, -1),
        (0, -1) => (-1, 0),
        _ => throw new ArgumentException("Invalid direction"),
    };
}

(int, int) Add((int, int) a, (int, int) b) {
    return (a.Item1 + b.Item1, a.Item2 + b.Item2);
}

int CountPositionsVisited(char[,] grid) {
    int visited = 0;
    for (int x = 0; x < grid.GetLength(0); x++) {
        for (int y = 0; y < grid.GetLength(1); y++) {
            if (grid[x, y] != '.' && grid[x, y] != '#') visited++;
        }
    }
    return visited + 1;
}

int CountObstaclePlacements(char[,] grid, (int, int) startingPosition) {
    int circlesFound = 0;

    (int, int) position = startingPosition;
    (int, int) direction = (-1, 0);

    while (true) {
        (int, int) next = Add(position, direction);

        if (next.Item1 < 0 || next.Item1 >= grid.GetLength(0) || next.Item2 < 0 || next.Item2 >= grid.GetLength(1)) break;

        if (grid[next.Item1, next.Item2] == '#') {
            direction = Rotate90Deg(direction);
            continue;
        }

        char[,] gridCopy = CopyGrid(grid, next);
        if (grid[next.Item1, next.Item2] != 'M' && IsRunningInCircle(gridCopy, startingPosition)) {
            circlesFound++;
        }

        grid[position.Item1, position.Item2] = 'M';
        position = next;
    }

    return circlesFound;
}

char[,] CopyGrid(char[,] grid, (int, int) newObstacle) {
    char[,] newGrid = new char[grid.GetLength(0), grid.GetLength(1)];
    
    for (int x = 0; x < grid.GetLength(0); x++) {
        for (int y = 0; y < grid.GetLength(1); y++) {
            newGrid[x, y] = grid[x, y];
        }
    }

    newGrid[newObstacle.Item1, newObstacle.Item2] = 'O';
    return newGrid;
}

bool IsRunningInCircle(char[,] grid, (int, int) startingPosition) {
    List<(int, int, int, int)> visited = [];

    (int, int) startDirection = (-1, 0);

    (int, int) position = startingPosition;
    (int, int) direction = startDirection;

    while (true) {
        visited.Add((position.Item1, position.Item2, direction.Item1, direction.Item2));

        (int, int) next = Add(position, direction);

        if (next.Item1 < 0 || next.Item1 >= grid.GetLength(0) || next.Item2 < 0 || next.Item2 >= grid.GetLength(1)) return false;
        if (VisitedBefore(visited, next, direction)) return true;

        char val = grid[next.Item1, next.Item2];
        if (val == '#' || val == 'O') {
            direction = Rotate90Deg(direction);
            continue;
        }

        grid[position.Item1, position.Item2] = 'X';
        position = next;
    }
}

bool VisitedBefore(List<(int, int, int, int)> visited, (int, int) position, (int, int) direction) {
    foreach (var transform in visited) {
        if (
            transform.Item1 == position.Item1 && transform.Item2 == position.Item2
            && transform.Item3 == direction.Item1 && transform.Item4 == direction.Item2
        ) return true;
    }
    return false;
}