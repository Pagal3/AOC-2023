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

    static void Part1()
    {
        // only 12 red cubes, 13 green cubes, and 14 blue cubes

        int sum = 0;

        foreach (string line in lines)
        {
            bool possible = true;

            for (int i = 13; i < 25; i++)
            {
                if (line.Substring(line.IndexOf(':')).IndexOf((i + 3).ToString()) != -1 ||
                    line.IndexOf(i.ToString() + " red") != -1 ||
                    line.IndexOf((i + 1).ToString() + " green") != -1 ||
                    line.IndexOf((i + 2).ToString() + " blue") != -1)
                {
                    possible = false;
                    break;
                }
            }

            if (possible)
            {
                Console.WriteLine("Possible line: " + line[5..line.IndexOf(':')]);
                sum += int.Parse(line[5..line.IndexOf(':')]);
            }

        }

        Console.WriteLine(sum);

    }


    static void Part2() 
    {

        int sum = 0;

        foreach (string line in lines)
        {
            // Reds
            //StringBuilder sb_number = new StringBuilder();
            //StringBuilder sb_color = new StringBuilder();
            //for (int i = line.IndexOf(':') + 2 /*Starts from first number - after ': '*/; i < line.Length; i++)
            //{
            //    if (48 <= line[i] && line[i] <= 57)
            //    {
            //        sb_number.Append(line[i]);
            //    } else if {

            //    }
            //}

            string[] colors = line.Substring(line.IndexOf(':') + 2).Replace(';', ',').Split(", ");
            int red_max = 0, green_max = 0, blue_max = 0;
            foreach (string color in colors)
            {
                string[] num_w_color = color.Split(' ');
                //Console.WriteLine($"Length: {num_w_color.Length}; Num: {num_w_color[0]}; Color: {num_w_color[1]}");
                switch (num_w_color[1])
                {
                    case "red":
                        if (int.Parse(num_w_color[0]) > red_max) red_max = int.Parse(num_w_color[0]);
                        break;
                    case "green":
                        if (int.Parse(num_w_color[0]) > green_max) green_max = int.Parse(num_w_color[0]);
                        break;
                    case "blue":
                        if (int.Parse(num_w_color[0]) > blue_max) blue_max = int.Parse(num_w_color[0]);
                        break;
                    default:
                        Console.WriteLine("Unreachable case!");
                        break;
                }
            }

            int cubed = red_max * green_max * blue_max;
            if (cubed == 0) { Console.WriteLine("Cube is zero! - ", line); return; }

            sum += cubed;


        }

        Console.WriteLine(sum);


    }

    static void Main(string[] args)
    {
		ReadInput("input");
		
        //Part1();
        Part2();
    }
}