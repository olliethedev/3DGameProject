using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _3DGameProject
{
    class MyCamera
    {
        public Vector3 position;
        public Vector3 target;
        public Vector3 up = Vector3.Up;
        float  rotY= -80;
        float rotX =0;
        public MyCamera(Vector3 target, Vector3 position)
        {
            this.position = position;
            this.target = target;
        }
        public void onUpdate(float dX, float dY, Vector3 move, float speed)
        {
            rotX += dX * speed;
             rotY += dY * speed;
            Console.Out.WriteLine("rotx:" + rotX + " roty:" + rotY);
            /*if (rotX>360||rotX<-360)
            {
                rotX = 0;
            }
            if (rotY > 90)
            {
                rotY = 90;
            }
            if (rotY < -90)
            {
                rotY = -90;
            }*/
            Matrix rM = Matrix.CreateRotationY(MathHelper.ToRadians(rotX))* Matrix.CreateRotationX(MathHelper.ToRadians(rotY));
            Vector3 rotatedVector = Vector3.Transform(move, rM);
            position +=  rotatedVector;
            target = Vector3.Transform(Vector3.Forward, rM) + position;
            up = Vector3.Transform(Vector3.Up, rM);
        }
    }
}
