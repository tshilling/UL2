using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityStandardAssets.Characters.FirstPerson;

public class WorldScript : MonoBehaviour
{
    public GameObject baseChunk;
    public GameObject Player;
    public GameObject Target;
    public GameObject baseBlock;
    private Vector3Int LastPosition = new Vector3Int(0, 0, 0);
    public const byte ChunkSize = 16;
    public const byte ChunkSizeP2 = ChunkSize + 2;
    public static Vector2Int chunkDistance = new Vector2Int(2, 4);

    private Dictionary<ChunkObject, bool> chunksBuilding;
    private List<GameObject> LooseBlocks = new List<GameObject>();
    public WorldScript()
    {
        WorldCreator.Init(1234);
        //ChunkObject.myNoise.SetSeed(1234);
    }

    // Use this for initialization
    List<Vector3Int> RenderList = new List<Vector3Int>();

    void Awake()
    {
        this.gameObject.SetActive(true);
        UnityEngine.Debug.Log("Awake");
    }

    void Start()
    {
        //Player = GameObject.Find("FirstPersonCharacter");
        //Target = GameObject.Find("TargetCube");
        LoadScene();
    }

    public void LoadScene()
    {
        List<Vector3Int> TempList = new List<Vector3Int>();
        for (int X = -chunkDistance.x; X < chunkDistance.x; X += 1)
        {
            for (int Y = chunkDistance.y; Y >= 0; Y -= 1)
            {
                for (int Z = -chunkDistance.x; Z < chunkDistance.x; Z += 1)
                {
                    TempList.Add(new Vector3Int(X, Y, Z));
                }
            }
        }

        while (TempList.Count > 0)
        {
            float minV = 99999;
            int minI = 0;
            for (int i = 0; i < TempList.Count; i++)
            {
                float D = TempList[i].sqrMagnitude;
                if (D < minV)
                {
                    minV = D;
                    minI = i;
                }
            }

            RenderList.Add(TempList[minI]);
            TempList.RemoveAt(minI);
        }

        StartCoroutine(LoadInitScene());
    }

    // Update is called once per frame
    bool SceneLoaded = false;
    public Slider loadingSlider;
    int LoadCount = 0;


    private IEnumerator LoadInitScene()
    {
        Player.GetComponent<FirstPersonController>().Freeze();

        foreach (Vector3Int V in RenderList)
        {
            Vector3Int WPt = V * 16;
            GameObject GO = Instantiate(baseChunk, new Vector3(WPt.x, WPt.y, WPt.z), Quaternion.identity);
            ChunkObject CO = GO.GetComponent<ChunkObject>();
            CO.ChunkBuilt += OnChunkBuilt;
           
            Chunks.Add(WPt, GO);
            CO.asyncBuildChunk();
           
            yield return null;

        }


        while (LoadCount < RenderList.Count)
        {
            yield return null;
        }
        
        loadingSlider.gameObject.SetActive(false);
        Player.GetComponent<FirstPersonController>().UnFreeze();
        
        yield return null;
    }

    private void OnChunkBuilt(ChunkObject chunk)
    {
        LoadCount++;
    }

    void Update()
    {
        loadingSlider.value = ((float) LoadCount / (float) (RenderList.Count - 1)) / .9f;
    }

    public Dictionary<Vector3Int, GameObject> Chunks = new Dictionary<Vector3Int, GameObject>();
    private int WorldUpdateCount = 0;
    private Stack<GameObject> Deactivated = new Stack<GameObject>();
    bool Updating = false;

