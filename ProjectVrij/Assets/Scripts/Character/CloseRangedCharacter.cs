using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangedCharacter : Character
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform shootPosition;
    [SerializeField]
    private float reloadTime = 0.5f;
    private bool reloading = false;

    protected override void Start()
    {
        PoolManager.instance.CreatePool(projectilePrefab, 10);

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
        Shoot();

    }
    public void Shoot()
    {
        if (reloading) { return; }
        reloading = true;
        GameObject projectile = PoolManager.instance.ReuseObject(projectilePrefab, shootPosition.position, shootPosition.rotation);
        StartCoroutine(Reloading());
    }
    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }
}
