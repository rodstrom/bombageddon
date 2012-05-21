using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Bombageddon.Code.Graphics;

namespace Bombageddon.Code.Entities
{
    class PlatformData
    {
        public Texture2D Texture;
        public Texture2D HitTexture;

        public int points;
        public Vector2 position;

        public int life;

        public PlatformData(Bombageddon game, String textureName, String collisionName, Vector2 position, int points, int life)
        {
            Texture = game.Content.Load<Texture2D>(textureName);
            HitTexture = game.Content.Load<Texture2D>(collisionName);

            this.position = position;
            this.points = points;
            this.life = life;
        }

        public void Terminate()
        {
            //Texture.Dispose();
            //HitTexture.Dispose();

            Texture = null;
            HitTexture = null;
        }
    }
}
