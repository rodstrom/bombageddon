using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombageddon.Code.Graphics
{
    class AnimationFrame
    {
        public Texture2D SourceTexture
        {
            get;
            set;
        }

        public Rectangle SourceRectangle
        {
            get;
            set;
        }

        public Texture2D CollisionTexture
        {
            get;
            set;
        }

        public Color[,] ColorData
        {
            get;
            private set;
        }

        private void SetColorData(Texture2D texture)
        {
            Color[] ColorArray1D = new Color[texture.Width * texture.Height];
            texture.GetData(ColorArray1D);

            Color[,] ColorArray2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    ColorArray2D[x, y] = ColorArray1D[x + y * texture.Width];

            ColorData = ColorArray2D;
        }

        private void SetHeight()
        {
            for (int x = 0; x < SourceRectangle.Width; x++)
            {

            }
        }

        public AnimationFrame(Texture2D tex, Rectangle rect, Texture2D colTex)
        {
            SourceTexture = tex;
            SourceRectangle = rect;
            CollisionTexture = colTex;
            SetColorData(CollisionTexture);
        }
    }
}
