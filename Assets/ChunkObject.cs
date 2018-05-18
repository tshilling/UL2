using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChunkObject : MonoBehaviour
{
    public enum RemeshEnum
    {
        None = 0,
        Face = 1,
        Mesh = 2,
        FaceUrgent = 3,
        MeshUrgent = 4
    }

    public FastNoiseSIMD myNoise = new FastNoiseSIMD(123);
    private Vector3Int Position;
    public RemeshEnum RefreshRequired = RemeshEnum.None;

    // Status and states
    public ChunkObject()
    {
        LandGeometry = new GeometryData();
        WaterGeometry = new GeometryData();

        Ready = false;
        UpdateCount = 0;

        Blocks = new BlockClass[BlockProperties.ChunkSizeP2][][];

        for (var x = 0; x < BlockProperties.ChunkSize + 2; x++)
        {
            Blocks[x] = new BlockClass[BlockProperties.ChunkSizeP2][];
            for (var y = 0; y < BlockProperties.ChunkSizeP2; y++)
            {
                Blocks[x][y] = new BlockClass[BlockProperties.ChunkSizeP2];
            }
        }
    }

    public GameObject GetBlockMesh(Vector3 Pnt)
    {
        var chunkVertices = new List<Vector3>();
        var chunkTriangles = new List<int>();
        var chunkUV = new List<Vector2>();
        var Result = Instantiate(BaseBlock, transform.position + Pnt, Quaternion.identity);
        var V = Vector3Int.FloorToInt(Pnt);
        var CB = GetBlock(V);
        for (byte Dir = 0; Dir < 6; Dir++)
        {
            var DirUV = Dir;
            var B0 = (byte) (CB.Data.Occlude & (1 << Dir));
            if (B0 > 0)
            {
                for (var i = 0; i < 4; i++)
                    chunkVertices.Add(BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]] +
                                      GetBlock(V + BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]]).Data
                                          .ControlPoint);

                var V1 = chunkVertices[chunkVertices.Count - 1] - chunkVertices[chunkVertices.Count - 2];
                var V2 = chunkVertices[chunkVertices.Count - 3] - chunkVertices[chunkVertices.Count - 2];
                var N = Vector3.Cross(V1, V2).normalized;
                if (N.y > .3) DirUV = (byte) BlockClass.Direction.Up;

                var sc = chunkVertices.Count - 4;
                chunkTriangles.Add(sc);
                chunkTriangles.Add(sc + 1);
                chunkTriangles.Add(sc + 3);
                chunkTriangles.Add(sc + 1);
                chunkTriangles.Add(sc + 2);
                chunkTriangles.Add(sc + 3);
                var UV = CB.GetTex();
                var uv = new Vector2(UV[DirUV].x / 16f, (15 - UV[DirUV].y) / 16f);
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
        return Blocks[V.x + 1][V.y + 1][V.z + 1];
    }

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
        var LongPeriod = myNoise.GetSampledNoiseSet(Position.x, 0, Position.z, BlockProperties.ChunkSizeP2, 1,
            BlockProperties.ChunkSizeP2, 1);
        myNoise.SetNoiseType(FastNoiseSIMD.NoiseType.SimplexFractal);
        myNoise.SetFractalType(FastNoiseSIMD.FractalType.FBM);
        myNoise.SetFrequency(0.01f);
        var ShortPeriod = myNoise.GetSampledNoiseSet(Position.x, 0, Position.z, BlockProperties.ChunkSizeP2, 1,
            BlockProperties.ChunkSizeP2, 1);
        myNoise.SetNoiseType(FastNoiseSIMD.NoiseType.SimplexFractal);
        myNoise.SetFractalType(FastNoiseSIMD.FractalType.Billow);
        myNoise.SetFrequency(0.01f);
        myNoise.SetAxisScales(1, 1, 2f);
        var CavePeriod = myNoise.GetNoiseSet(Position.x, Position.z, Position.y, BlockProperties.ChunkSizeP2,
            BlockProperties.ChunkSizeP2, BlockProperties.ChunkSizeP2, 1f);
        myNoise.SetNoiseType(FastNoiseSIMD.NoiseType.SimplexFractal);
        myNoise.SetFractalType(FastNoiseSIMD.FractalType.Billow);
        myNoise.SetFrequency(0.01f);
        myNoise.SetAxisScales(2, 2, 2);
        var OutCroppingsPeriod = myNoise.GetNoiseSet(Position.x, Position.z, Position.y, BlockProperties.ChunkSizeP2,
            BlockProperties.ChunkSizeP2, BlockProperties.ChunkSizeP2, 1f);

        var index = 0;
        //Cave Information
        var index2 = 0;
        var index3 = 0;

        for (var X = 0; X < BlockProperties.ChunkSizeP2; X++)
        for (var Z = 0; Z < BlockProperties.ChunkSizeP2; Z++)
        {
            var LP = LongPeriod[index];
            var SP = ShortPeriod[index];
            index++;

            //Set Altitude
            for (var Y = 0; Y < BlockProperties.ChunkSizeP2; Y++)
            {
                var WPt = Position + new Vector3(X, Y, Z);
                var B = new BlockClass(BlockClass.BlockType.Grass, WPt);
                if (WPt.y == 0)
                {
                    B = new BlockClass(BlockClass.BlockType.BedRock, WPt);
                }
                else
                {
                    var i = LP * 32f + SP * 16f; // Number 0 to 128
                    i = i + BlockProperties.chunkDistance.y * 8 - WPt.y;
                    if (i < 0)
                        B = new BlockClass(BlockClass.BlockType.Air, WPt);
                    else if (i > 1) B = new BlockClass(BlockClass.BlockType.Dirt, WPt);

                    B.Data.Density =
                        (sbyte) Mathf.Clamp(i * sbyte.MaxValue, sbyte.MinValue, sbyte.MaxValue);
                    if (OutCroppingsPeriod[index3++] > .4f) B = new BlockClass(BlockClass.BlockType.BedRock, WPt);

                    if (CavePeriod[index2++] > .3f) B = new BlockClass(BlockClass.BlockType.Air, WPt);

                    if ((WPt.y < 30) & (B.Data.Type == BlockClass.BlockType.Air))
                    {
                        B = new BlockClass(BlockClass.BlockType.Water, WPt);
                        B.Data.Density = (sbyte) Mathf.Clamp(i * sbyte.MaxValue, sbyte.MinValue,
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

    public void postSeed()
    {
    }

    public async void asyncBuildChunk()
    {
        try
        {
            myNoise.SetSeed(1234);
            preSeed();
            Task T1 = Task.Run(() => { Seed(); });
            
            if (T1.Exception != null)
                UnityEngine.Debug.Log("Task 1: " + T1.Exception);
            if (T1.IsCanceled)
                UnityEngine.Debug.Log("Task 1 Cancelled");
            postSeed();
            preMesh();
            Task T2 = Task.Run(() => { Mesh(); });
            await T2;
            if (T2.Exception != null)
                UnityEngine.Debug.Log("Task 1: " + T1.Exception);
            if (T1.IsCanceled)
                UnityEngine.Debug.Log("Task 2 Cancelled");
            postMesh();
            ChunkBuilt?.Invoke(this);
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log("Error: " + e.InnerException);
        }
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
        for (var Z = 0; Z <= BlockProperties.ChunkSize; Z++)
        for (var Y = 0; Y <= BlockProperties.ChunkSize; Y++)
        for (var X = 0; X <= BlockProperties.ChunkSize; X++)
            Blocks[X][Y][Z].Data = CalcCP(X, Y, Z);

        Face();
    }

    public void Face()
    {
        LandGeometry.Clear();
        WaterGeometry.Clear();

        for (var Z = 0; Z < BlockProperties.ChunkSize; Z++)
        for (var Y = 0; Y < BlockProperties.ChunkSize; Y++)
        for (var X = 0; X < BlockProperties.ChunkSize; X++)
        {
            var V = new Vector3Int(X, Y, Z);
            if (GetBlock(V).Data.Type == BlockClass.BlockType.Water)
            {
                for (byte Dir = 0; Dir < 6; Dir++)
                {
                    if (GetBlock(V + BlockProperties.DirectionVector[Dir]).Data.Type == BlockClass.BlockType.Air)
                        AddFaceWater(WaterGeometry, Dir, V);
                }
            }
            else
            {
                for (byte Dir = 0; Dir < 6; Dir++)
                {
                    var B0 = (byte) (GetBlock(V).Data.Occlude & (1 << Dir));
                    if (B0 <= 0) continue;
                    var B1 = (byte) (GetBlock(V + BlockProperties.DirectionVector[Dir]).Data.Occlude &
                                     (1 << (5 - Dir)));
                    if (B1 == 0)
                        AddFace(LandGeometry, Dir, V);
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
        WaterMeshData.normals = WaterGeometry.Normals.ToArray(); //.RecalculateNormals();
        WaterCollider.sharedMesh = WaterMeshData;

        Ready = true;
    }

    private BlockClass.BlockData CalcCP(int X, int Y, int Z)
    {
        var Result = Blocks[X][Y][Z].Data;
        if (Result.CpLocked) return Result;
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

        for (var i = 0; i < 12; i++)
            if (((Adjacent[BlockProperties.BlockEdges[i, 0]].Density <= 0) &
                 (Adjacent[BlockProperties.BlockEdges[i, 1]].Density > 0)) |
                ((Adjacent[BlockProperties.BlockEdges[i, 1]].Density < 0) &
                 (Adjacent[BlockProperties.BlockEdges[i, 0]].Density >= 0)))
            {
                Result.ControlPoint += Vector3.LerpUnclamped(
                    BlockProperties.BlockPtsC[BlockProperties.BlockEdges[i, 0]],
                    BlockProperties.BlockPtsC[BlockProperties.BlockEdges[i, 1]],
                    -(float) Adjacent[BlockProperties.BlockEdges[i, 0]].Density /
                    (Adjacent[BlockProperties.BlockEdges[i, 1]].Density -
                     (float) Adjacent[BlockProperties.BlockEdges[i, 0]].Density));
                EdgeCrossings++;
            }

        if (EdgeCrossings != 0)
        {
            Result.ControlPoint /= (float) EdgeCrossings;
            var MaxB = Adjacent[0].Blockiness;
            for (var i = 1; i < Adjacent.Length; i++)
                if (Adjacent[i].Blockiness > MaxB)
                    MaxB = Adjacent[i].Blockiness;
            Result.ControlPoint = Vector3.Lerp(Result.ControlPoint, new Vector3(0.5f, 0.5f, 0.5f), MaxB / 255f);
        }
        else
        {
            Result.ControlPoint = new Vector3(0.5f, 0.5f, 0.5f);
        }

        return Result;
    }

    private void AddFace(GeometryData Geo, byte Dir, Vector3Int Center)
    {
        var CB = GetBlock(Center); // Blocks[Center.x+1,Center.y+1,Center.z+1];
        for (var i = 0; i < 4; i++)
        {
            var Blk = Center + BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]];
            Geo.Vertices.Add(Blk + Blocks[Blk.x + 1][Blk.y + 1][Blk.z + 1].Data.ControlPoint);
            Geo.Normals.Add(BlockProperties.DirectionVector[Dir]);
        }

        var V1 = Geo.Vertices[Geo.Vertices.Count - 1] -
                 Geo.Vertices[Geo.Vertices.Count - 2];
        var V2 = Geo.Vertices[Geo.Vertices.Count - 3] -
                 Geo.Vertices[Geo.Vertices.Count - 2];

        var N = Vector3.Cross(V1, V2).normalized;
        if (N.y > .3) Dir = (byte) BlockClass.Direction.Up; //Up

        var sc = Geo.Vertices.Count - 4; // squareCount << 2;//Multiply by 4

        Geo.Triangles.Add(sc);
        Geo.Triangles.Add(sc + 1);
        Geo.Triangles.Add(sc + 3);
        Geo.Triangles.Add(sc + 1);
        Geo.Triangles.Add(sc + 2);
        Geo.Triangles.Add(sc + 3);


        
        var V = CB.GetTex();

        var uv = new Vector2(V[Dir].x / 16f, (15 - V[Dir].y) / 16f);
        Geo.UV.Add(uv);
        Geo.UV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y));
        Geo.UV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y + BlockProperties.TUnit));
        Geo.UV.Add(new Vector2(uv.x, uv.y + BlockProperties.TUnit));
        //squareCount++;
    }

    private void AddFaceWater(GeometryData Geo, byte Dir, Vector3Int Center)
    {
        var CB = GetBlock(Center); // Blocks[Center.x+1,Center.y+1,Center.z+1];
        for (var i = 0; i < 4; i++)
        {
            var Blk = Center + BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]];
            //LandGeometry.Vertices.Add(Blk +  GetBlock(Blk).ControlPoint);
            var CP = Blocks[Blk.x + 1][Blk.y + 1][Blk.z + 1].Data.ControlPoint;
            //if(Dir== 0)
            //CP.y = 0f;
            Geo.Vertices.Add(Blk + CP);
            LandGeometry.Normals.Add(BlockProperties.DirectionVector[Dir]);
        }

        var sc = Geo.Vertices.Count - 4; // squareCount << 2;//Multiply by 4
        Geo.Triangles.Add(sc);
        Geo.Triangles.Add(sc + 1);
        Geo.Triangles.Add(sc + 3);
        Geo.Triangles.Add(sc + 1);
        Geo.Triangles.Add(sc + 2);
        Geo.Triangles.Add(sc + 3);
        var V = CB.GetTex();

        var uv = new Vector2(0, 0);
        Geo.UV.Add(uv);
        Geo.UV.Add(new Vector2(1, 0));
        Geo.UV.Add(new Vector2(1, 1));
        Geo.UV.Add(new Vector2(0, 1));
        //squareCount++;
    }

    public void Awake()
    {
    }

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

    public BlockClass[][][] Blocks { get; set; }

    public ChunkObject[,,] Neighbors = new ChunkObject[3, 3, 3];

    #endregion


    #region Events

    // This is an event creator for loading chunks, referenced by WorldScript;
    public delegate void OnChunkBuilt(ChunkObject chunk);

    public event OnChunkBuilt ChunkBuilt;

    #endregion


    #region Data Members

    private readonly GeometryData LandGeometry;
    private readonly GeometryData WaterGeometry;

    //Initialize a Block arrary of arrays.  Chunk Size + 2 for simplified processing

    #endregion
}