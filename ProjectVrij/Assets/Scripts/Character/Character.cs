using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : Entity
{

    /// <summary>
    /// Camera movement variables
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
    /// Walk movement variables
    /// </summary>
    [Header("Character Movement")]
    private bool isGrounded;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float jumpForce;

    /// <summary>
    /// Overige variables
    /// </summary>
    protected Rigidbody rb;
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private PlayerUI ui;

    private PlayerSpawner ps;

    private int points = 0;
    private Character characterThatHitYou;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        ps = Transform.FindObjectOfType<PlayerSpawner>();
        rb = GetComponent<Rigidbody>();
        Debug.Log(rb);
        isGrounded = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ui.SetHealthAmount(GetHealthPct());
    }

    public float GetHealthPct()
    {
        return (float)Health / (float)MaxHealth;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Hitbox")
        {
            Debug.Log("damage");
            characterThatHitYou = collision.gameObject.GetComponentInParent<Character>();
            Health -= 50;

        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hitbox")
        {
            Debug.Log("damage");
            characterThatHitYou = collision.gameObject.GetComponentInParent<Character>();
            Health -= 50;

        } else
        {
            isGrounded = true;
            anim.SetBool("isJumpingUp", false);
        }
    }
    public override void Death()
    {
        base.Death();
        if (characterThatHitYou != null)
        {
            characterThatHitYou.Points += 1;
        }
        Debug.Log("I am dead");
        Respawn();
    }
    public void Respawn()
    {
        Debug.Log("respawn");
        Health = MaxHealth;
        ps.RespawnPlayer(this);
    }
    public int Points
    {
        get { return points; }
        set { points = value;
            ui.SetPointText(points.ToString());
        }
    }

    //---------------movement functions-----------------
    /// <summary>
    /// rotates the view of the player
    /// </summary>
    /// <param name="x_input"> rotates the body horizontally </param>
    /// <param name="y_input"> rotates the camera vertically </param>
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

    /// <summary>
    /// Walks the player by changing the velocity of the rigidbody in directions of right and forth.
    /// </summary>
    /// <param name="h_input"></param>
    /// <param name="y_input"></param>
    public void Walking(float h_input, float y_input)
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.velocity += transform.right * h_input * walkSpeed;
        rb.velocity += transform.forward * y_input * walkSpeed;


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

    /// <summary>
    /// Jumps the player by adding force to the rigidbody
    /// </summary>
    public virtual void Jump()
    {
        Debug.Log(rb);
        if (isGrounded == true)
        {
            anim.SetBool("isJumpingUp", true);
            Vector3 jumpVelocity = rb.velocity;
            jumpVelocity.y = jumpForce;
            rb.velocity = jumpVelocity;
            isGrounded = false;
        }
    }
    public virtual void SpecialAttack()
    {
        Debug.Log("special attack base");
    }

    /// <summary>
    /// Makes the animation do something
    /// </summary>
    /// <param name="state"> The boolion that will determine whether the animation will do something</param>
    public void BasicAttack(bool state)
    {
        if (state == true)
        {
            anim.SetBool("isAttacking", true);
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }
}
