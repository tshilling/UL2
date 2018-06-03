using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Animations;
using UnityEngine;

public class PhysicsEngine : MonoBehaviour
{
    private static int _searchCount;
    public static int LastBreak = -1;
    public static int Depth = 6;
    public static int MaxBlockCount = (int)Mathf.Pow(((Depth) * 2), 3);
    public Material OtherMat;
    private List<GameObject> _physicsObjects;
    public GameObject PhysicsPrefab;
    public WorldScript World;
 
    public static int MaxSearchRadius = 8;
    /*########################### Forest Fire Seach #########################################
        Flood-fill (node, target-color, replacement-color):
      1. If target-color is equal to replacement-color, return.
      2. If color of node is not equal to target-color, return.
      3. Set Q to the empty queue.
      4. Set the color of node to replacement-color.
      5. Add node to the end of Q.
      6. While Q is not empty:
      7.     Set n equal to the first element of Q.
      8.     Remove first element from Q.
      9.     If the color of the node to the west of n is target-color,
                 set the color of that node to replacement-color and add that node to the end of Q.
     10.     If the color of the node to the east of n is target-color,
                 set the color of that node to replacement-color and add that node to the end of Q.
     11.     If the color of the node to the north of n is target-color,
                 set the color of that node to replacement-color and add that node to the end of Q.
     12.     If the color of the node to the south of n is target-color,
                 set the color of that node to replacement-color and add that node to the end of Q.
     13. Continue looping until Q is exhausted.
     14. Return.
    *///#######################################################################################

    private struct FFS_struct
    {
        public GameObject Object;
        public BlockClass Block;
    }

    private List<FFS_struct> CheckDirections(FFS_struct parent)
    {
        List<FFS_struct> output = new List<FFS_struct>();
        foreach (var dir in BlockProperties.DirectionVector)
        {
            FFS_struct neighbor = new FFS_struct();
            Vector3Int pos = parent.Block.Position + dir;
            neighbor.Block = World.GetBlock(pos);
            neighbor.Block.Position = pos;
            if (neighbor.Block.Data.IsSolid)
            {
                if (neighbor.Block.SearchMarker != _searchCount)
                {
                    neighbor.Block.SearchMarker = _searchCount;
                    neighbor.Object = Instantiate(PhysicsPrefab, neighbor.Block.Position, Quaternion.identity);
                    neighbor.Object.GetComponent<Rigidbody>().Sleep();
                    neighbor.Object.GetComponent<PhysicsObject>().SourceBlock = neighbor.Block;
                    _physicsObjects.Add(neighbor.Object);
                    output.Add(neighbor);
                    /*
                    if (parent.Block.Data.IsSolid)
                    {
                        var fj = neighbor.Object.AddComponent<FixedJoint>();
                        if (neighbor.Block.Strength < parent.Block.Strength)
                        {
                            fj.breakForce = neighbor.Block.Strength;
                            fj.breakTorque = neighbor.Block.Strength/2f;
                        }
                        else
                        {
                            fj.breakForce = parent.Block.Strength;
                            fj.breakTorque = parent.Block.Strength/2f;
                        }
                        fj.connectedBody = parent.Object.GetComponent<Rigidbody>();
                    }
                    */
                }
            }
        }
        return output;
    }
    
