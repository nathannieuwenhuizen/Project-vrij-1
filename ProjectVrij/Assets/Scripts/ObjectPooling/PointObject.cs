using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : PoolObject
{
    public SpawnPosition myPos;

    public void Update()
    {
        transform.Rotate(new Vector3(0, 1f, 0));
    }

    public override void OnObjectReuse()
    {

    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Character>() != null)
        {
            col.gameObject.GetComponent<Character>().Points++;
            //player gets score;
            this.Destroy();

            FindObjectsOfType<PointSpawner>()[0].SpawnPoint();
            myPos.IsVacant = true;

        }
    }
}
