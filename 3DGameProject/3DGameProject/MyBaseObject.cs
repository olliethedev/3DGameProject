using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DGameProject
{
    class MyBaseObject : IUpdatable, IDrawable
    {
        protected Vector3 position;
        protected Vector3 positionOld;
        protected Vector3 velocity;
        protected Model model;
        protected Texture2D texture;
        protected BasicEffect shader;
        protected GraphicsDevice g;
        protected Matrix scale;
 
        public MyBaseObject(Vector3 position, Vector3 velocity, Model model, Matrix scale, BasicEffect shader, GraphicsDevice g)
        {
            this.position = position;
            this.velocity = velocity;
            this.model = model;
            this.shader = shader;
            this.g = g;
            this.scale = scale;
        }
        public void setTexture(Texture2D texture)
        {
            this.texture = texture;
        }
        public void onDraw(Matrix view)
        {
            if (texture == null)
            {
                shader.TextureEnabled = false;
            }
            else
            {
                shader.TextureEnabled = true;
                shader.Texture = texture;
            }
            shader.View = view;
            Matrix world = Matrix.Identity * Matrix.CreateTranslation(position)*scale;
            foreach (ModelMesh mm in model.Meshes)
            {
                foreach (ModelMeshPart mmp in mm.MeshParts)
                {
                    shader.World = world;
                    g.SetVertexBuffer(mmp.VertexBuffer, mmp.VertexOffset);
                    g.Indices = mmp.IndexBuffer;
                    shader.CurrentTechnique.Passes[0].Apply();
                    g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mmp.NumVertices, mmp.StartIndex, mmp.PrimitiveCount);
                }
        }
        }

        public void onUpdate()
        {
            positionOld = position;
            position += velocity;            
        }

    }
}
