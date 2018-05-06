using System.Collections.Generic;
using UnityEngine;

public class MesherClass
{
    private GeometryData LandGeometry;
    private GeometryData WaterGeometry;
    
    private ChunkObject CO;
    private BlockClass.BlockData[][][] Data = new BlockClass.BlockData[WorldScript.ChunkSizeP2][][];

    public MesherClass(ChunkObject cO)
    {
        LandGeometry = new GeometryData();
        WaterGeometry = new GeometryData();
        
        CO = cO;
    }

    public bool Mesh()
    {
        for (int x = 0; x < WorldScript.ChunkSizeP2; x++)
        {
            Data[x] = new BlockClass.BlockData[WorldScript.ChunkSizeP2][];
            for (int y = 0; y < WorldScript.ChunkSizeP2; y++)
            {
                Data[x][y] = new BlockClass.BlockData[WorldScript.ChunkSizeP2];
                for (int z = 0; z < WorldScript.ChunkSizeP2; z++)
                {
                    Data[x][y][z] = CO.Blocks[x][y][z].Data; // new BlockClass.BlockData();
                }
            }
        }

        for (int Z = 0; Z <= WorldScript.ChunkSize; Z++)
        {
            for (int Y = 0; Y <= WorldScript.ChunkSize; Y++)
            {
                for (int X = 0; X <= WorldScript.ChunkSize; X++)
                {
                    Data[X][Y][Z] = CalcCP(X, Y, Z);
                }
            }
        }

        for (int Z = 0; Z < WorldScript.ChunkSize; Z++)
        {
            for (int Y = 0; Y < WorldScript.ChunkSize; Y++)
            {
                for (int X = 0; X < WorldScript.ChunkSize; X++)
                {
                    Vector3Int V = new Vector3Int(X, Y, Z);
                    // if (GetBlock(V).Data.Density > 0)
                    if (GetBlock(V).Data.Type == BlockClass.BlockType.Water)
                    {
                        for (byte Dir = 0; Dir < 6; Dir++)
                        {
                            if ((GetBlock(V + BlockProperties.DirectionVector[Dir]).Data.Type !=
                                 BlockClass.BlockType.Water))
                            {
                                //byte B1 = (byte)(GetBlock(V + DirectionVector[Dir]).Data.Occlude & (1 << (5 - Dir)));
                                //if (B1 == 0)
                                AddFaceWater(Dir, V);
                            }

                            //if (GetBlock(V + DirectionVector[Dir]).Data.Density <= 0)
                            //    AddFace(Dir, V);
                        }
                    }
                    else
                    {
                        for (byte Dir = 0; Dir < 6; Dir++)
                        {
                            byte B0 = (byte) (GetBlock(V).Data.Occlude & (1 << Dir));
                            if (B0 > 0)
                            {
                                byte B1 = (byte) (GetBlock(V + BlockProperties.DirectionVector[Dir]).Data.Occlude &
                                                  (1 << (5 - Dir)));
                                if (B1 == 0)
                                    AddFace(Dir, V);
                            }

                            //if (GetBlock(V + DirectionVector[Dir]).Data.Density <= 0)
                            //    AddFace(Dir, V);
                        }
                    }
                }
            }
        }

        return true;
        /*
         *                     for (byte Dir = 0; Dir < 6; Dir++)
                    {
                        byte B0 = (byte)(Data[X][Y][Z].Occlude & (1 << Dir));
                        if(B0 > 0)
                        {
                            byte B1 = (byte)(Data[X][Y][Z].Occlude & (1 << (5 - Dir)));
                            if(B1 == 0)
                                AddFace(Dir, V);
                        }
                        //if (GetBlock(V + DirectionVector[Dir]).Data.Density <= 0)
                        //if(B0)
                    }
                    */
    }

