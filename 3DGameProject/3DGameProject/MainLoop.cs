/*
 * 3d game project by Marwa, Olga, Philip, Oleksiy
 * Instruction:
 * Movements keys: "A","D","S","W"
 * Use mouse to aim.
 * Left mouse button to shoot.
 * Goal of the game is to shoot down all the boxes.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace _3DGameProject
{
    public class MainLoop : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<MyBaseObject> gameObjList = new List<MyBaseObject>();
        List<ICollidable> gameCollidables = new List<ICollidable>();
        BasicEffect shader;
        MyCamera camera;
        MyMouse mouse;
        MyKeyBoard keyboard;
        readonly float speed = 0.05f;
        Matrix viewMatrix;
        bool canFire = true;
        RequestDelete rd;
        private readonly float farPlane = 123;
        int levelCount = 1;

        public MainLoop()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            rd = new RequestDelete(deleteGameObj);
            base.Initialize();
        }
        private void deleteGameObj(MyBaseObject obj)
        {
            
            if (obj is ICollidable )
            {
                Console.WriteLine("colidable");
                gameCollidables.Remove((ICollidable)obj);
            }
            gameObjList.Remove(obj);
            if (gameCollidables.Count==1)
            {
                levelCount++;
                initGrid(levelCount);
            }
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            shader = new BasicEffect(GraphicsDevice);
            camera = new MyCamera(new Vector3(5, 0, 5), new Vector3(5, 30, 10));
            int midX = GraphicsDevice.Viewport.Width / 2;
            int midY = GraphicsDevice.Viewport.Height / 2;
            mouse = new MyMouse(new Vector2(midX,midY));
            keyboard = new MyKeyBoard();
            DefaultEffectsSettings();
           
            initGrid(levelCount);            
        }
        private void initGrid(int level)
        {

            Model boxM = Content.Load<Model>("box");
            Texture2D boxT = Content.Load<Texture2D>("boxtexture");
            float space = level;
            for (int x = 0; x < level; x++)
            {
                for (int y = 0; y < level; y++)
                {
                    MyPhysicalObject box = new MyPhysicalObject(new Vector3(x * space, 0, y * space), new Vector3(0, 0, 0), boxM, Matrix.Identity, shader, GraphicsDevice, 1, farPlane);
                    box.setDeleteFunction(rd);
                    gameObjList.Add(box);
                    gameCollidables.Add(box);
                    
                }
            }
        }
        private void initGameObjs() // not used
        {

            Model boxM = Content.Load<Model>("box");
            Texture2D boxT = Content.Load<Texture2D>("boxtexture");
            MyPhysicalObject box = new MyPhysicalObject(new Vector3(-4, 0.2f, 0), new Vector3(0, 0, 0), boxM, Matrix.Identity, shader, GraphicsDevice, 2, farPlane);
            box.setTexture(boxT);
            gameObjList.Add(box);
            gameCollidables.Add(box);


            Model boxMTwo = Content.Load<Model>("box");
            Texture2D boxTTwo = Content.Load<Texture2D>("boxtexture");
            MyPhysicalObject boxTwo = new MyPhysicalObject(new Vector3(8, -0.2f, 0), new Vector3(0, 0, 0), boxM, Matrix.Identity, shader, GraphicsDevice, 2, farPlane);
            box.setTexture(boxTTwo);
            gameObjList.Add(boxTwo);
            gameCollidables.Add(boxTwo);

            Model sphereM = Content.Load<Model>("sphere");
            MyPhysicalObject sphere = new MyPhysicalObject(new Vector3(4, 0, 0), new Vector3(0, 0, 0), sphereM, Matrix.Identity, shader, GraphicsDevice, 2, farPlane);
            gameObjList.Add(sphere);
            gameCollidables.Add(sphere);

            Model levelM = Content.Load<Model>("level1_withtexture");
            Texture2D levelT = Content.Load<Texture2D>("brickTexture");
            MyPhysicalObject level = new MyPhysicalObject(new Vector3(-4, 0, -2), new Vector3(0, 0, 0), levelM, Matrix.CreateScale(8.0f, 8.0f, 1.0f), shader, GraphicsDevice, 2, farPlane);
            level.setTexture(levelT);
            gameObjList.Add(level);
            gameCollidables.Add(level);
        }
        private void DefaultEffectsSettings()
        {
            shader.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, farPlane);
            
            shader.LightingEnabled = true;
            shader.AmbientLightColor = Color.Gray.ToVector3();

            shader.DirectionalLight0.Enabled = true;
            shader.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            //shader.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(0f, -1f, -0.5f));

            shader.DirectionalLight0.SpecularColor = Color.White.ToVector3();
            shader.PreferPerPixelLighting = true;
        }
        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            Vector3 camMove = keyboard.onUpdateGetPositionVector();
            Vector2 camDeltas = mouse.onUpdateGetDeltas();
            camera.onUpdate(camDeltas.X, camDeltas.Y, camMove, speed);
            viewMatrix = Matrix.CreateLookAt(camera.position, camera.target, camera.up);
            foreach (MyBaseObject baseObj in gameObjList)
            {
                baseObj.onUpdate();
            }
            List<ICollidable> tempList = new List<ICollidable>(gameCollidables);
            for (int a = tempList.Count - 1; a >= 0; a--)
            {
                for (int b = tempList.Count - 1; b >= 0; b--)
                {
                    if (a!=b)
                    {
                        tempList[a].onCollisionCheck(tempList[b]);
                    }
                }
            }
            if (mouse.clicked)
            {
                if (canFire)
                {
                    Model sphereM = Content.Load<Model>("sphere");
                    Vector3 vel = Vector3.Transform(camera.target, Matrix.CreateScale(0.2f));
                    MyPhysicalObject sphere = new MyPhysicalObject(camera.target, camera.target - camera.position, sphereM, Matrix.Identity, shader, GraphicsDevice, 2, farPlane);
                    sphere.setDeleteFunction(rd);
                    gameObjList.Add(sphere);
                    gameCollidables.Add(sphere);
                }                
                canFire = false;
            }
            else
            {
                canFire = true;
            }
            shader.DirectionalLight0.Direction = Vector3.Normalize(camera.target - camera.position);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (MyBaseObject baseObj in gameObjList)
            {
                baseObj.onDraw(viewMatrix);
            }

            base.Draw(gameTime);
        }
    }
}
