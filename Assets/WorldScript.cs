using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using Debug = UnityEngine.Debug;

public class WorldScript : MonoBehaviour
{
    private static readonly bool[] MouseState = {false, false, false};
    public GameObject baseBlock;
    public GameObject baseChunk;

    public Dictionary<Vector3Int, GameObject> Chunks = new Dictionary<Vector3Int, GameObject>();

    private Dictionary<ChunkObject, bool> chunksBuilding;
    private readonly Stack<GameObject> Deactivated = new Stack<GameObject>();
    private Vector3Int LastPosition = new Vector3Int(0, 0, 0);
    private int LoadCount;
    public Slider loadingSlider;
    public List<GameObject> LooseBlocks = new List<GameObject>();
    private PhysicsEngine MyPhysics;

    private List<GameObject> PhysBlocks = new List<GameObject>();
    public GameObject PhysicsPrefab;
    private Vector3Int PhysicsSeed = new Vector3Int();
    public GameObject Player;

    // Use this for initialization
    private readonly List<Vector3Int> RenderList = new List<Vector3Int>();

    // Update is called once per frame
    private bool SceneLoaded = false;
    public GameObject Target;
    public Material TargetMaterial1;
    public Material TargetMaterial2;
    private bool Updating = false;

    private readonly Stopwatch WatchdogTimer = new Stopwatch();
    private int WorldUpdateCount = 0;

    private void Awake()
    {
        gameObject.SetActive(true);
        Debug.Log("Awake");
    }

    private void Start()
    {
        MyPhysics = new PhysicsEngine();
        MyPhysics.World = this;
        MyPhysics.PhysicsPrefab = PhysicsPrefab;
        MyPhysics.OtherMat = TargetMaterial1;
        LoadScene();
    }

