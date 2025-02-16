using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MainMenuController: MonoBehaviourPunCallbacks
{
    [Header("References")]

    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public TMP_InputField nicknameInput;

    [Space]
    [Header("Settings")]

    public int maxPlayer;

    public void SetPlayerName()
    {
        PhotonNetwork.NickName = nicknameInput.text;
    }
    public void CreateRoom()
    {
        if(!string.IsNullOrEmpty(nicknameInput.text))
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayer;
            PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        }
        else
        {
            Debug.Log("Player Name is Empty");
        }
        
    }
    public void JoinRoom()
    {
        if(!string.IsNullOrEmpty(nicknameInput.text))
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
        else
        {
            Debug.Log("Player Name is Empty");
        }
    }

    public void JoinRoomInList(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
