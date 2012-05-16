using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Bombageddon.Code.Entities
{
    struct CivilianData
    {
        public string civilianType;

        public Texture2D panicSheet;
        public Texture2D collisionSheet;
        public Texture2D splatDeathSheet;
        public Texture2D randomDeathSheet1;
        public Texture2D randomDeathSheet2;

        public int panicFramesCount;
        public int splatDeathFramesCount;
        public int deathFramesCount1;
        public int deathFramesCount2;
    }
}