    private void ForestFireSearchBuild(Vector3 input)
    {
        _searchCount++;
        _physicsObjects.Clear();

        // 2. Initialize Empty Q
        List<FFS_struct> q = new List<FFS_struct>();

        FFS_struct Parent = new FFS_struct
        {
            Block = World.GetBlock(input)
        };
        Parent.Block.SearchMarker = _searchCount;
        Parent.Block.Position = Vector3Int.RoundToInt(input);
        if (Parent.Block.Data.IsSolid)
        {
            Parent.Object = Instantiate(PhysicsPrefab, Parent.Block.Position, Quaternion.identity);
            Parent.Object.GetComponent<Rigidbody>().Sleep();
            _physicsObjects.Add(Parent.Object);
            Parent.Object.GetComponent<PhysicsObject>().SourceBlock = Parent.Block;

        }
        q.AddRange(CheckDirections(Parent));

        while (q.Count > 0)
        {
            Parent = q[0];
            q.RemoveAt(0);
            if (((Parent.Block.Position - input).magnitude >= Depth) && (_physicsObjects.Count > 100))  // This ensures that really long items get generated correctly.
            {
                Parent.Object.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                Parent.Object.GetComponent<PhysicsObject>().Grounded = true;
                Parent.Object.GetComponent<PhysicsObject>().SourceBlock = Parent.Block;
            }
            else
                q.AddRange(CheckDirections(Parent));
            if (_physicsObjects.Count > MaxBlockCount)
                break;
        }
        for(int i =0; i < _physicsObjects.Count; i++)
        {
            if (_physicsObjects[i] != null)
            {
                var P1 = _physicsObjects[i].transform.position;
                var ParentBlock = _physicsObjects[i].GetComponent<PhysicsObject>().SourceBlock;
                for (int i2 = i + 1; i2 < _physicsObjects.Count; i2++)
                {
                    if (_physicsObjects[i2] != null)
                    {
                        var P2 = _physicsObjects[i2].transform.position;

                        var Distance = (P2 - P1).magnitude;
                        if (Distance < 1.1)
                        {
                            var NeighborBlock = _physicsObjects[i2].GetComponent<PhysicsObject>().SourceBlock;
                            var FJ = _physicsObjects[i].AddComponent<FixedJoint>();
                            FJ.breakForce = Mathf.Min(NeighborBlock.Strength, ParentBlock.Strength);
                            FJ.breakTorque = Mathf.Min(NeighborBlock.Strength, ParentBlock.Strength);
                            FJ.connectedBody = _physicsObjects[i2].GetComponent<Rigidbody>();
                        }
                    }
                }
                if (!_physicsObjects[i].GetComponent<PhysicsObject>().Grounded)
                {
                    _physicsObjects[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    _physicsObjects[i].GetComponent<Rigidbody>().useGravity = true;

                }
            }
        }


    }
    bool rebuilding = false;
    int delaycount = 0;
    public void RebuildModel(Vector3 pnt)
    {
        rebuilding = true;
        if (_physicsObjects == null)
            _physicsObjects = new List<GameObject>();

        foreach (var g in _physicsObjects) DestroyImmediate(g);
        _physicsObjects.Clear(); // = new List<GameObject>();
 
        ForestFireSearchBuild(pnt);
        foreach (var g in _physicsObjects)
        {
            //g.GetComponent<Rigidbody>().maxDepenetrationVelocity = 0.01f;
            g.GetComponent<Rigidbody>().WakeUp();
        }
        rebuilding = false;
    }
    public List<Vector3> PhysicsUpdate()
    {
        var blocksToAdd = new List<Vector3>();
        if (_physicsObjects == null)
            return blocksToAdd;
       for (var i = 0; i < _physicsObjects.Count; i++)
            if (_physicsObjects[i] != null)
            { 
                if (((_physicsObjects[i].GetComponent<PhysicsObject>().OriginalPosition - _physicsObjects[i].transform.position).sqrMagnitude >= 1) || _physicsObjects[i].GetComponent<PhysicsObject>().ReadyToPhysics)
                 {
                        var cp = _physicsObjects[i].GetComponent<PhysicsObject>().OriginalPosition;
                        if (!blocksToAdd.Contains(cp))
                            blocksToAdd.Add(cp);
                        DestroyImmediate(_physicsObjects[i]);
                        _physicsObjects.RemoveAt(i--);
                }
                
            }
            else
            {
                _physicsObjects.RemoveAt(i--);
            }
       return blocksToAdd;
    }
}