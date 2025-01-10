using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerAbility : MonoBehaviour
{
    PhotonView photonView;
    [HideInInspector] public string Ability;
    private TMP_Text abilityNameText;

    [Header("Effects")]
    public float blindEffectTime;
    [Space]
    public float slowEffectTime;
    [Space]
    public float reverseControlEffectTime;
    [Space]
    public float shieldEffectTime;
    public bool isShieldActive;
    [Space]
    public float RocketEffectTime;
    public float RocketPower;
    [Space]
    public float MuchSpeedEffectTime;
    [Space]
    public float InvinsibleEffectTime;
    Sprite playerModelSprite;
    
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    void Start()
    {
        Ability = "BlindEnemy";
        playerModelSprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        abilityNameText = transform.Find("PlayerModel/UICanvas/AbilityNameText")?.GetComponent<TMP_Text>();
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            useAbility();
        }
    }

    public void useAbility()
    {
        switch (Ability)
        {
            case "BlindEnemy":
                photonView.RPC("BlindEnemyAbility", RpcTarget.All);
                break;
            case "SlowEnemy":
                photonView.RPC("SlowEnemyAbility", RpcTarget.All); 
                break;
            case "ReverseControlEnemy":
                photonView.RPC("ReverseControlAbility", RpcTarget.All);
                break;
            case "ShieldPlayer":
                photonView.RPC("ShieldPlayerAbility", RpcTarget.All);
                break;
            case "Rocket":
                photonView.RPC("RocketAbility", RpcTarget.All);
                break;
            case "MuchSpeed":
                photonView.RPC("MuchSpeedAbility", RpcTarget.All);
                break;
            case "Invinsible":
                photonView.RPC("InvinsibleAbility", RpcTarget.All);
                break;
            default:
                return;
                break;
        }
    }
    #region BlindEnemy
    [PunRPC]
    private void BlindEnemyAbility()
    {
        //if (!photonView.IsMine && !isShieldActive)
        //{
            StartCoroutine(ApplyBlindEnemyEffect());
        //}
    }

    IEnumerator ApplyBlindEnemyEffect()
    {
        Ability = null;
       
        GameObject blindPanel1 = new GameObject("BlindPanel1");
        GameObject blindPanel2 = new GameObject("BlindPanel2");
        Canvas canvas = transform.Find("BlindEffectCanvas").GetComponent<Canvas>();
        if (canvas != null)
        {
            blindPanel1.transform.SetParent(canvas.transform, false);
            blindPanel2.transform.SetParent(canvas.transform, false);
            RectTransform rt1 = blindPanel1.AddComponent<RectTransform>();
            RectTransform rt2 = blindPanel2.AddComponent<RectTransform>();
            rt1.sizeDelta = new Vector2(Screen.width, Screen.height);
            rt2.sizeDelta = new Vector2(Screen.width, Screen.height);
        }

        CanvasRenderer cr1 = blindPanel1.AddComponent<CanvasRenderer>();
        CanvasRenderer cr2 = blindPanel2.AddComponent<CanvasRenderer>();
        Image img1 = blindPanel1.AddComponent<Image>();
        Image img2 = blindPanel2.AddComponent<Image>();
        img1.color = new Color(0, 0, 0, 0.94f); 
        img2.color = new Color(0, 0, 0, 0.94f); 

        yield return new WaitForSeconds(blindEffectTime);

        Destroy(blindPanel1);
        Destroy(blindPanel2);
    }
    #endregion
    #region SlowEnemy
    [PunRPC]
    private void SlowEnemyAbility()
    {
        if (!photonView.IsMine && !isShieldActive)
        {
            StartCoroutine(ApplySlowEnemyEffect());
        }
    }
    IEnumerator ApplySlowEnemyEffect()
    {
        Ability = null;
        if (abilityNameText != null)
        {
            abilityNameText.text = Ability;

        }
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if(movement != null)
        {
            float originalSpeed = movement.moveSpeed;
            movement.moveSpeed *= 0.5f;

            yield return new WaitForSeconds(slowEffectTime);

            movement.moveSpeed = originalSpeed;
        }
    }
    #endregion
    #region ReverseControl
    [PunRPC]
    private void ReverseControlAbility()
    {
        if (!photonView.IsMine && !isShieldActive)
        {
            StartCoroutine(ApplyReverseControlEnemyEffect());
        }
    }
    IEnumerator ApplyReverseControlEnemyEffect()
    {
        Ability = null;
        if (abilityNameText != null)
        {
            abilityNameText.text = Ability;

        }
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if(movement != null)
        {
            float originalSpeed = movement.moveSpeed;
            movement.moveSpeed *= -1;
            
            yield return new WaitForSeconds(reverseControlEffectTime);

            movement.moveSpeed = originalSpeed;
        }
    }
    #endregion
    #region ShieldPlayer
    [PunRPC]
    private void ShieldPlayerAbility()
    {   
        if(photonView.IsMine)
        {
            StartCoroutine(ApplyShieldPlayerEffect());
        }
    }
    IEnumerator ApplyShieldPlayerEffect()
    {
        Ability = null;
        if (abilityNameText != null)
        {
            abilityNameText.text = Ability;

        }
        isShieldActive = true;

        yield return new WaitForSeconds(shieldEffectTime);

        isShieldActive = false;
    }
    #endregion
    #region Rocket
    [PunRPC]
    private void RocketAbility()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(ApplyRocketAbilityEffect());
        }
    }
    IEnumerator ApplyRocketAbilityEffect()
    {
        Ability = null;
        if (abilityNameText != null)
        {
            abilityNameText.text = Ability;

        }
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            float originalGravity = rb.gravityScale;
            Vector2 originalVelocity = rb.linearVelocity;

            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,RocketPower);

            yield return new WaitForSeconds(RocketEffectTime);

            rb.gravityScale = originalGravity;
            rb.linearVelocity = originalVelocity;
        }
    }
    #endregion
    #region MuchSpeed
    [PunRPC]
    private void MuchSpeedAbility()
    {
        if (!photonView.IsMine)
        {
            StartCoroutine(ApplyMuchSpeedAbility());
        }
    }
    IEnumerator ApplyMuchSpeedAbility()
    {
        Ability = null;
        if (abilityNameText != null)
        {
            abilityNameText.text = Ability;

        }
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if(playerMovement != null)
        {
            float originalSpeed = playerMovement.moveSpeed;
            playerMovement.moveSpeed *= 3f;

            yield return new WaitForSeconds(MuchSpeedEffectTime);
            
            playerMovement.moveSpeed = originalSpeed;
        }
    }
    #endregion
    #region Invinsible
    [PunRPC]
    private void InvinsibleAbility()
    {
        if (!photonView.IsMine)
        {
            StartCoroutine(ApplyInvinsibleAbilityEffect());
        }
    }
    IEnumerator ApplyInvinsibleAbilityEffect()
    {
        Ability = null;
        if (abilityNameText != null)
        {
            abilityNameText.text = Ability;

        }
        GameObject playerModel = transform.GetChild(0).gameObject;
        playerModel.GetComponent<SpriteRenderer>().sprite = null;

        yield return new WaitForSeconds(InvinsibleEffectTime);

        playerModel.GetComponent<SpriteRenderer>().sprite = playerModelSprite;
    }
    #endregion
}
