using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Bombageddon.Code.Entities;

/* Kollisionen fungerar som sådan:
 * 
 * Alla entiteter har två eller flera texturer.
 * En är den som visas, den andra är texturen som används för kollisionshanteringen.
 * 
 * Två entiteter tas in via BasicCheck, i detta fall är det en Player och en Platform.
 * Skär dess SourceRectangles varandra kallas AdvancedCheck automatiskt.
 * I AdvancedCheck skapas en ny rektangel som motsvarar den nya gemensamma ytan.
 * Inom denna rektangel jämförs alla pixlar, och är båda pixlar på samma koordinat röda..
 * BINGO! Kollision!
 * 
 * Beräkningen av vilken sida kollisionen ägde rum görs via beräkning av en vinkel.
 * Vinkeln är mellan den gemensamma rektangelns och plattformens centrum
*/

namespace Bombageddon.Code.Physics
{
    public enum Side
    {
        None = 0x00,
        Top = 0x01,
        Bottom = 0x02,
        Left = 0x04,
        Right = 0x08,
    };

    public class Collision
    {
        public Collision()
        {
        }

        public bool BasicCheck(Entity entity1, Entity entity2)
        {
            if (entity1.SourceRectangle.Intersects(entity2.SourceRectangle))
            {
                return AdvancedCheck(entity1, entity2);
            }
            return false;
        }

        public bool AdvancedCheck(Entity entity1, Entity entity2)
        {
            Rectangle midRect = Rectangle.Intersect(entity1.CollisionRectangle, entity2.CollisionRectangle);

            //Point pos1 = new Point(midRect.Location.X - entity1.CollisionRectangle.Location.X,
            //    midRect.Location.Y - entity1.CollisionRectangle.Location.Y);
            //Point pos2 = new Point(midRect.Location.X - entity2.CollisionRectangle.Location.X,
            //    midRect.Location.Y - entity2.CollisionRectangle.Location.Y);

            int D1x = (int)Vector2.Distance(new Vector2(midRect.Location.X, 0), new Vector2(entity1.CollisionRectangle.Location.X, 0));
            int D1y = (int)Vector2.Distance(new Vector2(0, midRect.Location.Y), new Vector2(0, entity1.CollisionRectangle.Location.Y));

            int D2x = (int)Vector2.Distance(new Vector2(midRect.Location.X, 0), new Vector2(entity2.CollisionRectangle.Location.X, 0));
            int D2y = (int)Vector2.Distance(new Vector2(0, midRect.Location.Y), new Vector2(0, entity2.CollisionRectangle.Location.Y));

            Point pos1 = new Point(D1x, D1y);
            Point pos2 = new Point(D2x, D2y);

            for (int x = 0; x < midRect.Width; x++)
            {
                for (int y = 0; y < midRect.Height; y++)
                {
                    if (entity1.ColorData[x + pos1.X, y + pos1.Y].R > 200 &&
                        entity2.ColorData[x + pos2.X, y + pos2.Y].R > 200)
                        //if (entity1.ColorData[x, y].R > 200 &&
                        //    entity2.ColorData[x, y].R > 200)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Side GetSidesCollided(Entity entity1, Entity entity2)
        {
            //Skapar en ny rektangel i den gemensamma ytan för entiteterna
            Rectangle midRect = Rectangle.Intersect(entity1.SourceRectangle, entity2.SourceRectangle);

            //Kollar vinkeln mellan den nya rektangelns mittpunkt och plattformens centrala bottenpunkt
            //float R = MathHelper.ToDegrees((float)Math.Atan2(entity2.SourceRectangle.Bottom - midRect.Center.Y,
            //   entity2.SourceRectangle.Center.X - midRect.Center.X));

            float R = MathHelper.ToDegrees((float)Math.Atan2(entity2.SourceRectangle.Center.Y - midRect.Center.Y,
               entity2.SourceRectangle.Center.X - midRect.Center.X));

            //C1 och C2 motsvarar de övre motsatta hörnen av plattformens kollisionsyta
            Vector2 C1 = Vector2.Zero;
            //for(int x = 0; x < entity2.HeightMap.Length; x++)
            //{
                //if (entity2.HeightMap[x] != 0)
                //{
                //    C1 = new Vector2(entity2.SourceRectangle.Left + x, entity2.SourceRectangle.Top + entity2.HeightMap[x]);
                //    break;
                //}
                //if (x >= entity2.HeightMap.Length)
                //{
                    C1 = new Vector2(entity2.SourceRectangle.Left, entity2.SourceRectangle.Top);
                //}
            //}

            Vector2 C2 = Vector2.Zero;
            //for (int x = entity2.HeightMap.Length - 1; x >= 0; x--)
            //{
                //if (entity2.HeightMap[x] != 0)
                //{
                //    C2 = new Vector2(entity2.SourceRectangle.Right + x, entity2.SourceRectangle.Top + entity2.HeightMap[x]);
                //    break;
                //}
                //if (x < 1)
                //{
                    C2 = new Vector2(entity2.SourceRectangle.Right, entity2.SourceRectangle.Top);
                //}
            //}

            //K1 är helt enkelt plattformens centrala bottenpunkt
            Vector2 K1 = new Vector2(entity2.SourceRectangle.Center.X, entity2.SourceRectangle.Bottom);
            //Vector2 K2 = new Vector2(entity2.SourceRectangle.Left, entity2.SourceRectangle.Top);
            //Vector2 K3 = new Vector2(entity2.SourceRectangle.Right, entity2.SourceRectangle.Top);

            //R1 och R2 motsvarar en vinkel av en linje som går från plattformens centrala bottenpunkt
            //till vardera övre kant.
            float R1 = MathHelper.ToDegrees((float)Math.Atan2(K1.Y - C1.Y, K1.X - C1.X));
            float R2 = MathHelper.ToDegrees((float)Math.Atan2(K1.Y - C2.Y, K1.X - C2.X));
            
            Side returnVal = Side.None;

            //Faller spelarens infallsvinkel mittemellan värdena för de två andra vinklarna är det en toppkollision
            //Top
            if (R > R1 && R < R2)
            {
                returnVal = (returnVal | Side.Top);
            }

            //Är spelarens infallsvinkel mindre än den vänstra plattformsvinkeln är det en vänsterkollision
            //Left
            if (R < R1)
            {
                returnVal = (returnVal | Side.Left);
            }

            ////Right
            //if (R < 135 && R > 45)
            //{
            //    returnVal = (returnVal | Side.Right);
            //}

            ////Bottom
            //if (R < 225 && R > 135)
            //{
            //    returnVal = (returnVal | Side.Bottom);
            //}

            return returnVal;
        } 
    }
}
