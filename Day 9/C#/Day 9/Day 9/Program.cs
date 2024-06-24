string[] lines; // the input is saved here
List<int[]>[] values_all; // stores all parsed values and calculated differences

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

        current_dir = System.IO.Directory.GetParent(current_dir).FullName;
    }

    Console.WriteLine($"ERROR: Specified input file \"{type_of_input_file}\" not found\nSearch depth: {search_depth}");

    Environment.Exit(1);
    return null;

}

void ReadInput(string type_of_input_file)
{
    string path_to_input = GetInputFilePath(type_of_input_file);

    lines = File.ReadAllLines(path_to_input);
}




void ParseValues()
{
    int length = lines[0].Count(c => c == ' ') + 1;

    // values = new int[lines.Length, length];

    for (int i = 0; i < lines.Length; i++)
    {
        List<int[]> all_line_ints = [];
        int[] first_ints = new int[length];

        string[] line = lines[i].Split(' ');
        for (int j = 0; j < length; j++)
            first_ints[j] = int.Parse(line[j]);

        all_line_ints.Add(first_ints);
        values_all[i] = all_line_ints;

            
    }

    // Console.WriteLine();

}

int[] GetDiffs(int[] values)
{
    int[] diffs = new int[values.Length - 1];

    for (int i = 0; i < diffs.Length; i++)
        diffs[i] = values[i + 1] - values[i];

    return diffs;
}

void Part1()
{

    //List<int[]> ints = [];
    //List<List<int[]>> test = [];
    

    


    Console.WriteLine();



}

void Part2()
{




}


ReadInput("input");
values_all = new List<int[]>[lines.Length];


ParseValues();



Part1();
// Part2();


