﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Bombageddon.Code.Input;
using Bombageddon.Code.Graphics;
using Bombageddon.Code.Physics;
using Microsoft.Xna.Framework.Input;

namespace Bombageddon.Code.Entities
{
    public class Player : Sprite
    {
        InputManager input;
        KineticVector kineticVector;

        Dictionary<int, Texture2D> bloodCover = new Dictionary<int, Texture2D>();

        public int StartTime;

        public int FuseTimer
        {
            get;
            set;
        }

        public Vector2 kinetics;

        public float fallTime;

        float snapShotTimer = 0.0f;
        int snapShotIndex = 0;

        public int points = 0;

        private int currentBloodSpatter = 0;
        private int pointsSinceUpgrade = 0;

        private bool _falling;
        public bool Falling
        {
            get
            {
                return _falling;
            }
            set
            {
                _falling = value;
                if (!_falling)
                {
                    fallTime = 0;
                    kinetics.Y = 0;
                }
            }
        }

        public bool end = false;

        private int furthestPointReached = 0;

        public Player(Bombageddon game, SpriteBatch spriteBatch, Vector2 position)
            : base(spriteBatch, game)
        {
            StartTime = int.Parse(game.config.getValue("Debug", "LevelTime")) * 1000;
            this.input = new InputManager(game);
            kineticVector = new KineticVector();
            this.fallTime = 0f;
            this.position = position;
            kinetics = Vector2.Zero;
            FuseTimer = StartTime;
        }

        protected override void LoadContent()
        {
            bloodCover.Add(0, game.Content.Load<Texture2D>(@"Graphics\BombBlood\1"));
            bloodCover.Add(1, game.Content.Load<Texture2D>(@"Graphics\BombBlood\2"));
            bloodCover.Add(2, game.Content.Load<Texture2D>(@"Graphics\BombBlood\3"));
            bloodCover.Add(3, game.Content.Load<Texture2D>(@"Graphics\BombBlood\4"));
            bloodCover.Add(4, game.Content.Load<Texture2D>(@"Graphics\BombBlood\5"));
            bloodCover.Add(5, game.Content.Load<Texture2D>(@"Graphics\BombBlood\6"));

            SourceTexture = bloodCover[0];
            //SourceTexture = Game.Content.Load<Texture2D>(@"Graphics\Bomb");
            hitTexture = game.Content.Load<Texture2D>(@"Graphics\Collision\Bomb_collision");
            GetColorData(hitTexture);
            CollisionRectangle = SourceRectangle;
            GetHeight();

            Origin = new Vector2(SourceTexture.Bounds.Center.X, SourceTexture.Bounds.Center.Y);

            Falling = true;
        }

        public override void Terminate()
        {
            //foreach (Texture2D b in bloodCover.Values)
            //{
            //    //b.Dispose();
            //    b = null;
            //}
            bloodCover.Clear();
            kineticVector.Terminate();
            kinetics = Vector2.Zero;
            base.Terminate();
        }

        public override void Update(GameTime gameTime)
        {
            if (currentBloodSpatter < 6 && points - pointsSinceUpgrade > 300)
            {
                SourceTexture = bloodCover[currentBloodSpatter++];
                pointsSinceUpgrade = points;
            }

            FuseTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (FuseTimer < 0)
            {
                FuseTimer = -1;
                if(position.Y > Bombageddon.GROUND - 100)
                    end = true;
            }

            if (input.CurrentMouse != input.MouseOriginal)
            {
                if (snapShotTimer > 0)
                {
                    snapShotTimer -= gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    if (!kineticVector.ReceivedInput(snapShotIndex, input.CurrentMouse))
                    {
                        snapShotTimer = 25;
                        snapShotIndex++;
                    }
                    else
                    {
                        float multiplier = 1f + points * 0.0001f;
                        MathHelper.Clamp(multiplier, 1f, 2f);
                        kinetics.Y += kineticVector.FinalVector.Y * float.Parse(game.config.getValue("Debug", "KineticMultiplier")) * multiplier;
                        kinetics.X += kineticVector.FinalVector.X * float.Parse(game.config.getValue("Debug", "KineticMultiplier")) * multiplier;
                        snapShotIndex = 0;
                        Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
                        input.CurrentMouse = input.MouseOriginal;
                    }
                }
            }

            MathHelper.Clamp(fallTime, 0, 5000);
            
            if (Falling)
            {
                fallTime += gameTime.ElapsedGameTime.Milliseconds;
                kinetics.Y += ((fallTime / 1000) * (fallTime / 1000) * 300);
            }

            //if (input.MouseRelative.Y < 0 && !falling)
            //{
            //    position.Y -= 10;

            //    //Runner.AudioManager.PlayEffect("Jump");
            //}

            //MathHelper.Clamp(kinetics.X, -200, 100);
            //MathHelper.Clamp(kinetics.Y, -200, 100);

            kinetics *= 0.99f;

            input.Update();
            Move(gameTime);
            base.Update(gameTime);
        }

        private void Move(GameTime gameTime)
        {
            position.X += (kinetics.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
            position.Y += (kinetics.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
            Rotation += (kinetics.X * (float)gameTime.ElapsedGameTime.TotalSeconds) / 100;

            if (bool.Parse(game.config.getValue("Debug", "Memtest")))
            {
                position.X += 10;
            }
            if(position.X > furthestPointReached)
            {
                furthestPointReached = (int)position.X;
            }
            else if (position.X < furthestPointReached - Bombageddon.WIDTH / 2)
            {
                position.X = furthestPointReached - Bombageddon.WIDTH / 2 + 5;
                kinetics = Vector2.Zero;
                //play sound too perhaps?
            }
        }
    }
}
