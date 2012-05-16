using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Bombageddon.Code.Graphics
{
    class FlyingPoint:Sprite
    {
        float alpha = 1f;

        public FlyingPoint(SpriteBatch spriteBatch, Bombageddon game, Texture2D texture, Vector2 position)
            : base(spriteBatch, game)
        {
            base.SourceTexture = texture;
            base.position = position;
        }

        public override void Update(GameTime gameTime)
        {
            position.Y -= 3f;
            alpha -= 0.01f;
            Scale *= 1.01f;

            if (alpha < 0f)
            {
                KillMe = true;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(SourceTexture, position, null, new Color(1f, 1f, 1f, alpha), Rotation, Origin, Scale, SpriteEffects.None, 1.0f);
        }
    }
}
