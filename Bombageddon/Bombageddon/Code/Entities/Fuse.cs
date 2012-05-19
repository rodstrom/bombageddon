using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bombageddon.Code.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Bombageddon.Code.Entities
{
    class Fuse : Animation
    {
        Texture2D fuse;
        Texture2D burntFuse;

        Vector2 fuseOrigin;
        Vector2 burntFuseOrigin;

        Vector2 fuseLineStart;

        Rectangle fuseBurningDown;
        int startWidth;

        public Fuse(Bombageddon game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {

        }

        protected override void LoadContent()
        {
            fuse = game.Content.Load<Texture2D>(@"Graphics\Fuse\Stubin");
            burntFuse = game.Content.Load<Texture2D>(@"Graphics\Fuse\brändstubin");

            Texture2D animTex = game.Content.Load<Texture2D>(@"Graphics\Fuse\stubinbrinn");
            Texture2D empty = new Texture2D(game.graphics.GraphicsDevice, 2000, 50);

            AnimationStrip burning = new AnimationStrip();
            for (int x = 0; x < animTex.Width / animTex.Height; x++)
            {
                burning.AddFrame(new AnimationFrame(animTex, new Rectangle(animTex.Height * x, 0, animTex.Height, animTex.Height), empty));
            }
            burning.TimeOnChange = 50;
            burning.WillLoop = true;

            this.AddAnimation("Burning", burning);
            this.AnimationName = "Burning";

            burning = null;
            animTex = null;
            empty = null;

            fuseOrigin = new Vector2(fuse.Bounds.Center.Y, fuse.Bounds.Left);
            burntFuseOrigin = new Vector2(burntFuse.Bounds.Center.Y, burntFuse.Bounds.Left);

            position = new Vector2(-200f, 1050f);
            fuseLineStart = position;
            position.Y += 38;

            fuseBurningDown = fuse.Bounds;
            startWidth = fuseBurningDown.Width;

            //game.AudioManager.LoadNewMusic("Fuse", @"Audio\Music\stubin");
            //game.AudioManager.SetMusic("Fuse");
            //game.AudioManager.PlayMusic();

            base.LoadContent();
        }

        public override void Terminate()
        {
            //fuse.Dispose();
            //burntFuse.Dispose();
            fuse = null;
            burntFuse = null;
            base.Terminate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            //Origin = new Vector2(CurrentFrame.SourceRectangle.Center.X, CurrentFrame.SourceRectangle.Center.Y);

            fuseBurningDown.Width = (int)((float)startWidth * ((float)game.Camera.Focus.FuseTimer / (float)game.Camera.Focus.StartTime));
            position.X = game.Camera.Focus.position.X - 250 + fuseBurningDown.Width;
            fuseLineStart.X = game.Camera.Focus.position.X - 240;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!burntFuse.IsDisposed && !fuse.IsDisposed)
            {
                SpriteBatch.Draw(burntFuse, fuseLineStart, null, Color.White, 0f, burntFuseOrigin, 1f, SpriteEffects.None, 0.0f);
                SpriteBatch.Draw(fuse, fuseLineStart, fuseBurningDown, Color.White, 0f, fuseOrigin, 1f, SpriteEffects.None, 0.0f);
            }

            base.Draw(gameTime);
        }
    }
}
