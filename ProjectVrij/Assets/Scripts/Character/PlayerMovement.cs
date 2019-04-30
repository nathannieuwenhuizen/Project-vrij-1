using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isGrounded;
    [Space]
    public float speed;
    [Space]
    public float jumpHeight;

    Rigidbody rb;
    CapsuleCollider col_size;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col_size = GetComponent<CapsuleCollider>();
        //anim = GetComponent<Animation>();
        isGrounded = true;
    }


    public void Jump()
    {
        if (isGrounded == true)
        {
            anim.SetBool("isJumpingUp", true);
            Vector3 jumpVelocity = rb.velocity;
            jumpVelocity.y = jumpHeight;
            rb.velocity = jumpVelocity;
            isGrounded = false;
        }
    }

    public void Walking(float h_input , float y_input)
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.velocity += transform.right * h_input * speed;
        rb.velocity += transform.forward * y_input * speed;


        anim.SetBool("isWalkingForward", y_input > 0.1);
        anim.SetBool("isWalkingBack", y_input < -0.1);
        anim.SetBool("isWalkingSide", h_input < -0.1 || h_input > 0.1);
        //else if(y_input < 0)
        //{
        //    anim.SetBool("isWalkingBack", true);
        //}
        //else if (h_input < 0 || h_input > 0)
        //{
        //    anim.SetBool("isWalkingSide", true);
        //}
    }
    public void Rotate(float y_input)
    {
        transform.Rotate( new Vector3(0, y_input, 0));
    }

    void OnCollisionEnter()
    {
        isGrounded = true;
        anim.SetBool("isJumpingUp", false);

    }

    public void BasicAttack(bool state)
    {
        if(state == true)
        {
            anim.SetBool("isAttacking", true);
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }
}
