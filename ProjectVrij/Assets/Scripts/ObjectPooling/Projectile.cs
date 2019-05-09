using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The basic projectile class that the character shoots
/// </summary>
public class Projectile : PoolObject
{
    //speed
    [SerializeField]
    private float speed = 0.8f;
    //when it destroys itself
    [SerializeField]
    private float destroyTime = 3f;
    public Character playerID;

    public void Update()
    {
        //goes forward with speed!
        transform.Translate(Vector3.forward * speed);

        //probably changed later?
        //Destroy(this.gameObject, destroyTime);
    }

    public override void OnObjectReuse()
    {

    }

    //when it hits something.
    public void OnCollisionEnter(Collision col)
    {
        Destroy(this.gameObject);
    }
}
