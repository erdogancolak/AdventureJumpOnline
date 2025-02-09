using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerAbility : MonoBehaviour
{
    PhotonView photonView;
    [Header("Settings")]
    private TMP_Text abilityNameText;
    private TMP_Text useAbilityText;
    private TMP_Text nicknameText;
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
        useAbilityText = transform.Find("PlayerModel/UICanvas/useAbilityText")?.GetComponent<TMP_Text>();
        nicknameText = transform.Find("PlayerModel/UICanvas/PlayerNickname")?.GetComponent<TMP_Text>();
        SetTextAbility(currentAbility.ToString() , false);
        SetNickname(PhotonNetwork.NickName);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentAbility != abilities.Empty)
        {
            useAbility();
        }
    }

    public void useAbility()
    {
        switch (currentAbility)
        {
            case abilities.Blind:
                photonView.RPC("BlindEffect", RpcTarget.Others);
                break;
            case abilities.Slow:
                photonView.RPC("SlowEffect", RpcTarget.Others);
                break;
            case abilities.Reverse:
                photonView.RPC("ReverseControlEffect", RpcTarget.Others);
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
    public void SetTextAbility(string Ability , bool useAbilityTextBool)
    {
        abilityNameText.text = Ability;
        useAbilityText.enabled = useAbilityTextBool;
    }

    public void ResetAbiliyUI()
    {
        currentAbility = abilities.Empty;
        if (abilityNameText != null)
        {
            abilityNameText.text = "";
            abilityNameText.text = currentAbility.ToString();
            useAbilityText.enabled = false;
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
                //StartCoroutine(ApplySlowEffect(playerMovement));
                StartCoroutine(ChangeSpeed(playerMovement, 0.5f, slowEffectTime));
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
                //StartCoroutine(ApplyReverseControlEffect(playerMovement));
                StartCoroutine(ChangeSpeed(playerMovement, -1f, reverseControlEffectTime));
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
#region Speed
    [PunRPC]
    public void SpeedEffect()
    {
        PlayerMovement[] playerMovements = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        foreach (PlayerMovement playerMovement in playerMovements)
        {
            if (playerMovement != null && playerMovement.GetComponent<PhotonView>().IsMine)
            {
                //StartCoroutine(ApplySpeedAbility(playerMovement));
                StartCoroutine(ChangeSpeed(playerMovement, 1.4f, MuchSpeedEffectTime));
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
    #endregion
    IEnumerator ChangeSpeed(PlayerMovement playerMovement,float changeSpeedFloat,float effectTime)
    {
        playerMovement.moveSpeed *= changeSpeedFloat;

        yield return new WaitForSeconds(effectTime);

        playerMovement.moveSpeed = 350;
    }

    private void SetNickname(string playerNickname)
    {
        if(playerNickname != null)
        {
            nicknameText.text = playerNickname;
        }
    }
}


