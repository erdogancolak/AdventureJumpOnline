using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerAbility : MonoBehaviour
{
    PhotonView photonView;
    [HideInInspector] public string Ability;

    [Header("Effects")]
    public float blindEffectTime;
    [Space]
    public float slowEffectTime;
    [Space]
    public float reverseControlEffectTime;
    
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    void Start()
    {
        Ability = "BlindEnemy";
        
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
            default:
                Debug.Log("Ability = " + Ability);
                break;
        }
    }
    #region BlindEnemy
    [PunRPC]
    private void BlindEnemyAbility()
    {
        if(!photonView.IsMine)
        {
            StartCoroutine(ApplyBlindEffect());
        }
    }

    IEnumerator ApplyBlindEffect()
    {
        Ability = null;
        GameObject blindPanel1 = new GameObject("BlindPanel1");
        GameObject blindPanel2 = new GameObject("BlindPanel2");
        Canvas canvas = FindObjectOfType<Canvas>(); 
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
        if (!photonView.IsMine)
        {
            StartCoroutine(ApplySlowEnemy());
        }
    }
    IEnumerator ApplySlowEnemy()
    {
        Ability = null;
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
        if (!photonView.IsMine)
        {
            StartCoroutine(ApplyReverseControlEnemy());
        }
    }
    IEnumerator ApplyReverseControlEnemy()
    {
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
}
