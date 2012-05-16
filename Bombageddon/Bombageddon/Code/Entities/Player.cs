using System;
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
    class Player : Sprite
    {
        InputManager input;
        KineticVector kineticVector;

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
            this.input = new InputManager(game);
            kineticVector = new KineticVector();
            this.fallTime = 0f;
            this.position = position;
            kinetics = Vector2.Zero;
            FuseTimer = 60000;
        }

        protected override void LoadContent()
        {
            ////AnimationStrip _runningAnim = new AnimationStrip();            
            //Texture2D _tmpSource = Game.Content.Load<Texture2D>(@"Graphics\Bomb");
            //Texture2D _tmpCol = Game.Content.Load<Texture2D>(@"Graphics\Collision\Bomb_collision");

            SourceTexture = Game.Content.Load<Texture2D>(@"Graphics\Bomb");

            hitTexture = Game.Content.Load<Texture2D>(@"Graphics\Collision\Bomb_collision");
            GetColorData(hitTexture);
            CollisionRectangle = SourceRectangle;
            GetHeight();

            Origin = new Vector2(SourceTexture.Bounds.Center.X, SourceTexture.Bounds.Center.Y);

            //for (int x = 0; x < 14; x++)
            //{
            //    _runningAnim.AddFrame(new AnimationFrame(_tmpSource, new Rectangle(64 * x, 0, 64, 64), _tmpCol));
            //}
            
            //_runningAnim.TimeOnChange = 50;
            //this.AddAnimation("Running", _runningAnim);

            Falling = true;
        }

        public override void Terminate()
        {
            kineticVector.Terminate();
            kinetics = Vector2.Zero;
            base.Terminate();
        }

        public override void Update(GameTime gameTime)
        {
            //if (this.AnimationName != "Running")
            //{
            //    this.AnimationName = "Running";
            //}
            FuseTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (FuseTimer < 0)
            {
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
                        kinetics.Y += kineticVector.FinalVector.Y * 1f;
                        kinetics.X += kineticVector.FinalVector.X * 1f;
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

            if(position.X > furthestPointReached)
            {
                furthestPointReached = (int)position.X;
            }
            else if (position.X < furthestPointReached - Bombageddon.WIDTH / 2)
            {
                position.X = furthestPointReached - Bombageddon.WIDTH / 2;
                kinetics = Vector2.Zero;
                //play sound too perhaps?
            }
        }
    }
}
