using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bombageddon.Code.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bombageddon.Code.Graphics;

namespace Bombageddon.Code.States
{
    class NameInputState : State
    {
        Texture2D background;

        LetterInput letters;

        private int choice = 0;

        private char[] nameAr = new char[3] { 'A', 'A', 'A' };

        public string Name
        {
            get;
            private set;
        }

        public NameInputState(Bombageddon game, String id)
            : base(game, id)
        {
            nextState = "HighScoreState";
            letters = new LetterInput();
            background = game.Content.Load<Texture2D>(@"Graphics\Backgrounds\ScoreBackground");
        }

        private void NameInput()
        {
            switch (inputManager.Trackball)
            {
                case Track.Left:
                    if (choice > 0)
                    {
                        letters.PreviousSlot();
                        choice--;
                    }
                    break;
                case Track.Right:
                    if (choice < 2)
                    {
                        letters.NextSlot();
                        choice++;
                    }
                    else
                    {
                        changeState = true;
                        outputCode = inputCode + ":" + nameAr[0] + nameAr[1] + nameAr[2];
                    }
                    break;
                case Track.Up:
                    letters.NextLetter();
                    nameAr[choice] = letters.CurrentLetter;
                    break;
                case Track.Down:
                    letters.PreviousLetter();
                    nameAr[choice] = letters.CurrentLetter;
                    break;
            }
        }

        public override void Terminate()
        {
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();

            NameInput();
        }

        private Color FocusColor(int index)
        {
            if (choice == index)
            {
                return Color.Green;
            }
            return Color.Red;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, Resolution.getTransformationMatrix());

            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);

            spriteBatch.DrawString(font, nameAr[0].ToString(), new Vector2(game.Window.ClientBounds.Center.X - 100, game.Window.ClientBounds.Center.Y), FocusColor(0), 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, nameAr[1].ToString(), new Vector2(game.Window.ClientBounds.Center.X, game.Window.ClientBounds.Center.Y), FocusColor(1), 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, nameAr[2].ToString(), new Vector2(game.Window.ClientBounds.Center.X + 100, game.Window.ClientBounds.Center.Y), FocusColor(2), 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);

            spriteBatch.End();
        }
    }
}
