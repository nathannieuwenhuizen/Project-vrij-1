﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The object that the players must collect to increase score.
/// </summary>
public class PointObject : PoolObject
{
    //my spawnPosition, must be changed to private and a getter/setter must be made later.
    public SpawnPosition myPos;

    public void Update()
    {
        //a simple rotation flare to catch the players eye.
        transform.Rotate(new Vector3(0, 1f, 0));
    }

    public override void OnObjectReuse()
    {
    }
    public void Spawn()
    {
        ParticleManager.instance.SpawnParticle(ParticleManager.instance.spawnPointParticle, transform.position, transform.rotation);
    }

    public IEnumerator Collected()
    {
        ParticleManager.instance.SpawnParticle(ParticleManager.instance.collectPointParticle, transform.position, transform.rotation);

        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        //waits for a time before respawning
        yield return new WaitForSeconds(1f);


        //returns to the poolmanager.
        this.Destroy();

        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;

        //if its spawned at a spawn position.
        if (myPos != null)
        {
            //pointspawner spawns a new point.
            FindObjectsOfType<PointSpawner>()[0].SpawnPoint();

            //the point own spawnposition becomes empty. (is called later to prevent the point being spawned on the same location twice)
            myPos.IsEmpty = true;
        }
    }

    /// <summary>
    /// When being hitted
    /// </summary>
    /// <param name="col"></param>
    public void OnCollisionEnter(Collision col)
    {
        //if its a player
        if (col.gameObject.GetComponent<Character>() != null)
        {
            Character character = col.gameObject.GetComponent<Character>();
            //if it isn't dead by fallback;
            if (character.Health != 0)
            {
                //player gets score;
                character.Points++;

                StartCoroutine(Collected());
            }
        }
    }
}
