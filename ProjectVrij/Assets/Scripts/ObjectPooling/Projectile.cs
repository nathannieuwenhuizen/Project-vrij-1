using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolObject
{
    [SerializeField]
    private float speed = 0.8f;
    [SerializeField]
    private float destroyTime = 3f;
    public Character playerID;

    public void Update()
    {
        transform.Translate(Vector3.forward * speed);
        Destroy(this.gameObject, destroyTime);
    }

    public override void OnObjectReuse()
    {

    }

    public void OnCollisionEnter(Collision col)
    {
        Destroy(this.gameObject);
    }
}
