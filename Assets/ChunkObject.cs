using System.Collections.Generic;
using System.Threading.Tasks;
using Assets;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class ChunkObject : MonoBehaviour
{
    private readonly GeometryData _solidGeometry;
    private readonly GeometryData _liquidGeometry;
    private readonly GeometryData _gasGeometry;
     
    public enum RemeshEnum
    {
        None = 0,
        Face = 1,
        Mesh = 2,
        FaceUrgent = 3,
        MeshUrgent = 4
    }

    public FastNoiseSIMD MyNoise = new FastNoiseSIMD(123);
    private Vector3Int _position;
    public RemeshEnum RefreshRequired = RemeshEnum.None;

    // Status and states
    public ChunkObject()
    {
        _solidGeometry = new GeometryData();
        _liquidGeometry = new GeometryData();

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

    public GameObject GetBlockMesh(Vector3 pnt)
    {
        var chunkVertices = new List<Vector3>();
        var chunkTriangles = new List<int>();
        var chunkUV = new List<Vector2>();
        var Result = Instantiate(WorldScript.BaseBlock, transform.position + pnt, Quaternion.identity);
        var V = Vector3Int.FloorToInt(pnt);
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

    public BlockClass GetBlock(Vector3Int v)
    {
        return Blocks[v.x + 1][v.y + 1][v.z + 1];
    }

    public void PreSeed()
    {
        _position = Vector3Int.FloorToInt(transform.position);
    }

    public void Seed()
    {
        WorldScript.Seeder.SeedChunk(_position, Blocks);
        /*
        //Basic Height Information
        MyNoise.SetAxisScales(1, 1, 1);
        MyNoise.SetNoiseType(FastNoiseSIMD.NoiseType.WhiteNoise);
        MyNoise.SetFrequency(1f);
        var RandomNumbers = MyNoise.GetSampledNoiseSet(_position.x, 0, _position.z, BlockProperties.ChunkSizeP2, 1, BlockProperties.ChunkSizeP2, 1);

        MyNoise.SetAxisScales(1, 1, 1);
        MyNoise.SetNoiseType(FastNoiseSIMD.NoiseType.Cubic);
        MyNoise.SetFrequency(0.01f);
        var LongPeriod = MyNoise.GetSampledNoiseSet(_position.x, 0, _position.z, BlockProperties.ChunkSizeP2, 1, BlockProperties.ChunkSizeP2, 1);
        MyNoise.SetNoiseType(FastNoiseSIMD.NoiseType.SimplexFractal);
        MyNoise.SetFractalType(FastNoiseSIMD.FractalType.FBM);
        MyNoise.SetFrequency(0.01f);
        var ShortPeriod = MyNoise.GetSampledNoiseSet(_position.x, 0, _position.z, BlockProperties.ChunkSizeP2, 1, BlockProperties.ChunkSizeP2, 1);
        MyNoise.SetNoiseType(FastNoiseSIMD.NoiseType.SimplexFractal);
        MyNoise.SetFractalType(FastNoiseSIMD.FractalType.Billow);
        MyNoise.SetFrequency(0.01f);
        MyNoise.SetAxisScales(1, 1, 2f);
        var CavePeriod = MyNoise.GetNoiseSet(_position.x, _position.z, _position.y, BlockProperties.ChunkSizeP2, BlockProperties.ChunkSizeP2, BlockProperties.ChunkSizeP2, 1f);
        MyNoise.SetNoiseType(FastNoiseSIMD.NoiseType.SimplexFractal);
        MyNoise.SetFractalType(FastNoiseSIMD.FractalType.Billow);
        MyNoise.SetFrequency(0.01f);
        MyNoise.SetAxisScales(2, 2, 2);
        var OutCroppingsPeriod = MyNoise.GetNoiseSet(_position.x, _position.z, _position.y, BlockProperties.ChunkSizeP2, BlockProperties.ChunkSizeP2, BlockProperties.ChunkSizeP2, 1f);

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
                var WPt = _position + new Vector3(X, Y, Z);
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
                    {
                        B = new BlockClass(BlockClass.BlockType.Air, WPt);
                    }
                    else if (i > 1)
                    {
                        B = new BlockClass(BlockClass.BlockType.Dirt, WPt);
                    }

                    B.Data.Density = (sbyte) Mathf.Clamp(i * sbyte.MaxValue, sbyte.MinValue, sbyte.MaxValue);
                    if (OutCroppingsPeriod[index3++] > .4f) B = new BlockClass(BlockClass.BlockType.BedRock, WPt);

                    if (CavePeriod[index2++] > .3f) B = new BlockClass(BlockClass.BlockType.Air, WPt);

                    if ((WPt.y < 30) & (B.Data.Type == BlockClass.BlockType.Air))
                    {
                        B = new BlockClass(BlockClass.BlockType.Water, WPt);
                        B.Data.Density = (sbyte) Mathf.Clamp(i * sbyte.MaxValue, sbyte.MinValue,
                            sbyte.MaxValue);
                    }
                }
                B.Data.Blockiness = 1;
                Blocks[X][Y][Z] = B;
            }
        }
        */
    }

    public void PostSeed()
    {
    }

    public async void AsyncBuildChunk()
    {
        try
        {
            MyNoise.SetSeed(1234);
            PreSeed();
            Task T1 = Task.Run(() => { Seed(); });
            
            if (T1.Exception != null)
                UnityEngine.Debug.Log("Task 1: " + T1.Exception);
            if (T1.IsCanceled)
                UnityEngine.Debug.Log("Task 1 Cancelled");
            PostSeed();

            Task T2 = Task.Run(() => { Mesh(); });
            await T2;
            if (T2.Exception != null)
                UnityEngine.Debug.Log("Task 1: " + T1.Exception);
            if (T1.IsCanceled)
                UnityEngine.Debug.Log("Task 2 Cancelled");
            PostMesh();
            ChunkBuilt?.Invoke(this);
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log("Error: " + e.InnerException);
        }
    }

    public async void AsyncReMeshChunk()
    {
        await Task.Run(() => { Mesh(); });
        PostMesh();
    }

    public async void AsyncReFaceChunk()
    {
        Mesh();
        await Task.Run(() => { Face(); });
        PostMesh();
    }

    public void Mesh()
    {
        MesherClass.CalculateControlPoints(Blocks);
        MesherClass.Face(Blocks, _solidGeometry, _liquidGeometry);
    }
    public void Face()
    {
        MesherClass.Face(Blocks, _solidGeometry, _liquidGeometry);
    }
    public void PostMesh()
    {
        MesherClass.PostMesh(gameObject.transform.GetChild(0).gameObject, _solidGeometry,true);
        MesherClass.PostMesh(gameObject.transform.GetChild(1).gameObject, _liquidGeometry, false);
    }

    

    public void Awake()
    {
    }

    private int c = 0;
    public void FixedUpdate()
    {
        c++;
        if (c < 60)
            return;
        c = 0;
        for(var x = 1; x<Blocks.Length-1;x++)
        for (var y = 1; y < Blocks[x].Length-1; y++)
        for (var z = 1; z < Blocks[x][y].Length-1; z++)
        {
            if(Blocks[x][y][z] != null)
            if (Blocks[x][y][z].Data.Type == BlockClass.BlockType.Water)
            {
                for (int i = 0; i < BlockProperties.FlowCheckPoints.Length; i++)
                {
                    var v = BlockProperties.FlowCheckPoints[i];
                    if (Blocks[x + v.x][y + v.y][z + v.z].Data.Type == BlockClass.BlockType.Water)
                    {
                        if (i == 0)
                            break;

                    }
                    if (Blocks[x + v.x][y + v.y][z + v.z].Data.Type == BlockClass.BlockType.Air)
                    {
                        Vector3 CP1 = Blocks[x][y][z].Data.ControlPoint;
                        Vector3 CP2 = Blocks[x + v.x][y + v.y][z + v.z].Data.ControlPoint + v;
                        if (CP2.y < CP1.y)
                        {
                            Vector3 Position = transform.position + new Vector3(x - 1, y - 1, z -1) + v;
                            foreach (var GO in WorldScript.ActiveWorld.SetBlock(Position,
                                new BlockClass(BlockClass.BlockType.Water, Position)))
                            {
                                GO.GetComponent<ChunkObject>().RefreshRequired = RemeshEnum.Mesh;
                            }

                        }
                        //If able to fall straight down
                        if (i == 0)
                            break;
                    }

                }
            }
        }
    }

    public void Update()
    {
        if ((RefreshRequired == ChunkObject.RemeshEnum.FaceUrgent) |
            (RefreshRequired == ChunkObject.RemeshEnum.Face))
        {
            //  LockAllLooseBlocks(true);
            Face();
            PostMesh();
            // LockAllLooseBlocks(false);
        }

        if ((RefreshRequired == ChunkObject.RemeshEnum.MeshUrgent) |
            (RefreshRequired == ChunkObject.RemeshEnum.Mesh))
        {
            //LockAllLooseBlocks(true);
            Mesh();
            PostMesh();
            //LockAllLooseBlocks(false);
        }

        RefreshRequired = ChunkObject.RemeshEnum.None;
    }
    #region Properties
    public BlockClass[][][] Blocks { get; set; }
    public ChunkObject[,,] Neighbors = new ChunkObject[3, 3, 3];
    #endregion

    #region Events

    // This is an event creator for loading chunks, referenced by WorldScript;
    public delegate void OnChunkBuilt(ChunkObject chunk);
    public event OnChunkBuilt ChunkBuilt;

    #endregion
}