    private IEnumerator UpdateChunksVisible()
    {
        //Updating = true;
        if (!Updating)
        {
            Updating = true;
            WorldUpdateCount++;

            Vector3Int Pos = Vector3Int.FloorToInt(Player.transform.position / 16f) * 16;
            Pos.y = 0;
            foreach (Vector3Int V in RenderList)
            {
                Vector3Int WPt = V * 16 + Pos;
                GameObject tempChunk;
                if (Chunks.TryGetValue(WPt, out tempChunk))
                {
                    if (tempChunk.GetComponent<ChunkObject>().reMeshRequired)
                    {
                        tempChunk.GetComponent<ChunkObject>().asyncReMeshChunk();
                    }

                    tempChunk.GetComponent<ChunkObject>().UpdateCount = WorldUpdateCount;
                }
                else
                {
                    if (Deactivated.Count > 0)
                    {
                        GameObject GO = Deactivated.Pop();
                        if (GO.activeSelf)
                        {
                        }
                        else
                        {
                            GO.transform.position = WPt;
                            Chunks.Add(WPt, GO);
                            WorldCreator.InitChunk(GO);
                            GO.GetComponent<ChunkObject>().UpdateCount = WorldUpdateCount;
                            //GO.SetActive(true);
                            UnityEngine.Debug.Log("Grabbed deactivate: " + WPt);
                        }
                    }
                    else
                    {
                        GameObject GO = Instantiate(baseChunk, new Vector3(WPt.x, WPt.y, WPt.z), Quaternion.identity);
                        //GO.GetComponent<ChunkObject>().UpdateCount = WorldUpdateCount;
                        Chunks.Add(WPt, GO);
                        GO.GetComponent<ChunkObject>().asyncBuildChunk();
                        //WorldCreator.InitChunk(GO);
                    }
                }

                yield return null;
            }

            yield return null;
            for (int i = 0; i < Chunks.Keys.Count; i++)
            {
                Vector3Int WPt = (Chunks.ElementAt(i).Key - Pos);
                WPt.x = WPt.x / 16;
                WPt.y = WPt.y / 16;
                WPt.z = WPt.z / 16;
                if (!RenderList.Contains(WPt)) // Not visited last two times
                {
                    UnityEngine.Debug.Log("Remove: " + WPt);
                    GameObject tempChunk = Chunks.ElementAt(i).Value;
                    tempChunk.SetActive(false);
                    Deactivated.Push(tempChunk);
                    Chunks.Remove(Chunks.ElementAt(i).Key);
                    i--;
                }
            }

            Updating = false;
        }

        yield return null;
    }

