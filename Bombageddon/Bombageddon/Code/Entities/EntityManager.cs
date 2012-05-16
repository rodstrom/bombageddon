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

        Dictionary<int, Texture2D> pointTextures = new Dictionary<int, Texture2D>();

        public Platform platform = null;

        public Sheeple sheeple = null;
        private int addThisManySheeples = 0;
        
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
            PlatformData temp = availablePlatforms[2];
            entityList.AddLast(new Platform(game, spriteBatch, temp));
            for (int i = 0; i < 5; i++)
            {
                entityList.AddLast(addPlatform());
            }

            CreateCivilianData();
            CivilianData civ = civilianData[0];
            Vector2 pos = new Vector2(player.position.X + 400f, Bombageddon.GROUND);
            Sheeple tmpSheeple = new Sheeple(spriteBatch, game, pos, civ);
            tmpSheeple.Initialize();
            entityList.AddLast(tmpSheeple);
            for (int i = 0; i < 20; i++)
            {
                entityList.AddLast(addSheeple());
            }

            CreateListOfPoints();
        }

        private void CreateListOfPoints()
        {
            pointTextures.Add(1, game.Content.Load<Texture2D>(@"Graphics\Points\+1"));
            pointTextures.Add(5, game.Content.Load<Texture2D>(@"Graphics\Points\+5"));
            pointTextures.Add(10, game.Content.Load<Texture2D>(@"Graphics\Points\10"));
            pointTextures.Add(50, game.Content.Load<Texture2D>(@"Graphics\Points\50"));
        }

        private Sheeple addSheeple()
        {
            Sheeple lastSheeple = (Sheeple)findLastOfType("Sheeple").Value;
            Vector2 pos = new Vector2(lastSheeple.position.X + random.Next(100, 300), Bombageddon.GROUND);
            CivilianData r = civilianData[random.Next(civilianData.Count)];
            Sheeple sheeple = new Sheeple(spriteBatch, game, pos, r);
            sheeple.Initialize();
            
            return sheeple;
        }

        private void CreateListOfAvailablePlatforms()
        {
            PlatformData platform = new PlatformData(game, @"Graphics\Spritesheets\Hus1_sheet", @"Graphics\Collision\Hus1_collision", 
                new Vector2(500f, Bombageddon.GROUND - 13f), 50);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Spritesheets\Hus2_sheet", @"Graphics\Collision\Hus2_collision", 
                new Vector2(100f, Bombageddon.GROUND - 13f), 50);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Spritesheets\Hus3_sheet", @"Graphics\Collision\Hus3_collision",
                new Vector2(100f, Bombageddon.GROUND - 13f), 50);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Buildings\Sten", @"Graphics\Buildings\Stencollision", 
                new Vector2(100f, Bombageddon.GROUND), -1);
            availablePlatforms.Add(platform);

            game.AudioManager.LoadNewEffect("Crash", @"Audio\Sound\Crash\buildingexplosion");
            game.AudioManager.LoadNewEffect("Crash", @"Audio\Sound\Crash\hosedesetruction1");
        }

        private void CreateCivilianData()
        {
            Texture2D empty = new Texture2D(game.graphics.GraphicsDevice, 2560, 256);
            CivilianData civilian = new CivilianData(game, "Man1", 10, empty);
            civilianData.Add(civilian);
            civilian = new CivilianData(game, "Man2", 10, empty);
            civilianData.Add(civilian);

            game.AudioManager.LoadNewEffect("Scream", @"Audio\Sound\Screams\Nej");
            game.AudioManager.LoadNewEffect("Scream", @"Audio\Sound\Screams\Skrik1");
            game.AudioManager.LoadNewEffect("Scream", @"Audio\Sound\Screams\Skrik2");
            game.AudioManager.LoadNewEffect("Scream", @"Audio\Sound\Screams\Skrik3");
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

        private void refreshSheeples()
        {
            Sheeple s = (Sheeple)findFirstOfType("Sheeple").Value;
            s.Terminate();
            entityList.Remove(s);
            if (findLastOfType("Sheeple").Value.position.X < player.position.X + Bombageddon.WIDTH)
            {
                entityList.AddLast(addSheeple());
            }
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
            foreach (CivilianData c in civilianData)
            {
                c.Terminate();
            } 
            entityList.Clear();
            availablePlatforms.Clear();
            civilianData.Clear();
            pointTextures.Clear();
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
                if (e.GetType().ToString().Equals("Platform"))
                {
                    entityList.AddLast(addPlatform());
                }
                else if (e.GetType().ToString().Equals("Sheeple"))
                {
                    if (findLastOfType("Sheeple").Value.position.X < player.position.X + Bombageddon.WIDTH)
                    {
                        entityList.AddLast(addSheeple());
                    }
                }
            }

            if (findLastOfType("Sheeple").Value.position.X < player.position.X + Bombageddon.WIDTH)
            {
                for (int i = addThisManySheeples; i > 0; i--)
                {
                    entityList.AddLast(addSheeple());
                    addThisManySheeples--;
                }
            }
            else
            {
                addThisManySheeples = 0;
            }

            Platform tempPlatform = (Platform)findFirstOfType("Platform").Value;
            if (tempPlatform.position.X < player.position.X - Bombageddon.WIDTH)
            {
                refreshPlatforms();
            }

            Sheeple tempSheeple = (Sheeple)findFirstOfType("Sheeple").Value;
            if (tempSheeple.position.X < player.position.X - Bombageddon.WIDTH)
            {
                refreshSheeples();
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

            backgroundManager.Draw(gameTime, true);
        }

        private void CollisionCheck()
        {
            List<KeyValuePair<int, Vector2>> addThesePoints = new List<KeyValuePair<int, Vector2>>();

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
                            game.AudioManager.PlayEffect("Crash");
                            addThesePoints.Add(new KeyValuePair<int, Vector2>(tmpPlat.pointsWorth, tmpPlat.position));
                        }
                        else
                        {
                            //player.FuseTimer -= 5000;
                            tmpPlat.ghost = true;
                            player.kinetics *= 0f;
                        }
                        //return;
                    }
                }
                if (entity.GetType().Name == "Sheeple")
                {
                    Sheeple tmpCiv = (Sheeple)entity;
                    if (!tmpCiv.ghost)
                    {
                        if (collision.BasicCheck(player, tmpCiv))
                        {
                            if (collision.GetSidesCollided(player, tmpCiv) == Side.Top)
                            {
                                tmpCiv.IsKilled(true);
                                //addThesePoints.Add(new KeyValuePair<int, Vector2>(5, tmpCiv.position - new Vector2(20f)));    //BONUS
                            }
                            else
                            {
                                tmpCiv.IsKilled(false);
                            }

                            player.points += tmpCiv.pointsWorth;
                            tmpCiv.ghost = true;
                            addThisManySheeples++;
                            addThesePoints.Add(new KeyValuePair<int, Vector2>(tmpCiv.pointsWorth, tmpCiv.position));
                        }
                    }
                }
            }
            foreach (KeyValuePair<int, Vector2> p in addThesePoints)
            {
                entityList.AddLast(new FlyingPoint(spriteBatch, game, pointTextures[p.Key], p.Value));
            }
            addThesePoints.Clear();

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
