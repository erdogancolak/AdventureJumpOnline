using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerAbility : MonoBehaviour
{
    PhotonView photonView;
    [Header("Settings")]
    /*[HideInInspector]*/ public string Ability;
    private TMP_Text abilityNameText;
    private bool isRocketFinish;

    [Space]

    [Header("EffectSettings")]
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
        Ability = null;
        playerModelSprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        abilityNameText = transform.Find("PlayerModel/UICanvas/AbilityNameText")?.GetComponent<TMP_Text>();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && Ability != null)
        {
            useAbility();
        }
    }

    public void useAbility()
    {
        Debug.Log(Ability + " Used");
        switch (Ability)
        {
            //case "BlindEnemy":
            //    photonView.RPC("BlindEnemyAbility", RpcTarget.Others);
            //    break;
            case "Slow":
                photonView.RPC("SlowEnemyAbility", RpcTarget.Others);
                break;
            case "Reverse":
                photonView.RPC("ReverseControlAbility", RpcTarget.Others);
                break;
            //case "ShieldPlayer":
            //    photonView.RPC("ShieldPlayerAbility", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
            //    break;
            //case "Rocket":
            //    photonView.RPC("RocketAbility", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
            //    break;
            case "Speed":
                photonView.RPC("MuchSpeedAbility", RpcTarget.Others);
                break;
            //case "Invinsible":
            //    photonView.RPC("InvinsibleAbility", RpcTarget.Others);
            //    break;
            default:
                return;
        }
        Ability = null;
        //ResetAbiliyUI();
    }
    public void ResetAbiliyUI()
    {
        Ability = null;
        if (abilityNameText != null)
        {
            abilityNameText.text = null;
        }
    }
    #region Blind
    [PunRPC]
    private void BlindAbility()
    {
        Debug.Log("Blind");
        StartCoroutine(ApplyBlindEffect());
    }

    IEnumerator ApplyBlindEffect()
    {
        Ability = null;
        GameObject blindPanel1 = new GameObject("BlindPanel1");
        Canvas canvas = transform.Find("PlayerModel/BlindEffectCanvas").GetComponent<Canvas>();
        if (canvas != null)
        {
            blindPanel1.transform.SetParent(canvas.transform, false);
            RectTransform rt1 = blindPanel1.AddComponent<RectTransform>();
            rt1.sizeDelta = new Vector2(Screen.width, Screen.height);
        }

        CanvasRenderer cr1 = blindPanel1.AddComponent<CanvasRenderer>();
        Image img1 = blindPanel1.AddComponent<Image>();
        img1.color = new Color(0, 0, 0, 0.94f); 

        yield return new WaitForSeconds(blindEffectTime);

        Destroy(blindPanel1);
    }
    #endregion
    #region Slow
    [PunRPC]
    public void SlowEnemyAbility()
    {
        Debug.Log("Slowun RPC ye girdi");

        PlayerMovement[] playerMovements = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        foreach(PlayerMovement playerMovement in playerMovements)
        {
            if(!playerMovement.GetComponent<PhotonView>().IsMine)
            {
                Debug.Log("Düþman Test");
                StartCoroutine(ApplySlowEffect(playerMovement));
            }
        }
    }
    IEnumerator ApplySlowEffect(PlayerMovement playerMovement)
    {
        Ability = null;
        Debug.Log("Slowun ÝKÝNCÝSÝNE GÝRDÝ");

        playerMovement.speed = PlayerMovement.Speeds.slow;

        Debug.Log(playerMovement.speed);
        //Debug.Log("Playerýn Slowlanmýþ Speedi" + playerMovement.moveSpeed.ToString());

        yield return new WaitForSeconds(slowEffectTime);

        playerMovement.speed = PlayerMovement.Speeds.regular;
        Debug.Log(playerMovement.speed);

        //Debug.Log("Playerýn Normale Dönmüþ Speedi" + playerMovement.moveSpeed.ToString());
        //PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        //if(playerMovement != null)
        //{
        //    Debug.Log("PlayerMovement BULUNDU"); 
        //    playerMovement.speed = PlayerMovement.Speeds.slow;

        //    yield return new WaitForSeconds(slowEffectTime);

        //    playerMovement.speed = PlayerMovement.Speeds.regular;
        //}
    }
    #endregion
    #region ReverseControl
    [PunRPC]
    public void ReverseControlAbility()
    {

        Debug.Log("Reverse");
        StartCoroutine(ApplyReverseControlEffect());
    }
    IEnumerator ApplyReverseControlEffect()
    {
        Ability = null;
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if(playerMovement != null)
        {
            playerMovement.speed = PlayerMovement.Speeds.reverse;
            
            yield return new WaitForSeconds(reverseControlEffectTime);

            playerMovement.speed = PlayerMovement.Speeds.regular;
        }
    }
    #endregion
    #region ShieldPlayer
    [PunRPC]
    private void ShieldPlayerAbility(int actorNumber)
    {
        if (photonView.Owner.ActorNumber != actorNumber) return;
        
        StartCoroutine(ApplyShieldPlayerEffect());
    }
    IEnumerator ApplyShieldPlayerEffect()
    {
        isShieldActive = true;

        yield return new WaitForSeconds(shieldEffectTime);

        isShieldActive = false;
    }
    #endregion
    #region Rocket
    [PunRPC]
    private void RocketAbility(int actorNumber)
    {
        if (photonView.Owner.ActorNumber != actorNumber) return;

        StartCoroutine(ApplyRocketAbilityEffect());
    }
    IEnumerator ApplyRocketAbilityEffect()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if(rb != null && !isRocketFinish)
        {
            isRocketFinish =true;
            float originalGravity = rb.gravityScale;
            Vector2 originalVelocity = rb.linearVelocity;

            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,RocketPower);

            yield return new WaitForSeconds(RocketEffectTime);

            isRocketFinish = false;
            rb.gravityScale = originalGravity;
            rb.linearVelocity = originalVelocity;
        }
    }
    #endregion
    #region Speed
    [PunRPC]
    public void MuchSpeedAbility()
    {
 
        Debug.Log("Speed");
        StartCoroutine(ApplySpeedAbility());
    }
    IEnumerator ApplySpeedAbility()
    {
        Ability = null;
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if(playerMovement != null)
        {
            playerMovement.speed = PlayerMovement.Speeds.fast;

            yield return new WaitForSeconds(MuchSpeedEffectTime);

            playerMovement.speed = PlayerMovement.Speeds.regular;
        }
    }
    #endregion
    #region Invinsible
    [PunRPC]
    private void InvinsibleAbility()
    {
        if (photonView.IsMine) return;

        Debug.Log("Invinsible");
        StartCoroutine(ApplyInvinsibleEffect());
    }
    IEnumerator ApplyInvinsibleEffect()
    {
        Ability = null;
        GameObject playerModel = transform.GetChild(0).gameObject;
        if(playerModel != null)
        {
            Debug.Log("Invisible Girdi");
            Debug.Log($"Applying InvinsibleEffect to {photonView.Owner.NickName}");
            playerModel.GetComponent<SpriteRenderer>().sprite = null;

            yield return new WaitForSeconds(InvinsibleEffectTime);

            playerModel.GetComponent<SpriteRenderer>().sprite = playerModelSprite;
        }
    }
    #endregion
}
