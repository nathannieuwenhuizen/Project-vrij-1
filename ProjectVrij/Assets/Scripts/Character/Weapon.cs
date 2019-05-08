using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform shootPosition;
    [SerializeField]
    private float reloadTime = 0.5f;
    private bool reloading = false;


    // Start is called before the first frame update
    void Start()
    {
        //PoolManager.instance.CreatePool(projectilePrefab, 10);
    }

    public void Shoot()
    {
        if (reloading) { return; }

        //GameObject projectile = PoolManager.instance.ReuseObject(projectilePrefab, shootPosition.position, shootPosition.rotation);
        GameObject bulletClone = Instantiate(projectilePrefab, shootPosition.position, shootPosition.rotation);
        bulletClone.GetComponent<Projectile>().playerID = this.GetComponent<Character>();
        StartCoroutine(Reloading());
    }
    IEnumerator Reloading()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Shoot();
        //}
    }
}
