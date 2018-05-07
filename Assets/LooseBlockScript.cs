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

    // Use this for initialization
    public void InitBlockFromWorld(WorldScript world, Vector3 Pnt)
    {
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
    /*
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
        List<Vector3> MinXMinY = SortMinMax(MinX, sortDimension.y, minMax.min);
        List<Vector3> MinXMaxY = MinXMinY.GetRange(2,2);
        MinXMinY.RemoveRange(2,2);
        List<Vector3> MaxXMinY = SortMinMax(MaxX, sortDimension.y, minMax.min);
        List<Vector3> MaxXMaxY = MaxXMinY.GetRange(2, 2);
        MaxXMinY.RemoveRange(2, 2);
        if (MinXMinY[0].z < MinXMinY[1].z)
        {
            Result[0, 0, 0] = MinXMinY[0];
            Result[0, 0, 1] = MinXMinY[1];
        }
        else
        {
            Result[0, 0, 0] = MinXMinY[1];
            Result[0, 0, 1] = MinXMinY[0];
        }
        if (MinXMaxY[0].z < MinXMaxY[1].z)
        {
            Result[0, 1, 0] = MinXMaxY[0];
            Result[0, 1, 1] = MinXMaxY[1];
        }
        else
        {
            Result[0, 1, 0] = MinXMaxY[1];
            Result[0, 1, 1] = MinXMaxY[0];
        }
        if (MaxXMinY[0].z < MaxXMinY[1].z)
        {
            Result[1, 0, 0] = MaxXMinY[0];
            Result[1, 0, 1] = MaxXMinY[1];
        }
        else
        {
            Result[1, 0, 0] = MaxXMinY[1];
            Result[1, 0, 1] = MaxXMinY[0];
        }
        if (MaxXMaxY[0].z < MaxXMaxY[1].z)
        {
            Result[1, 1, 0] = MaxXMaxY[0];
            Result[1, 1, 1] = MaxXMaxY[1];
        }
        else
        {
            Result[1, 1, 0] = MaxXMaxY[1];
            Result[1, 1, 1] = MaxXMaxY[0];
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
    */
    public void MeldBlockIntoWorld()
    {
        List<GameObject> AffectedChunks = new List<GameObject>();
        Vector3 Pnt = transform.position;

        //transform points into new position of block

        UnityEngine.Debug.Log("Transform Pos: " + transform.position);


        Quaternion T = transform.rotation;
        for (int i = 0; i < Corners.Count; i++)
        {
            Corners[i] = T * Corners[i];
            CornersSource[i] = T * CornersSource[i] - new Vector3(0.5f, 0.5f, 0.5f);
        }

        Vector3Int ChunkPos = Vector3Int.FloorToInt(Pnt / 16f) * 16;
        Vector3Int BlockPos = Vector3Int.RoundToInt(Pnt) - ChunkPos;
        Vector3 Delta = transform.position - ChunkPos - BlockPos;
        for (int i = 0; i < CornersSource.Count; i++)
        {
            CornersSource[i] = Vector3Int.RoundToInt(CornersSource[i]);
            Corners[i] += Delta;
            Vector3 NewPos = ChunkPos + BlockPos + CornersSource[i];
            BlockClass B = World.GetBlock(NewPos);
            if (CornersSource[i].x == 0 & CornersSource[i].y==0 & CornersSource[i].z==0)
                B = Block;
            B.Data.CPLocked = true;
            B.Data.ControlPoint = Corners[i] - CornersSource[i];
            AffectedChunks.AddRange(World.SetBlock(NewPos, B));
        }

        foreach (GameObject GO2 in AffectedChunks)
        {
            GO2.GetComponent<ChunkObject>().Mesh();
            GO2.GetComponent<ChunkObject>().postMesh();
        }
        GameObject.Destroy(this.gameObject);
    }

    // Update is called once per frame
    int stableCount = 0;
    void Update()
    {
        if (GetComponent<Rigidbody>().velocity.sqrMagnitude < 0.001)  //Mesh not moving
        {
            stableCount++;
            if (stableCount > 100)
            {
                MeldBlockIntoWorld();
            }
        }
        if (GetComponent<Rigidbody>().velocity.sqrMagnitude > 200)  //Mesh moving away fast
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
