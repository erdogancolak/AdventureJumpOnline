using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;

public class PlayerAbility : MonoBehaviour
{
    PhotonView photonView;
    [Header("Settings")]
    [HideInInspector] public string Ability;
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
        switch (Ability)
        {
            case "Blind":
                photonView.RPC("BlindEffect", RpcTarget.Others);
                break;
            case "Slow":
                photonView.RPC("SlowEffect", RpcTarget.Others);
                break;
            case "Reverse Control":
                photonView.RPC("ReverseControlEffect", RpcTarget.Others);
                break;
            case "Rocket":
                photonView.RPC("RocketEffect", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
                break;
            case "Speed":
                photonView.RPC("SpeedEffect", RpcTarget.Others);
                break;
            case "Invinsible":
                photonView.RPC("InvinsibleEffect", RpcTarget.Others);
                break;
            default:
                return;
        }
        ResetAbiliyUI();
    }
    public void SetTextAbility(string Ability)
    {
        abilityNameText.text = Ability;
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
    public void BlindEffect()
    {
        if(photonView.IsMine)
        {
            StartCoroutine(ApplyBlindEffect());
        }
    }
    IEnumerator ApplyBlindEffect()
    {
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
    public void SlowEffect()
    {
        PlayerMovement[] playerMovements = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        foreach(PlayerMovement playerMovement in playerMovements)
        {
            if(playerMovement != null && playerMovement.GetComponent<PhotonView>().IsMine)
            {
                StartCoroutine(ApplySlowEffect(playerMovement));
            }
        }
    }
    IEnumerator ApplySlowEffect(PlayerMovement playerMovement)
    {
        playerMovement.moveSpeed *= 0.5f;

        yield return new WaitForSeconds(slowEffectTime);

        playerMovement.moveSpeed = 350;
    }
#endregion
#region ReverseControl
    [PunRPC]
    public void ReverseControlEffect()
    {
        PlayerMovement[] playerMovements = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
        foreach (PlayerMovement playerMovement in playerMovements)
        {
            if (playerMovement != null && playerMovement.GetComponent<PhotonView>().IsMine)
            {
                StartCoroutine(ApplyReverseControlEffect(playerMovement));
            }
        }
    }
    IEnumerator ApplyReverseControlEffect(PlayerMovement playerMovement)
    {
        playerMovement.moveSpeed *= -1;

        yield return new WaitForSeconds(MuchSpeedEffectTime);

        playerMovement.moveSpeed *= -1;
    }
    #endregion
#region Rocket
    [PunRPC]
    public void RocketEffect(int actorNumber)
    {
        if (photonView.Owner.ActorNumber != actorNumber) return;

        StartCoroutine(ApplyRocketAbilityEffect());
    }
    IEnumerator ApplyRocketAbilityEffect()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null && !isRocketFinish)
        {
            isRocketFinish = true;
            float originalGravity = rb.gravityScale;
            Vector2 originalVelocity = rb.linearVelocity;

            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, RocketPower);

            yield return new WaitForSeconds(RocketEffectTime);

            isRocketFinish = false;
            rb.gravityScale = originalGravity;
            rb.linearVelocity = originalVelocity;
        }
    }
    #endregion
#region Speed
    [PunRPC]
    public void SpeedEffect()
    {
        PlayerMovement[] playerMovements = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        foreach (PlayerMovement playerMovement in playerMovements)
        {
            if (playerMovement != null && playerMovement.GetComponent<PhotonView>().IsMine)
            {
                StartCoroutine(ApplySpeedAbility(playerMovement));
            }
        }
    }
    public IEnumerator ApplySpeedAbility(PlayerMovement playerMovement)
    {
        playerMovement.moveSpeed *= 1.4f;
        yield return new WaitForSeconds(MuchSpeedEffectTime);

        playerMovement.moveSpeed = 350;
    }
    #endregion
#region Invinsible
    [PunRPC]
    public void InvinsibleEffect()
    {
        PlayerMovement[] playerMovements = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        foreach (PlayerMovement playerMovement in playerMovements)
        {
            if (playerMovement != null && playerMovement.GetComponent<PhotonView>().IsMine)
            {
                StartCoroutine(ApplyInvinsibleEffect());
            }
        }
    }
    IEnumerator ApplyInvinsibleEffect()
    {
        GameObject playerModel = transform.GetChild(0).gameObject;
        if (playerModel != null)
        {
            playerModel.GetComponent<SpriteRenderer>().sprite = null;

            yield return new WaitForSeconds(InvinsibleEffectTime);

            playerModel.GetComponent<SpriteRenderer>().sprite = playerModelSprite;
        }
    }
}
#endregion
