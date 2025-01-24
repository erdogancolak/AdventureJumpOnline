using System.Collections;
using UnityEngine;

public class StunPlatform : MonoBehaviour
{
    public float stunTime;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement playerMovement = collision.collider.GetComponent<PlayerMovement>();
        if(playerMovement != null)
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

        Destroy(this);
        platformScript.jumpPower = 12;
    }
}
