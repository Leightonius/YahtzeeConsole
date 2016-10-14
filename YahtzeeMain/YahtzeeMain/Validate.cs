using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YahtzeeMain
{
    public class Validate
    {
        public bool IsValidDieSelection(string s)
        {
            string sTrimmed = s.Trim();

            //If it's not a digit
            for (int i = 0; i < sTrimmed.Length; i++)
            {
                if (!char.IsDigit(sTrimmed[i])) return false;
            }

            //If it is out of range 1-5
            for (int i = 0; i < sTrimmed.Length; i++)
            {
                if (Convert.ToInt32(sTrimmed[i] - '0') < 1 || Convert.ToInt32(sTrimmed[i] - '0') > 5) return false;
            }

            //If it is a digit
            return true;
        }

        public bool IsValidDieInput(string s)
        {
            string sTrimmed = s.Trim();

            //If not all dice
            if (sTrimmed.Length != 5)
                return false;

            //If it's not a digit
            for (int i = 0; i < sTrimmed.Length; i++)
            {
                if (!char.IsDigit(sTrimmed[i])) return false;
            }

            //If it is out of range 1-6
            for (int i = 0; i < sTrimmed.Length; i++)
            {
                if (Convert.ToInt32(sTrimmed[i] - '0') < 1 || Convert.ToInt32(sTrimmed[i] - '0') > 6) return false;
            }

            return true;
        }
    }
}
