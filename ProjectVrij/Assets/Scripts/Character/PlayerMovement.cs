using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    /// <summary>
    /// Camera movement variables
    /// 
    /// 
    /// </summary>
    [Header("Camera Rotation Sensetivity")]
    [SerializeField]
    private float rotationSpeedY = 10f;
    [SerializeField]
    private float rotationSpeedX = 10f;
    [Space]
    [SerializeField]
    [Range(0f, 89f)]
    private float maxDownAngle = 45f;
    [SerializeField]
    [Range(0f, 89f)]
    private float maxUpAngle = 45f;
    [SerializeField]
    private Transform cameraPivot;

    /// <summary>
    /// Rest of the variables of movement
    /// </summary>
    [Space]
    [Header("Character Movement")]
    public bool isGrounded;
    [Space]
    public float speed;
    [Space]
    public float jumpHeight;

    /// <summary>
    /// Overige variables
    /// </summary>
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

    //rotates the view of the player
    public void Rotate(float x_input, float y_input)
    {
        //rotates the body horizontally
        transform.Rotate(new Vector3(0, x_input * rotationSpeedY, 0));

        //rotates the camera vertically
        RotateCameraVertical(y_input);
    }

    //rotates the camera vertical up to maxes and mins
    private void RotateCameraVertical(float y_input)
    {
        //rotates the camera pivot
        cameraPivot.Rotate(new Vector3(y_input * rotationSpeedX, 0, 0));

        //this is all for capping the rotation.
        Vector3 currentRotation = cameraPivot.localRotation.eulerAngles;
        if (currentRotation.x > 180)
        {
            currentRotation.x = -360 + currentRotation.x;
        }
        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxUpAngle, maxDownAngle);
        cameraPivot.localRotation = Quaternion.Euler(currentRotation);
    }

}
