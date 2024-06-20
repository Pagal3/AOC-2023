

using System.IO;
using System.Linq;
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

        int sum = 0;

        char first_digit = '0', second_digit = '0';

        foreach (string l in lines)
        {
            foreach (char c in l)
            {
                if (48 <= c && c <= 57) { first_digit = c; break; }
            }

            foreach (char c in l.Reverse())
            {
                if (48 <= c && c <= 57) { second_digit = c; break; }
            }

            string whole_number = first_digit.ToString() + second_digit.ToString();
            sum += int.Parse(whole_number);

            //numbers.Append(int.Parse(first_digit.ToString + second_digit.ToString));
            //sum += Int32.Parse( ((char) first_digit + second_digit) );
        }

        Console.WriteLine(sum);
    }

    static char NumberWordToChar(string s)
    {
        char digit = '0';

        if (s.Contains("one")) { digit = '1'; }
        else if (s.Contains("two")) { digit = '2'; }
        else if (s.Contains("three")) { digit = '3'; }
        else if (s.Contains("four")) { digit = '4'; }
        else if (s.Contains("five")) { digit = '5'; }
        else if (s.Contains("six")) { digit = '6'; }
        else if (s.Contains("seven")) { digit = '7'; }
        else if (s.Contains("eight")) { digit = '8'; }
        else if (s.Contains("nine")) { digit = '9'; }

        return digit;
    }

    static void Part2()
    {

        int sum = 0;

        char first_digit = '0', second_digit = '0';

        foreach (string l in lines)
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < l.Length; i++)
            {
                if (49 <= l[i] && l[i] <= 57)
                {
                    first_digit = l[i]; break;
                }
                sb.Append(l[i]);
                string builded_string = sb.ToString();

                first_digit = NumberWordToChar(builded_string);
                if (first_digit != '0') break;

            }
            


            sb.Clear();
            for (int i = l.Length - 1; i >= 0; i--)
            {
                if (49 <= l[i] && l[i] <= 57)
                {
                    second_digit = l[i]; break;
                }
                sb.Insert(0, l[i]);
                string builded_string = sb.ToString();

                second_digit = NumberWordToChar(builded_string);
                if (second_digit != '0') break;
            }

            


            string whole_number = first_digit.ToString() + second_digit.ToString();
            Console.WriteLine(whole_number);

            sum += int.Parse(whole_number);

            //numbers.Append(int.Parse(first_digit.ToString + second_digit.ToString));
            //sum += Int32.Parse( ((char) first_digit + second_digit) );
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