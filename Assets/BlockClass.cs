using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockClass
{
    public enum Direction
    {
        Up = 0,
        North = 1,
        East = 2,
        West = 3,
        South = 4,
        Down = 5
    };

    /// <summary>
    /// The psuedo-material that the current block represents. The default assignment is BlockType.Air.
    /// </summary>
    public enum BlockType
    {
        Air = 0,
        BedRock = 1,
        Dirt = 2,
        Grass = 3,
        Water = 4
    };

    // Provides BlockType information for each face of a six-sided cube, but what do the Y component of Vector2 represent?
    public static Vector2[][] Texture = new[]
    {
        new[]
        {
            new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0),
            new Vector2(0, 0)
        }, //Air
        new[]
        {
            new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 0),
            new Vector2(1, 0)
        }, //BedRock
        new[]
        {
            new Vector2(2, 0), new Vector2(2, 0), new Vector2(2, 0), new Vector2(2, 0), new Vector2(2, 0),
            new Vector2(2, 0)
        }, //Dirt
        new[]
        {
            new Vector2(0, 0), new Vector2(3, 0), new Vector2(3, 0), new Vector2(3, 0), new Vector2(3, 0),
            new Vector2(2, 0)
        }, //Grass
        new[]
        {
            new Vector2(14, 12), new Vector2(14, 12), new Vector2(14, 12), new Vector2(14, 12), new Vector2(14, 12),
            new Vector2(14, 12)
        }, //Air
    };

    public static byte[] Blockiness = { 0, 255, 255, 0, 1 };
    public static sbyte[] Density = { -126, 127, 127, 127, -127 };
    public static byte[] Occlude = { 0, 63, 63, 63, 0 };

    public struct BlockData
    {
        public sbyte Density;
        public byte Blockiness;
        public byte Occlude;
        public BlockType Type;
        public Vector3 ControlPoint;
        public bool CPLocked;
    }

    public BlockData Data;

    public BlockClass()
    {
        Data.CPLocked = false;
        Data.Type = BlockType.Air;

        //[NOTE]: Static casting the Data.Type enumeration to byte limits you to 255 possible types and will fail silently.
        Data.Density = Density[(int) Data.Type];
        Data.Blockiness = Blockiness[(int) Data.Type];
        Data.Occlude = Occlude[(int) Data.Type];
        Data.ControlPoint = new Vector3(.5f, .5f, .5f);
    }

    public BlockClass(BlockType type)
    {
        Data.CPLocked = false;
        Data.Type = type;
        Data.Density = Density[(int) Data.Type];
        Data.Blockiness = Blockiness[(int) Data.Type];
        Data.Occlude = Occlude[(int) Data.Type];
    }

    public Vector2[] GetTex()
    {
        return Texture[(byte) Data.Type];
    }
}