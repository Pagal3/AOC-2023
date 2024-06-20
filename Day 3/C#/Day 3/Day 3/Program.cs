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
		
        char[][] chars = new char[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            chars[i] = lines[i].ToCharArray();
        }

        for (int i = 0; i < chars.Length; i++)
        {
            StringBuilder number_builder = new StringBuilder();
            int current_num_index = -1;
            for (int j = 0; j < chars[i].Length; j++)
            {
                //Console.Write(chars[i][j] + " ");
                if ('0' <= chars[i][j] && chars[i][j] <= '9') // if number
                {
                    bool has_symbol = false;

                    if (number_builder.Length == 0) current_num_index = j;
                    number_builder.Append(chars[i][j]);

                    if ((j == chars[i].Length - 1) || !('0' <= chars[i][j+1] && chars[i][j+1] <= '9')) // if next char not number OR char last in line
                    {

                        if (j != chars[i].Length - 1 && chars[i][j+1] != '.') { has_symbol = true; }

                        string number_str = number_builder.ToString();
                        
                        number_builder.Clear();

                        // Checks for symbols in upper and lower line
                        // Start and end indexes depending if number position is at the start or end of the line
                        int start_index = current_num_index - 1;
                        int end_index = current_num_index + number_str.Length;
                        if (current_num_index == 0) start_index = 0; // if already at the first position
                        if (end_index == chars[i].Length) end_index = chars[i].Length - 1; // if 

                        // Upper line check
                        if (i != 0 && !has_symbol) // if line isn't first one
                        {
                            for (int k = start_index; k <= end_index; k++) // 
                            {
                                if (!('0' <= chars[i - 1][k] && chars[i - 1][k] <= '9') && chars[i - 1][k] != '.') // if not number AND not period (.)
                                {
                                    has_symbol = true; break;
                                }
                            }
                        }
                        // Lower line check
                        if (i != lines.Length - 1 && !has_symbol) // if not last line
                        {
                            for (int k = start_index; k <= end_index; k++) // 
                            {
                                if (!('0' <= chars[i + 1][k] && chars[i + 1][k] <= '9') && chars[i + 1][k] != '.') // if not number AND not period (.)
                                {
                                    has_symbol = true; break;
                                }
                            }
                        }

                        // Sides check
                        // Left side
                        if (current_num_index != 0 && !has_symbol)
                        {
                            if (!('0' <= chars[i][current_num_index - 1] && chars[i][current_num_index - 1] <= '9') && chars[i][current_num_index - 1] != '.') // if not number AND not period (.)
                            {
                                has_symbol = true;
                            }
                        }
                        // Right side
                        if (end_index != chars[i].Length && !has_symbol)
                        {
                            if (!('0' <= chars[i][end_index] && chars[i][end_index] <= '9') && chars[i][end_index] != '.') // if not number AND not period (.)
                            {
                                has_symbol = true;
                            }
                        }


                        if (has_symbol)
                        {
                            sum += int.Parse(number_str);
                        }

                    }

                    

                }
            }
            //Console.WriteLine();
        }

        Console.WriteLine(sum);
    }

    static int GetFullNumber(int digit_index, string line)
    {
        Console.WriteLine($"In GetFullNumber() - digit_index: {digit_index}; line: {line}");

        StringBuilder number_builder = new StringBuilder();
        int new_index = digit_index;

        //while (char.IsDigit(line[new_index]))
        //{
        //    new_index--;
        //    Console.WriteLine($"new_index: {new_index}");
        //    if (new_index - 1 == -1) break;
        //}

        for (; char.IsDigit(line[new_index]); new_index--) // Goes to the beginning of the number
        {
            Console.WriteLine($"new_index: {new_index}");
            if (new_index - 1 == -1) break;
            if (!(char.IsDigit(line[new_index - 1]))) break;
        }

        Console.WriteLine($"2nd while new_index: {new_index}");
        for (; char.IsDigit(line[new_index]); new_index++) // Saves the number
        {
            Console.WriteLine($"new_index: {new_index};");
            number_builder.Append(line[new_index]);

            if (new_index + 1 == line.Length) break;
            if (!(char.IsDigit(line[new_index + 1]))) break;
        }

        
        //while (char.IsDigit(line[new_index]))
        //{
            
        //    number_builder.Append(line[new_index]);
            
        //    Console.WriteLine($"new_index: {new_index}; number: {number_builder}");

        //    if (new_index + 1 == line.Length) break;
        //    else new_index++;
        //}

        Console.WriteLine($"Full number: ({number_builder})");

        return int.Parse(number_builder.ToString());

    }

    static bool ArrayFull(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != 0 && arr[i] != null && i == arr.Length - 1)
            {
                return true;
            }
        }
        return false;
    }

    static int[] InsertInArray(int[] arr, int value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == 0) { arr[i] = value; break; }
            if (i == arr.Length - 1) 
                Console.WriteLine("Unreachable: Should be empty");
        }
        return arr;
    }

    static void Part2()
    {
        
        int sum = 0;

        char[][] chars = new char[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            chars[i] = lines[i].ToCharArray();
        }


        for (int i = 0; i < chars.Length; i++)
        {

            for (int j = 0; j < chars[i].Length; j++)
            {
                if (chars[i][j] == '*')
                {
                    int[] gears = new int[2];


                    // upper line
                    if (char.IsDigit(chars[i - 1][j])) // top middle
                        //gears.Append(GetFullNumber(j, lines[i - 1]));
                        gears = InsertInArray(gears, GetFullNumber(j, lines[i - 1]));
                    else
                    {
                        if (char.IsDigit(chars[i - 1][j - 1])) // top left
                                                               //gears.Append(GetFullNumber(j - 1, lines[i - 1]));
                            gears = InsertInArray(gears, GetFullNumber(j - 1, lines[i - 1]));
                        if (char.IsDigit(chars[i - 1][j + 1])) // top right
                                                               //gears.Append(GetFullNumber(j + 1, lines[i - 1]));
                            gears = InsertInArray(gears, GetFullNumber(j + 1, lines[i - 1]));
                    }

                    if (ArrayFull(gears))
                    {

                        sum += gears[0] * gears[1]; continue;
                    }

                    if (i + 1 != chars.Length)
                    {

                        // bottom line
                        if (char.IsDigit(chars[i + 1][j])) // bottom middle
                                                           //gears.Append(GetFullNumber(j, lines[i + 1]));
                            gears = InsertInArray(gears, GetFullNumber(j, lines[i + 1]));
                        else
                        {
                            if (char.IsDigit(chars[i + 1][j - 1])) // bottom left
                                                                   //gears.Append(GetFullNumber(j - 1, lines[i + 1]));
                                gears = InsertInArray(gears, GetFullNumber(j - 1, lines[i + 1]));
                            if (char.IsDigit(chars[i + 1][j + 1])) // bottom right
                                                                        //gears.Append(GetFullNumber(j + 1, lines[i + 1]));
                                gears = InsertInArray(gears, GetFullNumber(j + 1, lines[i + 1]));
                        }
                    }    

                    if (ArrayFull(gears))
                    {
                        sum += gears[0] * gears[1]; continue;
                    }

                    // Left side
                    if (char.IsDigit(chars[i][j - 1]))
                        //gears.Append(GetFullNumber(j - 1, lines[i]));
                        gears = InsertInArray(gears, GetFullNumber(j - 1, lines[i]));

                    if (ArrayFull(gears))
                    {
                        sum += gears[0] * gears[1]; continue; 
                    }

                    // Right side
                    if (char.IsDigit(chars[i][j + 1]))
                        //gears.Append(GetFullNumber(j + 1, lines[i]));
                        gears = InsertInArray(gears, GetFullNumber(j + 1, lines[i]));

                    if (ArrayFull(gears))
                    {
                        sum += gears[0] * gears[1]; continue;
                    } else { 
                        Console.WriteLine("Didn't find gear/s!");  
                        foreach (int gear in gears)
                        {
                            Console.WriteLine($"Gear: {gear}");
                        }
                    
                    }



                }
            }
        }

        Console.WriteLine($"Sum: {sum}");
    }

    static void Main(string[] args)
    {
		ReadInput("input");
		
        //Part1();
        Part2();
    }

}