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

        Dictionary<String, String> platformFiles = new Dictionary<String, String>();
        //Dictionary<String, Rectangle[]> platformFiles = new Dictionary<String, Rectangle[]>();

        public Platform platform = null;
        
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

            player = new Player(game, spriteBatch, new Vector2(300f, Bombageddon.GROUND - 64f));
            player.Initialize();
            entityList.AddLast(player);

            //platform = new Platform(game, spriteBatch, @"Graphics\Platforms\hus3", @"Graphics\Collision\Platforms\hus3_collision", new Vector2(-100f, Bombageddon.HEIGHT + 600f));
            //platform.Initialize();
            //entityList.AddLast(platform);

            //platformFiles.Add(@"Graphics\Platforms\hus1", @"Graphics\Collision\Platforms\Hus1_collision");

            //for (int i = 0; i < 5; i++)
            //{
            //    entityList.AddLast(addPlatform());
            //}
        }

        private LinkedListNode<Entity> findFirstOfType(String type)
        {
            LinkedListNode<Entity> temp = entityList.First;
            bool foundFirstPlatform = false;
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
            } while (!foundFirstPlatform);
            return null;
        }

        private LinkedListNode<Entity> findLastOfType(String type)
        {
            LinkedListNode<Entity> temp = entityList.Last;
            bool foundLastPlatform = false;
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
            } while (!foundLastPlatform);
            return null;
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
            int rand = random.Next(platformFiles.Count);
            string filename = platformFiles.ElementAt(rand).Key;
            string collisionname = platformFiles.ElementAt(rand).Value;
            Platform lastPlatform = (Platform)findLastOfType("Platform").Value;
            float posX = lastPlatform.position.X + lastPlatform.SourceRectangle.Width + random.Next(200, 700);
            Vector2 position = new Vector2(posX, Bombageddon.HEIGHT + 600f);
            platform = new Platform(game, spriteBatch, filename, collisionname, position);
            platform.Initialize();

            return platform;
        }

        public void Terminate()
        {
            player = null;
            platform = null;
            foreach (Entity e in entityList)
            {
                e.Terminate();
            }
            entityList.Clear();
        }

        public void Update(GameTime gameTime)
        {
            backgroundManager.Update(gameTime, (int)player.position.X);

            foreach (Entity entity in entityList)
            {
                entity.Update(gameTime);
            }

            //Platform tempPlatform = (Platform)findFirstOfType("Platform").Next.Value;
            //if (tempPlatform.SourceRectangle.Right < player.position.X - Bombageddon.WIDTH)
            //{
            //    refreshPlatforms();
            //}

            CollisionCheck();
        }

        public void Draw(GameTime gameTime)
        {
            backgroundManager.Draw(gameTime);

            foreach (Entity entity in entityList)
            {
                entity.Draw(gameTime);
            }

            player.Draw(gameTime);

            Texture2D t = new Texture2D(game.graphics.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White }); 
            spriteBatch.Draw(t, new Rectangle((int)player.position.X - Bombageddon.WIDTH, Bombageddon.GROUND, Bombageddon.WIDTH * 4, 4), Color.Red); // Bottom
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
            if (player.SourceRectangle.Bottom > Bombageddon.GROUND)
            {
                player.lose = false;
                player.position.Y = Bombageddon.GROUND - player.SourceRectangle.Center.X;
                player.falling = false;
            }

            foreach (Entity entity in entityList)
            {
                if (entity.GetType().Name == "Platform")
                {
                    Platform tmpPlat = (Platform)entity;
                    if (!collision.BasicCheck(player, tmpPlat))
                    {
                        player.falling = true;
                    }
                    else
                    {
                        Side sides = collision.GetSidesCollided(player, tmpPlat);

                        //if ((int)sides % 2 == (int)Side.Top)
                        //Är det en toppkollision bryts spelarens fall och placeras i samma höjd som kollisionsytan.
                        if (sides == Side.Top)
                        {
                            player.falling = false;
                            player.position.Y = tmpPlat.SourceRectangle.Y + tmpPlat.HeightMap[
                                (int)MathHelper.Clamp((player.SourceRectangle.X - tmpPlat.SourceRectangle.X),
                                0, tmpPlat.SourceRectangle.Width)] - 
                                    player.SourceRectangle.Height / 2;
                        }
                        //Är det en vänsterkollision stoppas spelarens
                        if (sides == Side.Left)
                        {
                            player.position.X = (tmpPlat.CollisionRectangle.Left - player.SourceRectangle.Width / 2) - 2;
                            player.falling = true;
                        }
                        return;
                    }
                }
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
