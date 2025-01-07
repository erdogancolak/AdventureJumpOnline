using UnityEngine;
using Photon.Pun;

public class PlayerCameraFollow : MonoBehaviour
{
    public Vector3 offset; // Kameranýn oyuncuya olan mesafesi
    private Transform playerTransform; // Takip edilecek oyuncunun transformu
    private PhotonView photonView; // PhotonView referansý

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        // Sadece yerel oyuncu için kamera kontrolünü aktif et
        if (photonView.IsMine)
        {
            // Kamerayý oyuncu nesnesine baðla
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = offset; // Offset ile konumu ayarla
            Camera.main.transform.localRotation = Quaternion.identity; // Kamerayý düzle
        }
    }
}