    private void FixedUpdate()
    {
        //########################## Remerge Loose Blocks ###########################
        /*
        for(int i = 0; i < LooseBlocks.Count; i++)
        {
            GameObject GO = LooseBlocks[i];
            if(GO.GetComponent<Rigidbody>().velocity.sqrMagnitude < 0.001)  //Mesh not moving
            {
                List<GameObject> results = SetBlock(Vector3Int.RoundToInt(GO.transform.position), new BlockClass(BlockClass.BlockType.BedRock));
                foreach (GameObject GO2 in results)
                {
                    GO2.GetComponent<ChunkObject>().asyncReMeshChunk();
                    //GO.GetComponent<ChunkObject>().asyncReFaceChunk();
                }
                LooseBlocks.RemoveAt(i);
                GameObject.DestroyImmediate(GO);
                i--;
            }
            
        }*/
        //###########################################################################
        Vector3Int Delta = Vector3Int.FloorToInt(LastPosition - Player.transform.position);
        if (Mathf.Abs(Delta.x) > ChunkSize | Mathf.Abs(Delta.z) > ChunkSize)
        {
            Vector3Int Pos = Vector3Int.FloorToInt(Player.transform.position / 16f) * 16;
            //UpdateChunksVisible();

            StartCoroutine("UpdateChunksVisible");
            LastPosition = Pos;
        }

        ////////////////////////////
        RaycastHit RH;
        Transform FPC = Player.transform.GetChild(0);
        if (FPC != null)
        {
            Physics.Raycast(FPC.position, FPC.forward, out RH);
            if (RH.distance < 10)
            {
                Target.SetActive(true);
                float MW = Input.GetAxis("Mouse ScrollWheel");
                if (MW != 0)
                {
                    Target.transform.position = Vector3Int.RoundToInt(RH.point - RH.normal * .5f);
                    Vector3Int CP = Vector3Int.FloorToInt(Target.transform.position);
                    BlockClass B = GetBlock(CP);
                    if (B.Data.Type != BlockClass.BlockType.Air)
                    {
                        B.Data.Blockiness = (byte)Mathf.Clamp((float)B.Data.Blockiness + (MW * 160f), byte.MinValue + 1,
                            byte.MaxValue); //MW steps by 0.1
                        List<GameObject> results = SetBlock(CP, B);
                        foreach (GameObject GO in results)
                        {
                            WorldCreator.RefreshChunk(GO);
                        }
                    }
                }
                else if (Input.GetMouseButton(0)) //Destroy Block
                    Target.transform.position = Vector3Int.RoundToInt(RH.point - RH.normal * .5f);
                else if (Input.GetMouseButton(1)) // Build Block
                    Target.transform.position = Vector3Int.RoundToInt(RH.point + RH.normal * .5f);
                else
                {
                    Target.transform.position = Vector3Int.RoundToInt(RH.point - RH.normal * .5f);
                }

                if (Input.GetMouseButtonUp(1))
                {
                    Target.transform.position = Vector3Int.RoundToInt(RH.point + RH.normal * .5f);
                    Vector3Int CP = Vector3Int.FloorToInt(Target.transform.position);

                    List<GameObject> results = SetBlock(CP, new BlockClass(BlockClass.BlockType.Grass));
                    foreach (GameObject GO in results)
                    {
                        WorldCreator.RefreshChunk(GO);
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    Target.transform.position = Vector3Int.RoundToInt(RH.point - RH.normal * .5f);
                    Vector3Int CP = Vector3Int.FloorToInt(Target.transform.position);
                    GameObject tempBlock = GetBlockMesh(CP); // Extract Block from Mesh
                    BlockClass LastB = GetBlock(CP);
                    BlockClass NewB = new BlockClass(BlockClass.BlockType.Air);
                    NewB.Data.Blockiness = LastB.Data.Blockiness;
                    NewB.Data.Density = LastB.Data.Density;
                    NewB.Data.ControlPoint = LastB.Data.ControlPoint;
                    
                    if (tempBlock != null)
                    {
                        tempBlock.transform.Translate(RH.normal); //Pop up so it doesn't fall through the world
                        List<GameObject> results = SetBlock(CP, NewB);
                        foreach (GameObject GO in results)
                        {
                            GO.GetComponent<ChunkObject>().Face();
                            GO.GetComponent<ChunkObject>().postMesh();
                            //GO.GetComponent<ChunkObject>().asyncReFaceChunk();
                        }
                    }
                    LooseBlocks.Add(tempBlock);
                }
            }
            else
            {
                Target.SetActive(false);
            }
        }
        else
        {
            Target.SetActive(false);
        }
    }

    private BlockClass GetBlock(Vector3 Pnt)
    {
        Vector3Int ChunkPos = Vector3Int.FloorToInt(Pnt / 16f) * 16;
        Vector3Int BlockPos = Vector3Int.FloorToInt(Pnt) - ChunkPos;
        //Test that block is contained in Block
        if (BlockPos.x >= 0 & BlockPos.y >= 0 & BlockPos.z >= 0 & BlockPos.x < ChunkSize & BlockPos.y < ChunkSize &
            BlockPos.z < ChunkSize)
        {
            GameObject tempChunk;
            if (Chunks.TryGetValue(ChunkPos, out tempChunk))
            {
                return tempChunk.GetComponent<ChunkObject>().Blocks[BlockPos.x + 1][BlockPos.y + 1][BlockPos.z + 1];
            }
        }

        return new BlockClass(BlockClass.BlockType.Air);
    }

    public GameObject GetBlockMesh(Vector3 Pnt)
    {
        Vector3Int ChunkPos = Vector3Int.FloorToInt(Pnt / 16f) * 16;
        Vector3Int BlockPos = Vector3Int.FloorToInt(Pnt) - ChunkPos;
        //Test that block is contained in Block
        if (BlockPos.x >= 0 & BlockPos.y >= 0 & BlockPos.z >= 0 & BlockPos.x < ChunkSize & BlockPos.y < ChunkSize &
            BlockPos.z < ChunkSize)
        {
            GameObject result = Instantiate(baseBlock, Pnt, Quaternion.identity);
            result.GetComponent<LooseBlockScript>().InitBlockFromWorld(this, Pnt);
            //GameObject tempChunk;
            //if (Chunks.TryGetValue(ChunkPos, out tempChunk))
            //{
            //    return tempChunk.GetComponent<ChunkObject>().GetBlockMesh(BlockPos);
            //}
            return result;
        }

        return null;
    }

    public List<GameObject> SetBlock(Vector3 Pnt, BlockClass B)
    {
        List<GameObject> Affected = new List<GameObject>();
        Vector3Int ChunkPos = Vector3Int.FloorToInt(Pnt / 16f) * 16;
        Vector3Int BlockPos = Vector3Int.FloorToInt(Pnt) - ChunkPos;
        //Test that block is contained in Block
        UnityEngine.Debug.Log("SetBlock");
        if (BlockPos.x >= 0 & BlockPos.y >= 0 & BlockPos.z >= 0 & BlockPos.x < ChunkSize & BlockPos.y < ChunkSize &
            BlockPos.z < ChunkSize)
        {
            UnityEngine.Debug.Log("In SetBlock If: " + ChunkPos);
            GameObject tempChunk;
            if (Chunks.TryGetValue(ChunkPos, out tempChunk))
            {
                UnityEngine.Debug.Log("In Chunk: " + ChunkPos);
                tempChunk.GetComponent<ChunkObject>().Blocks[BlockPos.x + 1][BlockPos.y + 1][BlockPos.z + 1] = B;
                Affected.Add(tempChunk);
                Vector3Int ChunkOffset = new Vector3Int();
                if (BlockPos.x == 0)
                    ChunkOffset.x--;
                if (BlockPos.y == 0)
                    ChunkOffset.y--;
                if (BlockPos.z == 0)
                    ChunkOffset.z--;
                if (BlockPos.x == ChunkSize - 1)
                    ChunkOffset.x++;
                if (BlockPos.y == ChunkSize - 1)
                    ChunkOffset.y++;
                if (BlockPos.z == ChunkSize - 1)
                    ChunkOffset.z++;
                if (ChunkOffset.x != 0)
                {
                    if (Chunks.TryGetValue(ChunkPos + new Vector3Int(ChunkOffset.x * 16, 0, 0), out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>().Blocks[(BlockPos.x + 1) - ChunkOffset.x * ChunkSize][
                            BlockPos.y + 1][BlockPos.z + 1] = B;
                        Affected.Add(tempChunk);
                    }
                }

                if (ChunkOffset.y != 0)
                {
                    if (Chunks.TryGetValue(ChunkPos + new Vector3Int(0, ChunkOffset.y * 16, 0), out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>().Blocks[BlockPos.x + 1][
                            (BlockPos.y + 1) - ChunkOffset.y * ChunkSize][BlockPos.z + 1] = B;
                        Affected.Add(tempChunk);
                    }
                }

                if (ChunkOffset.z != 0)
                {
                    if (Chunks.TryGetValue(ChunkPos + new Vector3Int(0, 0, ChunkOffset.z * 16), out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>().Blocks[BlockPos.x + 1][BlockPos.y + 1][
                            (BlockPos.z + 1) - ChunkOffset.z * ChunkSize] = B;
                        Affected.Add(tempChunk);
                    }
                }

                if (ChunkOffset.x != 0 & ChunkOffset.y != 0)
                {
                    if (Chunks.TryGetValue(ChunkPos + new Vector3Int(ChunkOffset.x * 16, ChunkOffset.y * 16, 0),
                        out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>().Blocks[(BlockPos.x + 1) - ChunkOffset.x * ChunkSize][
                            (BlockPos.y + 1) - ChunkOffset.y * ChunkSize][BlockPos.z + 1] = B;
                        Affected.Add(tempChunk);
                    }
                }

                if (ChunkOffset.x != 0 & ChunkOffset.z != 0)
                {
                    if (Chunks.TryGetValue(ChunkPos + new Vector3Int(ChunkOffset.x * 16, 0, ChunkOffset.z * 16),
                        out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>().Blocks[(BlockPos.x + 1) - ChunkOffset.x * ChunkSize]
                            [BlockPos.y + 1][(BlockPos.z + 1) - ChunkOffset.z * ChunkSize] = B;
                        Affected.Add(tempChunk);
                    }
                }

                if (ChunkOffset.y != 0 & ChunkOffset.z != 0)
                {
                    if (Chunks.TryGetValue(ChunkPos + new Vector3Int(0, ChunkOffset.y * 16, ChunkOffset.z * 16),
                        out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>().Blocks[BlockPos.x + 1][
                                (BlockPos.y + 1) - ChunkOffset.y * ChunkSize][
                                (BlockPos.z + 1) - ChunkOffset.z * ChunkSize]
                            = B;
                        Affected.Add(tempChunk);
                    }
                }

                if (ChunkOffset.x != 0 & ChunkOffset.y != 0 & ChunkOffset.z != 0)
                {
                    if (Chunks.TryGetValue(
                        ChunkPos + new Vector3Int(ChunkOffset.x * 16, ChunkOffset.y * 16, ChunkOffset.z * 16),
                        out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>().Blocks[(BlockPos.x + 1) - ChunkOffset.x * ChunkSize][
                                (BlockPos.y + 1) - ChunkOffset.y * ChunkSize][
                                (BlockPos.z + 1) - ChunkOffset.z * ChunkSize]
                            = B;
                        Affected.Add(tempChunk);
                    }
                }
            }
        }

        return Affected;
    }

    private void LateUpdate()
    {
    }
}