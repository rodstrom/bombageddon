using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Bombageddon.Code.Graphics;
using Microsoft.Xna.Framework;

namespace Bombageddon.Code.Entities
{
    public class Entity : DrawableGameComponent
    {
        public float Rotation
        {
            get;
            set;
        }

        public float Scale
        {
            get;
            set;
        }

        public Vector2 Origin
        {
            get;
            set;
        }


        public Vector2 position;

        public Color[,] ColorData
        {
            get;
            protected set;
        }

        public int[] HeightMap
        {
            get;
            protected set;
        }

        public virtual void Terminate()
        {

        }

        protected Rectangle sourceRectangle;
        protected Rectangle collisionRectangle;

        public Rectangle SourceRectangle
        {
            get { return sourceRectangle; }
            protected set { sourceRectangle = value; }
        }

        public Rectangle CollisionRectangle
        {
            get { return collisionRectangle; }
            protected set { collisionRectangle = value; }
        }

        public Boolean KillMe
        {
            get;
            set;
        }

        protected Bombageddon Runner
        {
            get;
            private set;
        }

        protected SpriteBatch SpriteBatch
        {
            get;
            private set;
        }

        public float Layer
        {
            get;
            set;
        }

        public Entity(Bombageddon game, SpriteBatch spriteBatch)
            : base(game)
        {
            Runner = game;
            SpriteBatch = spriteBatch;
            Scale = 1f;
            Rotation = 0.0f;
            Layer = 1f;
            KillMe = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
