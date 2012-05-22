using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bombageddon.Code.Input;

namespace Bombageddon.Code.States
{
    abstract class State
    {
        private String id;
        protected Bombageddon game;
        protected InputManager inputManager;

        public String inputCode = "";
        public virtual String InputCode
        {
            get
            {
                return inputCode;
            }
            set
            {
                inputCode = value;
            }
        }
        public String outputCode = "";

        protected SpriteBatch spriteBatch;
        protected SpriteFont font;

        public bool changeState = false;
        public String nextState;

        public State(Bombageddon game, String id)
        {
            this.id = id;
            this.game = game;

            inputManager = new InputManager(game);
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            font = game.Content.Load<SpriteFont>(@"Fonts\font");
        }

        public abstract void Terminate();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);

        public String ID
        {
            get
            {
                return id;
            }
        }
    }
}
