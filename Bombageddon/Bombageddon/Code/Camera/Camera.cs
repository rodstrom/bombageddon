using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bombageddon.Code.Entities;
using Bombageddon.Code.Graphics;

namespace Bombageddon.Code.Camera
{
    public class Camera2D : GameComponent
    {
        public Viewport View
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public Vector2 Center
        {
            get;
            set;
        }

        private float _zoom;
        public float Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = value;
                MathHelper.Clamp(Zoom, 0.1f, 5f);
                Center = new Vector2(View.Width * (0.5f / Zoom), View.Height * (0.5f / Zoom));
            }
        }

        public float Rotation
        {
            get;
            set;
        }

        public Matrix Transform
        {
            get;
            set;
        }

        public Entity Focus
        {
            get;
            set;
        }

        public Camera2D(Viewport view, Game game) : base(game)
        {
            View = view;
            Zoom = 1.0f;
            Rotation = 0.0f;
            Center = new Vector2(View.Width / 2, View.Height / 2);
        }

        public Camera2D(Viewport view, float zoom, float rotation, Game game) : base(game)
        {
            View = view;
            Zoom = zoom;
            Rotation = rotation;
            Center = new Vector2(View.Width * (0.5f / Zoom), View.Height * (0.5f / Zoom));
        }

        public override void Update(GameTime gameTime)
        {
            if (Focus != null)
            {
                Position = new Vector2(Focus.position.X + Center.X / 2, Bombageddon.HEIGHT / 2);
            }
            Transform = Resolution.getTransformationMatrix();
            Transform = Matrix.Identity *
                Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateTranslation(Center.X, Center.Y, 0) *
                Matrix.CreateScale(new Vector3(Zoom));
        }
    }
}
