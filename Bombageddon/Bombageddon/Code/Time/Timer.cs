using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombageddon.Code.Time
{
    public class Timer : GameComponent
    {
        public List<Event> timerList = new List<Event>();

        public Event MainEvent;
        public Event SubEvent;

        public Timer(Game game)
            : base(game)
        {
            this.Initialize();
        }

        public override void Initialize()
        {
            MainEvent = new Event();
            MainEvent.currentTime = 0.0f;
            MainEvent.lastTime = 0.0f;
            MainEvent.active = false;

            base.Initialize();
        }
        
        public void StartTimer()
        {
            MainEvent.active = true;
        }

        public void StopTimer()
        {
            MainEvent.active = false;
        }

        public void ResetTimer()
        {
            MainEvent.currentTime = 0.0f;
            MainEvent.lastTime = 0.0f;
        }

        public void CreateSubTimer()
        {
            SubEvent = new Event();
            SubEvent.currentTime = 0.0f;
            SubEvent.lastTime = 0.0f;
            SubEvent.active = false;
            timerList.Add(SubEvent);
        }

        public void RemoveSubTimer(int index)
        {
            if (timerList.Count != 0)
            {
                timerList.RemoveAt((int)MathHelper.Clamp(index, 0, timerList.Count - 1));
            }
        }

        public void StartSubTimer(int index)
        {
            if (timerList.Count != 0)
            {
                timerList[(int)MathHelper.Clamp(index, 0, timerList.Count - 1)].active = true;
            }
        }

        public void StopSubTimer(int index)
        {
            if (timerList.Count != 0)
            {
                timerList[(int)MathHelper.Clamp(index, 0, timerList.Count - 1)].active = false;
            }
        }

        public void ResetSubTimer(int index)
        {
            if (timerList.Count != 0)
            {
                timerList[(int)MathHelper.Clamp(index, 0, timerList.Count - 1)].currentTime = 0.0f;
                timerList[(int)MathHelper.Clamp(index, 0, timerList.Count - 1)].lastTime = 0.0f;
            }
        }

        public void SaveTimeScore()
        {

        }

        public string ToString(String format)
        {
            if (format == "ms_total")
            {
                return MainEvent.currentTime.ToString();
            }
            if (format == "s_total")
            {
                return (MainEvent.currentTime / 1000).ToString();
            }
            if (format == "m_total")
            {
                return (MainEvent.currentTime / 60000).ToString();
            }
            if (format == "ms_mod")
            {
                return (MainEvent.currentTime % 1000).ToString();
            }
            if (format == "s_mod")
            {
                return (MainEvent.currentTime % 60000).ToString();
            }
            return base.ToString();
        }

        public int ToInteger(String format)
        {
            if (format == "ms_total")
            {
                return (int)MainEvent.currentTime;
            }
            if (format == "s_total")
            {
                return (int)MainEvent.currentTime / 1000;
            }
            if (format == "m_total")
            {
                return (int)MainEvent.currentTime / 60000;
            }
            if (format == "ms_mod")
            {
                return (int)MainEvent.currentTime % 1000;
            }
            if (format == "s_mod")
            {
                return (int)MainEvent.currentTime % 60000;
            }
            return 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (MainEvent.active)
            {
                MainEvent.lastTime = MainEvent.currentTime;
                MainEvent.currentTime = gameTime.ElapsedGameTime.Milliseconds + MainEvent.lastTime;
            }

            for ( int x = 0; x < timerList.Count; x++ )
            {
                if (timerList[x].active)
                {
                    timerList[x].lastTime = timerList[x].currentTime;
                    timerList[x].currentTime = gameTime.ElapsedGameTime.Milliseconds + timerList[x].lastTime;
                }
            }

            base.Update(gameTime);
        }
    }
}
