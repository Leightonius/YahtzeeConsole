using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YahtzeeMain
{
    public class Scoreboard
    {
        private int[] scores = new int[15];
        public readonly Dictionary<int, string> scoreTitles = new Dictionary<int, string>();
        private bool upperBonusScored = false;

        //Constructor
        public Scoreboard()
        {
            //Initialize blank scoreboard
            for (int score = 0; score < 13; score++)
                scores[score] = -1;
            scores[13] = 0; //Set upper bonus
            scores[14] = 0; //Set yahtzee bonus

            //Initialize scoreTitles
            scoreTitles.Add(0, "1s");
            scoreTitles.Add(1, "2s");
            scoreTitles.Add(2, "3s");
            scoreTitles.Add(3, "4s");
            scoreTitles.Add(4, "5s");
            scoreTitles.Add(5, "6s");
            scoreTitles.Add(6, "3 of a Kind");
            scoreTitles.Add(7, "4 of a Kind");
            scoreTitles.Add(8, "Full House");
            scoreTitles.Add(9, "Small Straight");
            scoreTitles.Add(10, "Large Straight");
            scoreTitles.Add(11, "Chance");
            scoreTitles.Add(12, "Yahtzee");
            scoreTitles.Add(13, "Upper Bonus");
            scoreTitles.Add(14, "Yahtzee Bonus");
        }

        //Get/Set - scores
        public int[] Scores
        {
            get { return scores; }
            set { scores = value; }
        }

        //Get/Set - upperBonusScored
        public bool UpperBonusScored
        {
            get { return upperBonusScored; }
            set { upperBonusScored = value; }
        }

        //Method - Get score total
        public int GetScoreTotal()
        {
            int scoreTotal = 0;

            foreach (int score in scores)
                if (score >= 0)
                    scoreTotal = scoreTotal + score;
            return scoreTotal;
        }

        //Method - Score Upper Bonus
        public void ScoreUpperBonus()
        {
            if (scores[0] + scores[1] + scores[2] + scores[3] + scores[4] + scores[5] >= 63)
            {
                scores[13] = 35;
                upperBonusScored = true;
            }
            if (scores[0] != -1 && scores[1] != -1 && scores[2] != -1 && scores[3] != -1 && scores[4] != -1 && scores[5] != -1)
                upperBonusScored = true;
        }

        //Method - Validate Roll
        public bool ValidateRoll(int scoreChoice, Player player)
        {
            switch (scoreChoice)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:// 1-6s
                    return true;
                case 6:// 3 of a Kind
                    int[] count3OfAKind = { 0, 0, 0 };

                    //count number of each dice
                    foreach (int die in player.Roll)
                    {
                        if (die == player.Roll[0])
                            count3OfAKind[0]++;
                        else if (die == player.Roll[1])
                            count3OfAKind[1]++;
                        else if (die == player.Roll[2])
                            count3OfAKind[2]++;
                    }

                    //return if roll is valid or not
                    if (count3OfAKind[0] >= 3 || count3OfAKind[1] >= 3 || count3OfAKind[2] >= 3)
                        return true;
                    else
                        return false;

                case 7:// 4 of a Kind
                    int[] count4OfAKind = { 0, 0 };

                    //count the number of each dice
                    foreach (int die in player.Roll)
                    {
                        if (die == player.Roll[0])
                            count4OfAKind[0]++;
                        else if (die == player.Roll[1])
                            count4OfAKind[1]++;
                    }

                    //return if roll is valid or not
                    if (count4OfAKind[0] >= 4 || count4OfAKind[1] >= 4)
                        return true;
                    else
                        return false;

                case 8:// Full House
                    int die2 = 0;
                    int[] countFullHouse = { 0, 0 };

                    //get second number
                    foreach (int die in player.Roll)
                        if (die != player.Roll[0])
                            die2 = die;

                    //count total of each number
                    foreach (int die in player.Roll)
                    {
                        if (die == player.Roll[0])
                            countFullHouse[0]++;
                        else if (die == die2)
                            countFullHouse[1]++;
                    }

                    //return if roll is valid or not
                    if ((countFullHouse[0] == 2 && countFullHouse[1] == 3) ||
                        (countFullHouse[0] == 3 && countFullHouse[1] == 2))
                        return true;
                    else
                        return false;

                case 9:// Small Straight
                    bool[] isSmallStraight = { false, false, false, false, false, false, false };
                    int countSmallStraight = 0;

                    //check for each number
                    foreach (int die in player.Roll)
                        isSmallStraight[die] = true;

                    //Count consecutive numbers
                    foreach (bool numberIsHere in isSmallStraight)
                    {
                        if (numberIsHere)
                            countSmallStraight++;
                            if (countSmallStraight == 4)
                                break;
                        else if (!numberIsHere)
                            countSmallStraight = 0;
                    }

                    //return if roll is valid or not
                    if (countSmallStraight >= 4)
                        return true;
                    else
                        return false;

                case 10:// Large Straight
                    bool[] countLargeStraight = { true, false, false, false, false, false, false };

                    //Check for each number
                    foreach (int die in player.Roll)
                        countLargeStraight[die] = true;

                    //return if roll is valid or not
                    if (countLargeStraight[2] &&
                        countLargeStraight[3] &&
                        countLargeStraight[4] &&
                        countLargeStraight[5] && (
                        countLargeStraight[1] || countLargeStraight[6]))
                        return true;
                    else
                        return false;

                case 11:// Chance                    
                    return true;
                default:
                    return false;

                case 12:// Yahtzee
                    foreach (int die in player.Roll)
                        if (die != player.Roll[0])
                            return false;
                    return true;
            }
        }

        //Method - Set scores
        public bool SetScore(int scoreChoice, bool validRoll, Player player)
        {
            switch (scoreChoice)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:// 1-6s
                    int singleScoreTotal = 0;
                    for (int die = 0; die < 5; die++)
                        if (player.Roll[die] == scoreChoice + 1)
                            singleScoreTotal = singleScoreTotal + scoreChoice + 1;
                    scores[scoreChoice] = singleScoreTotal;
                    return true;

                case 6:// 3 of a Kind
                    if (validRoll)
                    {
                        scores[6] = player.TotalRoll();
                        return true;
                    }
                    else
                    {
                        scores[6] = 0;
                        return false;
                    }

                case 7:// 4 of a Kind
                    if (validRoll)
                    {
                        scores[7] = player.TotalRoll();
                        return true;
                    }
                    else
                    {
                        scores[7] = 0;
                        return false;
                    }

                case 8:// Full House
                    if (validRoll)
                    {
                        scores[8] = 25;
                        return true;
                    }
                    else
                    {
                        scores[8] = 0;
                        return false;
                    }

                case 9:// Small Straight
                    if (validRoll)
                    {
                        scores[9] = 30;
                        return true;
                    }
                    else
                    {
                        scores[9] = 0;
                        return false;
                    }

                case 10:// Large Straight
                    if (validRoll)
                    {
                        scores[10] = 40;
                        return true;
                    }
                    else
                    {
                        scores[10] = 0;
                        return false;
                    }

                case 11:// Chance
                    scores[11] = player.TotalRoll();
                    return true;
                default:
                    return false;

                case 12:// Yahtzee
                    if (validRoll)
                    {
                        scores[12] = 50;
                        return true;
                    }
                    else
                    {
                        scores[12] = 0;
                        return false;
                    }

            }
        }

        //Method - Return either dash or score as a string
        public string GetScoreOrDash(int score)
        {
            if (score < 0)
                return "-";
            else
                return Convert.ToString(score);
        }

        //Method - Display Scoreboard
        public void DisplayScoreboard(bool clearConsole)
        {
            if (clearConsole)
                Clear();

            //Row 1
            WriteLine(" ----------------------------------------------------- ");
            WriteLine("|1: 1's  |2: 2's  |3: 3's  |4: 4's  |5: 5's  |6: 6's  |");
            Write('|');
            for (int i = 0; i < 6; i++)
                Write("{0}|", GetScoreOrDash(scores[i]).PadLeft(5).PadRight(8));
            WriteLine();
            //Row 2
            WriteLine(" ----------------------------------------------------- ");
            WriteLine("|7:  3 of a kind   |8:  4 of a kind   |9:  Full House |");
            Write('|');
            Write(GetScoreOrDash(scores[6]).PadLeft(9).PadRight(18) + '|');
            Write(GetScoreOrDash(scores[7]).PadLeft(9).PadRight(18) + '|');
            Write(GetScoreOrDash(scores[8]).PadLeft(8).PadRight(15) + '|');
            WriteLine();
            //Row 3
            WriteLine(" ----------------------------------------------------- ");
            WriteLine("|10: Small Straight  |11: Large Straight  |12: Chance |");
            Write('|');
            Write(GetScoreOrDash(scores[9]).PadLeft(10).PadRight(20) + '|');
            Write(GetScoreOrDash(scores[10]).PadLeft(10).PadRight(20) + '|');
            Write(GetScoreOrDash(scores[11]).PadLeft(6).PadRight(11) + '|');
            WriteLine();
            //Row 4
            WriteLine(" ----------------------------------------------------- ");
            WriteLine("|13:  Yahtzee   | UPPER BONUS | YAHTZEE BONUS | TOTAL |");
            Write('|');
            Write(GetScoreOrDash(scores[12]).PadLeft(8).PadRight(15) + '|');
            Write(GetScoreOrDash(scores[13]).PadLeft(7).PadRight(13) + '|');
            Write(GetScoreOrDash(scores[14]).PadLeft(8).PadRight(15) + '|');
            Write(Convert.ToString(GetScoreTotal()).PadLeft(4).PadRight(7));
            WriteLine();
            //End
            WriteLine(" ----------------------------------------------------- ");
            WriteLine();
        }
    }
}
