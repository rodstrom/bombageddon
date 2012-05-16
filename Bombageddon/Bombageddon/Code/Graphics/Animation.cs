﻿using System;
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

        AnimationFrame currentFrame = null;

        public override void Terminate()
        {
            base.Terminate();
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
        }

        public AnimationFrame CurrentFrame
        {
            get { return currentFrame; }
        }

        public Color[,] SetColorData
        {
            get { return currentFrame.ColorData; }
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
            currentFrame = animationList[currentAnimation].getCurrentFrame(gameTime);

            Origin = new Vector2(currentFrame.SourceRectangle.Width * 0.5f, currentFrame.SourceRectangle.Height);

            UpdateSourceRectangle();
            UpdateCollisionRectangle();
            ColorData = SetColorData;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            try
            {
                SpriteBatch.Draw(currentFrame.SourceTexture,
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
