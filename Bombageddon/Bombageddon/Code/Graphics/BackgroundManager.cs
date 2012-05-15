using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombageddon.Code.Graphics
{
    class BackgroundManager
    {
        private SpriteBatch spriteBatch;
        private Bombageddon game;

        private Random rand;

        private LinkedList<KeyValuePair<int, Background>> backgroundList = new LinkedList<KeyValuePair<int, Background>>();
        private List<KeyValuePair<int, string>> backgroundFilenames = new List<KeyValuePair<int, string>>();

        private enum Layers
        {
            SKY = 0,
            MAIN,
            SKYLINE,
            BUILDINGS,
            CLOUDS,
            FADER
        }

        public BackgroundManager(Bombageddon game, SpriteBatch spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
        }

        public void Initialize()
        {
            backgroundFilenames.Add(new KeyValuePair<int, string>((int)Layers.MAIN, @"Graphics\Backgrounds\Main"));

            KeyValuePair<int, Background> background = new KeyValuePair<int, Background>((int)Layers.MAIN, 
                new Background(@"Graphics\Backgrounds\Main", spriteBatch, game, new Vector2(-300f, Bombageddon.HEIGHT)));
            background.Value.Initialize();
            backgroundList.AddLast(background);
            backgroundList.AddLast(addBackground((int)Layers.MAIN));
            backgroundList.AddLast(addBackground((int)Layers.MAIN));
            ////background = new KeyValuePair<int, Background>((int)Layers.MOON, 
            ////    new Background(@"Graphics\Backgrounds\Moon", spriteBatch, game, new Vector2(-300f, Runner.HEIGHT)));
            ////background.Value.Initialize();
            ////backgroundList.AddLast(background);
            //background = new KeyValuePair<int, Background>((int)Layers.SKYLINE, 
            //    new Background(@"Graphics\Backgrounds\Skyline1", spriteBatch, game, new Vector2(-300f, Bombageddon.HEIGHT)));
            //background.Value.Initialize();
            //backgroundList.AddLast(background);
            //backgroundList.AddLast(addBackground((int)Layers.SKYLINE));
            //backgroundList.AddLast(addBackground((int)Layers.SKYLINE));
            //background = new KeyValuePair<int, Background>((int)Layers.BUILDINGS, 
            //    new Background(@"Graphics\Backgrounds\hus1", spriteBatch, game, new Vector2(600f, Bombageddon.HEIGHT)));
            //background.Value.Initialize();
            //backgroundList.AddLast(background);
            //backgroundList.AddLast(addBackground((int)Layers.BUILDINGS));
            //backgroundList.AddLast(addBackground((int)Layers.BUILDINGS));
            //background = new KeyValuePair<int, Background>((int)Layers.CLOUDS,
            //    new Background(@"Graphics\Backgrounds\Clouds\9", spriteBatch, game, new Vector2(300f, Bombageddon.HEIGHT / 2)));
            //background.Value.Initialize();
            //backgroundList.AddLast(background);
            //backgroundList.AddLast(addBackground((int)Layers.CLOUDS));
            //backgroundList.AddLast(addBackground((int)Layers.CLOUDS));

            rand = new Random();
        }

        private List<Background> getBackgroundsByLayer(int requestedLayer)
        {
            //IEnumerable<Background> backgrounds =
            //    from layer in backgroundList.ToArray()
            //    where layer.Key > requestedLayer
            //    select layer.Value;

            List<Background> bgList = new List<Background>();

            foreach (KeyValuePair<int, Background> b in backgroundList)
            {
                if (b.Key == requestedLayer)
                {
                    bgList.Add(b.Value);
                }
            }

            return bgList;
        }

        public void Update(GameTime gameTime, int playerPosX)
        {
            List<KeyValuePair<int, Background>> removeBackgrounds = new List<KeyValuePair<int, Background>>();
            foreach (KeyValuePair<int, Background> background in backgroundList)
            {
                background.Value.Update(gameTime);
                if (background.Value.KillMe)
                {
                    removeBackgrounds.Add(background);
                }
            }

            foreach (KeyValuePair<int, Background> b in removeBackgrounds)
            {
                backgroundList.Remove(b);
            }

            refreshBackgrounds(playerPosX);
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < 6; i++)
            {
                foreach (Background background in getBackgroundsByLayer(i))
                {
                    background.Draw(gameTime);
                }
            }
        }
        
        private void refreshBackgrounds(int playerPosX)
        {
            List<int> newBackgrounds = new List<int>();
            foreach (KeyValuePair<int, Background> background in backgroundList)
            {
                if (background.Value.SourceRectangle.Right < playerPosX - Bombageddon.WIDTH / 2)
                {
                    background.Value.KillMe = true;
                    newBackgrounds.Add(background.Key);
                }
            }
            foreach (int i in newBackgrounds)
            {
                backgroundList.AddLast(addBackground(i));
            }
        }

        private KeyValuePair<int, Background> addBackground(int layer)
        {
            int offset = 0;
            if (layer == (int)Layers.BUILDINGS || layer == (int)Layers.CLOUDS)
            {
                offset = rand.Next(100, 600);
            }
            Vector2 position = new Vector2(getBackgroundsByLayer(layer).Last().position.X + getBackgroundsByLayer(layer).Last().SourceRectangle.Width + offset, Bombageddon.HEIGHT);

            string filename = "";
            bool keepTrying = true;
            do{
                rand = new Random();
                int randomFilename = rand.Next(backgroundFilenames.Count);
                filename = backgroundFilenames.ElementAt(randomFilename).Value;
                if (backgroundFilenames.ElementAt(randomFilename).Key == layer)
                {
                    keepTrying = false;
                }
            }while(keepTrying);

            Background background = new Background(filename, spriteBatch, game, position);
            background.Initialize();

            return new KeyValuePair<int,Background>(layer, background);
        }
    }
}
