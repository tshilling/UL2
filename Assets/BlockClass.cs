using UnityEngine;

public class BlockClass
{
    /// <summary>
    ///     The psuedo-material that the current block represents. The default assignment is BlockType.Air.
    /// </summary>
    public enum BlockType
    {
        Air = 0,
        BedRock = 1,
        Dirt = 2,
        Grass = 3,
        Water = 4
    }

    public enum Direction
    {
        Up = 0,
        North = 1,
        East = 2,
        West = 3,
        South = 4,
        Down = 5
    }

    public Vector3Int Position = new Vector3Int();
    // Provides BlockType information for each face of a six-sided cube, but what do the Y component of Vector2 represent?
    public static Vector2[][] Texture =
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
        } //Air
    };
    public static byte[] Blockiness = {0, 255, 255, 255, 1};
    public static sbyte[] Density = {-126, 127, 127, 127, -127};
    public static byte[] Occlude = {0, 63, 63, 63, 0};
    private static byte[] _strength = {15, 15, 15, 15, 15};
    public BlockData Data;
    public int SearchMarker = 0; // Used for Physics Engine

    public BlockClass()
    {
        Position = new Vector3Int(0,0,0);
        Data.Type = BlockType.Air;
        Data.IsSolid = false;
        InitBlock();
    }
    public BlockClass(BlockType type, Vector3 pos)
    {
        Position = Vector3Int.FloorToInt(pos);
        Data.Type = type;
        InitBlock();
    }
    public BlockClass(BlockType type, Vector3Int pos)
    {
        Position = pos;
        Data.Type = type;
        InitBlock();
    }
    private void InitBlock()
    {
        Data.CpLocked = false;
        Data.Density = Density[(int)Data.Type];
        Data.Blockiness = Blockiness[(int)Data.Type];
        Data.Occlude = Occlude[(int)Data.Type];
        Data.ControlPoint = new Vector3(.5f, .5f, .5f);
        if ((Data.Type == BlockType.Air) | (Data.Type == BlockType.Water))
            Data.IsSolid = false;
        else
            Data.IsSolid = true;
    }

    public float Strength
    {
        get { return ((float) _strength[(int) Data.Type]) * 50f; }
    }

    public Vector2[] GetTex()
    {
        return Texture[(byte) Data.Type];
    }

    public struct BlockData
    {
        public sbyte Density;
        public byte Blockiness;
        public byte Occlude;
        public BlockType Type;
        public Vector3 ControlPoint;
        public bool CpLocked;
        public bool IsSolid;
    }
}