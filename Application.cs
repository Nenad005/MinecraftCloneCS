using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;
using OpenTK.Mathematics;
using System.Diagnostics;

namespace BasicOpenTK
{
    public class Application : GameWindow
    {
        private Shaders sp;
        private static Bitmap TexBMP = new Bitmap("./Assets/atlas.png");
        private static System.Drawing.Imaging.BitmapData ?TexData;
        private static float step = 0.1f; 
        private static Vector2 mouse = new Vector2(0f,0f);
        private FirstPersonCamera camera = new FirstPersonCamera(0, 64, 0);
        private float FOV = 90;
        private bool drawLines = false;
        public Vector2 Screen = new Vector2(1920f, 1080f);
        public static int[,,] positionsT = new int[16, 16, 16];
        public static Tuple<float[], uint[]> chunk;



        public Application(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws)
        {
            Screen = nws.Size;
            this.CenterWindow();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            Screen.X = e.Width;
            Screen.Y = e.Height;
            GL.Viewport(0, 0, e.Width, e.Height);
            Console.WriteLine($"Resize : W : {e.Width}, H : {e.Height}");
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            CursorGrabbed = true;
            GL.Enable(EnableCap.DepthTest);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);


            IsVisible = true;


            sp = new Shaders("./Render/glsl/vertex_shader.glsl", "./Render/glsl/fragment_shader.glsl");
            TexData = TexBMP.LockBits(new Rectangle(0, 0, TexBMP.Width, TexBMP.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            // Random rnd = new Random();


            // for(int x = 0; x < positionsT.GetLength(0); x++)
            // {
            //     for(int y = 0; y < positionsT.GetLength(1); y++)
            //     {
            //         for(int z = 0; z < positionsT.GetLength(2); z++)
            //         {
            //             positionsT[x, y, z] = 1;
            //         }
            //     }
            // }


            // Stopwatch sw = new Stopwatch();
            // sw.Start();
            // verts = VertexCalc.GetVertices3D(positionsT);
            // sw.Stop();
            // Indices = VertexCalc.GetIndices(verts);
            // Console.WriteLine($"{positionsT.GetLength(0)*positionsT.GetLength(1)*positionsT.GetLength(2)} blokova se renderovalo za {sw.ElapsedMilliseconds}ms.");

            GameSettings blaS = new GameSettings();
            blaS.RenderDistance = 12;
            Game bla = new Game(blaS);
            // bla.Print();
            
            chunk = bla.chunksData[new Vector2(0,0)];
            base.OnLoad();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Escape) //nevidljivi mis pali-gasi
            {
                CursorVisible = CursorVisible ? false : true;
                CursorGrabbed = !CursorVisible;
            }
            if (e.Key == Keys.G) //wireframe render pali-gasi
            {
                drawLines = !drawLines;
                
                if (drawLines) GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                else GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
            if (e.Key == Keys.T)
            {
                camera.SetTP(positionsT.GetLength(0)/2, positionsT.GetLength(1)+1, positionsT.GetLength(2)/2);
                // camera.SetTP();
            }
            if (e.Key == Keys.P)
            {
                camera.TP();
            }
            if (e.Key == Keys.H)
            {
                camera.PrintPos();
            }
            base.OnKeyDown(e);
        }

        protected override void OnUnload()
        {
            TexBMP.UnlockBits(TexData);
            base.OnUnload();
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouse.X = e.X;
            mouse.Y = e.Y;
            base.OnMouseMove(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            camera.SetLookDirection(mouse);
            if (KeyboardState.IsKeyDown(Keys.W))         camera.move(0, 0, step);
            if (KeyboardState.IsKeyDown(Keys.S))         camera.move(0, 0, -step);
            if (KeyboardState.IsKeyDown(Keys.D))         camera.move(step, 0, 0);
            if (KeyboardState.IsKeyDown(Keys.A))         camera.move(-step, 0, 0);
            if (KeyboardState.IsKeyDown(Keys.Space))     camera.move(0, step, 0);
            if (KeyboardState.IsKeyDown(Keys.LeftShift)) camera.move(0, -step, 0);
            if (KeyboardState.IsKeyDown(Keys.KeyPadAdd) && FOV < 174.9f)      FOV += 0.5f;
            if (KeyboardState.IsKeyDown(Keys.KeyPadSubtract) && FOV > 0.6f)   FOV -= 0.5f;

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            sp.Activate();

            GL.ClearColor(0.65f,0.65f,1f, 0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            // float[] verts = new float[]
            // {
            //     0f, 1f, 1f,     0f, 0f, //0 gore levo             PREDNJA
            //     1f, 1f, 1f,     0f, 1f, //0 5ore desno
            //     0f, 0f, 1f,     0f, 2f, //2 dole levo
            //     1f, 0f, 1f,     0f, 3f, //3 dole desno

            //     1f, 1f, 1f,     5f, 0f, //4 gore levo             DESNA
            //     1f, 1f, 0f,     5f, 1f, //5 gore desno
            //     1f, 0f, 1f,     5f, 2f, //6 dole levo
            //     1f, 0f, 0f,     5f, 3f, //7 dole desno

            //     0f, 1f, 0f,     5f, 1f, //8 gore levo             ZADNJE
            //     1f, 1f, 0f,     5f, 0f, //9 gore desno
            //     0f, 0f, 0f,     5f, 3f, //10 dole levo
            //     1f, 0f, 0f,     5f, 2f, //11 dole desno

            //     0f, 1f, 1f,     5f, 1f, //12 gore levo            LEVA
            //     0f, 1f, 0f,     5f, 0f, //13 gore desno
            //     0f, 0f, 1f,     5f, 3f, //14 dole levo
            //     0f, 0f, 0f,     5f, 2f, //15 dole desno

            //     0f, 1f, 1f,     5f, 0f, //16 gore levo            GORNJA
            //     1f, 1f, 1f,     5f, 1f, //17 gore desno
            //     0f, 1f, 0f,     5f, 2f, //18 gore levo
            //     1f, 1f, 0f,     5f, 3f, //19 gore desno

            //     0f, 0f, 1f,     7f, 0f, //20 gore levo            DONJA
            //     1f, 0f, 1f,     7f, 1f, //21 gore desno
            //     0f, 0f, 0f,     7f, 2f, //22 gore levo
            //     1f, 0f, 0f,     7f, 3f, //23 gore desno

            // uint[] indices = { 0,1,3,0,3,2,  4,5,7,4,7,6,  8,9,11,8,11,10,  12,13,15,12,15,14,  16,17,19,16,19,18,  20,21,23,20,23,22};

            Texture tex = new Texture(ref TexData, ref sp, 0);

            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(rad(FOV), 1280.0f / 720.0f, 0.1f, 100000f);

            var transform = camera.GetMatrix() * proj;   
            sp.SetUniformMat4f("proj", ref transform);

            VertexBuffer vbo = new VertexBuffer(chunk.Item1, BufferUsageHint.StaticDraw);
            
            VertexArray vao = new VertexArray();
            vao.Bind();
            vao.LinkVBO(ref vbo);

            IndexBuffer ibo = new IndexBuffer(chunk.Item2, BufferUsageHint.StaticDraw);

            GL.DrawElements(PrimitiveType.Triangles, chunk.Item2.Length, DrawElementsType.UnsignedInt, 0);

            tex.Unbind();
            tex.Delete();           
            vao.Unbind();
            vao.Delete();
            vbo.Delete();
            ibo.Delete();
            

            sp.Delete();
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        public static float rad(float degree)
        {
            return (float)Math.PI / (180.0f / degree);
        }

    }
}
