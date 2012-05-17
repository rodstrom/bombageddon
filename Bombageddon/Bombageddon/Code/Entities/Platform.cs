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
        public Platform(Bombageddon game, SpriteBatch spriteBatch, PlatformData construct)
            : base(game, spriteBatch)
        {
            this.position = construct.position;
            base.pointsWorth = construct.points;

            AnimationStrip explosion = new AnimationStrip();
            for (int x = 0; x < construct.Texture.Width / construct.Texture.Height; x++)
            {
                explosion.AddFrame(new AnimationFrame(construct.Texture, new Rectangle(construct.Texture.Height * x, 0, construct.Texture.Height, construct.Texture.Height), construct.HitTexture));
            }
            explosion.TimeOnChange = 50;

            this.AddAnimation("Explosion", explosion);
            this.AnimationName = "Explosion";

            explosion = null;

            this.pause = true;
            CurrentIndex = 0;

            //this.Scale *= 2f;

            //this.sourceTexture = texture;
            //this.hitTexture = hitTexture;

            //this._hitRectangle = hitRectangle;
            //this._hitRectangle2 = new Rectangle(0, 0, 0, 0);
            //this.hitRect2Enabled = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (pause && CurrentIndex > 0)
            {
                CurrentIndex = 0;
            }

            base.Update(gameTime);
        }

        //public void IsKilled()
        //{
        //    this.AnimationName = "Explosion";
        //}

    }
}
