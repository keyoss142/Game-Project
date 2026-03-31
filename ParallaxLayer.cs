using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxMultiplier = 0.5f;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier, 0f, 0f);

        lastCameraPosition = cameraTransform.position;
    }
}