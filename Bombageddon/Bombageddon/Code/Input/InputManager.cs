using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Bombageddon.Code.Physics;

namespace Bombageddon.Code.Input
{
    public enum Track
    {
        Left = -2,
        Up = -1,
        None = 0,
        Down = 1,
        Right = 2
    };

    class InputManager
    {
        Bombageddon Game;
        
        KeyboardState lastKey;
        KeyboardState currentKey;

        public float inputLock = 0.0f;

        MouseState currentMouseState, lastMouseState, originalMouseState;

        Vector2 mouseAbsolute = Vector2.Zero;
        Vector2 mouseRelative = Vector2.Zero;

        //Keys up;
        //Keys right;
        //Keys left;

        //Keys select;
        Keys escape = Keys.Escape;
        //Keys space = Keys.Space;

        int wait = 0;

        public Vector2 MouseAbsolute
        {
            get { return mouseAbsolute; }
        }

        public Vector2 MouseRelative
        {
            get { return mouseRelative; }
        }

        public MouseState MouseOriginal
        {
            get { return originalMouseState; }
        }

        public MouseState CurrentMouse
        {
            get { return currentMouseState; }
            set { currentMouseState = value; }
        }

        public bool LeftButton
        {
            get
            {
                if(currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    return true;
                }
                return false;
            }
        }

        public InputManager(Bombageddon game)
        {
            Game = game; 
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            originalMouseState = Mouse.GetState();
            //up = setKey(game.config.getValue("Controls", "Up"));
            //right = setKey(game.config.getValue("Controls", "Right"));
            //left = setKey(game.config.getValue("Controls", "Left"));
            //select = setKey(game.config.getValue("Controls", "Select"));
            //pause = setKey(game.config.getValue("Controls", "Pause"));
            //space = setKey(game.config.getValue("Controls", "Space"));
        }

        //private Keys setKey(String newKey)
        //{
        //    try
        //    {
        //        return (Keys)Enum.Parse(typeof(Keys), newKey);
        //    }
        //    catch (Exception)
        //    {
        //        return Keys.F24;
        //    }
        //}

        public void Update()
        {
            lastKey = currentKey;
            currentKey = Keyboard.GetState();

            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }
        
        //public bool Up
        //{
        //    get
        //    {
        //        if (currentKey.IsKeyDown(up))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}


        //public bool Right
        //{
        //    get
        //    {
        //        if (currentKey.IsKeyDown(right))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}

        //public bool RightOnce
        //{
        //    get
        //    {
        //        if (currentKey.IsKeyUp(right) && lastKey.IsKeyDown(right))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}

        //public bool Left
        //{
        //    get
        //    {
        //        if (currentKey.IsKeyDown(left))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}

        //public bool LeftOnce
        //{
        //    get
        //    {
        //        if (currentKey.IsKeyUp(left) && lastKey.IsKeyDown(left))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}

        //public bool Select
        //{
        //    get
        //    {
        //        if (currentKey.IsKeyUp(select) && lastKey.IsKeyDown(select))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}

        public bool Exit
        {
            get
            {
                //if (currentKey.IsKeyUp(pause) && lastKey.IsKeyDown(pause))
                //{
                //    return true;
                //}
                if (LeftButton)
                {
                    return true;
                }
                return false;
            }
        }

        public bool Escape
        {
            get
            {
                if (currentKey.IsKeyDown(escape) && lastKey.IsKeyUp(escape))
                {
                    return true;
                }
                return false;
            }
        }

        public bool LongTrackBallSwing
        {
            get
            {
                if (LooseRulesTrackball != Track.None)
                {
                    if (Math.Abs(currentMouseState.X - lastMouseState.X) > 20 || Math.Abs(currentMouseState.Y - lastMouseState.Y) > 20)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public Track LooseRulesTrackball
        {
            get
            {
                Vector2 diff = new Vector2(lastMouseState.Y - currentMouseState.Y, lastMouseState.X - currentMouseState.X);
                diff.Normalize();
                float R = MathHelper.ToDegrees((float)Math.Atan2(-diff.X, diff.Y));

                if (R < 45 && R > -45)
                    return Track.Left;
                if (R < 135 && R > 45)
                    return Track.Down;
                if (R > -225 && R < -135)
                    return Track.Right;
                if (R > -135 && R < -45)
                    return Track.Up;

                return Track.None;
            }
        }

        public Track Trackball
        {
            get
            {
                wait++;

                if (wait > 10)
                {
                    Vector2 diff = new Vector2(lastMouseState.Y - currentMouseState.Y, lastMouseState.X - currentMouseState.X);
                    diff.Normalize();
                    float R = MathHelper.ToDegrees((float)Math.Atan2(-diff.X, diff.Y));


                    if (R < 10 && R > -10)
                    {
                        Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
                        wait = 0;
                        return Track.Left;
                    }
                    if (R < 100 && R > 80)
                    {
                        Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
                        wait = 0;
                        return Track.Down;
                    }
                    if (R > -190 && R < -170)
                    {
                        Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
                        wait = 0;
                        return Track.Right;
                    }
                    if (R > -100 && R < -80)
                    {
                        Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
                        wait = 0;
                        return Track.Up;
                    }
                }

                return Track.None;
            }
        }
    }
}
