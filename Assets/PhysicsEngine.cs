using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Animations;
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

    private struct FFSReturn
    {
        public Vector3 Pnt;
        public bool Grounded;
    }

    public static int MaxSearchRadius = 6;

    private List<FFSReturn> ForestFireSearch(Vector3 input)
    {
        List<FFSReturn> result = new List<FFSReturn>();

        SearchCount++;
        List<FFSReturn> Q = new List<FFSReturn>();
        BlockClass B = World.GetBlock(input);

        FFSReturn ffs = new FFSReturn();
        ffs.Grounded = false;
        ffs.Pnt = input;
        if (B.Data.isSolid)
        {
            result.Add(ffs);
        }

        B.SearchMarker = SearchCount;
        Q.Add(ffs);
        while (Q.Count > 0)
        {
            FFSReturn origin = Q[0];
            Q.RemoveAt(0);
            foreach (Vector3 Dir in BlockProperties.DirectionVector)
            {
                Vector3 Pnt = origin.Pnt + Dir;
                ffs.Pnt = Pnt;
                ffs.Grounded = false;
                BlockClass neighbor = World.GetBlock(Pnt);
                if (neighbor.Data.isSolid)
                {
                    if (neighbor.SearchMarker < SearchCount)
                    {
                        neighbor.SearchMarker = SearchCount;
                        if ((input - Pnt).magnitude > MaxSearchRadius)
                        {
                            ffs.Grounded = true;
                        }
                        else
                        {
                            Q.Add(ffs);
                        }

                        result.Add(ffs);
                    }
                }
            }


            if (result.Count > 5000)
            {
                break;
            }
        }

        return result;

    }

    private struct FF2
    {
        public Vector3 Pnt;
        public GameObject GO;
    }
    private void ForestFireSearchBuild(Vector3 input)
    {
        SearchCount++;
        PhysicsObjects.Clear();
        List<GameObject> Q = new List<GameObject>();
        
        BlockClass B = World.GetBlock(input);
        B.SearchMarker = SearchCount;
        GameObject parent = new GameObject();
        bool firstSolid = false;
        if (B.Data.isSolid)
        {
            firstSolid = true;
            parent = Instantiate(PhysicsPrefab, input, Quaternion.identity);
            PhysicsObjects.Add(parent);
        }
        Q.Add(parent);
        
        while (Q.Count > 0)
        {
            Vector3 origin = input;
            if (firstSolid)
            {            
                origin = Q[0].transform.position;
                parent = Q[0];
            }
            Q.RemoveAt(0);
            foreach (Vector3 Dir in BlockProperties.DirectionVector)
            {
                Vector3 Pnt = origin + Dir;
                BlockClass neighbor = World.GetBlock(Pnt);
                if (neighbor.Data.isSolid)
                {
                    if (neighbor.SearchMarker < SearchCount)
                    {
                        var G = Instantiate(PhysicsPrefab, Pnt, Quaternion.identity);
                        PhysicsObjects.Add(G);
                        if (firstSolid)
                        {
                            var FJ = G.AddComponent<FixedJoint>();
                            FJ.breakForce = 1000;
                            FJ.breakTorque = 400;
                            FJ.connectedBody = parent.GetComponent<Rigidbody>();
                        }
                        neighbor.SearchMarker = SearchCount;
                        if ((input - Pnt).magnitude > MaxSearchRadius & PhysicsObjects.Count > 100)
                        {
                            G.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        }
                        else
                        {
                            Q.Add(G);
                        }
                    }
                }
            }
            firstSolid = true;


            if (PhysicsObjects.Count > 5000)
            {
                break;
            }
        }

    }
    public void RefreshModel(Vector3 Pnt)
    {
        UnityEngine.Debug.Log("In Here");
        if (PhysicsObjects == null)
            PhysicsObjects = new List<GameObject>();

        foreach (var G in PhysicsObjects) DestroyImmediate(G);
        PhysicsObjects.Clear(); // = new List<GameObject>();
 
        /*
        List<FFSReturn> results = ForestFireSearch(Pnt);
        UnityEngine.Debug.Log("FFS: "+results.Count);
        foreach (var FFS in results)
        {
            var G = Instantiate(PhysicsPrefab, FFS.Pnt, Quaternion.identity);
            PhysicsObjects.Add(G);
            if(FFS.Grounded)
                G.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            G.GetComponent<Rigidbody>().Sleep();
        }
        */
        ForestFireSearchBuild(Pnt);
        //Connect all Parts
        /*
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
            */
    }

    public List<Vector3> PhysicsUpdate()
    {
        var BlocksToAdd = new List<Vector3>();

        if (PhysicsObjects == null)
            return BlocksToAdd;

        //if (LastBreak > 0)
        //{
        //    LastBreak--;
        //    return BlocksToAdd;
        //}

        //if (LastBreak < 0)
       //     return BlocksToAdd;
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