using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEngine {

    // returns a list of Blocks within a specified search distance (SearchDepth).  
    public struct FillRTNType
    {
        public List<BlockClass> Blocks;
        public List<Vector3Int> Position;
        public int MaxDepthFound;
    };
    private static int SearchCount = 0;
    public static Vector3Int OriginalPos = new Vector3Int();
    private static FillRTNType FillSearch(BlockClass CurrentBlock, WorldScript World, Vector3Int Position, int SearchIndex, int SearchDepth)
    {
        FillRTNType Result = new FillRTNType();
        Result.Position = new List<Vector3Int>();
        Result.Blocks = new List<BlockClass>();
        Result.MaxDepthFound = SearchDepth;

        CurrentBlock.SearchMarker = SearchIndex;
        Result.Blocks.Add(CurrentBlock);
        Result.Position.Add(Position);
        float Dis = (Position - OriginalPos).magnitude;
        if (Dis > 4)
            return Result;
        //if (SearchDepth == 0)   //I have gone as far as I should;
        //{
        //    return Result;
        //}
        for(int i = 0; i < BlockProperties.DirectionVector.Length; i++)
        {
            BlockClass B = World.GetBlock(Position + BlockProperties.DirectionVector[i]);
            if(B.Data.Type!= BlockClass.BlockType.Air && B.Data.Type != BlockClass.BlockType.Water)
            {
                if(B.SearchMarker < SearchIndex)
                {
                    FillRTNType SubResult = FillSearch(B, World, Position + BlockProperties.DirectionVector[i], SearchIndex, SearchDepth - 1);
                    Result.Blocks.AddRange(SubResult.Blocks);
                    Result.Position.AddRange(SubResult.Position);
                    if(SubResult.MaxDepthFound < Result.MaxDepthFound)
                    {
                        Result.MaxDepthFound = SubResult.MaxDepthFound;
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
        Result.MaxDepthFound = SearchDepth;
        SearchCount++;
        BlockClass B = World.GetBlock(Position);
        OriginalPos = Position;
        if (B != null)
        {
            Result = FillSearch(B, World, Position, SearchCount, SearchDepth);
        }
        return Result;
    }

}
