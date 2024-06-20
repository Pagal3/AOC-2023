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

        foreach (string line in lines)
        {
            string[] wins_str = line[(line.IndexOf(": ") + 2)..line.IndexOf(" |")].Trim().Split(' ');
            string[] haves_str = line.Substring(line.IndexOf("| ") + 2).Split(' ');
            //Console.WriteLine($"({wins_str}) ({haves_str})");

            //for (int i = 0; i < wins_str.Length; i++)
            //    Console.Write($"{wins_str[i]} ");
            //Console.Write("| ");

            //for (int i = 0; i < haves_str.Length; i++)
            //    Console.Write($"{haves_str[i]} ");
            //Console.WriteLine();

            int found_cnt = 0;
            for (int i = 0; i < wins_str.Length; i++) 
            {
                if (wins_str[i] != "" && wins_str[i] != " ") // Make sure blanks aren't counted as matches
                    for (int j = 0; j < haves_str.Length; j++)
                        if (wins_str[i] == haves_str[j]) 
                        { 
                            found_cnt++;  break; 
                        }
                
            }

            if (found_cnt > 0)
                sum += (int) Math.Pow(2, found_cnt - 1);



        }

        Console.WriteLine(sum);
        // 45899 - too high
        // 21821 - CORRECT ANSWER


    }

    // -------- PART 2 STUFF --------

    struct Card
    {
        public int id;
        public int copies;
        public int matches_found;
        public int[] wins;
        public int[] haves;
    }

    static int[] ToIntArr(string[] arr)
    {
        List<int> list = new List<int>();
        foreach (string str in arr)
        {
            if (str != "" && str != " ") // Make sure blanks aren't counted as matches
                list.Add(int.Parse(str));
        }

        return list.ToArray();
    }

    static void Part2()
    {
        int sum = 0;

        Card[] cards = new Card[lines.Length];

        // Parsing strings into cards 
        for (int i = 0; i < lines.Length; i++)
        {
            string[] wins_str = lines[i][(lines[i].IndexOf(": ") + 2)..lines[i].IndexOf(" |")].Trim().Split(' ');
            string[] haves_str = lines[i].Substring(lines[i].IndexOf("| ") + 2).Split(' ');
            string id = lines[i][(lines[i].IndexOf(' ') + 1)..lines[i].IndexOf(':')];

            //Console.WriteLine($"({id})");

            cards[i].id = int.Parse(id);
            cards[i].wins = ToIntArr(wins_str);
            cards[i].haves = ToIntArr(haves_str);
            cards[i].copies = 1;
        }

        // Searching for matches
        for (int i = 0; i < cards.Length; i++)
        {
            foreach (int win in cards[i].wins)
                if (cards[i].haves.Contains(win))
                {
                    cards[i].matches_found++;
                }
        }

        foreach (Card card in cards)
        {
            if (card.matches_found > 0)
            {
                for (int i = 1; i <= card.matches_found; i++)
                {
                    //cards[card.id + i].copies += card.copies;
                    for (int j = 0; j < cards.Length; j++)
                    {
                        if (cards[j].id == card.id + i)
                        {
                            cards[j].copies += card.copies;
                        }
                    }
                }
            }
        }


        foreach (Card card in cards)
        {
            sum += card.copies;
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