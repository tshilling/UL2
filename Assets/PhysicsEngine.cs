using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEngine {

    // returns a list of Blocks within a specified search distance (SearchDepth).  
    public struct FillRTNType
    {
        public List<BlockClass> Blocks;
        public List<Vector3Int> Position;
        public float MaxDistance;
    };
    private static int SearchCount = 0;
    private static FillRTNType FillSearch(Vector3Int OriginalPos, WorldScript World, Vector3Int Position, int SearchIndex, int SearchDepth)
    {
        BlockClass B = World.GetBlock(Position);

        FillRTNType Result = new FillRTNType();
        Result.Position = new List<Vector3Int>();
        if (B.Data.Type != BlockClass.BlockType.Air && B.Data.Type != BlockClass.BlockType.Water)
        {
            Result.Position.Add(Position);
        }
        float Dis = (Position - OriginalPos).magnitude;
        Result.MaxDistance = Dis;
        if (Dis > SearchDepth)
            return Result;

        for(int i = 0; i < BlockProperties.DirectionVector.Length; i++)
        {
            B = World.GetBlock(Position + BlockProperties.DirectionVector[i]);
            if(B.Data.Type!= BlockClass.BlockType.Air && B.Data.Type != BlockClass.BlockType.Water)
            {
                if(B.SearchMarker < SearchIndex)
                {
                    B.SearchMarker = SearchIndex;
                    FillRTNType SubResult = FillSearch(OriginalPos, World, Position + BlockProperties.DirectionVector[i], SearchIndex, SearchDepth);
                    //Result.Blocks.AddRange(SubResult.Blocks);
                    Result.Position.AddRange(SubResult.Position);
                    if(SubResult.MaxDistance > Result.MaxDistance)
                    {
                        Result.MaxDistance = SubResult.MaxDistance;
                    }
                }
            }
        }
        return Result;
    }
    public static FillRTNType FillSearchBlocks(WorldScript World, Vector3Int Position, int SearchDepth)
    {
        FillRTNType Result = new FillRTNType();
        Result.Position = new List<Vector3Int>();
        Result.Blocks = new List<BlockClass>();
        Result.MaxDistance = 0;
        SearchCount++;
        BlockClass B = World.GetBlock(Position);
        if (B != null)
        {
            B.SearchMarker = SearchCount;
            Result = FillSearch(Position, World, Position, SearchCount, SearchDepth);
        }
        return Result;
    }

}
