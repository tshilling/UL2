using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public Vector3Int OriginalPosition;
    public BlockClass SourceBlock;

    // Use this for 
    private void Awake()
    {
        OriginalPosition = Vector3Int.FloorToInt(transform.position);
    }
   
}