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

bool AreAllSame(int[] ints) 
{
    foreach (int num in ints)
        if (num != ints[0]) return false;
    return true;
}

void PopulateWithAllDifferences() 
{
    for (int i = 0; i < values_all.Length; i++) // for each input line/values
    {
        int[] diffs = values_all[i][0];

        do {
            diffs = GetDiffs(diffs);
            values_all[i].Add(diffs);
        } while (!AreAllSame(diffs));
    }
}

int GetNextValue(int index) 
{
    int length = values_all[index].Count();

    int difference_to_add = values_all[index][length - 1][0]; // number that is at the bottom of the "pyramid"

    for (int i = length - 2; i >= 0; i--)
        difference_to_add += values_all[index][i].Last();

    return difference_to_add;
}

int GetPrevValue(int index) 
{
    int length = values_all[index].Count();

    int difference_to_subtract = values_all[index][length - 1][0]; // number that is at the bottom of the "pyramid"

    for (int i = length - 2; i >= 0; i--)
        difference_to_subtract = values_all[index][i][0] - difference_to_subtract;

    return difference_to_subtract;
}

void Part1()
{

    PopulateWithAllDifferences();

    int sum = 0;
    for (int i = 0; i < values_all.Length; i++) // for each input line/values
        sum += GetNextValue(i);

    Console.WriteLine($"Sum: {sum}");
}

void Part2()
{
    PopulateWithAllDifferences();

    int sum = 0;
    for (int i = 0; i < values_all.Length; i++) // for each input line/values
        sum += GetPrevValue(i);

    Console.WriteLine($"Sum: {sum}");
}


ReadInput("input");
values_all = new List<int[]>[lines.Length];
ParseValues();

// Part1();
Part2();


