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
    public Material TargetMaterial1;
    public Material TargetMaterial2;
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

    Stopwatch WatchdogTimer = new Stopwatch();
   
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

        WatchdogTimer.Start();
        while (LoadCount < RenderList.Count)
        {
            UnityEngine.Debug.Log("Waiting");
            if (WatchdogTimer.ElapsedMilliseconds > 3000)
            {
                break;
            }

            yield return null;
        }

        UnityEngine.Debug.Log("Done Loading");
        loadingSlider.gameObject.SetActive(false);
        Player.GetComponent<FirstPersonController>().UnFreeze();
        StartCoroutine(UpdateWorld());
        yield return null;
    }

    private void OnChunkBuilt(ChunkObject chunk)
    {
        LoadCount++;
        WatchdogTimer.Reset();
        WatchdogTimer.Start();
    }

    void Update()
    {
        loadingSlider.value = ((float) LoadCount / (float) (RenderList.Count - 1)) / .9f;
    }

    public Dictionary<Vector3Int, GameObject> Chunks = new Dictionary<Vector3Int, GameObject>();
    private int WorldUpdateCount = 0;
    private Stack<GameObject> Deactivated = new Stack<GameObject>();
    bool Updating = false;
    private void CheckIfUpdateRequired(GameObject GO)
    {
        ChunkObject CO = GO.GetComponent<ChunkObject>();
        if (CO.RefreshRequired == ChunkObject.RemeshEnum.FaceUrgent | CO.RefreshRequired == ChunkObject.RemeshEnum.Face)
        {
            CO.Face();
            CO.postMesh();
        }
        if (CO.RefreshRequired == ChunkObject.RemeshEnum.MeshUrgent | CO.RefreshRequired == ChunkObject.RemeshEnum.Mesh)
        {
            CO.Mesh();
            CO.postMesh();
        }
        CO.RefreshRequired = ChunkObject.RemeshEnum.None;
    }
    private void LoadNewChunk(Vector3Int WPt)
    {
        if (Deactivated.Count > 0)
        {
            GameObject GO = Deactivated.Pop();
            GO.SetActive(false);
            GO.transform.position = WPt;
            Chunks.Add(WPt, GO);
            GO.GetComponent<ChunkObject>().asyncBuildChunk();
            GO.SetActive(true);
        }
        else
        {
            GameObject GO = Instantiate(baseChunk, new Vector3(WPt.x, WPt.y, WPt.z), Quaternion.identity);
            Chunks.Add(WPt, GO);
            GO.GetComponent<ChunkObject>().asyncBuildChunk();
        }

    }
    private IEnumerator UpdateWorld()
    {
        Stopwatch SW = new Stopwatch();
        SW.Start();
        while (true)
        {
            Vector3Int ChunkPos = Vector3Int.FloorToInt(Player.transform.position / 16f);
            ChunkPos.y = 0;
            Vector3Int Pos = ChunkPos * 16;
            //Check For Remesh
            UnityEngine.Debug.Log("Loose Blocks: " + LooseBlocks.Count);
            for (int i = 0; i < LooseBlocks.Count; i++)
            {
                if (LooseBlocks[i] != null)
                {
                    if (LooseBlocks[i].GetComponent<LooseBlockScript>().ReadyForRemesh)
                    {
                        LooseBlocks[i].GetComponent<LooseBlockScript>().MeldBlockIntoWorld();
                        LooseBlocks.RemoveAt(i);
                        i--;
                    }
                }
                else
                {
                    LooseBlocks.RemoveAt(i);
                    i--;
                }
            }
            //Remove unneeded chunks
            for (int i = 0; i < Chunks.Keys.Count; i++)
            {
                Vector3Int WPt = (Chunks.ElementAt(i).Key - Pos);
                WPt.x = WPt.x / 16;
                WPt.y = WPt.y / 16;
                WPt.z = WPt.z / 16;
                if (!RenderList.Contains(WPt))
                {
                    UnityEngine.Debug.Log("Remove: " + WPt);
                    GameObject tempChunk = Chunks.ElementAt(i).Value;
                    tempChunk.SetActive(false);
                    Deactivated.Push(tempChunk);
                    Chunks.Remove(Chunks.ElementAt(i).Key);
                    i--;
                }
            }

            foreach (Vector3Int V in RenderList)
            {
                Vector3Int WPt = V * 16 + Pos;
                GameObject tempChunk;
                // If chunk exists
                if (Chunks.TryGetValue(WPt, out tempChunk))
                {
                    CheckIfUpdateRequired(tempChunk);
                }
                else // If it doesn't exist yet but should
                {
                    LoadNewChunk(WPt);
                }
                if (SW.ElapsedMilliseconds > 15)
                {
                    SW.Stop();
                    yield return null;
                    SW.Reset();
                    SW.Start();
                }
            }

            if (SW.ElapsedMilliseconds > 15)
            {
                SW.Stop();
                yield return null;
                SW.Reset();
                SW.Start();
            }
        }
    }
    private List<GameObject> PerformLooseBlock(Vector3Int CP)
    {
        List<GameObject> results = new List<GameObject>();
        PhysicsEngine.FillRTNType R = PhysicsEngine.FillSearchBlocks(this, CP, 4);
        List<GameObject> NewBlocks = new List<GameObject>();
        for (int i = 0; i < R.Position.Count; i++)
        {
            CP = R.Position[i];
            GameObject tempBlock = GetBlockMesh(CP); // Extract Block from Mesh
            NewBlocks.Add(tempBlock);
            tempBlock.GetComponent<Rigidbody>().Sleep();
            BlockClass LastB = GetBlock(CP);
            BlockClass NewB = new BlockClass(BlockClass.BlockType.Air);
            NewB.Data.Blockiness = LastB.Data.Blockiness;
            NewB.Data.Density = LastB.Data.Density;
            NewB.Data.ControlPoint = LastB.Data.ControlPoint;

            List<GameObject> r2 = SetBlock(CP, NewB);
            foreach (GameObject GO in r2)
            {
                if (!results.Contains(GO))
                {
                    results.Add(GO);
                }
            }
        }
        //################################
        // Need to now join each block with its neighbor.
            
        for(int i = 0; i < NewBlocks.Count; i++)
        {
            Vector3 Pos = NewBlocks[i].transform.position;
            for(int i2 = 0; i2 < NewBlocks.Count; i2++)
            {
                if (i != i2)
                {
                    float Dis = (NewBlocks[i2].transform.position - NewBlocks[i].transform.position).sqrMagnitude;
                    if(Dis <= 1)
                    {
                        Joint J = NewBlocks[i].AddComponent<FixedJoint>();
                        J.breakForce = 300;
                        J.breakTorque = 300;
                        J.connectedBody = NewBlocks[i2].GetComponent<Rigidbody>();
                    }
                }
            }

        }
        //Form Joints between NewBlocks and World
        for (int i = 0; i < NewBlocks.Count; i++)
        {
            Vector3 Pos = NewBlocks[i].transform.position;
            for(int i2 = 0; i2 < BlockProperties.DirectionVector.Length; i2++)
            {
                BlockClass B = GetBlock(Pos + BlockProperties.DirectionVector[i2]);
                if (B.Data.isSolid)
                {
                    Joint J = NewBlocks[i].AddComponent<FixedJoint>();
                    J.breakForce = Mathf.Infinity;
                    J.breakTorque = Mathf.Infinity;
                    J.connectedBody = GetChunkGameObject(Pos + BlockProperties.DirectionVector[i2]).GetComponent<Rigidbody>();
                }
            }
        }
            //################################
        foreach (GameObject GO in NewBlocks)
        {
            GO.GetComponent<Rigidbody>().WakeUp();
        }
        return results;
    }
    private void FixedUpdate()
    {
       
        RaycastHit RH;
        Transform FPC = Player.transform.GetChild(0);
        if (FPC != null)
        {
            BlockClass BC = GetBlock(FPC.position);
            if(BC.Data.Type == BlockClass.BlockType.Water)
            {
                FPC.gameObject.GetComponent<UnderWater>().Under = true;
            }
            else
            {

                FPC.gameObject.GetComponent<UnderWater>().Under = false;
            }
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
                        B.Data.Blockiness = (byte)Mathf.Clamp((float)B.Data.Blockiness + (MW * 160f),
                            byte.MinValue + 1,
                            byte.MaxValue); //MW steps by 0.1
                        List<GameObject> results = SetBlock(CP, B);
                        foreach (GameObject GO in results)
                        {
                            GO.GetComponent<ChunkObject>().Face();//.Mesh();
                            GO.GetComponent<ChunkObject>().postMesh();
                            //GO.GetComponent<ChunkObject>().RefreshRequired = ChunkObject.RemeshEnum.FaceUrgent;
                        }
                    }
                }
                else if (Input.GetMouseButton(0)) //Destroy Block
                {
                    Target.transform.position = Vector3Int.RoundToInt(RH.point - RH.normal * .5f);
                    Vector3Int CP = Vector3Int.FloorToInt(Target.transform.position);
                    LooseBlockScript.InitBlockFromWorld(Target, this, CP);
                    Target.GetComponent<MeshRenderer>().material = TargetMaterial1;
                }
                else if (Input.GetMouseButton(1)) // Build Block
                {
                    Target.transform.position = Vector3Int.RoundToInt(RH.point + RH.normal * .5f);
                    Vector3Int CP = Vector3Int.FloorToInt(Target.transform.position);
                    LooseBlockScript.InitBlockAsCube(Target);
                    Target.GetComponent<MeshRenderer>().material = TargetMaterial2;
                }
                else
                {
                    Target.transform.position = Vector3Int.RoundToInt(RH.point - RH.normal * .5f);
                    Vector3Int CP = Vector3Int.FloorToInt(Target.transform.position);
                    LooseBlockScript.InitBlockFromWorld(Target, this, CP);
                    Target.GetComponent<MeshRenderer>().material = TargetMaterial1;
                }

                if (Input.GetMouseButtonUp(1))  // Destroy Block
                {
                    List<GameObject> results = new List<GameObject>();
                    Target.transform.position = Vector3Int.RoundToInt(RH.point + RH.normal * .5f);
                    Vector3Int CP = Vector3Int.FloorToInt(Target.transform.position);
                    results.AddRange(SetBlock(CP, new BlockClass(BlockClass.BlockType.Grass)));

                    foreach (GameObject GO in results)
                    {
                        //GO.GetComponent<ChunkObject>().RefreshRequired = ChunkObject.RemeshEnum.MeshUrgent;
                        GO.GetComponent<ChunkObject>().Mesh();
                        GO.GetComponent<ChunkObject>().postMesh();
                    }
                    
                }

                if (Input.GetMouseButtonUp(0))  // Destroy Block
                {
                    Target.transform.position = Vector3Int.RoundToInt(RH.point - RH.normal * .5f);
                    Vector3Int CP = Vector3Int.FloorToInt(Target.transform.position);
                    GameObject tempBlock = GetBlockMesh(CP); // Extract Block from Mesh
                    BlockClass LastB = GetBlock(CP);
                    BlockClass NewB = new BlockClass(BlockClass.BlockType.Air);
                    NewB.Data.Blockiness = LastB.Data.Blockiness;
                    NewB.Data.Density = LastB.Data.Density;
                    NewB.Data.ControlPoint = LastB.Data.ControlPoint;
                    List<GameObject> results = new List<GameObject>();
                    if (tempBlock != null)
                    {
                        tempBlock.transform.Translate(RH.normal); //Pop up so it doesn't fall through the world
                        results.AddRange(SetBlock(CP, NewB));

                        //PhysicsSeed = CP;

                        //StartCoroutine(UpdateVoxelPhysics());
                    }
                    List<GameObject> R2 = PerformLooseBlock(CP);
                    foreach (GameObject GO in results)
                    {
                        //GO.GetComponent<ChunkObject>().RefreshRequired = ChunkObject.RemeshEnum.MeshUrgent;
                        GO.GetComponent<ChunkObject>().Face();
                        GO.GetComponent<ChunkObject>().postMesh();
                        //GO.GetComponent<ChunkObject>().asyncReFaceChunk();
                    }
                    foreach (GameObject GO in R2)
                    {
                        if (!results.Contains(GO))
                        {
                            //GO.GetComponent<ChunkObject>().RefreshRequired = ChunkObject.RemeshEnum.MeshUrgent;
                            GO.GetComponent<ChunkObject>().Face();
                            GO.GetComponent<ChunkObject>().postMesh();
                            //GO.GetComponent<ChunkObject>().asyncReFaceChunk();
                        }
                    }
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
    public GameObject GetChunkGameObject(Vector3 Pnt)
    {
        Vector3Int ChunkPos = Vector3Int.FloorToInt(Pnt / 16f) * 16;
        GameObject tempChunk;
        if (Chunks.TryGetValue(ChunkPos, out tempChunk))
        {
            return tempChunk;
        }
        return null;
    }
    public BlockClass GetBlock(Vector3 Pnt)
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
            LooseBlocks.Add(result);
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
        if (BlockPos.x >= 0 & BlockPos.y >= 0 & BlockPos.z >= 0 & BlockPos.x < ChunkSize & BlockPos.y < ChunkSize &
            BlockPos.z < ChunkSize)
        {
            GameObject tempChunk;
            if (Chunks.TryGetValue(ChunkPos, out tempChunk))
            {
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
    Vector3Int PhysicsSeed = new Vector3Int();

    private IEnumerator UpdateVoxelPhysics()
    {
        List<Vector3Int> Result = new List<Vector3Int>();
        yield return null;
    }
    private void LateUpdate()
    {
    }
}