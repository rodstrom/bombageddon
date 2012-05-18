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

        public bool blazeOfGloryDone = false;
        bool blazeOfGloryStarted = false;

        int birdsInTheAir = 0;

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
            PlatformData temp = availablePlatforms[1];
            entityList.AddLast(new Platform(game, spriteBatch, temp));
            for (int i = 0; i < 5; i++)
            {
                entityList.AddLast(addPlatform());
            }

            CreateCivilianData();
            CivilianData civ = civilianData[0];
            Vector2 pos = new Vector2(player.position.X + 400f, Bombageddon.GROUND - 8);
            Sheeple tmpSheeple = new Sheeple(spriteBatch, game, pos, civ);
            tmpSheeple.Initialize();
            entityList.AddLast(tmpSheeple);
            for (int i = 0; i < 30; i++)
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

            game.AudioManager.LoadNewEffect("Point", @"Audio\Sound\Points\point1");
            game.AudioManager.LoadNewEffect("Point", @"Audio\Sound\Points\point2");
            game.AudioManager.LoadNewEffect("Point", @"Audio\Sound\Points\point3");
        }

        private Sheeple addSheeple()
        {
            bool tryAgain;
            Sheeple sheeple;

            do
            {
                tryAgain = false;
                Sheeple lastSheeple = (Sheeple)findLastOfType("Sheeple").Value;
                Vector2 pos = new Vector2(lastSheeple.position.X + random.Next(30, 150), Bombageddon.GROUND - 8);
                CivilianData r = civilianData[random.Next(civilianData.Count)];
                sheeple = new Sheeple(spriteBatch, game, pos, r);
                sheeple.Initialize();
                if (sheeple.data.type == "Bird" && birdsInTheAir > 2)
                {
                    tryAgain = true;
                }
                else if (sheeple.data.type == "Bird")
                {
                    birdsInTheAir++;
                }
            } while (tryAgain);
            
            return sheeple;
        }

        private void CreateListOfAvailablePlatforms()
        {
            PlatformData platform = new PlatformData(game, @"Graphics\Spritesheets\Hus1_sheet", @"Graphics\Collision\Hus1_collision",
                new Vector2(500f, Bombageddon.GROUND - 10f), 50);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Spritesheets\Hus2_sheet", @"Graphics\Collision\Hus2_collision",
                new Vector2(100f, Bombageddon.GROUND - 10f), 50);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Spritesheets\Hus3_sheet", @"Graphics\Collision\Hus3_collision",
                new Vector2(100f, Bombageddon.GROUND - 10f), 50);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Spritesheets\Hus4_sheet", @"Graphics\Collision\Hus4_collision",
                new Vector2(100f, Bombageddon.GROUND - 10f), 50);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Buildings\Sten", @"Graphics\Buildings\Stencollision",
                new Vector2(100f, Bombageddon.GROUND), -1);
            availablePlatforms.Add(platform);

            game.AudioManager.LoadNewEffect("Crash", @"Audio\Sound\Crash\buildingexplosion");
            game.AudioManager.LoadNewEffect("Crash", @"Audio\Sound\Crash\hosedesetruction1");
        }

        private void CreateCivilianData()
        {
            CivilianData civilian = new CivilianData(game, "Man1", 10, "Man");
            civilianData.Add(civilian);
            civilian = new CivilianData(game, "Man2", 10, "Man");
            civilianData.Add(civilian);
            civilian = new CivilianData(game, "Woman1", 10, "Woman");
            civilianData.Add(civilian);

            civilian = new CivilianData(game, "Cow", 10, "Cow");
            civilianData.Add(civilian);
            civilian = new CivilianData(game, "Sheep", 10, "Sheep");
            civilianData.Add(civilian);

            civilian = new CivilianData(game, "Bird1", 50, "Bird");
            civilianData.Add(civilian); 
            civilian = new CivilianData(game, "Bird2", 50, "Bird");
            civilianData.Add(civilian); 
            civilian = new CivilianData(game, "Bird3", 50, "Bird");
            civilianData.Add(civilian);
            civilian = new CivilianData(game, "Bird4", 50, "Bird");
            civilianData.Add(civilian);
            //civilian = new CivilianData(game, "Swan", 50, "Bird");
            //civilianData.Add(civilian);

            game.AudioManager.LoadNewEffect("Man", @"Audio\Sound\Screams\Nej");
            game.AudioManager.LoadNewEffect("Man", @"Audio\Sound\Screams\Skrik1");
            game.AudioManager.LoadNewEffect("Man", @"Audio\Sound\Screams\Skrik2");
            game.AudioManager.LoadNewEffect("Man", @"Audio\Sound\Screams\Skrik4");
            game.AudioManager.LoadNewEffect("Man", @"Audio\Sound\Screams\Skrik5");
            game.AudioManager.LoadNewEffect("Man", @"Audio\Sound\Screams\Skrik6");

            game.AudioManager.LoadNewEffect("Woman", @"Audio\Sound\Screams\Skrik3");

            game.AudioManager.LoadNewEffect("Squish", @"Audio\Sound\Squish\squish1");
            game.AudioManager.LoadNewEffect("Squish", @"Audio\Sound\Squish\squish2");
            game.AudioManager.LoadNewEffect("Squish", @"Audio\Sound\Squish\squish3");
            game.AudioManager.LoadNewEffect("Squish", @"Audio\Sound\Squish\squish4");

            game.AudioManager.LoadNewEffect("Cow", @"Audio\Sound\Animals\ko");
            game.AudioManager.LoadNewEffect("Sheep", @"Audio\Sound\Animals\sheep");
            game.AudioManager.LoadNewEffect("Bird", @"Audio\Sound\Animals\bird");
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
            if (s.data.type == "Bird")
            {
                birdsInTheAir--;
            }
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

            if (!blazeOfGloryStarted)
            {
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
                        Sheeple temp = (Sheeple)e;
                        if (temp.data.type == "Bird")
                        {
                            birdsInTheAir--;
                        }
                        temp = null;
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

            if (player.end && !blazeOfGloryStarted)
            {
                Explosion explosion = new Explosion(game, spriteBatch);
                explosion.Initialize();
                entityList.AddLast(explosion);

                blazeOfGloryStarted = true;
                player.KillMe = true;
            }
            else if (player.end && blazeOfGloryStarted)
            {
                Explosion explosion = (Explosion)findFirstOfType("Explosion").Value;
                if (explosion.killEverything)
                {
                    foreach (Entity e in entityList)
                    {
                        e.ghost = true;
                        if (e.GetType().Name == "Platform")
                        {
                            Platform p = (Platform)e;
                            p.pause = false;
                        }
                        else if (e.GetType().Name == "Sheeple")
                        {
                            Sheeple s = (Sheeple)e;
                            s.IsKilled(false);
                        }
                    }
                    explosion.killEverything = false;
                }
                if (explosion.endGame)
                {
                    blazeOfGloryDone = true;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            backgroundManager.Draw(gameTime, false);

            foreach (Entity entity in entityList)
            {
                entity.Draw(gameTime);
            }

            backgroundManager.Draw(gameTime, true);

            if (blazeOfGloryStarted)
            {
                findFirstOfType("Explosion").Value.Draw(gameTime);
            }
        }

        private void CollisionCheck()
        {
            List<KeyValuePair<int, Vector2>> addThesePoints = new List<KeyValuePair<int, Vector2>>();

            foreach (Entity entity in entityList)
            {
                if (entity.GetType().Name == "Platform")
                {
                    Platform tmpPlat = (Platform)entity;
                    if(!tmpPlat.ghost)
                    {
                        if (collision.BasicCheck(player, tmpPlat))
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
