using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bombageddon.Code.Input
{
    class LetterInput
    {
        List<char> letterList = new List<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray());

        int currentIndex = 0;

        public LetterInput() 
        {
        }

        public char CurrentLetter
        {
            get { return letterList[currentIndex]; }
        }

        public void NextLetter()
        {
            currentIndex++;
            if (currentIndex > letterList.Count - 1)
            {
                currentIndex = 0;
            }
        }

        public void PreviousLetter()
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = letterList.Count - 1;
            }
        }

        public bool LockLetter()
        {
            return true;
        }
    }
}
