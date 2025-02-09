using Photon.Pun;
using UnityEngine;

public class BasePlatformSpawner : MonoBehaviourPun
{
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject platform = PhotonNetwork.Instantiate("BasePlatform", Vector3.zero, Quaternion.identity);
        }
    }
}
