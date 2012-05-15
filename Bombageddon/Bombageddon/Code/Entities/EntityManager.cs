using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bombageddon.Code.Graphics;
using Bombageddon.Code.Physics;

namespace Bombageddon.Code.Entities
{
    class EntityManager
    {
        Bombageddon game;
        SpriteBatch spriteBatch;
        BackgroundManager backgroundManager;

        Collision collision;

        public Player player;
        public LinkedList<Entity> entityList = new LinkedList<Entity>();
        Random random = new Random();

        //Dictionary<String, String> platformFiles = new Dictionary<String, String>();
        //Dictionary<String, Rectangle[]> platformFiles = new Dictionary<String, Rectangle[]>();

        //public Platform platform = null;
        List<PlatformData> availablePlatforms = new List<PlatformData>();
        
        public EntityManager(Bombageddon game, SpriteBatch spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
        }

        public void Initialize()
        {
            collision = new Collision();

            backgroundManager = new BackgroundManager(game, spriteBatch);
            backgroundManager.Initialize();

            player = new Player(game, spriteBatch, new Vector2(300f, Bombageddon.GROUND - 256f));
            player.Initialize();
            entityList.AddLast(player);

            CreateListOfAvailablePlatforms();
            PlatformData temp = availablePlatforms[0];
            entityList.AddLast(new Platform(game, spriteBatch, temp.Texture, temp.HitTexture, temp.position, temp.points));

            for (int i = 0; i < 5; i++)
            {
                entityList.AddLast(addPlatform());
            }
        }

        private void CreateListOfAvailablePlatforms()
        {
            PlatformData platform = new PlatformData(game, @"Graphics\Buildings\Hus1", @"Graphics\Collision\Hus1_collision", 
                new Vector2(100f, Bombageddon.GROUND - 15f), 10);
            availablePlatforms.Add(platform); 

            platform = new PlatformData(game, @"Graphics\Buildings\Hus2", @"Graphics\Collision\Hus2_collision",
                new Vector2(100f, Bombageddon.GROUND - 15f), 10);
            availablePlatforms.Add(platform);
        }

        private LinkedListNode<Entity> findFirstOfType(String type)
        {
            LinkedListNode<Entity> temp = entityList.First;
            do
            {
                if (temp.Value.GetType().Name.Equals(type))
                {
                    return temp;
                }
                else
                {
                    temp = temp.Next;
                }
            } while (true);
        }

        private LinkedListNode<Entity> findLastOfType(String type)
        {
            LinkedListNode<Entity> temp = entityList.Last;
            do
            {
                if (temp.Value.GetType().Name.Equals(type))
                {
                    return temp;
                }
                else
                {
                    temp = temp.Previous;
                }
            } while (true);
        }

        private void refreshPlatforms()
        {
            Platform p = (Platform)findFirstOfType("Platform").Value;
            p.Terminate();
            entityList.Remove(p);
            entityList.AddLast(addPlatform());
        }

        private Platform addPlatform()
        {
            Platform lastPlatform = (Platform)findLastOfType("Platform").Value;
            float posX = lastPlatform.position.X + random.Next(600, 1000);
            PlatformData r = availablePlatforms[random.Next(availablePlatforms.Count)];
            Platform platform = new Platform(game, spriteBatch, r.Texture, r.HitTexture, r.position, r.points);
            platform.position.X = posX;
            platform.Initialize();

            return platform;
        }

        public void Terminate()
        {
            foreach (Entity e in entityList)
            {
                e.Terminate();
            } 
            foreach (PlatformData p in availablePlatforms)
            {
                p.Terminate();
            }
            entityList.Clear();
            availablePlatforms.Clear();
            player = null;
        }

        public void Update(GameTime gameTime)
        {
            backgroundManager.Update(gameTime, (int)player.position.X);

            List<Entity> removeList = new List<Entity>();
            foreach (Entity entity in entityList)
            {
                entity.Update(gameTime);
                if (entity.KillMe)
                {
                    removeList.Add(entity);
                }
            }

            foreach (Entity e in removeList)
            {
                entityList.Remove(e);
                entityList.AddLast(addPlatform());
            }

            Platform tempPlatform = (Platform)findFirstOfType("Platform").Value;
            if (tempPlatform.position.X < player.position.X - Bombageddon.WIDTH / 2)
            {
                refreshPlatforms();
            }

            CollisionCheck();
        }

        public void Draw(GameTime gameTime)
        {
            backgroundManager.Draw(gameTime, false);

            foreach (Entity entity in entityList)
            {
                entity.Draw(gameTime);
            }

            //player.Draw(gameTime);

            backgroundManager.Draw(gameTime, true);

            //Texture2D t = new Texture2D(game.graphics.GraphicsDevice, 1, 1);
            //t.SetData(new[] { Color.White }); 
            //spriteBatch.Draw(t, new Rectangle((int)player.position.X - Bombageddon.WIDTH, Bombageddon.GROUND, Bombageddon.WIDTH * 4, 4), Color.Red); // Bottom
        }

        //private void CollisionCheck()
        //{
        //    if (player.SourceRectangle.Bottom > Runner.HEIGHT)
        //    {
        //        player.lose = true;
        //    }

