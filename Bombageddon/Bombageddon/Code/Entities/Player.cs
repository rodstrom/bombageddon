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

        public Vector2 kinetics;

        public float fallTime;
        public float runTime;

        float snapShotTimer = 0.0f;
        int snapShotIndex = 0;

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
                if (!Falling)
                {
                    fallTime = 0;
                    kinetics.Y = 0;
                }
            }
        }

        public bool win = false;
        public bool lose = false;

        public Player(Bombageddon game, SpriteBatch spriteBatch, Vector2 position)
            : base(spriteBatch, game)
        {
            this.input = new InputManager(game);
            kineticVector = new KineticVector();
            this.fallTime = 0f;
            this.runTime = 0f;
            this.position = position;
            kinetics = Vector2.Zero;
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
        }

        public override void Terminate()
        {
            kineticVector.Terminate();
            base.Terminate();
        }

        public override void Update(GameTime gameTime)
        {
            //if (this.AnimationName != "Running")
            //{
            //    this.AnimationName = "Running";
            //}

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
                        kinetics.Y -= kineticVector.FinalVector.Y * 2;
                        kinetics.X -= kineticVector.FinalVector.X * 1;
                        snapShotIndex = 0;
                        Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
                        input.CurrentMouse = input.MouseOriginal;
                    }
                }
            }

            runTime += gameTime.ElapsedGameTime.Milliseconds;

            //MathHelper.Clamp(kinetics.X, -100, 50);
            //MathHelper.Clamp(kinetics.Y, -500, 50);
            MathHelper.Clamp(runTime, 0, 5000);
            MathHelper.Clamp(fallTime, 0, 5000);

            if (kinetics.X < 300f)
            {
                kinetics.X += ((runTime / 1000) * (runTime / 1000) * 2);
            }
            if (kinetics.X < 900f)
            {
                kinetics.X += ((runTime / 1000) * (runTime /  1000));
            }
            if (kinetics.X > 1050f)
            {
                kinetics.X -= ((runTime / 1000) * (runTime / 1000) * 0.5f);
            }

            if (Falling)
            {
                fallTime += gameTime.ElapsedGameTime.Milliseconds;
                kinetics.Y += ((fallTime / 1000) * (fallTime / 1000) * 500);
            }

            //if (input.MouseRelative.Y < 0 && !falling)
            //{
            //    position.Y -= 10;

            //    //Runner.AudioManager.PlayEffect("Jump");
            //}
            
            input.Update();
            Move(gameTime);
            base.Update(gameTime);
        }

        private void Move(GameTime gameTime)
        {
            position.X += (kinetics.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
            position.Y += (kinetics.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
            Rotation += 0.1f;

            if (position.Y > Bombageddon.HEIGHT - SourceRectangle.Center.X)
            {
                Falling = false;
            }
            else
            {
                Falling = true;
            }
        }
    }
}
