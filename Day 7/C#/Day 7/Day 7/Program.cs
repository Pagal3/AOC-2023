using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;


/*
 --- Day 7: Camel Cards ---

Your all-expenses-paid trip turns out to be a one-way, five-minute ride in an airship. (At least it's a cool airship!) It drops you off at the edge of a vast desert and descends back to Island Island.

"Did you bring the parts?"

You turn around to see an Elf completely covered in white clothing, wearing goggles, and riding a large camel.

"Did you bring the parts?" she asks again, louder this time. You aren't sure what parts she's looking for; you're here to figure out why the sand stopped.

"The parts! For the sand, yes! Come with me; I will show you." She beckons you onto the camel.

After riding a bit across the sands of Desert Island, you can see what look like very large rocks covering half of the horizon. The Elf explains that the rocks are all along the part of Desert Island that is directly above Island Island, making it hard to even get there. Normally, they use big machines to move the rocks and filter the sand, but the machines have broken down because Desert Island recently stopped receiving the parts they need to fix the machines.

You've already assumed it'll be your job to figure out why the parts stopped when she asks if you can help. You agree automatically.

Because the journey will take a few days, she offers to teach you the game of Camel Cards. Camel Cards is sort of similar to poker except it's designed to be easier to play while riding a camel.

In Camel Cards, you get a list of hands, and your goal is to order them based on the strength of each hand. A hand consists of five cards labeled one of A, K, Q, J, T, 9, 8, 7, 6, 5, 4, 3, or 2. The relative strength of each card follows this order, where A is the highest and 2 is the lowest.

Every hand is exactly one type. From strongest to weakest, they are:

    Five of a kind, where all five cards have the same label: AAAAA
    Four of a kind, where four cards have the same label and one card has a different label: AA8AA
    Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
    Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
    Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
    One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
    High card, where all cards' labels are distinct: 23456

Hands are primarily ordered based on type; for example, every full house is stronger than any three of a kind.

If two hands have the same type, a second ordering rule takes effect. Start by comparing the first card in each hand. If these cards are different, the hand with the stronger first card is considered stronger. If the first card in each hand have the same label, however, then move on to considering the second card in each hand. If they differ, the hand with the higher second card wins; otherwise, continue with the third card in each hand, then the fourth, then the fifth.

So, 33332 and 2AAAA are both four of a kind hands, but 33332 is stronger because its first card is stronger. Similarly, 77888 and 77788 are both a full house, but 77888 is stronger because its third card is stronger (and both hands have the same first and second card).

To play Camel Cards, you are given a list of hands and their corresponding bid (your puzzle input). For example:

32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483

This example shows five hands; each hand is followed by its bid amount. Each hand wins an amount equal to its bid multiplied by its rank, where the weakest hand gets rank 1, the second-weakest hand gets rank 2, and so on up to the strongest hand. Because there are five hands in this example, the strongest hand will have rank 5 and its bid will be multiplied by 5.

So, the first step is to put the hands in order of strength:

    32T3K is the only one pair and the other hands are all a stronger type, so it gets rank 1.
    KK677 and KTJJT are both two pair. Their first cards both have the same label, but the second card of KK677 is stronger (K vs T), so KTJJT gets rank 2 and KK677 gets rank 3.
    T55J5 and QQQJA are both three of a kind. QQQJA has a stronger first card, so it gets rank 5 and T55J5 gets rank 4.

Now, you can determine the total winnings of this set of hands by adding up the result of multiplying each hand's bid with its rank (765 * 1 + 220 * 2 + 28 * 3 + 684 * 4 + 483 * 5). So the total winnings in this example are 6440.

Find the rank of every hand in your set. What are the total winnings?

--- Part Two ---

To make things a little more interesting, the Elf introduces one additional rule. Now, J cards are jokers - wildcards that can act like whatever card would make the hand the strongest type possible.

To balance this, J cards are now the weakest individual cards, weaker even than 2. The other cards stay in the same order: A, K, Q, T, 9, 8, 7, 6, 5, 4, 3, 2, J.

J cards can pretend to be whatever card is best for the purpose of determining hand type; for example, QJJQ2 is now considered four of a kind. However, for the purpose of breaking ties between two hands of the same type, J is always treated as J, not the card it's pretending to be: JKKK2 is weaker than QQQQ2 because J is weaker than Q.

Now, the above example goes very differently:

32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483

    32T3K is still the only one pair; it doesn't contain any jokers, so its strength doesn't increase.
    KK677 is now the only two pair, making it the second-weakest hand.
    T55J5, KTJJT, and QQQJA are now all four of a kind! T55J5 gets rank 3, QQQJA gets rank 4, and KTJJT gets rank 5.

With the new joker rule, the total winnings in this example are 5905.

Using the new joker rule, find the rank of every hand in your set. What are the new total winnings?


 */

