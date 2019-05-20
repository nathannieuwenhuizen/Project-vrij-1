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

    [SerializeField]
    private BoxCollider chargeHitbox;

    private bool isCharging = false;
    private InputHandler inputHandlerScript;


    protected override void Start()
    {
        Debug.Log("start child");
        rb = GetComponent<Rigidbody>();
        inputHandlerScript = GetComponent<InputHandler>();
        chargeHitbox.enabled = false;
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
        while(forceDuration < maxForceIncreaseDuration)
        {
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

}
