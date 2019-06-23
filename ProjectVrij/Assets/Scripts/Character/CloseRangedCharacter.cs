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
    private float SpreadreloadTime = 0.5f;
    [SerializeField]
    private AudioClip shootSound;

    [FMODUnity.EventRef] public string iceShot;
    [FMODUnity.EventRef] public string fireShot;
   


    private bool Spreadreloading = false;
    private bool reloading = false;

    protected override void Start()
    {
        PoolManager.instance.CreatePool(projectilePrefab, 10);
        PoolManager.instance.CreatePool(spreadProjectilePrefab, amountOfSpreadProjectiles * GameInformation.PLAYER_COUNT);

        rb = GetComponent<Rigidbody>();
        ui.SetCharacterType(1);
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void SecondSpecialAttack()
    {
        base.SpecialAttack();

        if (Spreadreloading) { return; }
        Spreadreloading = true;

        SetAnimation("shooting", true);
        StartCoroutine(SpreadShoot());


    }

    public override void SpecialAttack()
    {
        base.SpecialAttack();

        if (reloading) { return; }
        reloading = true;

        SetAnimation("shooting", true);
        StartCoroutine(PushBack());

        FMODUnity.RuntimeManager.PlayOneShot(iceShot, transform.position);

    }

    /// <summary>
    /// Shoots a projectile
    /// </summary>
    public void InstantiateBullet(GameObject obj, int damage)
    {
        PlaySound(movementAudioSource, shootSound);
        GameObject projectile = PoolManager.instance.ReuseObject(obj, shootPosition.position, shootPosition.rotation);

        projectile.GetComponent<Projectile>().Spawn();
        projectile.GetComponent<Hitbox>().Character = this;
        projectile.GetComponent<Hitbox>().Damage = damage;
        
    }

    public IEnumerator PushBack()
    {
        yield return new WaitForSeconds(0.2f);
        //ParticleManager.instance.SpawnParticle(ParticleManager.instance.projectileSpawn, shootPosition.position, transform.rotation);
        InstantiateBullet(projectilePrefab, projectileDamage);

        ui.CoolDownAttack2(reloadTime);
        yield return StartCoroutine(Reloading(reloadTime));
        reloading = false;

    }

    public IEnumerator SpreadShoot()
    {
        FMODUnity.RuntimeManager.PlayOneShot(fireShot);
        yield return new WaitForSeconds(0.2f);

        ParticleManager.instance.SpawnParticle(ParticleManager.instance.projectileSpawn, shootPosition.position, transform.rotation);

        cameraPivot.Rotate(new Vector3(0, -shootAngle, 0));
        for (int i = 0; i < amountOfSpreadProjectiles; i++)
        {
            InstantiateBullet(spreadProjectilePrefab, spreadDamage);
            cameraPivot.Rotate(new Vector3(0, (shootAngle * 2) / amountOfSpreadProjectiles, 0));
        }
        cameraPivot.Rotate(new Vector3(0, -shootAngle, 0));

        ui.CoolDownAttack1(SpreadreloadTime);
        yield return StartCoroutine(Reloading(SpreadreloadTime));
        Spreadreloading = false;
    }


    /// <summary>
    /// Reloads the weapon, making it not usable for reloadtime long.
    /// </summary>
    /// <returns></returns>
    IEnumerator Reloading(float duration)
    {
        yield return new WaitForSeconds(duration);
        SetAnimation("shooting", false);

    }
}
