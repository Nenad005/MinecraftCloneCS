using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using System.Diagnostics;


namespace BasicOpenTK
{
    class Game
    {
        private FirstPersonCamera camera;
        private int renderDistance;
        public Dictionary<Vector2, Tuple<float[], uint[]>> chunksData = new Dictionary<Vector2, Tuple<float[], uint[]>>();
        private List<Vector2> toBeRendered = new();

        public Game(GameSettings settings)//save, seed . . .
        {
            camera = new FirstPersonCamera(0, 16, 0);//GetCameraPos iz save fajla
            renderDistance = settings.RenderDistance;
            LoadChunksSpawn();
        }

        public void LoadChunksSpawn()
        {
            int DIM = (renderDistance*2 + 1) * (renderDistance*2 + 1);
            var currChunkPos = GetCurrChunkPos();


            for(int i = 0; i < DIM; i++)
            {
                int x = ( i % ( renderDistance * 2 + 1 ) ) - renderDistance;
                int z = ( i / ( renderDistance * 2 + 1 ) ) - renderDistance;
                toBeRendered.Add(new Vector2(x, z) + currChunkPos);
            }


            Stopwatch sw = new Stopwatch();

            sw.Start();
            List<Task> tasks = new List<Task>();
            foreach(var chunk in toBeRendered)
            {
                Task task1 = Task.Factory.StartNew(() => {LoadChunk(chunk);});
                tasks.Add(task1);
            }
            Task.WaitAll(tasks.ToArray());
            // foreach(var chunk in toBeRendered)
            // {
            //     LoadChunk(chunk);
            // }
            sw.Stop();
            Console.WriteLine($"{DIM} cankova renderovano za {sw.ElapsedMilliseconds/1000:F2} sekundi.");
        }

        public void Print()
        {
            Console.WriteLine($"Curr chunkPos --> {GetCurrChunkPos().X} : {GetCurrChunkPos().Y}");
            foreach(var chunk in toBeRendered)
            {
                Console.WriteLine($"{chunk.X} : {chunk.Y}");
            }
        }

        public void LoadChunk(Vector2 chunk)
        {
            int[,,] pos = new int[16, 100, 16];
            for (int x = 0; x < pos.GetLength(0); x++)
            {
                for (int y = 0; y < pos.GetLength(1); y++)
                {
                    for (int z = 0; z < pos.GetLength(2); z++)
                    {
                        pos[x,y,z] = 1;
                    } 
                }
            }
            float[] verts = VertexCalc.GetVertices3D(pos);
            uint[] indices = VertexCalc.GetIndices(verts);
            chunksData[chunk] = Tuple.Create(verts, indices);
            Console.WriteLine($"Rendered --> {chunk.X} : {chunk.Y}");
        }

        public Tuple<float[], uint[]> GetDrawData()
        {
            return chunksData[GetCurrChunkPos()];
        }

        public Vector2 GetCurrChunkPos()
        {
            return new Vector2((int)camera.pos.X / 16, (int)camera.pos.Z / 16);
        }
    }

    struct GameSettings
    {
        public int RenderDistance;
        // public Vector2 Resolution;
        // public Dictionary<Keys, int> KeyBindings;

    }
}