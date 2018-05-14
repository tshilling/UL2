using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseBlockScript : MonoBehaviour {

    BlockClass Block;
    WorldScript World;
    List<Vector3> chunkVertices = new List<Vector3>();
    List<int> chunkTriangles = new List<int>();
    List<Vector2> chunkUV = new List<Vector2>();
    List<Vector3> Corners = new List<Vector3>();
    List<Vector3> CornersSource = new List<Vector3>();
    public LooseBlockScript()
    {
        //this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //this.gameObject.AddComponent();
    }
    void FixedUpdate()
    {
        if (!GetComponent<FixedJoint>())
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //Destroy(this);
        }
    }
    // Use this for initialization
    public static void InitBlockFromWorld(GameObject G, WorldScript world, Vector3 Pnt)
    {
        if(G.GetComponent<Rigidbody>())
            G.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        BlockClass Block;
        WorldScript World = world;
        List<Vector3> chunkVertices = new List<Vector3>();
        List<int> chunkTriangles = new List<int>();
        List<Vector2> chunkUV = new List<Vector2>();
        List<Vector3> Corners = new List<Vector3>();
        List<Vector3> CornersSource = new List<Vector3>();

        Vector3Int ChunkPos = Vector3Int.FloorToInt(Pnt / 16f) * 16;
        Vector3Int BlockPos = Vector3Int.FloorToInt(Pnt) - ChunkPos;
        //Test that block is contained in Block
        if (BlockPos.x >= 0 & BlockPos.y >= 0 & BlockPos.z >= 0 & BlockPos.x < WorldScript.ChunkSize & BlockPos.y < WorldScript.ChunkSize &
            BlockPos.z < WorldScript.ChunkSize)
        {
            GameObject tempChunk;
            if (world.Chunks.TryGetValue(ChunkPos, out tempChunk))
            {
                ChunkObject CO = tempChunk.GetComponent<ChunkObject>();
                Block = CO.GetBlock(BlockPos);
                for (int i = 0; i < 8; i++)
                {
                    Corners.Add(CO.GetBlock(BlockPos + BlockProperties.FacePts[i]).Data.ControlPoint + BlockProperties.FacePts[i]);
                    CO.GetBlock(BlockPos + BlockProperties.FacePts[i]).Data.CPLocked = true;
                    CornersSource.Add(BlockProperties.FacePts[i] + new Vector3(0.5f, 0.5f, 0.5f));
                }
                chunkVertices.Clear();
                chunkTriangles.Clear();
                chunkUV.Clear();
                //####################################
                for (byte Dir = 0; Dir < 6; Dir++)
                {
                    byte DirUV = Dir;
                    byte B0 = (byte)(Block.Data.Occlude & (1 << Dir));
                    if (B0 > 0)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            chunkVertices.Add(BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]] + CO.GetBlock(BlockPos + BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]]).Data.ControlPoint);
                        }
                        Vector3 V1 = chunkVertices[chunkVertices.Count - 1] - chunkVertices[chunkVertices.Count - 2];
                        Vector3 V2 = chunkVertices[chunkVertices.Count - 3] - chunkVertices[chunkVertices.Count - 2];
                        Vector3 N = Vector3.Cross(V1, V2).normalized;
                        if (N.y > .3)
                        {
                            DirUV = (byte)BlockClass.Direction.Up;
                        }
                        int sc = chunkVertices.Count - 4;
                        chunkTriangles.Add(sc);
                        chunkTriangles.Add(sc + 1);
                        chunkTriangles.Add(sc + 3);
                        chunkTriangles.Add(sc + 1);
                        chunkTriangles.Add(sc + 2);
                        chunkTriangles.Add(sc + 3);
                        Vector2[] UV = Block.GetTex();
                        Vector2 uv = new Vector2(UV[DirUV].x / 16f, (15 - UV[DirUV].y) / 16f);
                        chunkUV.Add(uv);
                        chunkUV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y));
                        chunkUV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y + BlockProperties.TUnit));
                        chunkUV.Add(new Vector2(uv.x, uv.y + BlockProperties.TUnit));
                    }
                }
                //####################################
                // Generate Mesh
                Mesh Rmesh;
                MeshCollider Rcol;
                Rmesh = G.GetComponent<MeshFilter>().mesh;
                Rcol = G.GetComponent<MeshCollider>();
                Rmesh.Clear();
                Rmesh.vertices = chunkVertices.ToArray();
                Rmesh.triangles = chunkTriangles.ToArray();
                Rmesh.uv = chunkUV.ToArray();
                Rmesh.RecalculateNormals();
                if(G.GetComponent<MeshCollider>())
                    Rcol.sharedMesh = Rmesh;
            }
        }
    }
    public static void InitBlockAsCube(GameObject G)
    {
        BlockClass Block;

        List<Vector3> chunkVertices = new List<Vector3>();
        List<int> chunkTriangles = new List<int>();
        List<Vector2> chunkUV = new List<Vector2>();
        List<Vector3> Corners = new List<Vector3>();
        List<Vector3> CornersSource = new List<Vector3>();

        Block = new BlockClass(BlockClass.BlockType.Grass);
        chunkVertices.Clear();
        chunkTriangles.Clear();
        chunkUV.Clear();
        //####################################
        for (byte Dir = 0; Dir < 6; Dir++)
        {
            byte DirUV = Dir;
            for (int i = 0; i < 4; i++)
            {
                Vector3 Pt = BlockProperties.FacePts[i];
                Pt *= 0.5f;
                chunkVertices.Add(BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]] + new Vector3(0.5f,0.5f,0.5f));
            }
            Vector3 V1 = chunkVertices[chunkVertices.Count - 1] - chunkVertices[chunkVertices.Count - 2];
            Vector3 V2 = chunkVertices[chunkVertices.Count - 3] - chunkVertices[chunkVertices.Count - 2];
            Vector3 N = Vector3.Cross(V1, V2).normalized;
            if (N.y > .3)
            {
                DirUV = (byte)BlockClass.Direction.Up;
            }
            int sc = chunkVertices.Count - 4;
            chunkTriangles.Add(sc);
            chunkTriangles.Add(sc + 1);
            chunkTriangles.Add(sc + 3);
            chunkTriangles.Add(sc + 1);
            chunkTriangles.Add(sc + 2);
            chunkTriangles.Add(sc + 3);
            Vector2[] UV = Block.GetTex();
            Vector2 uv = new Vector2(UV[DirUV].x / 16f, (15 - UV[DirUV].y) / 16f);
            chunkUV.Add(uv);
            chunkUV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y));
            chunkUV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y + BlockProperties.TUnit));
            chunkUV.Add(new Vector2(uv.x, uv.y + BlockProperties.TUnit));
        }
        //####################################
        // Generate Mesh
        Mesh Rmesh;
        MeshCollider Rcol;
        Rmesh = G.GetComponent<MeshFilter>().mesh;
        Rcol = G.GetComponent<MeshCollider>();
        Rmesh.Clear();
        Rmesh.vertices = chunkVertices.ToArray();
        Rmesh.triangles = chunkTriangles.ToArray();
        Rmesh.uv = chunkUV.ToArray();
        Rmesh.RecalculateNormals();
        if (G.GetComponent<MeshCollider>())
            Rcol.sharedMesh = Rmesh;
    }
    public void InitBlockFromWorld(WorldScript world, Vector3 Pnt)
    {
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        World = world;

        Vector3Int ChunkPos = Vector3Int.FloorToInt(Pnt / 16f) * 16;
        Vector3Int BlockPos = Vector3Int.FloorToInt(Pnt) - ChunkPos;
        //Test that block is contained in Block
        if (BlockPos.x >= 0 & BlockPos.y >= 0 & BlockPos.z >= 0 & BlockPos.x < WorldScript.ChunkSize & BlockPos.y < WorldScript.ChunkSize &
            BlockPos.z < WorldScript.ChunkSize)
        {
            GameObject tempChunk;
            if (world.Chunks.TryGetValue(ChunkPos, out tempChunk))
            {
                ChunkObject CO = tempChunk.GetComponent<ChunkObject>();
                Block = CO.GetBlock(BlockPos);
                for(int i = 0; i < 8; i++) { 
                    Corners.Add(CO.GetBlock(BlockPos + BlockProperties.FacePts[i]).Data.ControlPoint+ BlockProperties.FacePts[i]);
                    CO.GetBlock(BlockPos + BlockProperties.FacePts[i]).Data.CPLocked = true;
                    CornersSource.Add(BlockProperties.FacePts[i]+new Vector3(0.5f,0.5f,0.5f));
                }
                chunkVertices.Clear();
                chunkTriangles.Clear();
                chunkUV.Clear();
                //####################################
                for (byte Dir = 0; Dir < 6; Dir++)
                {
                    byte DirUV = Dir;
                    byte B0 = (byte)(Block.Data.Occlude & (1 << Dir));
                    if (B0 > 0)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            chunkVertices.Add(BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]] + CO.GetBlock(BlockPos + BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]]).Data.ControlPoint);
                        }
                        Vector3 V1 = chunkVertices[chunkVertices.Count - 1] - chunkVertices[chunkVertices.Count - 2];
                        Vector3 V2 = chunkVertices[chunkVertices.Count - 3] - chunkVertices[chunkVertices.Count - 2];
                        Vector3 N = Vector3.Cross(V1, V2).normalized;
                        if (N.y > .3)
                        {
                            DirUV = (byte)BlockClass.Direction.Up;
                        }
                        int sc = chunkVertices.Count - 4;
                        chunkTriangles.Add(sc);
                        chunkTriangles.Add(sc + 1);
                        chunkTriangles.Add(sc + 3);
                        chunkTriangles.Add(sc + 1);
                        chunkTriangles.Add(sc + 2);
                        chunkTriangles.Add(sc + 3);
                        Vector2[] UV = Block.GetTex();
                        Vector2 uv = new Vector2(UV[DirUV].x / 16f, (15 - UV[DirUV].y) / 16f);
                        chunkUV.Add(uv);
                        chunkUV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y));
                        chunkUV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y + BlockProperties.TUnit));
                        chunkUV.Add(new Vector2(uv.x, uv.y + BlockProperties.TUnit));
                    }
                }
                //####################################
                // Generate Mesh
                Mesh Rmesh;
                MeshCollider Rcol;
                Rmesh = GetComponent<MeshFilter>().mesh;
                Rcol = GetComponent<MeshCollider>();
                Rmesh.Clear();
                Rmesh.vertices = chunkVertices.ToArray();
                Rmesh.triangles = chunkTriangles.ToArray();
                Rmesh.uv = chunkUV.ToArray();
                Rmesh.RecalculateNormals();
                Rcol.sharedMesh = Rmesh;
            }
        }
    }

    public Vector3[,,] OrderPoints()
    {
        UnityEngine.Debug.Log("Point Count: " + Corners.Count);
        List<Vector3> Input = new List<Vector3>(); 
        for(int i = 0; i < Corners.Count; i++)
        {
            Input.Add(Corners[i]);
        }

        Vector3[,,] Result = new Vector3[2, 2, 2];
        List<Vector3> MinX = SortMinMax(Input, sortDimension.x, minMax.min);
        List<Vector3> MaxX = MinX.GetRange(4, 4);
        MinX.RemoveRange(4, 4);
        List<Vector3> MinXMinZ = SortMinMax(MinX, sortDimension.z, minMax.min);
        List<Vector3> MinXMaxZ = MinXMinZ.GetRange(2,2);
        MinXMinZ.RemoveRange(2,2);
        List<Vector3> MaxXMinZ = SortMinMax(MaxX, sortDimension.z, minMax.min);
        List<Vector3> MaxXMaxZ = MaxXMinZ.GetRange(2, 2);
        MaxXMinZ.RemoveRange(2, 2);
        if (MinXMinZ[0].y < MinXMinZ[1].y)
        {
            Result[0, 0, 0] = MinXMinZ[0];
            Result[0, 1, 0] = MinXMinZ[1];
        }
        else
        {
            Result[0, 0, 0] = MinXMinZ[1];
            Result[0, 1, 0] = MinXMinZ[0];
        }
        if (MinXMaxZ[0].y < MinXMaxZ[1].y)
        {
            Result[0, 0, 1] = MinXMaxZ[0];
            Result[0, 1, 1] = MinXMaxZ[1];
        }
        else
        {
            Result[0, 0, 1] = MinXMaxZ[1];
            Result[0, 1, 1] = MinXMaxZ[0];
        }
        if (MaxXMinZ[0].y < MaxXMinZ[1].y)
        {
            Result[1, 0, 0] = MaxXMinZ[0];
            Result[1, 1, 0] = MaxXMinZ[1];
        }
        else
        {
            Result[1, 0, 0] = MaxXMinZ[1];
            Result[1, 1, 0] = MaxXMinZ[0];
        }
        if (MaxXMaxZ[0].y < MaxXMaxZ[1].y)
        {
            Result[1, 0, 1] = MaxXMaxZ[0];
            Result[1, 1, 1] = MaxXMaxZ[1];
        }
        else
        {
            Result[1, 0, 1] = MaxXMaxZ[1];
            Result[1, 1, 1] = MaxXMaxZ[0];
        }
        return Result;
    }
    public enum sortDimension {x=0,y=1,z=2 };
    public enum minMax { min=0,max=1};

    public List<Vector3> SortMinMax(List<Vector3> Input, sortDimension dimension, minMax minValue)
    {
        List<Vector3> Result = new List<Vector3>();
        while(Input.Count > 0)
        {
            float mV;
            if (dimension == sortDimension.x)
                mV = Input[0].x;
            else if (dimension == sortDimension.y)
                mV = Input[0].y;
            else
                mV = Input[0].z;
            int mI = 0;
            for(int i = 0; i < Input.Count; i++)
            {
                if (minValue == minMax.min)
                {
                    if (dimension == sortDimension.x)
                    {
                        if (Input[i].x < mV)
                        {
                            mV = Input[i].x;
                            mI = i;
                        }
                    }
                    else if (dimension == sortDimension.y)
                    {
                        if (Input[i].y < mV)
                        {
                            mV = Input[i].y;
                            mI = i;
                        }
                    }
                    else
                    {
                        if (Input[i].z < mV)
                        {
                            mV = Input[i].z;
                            mI = i;
                        }
                    }
                }
                else
                {
                    if (dimension == sortDimension.x)
                    {
                        if (Input[i].x > mV)
                        {
                            mV = Input[i].x;
                            mI = i;
                        }
                    }
                    else if (dimension == sortDimension.y)
                    {
                        if (Input[i].y > mV)
                        {
                            mV = Input[i].y;
                            mI = i;
                        }
                    }
                    else
                    {
                        if (Input[i].z > mV)
                        {
                            mV = Input[i].z;
                            mI = i;
                        }
                    }
                }
            }
            Result.Add(Input[mI]);
            Input.RemoveAt(mI);
        }
        return Result;
    }
   
    public void MeldBlockIntoWorld()
    {
        List<GameObject> AffectedChunks = new List<GameObject>();
        Vector3 Pnt = transform.position;
        Pnt.x = Mathf.Round(Pnt.x);
        Pnt.y = Mathf.Round(Pnt.y);
        Pnt.z = Mathf.Round(Pnt.z);
        // In order to get the chunk that coorisponds to the correct block, PNT must be rounded prior to the next operation
        Vector3Int ChunkPos = Vector3Int.FloorToInt(Pnt / 16f) * 16;
        Vector3Int BlockPos = Vector3Int.RoundToInt(Pnt) - ChunkPos;

        // Apply the blocks resulting transformation to each point of the original block
        Quaternion T = transform.rotation;
        for (int i = 0; i < Corners.Count; i++)
        {
            Corners[i] = T * Corners[i];
        }
        Vector3[,,] Results = OrderPoints();
        Vector3 Delta = transform.position - ChunkPos - BlockPos;
        for(int i = 0; i < BlockProperties.FacePts.Length; i++)
        {
            Vector3Int V = BlockProperties.FacePts[i];
            Vector3 NewPos = ChunkPos + BlockPos + V;
            BlockClass B = World.GetBlock(NewPos);
            if (i == 0)
                B = Block;
            B.Data.ControlPoint = Results[V.x + 1, V.y + 1, V.z + 1] - V + Delta;
            B.Data.ControlPoint.x = Mathf.Clamp01(B.Data.ControlPoint.x);
            B.Data.ControlPoint.y = Mathf.Clamp01(B.Data.ControlPoint.y);
            B.Data.ControlPoint.z = Mathf.Clamp01(B.Data.ControlPoint.z);
            B.Data.CPLocked = true;
            AffectedChunks.AddRange(World.SetBlock(NewPos, B));
        }
        foreach (GameObject GO2 in AffectedChunks)
        {
            GO2.GetComponent<ChunkObject>().RefreshRequired = ChunkObject.RemeshEnum.Mesh;
            //GO2.GetComponent<ChunkObject>().Mesh();
            //GO2.GetComponent<ChunkObject>().postMesh();
        }
        GameObject.Destroy(this.gameObject);
    }
    // Update is called once per frame
    int stableCount = 0;
    public bool ReadyForRemesh = false;
    static void MarkForRefresh(GameObject GO)
    {
        GO.GetComponent<LooseBlockScript>().ReadyForRemesh = true;
        //Stabalize all attached blocks
        Joint[] Jo = GO.GetComponents<Joint>();
        foreach(Joint J in Jo)
        {
            try
            {
                if (J.connectedBody != null)
                    if (J.connectedBody.gameObject != null)
                    {
                            if (!J.connectedBody.gameObject.GetComponent<LooseBlockScript>().ReadyForRemesh)
                                MarkForRefresh(J.connectedBody.gameObject);
                    }
            }
            catch
            {

            }
        }

    }
    void Update()
    {
        if (GetComponent<Rigidbody>().velocity.sqrMagnitude < 0.01)  //Mesh not moving
        {
            if (!ReadyForRemesh)
            {
                stableCount++;
                if (stableCount > 100)
                {
                    MarkForRefresh(this.gameObject);
                }
            }
        }
        if (GetComponent<Rigidbody>().velocity.sqrMagnitude > 500)  //Mesh moving away fast
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
