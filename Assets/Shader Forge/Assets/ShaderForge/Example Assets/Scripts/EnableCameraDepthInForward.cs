//
// Attach this script to your camera in order to use depth nodes in forward rendering
//

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EnableCameraDepthInForward : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Set();
    }
#endif
    private void Start()
    {
        Set();
    }

    private void Set()
    {
        if (GetComponent<Camera>().depthTextureMode == DepthTextureMode.None)
            GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }
}