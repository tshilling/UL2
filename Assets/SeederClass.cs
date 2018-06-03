using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeederClass{
    private int _seed = 0;
    const int NumberOfBioms = 4;
    public FastNoiseSIMD Noise;
    private enum BioEnum
    {
        GrassLand = 0,
        Desert = 1,
        HillCountry = 2,
        Mountains = 3        
    }
    public SeederClass(int seed)
    {
        _seed = seed;
        Noise = new FastNoiseSIMD(_seed);
    }
    public SeederClass()
    {
        _seed = System.DateTime.Now.Millisecond * System.DateTime.Now.Day;
        Noise = new FastNoiseSIMD(_seed);
    }
    // Only one of these per Y column
    int[,] Biom =      new int[BlockProperties.ChunkSizeP2, BlockProperties.ChunkSizeP2];
    float[,]    Altitude =  new float  [BlockProperties.ChunkSizeP2, BlockProperties.ChunkSizeP2];
    private void SetAltitude(Vector3Int _position)
    {
        Noise.SetPerturbType(FastNoiseSIMD.PerturbType.None);
        Noise.SetAxisScales(1, 1, 1);
        Noise.SetNoiseType(FastNoiseSIMD.NoiseType.Cubic);
        Noise.SetFrequency(0.01f);
        var LongPeriod = Noise.GetSampledNoiseSet(_position.x, 0, _position.z, BlockProperties.ChunkSizeP2, 1, BlockProperties.ChunkSizeP2, 1);
        Noise.SetNoiseType(FastNoiseSIMD.NoiseType.SimplexFractal);
        Noise.SetFractalType(FastNoiseSIMD.FractalType.FBM);
        Noise.SetFrequency(0.005f);
        var ShortPeriod = Noise.GetSampledNoiseSet(_position.x, 0, _position.z, BlockProperties.ChunkSizeP2, 1, BlockProperties.ChunkSizeP2, 1);
        int index = 0;
        for(int x = 0; x < BlockProperties.ChunkSizeP2; x++)
        {
            for(int z=0;z< BlockProperties.ChunkSizeP2; z++)
            {
                Altitude[x, z] = LongPeriod[index] * 32 + 16 + ShortPeriod[index++]*(float)(Biom[x,z]*8); 
            }
        }
    }
    private void SetBiom(Vector3Int _position)
    {
        Noise.SetAxisScales(1, 1, 1);
        Noise.SetNoiseType(FastNoiseSIMD.NoiseType.Cellular);
        Noise.SetCellularDistanceFunction(FastNoiseSIMD.CellularDistanceFunction.Natural);
        Noise.SetCellularReturnType(FastNoiseSIMD.CellularReturnType.NoiseLookup);
        Noise.SetFrequency(0.01f);
        var ShortPeriod = Noise.GetSampledNoiseSet(_position.x, 0, _position.z, BlockProperties.ChunkSizeP2, 1, BlockProperties.ChunkSizeP2, 1);
        int index = 0;
        for (int x = 0; x < BlockProperties.ChunkSizeP2; x++)
        {
            for (int z = 0; z < BlockProperties.ChunkSizeP2; z++)
            {
                Biom[x, z] = (int)(ShortPeriod[index++] * 4f);
            }
        }
    }
    public void SeedChunk(Vector3Int _position, BlockClass[][][] Blocks)
    {
        if (Blocks == null)
            return;
        SetBiom(_position);
        SetAltitude(_position);


        Noise.SetAxisScales(1, 1, 1);
        Noise.SetNoiseType(FastNoiseSIMD.NoiseType.WhiteNoise);
        Noise.SetFrequency(1f);
        var RandomNumbers = Noise.GetSampledNoiseSet(_position.x, 0, _position.z, BlockProperties.ChunkSizeP2, 1, BlockProperties.ChunkSizeP2, 1);

        Noise.SetAxisScales(1, 1, 1);
        Noise.SetNoiseType(FastNoiseSIMD.NoiseType.Cubic);
        Noise.SetFrequency(0.01f);
       
        var CavePeriod = Noise.GetNoiseSet(_position.x, _position.z, _position.y, BlockProperties.ChunkSizeP2, BlockProperties.ChunkSizeP2, BlockProperties.ChunkSizeP2, 1f);
        Noise.SetNoiseType(FastNoiseSIMD.NoiseType.SimplexFractal);
        Noise.SetFractalType(FastNoiseSIMD.FractalType.Billow);
        Noise.SetFrequency(0.01f);
        Noise.SetAxisScales(2, 2, 2);
        var OutCroppingsPeriod = Noise.GetNoiseSet(_position.x, _position.z, _position.y, BlockProperties.ChunkSizeP2, BlockProperties.ChunkSizeP2, BlockProperties.ChunkSizeP2, 1f);

        var index = 0;
        //Cave Information
        var index2 = 0;
        var index3 = 0;

        for (var X = 0; X < BlockProperties.ChunkSizeP2; X++)
            for (var Z = 0; Z < BlockProperties.ChunkSizeP2; Z++)
            {
                index++;

                //Set Altitude
                for (var Y = 0; Y < BlockProperties.ChunkSizeP2; Y++)
                {
                    var WPt = _position + new Vector3(X, Y, Z);
                    var B = new BlockClass(BlockClass.BlockType.Dirt, WPt);
                    if (Biom[X,Z] == 0)
                        B = new BlockClass(BlockClass.BlockType.Grass, WPt);
                    if (Biom[X, Z] == 1)
                        B = new BlockClass(BlockClass.BlockType.Sand, WPt);
                    if (Biom[X, Z] == 2)
                        B = new BlockClass(BlockClass.BlockType.Granite, WPt);
                    if (WPt.y == 0)
                    {
                        B = new BlockClass(BlockClass.BlockType.BedRock, WPt);
                    }
                    else
                    {
                        var i = Altitude[X, Z];////LP * 32f + SP * 16f; // Number 0 to 128
                        i = i + BlockProperties.chunkDistance.y * 8 - WPt.y;
                        if (i < 0)
                        {
                            B = new BlockClass(BlockClass.BlockType.Air, WPt);
                        }
                        else if (i > 1)
                        {
                            B = new BlockClass(BlockClass.BlockType.Dirt, WPt);
                        }

                        B.Data.Density = (sbyte)Mathf.Clamp(i * sbyte.MaxValue, sbyte.MinValue, sbyte.MaxValue);
                        if (OutCroppingsPeriod[index3++] > .4f) B = new BlockClass(BlockClass.BlockType.BedRock, WPt);

                        if (CavePeriod[index2++] > .3f) B = new BlockClass(BlockClass.BlockType.Air, WPt);

                        if ((WPt.y < 30) & (B.Data.Type == BlockClass.BlockType.Air))
                        {
                            B = new BlockClass(BlockClass.BlockType.Water, WPt);
                            B.Data.Density = (sbyte)Mathf.Clamp(i * sbyte.MaxValue, sbyte.MinValue,
                                sbyte.MaxValue);
                        }
                    }
                    B.Data.Blockiness = 1;
                    Blocks[X][Y][Z] = B;
                }
            }

    }
}
