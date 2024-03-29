﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bombageddon.Code.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Bombageddon.Code.Entities
{
    class Sheeple : Animation
    {
        enum State
        {
            Running,
            Splat,
            Flat,
            Boom
        };

        State currentState;

        public CivilianData data;
        Random random = new Random(DateTime.Now.Millisecond);
        int direction;

        private Vector2 originalPosition;

        public Sheeple(SpriteBatch spriteBatch, Bombageddon game, Vector2 spawnposition, CivilianData data)
            : base(game, spriteBatch)
        {
            position = spawnposition;
            originalPosition = spawnposition;
            if(data.type == "Bird")
            {
                position.Y = random.Next(400, 800);
                effect = SpriteEffects.FlipHorizontally;
            }
            this.data = data;
            base.pointsWorth = data.pointsWorth;
        }

        protected override void LoadContent()
        {
            AnimationStrip _runningAnim = new AnimationStrip();

            for (int x = 0; x < data.panicFramesCount; x++)
            {
                _runningAnim.AddFrame(new AnimationFrame(data.panicSheet, 
                    new Rectangle((data.panicSheet.Width / data.panicFramesCount) * x, 0, 
                        (data.panicSheet.Width / data.panicFramesCount), data.panicSheet.Height), 
                    data.collisionSheet));
            }

            _runningAnim.TimeOnChange = 50;
            _runningAnim.WillLoop = true;
            this.AddAnimation("Running", _runningAnim);
            
            _runningAnim = null;

            AnimationStrip _splatAnim = new AnimationStrip();

            for (int x = 0; x < data.splatDeathFramesCount; x++)
            {
                _splatAnim.AddFrame(new AnimationFrame(
                    data.splatDeathSheet,
                    new Rectangle((data.splatDeathSheet.Width / data.splatDeathFramesCount) * x, 0,
                        (data.splatDeathSheet.Width / data.splatDeathFramesCount),
                        data.splatDeathSheet.Height)));
            }

            _splatAnim.TimeOnChange = 50;
            this.AddAnimation("Splat!", _splatAnim);

            _splatAnim = null;

            AnimationStrip _death1Anim = new AnimationStrip();

            for (int x = 0; x < data.deathFramesCount1; x++)
            {
                _death1Anim.AddFrame(new AnimationFrame(
                    data.randomDeathSheet1,
                    new Rectangle((data.randomDeathSheet1.Width / data.deathFramesCount1) * x, 0,
                        (data.randomDeathSheet1.Width / data.deathFramesCount1),
                        data.randomDeathSheet1.Height)));
            }

            _death1Anim.TimeOnChange = 50;
            this.AddAnimation("Death1", _death1Anim);

            _death1Anim = null;

            AnimationStrip _death2Anim = new AnimationStrip();

            for (int x = 0; x < data.deathFramesCount2; x++)
            {
                _death2Anim.AddFrame(new AnimationFrame(
                    data.randomDeathSheet2,
                    new Rectangle((data.randomDeathSheet2.Width / data.deathFramesCount2) * x, 0,
                        (data.randomDeathSheet2.Width / data.deathFramesCount2),
                        data.randomDeathSheet2.Height)));
            }

            _death2Anim.TimeOnChange = 50;
            this.AddAnimation("Death2", _death2Anim);

            _death2Anim = null;
            
            currentState = State.Running;
            direction = 1;

            base.LoadContent();
        }
        
        public override void Update(GameTime gameTime)
        {
            if (currentState == State.Running)
            {
                if (this.AnimationName != "Running")
                    this.AnimationName = "Running";

                int movement = 50;
                if (data.type == "Bird")
                {
                    movement = 400;
                }

                if (position.X > (originalPosition.X + movement))
                {
                    direction = -1;
                    if (data.type == "Cow" || data.type == "Sheep" || data.type == "Horse")
                    {
                        effect = SpriteEffects.FlipHorizontally;
                    }
                    else if (data.type == "Bird")
                    {
                        effect = SpriteEffects.None;
                    }
                }
                else if (position.X < (originalPosition.X - movement))
                {
                    direction = 1;
                    if (data.type == "Cow" || data.type == "Sheep" || data.type == "Horse")
                    {
                        effect = SpriteEffects.None;
                    }
                    else if (data.type == "Bird")
                    {
                        effect = SpriteEffects.FlipHorizontally;
                    }
                }

                position.X += direction * 2 * (float)random.NextDouble();
            }
            else
            {
                if (currentState == State.Splat && AnimationName != "Splat!")
                    this.AnimationName = "Splat!";
                if (currentState == State.Boom && AnimationName != "Death1")
                    this.AnimationName = "Death1";
                if (currentState == State.Flat && AnimationName != "Death2")
                    this.AnimationName = "Death2";

                //if (CurrentIndex == AnimationFrames)
                //{
                //    pause = true;
                //}
            }

            base.Update(gameTime);
        }

        public void IsKilled(bool topHit)
        {
            if (topHit)
                currentState = State.Splat;
            else
                if (random.Next(0, 100) > 50)
                    currentState = State.Flat;
                else
                    currentState = State.Boom;

            game.AudioManager.PlayEffect(data.type);
            game.AudioManager.PlayEffect("Squish");
        }
    }
}
