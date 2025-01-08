using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    public GameObject playerModel;

    PhotonView view;

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
        if (view.IsMine)
        {
            Movement();
        }
    }
    private void Movement()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), 0);
        Vector2 moveAmount = moveInput * moveSpeed * Time.deltaTime;
        transform.position += (Vector3)moveAmount;
    }
}


