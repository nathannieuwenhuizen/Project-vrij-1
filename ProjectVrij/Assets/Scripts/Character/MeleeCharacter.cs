using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A special character that does melee combat
/// </summary>
public class MeleeCharacter : Character
{
    
    //private KeyCode specialAttackCode;
    [Header("Charge values")]
    private float forceDuration = 0f;
    [SerializeField]
    private float chargeSpeed = 100000f;
    [SerializeField]
    private float maxChargeDuration = 0.5f;
    [SerializeField]
    private float maxForceIncreaseDuration = 2f;
    [SerializeField]
    private float maxChargeDamage = 50f;

    private bool isCharging = false;

    [SerializeField]
    private BoxCollider chargeHitbox;

    [Space]
    [Header("Sowrd values")]
    [Range(0, 180)]
    [SerializeField]
    private float swordStartAngle = 50f;
    [Range(0, 180)]
    [SerializeField]
    private float swordEndAngle = 50f;

    [SerializeField]
    private float swordSpeed = 4f;
    [SerializeField]
    private int swordDamage = 80;
    [SerializeField]
    private Transform swordPivot;
    [SerializeField]
    private Hitbox swordHitBox;

    private bool isAttackingWithSword;
    
    private InputHandler inputHandlerScript;


    protected override void Start()
    {
        Debug.Log("start child");
        rb = GetComponent<Rigidbody>();
        inputHandlerScript = GetComponent<InputHandler>();
        chargeHitbox.enabled = false;

        swordHitBox.Character = this;
        swordHitBox.Damage = swordDamage;
        swordHitBox.gameObject.SetActive(false);
        //KeyCode specialAttackCode = inputHandlerScript.specialAttackCode;
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    public override void SpecialAttack()
    {
        base.SpecialAttack();
        StartCoroutine(IncreaseForce());

        //here comes the code for the charge
        Debug.Log("CHAARGE!");

    }

    private IEnumerator IncreaseForce()
    {
        camera.GetComponent<CameraShake>().Shake(60, 0.1f);
        while(forceDuration < maxForceIncreaseDuration)
        {
            camera.GetComponent<CameraShake>().intensity = forceDuration * 10f;
            forceDuration += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        forceDuration = maxForceIncreaseDuration;
        SpecialAttackRelease();
    }

    public override void SpecialAttackRelease()
    {
        if (isCharging || forceDuration == 0)
        {
            return;
        }
        base.SpecialAttackRelease();
        StopAllCoroutines();
        camera.GetComponent<CameraShake>().StopShake();
        GetComponent<InputHandler>().enabled = false;
        Vector3 localForce = transform.forward * chargeSpeed;
        GetComponent<Rigidbody>().AddForce(localForce);
        
        isCharging = true;
        float percentage = forceDuration / maxForceIncreaseDuration;
        chargeHitbox.GetComponent<Hitbox>().Damage = (int)(percentage * maxChargeDamage);
        StartCoroutine(Charging(percentage * maxChargeDuration));
        forceDuration = 0;

        Debug.Log("RELEEAASSEEE!");
    }

    private IEnumerator Charging(float duration)
    {
        chargeHitbox.enabled = true;
        yield return new WaitForSeconds(duration);
        chargeHitbox.enabled = false;
        isCharging = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<InputHandler>().enabled = true;
    }

    public override void SecondSpecialAttack()
    {
        base.SpecialAttack();
        SwordAttack();

    }
    public void SwordAttack()
    {
        if (isAttackingWithSword) { return; }

        StartCoroutine(SwordAttacking());
    }
    IEnumerator SwordAttacking()
    {
        isAttackingWithSword = true;
        anim.SetBool("slashing", true);
        yield return new WaitForSeconds(0.3f);

        swordHitBox.gameObject.SetActive(true);

        swordPivot.Rotate(new Vector3(0,-swordStartAngle, 0));
        while (Mathf.Rad2Deg * swordPivot.localRotation.y < swordEndAngle)
        {
            swordPivot.Rotate(new Vector3(0, swordSpeed, 0));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        anim.SetBool("slashing", false);

        swordPivot.Rotate(new Vector3(0, -swordStartAngle * 2, 0));
        swordHitBox.gameObject.SetActive(false);
        isAttackingWithSword = false;
    }

}
