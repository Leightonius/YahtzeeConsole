using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YahtzeeMain
{
    public class Player
    {
        public static Validate validate = new Validate();

        //constructor
        public Player()
        {
            scoreboard = new Scoreboard();
        }

        //variables
        public string Name { get; set; }
        public Scoreboard scoreboard;
        private int[] roll = new int[5];

        //Get/Set roll
        public int[] Roll
        {
            get { return roll; }
            set { roll = value; }
        }

        //Method - Sum of all dice
        public int TotalRoll()
        {
            int rollSum = 0;

            foreach (int die in roll)
            {
                rollSum = rollSum + die;
            }

            return rollSum;
        }

        //Method - Roll and save dice
        public void RollDice(bool[] saved, bool debug = false)
        {
            if (debug)
            {
                string input;
                do
                {
                    Write("DEBUG Input dice: ");
                    input = ReadLine();
                }
                while (!validate.IsValidDieInput(input));
                
                for (int x = 0; x < input.Length; ++x)
                    roll[x] = Convert.ToInt32(input[x] - '0');
            }
            else
            {
                Random rand = new Random();

                for (int die = 0; die < 5; die++)
                {
                    if (!saved[die]) //Don't reroll if saved
                    {
                        roll[die] = rand.Next(1, 7);
                    }
                } 
            }
        }

        //Method - Display ASCII Roll
        public void DisplayRoll()
        {

            //DISPLAY ROLL
            //Titles and Header
            WriteLine("    Die 1     Die 2     Die 3     Die 4     Die 5");
            WriteLine("    -----     -----     -----     -----     -----");
            //Row 1
            foreach (int die in roll)
            {
                Write("   ");

                switch (die)
                {
                    case 0:
                        break;
                    case 1:
                        Write("|     |");
                        break;
                    case 2:
                    case 3:
                        Write("|\u2219    |");
                        break;
                    case 4:
                    case 5:
                    case 6:
                        Write("|\u2219   \u2219|");
                        break;
                    default:
                        break;
                }
            }
            //Row 2
            WriteLine();

            foreach (int die in roll)
            {
                Write("   ");

                switch (die)
                {
                    case 0:
                        break;
                    case 1:
                        Write("|  \u2219  |");
                        break;
                    case 2:
                        Write("|     |");
                        break;
                    case 3:
                        Write("|  \u2219  |");
                        break;
                    case 4:
                        Write("|     |");
                        break;
                    case 5:
                        Write("|  \u2219  |");
                        break;
                    case 6:
                        Write("|\u2219   \u2219|");
                        break;
                    default:
                        break;
                }
            }
            //Row 3
            WriteLine();

            foreach (int die in roll)
            {
                Write("   ");

                switch (die)
                {
                    case 0:
                        break;
                    case 1:
                        Write("|     |");
                        break;
                    case 2:
                    case 3:
                        Write("|    \u2219|");
                        break;
                    case 4:
                    case 5:
                    case 6:
                        Write("|\u2219   \u2219|");
                        break;
                    default:
                        break;
                }
            }
            //Footer
            WriteLine();
            WriteLine("    -----     -----     -----     -----     -----");
        }
    }
}
