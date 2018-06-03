using UnityEngine;

namespace Assets
{
    public class MesherClass
    {
        public static bool CalculateControlPoints(BlockClass[][][] blocks)
        {
            for (var X = 0; X < blocks.Length-1; X++)
            for (var Y = 0; Y < blocks[X].Length-1; Y++)
            for (var Z = 0; Z < blocks[X][Y].Length-1; Z++)
            {
                if (!blocks[X][Y][Z].Data.CpLocked)
                {
                    blocks[X][Y][Z].Data.CpLocked = true;
                    blocks[X][Y][Z].Data.ControlPoint = Vector3.zero;
                    byte edgeCrossings = 0;
                    BlockClass.BlockData[] adjacent =
                    {
                        blocks[X][Y][Z].Data,
                        blocks[X + 1][Y][Z].Data,
                        blocks[X + 1][Y + 1][Z].Data,
                        blocks[X][Y + 1][Z].Data,
                        blocks[X][Y][Z + 1].Data,
                        blocks[X + 1][Y][Z + 1].Data,
                        blocks[X + 1][Y + 1][Z + 1].Data,
                        blocks[X][Y + 1][Z + 1].Data
                    };

                    for (var i = 0; i < 12; i++)
                        //if(adjacent[BlockProperties.BlockEdges[i, 0]].IsSolid != adjacent[BlockProperties.BlockEdges[i, 1]].IsSolid) { 
                        if (((adjacent[BlockProperties.BlockEdges[i, 0]].Density <= 0) &&
                             (adjacent[BlockProperties.BlockEdges[i, 1]].Density > 0)) ||
                            ((adjacent[BlockProperties.BlockEdges[i, 1]].Density < 0) &&
                             (adjacent[BlockProperties.BlockEdges[i, 0]].Density >= 0)))
                        {

                                    blocks[X][Y][Z].Data.ControlPoint += Vector3.LerpUnclamped(
                                    BlockProperties.BlockPtsC[BlockProperties.BlockEdges[i, 0]],
                                    BlockProperties.BlockPtsC[BlockProperties.BlockEdges[i, 1]],
                                    -(float) adjacent[BlockProperties.BlockEdges[i, 0]].Density /
                                    (adjacent[BlockProperties.BlockEdges[i, 1]].Density -
                                     (float) adjacent[BlockProperties.BlockEdges[i, 0]].Density));
                                edgeCrossings++;
                        }

                    if (edgeCrossings != 0)
                    {
                        blocks[X][Y][Z].Data.ControlPoint /= (float) edgeCrossings;
                        var maxB = adjacent[0].Blockiness;
                        for (var i = 1; i < adjacent.Length; i++)
                        {
                            if (adjacent[i].Blockiness > maxB)
                            {
                                maxB = adjacent[i].Blockiness;
                            }
                        }

                        blocks[X][Y][Z].Data.ControlPoint = Vector3.Lerp(blocks[X][Y][Z].Data.ControlPoint, new Vector3(0.5f, 0.5f, 0.5f), maxB / 255f);
                    }
                    else
                    {
                        blocks[X][Y][Z].Data.ControlPoint = new Vector3(0.5f, 0.5f, 0.5f);
                    }
                }
            }
            return true;
        }  
        public static bool Face(BlockClass[][][] blocks, GeometryData landGeometry, GeometryData waterGeometry)
        {
            landGeometry.Clear();
            waterGeometry.Clear();

            for (var Z = 0; Z < BlockProperties.ChunkSize; Z++)
                for (var Y = 0; Y < BlockProperties.ChunkSize; Y++)
                    for (var X = 0; X < BlockProperties.ChunkSize; X++)
                    {
                        var V = new Vector3Int(X, Y, Z);
                        BlockClass.BlockData currentData = blocks[V.x+1][V.y+1][V.z+1].Data;
                        if (currentData.Type == BlockClass.BlockType.Water)
                        {
                            for (byte Dir = 0; Dir < 6; Dir++)
                            {
                                if (blocks[1+V.x+ BlockProperties.DirectionVector[Dir].x][1+V.y+ BlockProperties.DirectionVector[Dir].y][1+V.z + BlockProperties.DirectionVector[Dir].z].Data.Type != BlockClass.BlockType.Water)
                                    AddFaceWater(blocks, waterGeometry, Dir, V);
                            }
                        }
                        else
                        {
                            for (byte Dir = 0; Dir < 6; Dir++)
                            {
                                var B0 = (byte)(currentData.Occlude & (1 << Dir));
                                if (B0 <= 0) continue;
                                var B1 = (byte)(blocks[1+V.x + BlockProperties.DirectionVector[Dir].x][1+V.y + BlockProperties.DirectionVector[Dir].y][1+V.z + BlockProperties.DirectionVector[Dir].z].Data.Occlude &
                                                 (1 << (5 - Dir)));
                                if (B1 == 0)
                                    AddFace(blocks, landGeometry, Dir, V);
                            }
                        }
                    }

            return true;
        }

