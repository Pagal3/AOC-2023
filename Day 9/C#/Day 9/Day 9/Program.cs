string[] lines; // the input is saved here

string GetInputFilePath(string type_of_input_file)
{
    if (!type_of_input_file.EndsWith(".txt"))
        type_of_input_file += ".txt";

    string current_exe_directory = Environment.CurrentDirectory;
    string path_to_input = Path.GetFullPath(Path.Combine(current_exe_directory, @"..\..\..\..\..\..\"));


    foreach (var path in Directory.GetFiles(path_to_input))
    {
        //Console.WriteLine(path); // full path
        //Console.WriteLine(System.IO.Path.GetFileName(path)); // file name

        if (System.IO.Path.GetFileName(path) == type_of_input_file)
            return path;
    }

    Console.WriteLine($"ERROR: Specified input file \"{type_of_input_file}\" not found in directory \"{path_to_input}\"");

    return null;

}

void ReadInput(string type_of_input_file)
{
    string path_to_input = GetInputFilePath(type_of_input_file);

    if (path_to_input == null)
        Environment.Exit(1);

    lines = File.ReadAllLines(path_to_input);
}




int[,] values;
void ParseValues()
{
    int length = lines[0].Count(c => c == ' ') + 1;

    values = new int[lines.Length, length];

    for (int i = 0; i < lines.Length; i++)
    {
        List<int[]> all_line_ints = [];
        int[] ints = new int[length];

        string[] line = lines[i].Split(' ');
        for (int j = 0; j < length; j++)
        {
            values[i, j] = int.Parse(line[j]);
            ints[j] = int.Parse(line[j]);

        }


            
    }
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
    

    for (int i = 0; i < values.Length; i++)
    {

    }


    Console.WriteLine();



}

void Part2()
{




}



ReadInput("input");
List<int[]>[] values_all = new List<int[]>[lines.Length];
ParseValues();



Part1();
// Part2();