class Program
{
    struct Hand
    {
        public string cards;
        public int bid;
    }

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

    // Parses hand/bid list into hashtabe (key: bid, value: Hand() )
    //static Hashtable ParseHands()
    //{
    //    Hashtable hands = [];

    //    foreach (string str in lines)
    //    {
    //        string hand = str[0..str.IndexOf(' ')];
    //        int bid = int.Parse(str[(str.IndexOf(' ') + 1)..]);

    //        int[] bid_rank = [bid, -1];

    //        hands.Add(hand, bid_rank);


    //    }
    //    return hands;
    //}

    /*    static OrderedDictionary ParseHands()
        {
            OrderedDictionary hands = [];

            foreach (string str in lines)
            {
                string hand = str[0..str.IndexOf(' ')];
                int bid = int.Parse(str[(str.IndexOf(' ') + 1)..]);

                int[] bid_rank = [ConvertHandToNumber(hand), bid];

                hands.Add(hand, bid_rank);


            }
            return hands;
        }*/

    static SortedDictionary<long, Hand> ParseHands_Pt1()
    {
        SortedDictionary<long, Hand> hands = [];

        foreach (string str in lines)
        {
            string cards_in_hand = str[0..str.IndexOf(' ')];
            int bid = int.Parse(str[(str.IndexOf(' ') + 1)..]);

            Hand hand = new Hand();
            hand.cards = cards_in_hand;
            hand.bid = bid;

            hands.Add(ConvertHandToNumber_Pt1(cards_in_hand), hand);
        }
        return hands;
    }

    static SortedDictionary<long, Hand> ParseHands_Pt2()
    {
        SortedDictionary<long, Hand> hands = [];

        foreach (string str in lines)
        {
            string cards_in_hand = str[0..str.IndexOf(' ')];
            int bid = int.Parse(str[(str.IndexOf(' ') + 1)..]);

            Hand hand = new Hand();
            hand.cards = cards_in_hand;
            hand.bid = bid;

            hands.Add(ConvertHandToNumber_Pt2(cards_in_hand), hand);
        }
        return hands;
    }


    static int Count(string source_str, char char_to_count)
    {
        int count = 0;
        foreach (char ch in source_str)
            if (ch == char_to_count) count++;
        return count;
    }

    static int JokerCount(string source_str)
    {
        return Count(source_str, 'J');
    }

    static char GetNextDifferentChar(string source_str, char current_char)
    {
        foreach (char ch in source_str)
            if (ch != current_char) return ch;
        return current_char;
    }

    static int GetCardRank_Pt1(char card)
    {
        switch (card)
        {
            case 'A': return 14;
            case 'K': return 13;
            case 'Q': return 12;
            case 'J': return 11;
            case 'T': return 10;
            case '9': return 9;
            case '8': return 8;
            case '7': return 7;
            case '6': return 6;
            case '5': return 5;
            case '4': return 4;
            case '3': return 3;
            case '2': return 2;
            default:
                Console.WriteLine($"NOT a card: ({card}) returned -1. At fn GetCardRank()");
                return -1;
        }
    }

    static int GetCardRank_Pt2(char card)
    {
        switch (card)
        {
            case 'A': return 14;
            case 'K': return 13;
            case 'Q': return 12;
            case 'J': return 1;     // Weakest individual card
            case 'T': return 10;
            case '9': return 9;
            case '8': return 8;
            case '7': return 7;
            case '6': return 6;
            case '5': return 5;
            case '4': return 4;
            case '3': return 3;
            case '2': return 2;
            default:
                Console.WriteLine($"NOT a card: ({card}) returned -1. At fn GetCardRank()");
                return -1;
        }
    }

    // Returns: 1 if Bigger, 0 if Smaller, 2 if Equals/Same size
    /*    static int FirstCardBiggerOrSmallerThanSecond(char first_card, char second_card)
        {
            int card1 = GetCardRank(first_card);
            int card2 = GetCardRank(second_card);

            if (card1 == card2) return 2;
            if (card1 > card2) return 1;
            else return 0;
        }*/

