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
        public int pointsWorth = 0;
        public bool ghost = false;

        public Platform(Bombageddon game, SpriteBatch spriteBatch, PlatformData construct)
            : base(game, spriteBatch)
        {
            this.position = construct.position;
            this.pointsWorth = construct.points;

            this.AddAnimation("Explosion", construct.explosion);
            //this.AddAnimation("Still", construct.still);

            this.AnimationName = "Explosion";

            this.pause = true;

            //this.Scale *= 2f;

            //this.sourceTexture = texture;
            //this.hitTexture = hitTexture;

            //this._hitRectangle = hitRectangle;
            //this._hitRectangle2 = new Rectangle(0, 0, 0, 0);
            //this.hitRect2Enabled = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (AnimationName == "Explosion" && CurrentIndex == AnimationFrames)
            {
                //KillMe = true;
                pause = true;
            }

            base.Update(gameTime);
        }

        //public void IsKilled()
        //{
        //    this.AnimationName = "Explosion";
        //}

    }
}
