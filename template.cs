string[] lines; // the input is saved here

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


void Part1()
{
	


	
}

void Part2()
{
	


	
}


ReadInput("input");

Part1();
// Part2();