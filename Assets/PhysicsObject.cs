using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public Vector3Int OriginalPosition;

    // Use this for 
    private void Awake()
    {
        OriginalPosition = Vector3Int.FloorToInt(transform.position);
    }

    private void OnJointBreak(float breakForce)
    {
        PhysicsEngine.LastBreak = 10;
    }
}