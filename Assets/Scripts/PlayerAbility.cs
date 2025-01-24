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
    private TMP_Text abilityNameText;
    [HideInInspector] public enum abilities { Empty, Blind, Slow, Reverse, Speed, Invinsible, Rocket }
    public abilities currentAbility;

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
        currentAbility = abilities.Empty;

        abilityNameText = transform.Find("PlayerModel/UICanvas/AbilityNameText")?.GetComponent<TMP_Text>();
        SetTextAbility(currentAbility.ToString());

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && currentAbility != abilities.Empty)
        {
            useAbility();
        }
    }

    public void useAbility()
    {
        Debug.Log(currentAbility + " Used");
        switch (currentAbility)
        {
            case abilities.Blind:
                Debug.Log("Blind");
                photonView.RPC("BlindEffect", RpcTarget.Others);
                break;
            case abilities.Slow:
                photonView.RPC("SlowEffect", RpcTarget.Others);
                break;
            case abilities.Reverse:
                photonView.RPC("ReverseControlEffect", RpcTarget.Others);
                break;
            case abilities.Rocket:
                photonView.RPC("RocketEffect", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
                break;
            case abilities.Speed:
                photonView.RPC("SpeedEffect", RpcTarget.Others);
                break;
            case abilities.Invinsible:
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
        Debug.Log("Reset");
        currentAbility = abilities.Empty;
        if (abilityNameText != null)
        {
            abilityNameText.text = "";
            abilityNameText.text = currentAbility.ToString();
        }
    }
#region Blind
    [PunRPC]
    public void BlindEffect()
    {
        PlayerMovement[] playerMovements = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        foreach (PlayerMovement playerMovement in playerMovements)
        {
            if (playerMovement != null && playerMovement.GetComponent<PhotonView>().IsMine)
            {
                StartCoroutine(ApplyBlindEffect());
            }
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
        PhotonView[] photonViews = Object.FindObjectsByType<PhotonView>(FindObjectsSortMode.None);

        foreach (PhotonView photonView in photonViews)
        {
            if (photonView != null && photonView.IsMine)
            {
                StartCoroutine(ApplyInvinsibleEffect(photonView));
            }
        }
    }
    IEnumerator ApplyInvinsibleEffect(PhotonView photonView)
    {
        GameObject playerModel = photonView.gameObject;
        if (playerModel != null)
        {
            SpriteRenderer playerSprite = playerModel.transform.GetChild(0).GetComponent<SpriteRenderer>();
            playerSprite.enabled = false;

            yield return new WaitForSeconds(InvinsibleEffectTime);

            playerSprite.enabled = true;
        }
    }
}
#endregion
