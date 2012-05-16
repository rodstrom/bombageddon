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
        //string textureName;
        //string collisionName;

        public int pointsWorth = 0;
        public bool ghost = false;

        //private Rectangle _hitRectangle;
        //public Rectangle HitRectangle
        //{
        //    get
        //    {
        //        Rectangle r = new Rectangle();
        //        r.X = Rectangle.Left + _hitRectangle.X;
        //        r.Y = Rectangle.Top + _hitRectangle.Y;
        //        r.Width = _hitRectangle.Width - _hitRectangle.X;
        //        r.Height = Rectangle.Bottom - r.Y;
        //        return r;
        //    }
        //}

        //public bool hitRect2Enabled;
        //private Rectangle _hitRectangle2;
        //public Rectangle HitRectangle2
        //{
        //    get
        //    {
        //        if (hitRect2Enabled)
        //        {
        //            Rectangle r = new Rectangle();
        //            r.X = Rectangle.Left + _hitRectangle2.X;
        //            r.Y = Rectangle.Top + _hitRectangle2.Y;
        //            r.Width = _hitRectangle2.Width - _hitRectangle2.X;
        //            r.Height = _hitRectangle2.Height;
        //            return r;
        //        }
        //        return _hitRectangle2;
        //    }
        //}

        public Platform(Bombageddon game, SpriteBatch spriteBatch, PlatformData construct)
            : base(game, spriteBatch)
        {
            this.position = construct.position;
            this.pointsWorth = construct.points;

            this.AddAnimation("Explosion", construct.explosion);
            this.AddAnimation("Still", construct.still);

            this.AnimationName = "Still";

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

        public void IsKilled()
        {
            this.AnimationName = "Explosion";
        }

        //public Platform(Runner game, SpriteBatch spriteBatch, string filename, string collisionfile, Vector2 position)
        //    : base(spriteBatch, game)
        //{
        //    this.position = position;
        //    textureName = filename;
        //    collisionName = collisionfile;
        //    //this._hitRectangle = hitRectangles[0];
        //    //this._hitRectangle2 = hitRectangles[1];
        //    //this.hitRect2Enabled = true;
        //}

        //protected override void LoadContent()
        //{
        //    //SourceTexture = Game.Content.Load<Texture2D>(textureName);

        //    //hitTexture = Game.Content.Load<Texture2D>(collisionName);
        //    //GetColorData(hitTexture);
        //    //CollisionRectangle = SourceRectangle;
        //    ////GetHeight();

        //    //Origin = new Vector2(SourceTexture.Bounds.Center.X, SourceTexture.Bounds.Bottom);

        //    Scale *= 2f;
        //}
    }
}
