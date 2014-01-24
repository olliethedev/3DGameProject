using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace _3DGameProject
{
    class MyKeyBoard
    {
        public MyKeyBoard()
        {
        }
        public Vector3 onUpdateGetPositionVector()
        {
            KeyboardState ks = Keyboard.GetState();
            Vector3 moveVec = new Vector3();
            if (ks.IsKeyDown(Keys.W))
            {
                moveVec += new Vector3(0, 0, -1);
            }
            if (ks.IsKeyDown(Keys.S))
            {
                moveVec += new Vector3(0, 0, 1);
            }
            if (ks.IsKeyDown(Keys.A))
            {
                moveVec += new Vector3(-1, 0, 0);
            }
            if (ks.IsKeyDown(Keys.D))
            {
                moveVec += new Vector3(1, 0, 0);
            }
            return moveVec;
        }
    }
}
