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

        Sheeple tempSheeple;
        Platform tempPlatform;
        LinkedListNode<Entity> temp;

        int birdsInTheAir = 0;
        int currentBuildingLevel = 25;

        Vector2 startingIntervals = new Vector2(200, 350);
        Vector2 sheepleRandomInterval;
        Vector2 platformRandomInterval;
        public float playerKineticMultiplier = 1f;

        Collision collision;

        public Player player;
        public LinkedList<Entity> entityList = new LinkedList<Entity>();
        Random random = new Random();

        //Dictionary<String, String> platformFiles = new Dictionary<String, String>();

        //public Platform platform = null;
        List<PlatformData> availablePlatforms = new List<PlatformData>();
        List<CivilianData> civilianData = new List<CivilianData>();

        Dictionary<int, Texture2D> pointTextures = new Dictionary<int, Texture2D>();
        List<KeyValuePair<int, Vector2>> addThesePoints = new List<KeyValuePair<int, Vector2>>();

        private List<Entity> removeList = new List<Entity>();

        Vector2 pos;

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

            player = new Player(game, spriteBatch, new Vector2(0f, Bombageddon.GROUND - 200f));
            player.Initialize();
            entityList.AddLast(player);

            CreateListOfAvailablePlatforms();
            entityList.AddLast(new Platform(game, spriteBatch, availablePlatforms[5]));
            //for (int i = 0; i < 5; i++)
            //{
            //    entityList.AddLast(addPlatform());
            //}

            //temp.Terminate();

            CreateCivilianData();
            pos = new Vector2(player.position.X + 100f, Bombageddon.GROUND - 8);
            tempSheeple = new Sheeple(spriteBatch, game, pos, civilianData[0]);
            tempSheeple.Initialize();
            entityList.AddLast(tempSheeple);
            //for (int i = 0; i < 30; i++)
            //{
            //    entityList.AddLast(addSheeple());
            //}

            //civ.Terminate();

            CreateListOfPoints();

            birdsInTheAir = 0;
        }

        private void CreateListOfPoints()
        {
            pointTextures.Add(10, game.Content.Load<Texture2D>(@"Graphics\Points\10"));
            pointTextures.Add(25, game.Content.Load<Texture2D>(@"Graphics\Points\25"));
            pointTextures.Add(50, game.Content.Load<Texture2D>(@"Graphics\Points\50"));
            pointTextures.Add(75, game.Content.Load<Texture2D>(@"Graphics\Points\75"));
            pointTextures.Add(100, game.Content.Load<Texture2D>(@"Graphics\Points\100"));

            pointTextures.Add(-5, game.Content.Load<Texture2D>(@"Graphics\Points\x5"));

            pointTextures.Add(-10, game.Content.Load<Texture2D>(@"Graphics\Points\+10"));

            game.AudioManager.LoadNewEffect("Point", @"Audio\Sound\Points\point1");
            game.AudioManager.LoadNewEffect("Point", @"Audio\Sound\Points\point2");
            game.AudioManager.LoadNewEffect("Point", @"Audio\Sound\Points\point3");
        }

        private Sheeple addSheeple()
        {
            bool tryAgain;

            do
            {
                tryAgain = false;
                Sheeple lastSheeple = (Sheeple)findLastOfType("Sheeple").Value;
                pos = new Vector2(lastSheeple.position.X + random.Next((int)sheepleRandomInterval.X, (int)sheepleRandomInterval.Y), Bombageddon.GROUND - 8);
                CivilianData r = civilianData[random.Next(civilianData.Count)];
                tempSheeple = new Sheeple(spriteBatch, game, pos, r);
                tempSheeple.Initialize();
                if (tempSheeple.data.type == "Bird" && birdsInTheAir > 2)
                {
                    tryAgain = true;
                }
                else if (tempSheeple.data.type == "Bird")
                {
                    birdsInTheAir++;
                }
            } while (tryAgain);

            return tempSheeple;
        }

        private void CreateListOfAvailablePlatforms()
        {
            PlatformData platform = new PlatformData(game, @"Graphics\Spritesheets\Hus1_sheet", @"Graphics\Collision\Hus1_collision",
                new Vector2(100f, Bombageddon.GROUND - 10f), 50, 1500);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Spritesheets\Hus2_sheet", @"Graphics\Collision\Hus2_collision",
                new Vector2(100f, Bombageddon.GROUND - 10f), 50, 2000);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Spritesheets\Hus3_sheet", @"Graphics\Collision\Hus3_collision",
                new Vector2(100f, Bombageddon.GROUND - 10f), 75, 8000);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Spritesheets\Hus4_sheet", @"Graphics\Collision\Hus4_collision",
                new Vector2(100f, Bombageddon.GROUND - 10f), 75, 7000);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Spritesheets\Hus5_sheet", @"Graphics\Collision\Hus5_collision",
                new Vector2(100f, Bombageddon.GROUND - 10f), 100, 12000);
            availablePlatforms.Add(platform);

            platform = new PlatformData(game, @"Graphics\Spritesheets\Hus6_sheet", @"Graphics\Collision\Hus6_collision",
                new Vector2(200f, Bombageddon.GROUND - 10f), 25, 500);
            availablePlatforms.Add(platform);

            //platform = new PlatformData(game, @"Graphics\Buildings\Sten", @"Graphics\Buildings\Stencollision",
            //    new Vector2(100f, Bombageddon.GROUND), -1, 1000000);
            //availablePlatforms.Add(platform);

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
            civilian = new CivilianData(game, "Horse", 10, "Horse");
            civilianData.Add(civilian);

            civilian = new CivilianData(game, "Bird1", 25, "Bird");
            civilianData.Add(civilian); 
            civilian = new CivilianData(game, "Bird2", 25, "Bird");
            civilianData.Add(civilian); 
            civilian = new CivilianData(game, "Bird3", 25, "Bird");
            civilianData.Add(civilian);
            civilian = new CivilianData(game, "Bird4", 25, "Bird");
            civilianData.Add(civilian);
            civilian = new CivilianData(game, "Swan", 25, "Bird");
            civilianData.Add(civilian);

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
            game.AudioManager.LoadNewEffect("Horse", @"Audio\Sound\Animals\horse");
        }

        private LinkedListNode<Entity> findFirstOfType(String type)
        {
            temp = entityList.First;
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
            temp = entityList.Last;
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
            tempSheeple = (Sheeple)findFirstOfType("Sheeple").Value;
            if (tempSheeple.data.type == "Bird")
            {
                birdsInTheAir--;
            }
            tempSheeple.Terminate();
            entityList.Remove(tempSheeple);
            if (findLastOfType("Sheeple").Value.position.X < player.position.X + Bombageddon.WIDTH * 1f)
            {
                entityList.AddLast(addSheeple());
            }
        }

        private void refreshPlatforms()
        {
            findFirstOfType("Platform").Value.Terminate();
            entityList.Remove(findFirstOfType("Platform").Value);
            if (findLastOfType("Platform").Value.position.X < player.position.X + Bombageddon.WIDTH * 1f)
            {
                entityList.AddLast(addPlatform());
            }
        }

        private Platform addPlatform()
        {
            bool tooHard;
            int randomPlatform;
            float posX;
            do
            {
                tooHard = false;
                tempPlatform = (Platform)findLastOfType("Platform").Value;
                posX = tempPlatform.position.X + random.Next((int)platformRandomInterval.X, (int)platformRandomInterval.Y);
                randomPlatform = random.Next(availablePlatforms.Count);
                if (availablePlatforms[randomPlatform].points > currentBuildingLevel
                    || (availablePlatforms[randomPlatform].points == 25 && player.points > 500))
                {
                    tooHard = true;
                }
            } while (tooHard);

            tempPlatform = new Platform(game, spriteBatch, availablePlatforms[randomPlatform]);
            tempPlatform.Initialize();
            tempPlatform.position.X = posX;

            return tempPlatform;
        }

        public void Terminate()
        {
            //game.Content.Unload();

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
            //foreach (Texture2D p in pointTextures.Values)
            //{
            //    p.Dispose();
            //    //p = null;
            //}
            pointTextures.Clear();
            entityList.Clear();
            availablePlatforms.Clear();
            civilianData.Clear();
            pointTextures.Clear();
            //player = null;
            backgroundManager.Terminate();
        }

        public void Update(GameTime gameTime)
        {
            int multiplier = 7;
            if (player.points > 200)
            {
                //multiplier--;
                currentBuildingLevel = 50;
            }
            if (player.points > 500)
            {
                multiplier--;
            }
            if (player.points > 600)
            {
                //multiplier--;
            }
            if (player.points > 1000)
            {
                multiplier--;
                currentBuildingLevel = 75;
            }
            if (player.points > 1500)
            {
                multiplier--;
            }
            if (player.points > 2000)
            {
                //multiplier--;
                currentBuildingLevel = 100;
            }
            if (player.points > 2500)
            {
                multiplier--;
            }
            if (player.points > 4500)
            {
                multiplier--;
            }

            platformRandomInterval = startingIntervals * multiplier * 1f;
            sheepleRandomInterval = startingIntervals * multiplier * 0.2f;
            if (player.points < 1000)
            {
                sheepleRandomInterval = startingIntervals * multiplier * 0.1f;
            }
            playerKineticMultiplier = 1f + player.points * 0.0007f;
            MathHelper.Clamp(playerKineticMultiplier, 1f, 4f);

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
                backgroundManager.Update(gameTime);

                foreach (Entity e in removeList)
                {
                    e.Terminate();
                    entityList.Remove(e);
                    //if (e.GetType().ToString().Equals("Platform"))
                    //{
                    //    entityList.AddLast(addPlatform());
                    //}
                    if (e.GetType().ToString().Equals("Sheeple"))
                    {
                        if (findLastOfType("Sheeple").Value.position.X < player.position.X + Bombageddon.WIDTH * 0.7f)
                        {
                            entityList.AddLast(addSheeple());
                        }
                        tempSheeple = (Sheeple)e;
                        if (tempSheeple.data.type == "Bird")
                        {
                            birdsInTheAir--;
                        }
                    }
                }

                removeList.Clear();

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

                if (findFirstOfType("Platform").Value.position.X < player.position.X - Bombageddon.WIDTH)
                {
                    refreshPlatforms();
                }

                if (findFirstOfType("Sheeple").Value.position.X < player.position.X - Bombageddon.WIDTH)
                {
                    refreshSheeples();
                }                

                if (findLastOfType("Sheeple").Value.position.X < player.position.X + Bombageddon.WIDTH * 1f)
                {
                    entityList.AddLast(addSheeple());
                } 
                
                if (findLastOfType("Platform").Value.position.X < player.position.X + Bombageddon.WIDTH * 1f)
                {
                    entityList.AddLast(addPlatform());
                }

                CollisionCheck();
            }

            if (player.end && !blazeOfGloryStarted)
            {
                Explosion explosion = new Explosion(game, spriteBatch);
                explosion.Initialize();
                entityList.AddLast(explosion);

                blazeOfGloryStarted = true;
                //player.Terminate();
                entityList.Remove(player);
            }
            else if (player.end && blazeOfGloryStarted)
            {
                Explosion explosion = (Explosion)findFirstOfType("Explosion").Value;
                if (explosion.killEverything)
                {
                    foreach (Entity e in entityList)
                    {
                        if (e.position.X > player.position.X - 500f && e.position.X < player.position.X + 500f)
                        {
                            Boolean points = false;
                            if (e.GetType().Name == "Platform")
                            {
                                tempPlatform = (Platform)e;
                                tempPlatform.pause = false;
                                points = true;
                            }
                            else if (e.GetType().Name == "Sheeple")
                            {
                                tempSheeple = (Sheeple)e;
                                tempSheeple.IsKilled(false);
                                points = true;
                            }
                            if (points && !e.ghost && e.pointsWorth != -1)
                            {
                                player.points += e.pointsWorth * 5;
                                addThesePoints.Add(new KeyValuePair<int, Vector2>(e.pointsWorth, e.position));
                                addThesePoints.Add(new KeyValuePair<int, Vector2>(-5, e.position + new Vector2(-20, 20)));
                            }
                            e.ghost = true;
                        }
                    }
                    explosion.killEverything = false;
                }
                if (explosion.endGame)
                {
                    blazeOfGloryDone = true;
                }
            }
            
            foreach (KeyValuePair<int, Vector2> p in addThesePoints)
            {
                entityList.AddLast(new FlyingPoint(spriteBatch, game, pointTextures[p.Key], p.Value));
            }
            addThesePoints.Clear();
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
            foreach (Entity entity in entityList)
            {
                if (entity.GetType().Name == "Platform")
                {
                    tempPlatform = (Platform)entity;
                    if (!tempPlatform.ghost)
                    {
                        if (collision.BasicCheck(player, tempPlatform))
                        {
                            if (tempPlatform.pointsWorth > 0)
                            {
                                if (tempPlatform.life < 0)
                                {
                                    tempPlatform.pause = false;
                                    tempPlatform.ghost = true;
                                    player.kinetics *= 0.9f;
                                    if ((tempPlatform.pointsWorth > 50 && currentBuildingLevel >= 75) || currentBuildingLevel < 75)
                                    {
                                        player.kinetics *= 0.3f;
                                    }
                                    player.points += tempPlatform.pointsWorth;
                                    game.AudioManager.PlayEffect("Crash");
                                    addThesePoints.Add(new KeyValuePair<int, Vector2>(tempPlatform.pointsWorth, tempPlatform.position));
                                }
                                else
                                {
                                    tempPlatform.life -= (int)Math.Abs(player.kinetics.X * playerKineticMultiplier);
                                    tempPlatform.life -= (int)Math.Abs(player.kinetics.Y * playerKineticMultiplier);
                                    player.kinetics *= -0.3f;
                                    float move = 3f;
                                    if (player.kinetics.X > 0)
                                    {
                                        move *= -1f;
                                    }
                                    player.position += new Vector2(move);
                                }
                            }
                            else
                            {
                                //player.FuseTimer -= 1000;
                                tempPlatform.ghost = true;
                                player.kinetics *= -0.3f;
                            }
                        }
                        //return;
                    }
                }
                if (entity.GetType().Name == "Sheeple")
                {
                    tempSheeple = (Sheeple)entity;
                    if (!tempSheeple.ghost)
                    {
                        if (collision.BasicCheck(player, tempSheeple))
                        {
                            if (collision.GetSidesCollided(player, tempSheeple) == Side.Top)
                            {
                                tempSheeple.IsKilled(true);
                                //addThesePoints.Add(new KeyValuePair<int, Vector2>(5, tempSheeple.position - new Vector2(20f)));    //BONUS
                            }
                            else
                            {
                                tempSheeple.IsKilled(false);
                            }

                            player.points += tempSheeple.pointsWorth;
                            tempSheeple.ghost = true;
                            addThisManySheeples++;
                            addThesePoints.Add(new KeyValuePair<int, Vector2>(tempSheeple.pointsWorth, tempSheeple.position));
                            if (tempSheeple.data.type == "Bird")
                            {
                                addThesePoints.Add(new KeyValuePair<int, Vector2>(-10, tempSheeple.position + new Vector2(-10, 30)));
                                player.FuseTimer += 5000;
                            }
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
