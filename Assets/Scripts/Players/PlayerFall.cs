using UnityEngine;
using Photon.Pun;

public class PlayerFall : MonoBehaviourPunCallbacks
{
    public float fallThreshold = -10f;  // D���� h�z� e�i�i
    private Rigidbody2D rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // E�er oyuncunun Y h�z� belirli bir e�ik de�erinin alt�na d��erse, oyuncu �l�r
        if (rb.linearVelocity.y < fallThreshold)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Oyuncu �ld�!");

    }
}


