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
    
    private async void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerAbility playerAbility = collision.GetComponent<PlayerAbility>();
        if (playerAbility != null && playerAbility.Ability == null)
        {
            Canvas uiCanvas = playerAbility.transform.Find("PlayerModel/UICanvas")?.GetComponent<Canvas>();
            if(uiCanvas != null)
            {
                TMP_Text abilityText = uiCanvas.transform.Find("AbilityNameText").GetComponent<TMP_Text>();
                if(abilityText != null)
                {

                    int randomAbility = Random.Range(0, abilityCount);

                    switch (randomAbility)
                    {
                        case 0:
                            playerAbility.Ability = "BlindEnemy";
                            abilityText.text = "Blind";
                            break;
                        case 1:
                            playerAbility.Ability = "SlowEnemy";
                            abilityText.text = "Slow";
                            break;
                        case 2:
                            playerAbility.Ability = "ReverseControlEnemy";
                            abilityText.text = "Reverse Control";
                            break;
                        case 3:
                            playerAbility.Ability = "ShieldPlayer";
                            abilityText.text = "Shield";
                            break;
                        case 4:
                            playerAbility.Ability = "Rocket";
                            abilityText.text = "Rocket";
                            break;
                        case 5:
                            playerAbility.Ability = "MuchSpeed";
                            abilityText.text = "Speed";
                            break;
                        case 6:
                            playerAbility.Ability = "Invinsible";
                            abilityText.text = "Invinsible";
                            break;
                    }
                    RespawnAbility();
                }
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
