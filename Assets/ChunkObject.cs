using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChunkObject : MonoBehaviour
{
    #region UnityObjects

    public GameObject BaseBlock;
    public GameObject Water;

    // Mesh Construction Data
    public Mesh LandMeshData;
    public MeshCollider LandMeshCollider;
    public Mesh WaterMeshData;
    public MeshCollider WaterCollider;

    #endregion

    #region Properties

    public bool Ready { get; set; }
    public int UpdateCount { get; set; }
   
    public BlockClass[][][] Blocks
    {
        get { return _blocks; }
        set
        {
            _blocks = value;
            //ReMeshRequired = true;
        }
    }

    #endregion


    #region Events

    // This is an event creator for loading chunks, referenced by WorldScript;
    public delegate void OnChunkBuilt(ChunkObject chunk);
    public event OnChunkBuilt ChunkBuilt;

    #endregion


    #region Data Members

    private GeometryData LandGeometry;
    private GeometryData WaterGeometry;

    //Initialize a Block arrary of arrays.  Chunk Size + 2 for simplified processing
    private BlockClass[][][] _blocks;

    #endregion
    public enum RemeshEnum {None=0, Face=1, Mesh=2, FaceUrgent=3,MeshUrgent=4};
    public RemeshEnum RefreshRequired = RemeshEnum.None;
    
    // Status and states
    public ChunkObject()
    {
        LandGeometry = new GeometryData();
        WaterGeometry = new GeometryData();

        Ready = false;
        UpdateCount = 0;

        _blocks = new BlockClass[WorldScript.ChunkSizeP2][][];

        for (int x = 0; x < WorldScript.ChunkSize + 2; x++)
        {
            Blocks[x] = new BlockClass[WorldScript.ChunkSizeP2][];
            for (int y = 0; y < WorldScript.ChunkSizeP2; y++)
            {
                Blocks[x][y] = new BlockClass[WorldScript.ChunkSizeP2];
            }
        }
    }

    public GameObject GetBlockMesh(Vector3 Pnt)
    {
        List<Vector3> chunkVertices = new List<Vector3>();
        List<int> chunkTriangles = new List<int>();
        List<Vector2> chunkUV = new List<Vector2>();
        GameObject Result = Instantiate(BaseBlock, transform.position + Pnt, Quaternion.identity);
        Vector3Int V = Vector3Int.FloorToInt(Pnt);
        BlockClass CB = GetBlock(V);
        for (byte Dir = 0; Dir < 6; Dir++)
        {
            byte DirUV = Dir;
            byte B0 = (byte) (CB.Data.Occlude & (1 << Dir));
            if (B0 > 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    chunkVertices.Add(BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]] +
                                      GetBlock(V + BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]]).Data
                                          .ControlPoint);
                }

                Vector3 V1 = chunkVertices[chunkVertices.Count - 1] - chunkVertices[chunkVertices.Count - 2];
                Vector3 V2 = chunkVertices[chunkVertices.Count - 3] - chunkVertices[chunkVertices.Count - 2];
                Vector3 N = Vector3.Cross(V1, V2).normalized;
                if (N.y > .3)
                {
                    DirUV = (byte) BlockClass.Direction.Up;
                }

                int sc = chunkVertices.Count - 4;
                chunkTriangles.Add(sc);
                chunkTriangles.Add(sc + 1);
                chunkTriangles.Add(sc + 3);
                chunkTriangles.Add(sc + 1);
                chunkTriangles.Add(sc + 2);
                chunkTriangles.Add(sc + 3);
                Vector2[] UV = CB.GetTex();
                Vector2 uv = new Vector2(UV[DirUV].x / 16f, (15 - UV[DirUV].y) / 16f);
                chunkUV.Add(uv);
                chunkUV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y));
                chunkUV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y + BlockProperties.TUnit));
                chunkUV.Add(new Vector2(uv.x, uv.y + BlockProperties.TUnit));
            }
        }

        Mesh Rmesh;
        MeshCollider Rcol;
        Rmesh = Result.GetComponent<MeshFilter>().mesh;
        Rcol = Result.GetComponent<MeshCollider>();
        Rmesh.Clear();
        Rmesh.vertices = chunkVertices.ToArray();
        Rmesh.triangles = chunkTriangles.ToArray();
        Rmesh.uv = chunkUV.ToArray();
        Rmesh.RecalculateNormals();
        Rcol.sharedMesh = Rmesh;
        return Result;
    }

    public BlockClass GetBlock(Vector3Int V)
    {
        return _blocks[V.x + 1][V.y + 1][V.z + 1];
    }

    public FastNoiseSIMD myNoise = new FastNoiseSIMD(123);
    Vector3Int Position;

    public void preSeed()
    {
        Position = Vector3Int.FloorToInt(transform.position);
    }

    public void Seed()
    {
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
                    /*
                    if (B.Data.Density <= 0)
                        B.Data.Density = (sbyte) Mathf.Clamp(B.Data.Density, -126f, -1f);
                    if (B.Data.Density >= 0)
                        B.Data.Density = (sbyte)Mathf.Clamp(B.Data.Density, 1f, 127f);*/
                    B.Data.Blockiness = 1;
                    Blocks[X][Y][Z] = B;
                }
            }
        }
    }

    public void postSeed()
    {
    }

    public async void asyncBuildChunk()
    {
        myNoise.SetSeed(1234);
        preSeed();
        await Task.Run(() => { Seed(); });
        postSeed();
        preMesh();
        await Task.Run(() => { Mesh(); });
        postMesh();

        ChunkBuilt?.Invoke(this);
    }

    public async void asyncReMeshChunk()
    {
        preMesh();
        await Task.Run(() => { Mesh(); });
        postMesh();
    }

    public async void asyncReFaceChunk()
    {
        Mesh();

        await Task.Run(() => { Face(); });
        postMesh();
    }

    public void preMesh()
    {
    }

    public void Mesh()
    {
        for (int Z = 0; Z <= WorldScript.ChunkSize; Z++)
        {
            for (int Y = 0; Y <= WorldScript.ChunkSize; Y++)
            {
                for (int X = 0; X <= WorldScript.ChunkSize; X++)
                {
                    _blocks[X][Y][Z].Data = CalcCP(X, Y, Z);
                }
            }
        }

        Face();
    }

    public void Face()
    {
        LandGeometry.Clear();
        WaterGeometry.Clear();
       
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
                            if ((GetBlock(V + BlockProperties.DirectionVector[Dir]).Data.Type != BlockClass.BlockType.Water))
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
                                byte B1 = (byte) (GetBlock(V + BlockProperties.DirectionVector[Dir]).Data.Occlude & (1 << (5 - Dir)));
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
    }

    public void postMesh()
    {
        LandMeshData = GetComponent<MeshFilter>().mesh;
        LandMeshCollider = GetComponent<MeshCollider>();
        LandMeshData.Clear();
        LandMeshData.vertices = LandGeometry.Vertices.ToArray();
        LandMeshData.triangles = LandGeometry.Triangles.ToArray();
        LandMeshData.uv = LandGeometry.UV.ToArray();
        LandMeshData.RecalculateNormals();

        LandMeshCollider.sharedMesh = LandMeshData;

        WaterMeshData = transform.GetChild(0).GetComponent<MeshFilter>().mesh;
        WaterCollider = transform.GetChild(0).GetComponent<MeshCollider>();
        WaterMeshData.Clear();
        WaterMeshData.vertices = WaterGeometry.Vertices.ToArray();
        WaterMeshData.triangles = WaterGeometry.Triangles.ToArray();
        WaterMeshData.uv = WaterGeometry.UV.ToArray();
        WaterMeshData.RecalculateNormals();
        WaterCollider.sharedMesh = WaterMeshData;

        Ready = true;
    }

    private BlockClass.BlockData CalcCP(int X, int Y, int Z)
    {
        BlockClass.BlockData Result = Blocks[X][Y][Z].Data;
        if (Result.CPLocked)
        {
            return Result;
        }
        Result.ControlPoint = new Vector3(0, 0, 0);
        byte EdgeCrossings = 0;
        BlockClass.BlockData[] Adjacent =
        {
            Blocks[X][Y][Z].Data,
            Blocks[X + 1][Y][Z].Data,
            Blocks[X + 1][Y + 1][Z].Data,
            Blocks[X][Y + 1][Z].Data,
            Blocks[X][Y][Z + 1].Data,
            Blocks[X + 1][Y][Z + 1].Data,
            Blocks[X + 1][Y + 1][Z + 1].Data,
            Blocks[X][Y + 1][Z + 1].Data
        };

        for (int i = 0; i < 12; i++)
        {
            if ((Adjacent[BlockProperties.BlockEdges[i, 0]].Density <= 0 & Adjacent[BlockProperties.BlockEdges[i, 1]].Density > 0) |
                (Adjacent[BlockProperties.BlockEdges[i, 1]].Density < 0 & Adjacent[BlockProperties.BlockEdges[i, 0]].Density >= 0))
            {
                Result.ControlPoint += Vector3.LerpUnclamped(BlockProperties.BlockPtsC[BlockProperties.BlockEdges[i, 0]], BlockProperties.BlockPtsC[BlockProperties.BlockEdges[i, 1]],
                    (-(float) Adjacent[BlockProperties.BlockEdges[i, 0]].Density /
                     ((float) Adjacent[BlockProperties.BlockEdges[i, 1]].Density - (float) Adjacent[BlockProperties.BlockEdges[i, 0]].Density)));
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
        for (int i = 0; i < 4; i++)
        {
            Vector3Int Blk = Center + BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]];
            LandGeometry.Vertices.Add(Blk + _blocks[Blk.x + 1][Blk.y + 1][Blk.z + 1].Data.ControlPoint);
        }

        Vector3 V1 = LandGeometry.Vertices[LandGeometry.Vertices.Count - 1] - LandGeometry.Vertices[LandGeometry.Vertices.Count - 2];
        Vector3 V2 = LandGeometry.Vertices[LandGeometry.Vertices.Count - 3] - LandGeometry.Vertices[LandGeometry.Vertices.Count - 2];
        Vector3 N = Vector3.Cross(V1, V2).normalized;
        if (N.y > .3)
        {
            Dir = (byte) BlockClass.Direction.Up; //Up
        }

        int sc = LandGeometry.Vertices.Count - 4; // squareCount << 2;//Multiply by 4
        LandGeometry.Triangles.Add(sc);
        LandGeometry.Triangles.Add(sc + 1);
        LandGeometry.Triangles.Add(sc + 3);
        LandGeometry.Triangles.Add(sc + 1);
        LandGeometry.Triangles.Add(sc + 2);
        LandGeometry.Triangles.Add(sc + 3);
        Vector2[] V = CB.GetTex();

        Vector2 uv = new Vector2(V[Dir].x / 16f, (15 - V[Dir].y) / 16f);
        LandGeometry.UV.Add(uv);
        LandGeometry.UV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y));
        LandGeometry.UV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y + BlockProperties.TUnit));
        LandGeometry.UV.Add(new Vector2(uv.x, uv.y + BlockProperties.TUnit));
        //squareCount++;
    }

    void AddFaceWater(byte Dir, Vector3Int Center)
    {
        BlockClass CB = GetBlock(Center); // Blocks[Center.x+1,Center.y+1,Center.z+1];
        for (int i = 0; i < 4; i++)
        {
            Vector3Int Blk = Center + BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]];
            //LandGeometry.Vertices.Add(Blk +  GetBlock(Blk).ControlPoint);
            Vector3 CP = _blocks[Blk.x + 1][Blk.y + 1][Blk.z + 1].Data.ControlPoint;
            //if(Dir== 0)
            //CP.y = 0f;
            WaterGeometry.Vertices.Add(Blk + CP);
        }

        int sc = WaterGeometry.Vertices.Count - 4; // squareCount << 2;//Multiply by 4
        WaterGeometry.Triangles.Add(sc);
        WaterGeometry.Triangles.Add(sc + 1);
        WaterGeometry.Triangles.Add(sc + 3);
        WaterGeometry.Triangles.Add(sc + 1);
        WaterGeometry.Triangles.Add(sc + 2);
        WaterGeometry.Triangles.Add(sc + 3);
        Vector2[] V = CB.GetTex();

        Vector2 uv = new Vector2(0, 0);
        WaterGeometry.UV.Add(uv);
        WaterGeometry.UV.Add(new Vector2(1, 0));
        WaterGeometry.UV.Add(new Vector2(1, 1));
        WaterGeometry.UV.Add(new Vector2(0, 1));
        //squareCount++;
    }

    public void Awake()
    {
    }
}