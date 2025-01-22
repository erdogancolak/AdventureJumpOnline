using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    PhotonView view;

    [Header("References")]
    public GameObject playerModel;

    [Space]

    [Header("Settings")]
    public float moveSpeed;
    void Start()
    {
        view = GetComponent<PhotonView>();
        if(view.IsMine)
        {
            playerModel.GetComponent<SpriteRenderer>().color = Color.red;
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
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float sideWalk = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(sideWalk * moveSpeed * Time.deltaTime, rb.linearVelocity.y);
    }
}


