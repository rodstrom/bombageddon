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

        private List<KeyValuePair<int, Background>> removeBackgrounds = new List<KeyValuePair<int, Background>>();

        private enum Layers
        {
            SKY = 0,
            FBACK,
            FFRONT,
            GRASS,
            CLOUDS,
            RANDOM
        }

        public BackgroundManager(Bombageddon game, SpriteBatch spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
        }

        public void Initialize()
        {
            backgroundFilenames.Add(new KeyValuePair<int, string>((int)Layers.SKY, @"Graphics\Backgrounds\Mountains"));
            backgroundFilenames.Add(new KeyValuePair<int, string>((int)Layers.FBACK, @"Graphics\Backgrounds\ForestBack"));
            backgroundFilenames.Add(new KeyValuePair<int, string>((int)Layers.FFRONT, @"Graphics\Backgrounds\ForestFront"));
            backgroundFilenames.Add(new KeyValuePair<int, string>((int)Layers.GRASS, @"Graphics\Backgrounds\Ground"));
            backgroundFilenames.Add(new KeyValuePair<int, string>((int)Layers.RANDOM, @"Graphics\Backgrounds\Random\jordmedskelett"));
            backgroundFilenames.Add(new KeyValuePair<int, string>((int)Layers.RANDOM, @"Graphics\Backgrounds\Random\jordmedsten"));
            backgroundFilenames.Add(new KeyValuePair<int, string>((int)Layers.RANDOM, @"Graphics\Backgrounds\Random\jordmedjolt"));
            backgroundFilenames.Add(new KeyValuePair<int, string>((int)Layers.RANDOM, @"Graphics\Backgrounds\Random\jordmedlizardswanguy"));
            backgroundFilenames.Add(new KeyValuePair<int, string>((int)Layers.RANDOM, @"Graphics\Backgrounds\Random\Badanka"));
            backgroundFilenames.Add(new KeyValuePair<int, string>((int)Layers.RANDOM, @"Graphics\Backgrounds\Random\Jordmedboll"));
            backgroundFilenames.Add(new KeyValuePair<int, string>((int)Layers.RANDOM, @"Graphics\Backgrounds\Random\JordmedLaptop"));

            KeyValuePair<int, Background> background = new KeyValuePair<int, Background>((int)Layers.SKY,
                new Background(@"Graphics\Backgrounds\Mountains", spriteBatch, game, new Vector2(-Bombageddon.WIDTH, Bombageddon.HEIGHT)));
            background.Value.Initialize();
            background.Value.stuck = true;
            backgroundList.AddLast(background);
            background = new KeyValuePair<int, Background>((int)Layers.FBACK,
                 new Background(@"Graphics\Backgrounds\ForestBack", spriteBatch, game, new Vector2(-Bombageddon.WIDTH, Bombageddon.HEIGHT)));
            background.Value.Initialize();
            background.Value.velocityX = -1;
            backgroundList.AddLast(background);
            for (int i = 0; i < 3; i++)
            {
                backgroundList.AddLast(addBackground((int)Layers.FBACK));
            }
            background = new KeyValuePair<int, Background>((int)Layers.FFRONT,
                 new Background(@"Graphics\Backgrounds\ForestFront", spriteBatch, game, new Vector2(-Bombageddon.WIDTH, Bombageddon.HEIGHT)));
            background.Value.Initialize();
            //background.Value.velocityX = -2;
            backgroundList.AddLast(background);
            for (int i = 0; i < 3; i++)
            {
                backgroundList.AddLast(addBackground((int)Layers.FFRONT));
            }
            background = new KeyValuePair<int, Background>((int)Layers.GRASS,
                new Background(@"Graphics\Backgrounds\Ground", spriteBatch, game, new Vector2(-Bombageddon.WIDTH, Bombageddon.HEIGHT)));
            background.Value.Initialize();
            backgroundList.AddLast(background);
            for (int i = 0; i < 15; i++)
            {
                backgroundList.AddLast(addBackground((int)Layers.GRASS));
            }
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
            background = new KeyValuePair<int, Background>((int)Layers.RANDOM,
                new Background(@"Graphics\Backgrounds\Random\jordmedjolt", spriteBatch, game, new Vector2(700f, Bombageddon.HEIGHT)));
            background.Value.Initialize();
            backgroundList.AddLast(background);
            backgroundList.AddLast(addBackground((int)Layers.RANDOM));

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

        public void Terminate()
        {
            foreach (KeyValuePair<int, Background> b in backgroundList)
            {
                b.Value.Terminate();
            }
            backgroundList.Clear();
            removeBackgrounds.Clear();
            backgroundFilenames.Clear();
        }

        public void Update(GameTime gameTime)
        {
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
                b.Value.Terminate();
                backgroundList.Remove(b);
            }

            removeBackgrounds.Clear();

            refreshBackgrounds();
        }

        public void Draw(GameTime gameTime, bool front)
        {
            int start = 0;
            int end = 3;
            if (front)
            {
                start = 3;
                end = 6;
            }

            for (int i = start; i < end; i++)
            {
                foreach (Background background in getBackgroundsByLayer(i))
                {
                    background.Draw(gameTime);
                }
            }
        }
        
        private void refreshBackgrounds()
        {
            List<int> newBackgrounds = new List<int>();
            foreach (KeyValuePair<int, Background> background in backgroundList)
            {
                if (background.Value.SourceRectangle.Right < game.Camera.Focus.position.X - Bombageddon.WIDTH)
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
            if (layer == (int)Layers.RANDOM)
            {
                offset = rand.Next(1600, 3500);
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
            if (layer == 1)
            {
                background.velocityX = -1;
            }
            //else if (layer == 2)
            //{
            //    background.velocityX = -2;
            //}

            return new KeyValuePair<int,Background>(layer, background);
        }
    }
}
