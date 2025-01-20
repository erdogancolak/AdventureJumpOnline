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
                TMP_Text abilityText = playerAbility.transform.Find("PlayerModel/UICanvas/AbilityNameText").GetComponent<TMP_Text>();
                if(abilityText != null)
                {

                    int randomAbility = Random.Range(0, 3);

                    switch (randomAbility)
                    {
                        case 0:
                            playerAbility.Ability = "Slow";
                        Debug.Log(playerAbility.Ability);
                            break;
                        case 1:
                            playerAbility.Ability = "Speed";
                        Debug.Log(playerAbility.Ability);
                        break;
                        case 2:
                            playerAbility.Ability = "Reverse";
                        Debug.Log(playerAbility.Ability);
                        break;
                        //case 3:
                        //    playerAbility.Ability = "ShieldPlayer";
                        //    abilityText.text = "Shield";
                        //    break;
                        //case 4:
                        //    playerAbility.Ability = "Rocket";
                        //    abilityText.text = "Rocket";
                        //    break;
                        //case 3:
                        //   blind
                        //    break;
                        //case 4:
                        //    photonView.RPC("InvinsibleAbility", RpcTarget.Others);
                        //    break;
                    }
                    RespawnAbility();
                }
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