    // Five of a kind, where all five cards have the same label: AAAAA
    static bool IsHandType1(string hand)
    {
        // Checks if all chars are the same
        if (Count(hand, hand[0]) == 5) return true;
        else return false;
    }

    // Four of a kind, where four cards have the same label
    // and one card has a different label: AA8AA
    static bool IsHandType2(string hand)
    {
        if (Count(hand, hand[0]) == 4 || Count(hand, hand[1]) == 4) return true;
        else return false;
    }

    // Full house, where three cards have the same label,
    // and the remaining two cards share a different label: 23332
    static bool IsHandType3(string hand)
    {
        bool three_found = false;
        bool two_found = false;
        foreach (char ch in hand)
        {
            int count = Count(hand, ch);
            if (count == 3) three_found = true;
            else if (count == 2) two_found = true;

            if (three_found && two_found) return true;
        }
        return false;
    }

    // Three of a kind, where three cards have the same label,
    // and the remaining two cards are each different from any other card in the hand: TTT98
    static bool IsHandType4(string hand)
    {
        bool three_found = false;
        bool diff_found = false;
        foreach (char ch in hand)
        {
            int count = Count(hand, ch);
            if (count == 3) three_found = true;
            else if (count == 1) diff_found = true;

            if (three_found && diff_found) return true;
        }
        return false;
    }

    // Two pair, where two cards share one label,
    // two other cards share a second label, and the remaining card has a third label: 23432
    static bool IsHandType5(string hand)
    {
        char first_pair = ' ';
        char second_pair = ' ';
        foreach (char ch in hand)
        {
            if (Count(hand, ch) == 2)
            {
                if (first_pair == ' ') 
                    first_pair = ch;
                else if (second_pair == ' ' && ch != first_pair) 
                    second_pair = ch;
            }

            if (first_pair != second_pair && first_pair != ' ' && second_pair != ' ')
                return true;

        }
        return false;
    }

    // One pair, where two cards share one label,
    // and the other three cards have a different label from the pair and each other: A23A4
    static bool IsHandType6(string hand)
    {
        
        int diffs_found = 0;
        char pair = ' ';
        foreach (char ch in hand)
        {
            int count = Count(hand, ch);
            if (count == 2 && pair == ' ')
                pair = ch;

            if (count == 1) diffs_found++;

            if (pair != ' ' && diffs_found > 1)
                return true;
        }
        return false;
    }

    // High card, where all cards' labels are distinct: 23456
    static bool IsHandType7(string hand)
    {
        foreach (char ch in hand)
            if (Count(hand, ch) > 1) return false;
        return true;
    }

    static int GetHandType_Pt1(string hand)
    {
        if (IsHandType1(hand)) return 7;
        else if (IsHandType2(hand)) return 6;
        else if (IsHandType3(hand)) return 5;
        else if (IsHandType4(hand)) return 4;
        else if (IsHandType5(hand)) return 3;
        else if (IsHandType6(hand)) return 2;
        else if (IsHandType7(hand)) return 1;

        Console.WriteLine($"ERROR: No type match found for hand {hand}. At fn GetHandType()");
        return -1;
    }

    static int GetHandType_Pt2(string hand)
    {
        int joker_count = JokerCount(hand);
        if (joker_count > 0 && joker_count < 5)
            hand = UpdateHandWithJokers(hand);

        if (IsHandType1(hand)) return 7;
        else if (IsHandType2(hand)) return 6;
        else if (IsHandType3(hand)) return 5;
        else if (IsHandType4(hand)) return 4;
        else if (IsHandType5(hand)) return 3;
        else if (IsHandType6(hand)) return 2;
        else if (IsHandType7(hand)) return 1;

        Console.WriteLine($"ERROR: No type match found for hand {hand}. At fn GetHandType()");
        return -1;
    }

    static string UpdateHandWithJokers(string hand)
    {
        char char_to_replace_with = WhichCharWillReplaceJoker(hand);
        char[] hand_chars = hand.ToCharArray();
        for (int i = 0; i < hand.Length; i++)
            if (hand[i] == 'J') hand_chars[i] = char_to_replace_with;

        hand = "";
        foreach (char c in hand_chars)
            hand += c;
        return hand;
    }
    
