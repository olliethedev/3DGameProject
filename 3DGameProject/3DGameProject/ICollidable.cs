using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace _3DGameProject
{
    interface ICollidable
    {
        
        void onCollisionCheck(ICollidable otherObj);
        Model getModel();
        Matrix getMatrix();
        Vector3 getVelocity();
        void setVelocity(Vector3 velocity);
        float getMass();


    }
}