    private BlockClass.BlockData CalcCP(int X, int Y, int Z)
    {
        BlockClass.BlockData Result = CO.Blocks[X][Y][Z].Data;
        Result.ControlPoint = new Vector3(0, 0, 0);
        byte EdgeCrossings = 0;
        BlockClass.BlockData[] Adjacent =
        {
            Data[X][Y][Z],
            Data[X + 1][Y][Z],
            Data[X + 1][Y + 1][Z],
            Data[X][Y + 1][Z],
            Data[X][Y][Z + 1],
            Data[X + 1][Y][Z + 1],
            Data[X + 1][Y + 1][Z + 1],
            Data[X][Y + 1][Z + 1]
        };

        for (int i = 0; i < 12; i++)
        {
            if ((Adjacent[BlockProperties.BlockEdges[i, 0]].Density <= 0 &
                 Adjacent[BlockProperties.BlockEdges[i, 1]].Density > 0) |
                (Adjacent[BlockProperties.BlockEdges[i, 1]].Density < 0 &
                 Adjacent[BlockProperties.BlockEdges[i, 0]].Density >= 0))
            {
                Result.ControlPoint += Vector3.LerpUnclamped(
                    BlockProperties.BlockPtsC[BlockProperties.BlockEdges[i, 0]],
                    BlockProperties.BlockPtsC[BlockProperties.BlockEdges[i, 1]],
                    (-(float) Adjacent[BlockProperties.BlockEdges[i, 0]].Density /
                     ((float) Adjacent[BlockProperties.BlockEdges[i, 1]].Density -
                      (float) Adjacent[BlockProperties.BlockEdges[i, 0]].Density)));
                EdgeCrossings++;
            }
        }

        if (EdgeCrossings != 0)
        {
            Result.ControlPoint /= (float) EdgeCrossings;
            byte MaxB = Adjacent[0].Blockiness;
            for (int i = 1; i < Adjacent.Length; i++)
            {
                if (Adjacent[i].Blockiness > MaxB)
                {
                    MaxB = Adjacent[i].Blockiness;
                }
            }

            Result.ControlPoint =
                (Vector3.Lerp(Result.ControlPoint, new Vector3(0.5f, 0.5f, 0.5f), (float) (MaxB) / 255f));
        }
        else
        {
            Result.ControlPoint = new Vector3(0.5f, 0.5f, 0.5f);
        }

        return Result;
    }

