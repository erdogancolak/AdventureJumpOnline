using UnityEngine;

public class Platforms : MonoBehaviour
{
    public float jumpPower;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
        if (rb != null && rb.linearVelocity.y <= 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }
    }
}