        const float pixelOffset = (1.0f / 256.0f);
        const float pixelOffset2 = (2.0f / 256.0f);
        private static void AddFace(BlockClass[][][] blocks, GeometryData geo, byte dir, Vector3Int center)
        {
            for (var i = 0; i < 4; i++)
            {
                var blk = center + BlockProperties.FacePts[BlockProperties.BlockFaces[dir, i]];
                geo.Vertices.Add(blk + blocks[blk.x+1][blk.y+1][blk.z+1].Data.ControlPoint);
                geo.Normals.Add(BlockProperties.DirectionVector[dir]);
            }

            var v1 = geo.Vertices[geo.Vertices.Count - 1] -
                     geo.Vertices[geo.Vertices.Count - 2];
            var v2 = geo.Vertices[geo.Vertices.Count - 3] -
                     geo.Vertices[geo.Vertices.Count - 2];

            var N = Vector3.Cross(v1, v2).normalized;
            if (N.y > .3) dir = (byte)BlockClass.Direction.Up; //Up

            var sc = geo.Vertices.Count - 4; // squareCount << 2;//Multiply by 4
            if ((geo.Vertices[geo.Vertices.Count - 3] - geo.Vertices[geo.Vertices.Count - 1]).sqrMagnitude <
                (geo.Vertices[geo.Vertices.Count - 4] - geo.Vertices[geo.Vertices.Count - 2]).sqrMagnitude)
            {
                geo.Triangles.Add(sc);
                geo.Triangles.Add(sc + 1);
                geo.Triangles.Add(sc + 3);
                geo.Triangles.Add(sc + 1);
                geo.Triangles.Add(sc + 2);
                geo.Triangles.Add(sc + 3);
            }
            else
            {
                geo.Triangles.Add(sc);
                geo.Triangles.Add(sc + 1);
                geo.Triangles.Add(sc + 2);
                geo.Triangles.Add(sc);
                geo.Triangles.Add(sc + 2);
                geo.Triangles.Add(sc + 3);

            }

            var v = blocks[center.x + 1][center.y + 1][center.z + 1].GetTex();

            var uv = new Vector2(v[dir].x / 16f + pixelOffset, (15 - v[dir].y) / 16f + pixelOffset);
            geo.UV.Add(uv);
            geo.UV.Add(new Vector2(uv.x + BlockProperties.TUnit - pixelOffset2, uv.y));
            geo.UV.Add(new Vector2(uv.x + BlockProperties.TUnit - pixelOffset2, uv.y + BlockProperties.TUnit - pixelOffset2));
            geo.UV.Add(new Vector2(uv.x, uv.y + BlockProperties.TUnit - pixelOffset2));
            //squareCount++;
        }
        private static void AddFaceWater(BlockClass[][][] blocks,GeometryData geo, byte dir, Vector3Int center)
        {
            for (var i = 0; i < 4; i++)
            {
                var FacePoint = BlockProperties.FacePts[BlockProperties.BlockFaces[dir, i]];
                var blk = center + FacePoint;
                var currentVert = blk + blocks[blk.x + 1][blk.y + 1][blk.z + 1].Data.ControlPoint;
                geo.Vertices.Add(currentVert);
                var newNormal = BlockProperties.DirectionVector[dir];
                /*
                if (dir == 0)   //UP
                {
                    var workingNormal = new Vector3Int(0, 0, 0);
                    var off = (FacePoint * new Vector3Int(2, 2, 2) + new Vector3Int(1, 1, 1));
                    if(blocks[center.x+off.x + 1][center.y + 1][center.z + 1].Data.Type != BlockClass.BlockType.Water)
                    {
                        workingNormal.x = off.x;
                    }
                    if (blocks[center.x  + 1][center.y + 1][center.z + off.z + 1].Data.Type != BlockClass.BlockType.Water)
                    {
                        workingNormal.z = off.z;
                    }
                    if (workingNormal.sqrMagnitude > 0.001)
                        newNormal = workingNormal;
                }*/

                geo.Normals.Add(newNormal);

            }

            var sc = geo.Vertices.Count - 4; // squareCount << 2;//Multiply by 4
            geo.Triangles.Add(sc);
            geo.Triangles.Add(sc + 1);
            geo.Triangles.Add(sc + 3);
            geo.Triangles.Add(sc + 1);
            geo.Triangles.Add(sc + 2);
            geo.Triangles.Add(sc + 3);
            //var v = blocks[center.x + 1][center.y + 1][center.z + 1].GetTex();

            var uv = new Vector2(0, 0);
            geo.UV.Add(uv);
            geo.UV.Add(new Vector2(1, 0));
            geo.UV.Add(new Vector2(1, 1));
            geo.UV.Add(new Vector2(0, 1));
        }
        public static bool PostMesh(GameObject gameObject, GeometryData geometry, bool calcNormals)
        {
            Mesh meshData = gameObject.GetComponent<MeshFilter>().mesh;
            meshData.Clear();
            meshData.vertices = geometry.Vertices.ToArray();
            meshData.triangles = geometry.Triangles.ToArray();
            meshData.uv = geometry.UV.ToArray();
            if (calcNormals)
            {
                meshData.RecalculateNormals();
            }
            else
            {
                meshData.normals = geometry.Normals.ToArray();
            }
            if (gameObject.GetComponent<MeshCollider>())
            {
                MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
                meshCollider.sharedMesh = meshData;
            }
            return true;
        }
    }
}
