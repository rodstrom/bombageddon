﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bombageddon.Code.Graphics;
using Bombageddon.Code.Entities;
using Bombageddon.Code.Time;
using Bombageddon.Code.Input;
using Bombageddon.Code.Camera;
using Microsoft.Xna.Framework.Input;

namespace Bombageddon.Code.States
{
    class PlayState : State
    {
        EntityManager entityManager;
        Fuse fuse;

        public EntityManager EntityManager
        {
            get
            {
                return entityManager;
            }
        }
        
        //Texture2D background;

        Vector2 guiPosition;

        int score = 0;

        bool pause = false;
        int pauseSelectedItem = 0;

        public PlayState(Bombageddon game, String id)
            : base(game, id)
        {
            entityManager = new EntityManager(game, spriteBatch);
            entityManager.Initialize();
            game.Camera.Focus = entityManager.player;

            nextState = "HighScoreState";

            game.Timer.StartTimer();

            fuse = new Fuse(game, spriteBatch);
            fuse.Initialize();

            game.AudioManager.SetMusic("Background");
            game.AudioManager.PlayMusic();
        }

        public override void Terminate()
        {
            fuse.Terminate();
            entityManager.Terminate();
            game.Timer.ResetTimer();
            score = 0;
            changeState = false;
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();

            if (inputManager.Exit)
            {
                game.Exit();
            }

            game.Camera.Update(gameTime);
            guiPosition.X = game.Camera.Position.X - game.Camera.Center.X + 20;
            guiPosition.Y = game.Camera.Position.Y - game.Camera.Center.Y + 10;

            if (inputManager.Escape)
            {
                pause = !pause;
            }

            KeyboardState key = Keyboard.GetState();

            if (key.IsKeyDown(Keys.Left))
            {
                game.Camera.Rotation -= 0.01f;
            }
            if (key.IsKeyDown(Keys.Right))
            {
                game.Camera.Rotation += 0.01f;
            }
            if (key.IsKeyDown(Keys.Up))
            {
                game.Camera.Zoom += 0.01f;
            }
            if (key.IsKeyDown(Keys.Down))
            {
                game.Camera.Zoom -= 0.01f;
            }

            //if (entityManager.player.win)
            //{
            //    outputCode = score;
            //    changeState = true;
            //}
            else if (entityManager.blazeOfGloryDone)
            {
                outputCode = score.ToString() + ":noname";
                changeState = true;
            }

            if (!pause)
            {
                game.Timer.Update(gameTime);
                //score = game.Timer.ToInteger("s_total");
                //score = (int)(entityManager.player.position.X - 300f) / 100;
                score = entityManager.player.points;
                entityManager.Update(gameTime);
            }
            else
            {
                SelectionScreen();
            }

            fuse.Update(gameTime);
        }

        private void SelectionScreen()
        {
            //if (inputManager.LeftOnce || inputManager.RightOnce)
            //{
            //    if (pauseSelectedItem == 0)
            //    {
            //        pauseSelectedItem = 1;
            //    }
            //    else
            //    {
            //        pauseSelectedItem = 0;
            //    }
            //}
            //if (inputManager.Select)
            //{
            //    if (pauseSelectedItem == 0)
            //    {
            //        pause = false;
            //    }
            //    else
            //    {
            //        changeState = true;
            //    }
            //}
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, game.Camera.Transform);

            entityManager.Draw(gameTime);
            fuse.Draw(gameTime);

            if (pause)
            {
                drawPauseDialog();
            }

            spriteBatch.DrawString(font, score.ToString(), guiPosition, Color.SaddleBrown, 0f, Vector2.Zero, (0.5f / game.Camera.Zoom) * 1f, SpriteEffects.None, 1f);

            if(bool.Parse(game.config.getValue("Debug", "ExtendedGUI")))
            {
                spriteBatch.DrawString(font, "Kinetic X: " + entityManager.player.kinetics.X.ToString() + System.Environment.NewLine + "Kinetic Y: " + entityManager.player.kinetics.Y.ToString() + System.Environment.NewLine + "Hit multiplier: " + entityManager.playerKineticMultiplier.ToString(), new Vector2(game.Camera.Position.X - 300, game.Camera.Position.Y + 500), Color.Chocolate);
            }
            spriteBatch.End();
        }

        private void drawPauseDialog()
        {
            Texture2D dialog = new Texture2D(game.graphics.GraphicsDevice, 500, 220); 
            Color[] data = new Color[500 * 220];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Black;
            dialog.SetData(data);
            Vector2 pos = new Vector2(game.Camera.Position.X - 250, game.Camera.Position.Y - 110);
            spriteBatch.Draw(dialog, pos, Color.White);

            Texture2D selection = new Texture2D(game.graphics.GraphicsDevice, 200, 80);
            data = new Color[200 * 80];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Gray;
            selection.SetData(data);
            pos = new Vector2(game.Camera.Position.X + (-240 + (240 * pauseSelectedItem)), game.Camera.Position.Y + 5);
            spriteBatch.Draw(selection, pos, Color.White);

            String dialogText = "Giving up?";
            Vector2 origin = font.MeasureString(dialogText);
            origin.X /= 2;
            pos = new Vector2(game.Camera.Position.X, game.Camera.Position.Y - 20);
            spriteBatch.DrawString(font, dialogText, pos, Color.White, 0f, origin, 1f, SpriteEffects.None, 1f);

            dialogText = "Fuck no!";
            origin = font.MeasureString(dialogText) / 2;
            pos = new Vector2(game.Camera.Position.X - 135, game.Camera.Position.Y + 45);
            spriteBatch.DrawString(font, dialogText, pos, Color.White, 0f, origin, 1f, SpriteEffects.None, 1f);

            dialogText = "Yeah...";
            origin = font.MeasureString(dialogText) / 2;
            pos = new Vector2(game.Camera.Position.X + 135, game.Camera.Position.Y + 45);
            spriteBatch.DrawString(font, dialogText, pos, Color.White, 0f, origin, 1f, SpriteEffects.None, 1f);
        }
    }
}
