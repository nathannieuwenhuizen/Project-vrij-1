using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// The character is the base entity that the player plays as. (with camera and UI call) 
/// This character will be the base that other special classes uses, such as the melee character or close ranged character.
/// </summary>
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
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float jumpForce;

    private bool isGrounded = true;


    //sounds
    [Header("Sounds")]
    [SerializeField]
    private AudioClip walkSound;
    [SerializeField]
    private AudioClip jumpSound;
    [SerializeField]
    private AudioClip landingSound;
    [SerializeField]
    private AudioClip gotHitSound;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private AudioClip wooshSound;

    private float walkIndex;
    protected AudioSource movementAudioSource;
    protected AudioSource voiceAudioSource;

    // Overige variables
    protected Rigidbody rb;
    [Header("overige dingen")]
    [SerializeField]
    private Animator anim;
    [SerializeField]
    public PlayerUI ui;
    [SerializeField]
    private Camera camera;

    private PlayerSpawner ps;

    [SerializeField]
    private int points = 0;
    [SerializeField]
    private int savePoints = 0;
    private Character characterThatHitYou;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //components are defined
        ps = Transform.FindObjectOfType<PlayerSpawner>();
        rb = GetComponent<Rigidbody>();

        //audiosources are added for the character
        movementAudioSource = gameObject.AddComponent<AudioSource>();
        voiceAudioSource = gameObject.AddComponent<AudioSource>();

        ui.SetPointText(points.ToString());
        //Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //If grounded, play music and tilt the camera bit.
        if (isGrounded)
        {
            //gets the max speed of the x-speed or z-speed
            float directionalSpeed = Mathf.Max(Mathf.Abs(rb.velocity.x), Mathf.Abs(rb.velocity.z));
            walkIndex += directionalSpeed / 100;
            if (walkIndex > 1)
            {
                walkIndex = 0;
                PlaySound(movementAudioSource, walkSound, .5f);
            }

            //tilts the camera a bit
            Vector3 currentRotation = cameraPivot.localRotation.eulerAngles;
            currentRotation.z = Mathf.Sin(walkIndex * Mathf.PI * 2); 
            //cameraPivot.localRotation = Quaternion.Euler(currentRotation);

        }
    }

    /// <summary>
    /// Returns the precentage of the health from the max health
    /// </summary>
    /// <returns></returns>
    public float GetHealthPct()
    {
        return (float)Health / (float)MaxHealth;
    }

    public void OnTriggerEnter(Collider collision)
    {
        //if its a hitbox class
        if (collision.gameObject.GetComponent<Hitbox>())
        {
            //if it doesnt come from the player itself
            if (collision.gameObject.GetComponent<Hitbox>().Character != this)
            {
                //take damage
                GotHit(collision.gameObject.GetComponent<Hitbox>());
            }
        }

        if(collision.gameObject.tag == "PointAltar" && Points != 0)
        {
            collision.GetComponent<ParticleSystem>().Play();
            TransferPointsToAltar();
        }
    }
    public void TransferPointsToAltar()
    {
        SavedPoints += Points;
        Points = 0;
        ui.SetPointText(points.ToString());
    }
    public void OnCollisionEnter(Collision collision)
    {
        //if its a hitbox class
        if (collision.gameObject.GetComponent<Hitbox>())
        {
            //if it doesnt come from the player itself
            if (collision.gameObject.GetComponent<Hitbox>().Character != this)
            {
                //take damage
                GotHit(collision.gameObject.GetComponent<Hitbox>());
            }
        }
        //else its just ground
        else
        {
            PlaySound(movementAudioSource, landingSound, 1f);
            isGrounded = true;
            anim.SetBool("isJumpingUp", false);
        }
    }

    /// <summary>
    /// Activated when something with a hitbox collider hitted the player
    /// </summary>
    /// <param name="hit"></param>
    private void  GotHit(Hitbox hit)
    {
        PlaySound(voiceAudioSource, gotHitSound, 1f);
        characterThatHitYou = hit.Character;
        Health -= hit.Damage;
    }

    /// <summary>
    /// Your death!
    /// </summary>
    public override void Death()
    {
        base.Death();

        //other player recieves point (MUST BE CHANGED LATER!)
        if (characterThatHitYou != null)
        {
            characterThatHitYou.Points += Points + 1;
        }
        Points = 0;

        //plays death sound
        PlaySound(voiceAudioSource, deathSound);

        //character falls back by changing the force of the rigidbody.
        Vector3 fallBackForce = Vector3.Normalize(transform.position - characterThatHitYou.transform.position) * 10f;
        fallBackForce.y = 5f;
        rb.velocity = fallBackForce;

        StartCoroutine(Respawning());
    }

    /// <summary>
    /// Just to make the death animation a bit longer.
    /// </summary>
    /// <returns></returns>
    IEnumerator Respawning()
    {
        yield return new WaitForSeconds(1f);
        Respawn();
    }
    /// <summary>
    /// Revives and spawns the player somewhere on the map
    /// </summary>
    public void Respawn()
    {
        Debug.Log("respawn");
        Health = MaxHealth;
        characterThatHitYou = null;
        ps.RespawnPlayer(this);
    }

    /// <summary>
    /// A setter for the points integer, when a health is set, the ui updates automaticly.
    /// </summary>
    public int Points
    {
        get { return points; }
        set { points = value;
            ui.SetPointText(points.ToString());
        }
    }

    public int SavedPoints
    {
        get { return savePoints; }
        set {
            savePoints = value;
            ui.SetSavedPointText(savePoints.ToString());
        }
    }

    /// <summary>
    /// A setter for the heath integer, when a health is set, the ui updates automaticly.
    /// </summary>
    public override int Health {
        get => base.Health;
        set {
            base.Health = value;
            ui.SetHealthAmount(GetHealthPct());
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

    /// <summary>
    /// rotates the camera vertical up to maxes and mins
    /// </summary>
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
        //if the character is dead, it shouldnt be able to move.
        if (Health == 0) { return; }

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

            //changes the y velocity
            Vector3 jumpVelocity = rb.velocity;
            jumpVelocity.y = jumpForce;
            rb.velocity = jumpVelocity;

            isGrounded = false;
            PlaySound(movementAudioSource, jumpSound, 1f);

        }
    }

    /// <summary>
    /// Here the special attack of the character is used.
    /// </summary>
    public virtual void SpecialAttack()
    {
        Debug.Log("special attack base");
    }

    public virtual void SpecialAttackRelease()
    {
        Debug.Log("release special attack");
    }

    /// <summary>
    /// Makes the animation do a basic attack
    /// </summary>
    /// <param name="state"> The boolion that will determine whether the animation will do something</param>
    public void BasicAttack(bool state)
    {
        if (state == true)
        {
            PlaySound(movementAudioSource, wooshSound);
            anim.SetBool("isAttacking", true);
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }


    /// <summary>
    /// Plays a certain sound to the audioclip with a certain volume.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    public void PlaySound(AudioSource source, AudioClip clip, float volume = 1f)
    {
        source.clip = clip;
        source.volume = volume;
        source.Play();
    }


    //changes camera size and changes input according to the palyer id and how many players are playing
    public void ApplyPlayerSetting(int playerID)
    {
        //controller
        GetComponent<InputHandler>().ControllerID = playerID;

        Vector2 cameraSize = new Vector2(0.5f, 1f);
        if (GameInformation.PLAYER_COUNT > 2)
        {
            cameraSize = new Vector2(.5f, .5f);
        }
        Vector2 cameraPos = new Vector2(0, 0);
        float widthOffset = 90;
        switch (playerID)
        {
            case 1:
                cameraPos = new Vector2(0, .5f);
                ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, widthOffset);
                ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, widthOffset);

                break;
            case 2:
                cameraPos = new Vector2(.5f, .5f);
                ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, widthOffset);
                ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, widthOffset);

                break;
            case 3:
                cameraPos = new Vector2(0, 0);
                ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, widthOffset);
                ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, widthOffset);

                break;
            case 4:
                cameraPos = new Vector2(.5f, 0);
                ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, widthOffset);
                ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, widthOffset);

                break;
            default:
                cameraPos = new Vector2(0, .5f);
                ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, widthOffset);
                ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, widthOffset);

                break;
        }
        if (GameInformation.PLAYER_COUNT <= 2)
        {
            cameraPos.y = 0f;
        }
        camera.rect = new Rect(cameraPos, cameraSize);
        Debug.Log("camera size" + cameraPos.y + " player count = " + GameInformation.PLAYER_COUNT);
    }
    }
