using System.Collections.Generic;
using UnityEngine;

public class PhysicsEngine : MonoBehaviour
{
    private static int SearchCount;
    public static int LastBreak = -1;
    public int depth = 6;
    public Material OtherMat;
    private List<GameObject> PhysicsObjects;
    public GameObject PhysicsPrefab;
    public WorldScript World;

    private static FillRTNType FillSearch(Vector3Int OriginalPos, WorldScript World, Vector3Int Position,
        int SearchIndex, int SearchDepth)
    {
        var B = World.GetBlock(Position);

        var Result = new FillRTNType();
        Result.Position = new List<Vector3Int>();
        Result.Grounded = new List<bool>();
        if (B.Data.Type != BlockClass.BlockType.Air && B.Data.Type != BlockClass.BlockType.Water)
            Result.Position.Add(Position);
        var Dis = (Position - OriginalPos).magnitude;
        Result.MaxDistance = Dis;
        if (Dis > SearchDepth)
        {
            Result.Grounded.Add(true);
            return Result;
        }

        Result.Grounded.Add(false);

        for (var i = 0; i < BlockProperties.DirectionVector.Length; i++)
        {
            B = World.GetBlock(Position + BlockProperties.DirectionVector[i]);
            if (B.Data.Type != BlockClass.BlockType.Air && B.Data.Type != BlockClass.BlockType.Water)
                if (B.SearchMarker < SearchIndex)
                {
                    B.SearchMarker = SearchIndex;
                    var SubResult = FillSearch(OriginalPos, World, Position + BlockProperties.DirectionVector[i],
                        SearchIndex, SearchDepth);
                    //Result.Blocks.AddRange(SubResult.Blocks);
                    Result.Position.AddRange(SubResult.Position);
                    Result.Grounded.AddRange(SubResult.Grounded);
                    if (SubResult.MaxDistance > Result.MaxDistance) Result.MaxDistance = SubResult.MaxDistance;
                }
        }

        return Result;
    }

    public static FillRTNType FillSearchBlocks(WorldScript World, Vector3Int Position, int SearchDepth)
    {
        var Result = new FillRTNType();
        Result.Position = new List<Vector3Int>();
        Result.Blocks = new List<BlockClass>();
        Result.Grounded = new List<bool>();
        Result.MaxDistance = 0;
        SearchCount++;
        var B = World.GetBlock(Position);
        if (B != null)
        {
            B.SearchMarker = SearchCount;
            Result = FillSearch(Position, World, Position, SearchCount, SearchDepth);
        }

        return Result;
    }

    private SearchResults RecursiveSearch(Vector3Int V, int I)
    {
        var SR = new SearchResults();
        SR.Points = new List<Vector3Int>();
        SR.DepthFound = I;
        var B = World.GetBlock(V);
        if (!B.Data.isSolid) return SR;
        if (B.SearchMarker == SearchCount)
            return SR;
        B.SearchMarker = SearchCount;
        SR.Points.Add(V);
        I++;
        foreach (var Dir in BlockProperties.DirectionVector)
        {
            var R = RecursiveSearch(V + Dir, I);
            SR.Points.AddRange(R.Points);
            if (R.DepthFound > I)
                I = R.DepthFound;
        }

        return SR;
    }

    public void RefreshModel(Vector3 Pnt)
    {
        if (PhysicsObjects == null)
            PhysicsObjects = new List<GameObject>();

        foreach (var G in PhysicsObjects) DestroyImmediate(G);
        PhysicsObjects.Clear(); // = new List<GameObject>();
        for (var x = -depth; x <= depth; x++)
        for (var y = -depth; y <= depth; y++)
        for (var z = -depth; z <= depth; z++)
        {
            var CP = new Vector3(x, y, z) + Pnt;
            var B = World.GetBlock(CP);
            if (B.Data.isSolid)
            {
                var G = Instantiate(PhysicsPrefab, CP, Quaternion.identity);
                PhysicsObjects.Add(G);
                G.GetComponent<Rigidbody>().Sleep();
                if ((Mathf.Abs(x) == depth) | (Mathf.Abs(y) == depth) | (Mathf.Abs(z) == depth)) //Grounded
                    G.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        /*
        FillRTNType RTN = FillSearchBlocks(World, Vector3Int.FloorToInt(Pnt), 6);
        for (int i = 0; i<RTN.Position.Count;i++)
        {
            GameObject G = Instantiate(PhysicsPrefab, RTN.Position[i], Quaternion.identity);
            if (RTN.Grounded[i] == true)
            {
                G.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            PhysicsObjects.Add(G);
            G.GetComponent<Rigidbody>().Sleep();
        }
        */
        //Connect all Parts
        for (var i1 = 0; i1 < PhysicsObjects.Count; i1++)
        for (var i2 = i1 + 1; i2 < PhysicsObjects.Count; i2++)
            if ((PhysicsObjects[i1].transform.position - PhysicsObjects[i2].transform.position).sqrMagnitude < 1.1)
            {
                var J = PhysicsObjects[i1].AddComponent<FixedJoint>();
                if (PhysicsObjects[i1])
                    J.breakForce = 2000;
                J.breakTorque = 100;
                J.enablePreprocessing = false;
                J.connectedBody = PhysicsObjects[i2].GetComponent<Rigidbody>();
            }
    }

    public List<Vector3> PhysicsUpdate()
    {
        var BlocksToAdd = new List<Vector3>();

        if (PhysicsObjects == null)
            return BlocksToAdd;

        if (LastBreak > 0)
        {
            LastBreak--;
            return BlocksToAdd;
        }

        if (LastBreak < 0)
            return BlocksToAdd;
        for (var i = 0; i < PhysicsObjects.Count; i++)
            if (PhysicsObjects[i] != null)
            {
                if (PhysicsObjects[i].GetComponent<Rigidbody>().velocity.sqrMagnitude > 3)
                {
                    var CP = PhysicsObjects[i].GetComponent<PhysicsObject>().OriginalPosition;
                    if (!BlocksToAdd.Contains(CP))
                        BlocksToAdd.Add(CP);
                    DestroyImmediate(PhysicsObjects[i]);
                    PhysicsObjects.RemoveAt(i--);
                }
            }
            else
            {
                PhysicsObjects.RemoveAt(i--);
            }

        return BlocksToAdd;
    }

    // returns a list of Blocks within a specified search distance (SearchDepth).  
    public struct FillRTNType
    {
        public List<BlockClass> Blocks;
        public List<Vector3Int> Position;
        public List<bool> Grounded;
        public float MaxDistance;
    }

    private struct SearchResults
    {
        public List<Vector3Int> Points;
        public int DepthFound;
    }
}