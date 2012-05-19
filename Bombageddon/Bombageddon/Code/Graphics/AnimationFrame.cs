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
        public void Terminate()
        {
            //SourceTexture.Dispose();
            SourceTexture = null;
        }

        public Texture2D SourceTexture
        {
            get;
            private set;
        }

        public Rectangle SourceRectangle
        {
            get;
            private set;
        }

        public Texture2D CollisionTexture
        {
            get;
            private set;
        }

        public bool Collidable
        {
            get;
            private set;
        }

        public Color[,] ColorData
        {
            get;
            private set;
        }

        public int[] HeightMap
        {
            get;
            private set;
        }

        private void SetColorData(Texture2D texture)
        {
            if (!texture.IsDisposed)
            {
                Color[] ColorArray1D = new Color[texture.Width * texture.Height];
                texture.GetData(ColorArray1D);

                Color[,] ColorArray2D = new Color[SourceRectangle.Width, SourceRectangle.Height];
                for (int x = 0; x < SourceRectangle.Width; x++)
                {
                    for (int y = 0; y < SourceRectangle.Height; y++)
                    {
                        ColorArray2D[x, y] = ColorArray1D[(x + y * SourceRectangle.Width) + SourceRectangle.Location.X];
                    }
                }

                ColorData = ColorArray2D;

                ColorArray1D = null;
                ColorArray2D = null;
            }
        }

        private void SetHeight()
        {
            int[] height = new int[SourceRectangle.Width];

            for (int x = 0; x < SourceRectangle.Width; x++)
            {
                for (int y = 0; y < SourceRectangle.Height; y++)
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

        public AnimationFrame(Texture2D tex, Rectangle rect, Texture2D colTex)
        {
            SourceTexture = tex;
            SourceRectangle = rect;
            CollisionTexture = colTex;
            SetColorData(colTex);
            SetHeight();
            Collidable = true;
        }

        public AnimationFrame(Texture2D tex, Rectangle rect)
        {
            SourceTexture = tex;
            SourceRectangle = rect;
            Collidable = false;
        }
    }
}
