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