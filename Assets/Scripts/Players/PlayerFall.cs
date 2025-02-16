using UnityEngine;
using Photon.Pun;

public class PlayerFall : MonoBehaviourPunCallbacks
{
    public float fallThreshold = -10f;  // Düþüþ hýzý eþiði
    private Rigidbody2D rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Eðer oyuncunun Y hýzý belirli bir eþik deðerinin altýna düþerse, oyuncu ölür
        if (rb.linearVelocity.y < fallThreshold)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Oyuncu öldü!");

    }
}


