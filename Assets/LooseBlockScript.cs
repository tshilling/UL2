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
                for(int i = 0; i < 8; i++)
                {
                    Corners.Add(CO.GetBlock(BlockPos + ChunkObject.FacePts[i]).Data.ControlPoint);

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
                            chunkVertices.Add(MesherClass.FacePts[MesherClass.BlockFaces[Dir, i]] + CO.GetBlock(BlockPos + MesherClass.FacePts[MesherClass.BlockFaces[Dir, i]]).Data.ControlPoint);
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
                        chunkUV.Add(new Vector2(uv.x + MesherClass.tUnit, uv.y));
                        chunkUV.Add(new Vector2(uv.x + MesherClass.tUnit, uv.y + MesherClass.tUnit));
                        chunkUV.Add(new Vector2(uv.x, uv.y + MesherClass.tUnit));
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
    public void MeldBlockIntoWorld()
    {
        Vector3 Pnt = transform.position;
        Vector3Int ChunkPos = Vector3Int.FloorToInt(Pnt / 16f) * 16;
        Vector3Int BlockPos = Vector3Int.FloorToInt(Pnt) - ChunkPos;
        GameObject GO;
        Vector3[,,] CP = new Vector3[2, 2, 2];
        List<Vector3> Min = new List<Vector3>();


        if (World.Chunks.TryGetValue(ChunkPos, out GO)){
            ChunkObject CO = GO.GetComponent<ChunkObject>();
            CO.GetBlock(BlockPos).Data = Block.Data;
        }

        List<GameObject> results = World.SetBlock(Vector3Int.RoundToInt(transform.position), new BlockClass(Block.Data.Type));
        foreach (GameObject GO2 in results)
        {
            GO2.GetComponent<ChunkObject>().Mesh();//.asyncReMeshChunk();
            GO2.GetComponent<ChunkObject>().postMesh();//.asyncReMeshChunk();
        }
        GameObject.DestroyImmediate(this.gameObject);
    }
    void Start () {
		
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
        if (GetComponent<Rigidbody>().velocity.sqrMagnitude > 200)  //Mesh not moving
        {
            GameObject.DestroyImmediate(this.gameObject);

        }
    }
}
