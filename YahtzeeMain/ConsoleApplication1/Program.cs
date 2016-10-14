#include <iostream>
#include <iomanip>
#include <fstream>
#include <cstdlib>
using namespace std;

void srand();
int  randNumGen();
void resetChoices(int aRoll[]);
void roll(int aRoll[]);
void displayRoll(int aRoll[]);
void holdDie(int *aRoll);
void turn(int *aRoll);
int  totalScore(int *aScores);
void dispScores(int *aScores);
char chooseScore(int *aScores);
void scoreRoll(char scoreChoice, int *aRoll, int *aScores);
void scoreNumbers(char scoreChoice, int *aRoll, int *aScores);
void score3OfAKind(int *aRoll, int *aScores);
void scoreFullHouse(int *aRoll, int *aScores);
void scoreSmallStraight(int *aRoll, int *aScores);
void scoreYahtzee(int *aRoll, int *aScores);

/********************************************************************
 * MAIN
 * plays yahtzee
 ********************************************************************/
int main()
{
   //variables
   int  aRoll[6] = {};
   int  aScores[11] = {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
   char scoreChoice;
   
   //seed random number generator
   srand();

   //play game
   for (int i = 1; i <= 10; i++)
   {
      turn(aRoll);
      scoreChoice = chooseScore(aScores);
      scoreRoll(scoreChoice, aRoll, aScores);
   }

   //display final score
   dispScores(aScores);
   
   return 0;
}

/********************************************************************
 * SRAND
 * seed the random number generator
 ********************************************************************/
void srand()
{
   srand(time(NULL));

   return;
}

/*********************************************************************
 * RAND NUM GEN
 * generates random number between 1 and 6
 *********************************************************************/
int randNumGen()
{
   //RAND_MAX is 2147483647
   return ((rand() / (RAND_MAX / 6)) + 1);
}

/**********************************************************************
 * RESET CHOICES
 * resets die to absolutes
 **********************************************************************/
void resetChoices(int aRoll[])
{
   //reset choices
   for (int i = 0; i <= 5; i++)
      aRoll[i] = abs(aRoll[i]);

   return;
}

/**********************************************************************
 * ROLL
 * generates new random numbers for chosen die
 **********************************************************************/
void roll(int aRoll[])
{
   //Roll die
   for(int die = 1; die < 6; die++)
      if (aRoll[die] >= 0)
         aRoll[die] = randNumGen();

   return;
}

/***********************************************************************
 * DISPLAY ROLL
 * displays the roll on the screen
 ***********************************************************************/
void displayRoll(int aRoll[])
{
   //display roll header
   cout << "Die 1   Die 2   Die 3   Die 4   Die 5" << endl;

   //display each die
   cout << "  "
        << abs(aRoll[1]) << setw(8)
        << abs(aRoll[2]) << setw(8)
        << abs(aRoll[3]) << setw(8)
        << abs(aRoll[4]) << setw(8)
        << abs(aRoll[5]) << endl;

   return;
}

/***********************************************************************
 * holdDie
 * get choices from user and reserve die
 ***********************************************************************/
void holdDie(int *aRoll)
{
   int  iDie[6];
   char choices[256];

   //prompt for and get choices
   cout << "Type which dice you want to keep. (i.e. '245')" << endl << "> ";
   cin  >> choices;

   //reserve die for choices
   for (int i = 0; choices[i]; i++)
      if (isdigit(choices[i]))
      {
         int dice = choices[i] - '0';
         if (dice > 0 && dice <= 5)
            aRoll[dice] = -(abs(aRoll[dice]));
      }

   return;
}

/***********************************************************************
 * TURN
 * roll three times
 ***********************************************************************/
void turn(int *aRoll)
{
   //turn
   for (int turn = 1; turn <= 3; turn++)
   {
      if (turn == 1)
         resetChoices(aRoll);

      roll(aRoll);
      displayRoll(aRoll);

      if (turn == 2)
         resetChoices(aRoll);

      if (turn == 1 || turn == 2)
         holdDie(aRoll);

      if (turn == 3)
         resetChoices(aRoll);
   }
   
   return;
}

/************************************************************************
 * TOTAL SCORE
 * totals score
 ************************************************************************/
int totalScore(int *aScores)
{
   //variables
   int sum = 0;
   
   //total the score
   for (int i = 1; i <= 10; i++)
      if (aScores[i] != -1)
         sum += aScores[i];

   return sum;
}
         
/*************************************************************************
 * DISP SCORES
 * display Scores on the screen
 *************************************************************************/
void dispScores(int *aScores)
{
   //display scores for 1-6s
   for (int i = 1; i <= 6; i++)
   {
      cout << i << ": " << i << "'s ------------- ";
      if (aScores[i] == -1)
         cout << '-' << endl;
      else
         cout << aScores[i] << endl;

   }

   //display score for 3 of a kind
   cout << "7: 3 of a kind ----- ";
   if (aScores[7] == -1)
      cout << '-' << endl;
   else
      cout << aScores[7] << endl;

   //display score for full house
   cout << "8: Full House ------ ";
   if (aScores[8] == -1)
      cout << '-' << endl;
   else
      cout << aScores[8] << endl;

   //display score for small straight
   cout << "9: Small Straight -- ";
   if (aScores[9] == -1)
      cout << '-' << endl;
   else
      cout << aScores[9] << endl;

   //display score for Yahtzee
   cout << "10: Yahtzee -------- ";
   if (aScores[10] == -1)
      cout << '-' << endl;
   else
      cout << aScores[10] << endl;

   //display total
   cout << "______________________" << endl
        << "             TOTAL - " << totalScore(aScores) << endl;
   
   return;
}

/***********************************************************************
 * CHOOSE SCORES
 * prompts for where user wants to score the roll and validates entry
 ***********************************************************************/
char chooseScore(int *aScores)
{
   //variables
   int choice;

   //prompt for score choice
   cout << "Where would you like to score the roll?" << endl;

   //display Scoreboard
   dispScores(aScores);

   //get score choice
   cout << "Choose 1-10: ";
   cin  >> choice;

   //validate score choice
   while (choice < 1 || choice > 10)
   {
      cout << "Invalid choice. Please type '1-10': ";
      cin  >> choice;
   }

   //check if score hasn't been scored yet
   while (aScores[choice] != -1)
   {
      cout << "That has already been scored. Please choose another option."
           << endl << "Choose 1-10: ";
      cin  >> choice;
   }
   
   return choice;
}

/************************************************************************
 * SCORE NUMBERS
 * scores the 1's through the 6's
 ************************************************************************/
void scoreNumbers(char scoreChoice, int *aRoll, int *aScores)
{
   //variables
   int sum = 0;
   
   //total score from die
   for (int i = 1; i <= 5; i++)
      if (aRoll[i] == scoreChoice)
         sum += scoreChoice;

   //score the die
   aScores[scoreChoice] = sum;
         
   return;
}

/*************************************************************************
 * SCORE 3 OF A KIND
 * validates roll and scores 3 of a kind
 *************************************************************************/
void score3OfAKind(int *aRoll, int *aScores)
{
   //variables
   int iCount[3] = {};
   int sums[3] = {};
   
   //count numbers of each dice
   for (int i = 1; i <= 5; i++)
   {
      if (aRoll[i] == aRoll[1])
         iCount[0]++;
      else if (aRoll[i] == aRoll[2])
         iCount[1]++;
      else if (aRoll[i] == aRoll[3])
         iCount[2]++;
   }

   //score 3 of a kind if roll is valid
   if (iCount[0] >= 3 || iCount[1] >= 3 || iCount[2] >= 3)
      for (int i = 1; i <= 5; i++)
         aScores[7] += aRoll[i];
   else
      aScores[7] = 0;
   
   return;
}

/************************************************************************
 * SCORE FULL HOUSE
 * validates roll and scores Full House
 ************************************************************************/
void scoreFullHouse(int *aRoll, int *aScores)
{
   //variables
   int iCount[2] = {};
   int die1 = aRoll[1];
   int die2 = 0;

   //check for two seperate numbers on die
   for (int i = 1; i <= 5; i++)
      if (aRoll[i] != aRoll[1])
         die2 = aRoll[i];

   //count numbers of each dice
   for (int i = 1; i <= 5; i++)
   {
      if (aRoll[i] == die1)
         iCount[0]++;
      else if (aRoll[i] == die2)
         iCount[1]++;
   }

   //score Full House if roll is valid
   if ((iCount[0] == 3 && iCount[1] == 2) ||
       (iCount[0] == 2 && iCount[1] == 3))
      aScores[8] = 25;
   else
      aScores[8] = 0;
   
   return;
}

/************************************************************************
 * SCORE SMALL STRAIGHT
 * validates roll and scores small straight
 ************************************************************************/
void scoreSmallStraight(int *aRoll, int *aScores)
{
   aScores[9] = 30;
   
   return;
}

/************************************************************************
 * SCORE YAHTZEE
 * validates roll and scores yahtzee
 ************************************************************************/
void scoreYahtzee(int *aRoll, int *aScores)
{
   //validate if roll is a yahtzee
   if  (aRoll[1] == aRoll[2] &&
        aRoll[1] == aRoll[3] &&
        aRoll[1] == aRoll[4] &&
        aRoll[1] == aRoll[5])
      aScores[10] = 50;
   else
      aScores[10] = 0;

   return;
}

/************************************************************************
 * SCORE ROLL
 * calls correct scoring function depending on users choice
 ************************************************************************/
void scoreRoll(char scoreChoice, int *aRoll, int *aScores)
{
   switch (scoreChoice)
   {
      //score 1-6s
      case 1:
      case 2:
      case 3:
      case 4:
      case 5:
      case 6:
         scoreNumbers(scoreChoice, aRoll, aScores);
         break;
      case 7:
         score3OfAKind(aRoll, aScores);
         break;
      case 8:
         scoreFullHouse(aRoll, aScores);
         break;
      case 9:
         scoreSmallStraight(aRoll, aScores);
         break;
      case 10:
         scoreYahtzee(aRoll, aScores);
         break;
   }
   
   return;
}



   
