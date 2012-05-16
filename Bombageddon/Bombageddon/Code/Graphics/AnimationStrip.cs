using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bombageddon.Code.Graphics
{
    class AnimationStrip
    {
        List<AnimationFrame> frames = new List<AnimationFrame>();

        int timeOnChange = 0;
        int currentTime = 0;
        int currentIndex = 0;

        bool looping = false;

        private AnimationFrame LastFrame
        {
            get
            {
                return frames[frames.Count - 1];
            }
        }

        public bool WillLoop
        {
            set
            {
                looping = value;
            }
        }

        public int AnimationFrames
        {
            get
            {
                return frames.Count - 1;
            }
        }

        public int CurrentIndex
        {
            get { return currentIndex; }
        }

        public int TimeOnChange
        {
            get { return timeOnChange; }
            set { timeOnChange = value; }
        }

        public AnimationStrip()
        {
        }

        public void Reset()
        {
            currentIndex = 0;
            currentTime = 0;
        }

        public void AddFrame(AnimationFrame spriteFrame)
        {
            frames.Add(spriteFrame);
        }

        public AnimationFrame getCurrentFrame(GameTime gameTime)
        {
            int newTime = currentTime + gameTime.ElapsedGameTime.Milliseconds;

            if (newTime >= timeOnChange)
            {
                currentIndex++;
                
                if (currentIndex > frames.Count - 1)
                {
                    if (looping)
                        currentIndex = 0;
                    else
                        return LastFrame;
                }

                currentTime = 0;
            }
            else
            {
                currentTime = newTime;
            }

            return frames[currentIndex];
        }
    }
}
