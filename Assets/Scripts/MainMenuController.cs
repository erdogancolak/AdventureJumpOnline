using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MainMenuController: MonoBehaviourPunCallbacks
{
    public int maxPlayer;
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public TMP_InputField nicknameInput;
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
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
