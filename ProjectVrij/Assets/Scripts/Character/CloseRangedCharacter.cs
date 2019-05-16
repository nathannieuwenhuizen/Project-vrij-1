using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A special character that fires addition projectiles on a short distance range.
/// </summary>
public class CloseRangedCharacter : Character
{
    [Header("Projectile Info")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform shootPosition;
    [SerializeField]
    private float reloadTime = 0.5f;
    [SerializeField]
    private int projectileDamage = 30;
    private bool reloading = false;
    [SerializeField]
    private AudioClip shootSound;


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

    /// <summary>
    /// Shoots a projectile
    /// </summary>
    public void Shoot()
    {
        if (reloading) { return; }
        reloading = true;
        PlaySound(movementAudioSource, shootSound);
        GameObject projectile = PoolManager.instance.ReuseObject(projectilePrefab, shootPosition.position, shootPosition.rotation);

        projectile.GetComponent<Projectile>().Spawn();
        projectile.GetComponent<Hitbox>().Character = this;
        projectile.GetComponent<Hitbox>().Damage = projectileDamage;
        StartCoroutine(Reloading());
    }

    /// <summary>
    /// Reloads the weapon, making it not usable for reloadtime long.
    /// </summary>
    /// <returns></returns>
    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }
}
