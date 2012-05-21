using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bombageddon.Code.Camera;
using Bombageddon.Code.Entities;

namespace Bombageddon.Code.Graphics
{
    public class Sprite : Entity
    {
        protected Texture2D sourceTexture;
        protected Texture2D hitTexture;

        public override void Terminate()
        {
            //sourceTexture.Dispose();
            //if (hitTexture != null)
            //{
            //    hitTexture.Dispose();
            //}

            sourceTexture = null;
            hitTexture = null;
            ColorData = null;
            base.Terminate();
        }

        public Texture2D HitTexture
        {
            get { return hitTexture; }
        }

        public Texture2D SourceTexture
        {
            get { return sourceTexture; }

            set
            {
                sourceTexture = value;

                if (sourceTexture != null)
                {
                    SourceRectangle = new Rectangle(0, 0, value.Width, value.Height);
                }
            }
        }

        protected void GetColorData(Texture2D texture)
        {
            Color[] ColorArray1D = new Color[texture.Width * texture.Height];
            texture.GetData(ColorArray1D);

            Color[,] ColorArray2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    ColorArray2D[x, y] = ColorArray1D[x + y * texture.Width];

            ColorData = ColorArray2D;
        }

        protected void GetHeight()
        {
            int[] height = new int[sourceTexture.Width];

            for (int x = 0; x < SourceTexture.Width; x++)
            {
                for (int y = 0; y < SourceTexture.Height; y++)
                {
                    if (ColorData[x, y].R > 200)
                    {
                        height[x] = y;
                        break;
                    }
                }
            }

            HeightMap = height;
        }

        public Sprite(SpriteBatch spriteBatch, Bombageddon game)
            : base(game, spriteBatch)
        {
            sourceTexture = null;
        }

        private void UpdateSourceRectangle()
        {
            int x = (int)(position.X - Origin.X);
            int y = (int)(position.Y - Origin.Y);
            SourceRectangle = new Rectangle(x, y, SourceTexture.Width, SourceTexture.Height);
        }

        private void UpdateCollisionRectangle()
        {
            collisionRectangle = sourceRectangle;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateSourceRectangle();
            UpdateCollisionRectangle();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!SourceTexture.IsDisposed)
            {
                SpriteBatch.Draw(SourceTexture, position, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0.0f);
            }

            if (hitTexture != null && !hitTexture.IsDisposed && bool.Parse(game.config.getValue("Debug", "Hitbox")))
            {
                SpriteBatch.Draw(hitTexture, position, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0.0f);
            }
        }
    }
}
