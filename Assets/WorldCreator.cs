using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class WorldCreator
{
    public static FastNoiseSIMD myNoise = new FastNoiseSIMD(123);

    // Use this for initialization
    public static void Init(int seed)
    {
        myNoise.SetSeed(seed);
    }

    public static void InitChunk(GameObject GO)
    {
        Create(GO);
    }

    public static void InitChunkBlocking(GameObject GO)
    {
        ChunkObject CO = GO.GetComponent<ChunkObject>();
        Vector3Int Position = Vector3Int.FloorToInt(CO.transform.position);
        WorldCreator.SeedChunk(CO, Position);
        MesherClass Mesher = new MesherClass(CO);
        Mesher.Mesh();
        Mesher.FinalizeMesh();
        GO.SetActive(true);
    }

    public static void RefreshChunk(GameObject GO)
    {
        Refresh(GO);
    }

    private static async void Refresh(GameObject GO)
    {
        ChunkObject CO = GO.GetComponent<ChunkObject>();
        Vector3Int Position = Vector3Int.FloorToInt(CO.transform.position);
        MesherClass Mesher = new MesherClass(CO);
        await Task.Run(() => { Mesher.Mesh(); });
        Mesher.FinalizeMesh();
        GO.SetActive(true);
    }

    private static async void Create(GameObject GO)
    {
        ChunkObject CO = GO.GetComponent<ChunkObject>();
        Vector3Int Position = Vector3Int.FloorToInt(CO.transform.position);
        MesherClass Mesher = new MesherClass(CO);
        await Task.Run(() => { WorldCreator.SeedChunk(CO, Position); });
        await Task.Run(() => { Mesher.Mesh(); });
        Mesher.FinalizeMesh();
        GO.SetActive(true);
    }

    public static void SeedChunk(ChunkObject CO, Vector3Int Position)
    {
        BlockClass[][][] Blocks = CO.Blocks;

        //Basic Height Information
        myNoise.SetAxisScales(1, 1, 1);
        myNoise.SetNoiseType(FastNoiseSIMD.NoiseType.Cubic);
        myNoise.SetFrequency(0.01f);
        float[] LongPeriod = myNoise.GetSampledNoiseSet(Position.x, 0, Position.z, WorldScript.ChunkSizeP2, 1,
            WorldScript.ChunkSizeP2, 1);
        myNoise.SetNoiseType(FastNoiseSIMD.NoiseType.SimplexFractal);
        myNoise.SetFractalType(FastNoiseSIMD.FractalType.FBM);
        myNoise.SetFrequency(0.01f);
        float[] ShortPeriod = myNoise.GetSampledNoiseSet(Position.x, 0, Position.z, WorldScript.ChunkSizeP2, 1,
            WorldScript.ChunkSizeP2, 1);
        myNoise.SetNoiseType(FastNoiseSIMD.NoiseType.SimplexFractal);
        myNoise.SetFractalType(FastNoiseSIMD.FractalType.Billow);
        myNoise.SetFrequency(0.01f);
        myNoise.SetAxisScales(1, 1, 2f);
        float[] CavePeriod = myNoise.GetNoiseSet(Position.x, Position.z, Position.y, WorldScript.ChunkSizeP2,
            WorldScript.ChunkSizeP2, WorldScript.ChunkSizeP2, 1f);
        myNoise.SetNoiseType(FastNoiseSIMD.NoiseType.SimplexFractal);
        myNoise.SetFractalType(FastNoiseSIMD.FractalType.Billow);
        myNoise.SetFrequency(0.01f);
        myNoise.SetAxisScales(2, 2, 2);
        float[] OutCroppingsPeriod = myNoise.GetNoiseSet(Position.x, Position.z, Position.y, WorldScript.ChunkSizeP2,
            WorldScript.ChunkSizeP2, WorldScript.ChunkSizeP2, 1f);

        int index = 0;
        //Cave Information
        int index2 = 0;
        int index3 = 0;

        for (int X = 0; X < WorldScript.ChunkSizeP2; X++)
        {
            for (int Z = 0; Z < WorldScript.ChunkSizeP2; Z++)
            {
                float LP = (LongPeriod[index]);
                float SP = (ShortPeriod[index]);
                index++;

                //Set Altitude
                for (int Y = 0; Y < WorldScript.ChunkSizeP2; Y++)
                {
                    BlockClass B = new BlockClass(BlockClass.BlockType.Grass);
                    Vector3 WPt = Position + new Vector3(X, Y, Z);
                    if (WPt.y == 0)
                        B = new BlockClass(BlockClass.BlockType.BedRock);
                    else
                    {
                        float i = LP * 32f + SP * 16f; // Number 0 to 128
                        i = i + WorldScript.chunkDistance.y * 8 - WPt.y;
                        if (i < 0)
                        {
                            B = new BlockClass(BlockClass.BlockType.Air);
                        }
                        else if (i > 1)
                        {
                            B = new BlockClass(BlockClass.BlockType.Dirt);
                        }

                        B.Data.Density =
                            (sbyte) Mathf.Clamp(i * (float) sbyte.MaxValue, sbyte.MinValue, sbyte.MaxValue);
                        if (OutCroppingsPeriod[index3++] > .4f)
                        {
                            B = new BlockClass(BlockClass.BlockType.BedRock);
                        }

                        if (CavePeriod[index2++] > .3f)
                        {
                            B = new BlockClass(BlockClass.BlockType.Air);
                        }

                        if (WPt.y < 30 & B.Data.Type == BlockClass.BlockType.Air)
                        {
                            B = new BlockClass(BlockClass.BlockType.Water);
                            B.Data.Density = (sbyte) Mathf.Clamp(i * (float) sbyte.MaxValue, sbyte.MinValue,
                                sbyte.MaxValue);
                        }
                    }

                    B.Data.Blockiness = 1;
                    Blocks[X][Y][Z] = B;
                }
            }
        }
    }
}

