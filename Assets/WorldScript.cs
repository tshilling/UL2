using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using Debug = UnityEngine.Debug;

public class WorldScript : MonoBehaviour
{
    public static WorldScript ActiveWorld;

    public BlockClass.BlockType ActiveBlockType = BlockClass.BlockType.Dirt;
    // There was a tendency to get double mouse clicks for some reason.  This static is used to ensure the mosue is released between click events
    private static readonly bool[] MouseState = {false, false, false};

    // These are the basic prefabs for building the world.  They must be loaded in the "awake" on this worlf
    public static GameObject BaseBlock;
    private GameObject _baseChunk;
    private GameObject _physicsPrefab;
    private GameObject _target;
    private GameObject _player;

    public Dictionary<Vector3Int, GameObject> Chunks = new Dictionary<Vector3Int, GameObject>();

    private Dictionary<ChunkObject, bool> chunksBuilding;
    private readonly Stack<GameObject> Deactivated = new Stack<GameObject>();
    private Vector3Int LastPosition = new Vector3Int(0, 0, 0);
    private int _loadCount;
    private Vector3Int PhysicsSeed = new Vector3Int();
    private PhysicsEngine MyPhysics;
    private List<GameObject> PhysBlocks = new List<GameObject>();
    private bool SceneLoaded = false;

    public Slider loadingSlider;
    public List<GameObject> LooseBlocks = new List<GameObject>();


    private readonly List<Vector3Int> RenderList = new List<Vector3Int>();

    public Material TargetMaterial1;
    public Material TargetMaterial2;
    private bool Updating = false;

    private readonly Stopwatch WatchdogTimer = new Stopwatch();
    private int WorldUpdateCount = 0;

