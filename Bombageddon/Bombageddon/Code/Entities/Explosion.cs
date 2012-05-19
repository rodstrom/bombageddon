using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bombageddon.Code.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Bombageddon.Code.Entities
{
    class Explosion : Animation
    {
        public Boolean killEverything = false;
        public Boolean endGame = false;

        public Explosion(Bombageddon game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            Texture2D Texture = game.Content.Load<Texture2D>(@"Graphics\Explosion\BOOM_sheet2");
            AnimationStrip explosion = new AnimationStrip();
            int height = Texture.Height / 2;
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 13; x++)
                {
                    explosion.AddFrame(new AnimationFrame(Texture, new Rectangle(height * x, height * y, height, height)));
                }
            }
            explosion.TimeOnChange = 100;
            this.AddAnimation("Explosion", explosion);
            this.AnimationName = "Explosion";
            position = game.Camera.Focus.position;
            position.Y += 224f;
            Scale *= 4;

            game.AudioManager.LoadNewEffect("Explosion", @"Audio\Sound\Explosion\Explosion2");
            game.AudioManager.PlayEffect("Explosion");
            game.AudioManager.StopMusic();
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentIndex == 4)
            {
                killEverything = true;
            } 
            if (CurrentIndex == 24)
            {
                endGame = true;
            }

            base.Update(gameTime);
        }
    }
}
