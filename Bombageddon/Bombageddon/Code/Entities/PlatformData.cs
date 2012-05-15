using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Bombageddon.Code.Entities
{
    class PlatformData
    {
        public Texture2D Texture;
        public Texture2D HitTexture;

        public int points;
        public Vector2 position;

        public PlatformData(Bombageddon game, String textureName, String collisionName, Vector2 position, int points)
        {
            Texture = game.Content.Load<Texture2D>(textureName);
            HitTexture = game.Content.Load<Texture2D>(collisionName);
            this.position = position;
            this.points = points;
        }

        public void Terminate()
        {
            Texture.Dispose();
            HitTexture.Dispose();
        }
    }
}
