using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DGameProject
{
    class MyPhysicalObject : MyBaseObject, ICollidable, IDeletable
    {
        private float mass;
        private RequestDelete delete;
        private float farPlane;
        public MyPhysicalObject(Vector3 position, Vector3 velocity, Model model, Matrix scale, BasicEffect shader, GraphicsDevice g, float mass, float farPlane)
            : base(position, velocity, model, scale, shader, g)
        {
            this.mass = mass;
            this.farPlane = farPlane;
        }
  

       

        public void onCollisionCheck(ICollidable otherObj)
        {
            if (Math.Abs(this.position.X) > farPlane || Math.Abs(this.position.Y) > farPlane || Math.Abs(this.position.Z) > farPlane)
            {
                //Console.WriteLine("outside of far plane");
                onDelete();
            }
            else
            {
                Model mObj = otherObj.getModel();
                Matrix tramsform = otherObj.getMatrix();
                Matrix world = Matrix.Identity * Matrix.CreateTranslation(position) * scale;
                foreach (ModelMesh mm in model.Meshes)
                {
                    foreach (ModelMesh mObjMm in mObj.Meshes)
                    {
                        BoundingSphere myBs = mm.BoundingSphere.Transform(Matrix.Identity * Matrix.CreateTranslation(position) * scale);
                        BoundingSphere otherBs = mObjMm.BoundingSphere.Transform(tramsform);
                        if (myBs.Intersects(otherBs))
                        {
                            //applyCollision(this,otherObj,myBs.Center,otherBs.Center);
                            //position = positionOld;
                            //Vector3 tempV = this.getVelocity();
                            //this.setVelocity(otherObj.getVelocity());
                            //otherObj.setVelocity(tempV);
                            onDelete();
                        }
                    }

                }
            }
            
        }
        public Model getModel()
        {
            return model;
        }

        public Matrix getMatrix()
        {
            return Matrix.Identity * Matrix.CreateTranslation(position) * scale;
        }


        public void setVelocity(Vector3 velocity)
        {
            this.velocity = velocity;
        }


        public Vector3 getVelocity()
        {
            return this.velocity;
        }
        private void applyCollision(ICollidable us, ICollidable them, Vector3 centerUs, Vector3 centerThem)
        {
            Vector3 old1 = us.getVelocity();
            Vector3 old2 = them.getVelocity();

            Vector3 normalPlane = Vector3.Normalize(centerThem - centerUs);

            float nvel1 = Vector3.Dot(normalPlane, old1);
            float nvel2 = Vector3.Dot(normalPlane, old2);


            Vector3 refl1 = Vector3.Reflect(old1, normalPlane);
            //refl1.Normalize();
            Vector3 refl2 = Vector3.Reflect(old2, normalPlane);
            //refl2.Normalize();
            us.setVelocity( refl1 * ((us.getMass() - them.getMass()) + 2 * them.getMass()) / (us.getMass() + them.getMass()));
            them.setVelocity(refl2 * ((them.getMass() - us.getMass()) + 2 * us.getMass()) / (us.getMass() + them.getMass()));
       }


        public float getMass()
        {
            return this.mass;
        }

        public void setDeleteFunction(RequestDelete rd)
        {
            delete = rd;
        }

        public void onDelete()
        {
            if (delete != null)
            {
                delete(this);
            }            
        }
    }
}
