using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixJoint : MonoBehaviour {
    void FixedUpdate()
    {
        if (!GetComponent<FixedJoint>())
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Destroy(this);
        }
    }
}
