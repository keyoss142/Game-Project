using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    private Transform cameraTransform;
    private float spriteWidth;

    void Start()
    {
        cameraTransform = Camera.main.transform;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void LateUpdate()
    {
        float distance = cameraTransform.position.x - transform.position.x;
        if (Mathf.Abs(distance) >= spriteWidth)
        {
            float offset = distance > 0 ? spriteWidth : -spriteWidth;
            transform.position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
        }
    }
}