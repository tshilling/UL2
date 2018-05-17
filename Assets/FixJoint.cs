using UnityEngine;

public class FixJoint : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (!GetComponent<FixedJoint>())
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Destroy(this);
        }
    }
}