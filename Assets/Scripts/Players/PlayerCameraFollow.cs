using UnityEngine;
using Photon.Pun;

public class PlayerCameraFollow : MonoBehaviour
{
    PhotonView photonView;

    [Header("Settings")]
    public Vector3 offset;  
    private Transform target;  

    void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = offset;
            Camera.main.transform.localRotation = Quaternion.identity;
            Camera.main.orthographicSize = 4;

            target = transform;
        }
    }

    void Update()
    {
        if (!photonView.IsMine && target != null)
        {
            Vector3 desiredPosition = target.position;
            desiredPosition.z = Camera.main.transform.position.z;  
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, desiredPosition, Time.deltaTime * 5f);
        }
    }

    public void SetCameraTarget(Transform newTarget)
    {
        target = newTarget;
    }
}


