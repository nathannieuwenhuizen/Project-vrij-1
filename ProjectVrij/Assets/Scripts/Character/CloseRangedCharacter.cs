using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A special character that fires addition projectiles on a short distance range.
/// </summary>
public class CloseRangedCharacter : Character
{
    [Space]
    [Header("Shoot attack")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform shootPosition;
    [SerializeField]
    private float shootSpeed;
    [SerializeField]
    private int projectileDamage = 30;

    [Space]
    [Header("Spread Attack")]
    [SerializeField]
    private GameObject spreadProjectilePrefab;
    [SerializeField]
    private int spreadDamage = 20;
    [Range(10, 180)]
    [SerializeField]
    private float shootAngle = 50f;
    [SerializeField]
    private int amountOfSpreadProjectiles = 10;

    [Space]
    [Header("general shoot information")]
    [SerializeField]
    private float reloadTime = 0.5f;
    [SerializeField]
    private AudioClip shootSound;


    private bool reloading = false;

    protected override void Start()
    {
        PoolManager.instance.CreatePool(projectilePrefab, 10);
        PoolManager.instance.CreatePool(spreadProjectilePrefab, amountOfSpreadProjectiles * 4);

        Debug.Log("start child");
        rb = GetComponent<Rigidbody>();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void SecondSpecialAttack()
    {
        base.SpecialAttack();
        SpreadShoot();

    }

    public override void SpecialAttack()
    {
        base.SpecialAttack();

        if (reloading) { return; }

        reloading = true;
        Shoot(projectilePrefab, projectileDamage);
        StartCoroutine(Reloading());

    }

    /// <summary>
    /// Shoots a projectile
    /// </summary>
    public void Shoot(GameObject obj, int damage)
    {
        PlaySound(movementAudioSource, shootSound);
        GameObject projectile = PoolManager.instance.ReuseObject(obj, shootPosition.position, shootPosition.rotation);

        projectile.GetComponent<Projectile>().Spawn();
        projectile.GetComponent<Hitbox>().Character = this;
        projectile.GetComponent<Hitbox>().Damage = damage;
    }

    public void SpreadShoot()
    {
        Debug.Log("spread shoot");
        if (reloading) { return; }
        reloading = true;
        cameraPivot.Rotate(new Vector3(0, -shootAngle, 0));
        for (int i = 0; i < amountOfSpreadProjectiles; i++)
        {
            Shoot(spreadProjectilePrefab, spreadDamage);
            cameraPivot.Rotate(new Vector3(0, (shootAngle * 2) / amountOfSpreadProjectiles, 0));
        }
        cameraPivot.Rotate(new Vector3(0, -shootAngle, 0));

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
