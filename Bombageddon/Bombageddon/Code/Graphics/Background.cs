using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Bombageddon.Code.Graphics
{
    class Background : Sprite
    {
        //string _filename;
        public float velocityX = 0f;
        public bool stuck = false;

        public Background(Texture2D texture, SpriteBatch spriteBatch, Bombageddon game, Vector2 position)
            : base(spriteBatch, game)
        {
            SourceTexture = texture;
            base.position = position;
        }

        public Background(Texture2D texture, SpriteBatch spriteBatch, Bombageddon game, Vector2 position, int velocityX)
            : base(spriteBatch, game)
        {
            SourceTexture = texture;
            this.velocityX = velocityX;
            base.position = position;
        }

        protected override void LoadContent()
        {
            //SourceTexture = game.Content.Load<Texture2D>(_filename);
            Origin = new Vector2(SourceRectangle.Left, SourceRectangle.Bottom);
        }

        public override void Update(GameTime gameTime)
        {
            if (!stuck)
            {
                position.X -= velocityX * game.Camera.Focus.kinetics.X / 100;
            }
            else
            {
                position.X = game.Camera.Focus.position.X - Bombageddon.WIDTH / 4;
            }
            base.Update(gameTime);
        }
    }
}
