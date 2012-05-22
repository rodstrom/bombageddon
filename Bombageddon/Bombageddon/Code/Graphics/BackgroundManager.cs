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
        private Texture2D texture;

        private LinkedList<KeyValuePair<int, Background>> backgroundList = new LinkedList<KeyValuePair<int, Background>>();
        private List<KeyValuePair<int, Texture2D>> backgroundFilenames = new List<KeyValuePair<int, Texture2D>>();

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
            backgroundFilenames.Add(new KeyValuePair<int, Texture2D>((int)Layers.SKY, game.Content.Load<Texture2D>(@"Graphics\Backgrounds\NewBack")));
            //backgroundFilenames.Add(new KeyValuePair<int, Texture2D>((int)Layers.FBACK, game.Content.Load<Texture2D>(@"Graphics\Backgrounds\ForestBack")));
            //backgroundFilenames.Add(new KeyValuePair<int, Texture2D>((int)Layers.FFRONT, game.Content.Load<Texture2D>(@"Graphics\Backgrounds\ForestFront")));
            backgroundFilenames.Add(new KeyValuePair<int, Texture2D>((int)Layers.GRASS, game.Content.Load<Texture2D>(@"Graphics\Backgrounds\NewFront")));
            //backgroundFilenames.Add(new KeyValuePair<int, Texture2D>((int)Layers.RANDOM, game.Content.Load<Texture2D>(@"Graphics\Backgrounds\Random\jordmedskelett")));
            //backgroundFilenames.Add(new KeyValuePair<int, Texture2D>((int)Layers.RANDOM, game.Content.Load<Texture2D>(@"Graphics\Backgrounds\Random\jordmedsten")));
            //backgroundFilenames.Add(new KeyValuePair<int, Texture2D>((int)Layers.RANDOM, game.Content.Load<Texture2D>(@"Graphics\Backgrounds\Random\jordmedjolt")));
            //backgroundFilenames.Add(new KeyValuePair<int, Texture2D>((int)Layers.RANDOM, game.Content.Load<Texture2D>(@"Graphics\Backgrounds\Random\jordmedlizardswanguy")));
            //backgroundFilenames.Add(new KeyValuePair<int, Texture2D>((int)Layers.RANDOM, game.Content.Load<Texture2D>(@"Graphics\Backgrounds\Random\Badanka")));
            //backgroundFilenames.Add(new KeyValuePair<int, Texture2D>((int)Layers.RANDOM, game.Content.Load<Texture2D>(@"Graphics\Backgrounds\Random\Jordmedboll")));
            //backgroundFilenames.Add(new KeyValuePair<int, Texture2D>((int)Layers.RANDOM, game.Content.Load<Texture2D>(@"Graphics\Backgrounds\Random\JordmedLaptop")));

            KeyValuePair<int, Background> background = new KeyValuePair<int, Background>((int)Layers.SKY,
                new Background(game.Content.Load<Texture2D>(@"Graphics\Backgrounds\NewBack"), spriteBatch, game, new Vector2(-Bombageddon.WIDTH, Bombageddon.HEIGHT)));
            background.Value.Initialize();
            //background.Value.stuck = true;
            background.Value.velocityX = -1;
            backgroundList.AddLast(background);
            for (int i = 0; i < 2; i++)
            {
                backgroundList.AddLast(addBackground((int)Layers.SKY));
            }

            //background = new KeyValuePair<int, Background>((int)Layers.FBACK,
            //     new Background(game.Content.Load<Texture2D>(@"Graphics\Backgrounds\ForestBack"), spriteBatch, game, new Vector2(-Bombageddon.WIDTH, Bombageddon.HEIGHT)));
            //background.Value.Initialize();
            //background.Value.velocityX = -1;
            //backgroundList.AddLast(background);
            //for (int i = 0; i < 3; i++)
            //{
            //    backgroundList.AddLast(addBackground((int)Layers.FBACK));
            //}

            //background = new KeyValuePair<int, Background>((int)Layers.FFRONT,
            //     new Background(game.Content.Load<Texture2D>(@"Graphics\Backgrounds\ForestFront"), spriteBatch, game, new Vector2(-Bombageddon.WIDTH, Bombageddon.HEIGHT)));
            //background.Value.Initialize();
            ////background.Value.velocityX = -2;
            //backgroundList.AddLast(background);
            //for (int i = 0; i < 3; i++)
            //{
            //    backgroundList.AddLast(addBackground((int)Layers.FFRONT));
            //}

            background = new KeyValuePair<int, Background>((int)Layers.GRASS,
                new Background(game.Content.Load<Texture2D>(@"Graphics\Backgrounds\NewFront"), spriteBatch, game, new Vector2(-Bombageddon.WIDTH, Bombageddon.HEIGHT + 56f)));
            background.Value.Initialize();
            backgroundList.AddLast(background);
            for (int i = 0; i < 3; i++)
            {
                backgroundList.AddLast(addBackground((int)Layers.GRASS));
            }

            //background = new KeyValuePair<int, Background>((int)Layers.RANDOM,
            //    new Background(game.Content.Load<Texture2D>(@"Graphics\Backgrounds\Random\jordmedjolt"), spriteBatch, game, new Vector2(700f, Bombageddon.HEIGHT)));
            //background.Value.Initialize();
            //backgroundList.AddLast(background);
            //backgroundList.AddLast(addBackground((int)Layers.RANDOM));

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

            bool keepTrying = true;
            do{
                rand = new Random();
                int randomFilename = rand.Next(backgroundFilenames.Count);
                texture = backgroundFilenames.ElementAt(randomFilename).Value;
                if (backgroundFilenames.ElementAt(randomFilename).Key == layer)
                {
                    keepTrying = false;
                }
            }while(keepTrying);

            Background background = new Background(texture, spriteBatch, game, position);
            background.Initialize();
            if (layer == 0)
            {
                background.velocityX = -1;
            }
            else if (layer == (int)Layers.GRASS)
            {
                background.position.Y += 56f;
            }

            return new KeyValuePair<int,Background>(layer, background);
        }
    }
}
