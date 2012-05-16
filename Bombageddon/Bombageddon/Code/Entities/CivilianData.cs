using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Bombageddon.Code.Entities
{
    class CivilianData
    {
        public string civilianType;

        public Texture2D panicSheet;
        public Texture2D collisionSheet;
        public Texture2D splatDeathSheet;
        public Texture2D randomDeathSheet1;
        public Texture2D randomDeathSheet2;

        public int panicFramesCount
        {
            get { return panicSheet.Width / panicSheet.Height; }
        }
        public int splatDeathFramesCount
        {
            get { return splatDeathSheet.Width / splatDeathSheet.Height; }
        }
        public int deathFramesCount1
        {
            get { return randomDeathSheet1.Width / randomDeathSheet1.Height; }
        }
        public int deathFramesCount2
        {
            get { return randomDeathSheet2.Width / randomDeathSheet2.Height; }
        }

        public CivilianData(Bombageddon game, String filename)
        {
            this.civilianType = filename;

            filename = @"Graphics\Sheeples\" + filename + "\\" + filename;

            panicSheet = game.Content.Load<Texture2D>(filename + "_panic");
            collisionSheet = game.Content.Load<Texture2D>(filename + "_collision");
            splatDeathSheet = game.Content.Load<Texture2D>(filename + "_splat");
            randomDeathSheet1 = game.Content.Load<Texture2D>(filename + "_flatten");
            randomDeathSheet2 = game.Content.Load<Texture2D>(filename + "_explosion");
        }
    }
}