    public void LoadScene()
    {
        var TempList = new List<Vector3Int>();
        for (var X = -BlockProperties.chunkDistance.x; X < BlockProperties.chunkDistance.x; X += 1)
        for (var Y = BlockProperties.chunkDistance.y; Y >= 0; Y -= 1)
        for (var Z = -BlockProperties.chunkDistance.x; Z < BlockProperties.chunkDistance.x; Z += 1)
            TempList.Add(new Vector3Int(X, Y, Z));

        while (TempList.Count > 0)
        {
            float minV = 99999;
            var minI = 0;
            for (var i = 0; i < TempList.Count; i++)
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

    private IEnumerator LoadInitScene()
    {
        Player.GetComponent<FirstPersonController>().Freeze();

        foreach (var V in RenderList)
        {
            var WPt = V * 16;
            var GO = Instantiate(baseChunk, new Vector3(WPt.x, WPt.y, WPt.z), Quaternion.identity);
            var CO = GO.GetComponent<ChunkObject>();
            CO.ChunkBuilt += OnChunkBuilt;

            Chunks.Add(WPt, GO);
            CO.asyncBuildChunk();

            yield return null;
        }

        WatchdogTimer.Start();
        while (LoadCount < RenderList.Count)
        {
            Debug.Log("Waiting");
            if (WatchdogTimer.ElapsedMilliseconds > 3000) break;

            yield return null;
        }

        Debug.Log("Done Loading");
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

    private void Update()
    {
        loadingSlider.value = LoadCount / (float) (RenderList.Count - 1) / .9f;
    }

    private void CheckIfUpdateRequired(GameObject GO)
    {
        var CO = GO.GetComponent<ChunkObject>();
        if ((CO.RefreshRequired == ChunkObject.RemeshEnum.FaceUrgent) |
            (CO.RefreshRequired == ChunkObject.RemeshEnum.Face))
        {
            //  LockAllLooseBlocks(true);
            CO.Face();
            CO.postMesh();
            // LockAllLooseBlocks(false);
        }

        if ((CO.RefreshRequired == ChunkObject.RemeshEnum.MeshUrgent) |
            (CO.RefreshRequired == ChunkObject.RemeshEnum.Mesh))
        {
            //LockAllLooseBlocks(true);
            CO.Mesh();
            CO.postMesh();
            //LockAllLooseBlocks(false);
        }

        CO.RefreshRequired = ChunkObject.RemeshEnum.None;
    }

    private void LoadNewChunk(Vector3Int WPt)
    {
        //   if (Deactivated.Count > 0)
        //   {
        //       GameObject GO = Deactivated.Pop();
        //       GO.SetActive(false);
        //       GO.transform.position = WPt;
        //       Chunks.Add(WPt, GO);
        //       GO.GetComponent<ChunkObject>().asyncBuildChunk();
        //       GO.SetActive(true);
        //   }
        //   else
        //   {
        var GO = Instantiate(baseChunk, new Vector3(WPt.x, WPt.y, WPt.z), Quaternion.identity);
        Chunks.Add(WPt, GO);
        GO.GetComponent<ChunkObject>().asyncBuildChunk();
        //   }
    }

    private IEnumerator UpdateWorld()
    {
        var SW = new Stopwatch();
        SW.Start();
        while (true)
        {
            var ChunkPos = Vector3Int.FloorToInt(Player.transform.position / 16f);
            ChunkPos.y = 0;
            var Pos = ChunkPos * 16;
            //Check For Remesh
            for (var i = 0; i < LooseBlocks.Count; i++)
                if (LooseBlocks[i] != null)
                {
                    if (LooseBlocks[i].GetComponent<LooseBlockScript>().ReadyForRemesh)
                    {
                        LooseBlocks[i].GetComponent<LooseBlockScript>().MeldBlockIntoWorld();
                        //LooseBlocks.RemoveAt(i);
                        i--;
                    }
                }
                else
                {
                    LooseBlocks.RemoveAt(i);
                    i--;
                }

            //Remove unneeded chunks
            for (var i = 0; i < Chunks.Keys.Count; i++)
            {
                var WPt = Chunks.ElementAt(i).Key - Pos;
                WPt.x = WPt.x / 16;
                WPt.y = WPt.y / 16;
                WPt.z = WPt.z / 16;
                if (!RenderList.Contains(WPt))
                {
                    var tempChunk = Chunks.ElementAt(i).Value;
                    tempChunk.SetActive(false);
                    Deactivated.Push(tempChunk);
                    Chunks.Remove(Chunks.ElementAt(i).Key);
                    i--;
                }
            }

            foreach (var V in RenderList)
            {
                var WPt = V * 16 + Pos;
                GameObject tempChunk;
                // If chunk exists
                if (Chunks.TryGetValue(WPt, out tempChunk))
                    CheckIfUpdateRequired(tempChunk);
                else // If it doesn't exist yet but should
                    LoadNewChunk(WPt);
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

    private void InteractWithChunk(RaycastHit RH)
    {
        if (Input.GetMouseButtonDown(0))
            MouseState[0] = true;
        if (Input.GetMouseButtonDown(1))
            MouseState[1] = true;
        var CP = new Vector3Int();
        if (Input.GetMouseButton(1)) // Build Block
        {
            Target.transform.SetPositionAndRotation(Vector3Int.RoundToInt(RH.point + RH.normal * .5f),
                Quaternion.identity);
            CP = Vector3Int.FloorToInt(Target.transform.position);
            Target.GetComponent<LooseBlockScript>().InitBlockFromCube(this);
            Target.GetComponent<MeshRenderer>().material = TargetMaterial2;
        }
        else
        {
            Target.transform.SetPositionAndRotation(Vector3Int.RoundToInt(RH.point - RH.normal * .5f),
                Quaternion.identity);
            CP = Vector3Int.FloorToInt(Target.transform.position);
            Target.GetComponent<LooseBlockScript>().InitBlockFromWorld(this, CP);
            Target.GetComponent<MeshRenderer>().material = TargetMaterial1;
        }

        if (Input.GetMouseButtonUp(0)) // Destroy Block
        {
            if (MouseState[0])
            {
                Target.transform.SetPositionAndRotation(Vector3Int.RoundToInt(RH.point - RH.normal * .5f),
                    Quaternion.identity);
                CP = Vector3Int.FloorToInt(Target.transform.position);
                var LastB = GetBlock(Target.transform.position);
                var NewB = new BlockClass(BlockClass.BlockType.Air);
                NewB.Data.Blockiness = LastB.Data.Blockiness;
                NewB.Data.Density = LastB.Data.Density;
                NewB.Data.ControlPoint = LastB.Data.ControlPoint;
                var results = SetBlock(Target.transform.position, NewB);
                foreach (var GO in results)
                {
                    GO.GetComponent<ChunkObject>().Face();
                    GO.GetComponent<ChunkObject>().postMesh();
                }

                MyPhysics.PhysicsPrefab = PhysicsPrefab;
                MyPhysics.RefreshModel(Target.transform.position);
            }

            MouseState[0] = false;
        }

        if (Input.GetMouseButtonUp(1)) // Create Block
        {
            if (MouseState[1])
            {
                Target.transform.SetPositionAndRotation(Vector3Int.RoundToInt(RH.point + RH.normal * .5f),
                    Quaternion.identity);
                var NewB = new BlockClass(BlockClass.BlockType.BedRock);
                var results = SetBlock(Target.transform.position, NewB);
                foreach (var GO in results)
                {
                    GO.GetComponent<ChunkObject>().Mesh();
                    GO.GetComponent<ChunkObject>().postMesh();
                }

                MyPhysics.PhysicsPrefab = PhysicsPrefab;
                MyPhysics.RefreshModel(Target.transform.position);
            }

            MouseState[1] = false;
        }
    }

    private void InteractWithLoose(RaycastHit RH)
    {
        var CP = new Vector3Int();
        var GO = RH.rigidbody.gameObject;
        if (Input.GetMouseButton(0)) //Destroy Block
        {
            Target.transform.SetPositionAndRotation(GO.transform.position,
                GO.transform.rotation); // Vector3Int.RoundToInt(RH.point - RH.normal * .5f);
            Target.GetComponent<MeshRenderer>().material = TargetMaterial1;
            Target.GetComponent<MeshFilter>().mesh = GO.GetComponent<MeshFilter>().mesh;
        }
        else if (Input.GetMouseButton(1)) // Build Block
        {
            Target.transform.SetPositionAndRotation(Vector3Int.RoundToInt(RH.point + RH.normal * .5f),
                Quaternion.identity);
            CP = Vector3Int.FloorToInt(Target.transform.position);
            Target.GetComponent<LooseBlockScript>().InitBlockFromCube(this);
            Target.GetComponent<MeshRenderer>().material = TargetMaterial2;
        }
        else
        {
            Target.transform.SetPositionAndRotation(GO.transform.position,
                GO.transform.rotation); // Vector3Int.RoundToInt(RH.point - RH.normal * .5f);
            Target.GetComponent<MeshRenderer>().material = TargetMaterial1;
            Target.GetComponent<MeshFilter>().mesh = GO.GetComponent<MeshFilter>().mesh;
        }

        if (Input.GetMouseButtonUp(0)) // Destroy Block
        {
            CP = Vector3Int.FloorToInt(GO.transform.position);
            Destroy(GO);
        }

        if (Input.GetMouseButtonUp(1)) // Set Block
        {
            Target.transform.SetPositionAndRotation(Vector3Int.RoundToInt(RH.point + RH.normal * .5f),
                Quaternion.identity);
            CP = Vector3Int.FloorToInt(Target.transform.position);
            var temp = Instantiate(baseBlock, CP, Quaternion.identity);
            temp.GetComponent<LooseBlockScript>().InitBlockFromCube(this);
            temp.transform.position = CP;
            Joint J = temp.AddComponent<FixedJoint>();
            J.breakForce = 300;
            J.breakTorque = 300;
            J.connectedBody = RH.rigidbody;
            LooseBlocks.Add(temp);
        }
    }

    private void FixedUpdate()
    {
        var Results = MyPhysics.PhysicsUpdate();
        var NewBlocks = new List<GameObject>();
        var EffectedChunks = new List<GameObject>();
        for (var i = 0; i < Results.Count; i++)
        {
            var tempBlock = GetBlockMesh(Results[i]); // Extract Block from Mesh
            tempBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            NewBlocks.Add(tempBlock);
            tempBlock.GetComponent<Rigidbody>().Sleep();
            var LastB = GetBlock(Results[i]);
            var NewB = new BlockClass(BlockClass.BlockType.Air);
            NewB.Data.Blockiness = LastB.Data.Blockiness;
            NewB.Data.Density = LastB.Data.Density;
            NewB.Data.ControlPoint = LastB.Data.ControlPoint;
            var R = SetBlock(Results[i], NewB);
            foreach (var GO in R)
                if (!EffectedChunks.Contains(GO))
                    EffectedChunks.Add(GO);
        }

        for (var i = 0; i < NewBlocks.Count; i++)
        for (var i2 = i + 1; i2 < Results.Count; i2++)
            if ((NewBlocks[i].transform.position - NewBlocks[i2].transform.position).sqrMagnitude < 1.1)
            {
                var FJ = NewBlocks[i].AddComponent<FixedJoint>();
                FJ.breakForce = 300;
                FJ.breakTorque = 300;
                FJ.connectedBody = NewBlocks[i2].GetComponent<Rigidbody>();
            }

        foreach (var GO in EffectedChunks)
        {
            GO.GetComponent<ChunkObject>().Face();
            GO.GetComponent<ChunkObject>().postMesh();
        }

        RaycastHit RH;
        var FPC = Player.transform.GetChild(0);
        if (FPC != null)
        {
            var BC = GetBlock(FPC.position);
            if (BC.Data.Type == BlockClass.BlockType.Water)
                FPC.gameObject.GetComponent<UnderWater>().Under = true;
            else
                FPC.gameObject.GetComponent<UnderWater>().Under = false;
            Physics.Raycast(FPC.position, FPC.forward, out RH); //Layer mask 0 for chunk
            if (RH.distance < 10)
            {
                Target.SetActive(true);
                // If it hit a chunk object, something meshed into the world
                if (RH.rigidbody)
                {
                    if (RH.rigidbody.gameObject)
                    {
                        if (RH.rigidbody.gameObject.GetComponent<ChunkObject>())
                        {
                            InteractWithChunk(RH);
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
        var ChunkPos = Vector3Int.FloorToInt(Pnt / 16f) * 16;
        GameObject tempChunk;
        if (Chunks.TryGetValue(ChunkPos, out tempChunk)) return tempChunk;
        return null;
    }

    public BlockClass GetBlock(Vector3 Pnt)
    {
        var ChunkPos = Vector3Int.FloorToInt(Pnt / 16f) * 16;
        var BlockPos = Vector3Int.FloorToInt(Pnt) - ChunkPos;
        //Test that block is contained in Block
        if ((BlockPos.x >= 0) & (BlockPos.y >= 0) & (BlockPos.z >= 0) & (BlockPos.x < BlockProperties.ChunkSize) &
            (BlockPos.y < BlockProperties.ChunkSize) &
            (BlockPos.z < BlockProperties.ChunkSize))
        {
            GameObject tempChunk;
            if (Chunks.TryGetValue(ChunkPos, out tempChunk))
                return tempChunk.GetComponent<ChunkObject>().Blocks[BlockPos.x + 1][BlockPos.y + 1][BlockPos.z + 1];
        }

        return new BlockClass(BlockClass.BlockType.Air);
    }

    public GameObject GetBlockMesh(Vector3 Pnt)
    {
        var ChunkPos = Vector3Int.FloorToInt(Pnt / 16f) * 16;
        var BlockPos = Vector3Int.FloorToInt(Pnt) - ChunkPos;
        //Test that block is contained in Block
        if ((BlockPos.x >= 0) & (BlockPos.y >= 0) & (BlockPos.z >= 0) & (BlockPos.x < BlockProperties.ChunkSize) &
            (BlockPos.y < BlockProperties.ChunkSize) &
            (BlockPos.z < BlockProperties.ChunkSize))
        {
            var result = Instantiate(baseBlock, Pnt, Quaternion.identity);
            result.GetComponent<LooseBlockScript>().InitBlockFromWorld(this, Pnt);
            LooseBlocks.Add(result);
            return result;
        }

        return null;
    }

    public List<GameObject> SetBlock(Vector3 Pnt, BlockClass B)
    {
        var Affected = new List<GameObject>();
        var PS = BlockProperties.GetPosition(Pnt);
        var BlockPos = PS.BlockInChunk;
        //Test that block is contained in Block
        if ((BlockPos.x >= 0) & (BlockPos.y >= 0) & (BlockPos.z >= 0) & (BlockPos.x < BlockProperties.ChunkSize) &
            (BlockPos.y < BlockProperties.ChunkSize) &
            (BlockPos.z < BlockProperties.ChunkSize))
        {
            GameObject tempChunk;
            if (Chunks.TryGetValue(PS.ChunkInWorld, out tempChunk))
            {
                tempChunk.GetComponent<ChunkObject>().Blocks[BlockPos.x + 1][BlockPos.y + 1][BlockPos.z + 1] = B;
                Affected.Add(tempChunk);
                var ChunkOffset = new Vector3Int();
                if (BlockPos.x == 0)
                    ChunkOffset.x--;
                if (BlockPos.y == 0)
                    ChunkOffset.y--;
                if (BlockPos.z == 0)
                    ChunkOffset.z--;
                if (BlockPos.x == BlockProperties.ChunkSize - 1)
                    ChunkOffset.x++;
                if (BlockPos.y == BlockProperties.ChunkSize - 1)
                    ChunkOffset.y++;
                if (BlockPos.z == BlockProperties.ChunkSize - 1)
                    ChunkOffset.z++;
                if (ChunkOffset.x != 0)
                    if (Chunks.TryGetValue(PS.ChunkInWorld + new Vector3Int(ChunkOffset.x * 16, 0, 0), out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>()
                            .Blocks[BlockPos.x + 1 - ChunkOffset.x * BlockProperties.ChunkSize][
                                BlockPos.y + 1][BlockPos.z + 1] = B;
                        Affected.Add(tempChunk);
                    }

                if (ChunkOffset.y != 0)
                    if (Chunks.TryGetValue(PS.ChunkInWorld + new Vector3Int(0, ChunkOffset.y * 16, 0), out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>().Blocks[BlockPos.x + 1][
                            BlockPos.y + 1 - ChunkOffset.y * BlockProperties.ChunkSize][BlockPos.z + 1] = B;
                        Affected.Add(tempChunk);
                    }

                if (ChunkOffset.z != 0)
                    if (Chunks.TryGetValue(PS.ChunkInWorld + new Vector3Int(0, 0, ChunkOffset.z * 16), out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>().Blocks[BlockPos.x + 1][BlockPos.y + 1][
                            BlockPos.z + 1 - ChunkOffset.z * BlockProperties.ChunkSize] = B;
                        Affected.Add(tempChunk);
                    }

                if ((ChunkOffset.x != 0) & (ChunkOffset.y != 0))
                    if (Chunks.TryGetValue(PS.ChunkInWorld + new Vector3Int(ChunkOffset.x * 16, ChunkOffset.y * 16, 0),
                        out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>()
                            .Blocks[BlockPos.x + 1 - ChunkOffset.x * BlockProperties.ChunkSize][
                                BlockPos.y + 1 - ChunkOffset.y * BlockProperties.ChunkSize][BlockPos.z + 1] = B;
                        Affected.Add(tempChunk);
                    }

                if ((ChunkOffset.x != 0) & (ChunkOffset.z != 0))
                    if (Chunks.TryGetValue(PS.ChunkInWorld + new Vector3Int(ChunkOffset.x * 16, 0, ChunkOffset.z * 16),
                        out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>()
                            .Blocks[BlockPos.x + 1 - ChunkOffset.x * BlockProperties.ChunkSize]
                            [BlockPos.y + 1][BlockPos.z + 1 - ChunkOffset.z * BlockProperties.ChunkSize] = B;
                        Affected.Add(tempChunk);
                    }

                if ((ChunkOffset.y != 0) & (ChunkOffset.z != 0))
                    if (Chunks.TryGetValue(PS.ChunkInWorld + new Vector3Int(0, ChunkOffset.y * 16, ChunkOffset.z * 16),
                        out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>().Blocks[BlockPos.x + 1][
                                BlockPos.y + 1 - ChunkOffset.y * BlockProperties.ChunkSize][
                                BlockPos.z + 1 - ChunkOffset.z * BlockProperties.ChunkSize]
                            = B;
                        Affected.Add(tempChunk);
                    }

                if ((ChunkOffset.x != 0) & (ChunkOffset.y != 0) & (ChunkOffset.z != 0))
                    if (Chunks.TryGetValue(
                        PS.ChunkInWorld + new Vector3Int(ChunkOffset.x * 16, ChunkOffset.y * 16, ChunkOffset.z * 16),
                        out tempChunk))
                    {
                        tempChunk.GetComponent<ChunkObject>()
                                .Blocks[BlockPos.x + 1 - ChunkOffset.x * BlockProperties.ChunkSize][
                                    BlockPos.y + 1 - ChunkOffset.y * BlockProperties.ChunkSize][
                                    BlockPos.z + 1 - ChunkOffset.z * BlockProperties.ChunkSize]
                            = B;
                        Affected.Add(tempChunk);
                    }
            }
        }

        return Affected;
    }

    private void LateUpdate()
    {
    }
}