    private void Awake()
    {
        ActiveWorld = this;
        _target = Instantiate(Resources.Load("Prefabs/TargetPrefab") as GameObject,new Vector3(0,0,0),Quaternion.identity);
        _baseChunk = Instantiate(Resources.Load("Prefabs/ChunkPrefab") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
        BaseBlock = Instantiate(Resources.Load("Prefabs/BlockPrefab") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
        _physicsPrefab = Instantiate(Resources.Load("Prefabs/PhysicsPrefab") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
        gameObject.SetActive(true);
        Debug.Log("Awake");
    }

    private void Start()
    {
        _player = Instantiate(Resources.Load("myFPS") as GameObject, new Vector3(0, 150, 0), Quaternion.identity);


        try
        { 
            MyPhysics = gameObject.AddComponent<PhysicsEngine>();
            //MyPhysics = new PhysicsEngine();
            MyPhysics.World = this;
            MyPhysics.PhysicsPrefab = _physicsPrefab;
            MyPhysics.OtherMat = TargetMaterial1;
            LoadScene();
        }
        catch (System.Exception error)
        {
            UnityEngine.Debug.Log(error.Message);
            throw;
        }

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
        //_player.GetComponent<RigidBodyFPSController>().Freeze();

        foreach (var V in RenderList)
        {
            var WPt = V * 16;
            var GO = Instantiate(_baseChunk, new Vector3(WPt.x, WPt.y, WPt.z), Quaternion.identity);
            var CO = GO.GetComponent<ChunkObject>();
            CO.ChunkBuilt += OnChunkBuilt;

            Chunks.Add(WPt, GO);
           // CO.asyncBuildChunk();
            CO.PreSeed();
            CO.Seed();
            CO.Mesh();
            CO.PostMesh();

            yield return null;
        }

        WatchdogTimer.Start();
        while (_loadCount < RenderList.Count)
        {
            
            if (WatchdogTimer.ElapsedMilliseconds > 3000) break;

            yield return null;
        }

        Debug.Log("Done Loading");
        loadingSlider.gameObject.SetActive(false);
        
       // _player.GetComponent<RigidBodyFPSController>().UnFreeze();
        StartCoroutine(UpdateWorld());
        yield return null;
    }

    private void OnChunkBuilt(ChunkObject chunk)
    {
        _loadCount++;
        WatchdogTimer.Reset();
        WatchdogTimer.Start();
    }

    private void Update()
    {
        loadingSlider.value = _loadCount / (float) (RenderList.Count - 1) / .9f;
    }

    private void CheckIfUpdateRequired(GameObject GO)
    {
        try
        {
            var CO = GO.GetComponent<ChunkObject>();
            if ((CO.RefreshRequired == ChunkObject.RemeshEnum.FaceUrgent) |
                (CO.RefreshRequired == ChunkObject.RemeshEnum.Face))
            {
                //  LockAllLooseBlocks(true);
                CO.Face();
                CO.PostMesh();
                // LockAllLooseBlocks(false);
            }

            if ((CO.RefreshRequired == ChunkObject.RemeshEnum.MeshUrgent) |
                (CO.RefreshRequired == ChunkObject.RemeshEnum.Mesh))
            {
                //LockAllLooseBlocks(true);
                CO.Mesh();
                CO.PostMesh();
                //LockAllLooseBlocks(false);
            }

            CO.RefreshRequired = ChunkObject.RemeshEnum.None;
        }
        catch (System.Exception error)
        {
            Debug.Log("Error in checking for update requried: " + error.Message);
        }

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
        var GO = Instantiate(_baseChunk, new Vector3(WPt.x, WPt.y, WPt.z), Quaternion.identity);
        var CO = GO.GetComponent<ChunkObject>();
        Chunks.Add(WPt, GO);

        CO.PreSeed();
        CO.Seed();
        CO.Mesh();
        CO.PostMesh();
       // CO.asyncBuildChunk();
        //   }
    }

    private IEnumerator UpdateWorld()
    {
        var SW = new Stopwatch();
        SW.Start();
        while (true)
        {
            var ChunkPos = Vector3Int.FloorToInt(_player.transform.position / 16f);
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
            _target.SetActive(true);
            _target.transform.SetPositionAndRotation(Vector3Int.RoundToInt(RH.point + RH.normal * .5f),
                Quaternion.identity);
            CP = Vector3Int.FloorToInt(_target.transform.position);
            _target.GetComponent<LooseBlockScript>().InitBlockFromCube(CP);
            _target.GetComponent<MeshRenderer>().material = TargetMaterial2;
        }
        else if(Input.GetMouseButton(0)) // Build Block
        {
            _target.SetActive(true);
            _target.transform.SetPositionAndRotation(Vector3Int.RoundToInt(RH.point - RH.normal * .5f),
                Quaternion.identity);
            CP = Vector3Int.FloorToInt(_target.transform.position);
            _target.GetComponent<LooseBlockScript>().InitBlockFromWorld(CP);
            _target.GetComponent<MeshRenderer>().material = TargetMaterial1;
        }
        else
        {
            _target.SetActive(false);
        }

        if (Input.GetMouseButtonUp(0)) // Destroy Block
        {
            if (MouseState[0])
            {
                _target.transform.SetPositionAndRotation(Vector3Int.RoundToInt(RH.point - RH.normal * .5f),Quaternion.identity);
                var LastB = GetBlock(_target.transform.position);
                LastB.ChangeTypeTo(BlockClass.BlockType.Air);
                var results = SetBlock(_target.transform.position, LastB);
                foreach (var GO in results)
                {
                    GO.GetComponent<ChunkObject>().RefreshRequired = ChunkObject.RemeshEnum.Face;
                    //GO.GetComponent<ChunkObject>().Face();
                    //GO.GetComponent<ChunkObject>().PostMesh();
                }
                MyPhysics.PhysicsPrefab = _physicsPrefab;
                MyPhysics.RebuildModel(LastB.Position);
            }

            MouseState[0] = false;
        }

        if (Input.GetMouseButtonUp(1)) // Create Block
        {
            if (MouseState[1])
            {
                _target.transform.SetPositionAndRotation(Vector3Int.RoundToInt(RH.point + RH.normal * .5f),
                    Quaternion.identity);
                var NewB = new BlockClass(ActiveWorld.ActiveBlockType, _target.transform.position);
                var results = SetBlock(_target.transform.position, NewB);
                foreach (var GO in results)
                {
                    GO.GetComponent<ChunkObject>().RefreshRequired = ChunkObject.RemeshEnum.Mesh;
                    //GO.GetComponent<ChunkObject>().Mesh();
                    //GO.GetComponent<ChunkObject>().PostMesh();
                }

                MyPhysics.PhysicsPrefab = _physicsPrefab;
                MyPhysics.RebuildModel(_target.transform.position);
            }

            MouseState[1] = false;
        }
    }

    private void InteractWithLoose(RaycastHit RH)
    {
        if (Input.GetMouseButtonDown(0))
            MouseState[0] = true;
        if (Input.GetMouseButtonDown(1))
            MouseState[1] = true;
        var CP = new Vector3Int();
        var GO = RH.rigidbody.gameObject;
        if (Input.GetMouseButton(0)) //Destroy Block
        {
            _target.SetActive(true);
            if (MouseState[0])
            {
                _target.transform.SetPositionAndRotation(GO.transform.position,GO.transform.rotation);
                _target.GetComponent<MeshRenderer>().material = TargetMaterial1;
                _target.GetComponent<MeshFilter>().mesh = GO.GetComponent<MeshFilter>().mesh;
            }
        }
        else if (Input.GetMouseButton(1)) // Build Block
        {
            if (MouseState[1])
            {
                _target.SetActive(true);
                _target.transform.SetPositionAndRotation(RH.transform.position + RH.normal,RH.transform.rotation);
                _target.GetComponent<LooseBlockScript>().InitBlockFromCube(Vector3Int.FloorToInt(_target.transform.position));
                _target.GetComponent<MeshRenderer>().material = TargetMaterial2;
            }
        }
        else
        {
            _target.SetActive(false);
        }

        if (Input.GetMouseButtonUp(0)) // Destroy Block
        {
            CP = Vector3Int.FloorToInt(GO.transform.position);
            Destroy(GO);
        }

        if (Input.GetMouseButtonUp(1)) // Set Block
        {
            _target.transform.SetPositionAndRotation(RH.transform.position + RH.normal, RH.transform.rotation);    // Set initial Position
            var temp = Instantiate(BaseBlock, _target.transform.position, RH.transform.rotation);
            temp.GetComponent<LooseBlockScript>().InitBlockFromCube(_target.transform.position);
            temp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
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
            var LastB = GetBlock(Results[i]);
            var NewB = new BlockClass(BlockClass.BlockType.Air,LastB.Position);
            NewB.Data.Blockiness = LastB.Data.Blockiness;
            NewB.Data.Density = LastB.Data.Density;
            NewB.Data.ControlPoint = LastB.Data.ControlPoint;
            var R = SetBlock(Results[i], NewB);
            foreach (var GO in R)
                if (!EffectedChunks.Contains(GO))
                    EffectedChunks.Add(GO);
        }

        for (var i = 0; i < NewBlocks.Count; i++)
        for (var i2 = i + 1; i2 < NewBlocks.Count; i2++)
            if ((NewBlocks[i].transform.position - NewBlocks[i2].transform.position).sqrMagnitude < 1.1)
            {
                var FJ = NewBlocks[i].AddComponent<FixedJoint>();
                FJ.breakForce = 1000;
                FJ.breakTorque = 1000;
                FJ.connectedBody = NewBlocks[i2].GetComponent<Rigidbody>();
            }

        foreach (var GO in EffectedChunks)
        {
            GO.GetComponent<ChunkObject>().Mesh();
            GO.GetComponent<ChunkObject>().PostMesh();
        }

        RaycastHit RH;
        var FPC = _player.transform.GetChild(0);
        if (FPC != null)
        {
            var BC = GetBlock(FPC.position);
            Physics.Raycast(FPC.position, FPC.forward, out RH); //Layer mask 0 for chunk
            if (RH.distance < 10)
            {
                // If it hit a chunk object, something meshed into the world
                if (RH.collider)
                {
                    if (RH.collider.gameObject)
                    {
                        if (RH.collider.name == "Solid")
                        {
                            InteractWithChunk(RH);
                        }
                        else if (RH.collider.gameObject.GetComponent<LooseBlockScript>())
                        {
                            InteractWithLoose(RH);
                        }
                    }
                    else
                    {
                        _target.SetActive(false);
                    }
                }
                else
                {
                    _target.SetActive(false);
                }
            }
            else
            {
                _target.SetActive(false);
            }
        }
        else
        {
            _target.SetActive(false);
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

        return new BlockClass(BlockClass.BlockType.Air,Pnt);
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
            var result = Instantiate(BaseBlock, Pnt, Quaternion.identity);
            result.GetComponent<LooseBlockScript>().InitBlockFromWorld(Pnt);
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