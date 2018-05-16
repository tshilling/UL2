using UnityEngine;



public class BlockProperties
{
    public const float TUnit = 1f / 16f;
    
    public static readonly Vector3Int[] FacePts =
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

    public static readonly Vector3Int[] BlockPtsC =
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
    public static readonly byte[,] BlockFaces =
    {
        { 0, 4, 5, 1 }, //UP
        { 2, 3, 0, 1 }, //North
        { 3, 7, 4, 0 }, //East
        { 6, 2, 1, 5 }, //West
        { 7, 6, 5, 4 }, //South
        { 2, 6, 7, 3 } //Down 
    };

    public static readonly byte[,] BlockEdges =
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

    public static readonly Vector3Int[] DirectionVector =
    {
        new Vector3Int(0, 1, 0), //Up
        new Vector3Int(0, 0, 1), //North
        new Vector3Int(1, 0, 0), //East
        new Vector3Int(-1, 0, 0), //West
        new Vector3Int(0, 0, -1), //South
        new Vector3Int(0, -1, 0) // Down
    };

    public struct PositionStruct
    {
        public Vector3 RawInWorld;
        public Vector3Int BlockInWorld;
        public Vector3Int ChunkInWorld;
        public Vector3Int BlockInChunk;
    }
    public static PositionStruct GetPosition(Vector3 Input)
    {
        PositionStruct Result = new PositionStruct();
        Result.RawInWorld = Input;
        Result.BlockInWorld = Vector3Int.FloorToInt(Input);
        Result.ChunkInWorld = Vector3Int.FloorToInt(Input / 16f) * 16;
        Result.BlockInChunk = Result.BlockInWorld - Result.ChunkInWorld;
        return Result;
    }
}
