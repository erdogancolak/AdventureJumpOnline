using UnityEngine;
using Photon.Pun;

public class PlayerCameraFollow : MonoBehaviour
{
    PhotonView photonView;

    [Header("Settings")]
    public Vector3 offset; 
    

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        
        if (photonView.IsMine)
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = offset; 
            Camera.main.transform.localRotation = Quaternion.identity;
            Camera.main.orthographicSize = 4;
        }
    }
}

