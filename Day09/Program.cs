using System.Text;

List<int> input = ParseInput("../../../input.txt");
int length = input.Sum(c => c);
int[] fileSystem = new int[length];

CreateFileSystem(fileSystem, input);
CompactFileSystem(fileSystem);

long checksum = CalculateChecksum(fileSystem);
long blockChecksum = CompactWithBlocks(input);

Console.WriteLine("The compacted filesystems checksum is {0}", checksum);
Console.WriteLine("The checksum when compacting with blocks is {0}", blockChecksum);
return;


List<int> ParseInput(string path) {
    using var file = File.OpenRead(path);
    using var reader = new StreamReader(file, Encoding.UTF8);

    List<int> input = [];
    int nextChar;

    while ((nextChar = reader.Read()) >= 0) {
        input.Add(nextChar - 48);
    }

    return input;
}

void CreateFileSystem(int[] fileSystem, List<int> input) {
    int fileSystemIndex = 0;
    int fileIndex = 0;

    for (int i = 0; i < input.Count; i++) {
        bool writeFile = i % 2 == 0;

        for (int j = 0; j < input[i]; j++) {
            if (writeFile) fileSystem[fileSystemIndex] = fileIndex;
            else fileSystem[fileSystemIndex] = -1;
            fileSystemIndex++;
        }

        if (writeFile) fileIndex++;
    }
}

List<(int, int)> CreateBlockFileSystem(List<int> input) {
    List<(int, int)> blocks = [];
    int blockIndex = 0;

    for (int i = 0; i < input.Count; i++) {
        if (i % 2 != 0) {
            blocks.Add((-1, input[i]));
            continue;
        }
        blocks.Add((blockIndex, input[i]));
        blockIndex++;
    }

    return blocks;
}

void CompactFileSystem(int[] fileSystem) {
    int nextFreeSpace = FindNextFreeSpace(fileSystem);
    int max = fileSystem.Length - 1;

    for (int i = max; i >= 0; i--) {
        if (nextFreeSpace >= i) return;
        if (fileSystem[i] < 0) continue;

        fileSystem[nextFreeSpace] = fileSystem[i];
        fileSystem[i] = -1;

        nextFreeSpace = FindNextFreeSpace(fileSystem, nextFreeSpace);
    }
}

int FindNextFreeSpace(int[] fileSystem, int nextFree = 0) {
    for (int i = nextFree; i < fileSystem.Length; i++) {
        if (fileSystem[i] < 0) return i;
    }
    return int.MaxValue;
}

long CompactWithBlocks(List<int> input) {
    List<(int, int)> fileBlocks = CreateBlockFileSystem(input);

    for (int i = fileBlocks.Count - 1; i >= 0; i--) {
        if (fileBlocks[i].Item1 < 0) continue;
        PlaceAtFirstFree(fileBlocks, i);
    }

    return CalculateBlockChecksum(fileBlocks);
}

void PlaceAtFirstFree(List<(int, int)> fileBlocks, int blockIndex) {
    (int, int) currentBlock = fileBlocks[blockIndex];
    (int, int) freeBlock = (0, 0);
    int freeBlockIndex = -1;

    for (int i = 0; i < blockIndex; i++) {
        if (fileBlocks[i].Item1 >= 0 || fileBlocks[i].Item2 < currentBlock.Item2) continue;

        freeBlock = fileBlocks[i];
        freeBlockIndex = i;
        break;
    }

    if (freeBlockIndex < 0) return;

    fileBlocks[blockIndex] = (-1, currentBlock.Item2);
    fileBlocks.Insert(freeBlockIndex, currentBlock);

    int diff = freeBlock.Item2 - currentBlock.Item2;
    if (diff == 0) fileBlocks.RemoveAt(freeBlockIndex + 1);
    else fileBlocks[freeBlockIndex + 1] = (freeBlock.Item1, diff);
}

long CalculateBlockChecksum(List<(int, int)> fileBlocks) {
    long checksum = 0;
    int i = 0;

    for (int j = 0; j < fileBlocks.Count; j++) {
        (int, int) block = fileBlocks[j];

        if (block.Item1 < 0) {
            i += block.Item2;
            continue;
        }

        int max = i + block.Item2;
        for (; i < max; i++) {
            checksum += block.Item1 * i;
        }
    }

    return checksum;
}

long CalculateChecksum(int[] fileSystem) {
    long checksum = 0;
    
    for (int i = 0; i < fileSystem.Length; i++) {
        int val = fileSystem[i];
        if (val < 0) return checksum;

        checksum += val * i;
    }

    return checksum;
}