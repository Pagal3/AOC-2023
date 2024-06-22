using System.Collections;

string[] lines = null; // the input is saved here

Hashtable nodes = new Hashtable();

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

void ParseNodes()
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

string GetNextNode(string current_node_key, char left_or_right)
{
    string[] directions = (string[])nodes[current_node_key];

    if (left_or_right == 'L') return directions[0];
    else return directions[1];
}

string[] GetAllStartNodes() // Get all starting nodes - nodes that end with 'A'
{
    List<string> start_nodes = [];
    for (int i = 2; i < lines.Length; i++)
        if (lines[i][2] == 'A')
            start_nodes.Add(lines[i][0..3]);

    return start_nodes.ToArray();
}

bool AllEndWithZ(string[] nodes)
{
    foreach (string node in nodes)
        if (node[2] != 'Z')
            return false;

    return true;
}

void Part1()
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

long[] GetZFrequencies(string[] start_nodes, char[] directions)
{
    long[] freq_arr = new long[start_nodes.Length];

    for (int i = 0; i < start_nodes.Length; i++)
    {
        long freq = 0;
        for (int d = 0; d < directions.Length; d++)
        {
            freq++;

            start_nodes[i] = GetNextNode(start_nodes[i], directions[d]);

            if (start_nodes[i][2] == 'Z')
            {
                freq_arr[i] = freq; break;
            }
            if (d == directions.Length - 1) d = -1;
        }
    }

    return freq_arr;
}


// Function execution time was around 22 secs in Debug without optimizations, 
long GetCount(long[] freqs)
{
    long[] counts = new long[freqs.Length];
    freqs.CopyTo(counts, 0);

    int currently_checking = 1;
    while (currently_checking < counts.Length) // While not all elements are checked to match the same number
    {
        while (counts[currently_checking] != counts[0])
        {
            if (counts[currently_checking] < counts[0])
                counts[currently_checking] += freqs[currently_checking];
            else
                counts[0] += freqs[0];

            // This resets the current check index back to 1, if mismatch was found in index > 1, in order to check all elements for a match
            if (currently_checking > 1)
                currently_checking = 1;
        }
        currently_checking++;
    }
    return counts[0];
}

bool AllValuesIdentical(long[] longs)
{
    foreach (long num in longs)
        if (num != longs[0]) return false;

    return true;
}


void Part2()
{
    char[] directions = lines[0].ToCharArray();
    ParseNodes();

    string[] current_nodes = GetAllStartNodes();

    long[] freqs = GetZFrequencies(current_nodes, directions);

    var watch = System.Diagnostics.Stopwatch.StartNew();
    long result = GetCount(freqs);
    watch.Stop();
    var elapsedMs = watch.ElapsedMilliseconds;

    Console.WriteLine($"Result: {result}\nTime in ms: {elapsedMs}"); 

}

ReadInput("input");

//Part1();
Part2(); // Correct answer: 8245452805243