        //    player.falling = false;
        //    foreach(Entity entity in entityList)
        //    {
        //        if (entity.GetType().Name == "Platform")
        //        {
        //            Platform tmpPlat = (Platform)entity;
        //            if (!(player.Rectangle.Intersects(tmpPlat.HitRectangle)))
        //            {
        //                player.falling = true;
        //            }
        //            else if (player.Rectangle.Intersects(tmpPlat.HitRectangle))
        //            {
        //                SideCollided sides = GetSidesCollided(player.Rectangle, tmpPlat.HitRectangle);
        //                if ((int)sides % 2 == (int)SideCollided.Top)
        //                {
        //                    player.position.Y = (tmpPlat.HitRectangle.Top - player.Rectangle.Height / 2) - 2;
        //                    player.falling = false;
        //                }
        //                if (sides == SideCollided.Left)
        //                {
        //                    player.position.X = (tmpPlat.HitRectangle.Left - player.Rectangle.Width / 2) - 2;
        //                    player.falling = true;
        //                }
        //                return;
        //            }
        //            if(tmpPlat.hitRect2Enabled)
        //            {
        //                if (!(player.Rectangle.Intersects(tmpPlat.HitRectangle2)))
        //                {
        //                    player.falling = true;
        //                }
        //                else if (player.Rectangle.Intersects(tmpPlat.HitRectangle2))
        //                {
        //                    SideCollided sides = GetSidesCollided(player.Rectangle, tmpPlat.HitRectangle2);
        //                    if ((int)sides % 2 == (int)SideCollided.Top)
        //                    {
        //                        player.position.Y = (tmpPlat.HitRectangle2.Top - player.Rectangle.Height / 2) - 2;
        //                        player.falling = false;
        //                    }
        //                    return;
        //                }
        //            }
        //        }
        //    }
        //}

        //public enum SideCollided
        //{
        //    None = 0x00,
        //    Top = 0x01,
        //    Bottom = 0x02,
        //    Left = 0x04,
        //    Right = 0x08,
        //}

        private void CollisionCheck()
        {
            foreach (Entity entity in entityList)
            {
                if (entity.GetType().Name == "Platform")
                {
                    Platform tmpPlat = (Platform)entity;
                    if (!collision.BasicCheck(player, tmpPlat))
                    {
                        //player.Falling = true;
                    }
                    else
                    {
                        tmpPlat.KillMe = true;
                        player.kinetics *= 0.1f;
                        player.points += tmpPlat.pointsWorth;

                        //Side sides = collision.GetSidesCollided(player, tmpPlat);

                        ////if ((int)sides % 2 == (int)Side.Top)
                        ////Är det en toppkollision bryts spelarens fall och placeras i samma höjd som kollisionsytan.
                        //if (sides == Side.Top)
                        //{
                        //    //player.Falling = false;
                        //    player.position.Y = tmpPlat.SourceRectangle.Y + tmpPlat.HeightMap[
                        //        (int)MathHelper.Clamp((player.SourceRectangle.X - tmpPlat.SourceRectangle.X),
                        //        0, tmpPlat.SourceRectangle.Width)] - 
                        //            player.SourceRectangle.Height / 2;
                        //}
                        ////Är det en vänsterkollision stoppas spelarens
                        //if (sides == Side.Left)
                        //{
                        //    player.position.X = (tmpPlat.CollisionRectangle.Left - player.SourceRectangle.Width / 2) - 2;
                        //    //player.Falling = true;
                        //}
                        return;
                    }
                }
            }

            if (player.position.Y + 64 > Bombageddon.GROUND)
            {
                player.position.Y = Bombageddon.GROUND - 64 - 2;
                player.Falling = false;
            }
            else
            {
                player.Falling = true;
            }
        }

        //public static SideCollided GetSidesCollided(Rectangle sourceRect, Rectangle targetRect)
        //{
        //    Vector2 targetCenter = new Vector2(targetRect.Center.X, targetRect.Center.Y);
        //    Vector2 sourceCenter = new Vector2(sourceRect.Center.X, sourceRect.Center.Y);

        //    SideCollided returnVal = SideCollided.None;

        //    // test left side  
        //    if (sourceRect.Right > targetRect.Left && sourceRect.Left < targetRect.Left &&
        //        sourceRect.Bottom > targetRect.Top && sourceRect.Top < targetRect.Bottom)
        //        returnVal = (returnVal | SideCollided.Left);

        //    // test top side  
        //    if (sourceRect.Center.X > targetRect.Left && sourceRect.Center.X < targetRect.Right &&
        //        sourceRect.Bottom > targetRect.Top && sourceRect.Top - 500 < targetRect.Top)
        //        returnVal = (returnVal | SideCollided.Top);

        //    //// test right side  
        //    //if (sourcePoint.X > centerLocation.X && sourcePoint.X < targetRect.Right &&
        //    //    sourcePoint.Y > targetRect.Top && sourcePoint.Y < targetRect.Bottom)
        //    //    returnVal = (returnVal | SideCollided.Right);

        //    //// test bottom side  
        //    //if (sourcePoint.X > targetRect.Left && sourcePoint.X < targetRect.Right &&
        //    //    sourcePoint.Y > centerLocation.Y && sourcePoint.Y < targetRect.Bottom)
        //    //    returnVal = (returnVal | SideCollided.Bottom);


        //    return returnVal;
        //} 
    }
}
