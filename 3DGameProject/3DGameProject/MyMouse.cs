using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace _3DGameProject
{
    
    class MyMouse
    {
        MouseState ms ;
        Vector2 resetPoint;
        public bool clicked =false;
        public MyMouse(Vector2 resetPoint)
        {
            this.resetPoint = resetPoint;
            Mouse.SetPosition((int)resetPoint.X, (int)resetPoint.Y);
            ms = Mouse.GetState();            
        }
        public Vector2 onUpdateGetDeltas()
        {
            //calc for camera
            MouseState newMs = Mouse.GetState();
            Vector2 result = new Vector2(resetPoint.X- newMs.X, resetPoint.Y - newMs.Y);
            ms = newMs;
            Mouse.SetPosition((int)resetPoint.X, (int)resetPoint.Y);

            //click
            if (ButtonState.Pressed == newMs.LeftButton)
            {
                clicked = true;
            }
            else
            {
                clicked =false;
            }
            
            return result;
        }
    }
}
