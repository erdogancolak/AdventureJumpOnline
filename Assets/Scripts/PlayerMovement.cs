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
    
    public enum Speeds { regular,slow,reverse,fast}
    public Speeds speed;

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
            switch(speed)
            {
                case Speeds.regular:
                    Movement();
                    break;
                case Speeds.slow:
                    SlowMovement();
                    Debug.Log(speed);
                    break;
                case Speeds.reverse:
                    ReverseMovement();
                    Debug.Log(speed);
                    break;
                case Speeds.fast:
                    MuchSpeedMovement();
                    Debug.Log(speed);
                    break;
                default:
                    return;
            }
        }
    }
    private void Movement()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float sideWalk = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(sideWalk * moveSpeed * Time.deltaTime, rb.linearVelocity.y);

        Debug.Log("Movement ÝÇÝNDEKÝ SPEED " + moveSpeed);
    }
    private void SlowMovement()
    {
        Debug.Log("SlowMovement");
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);
        Vector2 moveAmount = moveInput * (moveSpeed * .5f) * Time.deltaTime;
        transform.position += (Vector3)moveAmount;
    }
    private void ReverseMovement()
    {
        Debug.Log("ReverseMovement");
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);
        Vector2 moveAmount = moveInput * (moveSpeed * -1) * Time.deltaTime;
        transform.position += (Vector3)moveAmount;
    }
    private void MuchSpeedMovement()
    {
        Debug.Log("FastMovement");
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);
        Vector2 moveAmount = moveInput * (moveSpeed * 2) * Time.deltaTime;
        transform.position += (Vector3)moveAmount;
    }
    
}


