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
    [SerializeField]
    private float forceForward;
    [SerializeField]
    private float forceUp;

    [SerializeField]
    private bool pushesPlayerBack = false;

    public Character playerID;
    private Character characterThatHitYou;

    [FMODUnity.EventRef] public string iceHit;




    public void FixedUpdate()
    {
        //goes forward with speed!
        transform.Translate(Vector3.forward * speed);

        //probably changed later?
        //Destroy(this.gameObject, destroyTime);
    }

    public override void OnObjectReuse()
    {
    }
    public override void Destroy()
    {
        base.Destroy();
    }
    public void Spawn()
    {
        StartCoroutine(Destroying());
        //GetComponent<Rigidbody>().AddForce(new Vector3(speed, 0, 0));
    }
    IEnumerator Destroying()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy();
    }
    //when it hits something.
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Projectile>())
        {
            //projectile.GetComponent<Character>().KnockBack(forceForward, forceUp);
            return;
        }
        if (col.gameObject.GetComponent<Character>() && pushesPlayerBack)
        {
            Debug.Log("PUSH BAACCCKKK");
            col.gameObject.GetComponent<MeleeCharacter>().KnockBack(10f, 10f, transform.position);
            FMODUnity.RuntimeManager.PlayOneShot(iceHit, transform.position);
            
        }
        ParticleManager.instance.SpawnParticle(ParticleManager.instance.projectileHit, transform.position, transform.rotation);
        Destroy();
    }
}
