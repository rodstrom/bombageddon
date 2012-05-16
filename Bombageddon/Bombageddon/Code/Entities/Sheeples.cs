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
        enum State
        {
            Running,
            Splat,
            Flat,
            Boom
        };

        State currentState;

        CivilianData data;

        Random random = new Random(DateTime.Now.Millisecond);

        private Vector2 originalPosition;

        public Sheeples(SpriteBatch spriteBatch, Bombageddon game, Vector2 spawnposition, CivilianData data)
            : base(game, spriteBatch)
        {
            position = spawnposition;
            originalPosition = spawnposition;
            this.data = data;
        }

        protected override void LoadContent()
        {
            AnimationStrip _runningAnim = new AnimationStrip();

            for (int x = 0; x < data.panicFramesCount; x++)
            {
                _runningAnim.AddFrame(new AnimationFrame(data.panicSheet, new Rectangle((data.panicSheet.Width / data.panicFramesCount) * x, 0, (data.panicSheet.Width / data.panicFramesCount), data.panicSheet.Height), data.collisionSheet));
            }

            _runningAnim.TimeOnChange = 50;
            this.AddAnimation("Running", _runningAnim);

            AnimationStrip _splatAnim = new AnimationStrip();

            for (int x = 0; x < data.splatDeathFramesCount; x++)
            {
                _splatAnim.AddFrame(new AnimationFrame(
                    data.splatDeathSheet,
                    new Rectangle((data.splatDeathSheet.Width / data.splatDeathFramesCount) * x, 0,
                        (data.splatDeathSheet.Width / data.splatDeathFramesCount),
                        data.splatDeathSheet.Height),
                    new Texture2D(GraphicsDevice, data.splatDeathSheet.Width, data.splatDeathSheet.Height)));
            }

            _splatAnim.TimeOnChange = 50;
            this.AddAnimation("Splat!", _splatAnim);
            
            currentState = State.Running;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (currentState == State.Running)
            {
                if (this.AnimationName != "Running")
                    this.AnimationName = "Running";

                position.X = originalPosition.X - random.Next(-7, 7);
            }
            if (currentState == State.Splat)
            {
                if (this.AnimationName != "Splat!")
                    this.AnimationName = "Splat!";
                if(this.AnimationName.
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
