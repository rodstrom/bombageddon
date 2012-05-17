using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bombageddon.Code.Entities;

namespace Bombageddon.Code.Graphics
{
    class Animation : Entity
    {
        Dictionary<String, AnimationStrip> animationList = new Dictionary<String, AnimationStrip>();
        String currentAnimation = "";
        public bool pause = false;

        AnimationFrame currentFrame = null;

        public override void Terminate()
        {
            pause = false;
            base.Terminate();
        }

        public void LoopAnimation(string anim, bool loop)
        {
            animationList[anim].WillLoop = loop;
        }

        protected int AnimationFrames
        {
            get
            {
                return animationList[AnimationName].AnimationFrames;
            }
        }

        protected int CurrentIndex
        {
            get
            {
                return animationList[AnimationName].CurrentIndex;
            }
            set
            {
                animationList[AnimationName].CurrentIndex = value;
            }
        }

        public AnimationFrame CurrentFrame
        {
            get { return currentFrame; }
        }

        public Color[,] SetColorData
        {
            get { return currentFrame.ColorData; }
        }

        public int[] SetHeightMap
        {
            get { return currentFrame.HeightMap; }
        }

        protected void UpdateSourceRectangle()
        {
            int x = (int)(position.X - Origin.X);
            int y = (int)(position.Y - Origin.Y);
            SourceRectangle = new Rectangle(x, y, currentFrame.SourceRectangle.Width, currentFrame.SourceRectangle.Height);
        }

        protected void UpdateCollisionRectangle()
        {
            CollisionRectangle = new Rectangle(SourceRectangle.X - currentFrame.SourceRectangle.X,
                SourceRectangle.Y - currentFrame.SourceRectangle.Y,
                currentFrame.SourceRectangle.Width,
                currentFrame.SourceRectangle.Height);
        }

        public string AnimationName
        {
            get { return currentAnimation; }
            set
            {
                animationList[currentAnimation].Reset();
                currentAnimation = value;
            }
        }

        public Color _color = Color.White;
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public void AddAnimation(string id, AnimationStrip anim)
        {
            animationList[id] = anim;
            currentAnimation = id;
        }

        public Animation(Bombageddon game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (currentFrame == null || ColorData == null || HeightMap == null)
            {
                currentFrame = animationList[currentAnimation].getCurrentFrame(gameTime);
                ColorData = SetColorData;
                HeightMap = SetHeightMap;
            }

            if (!CurrentFrame.Collidable)
            {
                ghost = true;
            }

            if (!pause)
            {
                currentFrame = animationList[currentAnimation].getCurrentFrame(gameTime);
                //ColorData = SetColorData;
            }
            //if (ColorData == null)
            //{
            //    ColorData = SetColorData;
            //}

            Origin = new Vector2(currentFrame.SourceRectangle.Width * 0.5f, currentFrame.SourceRectangle.Height);

            UpdateSourceRectangle();
            UpdateCollisionRectangle();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (currentFrame == null)
            {
                currentFrame = animationList[currentAnimation].getCurrentFrame(gameTime);
            }
            //try
            //{
                SpriteBatch.Draw(currentFrame.SourceTexture,
                                    position,
                                    currentFrame.SourceRectangle,
                                    Color,
                                    Rotation,
                                    Origin,
                                    Scale,
                                    SpriteEffects.None,
                                    0.0f);
            //}
            //catch (System.NullReferenceException)
            //{
            //}

            if (bool.Parse(game.config.getValue("Debug", "Hitbox")))
            {
                try
                {
                    SpriteBatch.Draw(currentFrame.CollisionTexture,
                                        position,
                                        currentFrame.SourceRectangle,
                                        Color,
                                        Rotation,
                                        Origin,
                                        Scale,
                                        SpriteEffects.None,
                                        0.0f);
                }
                catch (System.NullReferenceException)
                {
                    currentFrame = animationList[currentAnimation].getCurrentFrame(gameTime);
                } 
            }
        }
    }
}
