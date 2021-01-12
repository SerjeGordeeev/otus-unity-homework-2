using UnityEngine;

internal sealed class LookAtCamera : MonoBehaviour
{
    Transform mainCameraTransform;

    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }
    
    void Update()
    {
        transform.LookAt(mainCameraTransform);
    }
}