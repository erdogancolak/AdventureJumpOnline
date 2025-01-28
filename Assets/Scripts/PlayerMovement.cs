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
    void Start()
    {
        view = GetComponent<PhotonView>();
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
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float sideWalk = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(sideWalk * moveSpeed * Time.deltaTime, rb.linearVelocity.y);
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

        moveSpeed = 350;
    }
}


