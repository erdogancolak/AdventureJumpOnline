using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

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
        Ability = "Invinsible";
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
            case "Blind":
                photonView.RPC("ApplyBlindEffect", RpcTarget.All);
                break;
            case "Slow":
                photonView.RPC("ApplySlowEffect", RpcTarget.Others);
                break;
            case "Reverse Control":
                photonView.RPC("ApplyReverseControlEffect", RpcTarget.Others);
                break;
            case "ShieldPlayer":
                photonView.RPC("ShieldPlayerAbility", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
                break;
            case "Rocket":
                photonView.RPC("RocketAbility", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
                break;
            case "Speed":
                photonView.RPC("ApplySpeedAbility", RpcTarget.Others);
                break;
            case "Invinsible":
                photonView.RPC("ApplyInvinsibleEffect", RpcTarget.Others);
                break;
            default:
                return;
        }
        Ability = null;
        ResetAbiliyUI();
    }
    public void ResetAbiliyUI()
    {
        Ability = null;
        if (abilityNameText != null)
        {
            abilityNameText.text = "Empty";
        }
    }
#region Blind
    [PunRPC]
    public IEnumerator ApplyBlindEffect()
    {
        Ability = null;
        if (!photonView.IsMine)
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
    }
    #endregion
#region Slow
    [PunRPC]
    public IEnumerator ApplySlowEffect()
    {
        Ability = null;

        PlayerMovement[] playerMovements = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        foreach (PlayerMovement playerMovement in playerMovements)
        {

            if (playerMovement.GetComponent<PhotonView>().IsMine)
            {
                if (playerMovement != null)
                {
                    playerMovement.moveSpeed *= 0.5f;

                    yield return new WaitForSeconds(slowEffectTime);

                    playerMovement.moveSpeed = 350;
                }
            }
        }
    }
#endregion
#region ReverseControl
    [PunRPC]
    public IEnumerator ApplyReverseControlEffect()
    {
        Ability = null;

        PlayerMovement[] playerMovements = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        foreach (PlayerMovement playerMovement in playerMovements)
        {

            if (playerMovement.GetComponent<PhotonView>().IsMine)
            {
                if (playerMovement != null)
                {
                    playerMovement.isReversed = true;

                    yield return new WaitForSeconds(slowEffectTime);

                    playerMovement.isReversed = false;
                }
            }
        }
    }
    #endregion
#region ShieldPlayer
    [PunRPC]
    public void ShieldPlayerAbility(int actorNumber)
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
    public IEnumerator ApplySpeedAbility()
    {
        Ability = null;

        PlayerMovement[] playerMovements = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        foreach (PlayerMovement playerMovement in playerMovements)
        {
            if (playerMovement.GetComponent<PhotonView>().IsMine)
            {
                if (playerMovement != null)
                {
                    playerMovement.moveSpeed *= 1.4f;
                    yield return new WaitForSeconds(MuchSpeedEffectTime);

                    playerMovement.moveSpeed = 350;
                }
            }
        }
    }
    #endregion
#region Invinsible
    [PunRPC]
    IEnumerator ApplyInvinsibleEffect()
    {
        Ability = null;

        PlayerMovement[] playerMovements = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        foreach(PlayerMovement playerMovement in playerMovements)
        {
            if(playerMovement.GetComponent<PhotonView>().IsMine)
            {
                if (playerMovement != null)
                {
                    GameObject playerModel = transform.GetChild(0).gameObject;
                    if(playerModel != null)
                    {
                        playerModel.GetComponent<SpriteRenderer>().sprite = null;

                        yield return new WaitForSeconds(InvinsibleEffectTime);

                        playerModel.GetComponent<SpriteRenderer>().sprite = playerModelSprite;
                    }
                }
            }
        }
    }
}
#endregion
