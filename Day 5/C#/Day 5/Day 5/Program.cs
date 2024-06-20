

using System.Numerics;
using System.Linq;

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

    struct Map
    {
        public long[,] ranges;

    }

    static int HowManyNumberLines(int index)
    {
        // Counts how many three number lines there are under one map
        int cnt = 0;
        for (int i = index; i < lines.Length; i++, cnt++)
        {
            if (lines[i].Length == 0)
                return cnt;
        }

        return cnt;
    }

    static void Parse(int id, int index)
    {
        // Parsing three-number lines
        // Guaranteed that the first given line is with numbers

        maps[id].ranges = new long[HowManyNumberLines(index), 3];
        for (int i = index, cnt = 0; i < lines.Length; i++, cnt++)
        {
            long[] three_numbers;
            if (lines[i].Length != 0)
            {
                three_numbers = ToLongArr(lines[i].Split(' '));
                maps[id].ranges[cnt, 0] = three_numbers[0];
                maps[id].ranges[cnt, 1] = three_numbers[1];
                maps[id].ranges[cnt, 2] = three_numbers[2];


            }
            else { return; }

        }
    }

    static long[] ToLongArr(string[] arr)
    {
        List<long> list = new List<long>();
        foreach (string str in arr)
        {
            if (str != "" && str != " ") // Make sure blanks aren't counted as matches
                list.Add(long.Parse(str));
        }

        return list.ToArray();
    }

    static int GetID(string str)
    {
        //int id = -1;
        switch (str)
        {
            case "seed-to-soil":
                return 0;
            case "soil-to-fertilizer":
                return 1;
            case "fertilizer-to-water":
                return 2;
            case "water-to-light":
                return 3;
            case "light-to-temperature":
                return 4;
            case "temperature-to-humidity":
                return 5;
            case "humidity-to-location":
                return 6;
            default:
                return -1;
        }
    }

    static Map[] maps = new Map[7];

    static void Part1()
    {
        long[] seeds = ToLongArr(lines[0][(lines[0].IndexOf(' ') + 1)..].Split(' '));
        //seeds = SeedsRangeMod(ToLongArr(lines[0][(lines[0].IndexOf(' ') + 1)..].Split(' ')));

        //foreach (long seed in seeds)
        //    Console.WriteLine($"({seed})");

        long[,] converted = new long[seeds.Length, 8];
        for (int i = 0; i < converted.GetLength(0); i++) // Seeds to 0. pos
        {
            converted[i, 0] = seeds[i];
        }


        // Parsing file into maps[] long[,]     (don't look inside)
        string map_type = null;
        int id = -1;
        for (int i = 2; i < lines.Length; i++)
        {
            if (lines[i].Contains(" map:")) // if line is the map type indicator
            {
                map_type = lines[i][0..lines[i].IndexOf(" map:")];
                //Console.WriteLine($"({map_type})");
                id = GetID(map_type);
                Parse(id, i + 1);
            }
        }

        long lowest_location = long.MaxValue;

        for (int s = 0; s < converted.GetLength(0); s++) // for each seed
        {

            for (int m = 0; m < 7; m++) // for each map (block of three-numbers)
            {
                for (int l = 0; l < maps[m].ranges.GetLength(0); l++) // for each line of three-numbers
                {
                    long dest_start = maps[m].ranges[l, 0];
                    long source_start = maps[m].ranges[l, 1];
                    long range_length = maps[m].ranges[l, 2];

                    long seed = converted[s, m];

                    if (source_start <= seed && seed <= source_start + range_length - 1)
                    {
                        converted[s, m + 1] = seed + (dest_start - source_start);
                        break;
                    }

                    if (l == maps[m].ranges.GetLength(0) - 1) // if last line and no matches, using the same number
                    {
                        converted[s, m + 1] = seed;
                    }


                }
            }

            if (converted[s, 7] < lowest_location)
                lowest_location = converted[s, 7];

        }


        Console.WriteLine($"Lowest location: ({lowest_location})");

    }

    static long[] SeedsRangeMod(long[] seeds)
    {
        // Gets the length of the new array/seeds. Every other seed is summed.
        //long len = 0;
        //for (int i = 0; i < seeds.Length; i += 2)
        //    len += seeds[i];

        //long[] seeds_mod = new long[len];
        List<long> seeds_mod = new List<long>();

        for (long i = 0; i < seeds.Length; i += 2)
        {
            for (long j = 0; j < seeds[i + 1]; j++)
            {
                seeds_mod.Add(seeds[i] + j);
                //Console.Write($"{seeds[i] + j} ");
            }
        }




        return seeds_mod.ToArray();
    }

    static void ClearMaps()
    {
        foreach (Map map in maps)
        {
            for (int i = 0; i < map.ranges.GetLength(0); i++)
            {
                for (int j = 0; j < map.ranges.GetLength(1); j++)
                {
                    map.ranges[i, j] = 0;
                }
            }
        }
    }

    static long[,] ToSeedRange2DArr(long[] arr)
    {
        long[,] arr2d = new long[arr.Length / 2, 2];
        for (int i = 0, j = 0; i < arr.Length; i += 2, j++)
        {
            arr2d[j, 0] = arr[i];
            arr2d[j, 1] = arr[i] + arr[i + 1] - 1;
        }

        return arr2d;
    }

    static List<long[]> ToSeedRangeList(long[] arr)
    {
        List<long[]> longs = new List<long[]>();
        for (int i = 0; i < arr.Length; i += 2)
        {
            long[] one_range = [arr[i], (arr[i] + arr[i + 1] - 1)];
            longs.Add(one_range);
        }

        return longs;
    }

    static List<long[]> ComputeNextRanges(List<long[]> current_ranges, int block_index)
    {
        List<long[]> next_ranges = new List<long[]>();

        foreach (long[] current_range in current_ranges)
        {
            bool found_at_least_one_overlap = false;
            for (int l = 0; l < maps[block_index].ranges.GetLength(0); l++) // for each line
            {
                long dest_start = maps[block_index].ranges[l, 0];
                long source_start = maps[block_index].ranges[l, 1];
                long range_length = maps[block_index].ranges[l, 2];

                long[] target_range = [source_start, source_start + range_length - 1];

                long[] next_range = RangeOverlap(current_range, target_range);
                if (next_range == null) // if no overlap found
                {
                    if (l == maps[block_index].ranges.GetLength(0) - 1 && found_at_least_one_overlap == false) // if last line AND no previous overlaps were found
                    {
                        next_ranges.Add(current_range); // adds the current range to next ranges
                        break;
                    }
                    continue;
                }

                found_at_least_one_overlap = true;

                next_range[0] += dest_start - source_start;
                next_range[1] += dest_start - source_start;

                next_ranges.Add(next_range);



            }
        }

        return next_ranges;
    }

    static void Part2()
    {
        long[] seeds = ToLongArr(lines[0][(lines[0].IndexOf(' ') + 1)..].Split(' '));

        //for (int i = 0; i < seeds.GetLength(0); i++)
        //    Console.WriteLine($"[{seeds[i,0]};{seeds[i,1]}]");

        // Parsing file into maps[] long[,]     (don't look inside)
        string map_type = null;
        int id = -1;
        for (int i = 2; i < lines.Length; i++)
        {
            if (lines[i].Contains(" map:")) // if line is the map type indicator
            {
                map_type = lines[i][0..lines[i].IndexOf(" map:")];
                //Console.WriteLine($"({map_type})");
                id = GetID(map_type);
                Parse(id, i + 1);
            }
        }

        long lowest_location = long.MaxValue;

        List<long[]> current_ranges = ToSeedRangeList(seeds);

        current_ranges = ComputeNextRanges(current_ranges, 0);
        current_ranges = ComputeNextRanges(current_ranges, 1);
        current_ranges = ComputeNextRanges(current_ranges, 2);
        current_ranges = ComputeNextRanges(current_ranges, 3);
        current_ranges = ComputeNextRanges(current_ranges, 4);
        current_ranges = ComputeNextRanges(current_ranges, 5);
        current_ranges = ComputeNextRanges(current_ranges, 6);

        foreach (long[] location in current_ranges)
        {
            if (location[0] < lowest_location)
                lowest_location = location[0];
        }


        /*
        for (int s = 0; s < seeds.GetLength(0); s++) // for each seed
        {
            // long[] current_range = [seeds[s, 0], seeds[s, 1]];
            //List<long[]> current_ranges = [[seeds[s, 0], seeds[s, 1]]];
            //List<long[]> current_ranges_next = [];

            for (int m = 0; m < 7; m++) // for each map (block of three-numbers)
            {
                for (int c = 0; c < current_ranges.Count; c++) // for each range
                {
                    for (int l = 0; l < maps[m].ranges.GetLength(0); l++) // for each line of three-numbers
                    {
                        long dest_start = maps[m].ranges[l, 0];
                        long source_start = maps[m].ranges[l, 1];
                        long range_length = maps[m].ranges[l, 2];

                        long[] target_range = [source_start, source_start + range_length - 1];

                        // TODO: Ranges can overlap other ranges

                        long[] old_current_range = current_ranges[c];
                        // long[] old_current_range_backup = old_current_range;

                        old_current_range = RangeOverlap(old_current_range, target_range);
                        if (old_current_range == null) // Checks if there is an overlap or not. If not, continues to the next three nums.
                        {
                            // old_current_range = old_current_range_backup;
                            continue;
                        }

                        // Adjusting ranges
                        // old_current_range[0] += dest_start - source_start;
                        // old_current_range[1] += dest_start - source_start;

                        old_current_range[0] += dest_start - source_start;
                        old_current_range[1] += dest_start - source_start;

                        current_ranges_next.Add(old_current_range);

                        // break;


                    }

                    current_ranges = current_ranges_next;

                }




            }

            //if (current_range[0] < lowest_location)
            //    lowest_location = current_range[0];

        }
        */

        Console.WriteLine($"Lowest location: ({lowest_location})");


    }



    static long[] RangeOverlap(long[] source_range, long[] target_range)
    {
        //if (source_range[0] >= target_range[0] && source_range[1] <= target_range[1] || // inside
        //    source_range[0] <= target_range[0] && source_range[1] >= target_range[0] || // left is outside
        //    source_range[0] <= target_range[1] && source_range[1] >= target_range[1] || // right is outside
        //    source_range[0] <= target_range[0] && source_range[1] >= target_range[1]    // target is inside
        //    ) 
        if (source_range[0] < target_range[0] && source_range[1] <= target_range[0] || source_range[0] > target_range[1] && source_range[1] >= target_range[1])
            return null;
        // TODO: Ranges can overlap other ranges

        // if (!(source_range[0] >= target_range[0] && source_range[1] <= target_range[1]))
        //     return null;
        else
        {
            long[] new_range = [long.Max(source_range[0], target_range[0]), long.Min(source_range[1], target_range[1])];
            return new_range;
        }
    }


    static bool part2_on = false;

    static void Main()
    {
		ReadInput("input");
		
        // Part1();
        Part2();
    }


}