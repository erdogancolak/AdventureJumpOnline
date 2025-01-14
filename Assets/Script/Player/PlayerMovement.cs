using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed;
    Rigidbody2D rb;
    void Start()
    {
        if (!isLocalPlayer) return;

        rb = GetComponent<Rigidbody2D>();

        Camera.main.GetComponent<CameraFollow>().target = this.transform;
    }

    void FixedUpdate()
    {
        if(!isLocalPlayer) return;

        Movement();
    }
    private void Movement()
    {
        float sideWalk = Input.GetAxisRaw("Horizontal");

        Vector2 movement = new Vector2(sideWalk * moveSpeed * Time.deltaTime, rb.linearVelocity.y);
        rb.linearVelocity = movement;
    }
}
