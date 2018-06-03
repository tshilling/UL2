using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public bool ReadyToPhysics = false;
    public Vector3Int OriginalPosition;
    public BlockClass SourceBlock;
    public bool Grounded = false;
    public int Debounce = 0;
    public static int VisitCount = 0;
    public int Visit = 0;
    // Use this for 
    private void Awake()
    {
        OriginalPosition = Vector3Int.FloorToInt(transform.position);
    }
    private void OnJointBreak(float breakForce)
    {
        this.ReadyToPhysics = true;
        return;
    }

}