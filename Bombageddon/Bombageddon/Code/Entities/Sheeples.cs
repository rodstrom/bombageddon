using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bombageddon.Code.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Bombageddon.Code.Entities
{
    class Sheeples : Animation
    {
        private string spriteSheet = "";
        private string collisionSheet = "";

        private Vector2 originalPosition;

        public Sheeples(SpriteBatch spriteBatch, Bombageddon game, string sheet, string collision, Vector2 spawnposition)
            : base(game, spriteBatch)
        {
            spriteSheet = sheet;
            collisionSheet = collision;
            position = spawnposition;
            originalPosition = spawnposition;
        }

        protected override void LoadContent()
        {
            AnimationStrip _runningAnim = new AnimationStrip();
            Texture2D _tmpSource = Game.Content.Load<Texture2D>(spriteSheet);
            Texture2D _tmpCol = Game.Content.Load<Texture2D>(collisionSheet);

            for (int x = 0; x < 14; x++)
            {
                _runningAnim.AddFrame(new AnimationFrame(_tmpSource, new Rectangle(64 * x, 0, 64, 64), _tmpCol));
            }

            _runningAnim.TimeOnChange = 50;
            this.AddAnimation("Running", _runningAnim);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.AnimationName != "Running")
            {
                this.AnimationName = "Running";
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
