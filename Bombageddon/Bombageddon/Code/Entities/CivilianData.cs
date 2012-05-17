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

        //public Texture2D emptySheet;

        public int pointsWorth = 0;

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

        public string type;

        public CivilianData(Bombageddon game, String filename, int points, String type)
        {
            this.civilianType = filename;
            this.pointsWorth = points;
            this.type = type;

            //this.emptySheet = empty;

            filename = @"Graphics\Sheeples\" + filename + "\\" + filename;

            panicSheet = game.Content.Load<Texture2D>(filename + "_panic");
            collisionSheet = game.Content.Load<Texture2D>(filename + "_collision");
            splatDeathSheet = game.Content.Load<Texture2D>(filename + "_splat");
            randomDeathSheet1 = game.Content.Load<Texture2D>(filename + "_flatten");
            randomDeathSheet2 = game.Content.Load<Texture2D>(filename + "_explosion");
        }

        public void Terminate()
        {
            panicSheet = null;
            collisionSheet = null;
            splatDeathSheet = null;
            randomDeathSheet1 = null;
            randomDeathSheet2 = null;
        }
    }
}