    void AddFace(byte Dir, Vector3Int Center)
    {
        BlockClass CB = GetBlock(Center); // Blocks[Center.x+1,Center.y+1,Center.z+1];
        UnityEngine.Debug.Log("Add Face");
        for (int i = 0; i < 4; i++)
        {
            Vector3Int Blk = Center + BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]];
            //chunkVertices.Add(Blk +  GetBlock(Blk).ControlPoint);
            LandGeometry.Vertices.Add(Blk + Data[Blk.x + 1][Blk.y + 1][Blk.z + 1].ControlPoint);
        }

        Vector3 V1 = LandGeometry.Vertices[LandGeometry.Vertices.Count - 1] - LandGeometry.Vertices[LandGeometry.Vertices.Count - 2];
        Vector3 V2 = LandGeometry.Vertices[LandGeometry.Vertices.Count - 3] - LandGeometry.Vertices[LandGeometry.Vertices.Count - 2];
        Vector3 N = Vector3.Cross(V1, V2).normalized;
        if (N.y > .3)
        {
            Dir = (byte) BlockClass.Direction.Up; //Up
        }

        int sc = LandGeometry.Vertices.Count - 4; // squareCount << 2;//Multiply by 4
        LandGeometry.Triangles.Add(sc);
        LandGeometry.Triangles.Add(sc + 1);
        LandGeometry.Triangles.Add(sc + 3);
        LandGeometry.Triangles.Add(sc + 1);
        LandGeometry.Triangles.Add(sc + 2);
        LandGeometry.Triangles.Add(sc + 3);
        Vector2[] V = CB.GetTex();

        Vector2 uv = new Vector2(V[Dir].x / 16f, (15 - V[Dir].y) / 16f);
        LandGeometry.UV.Add(uv);
        LandGeometry.UV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y));
        LandGeometry.UV.Add(new Vector2(uv.x + BlockProperties.TUnit, uv.y + BlockProperties.TUnit));
        LandGeometry.UV.Add(new Vector2(uv.x, uv.y + BlockProperties.TUnit));
        //squareCount++;
    }

    void AddFaceWater(byte Dir, Vector3Int Center)
    {
        BlockClass CB = GetBlock(Center); // Blocks[Center.x+1,Center.y+1,Center.z+1];
        UnityEngine.Debug.Log("Add Face");
        for (int i = 0; i < 4; i++)
        {
            Vector3Int Blk = Center + BlockProperties.FacePts[BlockProperties.BlockFaces[Dir, i]];
            //chunkVertices.Add(Blk +  GetBlock(Blk).ControlPoint);
            Vector3 CP = Data[Blk.x + 1][Blk.y + 1][Blk.z + 1].ControlPoint;
            //if(Dir== 0)
            //CP.y = 0f;
            WaterGeometry.Vertices.Add(Blk + CP);
        }

        int sc = WaterGeometry.Vertices.Count - 4; // squareCount << 2;//Multiply by 4
        WaterGeometry.Triangles.Add(sc);
        WaterGeometry.Triangles.Add(sc + 1);
        WaterGeometry.Triangles.Add(sc + 3);
        WaterGeometry.Triangles.Add(sc + 1);
        WaterGeometry.Triangles.Add(sc + 2);
        WaterGeometry.Triangles.Add(sc + 3);
        Vector2[] V = CB.GetTex();

        Vector2 uv = new Vector2(0, 0);
        WaterGeometry.UV.Add(uv);
        WaterGeometry.UV.Add(new Vector2(1, 0));
        WaterGeometry.UV.Add(new Vector2(1, 1));
        WaterGeometry.UV.Add(new Vector2(0, 1));
        //squareCount++;
    }

    public void FinalizeMesh()
    {
        CO.LandMeshData = CO.GetComponent<MeshFilter>().mesh;
        CO.LandMeshCollider = CO.GetComponent<MeshCollider>();
        CO.LandMeshData.Clear();
        CO.LandMeshData.vertices = LandGeometry.Vertices.ToArray();
        CO.LandMeshData.triangles = LandGeometry.Triangles.ToArray();
        CO.LandMeshData.uv = LandGeometry.UV.ToArray();
        CO.LandMeshData.RecalculateNormals();
        CO.LandMeshCollider.sharedMesh = CO.LandMeshData;

        CO.WaterMeshData = CO.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
        CO.WaterCollider = CO.transform.GetChild(0).GetComponent<MeshCollider>();
        CO.WaterMeshData.Clear();
        CO.WaterMeshData.vertices = WaterGeometry.Vertices.ToArray();
        CO.WaterMeshData.triangles = WaterGeometry.Triangles.ToArray();
        CO.WaterMeshData.uv = WaterGeometry.UV.ToArray();
        CO.WaterMeshData.RecalculateNormals();
        CO.WaterCollider.sharedMesh = CO.WaterMeshData;

        CO.Ready = true;
        for (int x = 0; x < WorldScript.ChunkSizeP2; x++)
        {
            for (int y = 0; y < WorldScript.ChunkSizeP2; y++)
            {
                for (int z = 0; z < WorldScript.ChunkSizeP2; z++)
                {
                    CO.Blocks[x][y][z].Data.ControlPoint =
                        Data[x][y][z].ControlPoint; // new BlockClass.BlockData();
                }
            }
        }

        UnityEngine.Debug.Log("Did Finalize");
    }

    private BlockClass GetBlock(int X, int Y, int Z)
    {
        return CO.Blocks[X + 1][Y + 1][Z + 1];
    }

    private BlockClass GetBlock(Vector3Int Pnt)
    {
        return CO.Blocks[Pnt.x + 1][Pnt.y + 1][Pnt.z + 1];
    }
}