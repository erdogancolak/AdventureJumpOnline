using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    public Vector3 offset;

    private void LateUpdate()
    {
        if(target == null) return;

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = -10;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}
