using UnityEngine;
using Photon.Pun;

public class Jump : MonoBehaviourPunCallbacks
{  
    public float JumpSpeed = 5f;
    public int MaxJump = 1;
    int jumpAllowed;
    Rigidbody rb;
    bool onGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpAllowed = MaxJump;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!photonView.IsMine) return;
        if (Input.GetButtonDown("Jump"))
        {
            if (onGround)
            {
                jump();
                onGround = false;
            }

            else if (!onGround && jumpAllowed > 0)
            {
                jump();
                jumpAllowed--;

            
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
            jumpAllowed = MaxJump;

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = false;

        }
    }

    private void jump()
    {
        rb.AddForce(transform.up* JumpSpeed, ForceMode.VelocityChange);
    
    }
}
