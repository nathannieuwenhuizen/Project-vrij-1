using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolObject
{
    [SerializeField]
    private float speed = 0.8f;
    public void Update()
    {
        transform.Translate(Vector3.forward * speed);
    }

    public override void OnObjectReuse()
    {

    }

    public void OnCollisionEnter(Collision col)
    {
        this.Destroy();
    }
}
