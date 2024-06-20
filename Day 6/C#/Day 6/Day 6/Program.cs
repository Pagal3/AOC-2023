using System.Text;

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
	
    static List<int[]> TimesDistances = []; // Part 1
    static ulong[] TimeDistance = new ulong[2]; // Part 2

    static void ParseTimesDistancesPt1()
    {
        int[] times = ToIntArr(lines[0].Split(' '));
        int[] distances = ToIntArr(lines[1].Split(' '));

        for (int i = 0; i < times.Length; i++)
        {
            TimesDistances.Add([times[i],distances[i]]);
        }

        
    }

    static void ParseTimesDistancesPt2()
    {
        ulong time = ToULong_Combine(lines[0].Split(' '));
        ulong distance = ToULong_Combine(lines[1].Split(' '));

        TimeDistance[0] = time;
        TimeDistance[1] = distance;


    }

    static int[] ToIntArr(string[] arr)
    {
        List<int> list = [];
        foreach (string str in arr)
        {
            if (str != "" && str != " " && char.IsDigit(str[0])) // Checks if current string is digit by checking just the first char
                list.Add(int.Parse(str));
        }

        return list.ToArray();
    }

    static ulong ToULong_Combine(string[] arr)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string str in arr)
        {
            if (str != "" && str != " " && char.IsDigit(str[0])) // Checks if current string is digit by checking just the first char
                sb.Append(str);
        }

        return ulong.Parse(sb.ToString());
    }

    static void Part1()
    {
        int num_of_ways_to_beat_record = 1;

        for (int i = 0; i < TimesDistances.Count; i++)
        {
            int how_many_ways_to_win = 0;

            int race_total_time = TimesDistances[i][0];
            for (int ms = 1; ms < race_total_time; ms++)
            {
                //int ms_remaining = race_total_time - ms;

                // We let go of the button
                int travel_mm = ms * (race_total_time - ms);

                if (travel_mm > TimesDistances[i][1])
                {
                    how_many_ways_to_win++;
                }

            }

            if (how_many_ways_to_win > 0)
                num_of_ways_to_beat_record *= how_many_ways_to_win;


        }

        Console.WriteLine(num_of_ways_to_beat_record);

    }

    static void Part2()
    {

        for (ulong ms = 1; ms < TimeDistance[0]; ms++)
        {

            if ((ms * (TimeDistance[0] - ms)) > TimeDistance[1]) // if travel_mm > record
            {
                //ulong last_ms_record_beat = TimeDistance[0] - ms;
                Console.WriteLine((TimeDistance[0] - ms) - ms + 1);
                break;
            }
        }

    }


    static void Main()
    {
		ReadInput("input");
		
        //ParseTimesDistancesPt1();
        //Part1();

        ParseTimesDistancesPt2();
        Part2();
    }
}