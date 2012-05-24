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

        private string explanatoryText =    "Type your name by rolling the ball up or down\n" + 
                                            "Select a character by rolling right\n" + 
                                            "Go back by rolling left\n" + 
                                            "When you are done, roll right once more";
        private string congrats;

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
            congrats = "You win! " + inputCode + " points gets you into the highscore!";
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
                return Color.SaddleBrown;
            }
            return Color.Chocolate;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, Resolution.getTransformationMatrix());

            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);

            spriteBatch.DrawString(font, congrats, new Vector2(Bombageddon.WIDTH / 2, Bombageddon.HEIGHT / 4), Color.SaddleBrown, 0f, font.MeasureString(congrats) * 0.5f, 0.5f, SpriteEffects.None, 1f);
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.DrawString(font, nameAr[i].ToString(), new Vector2(Bombageddon.WIDTH / 2 - 100 + i * 100, Bombageddon.HEIGHT / 2), FocusColor(i), 0f, font.MeasureString(nameAr[i].ToString()) * 0.5f, 1f, SpriteEffects.None, 1f);
            }
            spriteBatch.DrawString(font, explanatoryText, new Vector2(Bombageddon.WIDTH / 2, Bombageddon.HEIGHT * 0.75f), Color.SaddleBrown, 0f, font.MeasureString(explanatoryText) * 0.5f, 0.3f, SpriteEffects.None, 1f);
            

            //spriteBatch.DrawString(font, nameAr[0].ToString(), new Vector2(game.Window.ClientBounds.Center.X - 100, game.Window.ClientBounds.Center.Y), FocusColor(0), 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            //spriteBatch.DrawString(font, nameAr[1].ToString(), new Vector2(game.Window.ClientBounds.Center.X, game.Window.ClientBounds.Center.Y), FocusColor(1), 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            //spriteBatch.DrawString(font, nameAr[2].ToString(), new Vector2(game.Window.ClientBounds.Center.X + 100, game.Window.ClientBounds.Center.Y), FocusColor(2), 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);

            spriteBatch.End();
        }
    }
}
