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

        if (playerAbility != null && playerAbility.Ability == null && photonView != null && photonView.IsMine)
        {
            int randomAbility = Random.Range(0, abilityCount);

            switch (randomAbility)
            {
                case 0:
                    playerAbility.Ability = "Slow";
                    break;
                case 1:
                    playerAbility.Ability = "Speed";
                    break;
                case 2:
                    playerAbility.Ability = "Reverse Control";
                    break;
                case 3:
                    playerAbility.Ability = "Rocket";
                    break;
                case 4:
                    playerAbility.Ability = "Blind";
                    break;
                case 5:
                    playerAbility.Ability = "Invinsible";
                    break;
            }
            playerAbility.SetTextAbility(playerAbility.Ability);

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
