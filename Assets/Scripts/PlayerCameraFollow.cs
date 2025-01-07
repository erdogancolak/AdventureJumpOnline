using UnityEngine;
using Photon.Pun;

public class PlayerCameraFollow : MonoBehaviour
{
    public Vector3 offset; // Kameran�n oyuncuya olan mesafesi
    private Transform playerTransform; // Takip edilecek oyuncunun transformu
    private PhotonView photonView; // PhotonView referans�

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        // Sadece yerel oyuncu i�in kamera kontrol�n� aktif et
        if (photonView.IsMine)
        {
            // Kameray� oyuncu nesnesine ba�la
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = offset; // Offset ile konumu ayarla
            Camera.main.transform.localRotation = Quaternion.identity; // Kameray� d�zle
        }
    }
}

