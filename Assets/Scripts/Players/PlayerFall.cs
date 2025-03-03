using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerFall : MonoBehaviourPunCallbacks
{
    [Header("References")]
    private Rigidbody2D rb;
    public TMP_Text deadText;

    [Header("Values")]
    public float fallThreshold;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.linearVelocity.y < fallThreshold)
        {
            Die();
        }
    }

    void Die()
    {
        deadText.text = "<color=red>You Dead</color>";
    }
}


