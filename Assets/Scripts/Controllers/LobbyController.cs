using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyController : MonoBehaviourPunCallbacks
{
    public Transform playerListContainer;
    public GameObject playerNamePrefab;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        UpdatePlayerList();
    }

    void UpdatePlayerList()
    {
        foreach (Transform child in playerListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject newPlayerName = Instantiate(playerNamePrefab, playerListContainer);
            newPlayerName.GetComponent<TMP_Text>().text = player.NickName;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && Input.GetKeyDown(KeyCode.X))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if(PhotonNetwork.PlayerList.Length >= 1)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;

                PhotonNetwork.LoadLevel("Game");
            }
        }
       
    }
}

