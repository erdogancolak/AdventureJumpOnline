using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject player;
    public float minX;
    public float maxX;

    private void Start()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(minX, maxX), 0.7f);
        PhotonNetwork.Instantiate(player.name, spawnPosition, Quaternion.identity);
    }
}

