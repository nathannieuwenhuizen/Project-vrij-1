using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCharacter : Character
{
    protected override void Start()
    {
        Debug.Log("start child");
        rb = GetComponent<Rigidbody>();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    public override void SpecialAttack()
    {
        base.SpecialAttack();

        //here comes the code for the charge
        Debug.Log("CHAARGE!");

    }

}