public class MesherClass
{
    public static Vector3Int[] FacePts =
    {
        new Vector3Int(0, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(-1, -1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(0, 0, -1),
        new Vector3Int(-1, 0, -1),
        new Vector3Int(-1, -1, -1),
        new Vector3Int(0, -1, -1)
    };

    public static Vector3Int[] BlockPtsC =
    {
        new Vector3Int(0, 0, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(1, 1, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(1, 0, 1),
        new Vector3Int(1, 1, 1),
        new Vector3Int(0, 1, 1)
    };

    //public enum Direction { Up = 0, North = 1, East = 2, West = 3, South = 4, Down = 6 };
    public static byte[,] BlockFaces =
    {
        { 0, 4, 5, 1 }, //UP
        { 2, 3, 0, 1 }, //North
        { 3, 7, 4, 0 }, //East
        { 6, 2, 1, 5 }, //West
        { 7, 6, 5, 4 }, //South
        { 2, 6, 7, 3 } //Down 
    };

    public static byte[,] BlockEdges =
    {
        { 0, 1 },
        { 1, 2 },
        { 2, 3 },
        { 3, 0 },
        { 4, 5 },
        { 5, 6 },
        { 6, 7 },
        { 7, 4 },
        { 0, 4 },
        { 1, 5 },
        { 2, 6 },
        { 3, 7 }
    };

    public static Vector3Int[] DirectionVector =
    {
        new Vector3Int(0, 1, 0), //Up
        new Vector3Int(0, 0, 1), //North
        new Vector3Int(1, 0, 0), //East
        new Vector3Int(-1, 0, 0), //West
        new Vector3Int(0, 0, -1), //South
        new Vector3Int(0, -1, 0)
    }; //Down

    public const float tUnit = 1f / 16f;

    private List<Vector3> chunkVertices = new List<Vector3>();
    private List<int> chunkTriangles = new List<int>();
    private List<Vector2> chunkUV = new List<Vector2>();
    private List<Vector3> wchunkVertices = new List<Vector3>();
    private List<int> wchunkTriangles = new List<int>();
    private List<Vector2> wchunkUV = new List<Vector2>();
    private ChunkObject CO;
    private BlockClass.BlockData[][][] Data = new BlockClass.BlockData[WorldScript.ChunkSizeP2][][];

    public MesherClass(ChunkObject cO)
    {
        CO = cO;
    }

    public bool Mesh()
    {
        for (int x = 0; x < WorldScript.ChunkSizeP2; x++)
        {
            Data[x] = new BlockClass.BlockData[WorldScript.ChunkSizeP2][];
            for (int y = 0; y < WorldScript.ChunkSizeP2; y++)
            {
                Data[x][y] = new BlockClass.BlockData[WorldScript.ChunkSizeP2];
                for (int z = 0; z < WorldScript.ChunkSizeP2; z++)
                {
                    Data[x][y][z] = CO.Blocks[x][y][z].Data; // new BlockClass.BlockData();
                }
            }
        }

        for (int Z = 0; Z <= WorldScript.ChunkSize; Z++)
        {
            for (int Y = 0; Y <= WorldScript.ChunkSize; Y++)
            {
                for (int X = 0; X <= WorldScript.ChunkSize; X++)
                {
                    Data[X][Y][Z] = CalcCP(X, Y, Z);
                }
            }
        }

        for (int Z = 0; Z < WorldScript.ChunkSize; Z++)
        {
            for (int Y = 0; Y < WorldScript.ChunkSize; Y++)
            {
                for (int X = 0; X < WorldScript.ChunkSize; X++)
                {
                    Vector3Int V = new Vector3Int(X, Y, Z);
                    // if (GetBlock(V).Data.Density > 0)
                    if (GetBlock(V).Data.Type == BlockClass.BlockType.Water)
                    {
                        for (byte Dir = 0; Dir < 6; Dir++)
                        {
                            if ((GetBlock(V + DirectionVector[Dir]).Data.Type != BlockClass.BlockType.Water))
                            {
                                //byte B1 = (byte)(GetBlock(V + DirectionVector[Dir]).Data.Occlude & (1 << (5 - Dir)));
                                //if (B1 == 0)
                                AddFaceWater(Dir, V);
                            }

                            //if (GetBlock(V + DirectionVector[Dir]).Data.Density <= 0)
                            //    AddFace(Dir, V);
                        }
                    }
                    else
                    {
                        for (byte Dir = 0; Dir < 6; Dir++)
                        {
                            byte B0 = (byte) (GetBlock(V).Data.Occlude & (1 << Dir));
                            if (B0 > 0)
                            {
                                byte B1 = (byte) (GetBlock(V + DirectionVector[Dir]).Data.Occlude & (1 << (5 - Dir)));
                                if (B1 == 0)
                                    AddFace(Dir, V);
                            }

                            //if (GetBlock(V + DirectionVector[Dir]).Data.Density <= 0)
                            //    AddFace(Dir, V);
                        }
                    }
                }
            }
        }

        return true;
        /*
         *                     for (byte Dir = 0; Dir < 6; Dir++)
                    {
                        byte B0 = (byte)(Data[X][Y][Z].Occlude & (1 << Dir));
                        if(B0 > 0)
                        {
                            byte B1 = (byte)(Data[X][Y][Z].Occlude & (1 << (5 - Dir)));
                            if(B1 == 0)
                                AddFace(Dir, V);
                        }
                        //if (GetBlock(V + DirectionVector[Dir]).Data.Density <= 0)
                        //if(B0)
                    }
                    */
    }

    private BlockClass.BlockData CalcCP(int X, int Y, int Z)
    {
        BlockClass.BlockData Result = CO.Blocks[X][Y][Z].Data;
        Result.ControlPoint = new Vector3(0, 0, 0);
        byte EdgeCrossings = 0;
        BlockClass.BlockData[] Adjacent =
        {
            Data[X][Y][Z],
            Data[X + 1][Y][Z],
            Data[X + 1][Y + 1][Z],
            Data[X][Y + 1][Z],
            Data[X][Y][Z + 1],
            Data[X + 1][Y][Z + 1],
            Data[X + 1][Y + 1][Z + 1],
            Data[X][Y + 1][Z + 1]
        };

        for (int i = 0; i < 12; i++)
        {
            if ((Adjacent[BlockEdges[i, 0]].Density <= 0 & Adjacent[BlockEdges[i, 1]].Density > 0) |
                (Adjacent[BlockEdges[i, 1]].Density < 0 & Adjacent[BlockEdges[i, 0]].Density >= 0))
            {
                Result.ControlPoint += Vector3.LerpUnclamped(BlockPtsC[BlockEdges[i, 0]], BlockPtsC[BlockEdges[i, 1]],
                    (-(float) Adjacent[BlockEdges[i, 0]].Density /
                     ((float) Adjacent[BlockEdges[i, 1]].Density - (float) Adjacent[BlockEdges[i, 0]].Density)));
                EdgeCrossings++;
            }
        }

        if (EdgeCrossings != 0)
        {
            Result.ControlPoint /= (float) EdgeCrossings;
            byte MaxB = Adjacent[0].Blockiness;
            for (int i = 1; i < Adjacent.Length; i++)
            {
                if (Adjacent[i].Blockiness > MaxB)
                {
                    MaxB = Adjacent[i].Blockiness;
                }
            }

            Result.ControlPoint =
                (Vector3.Lerp(Result.ControlPoint, new Vector3(0.5f, 0.5f, 0.5f), (float) (MaxB) / 255f));
        }
        else
        {
            Result.ControlPoint = new Vector3(0.5f, 0.5f, 0.5f);
        }

        return Result;
    }

    void AddFace(byte Dir, Vector3Int Center)
    {
        BlockClass CB = GetBlock(Center); // Blocks[Center.x+1,Center.y+1,Center.z+1];
        UnityEngine.Debug.Log("Add Face");
        for (int i = 0; i < 4; i++)
        {
            Vector3Int Blk = Center + FacePts[BlockFaces[Dir, i]];
            //chunkVertices.Add(Blk +  GetBlock(Blk).ControlPoint);
            chunkVertices.Add(Blk + Data[Blk.x + 1][Blk.y + 1][Blk.z + 1].ControlPoint);
        }

        Vector3 V1 = chunkVertices[chunkVertices.Count - 1] - chunkVertices[chunkVertices.Count - 2];
        Vector3 V2 = chunkVertices[chunkVertices.Count - 3] - chunkVertices[chunkVertices.Count - 2];
        Vector3 N = Vector3.Cross(V1, V2).normalized;
        if (N.y > .3)
        {
            Dir = (byte) BlockClass.Direction.Up; //Up
        }

        int sc = chunkVertices.Count - 4; // squareCount << 2;//Multiply by 4
        chunkTriangles.Add(sc);
        chunkTriangles.Add(sc + 1);
        chunkTriangles.Add(sc + 3);
        chunkTriangles.Add(sc + 1);
        chunkTriangles.Add(sc + 2);
        chunkTriangles.Add(sc + 3);
        Vector2[] V = CB.GetTex();

        Vector2 uv = new Vector2(V[Dir].x / 16f, (15 - V[Dir].y) / 16f);
        chunkUV.Add(uv);
        chunkUV.Add(new Vector2(uv.x + tUnit, uv.y));
        chunkUV.Add(new Vector2(uv.x + tUnit, uv.y + tUnit));
        chunkUV.Add(new Vector2(uv.x, uv.y + tUnit));
        //squareCount++;
    }

    void AddFaceWater(byte Dir, Vector3Int Center)
    {
        BlockClass CB = GetBlock(Center); // Blocks[Center.x+1,Center.y+1,Center.z+1];
        UnityEngine.Debug.Log("Add Face");
        for (int i = 0; i < 4; i++)
        {
            Vector3Int Blk = Center + FacePts[BlockFaces[Dir, i]];
            //chunkVertices.Add(Blk +  GetBlock(Blk).ControlPoint);
            Vector3 CP = Data[Blk.x + 1][Blk.y + 1][Blk.z + 1].ControlPoint;
            //if(Dir== 0)
            //CP.y = 0f;
            wchunkVertices.Add(Blk + CP);
        }

        int sc = wchunkVertices.Count - 4; // squareCount << 2;//Multiply by 4
        wchunkTriangles.Add(sc);
        wchunkTriangles.Add(sc + 1);
        wchunkTriangles.Add(sc + 3);
        wchunkTriangles.Add(sc + 1);
        wchunkTriangles.Add(sc + 2);
        wchunkTriangles.Add(sc + 3);
        Vector2[] V = CB.GetTex();

        Vector2 uv = new Vector2(0, 0);
        wchunkUV.Add(uv);
        wchunkUV.Add(new Vector2(1, 0));
        wchunkUV.Add(new Vector2(1, 1));
        wchunkUV.Add(new Vector2(0, 1));
        //squareCount++;
    }

    public void FinalizeMesh()
    {
        CO.mesh = CO.GetComponent<MeshFilter>().mesh;
        CO.col = CO.GetComponent<MeshCollider>();
        CO.mesh.Clear();
        CO.mesh.vertices = chunkVertices.ToArray();
        CO.mesh.triangles = chunkTriangles.ToArray();
        CO.mesh.uv = chunkUV.ToArray();
        CO.mesh.RecalculateNormals();
        CO.col.sharedMesh = CO.mesh;

        CO.waterMesh = CO.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
        CO.waterCol = CO.transform.GetChild(0).GetComponent<MeshCollider>();
        CO.waterMesh.Clear();
        CO.waterMesh.vertices = wchunkVertices.ToArray();
        CO.waterMesh.triangles = wchunkTriangles.ToArray();
        CO.waterMesh.uv = wchunkUV.ToArray();
        CO.waterMesh.RecalculateNormals();
        CO.waterCol.sharedMesh = CO.waterMesh;

        CO.ready = true;
        for (int x = 0; x < WorldScript.ChunkSizeP2; x++)
        {
            for (int y = 0; y < WorldScript.ChunkSizeP2; y++)
            {
                for (int z = 0; z < WorldScript.ChunkSizeP2; z++)
                {
                    CO.blocks[x][y][z].Data.ControlPoint = Data[x][y][z].ControlPoint; // new BlockClass.BlockData();
                }
            }
        }

        UnityEngine.Debug.Log("Did Finalize");
    }

    private BlockClass GetBlock(int X, int Y, int Z)
    {
        return CO.Blocks[X + 1][Y + 1][Z + 1];
    }

    private BlockClass GetBlock(Vector3Int Pnt)
    {
        return CO.Blocks[Pnt.x + 1][Pnt.y + 1][Pnt.z + 1];
    }
}