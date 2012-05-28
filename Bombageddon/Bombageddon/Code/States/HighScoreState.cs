using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bombageddon.Code.Graphics;
using Bombageddon.Code.Input;
using System.Threading;

namespace Bombageddon.Code.States
{
    class HighScoreState : State
    {
        int latestScore = 0;
        string hiscore = "";
        string congrats = "";
        string name = "";
        List<KeyValuePair<int, string>> highScoreList = new List<KeyValuePair<int, string>>(10);

        Texture2D background;

        InputFile scoreFile;

        bool waited = false;

        public override String InputCode
        {
            get
            {
                return inputCode;
            }
            set
            {
                inputCode = value;
                latestScore = int.Parse(inputCode.Split(':')[0]);
                name = inputCode.Split(':')[1];
                WriteNameToHighScore();
            }
        }

        public HighScoreState(Bombageddon game, String id)
            : base(game, id)
        {
            nextState = "PlayState";
            background = game.Content.Load<Texture2D>(@"Graphics\Backgrounds\ScoreBackground");
            scoreFile = new InputFile(@"Content\Highscore\highscore.txt");
            scoreFile.parse();
            ReadHighScoreList();
        }

        public override void Terminate()
        {
            changeState = false;
            waited = false;
            latestScore = 0;
            hiscore = "";
            congrats = "";
            nextState = "PlayState";
            highScoreList.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();

            if (!waited && hiscore != null && (name != "noname" || name == "youfailed"))
            {
                inputManager.ClearMouse();
                Thread.Sleep(2000);
                waited = true;
            }

            if (inputManager.Exit)
            {
                game.Exit();
            }

            if (inputManager.Escape || inputManager.LongTrackBallSwing || bool.Parse(game.config.getValue("Debug", "Memtest")))
            {
                changeState = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, Resolution.getTransformationMatrix());
            
            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);

            //spriteBatch.DrawString(font, "Bombageddon: Highscore", new Vector2(100, 100), Color.Red, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, congrats, new Vector2(100, 250), Color.SaddleBrown, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, hiscore, new Vector2(100, 400), Color.SaddleBrown, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 1f);

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
            if (name == "noname")
            {
                nextState = "NameInputState";
                outputCode = latestScore.ToString();
                changeState = true;
            }
            return name;
        }

        private void WriteNameToHighScore()
        {
            ReadHighScoreList();
            if (latestScore > highScoreList.ElementAt(9).Key)
            {
                congrats = "You win! " + latestScore + " points gets you into the highscore!";
                string name = GetCharacterInput();
                if (name != "noname")
                {
                    highScoreList.RemoveAt(9);
                    highScoreList.Add(new KeyValuePair<int, string>(latestScore, name));
                    SortHighScore();
                    WriteHighScoreList();
                }
            }
            else
            {
                congrats = "You lose! " + latestScore + " points aren't enough...";
                name = "youfailed";
            }
        }
    }
}
