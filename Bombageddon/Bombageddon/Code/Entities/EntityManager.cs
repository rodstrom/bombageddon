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

        //public Platform platform = null;
        List<PlatformData> availablePlatforms = new List<PlatformData>();
        List<CivilianData> civilianData = new List<CivilianData>();

        public Platform platform = null;

        public Sheeples sheeple = null;
        
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

            player = new Player(game, spriteBatch, new Vector2(300f, Bombageddon.GROUND - 512f));
            player.Initialize();
            entityList.AddLast(player);

            CreateListOfAvailablePlatforms();
            PlatformData temp = availablePlatforms[0];
            entityList.AddLast(new Platform(game, spriteBatch, temp));
            
            for (int i = 0; i < 5; i++)
            {
                entityList.AddLast(addPlatform());
            }

            CreateCivilianData();
            sheeple = new Sheeples(spriteBatch, game, new Vector2(500, Bombageddon.GROUND - civilianData[0].panicSheet.Height / 2), civilianData[0]);
            sheeple.Initialize();
            entityList.AddLast(sheeple);
            sheeple = new Sheeples(spriteBatch, game, new Vector2(1000, Bombageddon.GROUND - civilianData[0].panicSheet.Height / 2), civilianData[0]);
            sheeple.Initialize();
            entityList.AddLast(sheeple);
            sheeple = new Sheeples(spriteBatch, game, new Vector2(1200, Bombageddon.GROUND - civilianData[0].panicSheet.Height / 2), civilianData[0]);
            sheeple.Initialize();
            entityList.AddLast(sheeple);
        }

        private void CreateListOfAvailablePlatforms()
        {
            PlatformData platform = new PlatformData(game, @"Graphics\Spritesheets\Hus1_sheet", @"Graphics\Collision\Hus1_collision", 
                new Vector2(500f, Bombageddon.GROUND - 13f), 10);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Spritesheets\Hus2_sheet", @"Graphics\Collision\Hus2_collision", 
                new Vector2(100f, Bombageddon.GROUND - 13f), 10);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Buildings\Sten", @"Graphics\Buildings\Stencollision", 
                new Vector2(100f, Bombageddon.GROUND), -1);
            availablePlatforms.Add(platform);
        }

        private void CreateCivilianData()
        {
            CivilianData tmpCiv;
            tmpCiv.civilianType = "1";
            tmpCiv.panicSheet = game.Content.Load<Texture2D>(@"Graphics\Spritesheets\Man1sheet");
            tmpCiv.collisionSheet = game.Content.Load<Texture2D>(@"Graphics\Collision\Man1_collision");
            tmpCiv.panicFramesCount = 2;
            tmpCiv.splatDeathSheet = game.Content.Load<Texture2D>(@"Graphics\Spritesheets\man1plattningsheet");
            tmpCiv.splatDeathFramesCount = 5;
            tmpCiv.randomDeathSheet1 = game.Content.Load<Texture2D>(@"Graphics\Spritesheets\man1explosionsheet");
            tmpCiv.deathFramesCount1 = 5;
            tmpCiv.randomDeathSheet2 = game.Content.Load<Texture2D>(@"Graphics\Spritesheets\man1kavlingsheet");
            tmpCiv.deathFramesCount2 = 9;
            civilianData.Add(tmpCiv);
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
            Platform platform = new Platform(game, spriteBatch, r);
            platform.Initialize();
            platform.position.X = posX;

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
            if (tempPlatform.position.X < player.position.X - Bombageddon.WIDTH)
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
                    else if(!tmpPlat.ghost)
                    {
                        if (tmpPlat.pointsWorth > 0)
                        {
                            tmpPlat.pause = false;
                            tmpPlat.ghost = true;
                            player.kinetics *= 0.2f;
                            player.points += tmpPlat.pointsWorth;
                        }
                        else
                        {
                            //player.FuseTimer -= 5000;
                            tmpPlat.ghost = true;
                            player.kinetics *= 0f;
                        }

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
                if (entity.GetType().Name == "Sheeples")
                {
                    Sheeples tmpCiv = (Sheeples)entity;
                    if (collision.BasicCheck(player, tmpCiv))
                    {
                        if (collision.GetSidesCollided(player, tmpCiv) == Side.Top)
                        {
                            tmpCiv.IsKilled(true);
                        }
                        else
                        {
                            tmpCiv.IsKilled(false);
                        }
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
    }
}
