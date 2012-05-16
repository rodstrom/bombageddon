using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Bombageddon.Code.Graphics;
using Bombageddon.Code.Input;
using Bombageddon.Code.States;
using Bombageddon.Code.Audio;
using Bombageddon.Code.Camera;
using Bombageddon.Code.Time;
using Bombageddon.Code.Entities;

namespace Bombageddon
{
    public class Bombageddon : Microsoft.Xna.Framework.Game
    {
        public const int WIDTH = 1920;
        public const int HEIGHT = 1200;

        public const int GROUND = 1000;

        public InputFile config;

        StateManager stateManager;

        AudioManager audioManager = null;

        Timer timer;

        public GraphicsDeviceManager graphics;

        Camera2D camera = null;
        //SpriteBatch spriteBatch;

        //SpriteFont font = null;

        //Texture2D background;

        public Timer Timer
        {
            get { return timer; }
        }

        public Camera2D Camera
        {
            get { return camera; }
        }

        public AudioManager AudioManager
        {
            get { return audioManager; }
        }

        public Bombageddon()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //this.IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            config = new InputFile(@"Content\Configs\config.ini");
            config.parse();

            timer = new Timer(this);

            audioManager = new AudioManager(this);
            audioManager.LoadNewMusic("Background", @"Audio\Music\Background");
            audioManager.SetMusic("Background");
            audioManager.PlayMusic();

            Resolution.Init(ref graphics);
            Resolution.SetResolution(
                int.Parse(config.getValue("Video", "Width")),
                int.Parse(config.getValue("Video", "Height")),
                bool.Parse(config.getValue("Video", "Fullscreen"))
                );

            Viewport view = new Viewport(0, 0,
                int.Parse(config.getValue("Video", "Width")),
                int.Parse(config.getValue("Video", "Height"))
                );
            //Viewport view = new Viewport(0, 0, Bombageddon.WIDTH, Bombageddon.HEIGHT);
            camera = new Camera2D(view, 1f, 0f, this);

            this.IsMouseVisible = false;

            stateManager = new StateManager(this);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            stateManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            stateManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}