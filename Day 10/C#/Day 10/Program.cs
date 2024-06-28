

string[] lines; // the input is saved here
char[,] pipe_grid;

// x is row, y is column
// counting from the top-left - like a 2D array, because it is a 2D array
int sx = -1;
int sy = -1;

bool enable_pipe_route;
char[,] pipe_route = null;

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

bool HasConnection_Right(char pipe)
{
    if (new[] {'L', 'F', '-', 'S'}.Contains(pipe))
        return true;
    return false;
}

bool HasConnection_Left(char pipe)
{
    if (new[] {'J', '7', '-', 'S'}.Contains(pipe))
        return true;
    return false;
}

bool HasConnection_Top(char pipe)
{
    if (new[] {'L', 'J', '|', 'S'}.Contains(pipe))
        return true;
    return false;
}

bool HasConnection_Bottom(char pipe)
{
    if (new[] {'7', 'F', '|', 'S'}.Contains(pipe))
        return true;
    return false;
}

bool CanConnectTop(char current, char target) 
{
    if (HasConnection_Top(current) && HasConnection_Bottom(target))
        return true;
    return false;
}

bool CanConnectBottom(char current, char target) 
{
    if (HasConnection_Bottom(current) && HasConnection_Top(target))
        return true;
    return false;
}

bool CanConnectLeft(char current, char target) 
{
    if (HasConnection_Left(current) && HasConnection_Right(target))
        return true;
    return false;
}

bool CanConnectRight(char current, char target) 
{
    if (HasConnection_Right(current) && HasConnection_Left(target))
        return true;
    return false;
}

bool IsConnectionPossible(string connection_type, char current_pipe, char target_pipe)
{
    switch (connection_type)
    {
        case "top":
            return CanConnectTop(current_pipe, target_pipe);
        case "bottom":
            return CanConnectBottom(current_pipe, target_pipe);
        case "left":
            return CanConnectLeft(current_pipe, target_pipe);
        case "right":
            return CanConnectRight(current_pipe, target_pipe);
        default:
            Console.WriteLine($"ERROR-UNREACHABLE: Invalid connection type: {connection_type}. At fn IsConnectionPossible()");
            Environment.Exit(1);
            return false;
    }
}

bool PipesConnectable(int x_current, int y_current, int x_target, int y_target)
{
    // Check if out of bounds
    if (new[] {x_target, y_target}.Contains(-1) || x_target >= pipe_grid.GetLength(0) || y_target >= pipe_grid.GetLength(1))
        return false;

    string type = ConnectionType(x_current, y_current, x_target, y_target);
    if (IsConnectionPossible(type, pipe_grid[x_current, y_current], pipe_grid[x_target, y_target]))
        return true;
    return false;
}

int[] GetFirstPipeConnectedToS()
{
    // int[] x_y = new int[2];

    if (HasConnection_Left(pipe_grid[sx, sy + 1])) // right
        return [sx, sy + 1];
    if (HasConnection_Top(pipe_grid[sx + 1, sy])) // bottom
        return [sx + 1, sy];
    if (HasConnection_Right(pipe_grid[sx, sy - 1])) // left
        return [sx, sy - 1];
    if (HasConnection_Bottom(pipe_grid[sx - 1, sy])) // top
        return [sx - 1, sy];

    Console.WriteLine($"ERROR-UNREACHABLE: No valid connections found! At fn GetFirstPipeConnectedToS()");

    return null;
}

void MarkPipe(int x, int y)
{
    if (!enable_pipe_route)
        return;

    switch (pipe_grid[x,y])
    {
        case 'S':
            pipe_route[x, y] = '╬';
            break;
        case 'J':
            pipe_route[x, y] = '╝';
            break;
        case 'L':
            pipe_route[x, y] = '╚';
            break;
        case '7':
            pipe_route[x, y] = '╗';
            break;
        case 'F':
            pipe_route[x, y] = '╔';
            break;
        case '|':
            pipe_route[x, y] = '║';
            break;
        case '-':
            pipe_route[x, y] = '═';
            break;
        default:
            Console.WriteLine($"ERROR-UNREACHABLE: Unexpected pipe: {pipe_route[x, y]}. At fn MarkPipe()");
            return;
    }
}

void DumpPipeGrid()
{
    for (int i = 0; i < pipe_route.GetLength(0); i++)
    {
        for (int j = 0; j < pipe_route.GetLength(1); j++)
            Console.Write(pipe_route[i,j]);
        Console.WriteLine();
    }

}

void Part1()
{

    int[] first_connection = GetFirstPipeConnectedToS();
    int x = first_connection[0];
    int y = first_connection[1];

    int x_prev = sx;
    int y_prev = sy;

    int count = 0;

    

    do 
    {

        // if-checks exist to check for the previous pipe

        if (!(x == x_prev && y + 1 == y_prev)) // right
        {
            if (PipesConnectable(x, y, x, y + 1))
            {
                x_prev = x; y_prev = y; 
                y++; 
                count++;
                MarkPipe(x, y);
                continue;
            }
        }
        if (!(x + 1 == x_prev && y == y_prev)) // bottom
        {
            if (PipesConnectable(x, y, x + 1, y))
            {
                x_prev = x; y_prev = y;
                x++;
                count++;
                MarkPipe(x, y);
                continue;
            }
        }
        if (!(x == x_prev && y - 1 == y_prev)) // left
        {
            if (PipesConnectable(x, y, x, y - 1))
            {
                x_prev = x; y_prev = y;
                y--;
                count++;
                MarkPipe(x, y);
                continue;
            }
        }
        if (!(x - 1 == x_prev && y == y_prev)) // top
        {
            if (PipesConnectable(x, y, x - 1, y))
            {
                x_prev = x; y_prev = y;
                x--;
                count++;
                MarkPipe(x, y);
                continue;
            }
        }

    } while (pipe_grid[x,y] != 'S');

    count++;
	Console.WriteLine($"Farthest: {count / 2}");

    if (enable_pipe_route)
        DumpPipeGrid();

}

void Part2()
{
	


	
}


ReadInput("input");
ParsePipes();


enable_pipe_route = true;
if (enable_pipe_route)
    pipe_route = pipe_grid.Clone() as char[,];

Part1();

// Part2();