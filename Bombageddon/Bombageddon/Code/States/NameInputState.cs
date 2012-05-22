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
        LetterInput letters;

        private bool finished = false;
        private int choice = 0;

        private char[] nameAr = new char[3] { '_', '_', '_' };

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
        }

        private void NameInput()
        {
            switch (inputManager.Trackball)
            {
                case Track.Left:
                    if (choice > 0)
                        choice--;
                    break;
                case Track.Right:
                    if (choice < 2)
                        choice++;
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

            spriteBatch.DrawString(font, nameAr[0].ToString(), new Vector2(100, 100), FocusColor(0), 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, nameAr[1].ToString(), new Vector2(200, 100), FocusColor(1), 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, nameAr[2].ToString(), new Vector2(300, 100), FocusColor(2), 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);

            spriteBatch.End();
        }
    }
}
