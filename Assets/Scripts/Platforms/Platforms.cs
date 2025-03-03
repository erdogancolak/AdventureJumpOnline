using System.Collections;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    [Header("SFX")]
    public AudioClip audioClip;
    public float sfxVolume;

    [Space]

    [Header("Settings")]
    public float jumpPower;

    public float destroyFloat;
    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
        if (rb != null && rb.linearVelocity.y <= 0)
        {
            if(audioClip != null)
            {
                collision.gameObject.GetComponent<PlayerSFX>().PlaySFX(audioClip, sfxVolume);
            }
            rb.linearVelocity = new Vector2(0, jumpPower);
            Destroy(this.gameObject, destroyFloat);
        }
    }
}
