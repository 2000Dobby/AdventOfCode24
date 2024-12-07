using System.Diagnostics;

var stopwatch = Stopwatch.StartNew();
var (grid, startingPosition) = ParseInput("../../../input.txt");

int loopCount = CountObstaclePlacements(grid, startingPosition);
int positionsVisited = CountPositionsVisited(grid);
stopwatch.Stop();

Console.WriteLine("The guard visited {0} distinct positions", positionsVisited);
Console.WriteLine("The number of possible loops is {0}", loopCount);
Console.WriteLine("The execution took {0:F2}s", stopwatch.ElapsedMilliseconds / 1000d);

return;


(char[,], (int, int)) ParseInput(string path) {
    string[] lines = File.ReadAllLines(path);

    int rows = lines.Length;
    int cols = lines[0].Length;
    
    (int, int) startingPosition = (0, 0);
    char[,] grid = new char[rows, cols];

    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            char val = lines[i][j];
            grid[i, j] = val;
            if (val is '^') startingPosition = (i, j);
        }
    }

    return (grid, startingPosition);
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

int CountObstaclePlacements(char[,] grid, (int, int) startingPosition) { // ToDo: Optimization - Save path that has already been run since it is always the same 
    int circlesFound = 0;

    int width = grid.GetLength(0);
    int height = grid.GetLength(1);
    
    (int, int) position = startingPosition;
    (int, int) direction = (-1, 0);

    while (true) {
        (int, int) next = Add(position, direction);

        if (next.Item1 < 0 || next.Item1 >= width || next.Item2 < 0 || next.Item2 >= height) break;

        if (grid[next.Item1, next.Item2] == '#') {
            direction = Rotate90Deg(direction);
            continue;
        }

        if (grid[next.Item1, next.Item2] != 'X' && IsRunningInCircle(grid, startingPosition, next)) {
            circlesFound++;
        }

        grid[position.Item1, position.Item2] = 'X';
        position = next;
    }

    return circlesFound;
}

bool IsRunningInCircle(char[,] grid, (int, int) startingPosition, (int, int) newObstacle) {
    HashSet<((int, int), (int, int))> visited = [];

    char prev = grid[newObstacle.Item1, newObstacle.Item2];
    grid[newObstacle.Item1, newObstacle.Item2] = '#';

    int width = grid.GetLength(0);
    int height = grid.GetLength(1);
    
    (int, int) position = startingPosition;
    (int, int) direction = (-1, 0);
    
    bool loop;
    while (true) {
        if (!visited.Add((position, direction))) {
            loop = true;
            break;
        }
        
        (int, int) next = Add(position, direction);
        if (next.Item1 < 0 || next.Item1 >= width || next.Item2 < 0 || next.Item2 >= height) {
            loop = false;
            break;
        }

        if (grid[next.Item1, next.Item2] is '#') {
            direction = Rotate90Deg(direction);
            continue;
        }

        position = next;
    }
    
    grid[newObstacle.Item1, newObstacle.Item2] = prev;
    return loop;
}