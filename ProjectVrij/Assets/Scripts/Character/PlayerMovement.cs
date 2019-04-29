using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isGrounded;
    [Space]
    public float speed;
    public float w_speed;
    public float r_speed;
    [Space]
    public float jumpHeight;

    Rigidbody rb;
    CapsuleCollider col_size;

    private Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col_size = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        isGrounded = true;
    }


    public void Jump()
    {
        if (isGrounded == true)
        {
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
    }
    public void Rotate(float y_input)
    {
        transform.Rotate( new Vector3(0, y_input, 0));
    }

    void OnCollisionEnter()
    {
        isGrounded = true;
    }

    public void BasicAttack()
    {
        anim.Play("isAttacking", true);
    }
}
