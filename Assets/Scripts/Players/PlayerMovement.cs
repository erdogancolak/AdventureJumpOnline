using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    PhotonView view;

    [Header("References")]
    public GameObject playerModel;

    [Space]

    [Header("Settings")]
    public float moveSpeed;
    public float stunTime;
    void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(!view.IsMine)
        {
            Color newColor = playerModel.GetComponent<SpriteRenderer>().color;
            newColor.a = 100f / 255f;
            playerModel.GetComponent<SpriteRenderer>().color = newColor;
        }
    }

    void FixedUpdate()
    {
        if (view == null) return;

        if (view.IsMine)
        {
            Movement();
        }
    }
    private void Movement()
    {
        Animator animator = transform.GetChild(0).GetComponent<Animator>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float sideWalk = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(sideWalk * moveSpeed * Time.deltaTime, rb.linearVelocity.y);

        animator.SetBool("isRun", true);

        if (sideWalk == 0)
        {
            animator.SetBool("isRun", false);
        }
        Vector2 newScale = transform.GetChild(0).localScale;
        if (sideWalk < 0)
        {
            newScale.x = -1;
        }
        if (sideWalk > 0)
        {
            newScale.x = 1;
        }
        transform.GetChild(0).localScale = newScale;
    }
    public void Stun()
    {
        if(view.IsMine)
        {
            StartCoroutine(StunIE());
        }
    }
    IEnumerator StunIE()
    {
        moveSpeed = 0;

        yield return new WaitForSeconds(stunTime);

        moveSpeed = 200;
    }
}


