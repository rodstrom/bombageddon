using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bombageddon.Code.Time
{
    public class Event
    {
        public float currentTime, lastTime;

        public const float nullTime = 0.0f;

        public bool active;
    }
}
