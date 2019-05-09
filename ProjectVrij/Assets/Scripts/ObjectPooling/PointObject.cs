using System.Collections;
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

    /// <summary>
    /// When being hitted
    /// </summary>
    /// <param name="col"></param>
    public void OnCollisionEnter(Collision col)
    {
        //if its a player
        if (col.gameObject.GetComponent<Character>() != null)
        {

            //player gets score;
            col.gameObject.GetComponent<Character>().Points++;

            //returns to the poolmanager.
            this.Destroy();

            //pointspawner spawns a new point.
            FindObjectsOfType<PointSpawner>()[0].SpawnPoint();

            //the point own spawnposition becomes empty. (is called later to prevent the point being spawned on the same location twice)
            myPos.IsVacant = true;

        }
    }
}
