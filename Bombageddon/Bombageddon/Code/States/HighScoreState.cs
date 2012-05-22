using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bombageddon.Code.Graphics;
using Bombageddon.Code.Input;

namespace Bombageddon.Code.States
{
    class HighScoreState : State
    {
        int latestScore = 0;
        string hiscore = "";
        string congrats = "";
        List<KeyValuePair<int, string>> highScoreList = new List<KeyValuePair<int, string>>(10);

        InputFile scoreFile;

        public override int InputCode
        {
            get
            {
                return inputCode;
            }
            set
            {
                inputCode = value;
                latestScore = inputCode;
                WriteNameToHighScore();
            }
        }

        public HighScoreState(Bombageddon game, String id)
            : base(game, id)
        {
            nextState = "PlayState";
            scoreFile = new InputFile(@"Content\Highscore\highscore.txt");
            scoreFile.parse();
            ReadHighScoreList();
        }

        public override void Terminate()
        {
            changeState = false;
            latestScore = 0;
            hiscore = "";
            congrats = "";
            highScoreList.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();

            if (inputManager.Space)
            {
                game.Exit();
            }

            if (inputManager.Pause || bool.Parse(game.config.getValue("Debug", "Memtest")))
            {
                changeState = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, Resolution.getTransformationMatrix());
            
            spriteBatch.DrawString(font, "Bombageddon: Highscore", new Vector2(100, 100), Color.Red, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, congrats, new Vector2(100, 250), Color.Red, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, hiscore, new Vector2(100, 400), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            spriteBatch.End();
        }

        private void ReadHighScoreList()
        {
            highScoreList.Clear();
            for (int i = 0; i < 10; i++)
            {
                highScoreList.Add(new KeyValuePair<int, string>(
                    int.Parse(scoreFile.getValue("Player" + i.ToString(), "Score")), 
                    scoreFile.getValue("Player" + i.ToString(), "Name")));
            }
            SortHighScore();
        }

        private void WriteHighScoreList()
        {
            for (int i = 0; i < 10; i++)
            {
                scoreFile.addModify("Player" + i.ToString(), "Score", highScoreList[i].Key.ToString());
                scoreFile.addModify("Player" + i.ToString(), "Name", highScoreList[i].Value.ToString());
            }
            scoreFile.save();
        }

        private void SortHighScore()
        {
            hiscore = "";
            highScoreList.Sort((x, y) => y.Key.CompareTo(x.Key));
            for (int i = 0; i < 10; i++)
            {
                hiscore += highScoreList.ElementAt(i).ToString() + "\n";
            }
        }

        private string GetCharacterInput()
        {
            string[] nameAr = new string[3];
            do
            {
                nameAr[0] = "A";
                nameAr[1] = "B";
                nameAr[2] = "C";
            } while (nameAr[2] == "");

            string name = nameAr[0] + nameAr[1] + nameAr[2];

            return name;
        }

        private void WriteNameToHighScore()
        {
            ReadHighScoreList();
            if (latestScore > highScoreList.ElementAt(9).Key)
            {
                congrats = "Congratulations, you made the highscore with your " + latestScore + " points!";
                string name = GetCharacterInput();
                highScoreList.RemoveAt(9);
                highScoreList.Add(new KeyValuePair<int, string>(latestScore, name));
                SortHighScore();
                WriteHighScoreList();
            }
            else
            {
                congrats = "Sorry, you didn't make the highscore with your " + latestScore + " points...";
            }
        }
    }
}
