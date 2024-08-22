using OpenTK.Mathematics;

namespace BasicOpenTK
{
    public class VertexCalc
    {
        private static Dictionary<int, int[][]> SideVectors = new Dictionary<int, int[][]>
        {
            [0] = new int[][]
                    {
                        new int[]{0, 0, 0},
                        new int[]{1, 0, 0},
                        new int[]{0, 1, 0},
                        new int[]{1, 1, 0}
                    },
            [1] = new int[][]
                    {
                        new int[]{1, 0, 0},
                        new int[]{1, 0, 1},
                        new int[]{1, 1, 0},
                        new int[]{1, 1, 1}
                    },
            [2] = new int[][]
                    {
                        new int[]{1, 0, 1},
                        new int[]{0, 0, 1},
                        new int[]{1, 1, 1},
                        new int[]{0, 1, 1}
                    },
            [3] = new int[][]
                    {
                        new int[]{0, 0, 1},
                        new int[]{0, 0, 0},
                        new int[]{0, 1, 1},
                        new int[]{0, 1, 0}
                    },
            [4] = new int[][]
                    {
                        new int[]{0, 0, 1},
                        new int[]{1, 0, 1},
                        new int[]{0, 0, 0},
                        new int[]{1, 0, 0}
                    },
            [5] = new int[][]
                    {
                        new int[]{0, 1, 1},
                        new int[]{1, 1, 1},
                        new int[]{0, 1, 0},
                        new int[]{1, 1, 0}
                    },
        };

        public static Dictionary<int, int[]> CoordOffsets3D = new Dictionary<int, int[]>
        {
            [0] = new int[]{ 0, 0,-1 },
            [1] = new int[]{ 1, 0, 0 },
            [2] = new int[]{ 0, 0, 1 },
            [3] = new int[]{-1, 0, 0 },
            [4] = new int[]{ 0, 1, 0 },
            [5] = new int[]{ 0,-1, 0 },
        };
        

        public static float[] GetVertices1D(int[] positions)
        {
            List<float> verts = new();

            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i] == 0) continue;
                int texture = 5;
                for(int side = 0; side < 6; side++)
                {
                    try
                    {
                        if (positions[GetCoords(i, side)] != 0) continue; 
                    }
                    catch {}

                    switch(side)
                    {
                        case 0: texture = 8; break;
                        case 1: texture = 8; break;
                        case 2: texture = 8; break;
                        case 3: texture = 8; break;
                        case 4: texture = 8; break;
                        case 5: texture = 8; break;

                        default:
                            break;
                    }
                    for(int edge = 0; edge < 4; edge++)
                    {
                        verts.Add(i%16 + SideVectors[side][edge][0]);// + chunkCoords[0] * 16);
                        verts.Add(i/256 - SideVectors[side][edge][1]);// + chunkCoords[1] * 16);
                        verts.Add((i%256)/16 + SideVectors[side][edge][2]);// + chunkCoords[2] * 16);
                        verts.Add(texture);
                        verts.Add(edge % 2 == 0 ? edge + 1 : (edge+3)%4);//(edge % 2 == 0 ? (edge + 3) % 4 : Math.Abs((edge + 3) % 4 - 2));
                    }
                }
            }

            return verts.ToArray();
        }

        public static float[] GetVertices3D(int[,,] positions)
        {
            List<float> verts = new();
            List<uint> indices = new();
            for (int xi = 0; xi < positions.GetLength(0); xi++)
            {
                for (int yi = 0; yi < positions.GetLength(1); yi++)
                {
                    for (int zi = 0; zi < positions.GetLength(2); zi++)
                    {
                        if(positions[xi, yi, zi] == 0) continue;
                        int texture = 5;
                        for(int side = 0; side < 6; side++)
                        {
                            try
                            {
                                if (positions[xi+CoordOffsets3D[side][0],yi+CoordOffsets3D[side][1],zi+CoordOffsets3D[side][2]] != 0) continue;
                            }
                            catch {}

                            switch(side)
                            {
                                case 0: texture = 5; break;
                                case 1: texture = 5; break;
                                case 2: texture = 5; break;
                                case 3: texture = 5; break;
                                case 4: texture = 6; break;
                                case 5: texture = 7; break;

                                default:
                                    break;
                            }
                            for(int edge = 0; edge < 4; edge++)
                            {
                                verts.Add(xi + SideVectors[side][edge][0]);// + chunkCoords[0] * 16);
                                verts.Add(yi - SideVectors[side][edge][1]);// + chunkCoords[1] * 16);
                                verts.Add(zi + SideVectors[side][edge][2]);// + chunkCoords[2] * 16);
                                verts.Add(texture);
                                verts.Add(edge % 2 == 0 ? edge + 1 : (edge+3)%4);//(edge % 2 == 0 ? (edge + 3) % 4 : Math.Abs((edge + 3) % 4 - 2));
                            }
                        }
                    }
                }
            }
            return verts.ToArray();
        }

        public static uint[] GetIndices(float[] verts)
        {
            List<uint> indices = new List<uint>();
            for(uint i = 0; i < verts.Length/20; i++)
            {
                indices.Add(0 + i*4);
                indices.Add(1 + i*4);
                indices.Add(3 + i*4);
                indices.Add(0 + i*4);
                indices.Add(3 + i*4);
                indices.Add(2 + i*4);
            }
            return indices.ToArray();
        }

        public static int GetCoords(int i, int side)
        {
            int x = i%16+CoordOffsets3D[side][0];
            int y = i/256+CoordOffsets3D[side][1];
            int z = (i%256)/16+CoordOffsets3D[side][2];
            if (x > 15 || x < 0) return -1;
            if (y > 15 || y < 0) return -1;
            if (z > 15 || z < 0) return -1;
            return(x+16*z+256*y);
        }
    }
}