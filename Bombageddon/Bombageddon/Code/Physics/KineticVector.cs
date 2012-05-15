using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bombageddon.Code.Physics
{
    class KineticVector
    {
        Vector2 K1, K2, finalVector, F;
        //float R, D;

        public MouseState OriginalMouseState
        {
            get;
            set;
        }

        public Vector2 FinalVector
        {
            get { return finalVector; }
        }

        public KineticVector()
        {
            OriginalMouseState = Mouse.GetState();
            K1.X = OriginalMouseState.X;
            K1.Y = OriginalMouseState.Y;
        }

        public Vector2 CalculateKineticVector()
        {
            //D = Vector2.Distance(K1, K2);

            //R = (float)Math.Atan2(K2.Y - K1.Y, K2.X - K1.X);

            F = K2 - K1;

            return F;
        }

        public bool ReceivedInput(int snapshot, MouseState currentMouse)
        {
            bool calculated = false;
            if (snapshot == 1)
            {
                K2.X = currentMouse.X;
                K2.Y = currentMouse.Y;
            }
            if (snapshot == 2)
            {
                finalVector = Vector2.Zero;
                finalVector = CalculateKineticVector();
                calculated = true;
            }
            return calculated;
        }
    }
}