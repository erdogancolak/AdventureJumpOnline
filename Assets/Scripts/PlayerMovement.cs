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

    void Update()
    {
        if (view == null) return;

        if (view.IsMine)
        {
            switch(speed)
            {
                case Speeds.regular:
                    Debug.Log(speed);
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
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);
        Vector2 moveAmount = moveInput * moveSpeed * Time.deltaTime;
        transform.position += (Vector3)moveAmount;
    }
    private void SlowMovement()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);
        Vector2 moveAmount = moveInput * (moveSpeed * .5f) * Time.deltaTime;
        transform.position += (Vector3)moveAmount;
    }
    private void ReverseMovement()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);
        Vector2 moveAmount = moveInput * (moveSpeed * -1) * Time.deltaTime;
        transform.position += (Vector3)moveAmount;
    }
    private void MuchSpeedMovement()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);
        Vector2 moveAmount = moveInput * (moveSpeed * 2) * Time.deltaTime;
        transform.position += (Vector3)moveAmount;
    }
    
}


