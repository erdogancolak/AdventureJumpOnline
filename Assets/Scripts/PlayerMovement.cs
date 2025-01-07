using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();
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