    static char WhichCharWillReplaceJoker(string hand)
    {
        if (JokerCount(hand) >= 3) // if 3 or 4 jokers (5 is unreachable here)
            return GetNextDifferentChar(hand, 'J'); // gets the next non-joker card, doesn't matter which


        int max_count = 0;
        char char_with_max_count = '*';
        foreach (char ch in hand)
        {
            if (ch == 'J') continue;

            int count = Count(hand, ch);
            if (count > max_count)
            {
                max_count = count;
                char_with_max_count = ch;
            }
        }
        
        if (char_with_max_count == '*') Console.WriteLine($"ERROR: Char with max count is '*' in hand ({hand}). At fn WhichCharWillReplaceJoker()");
        
        return char_with_max_count;
    }

    static long ConvertHandToNumber_Pt1(string hand)
    {
        StringBuilder number_str = new StringBuilder();
        number_str.Append(GetHandType_Pt1(hand));

        foreach (char ch in hand)
        {
            int card_rank = GetCardRank_Pt1(ch);
            if (card_rank < 10)
                number_str.Append('0');
            number_str.Append(card_rank);
        }
        return long.Parse(number_str.ToString());
    }

    static long ConvertHandToNumber_Pt2(string hand)
    {
        StringBuilder number_str = new StringBuilder();
        number_str.Append(GetHandType_Pt2(hand));

        foreach (char ch in hand)
        {
            int card_rank = GetCardRank_Pt2(ch);
            if (card_rank < 10)
                number_str.Append('0');
            number_str.Append(card_rank);
        }
        return long.Parse(number_str.ToString());
    }

    /*
        static bool IsFirstSameTypeHandBiggerThanSecond(string first_hand, string second_hand)
        {
            for (int i = 0; i < first_hand.Length; i++)
            {
                int result = FirstCardBiggerOrSmallerThanSecond(first_hand[i], second_hand[i]);
                if (result == 1) return true;
                if (result == 0) return false;
            }

            Console.WriteLine($"ERROR: Hands {first_hand} and {second_hand} are completely identical. At fn IsFirstSameTypeHandBiggerThanSecond()");
            return false;
        }
    */

    /*    static bool IsFirstHandBiggerThanSecond(string first_hand, string second_hand)
        {
            int first_type = GetHandType(first_hand);
            int second_type = GetHandType(second_hand);

            // Smaller hand type means the hand is stronger/bigger
            if (first_type < second_type) return true;
            if (first_type > second_type) return false;

            // Hands are the same type
            if (IsFirstSameTypeHandBiggerThanSecond(first_hand, second_hand))
                return true;

            Console.WriteLine($"ERROR: Unreachable at fn IsFirstHandBiggerThanSecond()");
            return false;
        }*/


    static void Part1()
    {
        SortedDictionary<long, Hand> hands = ParseHands_Pt1();

        //int[] bruh = (int[])hands["6K58A"];
        //Console.WriteLine(bruh[0]);

        //string key = "6K58A";
        //if (hands.ContainsKey(key))
        //{
        //    hands[key] = (int[]) [555, bruh[1]];
        //    int[] ok = (int[])hands[key];
        //    Console.WriteLine($"{ok[0]} and {ok[1]}");
        //}


        long[] keys = new long[hands.Keys.Count];
        hands.Keys.CopyTo(keys, 0);

        //for (int i = 0; i < hands.Keys.Count; i++)
        //{
        //    Console.WriteLine($"Index: {i}, Key: {keys[i]}, Value: {(int[])hands[i]}");
        //}



        long total_winnings = 0;
        for (int i = 0; i < hands.Keys.Count; i++)
        {
            Hand hand = hands[keys[i]];
            total_winnings += hand.bid * (i + 1);
        }

        Console.WriteLine(total_winnings);

    }

    static void Part2()
    {
        SortedDictionary<long, Hand> hands = ParseHands_Pt2();

        long[] keys = new long[hands.Keys.Count];
        hands.Keys.CopyTo(keys, 0);

        long total_winnings = 0;
        for (int i = 0; i < hands.Keys.Count; i++)
        {
            Hand hand = hands[keys[i]];
            total_winnings += hand.bid * (i + 1);
        }

        Console.WriteLine(total_winnings);

    }

    static void Main()
    {
        ReadInput("input");

        Part1(); // Correct answer: 246912307
        Part2(); // Correct answer: 246894760
    }
}