

string[] lines; // the input is saved here
char[,] pipe_grid;

int sx = -1;
int sy = -1;


string GetInputFilePath(string type_of_input_file)
{
    if (!type_of_input_file.EndsWith(".txt"))
        type_of_input_file += ".txt";

    int search_depth = 10; // how many directories up will be searched in order to find the input file

    // string current_exe_directory = Environment.CurrentDirectory;
    // string path_to_input = Path.GetFullPath(Path.Combine(current_exe_directory, @"..\..\..\..\..\..\"));

    string current_dir = System.IO.Directory.GetCurrentDirectory();
    for (int i=0; i < search_depth; i++) 
    {
        foreach (var path in Directory.GetFiles(current_dir))
        {
            if (System.IO.Path.GetFileName(path) == type_of_input_file)
                return path;
        }

        try { current_dir = System.IO.Directory.GetParent(current_dir).FullName; }
        catch 
        {
            Console.WriteLine($"ERROR: Specified input file \"{type_of_input_file}\" not found. Reached search depth: {i}");
            Environment.Exit(1);
        }

    }

    Console.WriteLine($"ERROR: Specified input file \"{type_of_input_file}\" not found. Searched through all set {search_depth} directories.");
    Environment.Exit(1);
    return null;
}


void ReadInput(string type_of_input_file)
{
	string path_to_input = GetInputFilePath(type_of_input_file);

	if (path_to_input == null)
		Environment.Exit(1);

	lines = File.ReadAllLines(path_to_input);
}

void ParsePipes()
{
    pipe_grid = new char[lines.Length,lines[0].Length];

    for (int i = 0; i < lines.Length; i++) 
    {
        for (int j = 0; j < lines[0].Length; j++) 
        {
            pipe_grid[i,j] = lines[i][j];
            if (pipe_grid[i, j] == 'S')
            {
                sx = i;
                sy = j;
            }
        }
    }

    
}


string ConnectionType(int x_current, int y_current, int x_target, int y_target) 
{


    if (x_current == x_target) // on the same row
    {
        // Left, Right
        if (y_target < y_current) return "left";
        else return "right";
    }
    else // on the same column
    {
        // Top, Bottom
        if (x_target < x_current) return "top";
        else return "bottom";
    }
}

bool CanConnectTop(char current) 
{
    if (new[] { '|', '7', 'F' }.Contains(current))
        return true;
    return false;
}

bool CanConnectBottom(char current) 
{
    if (new[] { '-', 'L', 'F' }.Contains(current))
        return true;
    return false;
}

bool CanConnectLeft(char current) 
{
    if (new[] { '-', 'L', 'F' }.Contains(current))
        return true;
    return false;
}

bool CanConnectRight(char current) 
{
    if (new[] { '-', 'J', '7' }.Contains(current))
        return true;
    return false;
}

bool IsConnectionPossible(string connection_type, char target_pipe)
{
    switch (connection_type)
    {
        case "top":
            return CanConnectTop(target_pipe);
        case "bottom":
            return CanConnectBottom(target_pipe);
        case "left":
            return CanConnectLeft(target_pipe);
        case "right":
            return CanConnectRight(target_pipe);
        default:
            Console.WriteLine($"ERROR-UNREACHABLE: Invalid connection type: {connection_type}. At fn IsConnectionPossible()");
            Environment.Exit(1);
            return false;
    }
}


bool PipesConnectable(int x_current, int y_current, int x_target, int y_target)
{
    string type = ConnectionType(x_current, y_current, x_target, y_target);
    if (IsConnectionPossible(type, pipe_grid[x_target, y_target]))
        return true;
    return false;
}

int[] GetFirstPipeConnectedToS()
{
    int[] x_y = new int[2];

    if (pipe_grid[sx, sy + 1])

    return x_y;
}




void Part1()
{

    int x = sx;
    int y = sy;

    int count = 0;

    do 
    {
        

        count++;
    } while (pipe_grid[x,y] != 'S');

	Console.WriteLine($"Farthest: {count / 2}");
}

void Part2()
{
	


	
}


ReadInput("sample");
ParsePipes();

Part1();
// Part2();