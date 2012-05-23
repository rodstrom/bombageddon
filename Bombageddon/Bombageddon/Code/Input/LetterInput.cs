using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bombageddon.Code.Input
{
    class LetterInput
    {
        List<char> letterList = new List<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray());

        int[] currentIndex = new int[3] { 0, 0, 0 };
        int currentSlot = 0;

        public LetterInput() 
        {
        }

        public void NextSlot()
        {
            currentSlot++;
            if (currentSlot > 2)
            {
                currentSlot = 2;
            }
        }

        public void PreviousSlot()
        {
            currentSlot--;
            if (currentSlot < 0)
            {
                currentSlot = 0;
            }
        }

        public char CurrentLetter
        {
            get { return letterList[currentIndex[currentSlot]]; }
        }

        public void NextLetter()
        {
            currentIndex[currentSlot]++;
            if (currentIndex[currentSlot] > letterList.Count - 1)
            {
                currentIndex[currentSlot] = 0;
            }
        }

        public void PreviousLetter()
        {
            currentIndex[currentSlot]--;
            if (currentIndex[currentSlot] < 0)
            {
                currentIndex[currentSlot] = letterList.Count - 1;
            }
        }

        public bool LockLetter()
        {
            return true;
        }
    }
}
