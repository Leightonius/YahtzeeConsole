using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YahtzeeMain
{

    public class Program
    {
        public static Validate validate = new Validate();
        public static void Main(string[] args)
        {
            //Variables
            Player player = new Player();

            int turn = 1;
            bool[] savedDice = { false, false, false, false, false, false };
            bool debug = false;

            //Set Debug ON or OFF
            if (args.Length > 0)
                if (args[0] == "d" || args[0] == "D")
                    debug = true;

            //Welcome Player
            Clear();
            SetWindowSize(56, 35);
            WriteLine("                   WELCOME TO:                    ");
            WriteLine();
            WriteLine(" __     __      _    _ _______ ____________ ______");
            WriteLine(" \\ \\   / //\\   | |  | |__   __|___  /  ____|  ____|");
            WriteLine("  \\ \\_/ //  \\  | |__| |  | |     / /| |__  | |__   ");
            WriteLine("   \\   // /\\ \\ |  __  |  | |    / / |  __| |  __|  ");
            WriteLine("    | |/ ____ \\| |  | |  | |   / /__| |____| |____ ");
            WriteLine("    |_/_/    \\_\\_|  |_|  |_|  /_____|______|______|");
            WriteLine();
            WriteLine("               Press Enter to Begin                ");
            WriteLine();
            ReadLine();

            //Move through turns
            while (turn < 14)
            {
                //Roll Turn
                for (int rollTurn = 0; rollTurn < 3; rollTurn++)
                {

                    //Reset saved choices                   
                    if (rollTurn == 0)
                    {
                        for (int die = 0; die < 5; die++)
                        {
                            savedDice[die] = false;
                        }
                    }

                    //Roll Dice
                    player.RollDice(savedDice);

                    //Display Roll
                    Clear();
                    if (debug) WriteLine("Debug On");
                    WriteLine("|     YAHTZEE      |     Turn: {0}     |     Roll {1}     |", turn, rollTurn + 1);
                    WriteLine();
                    player.scoreboard.DisplayScoreboard(false);
                    WriteLine();
                    player.DisplayRoll();
                    WriteLine();

                    //Get which dice to save             
                    if (debug)
                    {
                        player.RollDice(savedDice, debug);
                        rollTurn = 2;
                    }
                    else if (rollTurn != 2)
                        savedDice = GetDiceToSave();
                }

                //Prompt for scoreChoiceInput
                WriteLine();
                WriteLine("Where would you like to score your roll?");
                WriteLine("(Select the number from the scoreboard.)");

                //validate input and score roll
                int scoreChoice = 0;
                string scoreChoiceInput;

                do
                {
                    WriteLine();
                    Write("Score choice: ");
                    scoreChoiceInput = ReadLine();

                    //Verify input is a number between 1 and                     
                    if (!Int32.TryParse(scoreChoiceInput, out scoreChoice) || scoreChoice < 1 || scoreChoice > 13)
                    {
                        WriteLine("\"Score Choice\" must be a number between 1 and 13.\nPlease try again.");
                        continue;
                    }

                    //Check if score has already been scored                    
                    if (player.scoreboard.Scores[scoreChoice - 1] >= 0)
                    {
                        Write(player.scoreboard.scoreTitles[scoreChoice - 1]);
                        if (scoreChoice < 7)
                        {
                            Write(" have ");
                        }
                        else
                        {
                            Write(" has ");
                        }
                        WriteLine("already been scored. Please select another score.");
                        continue;
                    }

                    //Validate roll for scoreChoice selected
                    bool validRoll;
                    if (!(validRoll = player.scoreboard.ValidateRoll(scoreChoice - 1, player)))
                    {
                        WriteLine();
                        WriteLine("Your roll was not a valid {0}.", player.scoreboard.scoreTitles[scoreChoice - 1]);
                        WriteLine("Would you like to score 0 in {0}?", player.scoreboard.scoreTitles[scoreChoice - 1]);
                        string savedInput;
                        do
                        {
                            WriteLine();
                            Write("'y' or 'n':");
                            savedInput = ReadLine();
                            if (savedInput.ToLower() != "y" && savedInput.ToLower() != "n")
                            {
                                WriteLine();
                                WriteLine("Invalid choice.");
                                continue;
                            }
                            break;
                        } while (true);
                        if (savedInput == "n")
                        {
                            continue;
                        }
                    }

                    //Score the roll
                    player.scoreboard.SetScore(scoreChoice - 1, validRoll, player);

                    //Check for upper bonus
                    if (!player.scoreboard.UpperBonusScored)
                    {
                        player.scoreboard.ScoreUpperBonus();
                    }

                    //Check for yahtzee bonus                    
                    if (player.scoreboard.Scores[12] == 50 && player.scoreboard.ValidateRoll(12, player) && scoreChoice != 13)
                    {
                        player.scoreboard.Scores[14] += 50;
                    }
                    break;
                } while (true);

                //Increment turn counter                
                turn++;
            }

            //End Game
            player.scoreboard.DisplayScoreboard(true);
            WriteLine();
            WriteLine("Thank you for playing Yahtzee.");
            WriteLine();
            WriteLine("Press Enter to close.");
            ReadLine();
        }

        //Get the dice to save
        public static bool[] GetDiceToSave(bool debug = false)
        {
            bool[] savedDice = { false, false, false, false, false };

            WriteLine();
            WriteLine("Which dice would you like to save?");
            WriteLine("(Type the numbers for each dice.)");
            do
            {
                WriteLine();
                Write("Save Choice: ");
                string savedInput = ReadLine();

                if (!validate.IsValidDieSelection(savedInput))
                {
                    WriteLine();
                    WriteLine("Invalid selection. Please type only 1-5 of the numbers \nof the dice you want to save.");
                    continue;
                }

                foreach (char c in savedInput)
                {
                    if (c - '0' >= 1 && c - '0' <= 5)
                    {
                        savedDice[c - '0' - 1] = true;
                    }
                }
                break;
            } while (true);

            return savedDice;
        }
    }
}
