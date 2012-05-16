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

        public AnimationStrip still;
        public AnimationStrip explosion;

        public int points;
        public Vector2 position;

        public PlatformData(Bombageddon game, String textureName, String collisionName, int animationFrames, int sideLength, Vector2 position, int points)
        {
            Texture = game.Content.Load<Texture2D>(textureName);
            HitTexture = game.Content.Load<Texture2D>(collisionName);

            still = new AnimationStrip();
            still.AddFrame(new AnimationFrame(Texture, new Rectangle(0, 0, sideLength, sideLength), HitTexture));
            still.TimeOnChange = 0;

            explosion = new AnimationStrip();
            for (int x = 0; x < animationFrames; x++)
            {
                explosion.AddFrame(new AnimationFrame(Texture, new Rectangle(sideLength * x, 0, sideLength, sideLength), HitTexture));
            }
            explosion.TimeOnChange = 50;

            this.position = position;
            this.points = points;
        }

        public void Terminate()
        {
            Texture = null;
            HitTexture = null;
        }
    }
}
