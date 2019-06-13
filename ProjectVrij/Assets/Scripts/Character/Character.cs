using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using FMOD;

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
    protected Transform cameraPivot;

    /// <summary>
    /// Walk movement variables
    /// </summary>
    [Header("Character Movement")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float jumpForce;

    private bool isGrounded = true;

    private bool doesNothing = false;
    private Coroutine doingNothing = null;
    //sounds
    [Header("Sounds")]
    [FMODUnity.EventRef] public string walkSound;
    [FMODUnity.EventRef] public string jumpSound;
    [FMODUnity.EventRef] public string landingSound;
    [FMODUnity.EventRef] public string gotHitSound;
    [FMODUnity.EventRef] public string deathSound;
    [FMODUnity.EventRef] public string wooshSound;

    [SerializeField]
    private AudioClip walkSound2;
    [SerializeField]
    private AudioClip jumpSound2;
    [SerializeField]
    private AudioClip landingSound2;
    [SerializeField]
    private AudioClip gotHitSound2;
    [SerializeField]
    private AudioClip deathSound2;
    [SerializeField]
    private AudioClip wooshSound2;

    private float walkIndex;
    protected AudioSource movementAudioSource;
    protected AudioSource voiceAudioSource;

    // Overige variables
    protected Rigidbody rb;
    [Header("overige dingen")]
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    public PlayerUI ui;
    [SerializeField]
    public Camera camera;
    [SerializeField]
    private GameObject pointPrefab;
    private bool knocked = false;

    private PlayerSpawner ps;

    //point information
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
        CameraFadeFromBlack();

        IsGrounded = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //If grounded, play music and tilt the camera bit.
        if (IsGrounded)
        {
            //gets the max speed of the x-speed or z-speed
            float directionalSpeed = Mathf.Max(Mathf.Abs(rb.velocity.x), Mathf.Abs(rb.velocity.z));
            walkIndex += directionalSpeed / 100;
            if (walkIndex > 1)
            {
                walkIndex = 0;
                PlaySound(movementAudioSource, walkSound2, .5f);
                FMODUnity.RuntimeManager.PlayOneShot(walkSound, transform.position);
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
            knocked = false;
            PlaySound(movementAudioSource, landingSound2, 1f);
            FMODUnity.RuntimeManager.PlayOneShot(landingSound, transform.position);
            IsGrounded = true;
            ParticleManager.instance.SpawnParticle(ParticleManager.instance.landImpactParticle, transform.position, transform.rotation);

        }
    }
    public void OnCollisionStay(Collision collision)
    {
        IsGrounded = true;
    }
    public void OnCollisionExit(Collision collision)
    {
        if (isGrounded)
        {
            isGrounded = false;
            StartCoroutine(IsJustInTheAir());
        }
    }
    IEnumerator IsJustInTheAir()
    {
        float duration = 0.3f;
        float index = 0;
        while (!isGrounded)
        {
            index += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            if (index > duration)
            {
                IsGrounded = false;
                break;
            }
        }
    }

    /// <summary>
    /// Activated when something with a hitbox collider hitted the player
    /// </summary>
    /// <param name="hit"></param>
    private void  GotHit(Hitbox hit)
    {
        camera.GetComponent<CameraShake>().Shake(0.05f);

        camera.GetComponent<CameraFade>().fadingColor = Color.red;
        camera.GetComponent<CameraFade>().fadingOut = false;
        camera.GetComponent<CameraFade>().alphaFadeValue = 0.3f;
        camera.GetComponent<CameraFade>().fadeSpeed = 1f;

        SetAnimation("hitted", true);
        StartCoroutine(SetanimationBoolFalse("hitted", 0.2f));

        PlaySound(voiceAudioSource, gotHitSound2, 1f);
        FMODUnity.RuntimeManager.PlayOneShot(gotHitSound, transform.position);
        characterThatHitYou = hit.Character;
        Health -= hit.Damage;
    }
    IEnumerator SetanimationBoolFalse(string val, float duration)
    {
        yield return new WaitForSeconds(duration);
        anim.SetBool(val, false);

    }
    /// <summary>
    /// Your death!
    /// </summary>
    public override void Death() 
    {
        base.Death();
        anim.SetLayerWeight(1, 0);
        SetAnimation("dead", true);

        //other player recieves point (MUST BE CHANGED LATER!)
        float spread = 100f;

        for (int i = 0; i < Points; i++)
        {
            PointObject point = PoolManager.instance.ReuseObject(pointPrefab, transform.position + new Vector3(0, 2f, 0), Quaternion.identity).GetComponent<PointObject>();
            point.myPos = null;
            point.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-spread, spread), Random.Range(spread, spread * 2), Random.Range(-spread, spread)));
        }

        Points = 0;

        //death particle at position and explode.
        ParticleManager.instance.SpawnParticle(ParticleManager.instance.deathParticle, transform.position + new Vector3(0,1.5f,0), transform.rotation);

        //plays death sound
        PlaySound(voiceAudioSource, deathSound2);
        FMODUnity.RuntimeManager.PlayOneShot(deathSound, transform.position);

        //character falls back by changing the force of the rigidbody.
        if (characterThatHitYou != null)
        {
            //characterThatHitYou.Points += Points + 1;
            //Points = 0;
            KnockBack(10f, 5f, characterThatHitYou.transform.position);
        }

        StartCoroutine(Respawning());
    }

    public void KnockBack(float force, float forceY, Vector3 hitBoxPos)
    {
        knocked = true;
        Vector3 fallBackForce = Vector3.Normalize(transform.position - hitBoxPos) * force;
        fallBackForce.y = forceY;
        rb.velocity = fallBackForce;
    }

    /// <summary>
    /// Just to make the death animation a bit longer.
    /// </summary>
    /// <returns></returns>
    IEnumerator Respawning()
    {
        yield return new WaitForSeconds(2f);
        CameraFadeToBlack();
        yield return new WaitForSeconds(.8f);
        Respawn();
    }
    /// <summary>
    /// Revives and spawns the player somewhere on the map
    /// </summary>
    public void Respawn()
    {
        SetAnimation("dead", false);
        anim.SetLayerWeight(1, 1);

        CameraFadeFromBlack();
        Health = MaxHealth;
        characterThatHitYou = null;
        ps.RespawnPlayer(this);

    }
    public void CameraFadeToBlack()
    {
        camera.GetComponent<CameraFade>().fadingColor = Color.black;
        camera.GetComponent<CameraFade>().fadingOut = true;
        camera.GetComponent<CameraFade>().alphaFadeValue = 0;
        camera.GetComponent<CameraFade>().fadeSpeed = .5f;
    }
    public void CameraFadeFromBlack()
    {
        camera.GetComponent<CameraFade>().fadingColor = Color.black;
        camera.GetComponent<CameraFade>().fadingOut = false;
        camera.GetComponent<CameraFade>().alphaFadeValue = 1;
        camera.GetComponent<CameraFade>().fadeSpeed = .5f;
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
        if (Health == 0 || knocked) { return; }

        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.velocity += transform.right * h_input * walkSpeed;
        rb.velocity += transform.forward * y_input * walkSpeed;

        anim.SetFloat("hMove", h_input);
        anim.SetFloat("yMove", y_input);
        if (h_input == 0 && y_input == 0)
        {
            DoesNothing();
        } else
        {
            DoesSomething();
        }
    }

    /// <summary>
    /// Jumps the player by adding force to the rigidbody
    /// </summary>
    public virtual void Jump()
    {
        if (IsGrounded == true)
        {

            //changes the y velocity
            Vector3 jumpVelocity = rb.velocity;
            jumpVelocity.y = jumpForce;
            rb.velocity = jumpVelocity;

            IsGrounded = false;
            PlaySound(movementAudioSource, jumpSound2, 1f);
            FMODUnity.RuntimeManager.PlayOneShot(jumpSound, transform.position);

            ParticleManager.instance.SpawnParticle(ParticleManager.instance.landImpactParticle, transform.position, transform.rotation);

        }
    }

    /// <summary>
    /// Here the special attack of the character is used.
    /// </summary>
    public virtual void SecondSpecialAttack()
    {
        //Debug.Log("second special attack base");
    }

    /// <summary>
    /// Here the special attack of the character is used.
    /// </summary>
    public virtual void SpecialAttack()
    {
        //Debug.Log("special attack base");
    }

    /// <summary>
    /// Here the special attack release of the character is used.
    /// </summary>
    public virtual void SpecialAttackRelease()
    {
        //Debug.Log("release special attack");
    }

    /// <summary>
    /// Makes the animation do a basic attack
    /// </summary>
    /// <param name="state"> The boolion that will determine whether the animation will do something</param>
    public void BasicAttack(bool state)
    {
        if (state == true)
        {
            PlaySound(movementAudioSource, wooshSound2);
            FMODUnity.RuntimeManager.PlayOneShot(wooshSound, transform.position);
            SetAnimation("isAttacking", true);
            StartCoroutine(SetanimationBoolFalse("isAttacking", 0.5f));

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
        //source.clip = clip;
        //source.volume = volume;
        //source.Play();
    }

    /// <summary>
    /// changes camera size and changes input according to the palyer id and how many players are playing
    /// </summary>
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
        float widthOffset = 10;
        float inset = 50;
        if (playerID % 2 == 0 && playerID != 0)
        {
            ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, Screen.width / 2 + inset, widthOffset);
        }
        else
        {
            ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, widthOffset);
        }
        switch (playerID)
        {
            case 1:
                cameraPos = new Vector2(0, .5f);
                break;
            case 2:
                cameraPos = new Vector2(.5f, .5f);
                break;
            case 3:
                cameraPos = new Vector2(0, 0);
                break;
            case 4:
                cameraPos = new Vector2(.5f, 0);
                break;
            default:
                cameraPos = new Vector2(0, .5f);
                break;
        }
        ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, widthOffset);

        if (GameInformation.PLAYER_COUNT <= 2)
        {
            cameraPos.y = 0f;
        } else
        {
            if (playerID < 3)
            {
                ui.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, Screen.height / 2 + inset / 2, widthOffset);
            }
        }
        camera.rect = new Rect(cameraPos, cameraSize);
    }

    IEnumerator DoingNothing()
    {
        yield return new WaitForSeconds(30f + Random.value * 10);
        anim.SetBool("yawning", true);
        anim.SetLayerWeight(1, 0);

        //yield return new WaitForSeconds(8.4f);
        //anim.SetBool("yawning", false);
        //anim.SetLayerWeight(1, 1);
        //doesNothing = false;
        //DoesNothing();
    }

    private bool IsGrounded
    {
        get { return isGrounded; }
        set {
            isGrounded = value;
            anim.SetBool("isJumpingUp", !isGrounded);
        }
    }
    public void SetAnimation(string state, bool val)
    {
        DoesSomething();
        anim.SetBool(state, val);
    }
    private void DoesNothing()
    {
        if (!doesNothing)
        {
            doesNothing = true;
            doingNothing = StartCoroutine(DoingNothing());
        }
    }
    private void DoesSomething()
    {
        if (doesNothing)
        {
            doesNothing = false;
            StopCoroutine(doingNothing);
            anim.SetBool("yawning", false);
            anim.SetLayerWeight(1, 1);

        }
    }
}
