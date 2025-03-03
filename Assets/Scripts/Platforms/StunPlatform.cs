using System.Collections;
using UnityEngine;

public class StunPlatform : MonoBehaviour
{
    [Header("SFX")]
    public AudioClip audioClip;
    public float sfxVolume;

    [Space]

    [Header("Values")]
    public float stunTime;

    public Sprite woodenPlatformSprite;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<PlayerSFX>().PlaySFX(audioClip,sfxVolume);
        PlayerMovement playerMovement = collision.collider.GetComponent<PlayerMovement>();
        Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
        if(playerMovement != null && rb.linearVelocity.y <= 0)
        {
            playerMovement.Stun();
            StartCoroutine(FinishStun());
        }
    }
    IEnumerator FinishStun()
    {
        Platforms platformScript = GetComponent<Platforms>();
        platformScript.jumpPower = 0;

        yield return new WaitForSeconds(stunTime);

        GetComponent<SpriteRenderer>().sprite = woodenPlatformSprite;
        Destroy(this);
        platformScript.jumpPower = 12;
    }
}
