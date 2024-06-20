using System.Collections;

class Program
{
    static string[] lines; // the input is saved here

    static string GetInputFilePath(string type_of_input_file)
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

    static void ReadInput(string type_of_input_file)
    {
        string path_to_input = GetInputFilePath(type_of_input_file);

        if (path_to_input == null)
            Environment.Exit(1);

        lines = File.ReadAllLines(path_to_input);
    }

    static Hashtable nodes = new Hashtable();

    static void ParseNodes()
    {
        for (int i = 2; i < lines.Length; i++)
        {
            //string key = lines[i][0..3];
            //string left = lines[i][7..10];
            //string right = lines[i][12..15];
            string[] left_right = [lines[i][7..10], lines[i][12..15]];
            nodes.Add(lines[i][0..3], left_right);
        }
    }

    static string GetNextNode(string current_node_key, char left_or_right)
    {
        string[] directions = (string[])nodes[current_node_key];

        if (left_or_right == 'L') return directions[0];
        else return directions[1];
    }


    static void Part1()
    {
        char[] directions = lines[0].ToCharArray();
        ParseNodes();

        int count = 0;
        string key = "AAA";
        for (int i = 0; i < directions.Length; i++)
        {
            count++;

            key = GetNextNode(key, directions[i]);

            if (key == "ZZZ") break;
            else if (i == directions.Length - 1) i = -1; // if last direction and not reached "ZZZ", start from the beginning 
        }

        Console.WriteLine(count);

    }

    static void Part2()
    {




    }

    static void Main()
    {
        ReadInput("input");

        Part1();
        // Part2();
    }
}


