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


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col_size = GetComponent<CapsuleCollider>();
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Walking();
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded == true)
        {
            Vector3 jumpVelocity = rb.velocity;
            jumpVelocity.y = jumpHeight;
            rb.velocity = jumpVelocity;
            isGrounded = false;
        }
    }

    void Walking()
    {
        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, rb.velocity.y, Input.GetAxis("Vertical") * speed);
    }

    void OnCollisionEnter()
    {
        isGrounded = true;
    }
}
