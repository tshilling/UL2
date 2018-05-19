using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Animations;
using UnityEngine;

public class PhysicsEngine : MonoBehaviour
{
    private static int _searchCount;
    public static int LastBreak = -1;
    public static int Depth = 6;
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
                    _physicsObjects.Add(neighbor.Object);
                    output.Add(neighbor);
                    if (parent.Block.Data.IsSolid)
                    {
                        var fj = neighbor.Object.AddComponent<FixedJoint>();
                        if (neighbor.Block.Strength < parent.Block.Strength)
                        {
                            fj.breakForce = neighbor.Block.Strength;
                            fj.breakTorque = neighbor.Block.Strength;
                        }
                        else
                        {
                            fj.breakForce = parent.Block.Strength;
                            fj.breakTorque = parent.Block.Strength;
                        }
                        fj.connectedBody = parent.Object.GetComponent<Rigidbody>();
                    }

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

        }
        q.AddRange(CheckDirections(Parent));

        while (q.Count > 0)
        {
            Parent = q[0];
            q.RemoveAt(0);
            if (((Parent.Block.Position - input).magnitude >= Depth)  && (_physicsObjects.Count > 100))
                Parent.Object.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            else
                q.AddRange(CheckDirections(Parent));
            if (_physicsObjects.Count > 2000)
                break;
        }
    }
    public void RebuildModel(Vector3 pnt)
    {
        UnityEngine.Debug.Log("In Here");
        if (_physicsObjects == null)
            _physicsObjects = new List<GameObject>();

        foreach (var g in _physicsObjects) DestroyImmediate(g);
        _physicsObjects.Clear(); // = new List<GameObject>();
 
        ForestFireSearchBuild(pnt);
        foreach (var g in _physicsObjects)
        {

            g.GetComponent<Rigidbody>().maxDepenetrationVelocity = 0.01f;
            g.GetComponent<Rigidbody>().WakeUp();
        }

        UnityEngine.Debug.Log("Total Phy: "+_physicsObjects.Count);
    }
    public List<Vector3> PhysicsUpdate()
    {
        var blocksToAdd = new List<Vector3>();
        
        if (_physicsObjects == null)
            return blocksToAdd;

       for (var i = 0; i < _physicsObjects.Count; i++)
            if (_physicsObjects[i] != null)
            {
                if (Vector3Int.RoundToInt(_physicsObjects[i].transform.position)!=_physicsObjects[i].GetComponent<PhysicsObject>().OriginalPosition)
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

        if(blocksToAdd.Count!=0) UnityEngine.Debug.Log("Blocks To Add: " + blocksToAdd.Count);
        return blocksToAdd;
    }
}