using System.Collections;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    public float jumpPower;
    public float destroyFloat;
    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
        if (rb != null && rb.linearVelocity.y <= 0)
        {
            rb.linearVelocity = new Vector2(0, jumpPower);
            Destroy(this.gameObject, destroyFloat);
        }
    }
}
