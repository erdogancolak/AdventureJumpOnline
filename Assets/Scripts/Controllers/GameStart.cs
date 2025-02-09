using System;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections;

public class GameStart : MonoBehaviour
{
    PhotonView photonView;

    public TMP_Text gameStartText;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();    
    }
    void Start()
    {
        Time.timeScale = 0;
    }

    void Update()
    {
        if(PhotonNetwork.IsMasterClient && Input.GetKeyDown(KeyCode.X))
        {
            photonView.RPC("startGame", RpcTarget.All);   
        }
    }

    [PunRPC]
    public void startGame()
    {
        StartCoroutine(startGameIE());
    }

    IEnumerator startGameIE()
    {
        photonView.RPC("UpdateGameStartText", RpcTarget.AllBuffered, "3...");
        yield return new WaitForSecondsRealtime(1);
        photonView.RPC("UpdateGameStartText", RpcTarget.AllBuffered, "2...");
        yield return new WaitForSecondsRealtime(1);
        photonView.RPC("UpdateGameStartText", RpcTarget.AllBuffered, "1...");
        yield return new WaitForSecondsRealtime(1);
        photonView.RPC("UpdateGameStartText", RpcTarget.AllBuffered, "GAME START!!");
        yield return new WaitForSecondsRealtime(.4f);

        photonView.RPC("DestroyGameStartText", RpcTarget.AllBuffered);

        photonView.RPC("setTimeScale", RpcTarget.AllBuffered, 1);
    }
    [PunRPC]
    void UpdateGameStartText(string newText)
    {
        if (gameStartText != null)
        {
            gameStartText.text = newText;
        }
    }

    [PunRPC]
    void DestroyGameStartText()
    {
        if (gameStartText != null)
        {
            Destroy(gameStartText.gameObject);
        }
    }

    [PunRPC]
    void setTimeScale(int value)
    {
        Time.timeScale = value;
    }
}
