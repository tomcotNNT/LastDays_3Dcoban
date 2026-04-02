using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Giúp thanh máu luôn quay mặt về phía Camera
        transform.LookAt(transform.position + cam.forward);
    }
}