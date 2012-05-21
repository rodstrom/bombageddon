 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bombageddon.Code.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Bombageddon.Code.Entities
{
    class Platform : Animation
    {
        public int life = 0;

        public Platform(Bombageddon game, SpriteBatch spriteBatch, PlatformData construct)
            : base(game, spriteBatch)
        {
            this.position = construct.position;
            base.pointsWorth = construct.points;

            this.life = construct.life;

            AnimationStrip explosion = new AnimationStrip();
            for (int x = 0; x < construct.Texture.Width / construct.Texture.Height; x++)
            {
                explosion.AddFrame(new AnimationFrame(construct.Texture, new Rectangle(construct.Texture.Height * x, 0, construct.Texture.Height, construct.Texture.Height), construct.HitTexture));
            }
            explosion.TimeOnChange = 50;

            this.AddAnimation("Explosion", explosion);
            this.AnimationName = "Explosion";

            explosion = null;

            //Pause("Explosion", true);
            pause = true;

            CurrentIndex = 0;
        }
    }
}
