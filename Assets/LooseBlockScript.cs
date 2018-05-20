using System.Collections.Generic;
using UnityEngine;

public class LooseBlockScript : MonoBehaviour
{
    public enum minMax
    {
        min = 0,
        max = 1
    }

    public enum sortDimension
    {
        x = 0,
        y = 1,
        z = 2
    }

    private BlockClass Block;
    private readonly List<int> chunkTriangles = new List<int>();
    private readonly List<Vector2> chunkUV = new List<Vector2>();
    private readonly List<Vector3> chunkVertices = new List<Vector3>();
    private readonly List<Vector3> Corners = new List<Vector3>();
    private int delay = 300;

    public bool Meldable = true;

    public bool ReadyForRemesh;
    // Update is called once per frame
    private int stableCount;
    public void InitBlockFromWorld(Vector3 Pnt)
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        var ChunkPos = Vector3Int.FloorToInt(Pnt / 16f) * 16;
        var BlockPos = Vector3Int.FloorToInt(Pnt) - ChunkPos;
        //Test that block is contained in Block
        if ((BlockPos.x >= 0) & (BlockPos.y >= 0) & (BlockPos.z >= 0) & (BlockPos.x < BlockProperties.ChunkSize) &
            (BlockPos.y < BlockProperties.ChunkSize) &
            (BlockPos.z < BlockProperties.ChunkSize))
        {
            GameObject tempChunk;
            if (WorldScript.ActiveWorld.Chunks.TryGetValue(ChunkPos, out tempChunk))
            {
                var CO = tempChunk.GetComponent<ChunkObject>();
                Block = CO.GetBlock(BlockPos);
                for (var i = 0; i < 8; i++)
                    Corners.Add(CO.GetBlock(BlockPos + BlockProperties.FacePts[i]).Data.ControlPoint +
                                BlockProperties.FacePts[i]);
                chunkVertices.Clear();
                chunkTriangles.Clear();
                chunkUV.Clear();
                //####################################
                for (byte Dir = 0; Dir < 6; Dir++)
                {
                    var DirUV = Dir;
                    var B0 = (byte) (Block.Data.Occlude & (1 << Dir));
                    if (B0 > 0)
                    {
                        for (var i = 0; i < 4; i++)
                            chunkVertices.Add(BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]] +
                                              CO.GetBlock(
                                                  BlockPos + BlockProperties.FacePts[
                                                      BlockProperties.BlockFaces[Dir, i]]).Data.ControlPoint);
                        var V1 = chunkVertices[chunkVertices.Count - 1] - chunkVertices[chunkVertices.Count - 2];
                        var V2 = chunkVertices[chunkVertices.Count - 3] - chunkVertices[chunkVertices.Count - 2];
                        var N = Vector3.Cross(V1, V2).normalized;
                        if (N.y > .3) DirUV = (byte) BlockClass.Direction.Up;
                        var sc = chunkVertices.Count - 4;
                        chunkTriangles.Add(sc);
                        chunkTriangles.Add(sc + 1);
                        chunkTriangles.Add(sc + 3);
                        chunkTriangles.Add(sc + 1);
                        chunkTriangles.Add(sc + 2);
                        chunkTriangles.Add(sc + 3);
                        var UV = Block.GetTex();
                        var uv = new Vector2(UV[DirUV].x / 16f, (15 - UV[DirUV].y) / 16f);
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

    public void InitBlockFromCube(Vector3 blockOrigin)
    {
        var G = gameObject;
        Block = new BlockClass(WorldScript.ActiveWorld.ActiveBlockType, blockOrigin);
        chunkVertices.Clear();
        chunkTriangles.Clear();
        chunkUV.Clear();
        //####################################
        Corners.Add(new Vector3(0.5f, 0.5f, 0.5f));
        Corners.Add(new Vector3(-0.5f, 0.5f, 0.5f));
        Corners.Add(new Vector3(0.5f, -0.5f, 0.5f));
        Corners.Add(new Vector3(-0.5f, -0.5f, 0.5f));
        Corners.Add(new Vector3(0.5f, 0.5f, -0.5f));
        Corners.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        Corners.Add(new Vector3(0.5f, -0.5f, -0.5f));
        Corners.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        for (byte Dir = 0; Dir < 6; Dir++)
        {
            var DirUV = Dir;
            for (var i = 0; i < 4; i++)
            {
                //Vector3 Pt = BlockProperties.FacePts[i];
                //Pt *= 0.5f;
                //Corners.Add(BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]] + new Vector3(0.5f, 0.5f, 0.5f));
                var Pnt = BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]] + new Vector3(0.5f, 0.5f, 0.5f);

                chunkVertices.Add(Pnt);
            }

            var V1 = chunkVertices[chunkVertices.Count - 1] - chunkVertices[chunkVertices.Count - 2];
            var V2 = chunkVertices[chunkVertices.Count - 3] - chunkVertices[chunkVertices.Count - 2];
            var N = Vector3.Cross(V1, V2).normalized;
            if (N.y > .3) DirUV = (byte) BlockClass.Direction.Up;
            var sc = chunkVertices.Count - 4;
            chunkTriangles.Add(sc);
            chunkTriangles.Add(sc + 1);
            chunkTriangles.Add(sc + 3);
            chunkTriangles.Add(sc + 1);
            chunkTriangles.Add(sc + 2);
            chunkTriangles.Add(sc + 3);
            var UV = Block.GetTex();
            var uv = new Vector2(UV[DirUV].x / 16f, (15 - UV[DirUV].y) / 16f);
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

    public Vector3[,,] OrderPoints()
    {
        var Input = new List<Vector3>();
        for (var i = 0; i < Corners.Count; i++) Input.Add(Corners[i]);

        var Result = new Vector3[2, 2, 2];
        var MinX = SortMinMax(Input, sortDimension.x, minMax.min);
        var MaxX = MinX.GetRange(4, 4);
        MinX.RemoveRange(4, 4);
        var MinXMinZ = SortMinMax(MinX, sortDimension.z, minMax.min);
        var MinXMaxZ = MinXMinZ.GetRange(2, 2);
        MinXMinZ.RemoveRange(2, 2);
        var MaxXMinZ = SortMinMax(MaxX, sortDimension.z, minMax.min);
        var MaxXMaxZ = MaxXMinZ.GetRange(2, 2);
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

    public List<Vector3> SortMinMax(List<Vector3> Input, sortDimension dimension, minMax minValue)
    {
        var Result = new List<Vector3>();
        while (Input.Count > 0)
        {
            float mV;
            if (dimension == sortDimension.x)
                mV = Input[0].x;
            else if (dimension == sortDimension.y)
                mV = Input[0].y;
            else
                mV = Input[0].z;
            var mI = 0;
            for (var i = 0; i < Input.Count; i++)
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

            Result.Add(Input[mI]);
            Input.RemoveAt(mI);
        }

        return Result;
    }
    public void MeldBlockIntoWorld()
    {
        var affectedChunks = new List<GameObject>();
        var pnt = transform.position;
        var pntI = Vector3Int.RoundToInt(pnt);

        // Apply the blocks resulting transformation to each point of the original block
        var T = transform.rotation;
        var vec = transform.eulerAngles;
        
        vec.x = Mathf.RoundToInt(vec.x / 90) * 90;
        vec.y = Mathf.RoundToInt(vec.y / 90) * 90;
        vec.z = Mathf.RoundToInt(vec.z / 90) * 90;
        //transform.eulerAngles = vec;
        Quaternion euler = Quaternion.Euler(vec);
        //var T = Quaternion; transform.rotation;
        if (Corners.Count < 8)
            return;
        for (var i = 0; i < Corners.Count; i++) Corners[i] = T * Corners[i];

        List<Vector3Int> newPositions = new List<Vector3Int>();
        int anchor = 0;
        for (var i = 0; i < BlockProperties.FaceBlocksCenter.Length; i++)
        {
            newPositions.Add(Vector3Int.FloorToInt(euler * BlockProperties.FaceBlocksCenter[i]));
            if (newPositions[i].x >= 0 & newPositions[i].y >= 0 & newPositions[i].z >= 0)
                anchor = i;
        }


        var Delta = transform.position - pntI;
        for (var i = 0; i < newPositions.Count; i++)
        {
            Vector3Int newPos = newPositions[i] + pntI;
            BlockClass B = WorldScript.ActiveWorld.GetBlock(newPos);
            if (B == null)
                B = new BlockClass(BlockClass.BlockType.Grass, this.Block.Position);
            if (i == anchor)
                B = Block;
            Vector3 newCp = Corners[i] - newPositions[i] + Delta;
            if (B.Data.CpLocked)
            {
                B.Data.ControlPoint = (newCp + B.Data.ControlPoint) / 2f; 

            }
            else
            {
                B.Data.ControlPoint = newCp;
            }

            B.Data.ControlPoint.x = Mathf.Clamp01(B.Data.ControlPoint.x);
            B.Data.ControlPoint.y = Mathf.Clamp01(B.Data.ControlPoint.y);
            B.Data.ControlPoint.z = Mathf.Clamp01(B.Data.ControlPoint.z);
            B.Data.CpLocked = true;
            affectedChunks.AddRange(WorldScript.ActiveWorld.SetBlock(newPos, B));
        }
        foreach (var go2 in affectedChunks)
            go2.GetComponent<ChunkObject>().RefreshRequired = ChunkObject.RemeshEnum.Mesh;
        DestroyBlock();
    }

    // Scans this and all connected blocks, marking all connected blocks as ready for refresh
    private static void MarkForRefresh(GameObject GO)
    {
        GO.GetComponent<LooseBlockScript>().ReadyForRemesh = true;
        //Stabalize all attached blocks
        var Jo = GO.GetComponents<Joint>();
        foreach (var J in Jo)
            try
            {
                if (J.connectedBody != null)
                    if (J.connectedBody.gameObject != null)
                        if (!J.connectedBody.gameObject.GetComponent<LooseBlockScript>().ReadyForRemesh)
                            MarkForRefresh(J.connectedBody.gameObject);
            }
            catch
            {
            }
    }
    private void FixedUpdate()
    {
        if (Meldable)
            if (GetComponent<Rigidbody>().IsSleeping()) //Mesh not moving; Velocity check for wobble
                if (!ReadyForRemesh)    // This section serves to debounce the remelding by delaying it 50 updates
                {
                    stableCount++;
                    if (stableCount > 50) MarkForRefresh(gameObject);
                }
            if (GetComponent<Rigidbody>().velocity.sqrMagnitude > 500) //Mesh moving away fast;  Also deals with items that fall off the world
                DestroyBlock();
    }
    public void DestroyBlock()
    {
        WorldScript.ActiveWorld.LooseBlocks.Remove(gameObject);
        Destroy(gameObject);
    }
}