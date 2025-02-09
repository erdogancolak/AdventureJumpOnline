using Photon.Pun;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectAbility : MonoBehaviour
{
    [Header("Settings")]
    public int abilityCount;

    public float respawnTime;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhotonView photonView = collision.GetComponent<PhotonView>();
        PlayerAbility playerAbility = collision.GetComponent<PlayerAbility>();

        if (playerAbility != null && playerAbility.currentAbility == PlayerAbility.abilities.Empty && photonView != null && photonView.IsMine)
        {
            int randomAbility = Random.Range(0, abilityCount);

            switch (randomAbility)
            {
                case 0:
                    playerAbility.currentAbility = PlayerAbility.abilities.Blind;
                    break;
                case 1:
                    playerAbility.currentAbility = PlayerAbility.abilities.Slow;
                    break;
                case 2:
                    playerAbility.currentAbility = PlayerAbility.abilities.Reverse;
                    break;
                case 3:
                    playerAbility.currentAbility = PlayerAbility.abilities.Speed;
                    break;
                case 4:
                    playerAbility.currentAbility = PlayerAbility.abilities.Invinsible;
                    break;
            }
            playerAbility.SetTextAbility(playerAbility.currentAbility.ToString() , true);

            RespawnAbility();
        }
    }

    private async void RespawnAbility()
    {
        gameObject.SetActive(false);

        await Task.Delay((int)(respawnTime * 1000));

        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.SetActive(true);
        }
    }
    
}
