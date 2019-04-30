using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolObject
{
    public void Update()
    {
        transform.Translate(Vector3.forward);
    }

    public override void OnObjectReuse()
    {

    }

    public void OnCollisionEnter(Collision col)
    {
        this.Destroy();
    }
}
