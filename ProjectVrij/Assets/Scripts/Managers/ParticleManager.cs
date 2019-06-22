using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Header("Character particles")]
    public ParticleGroup deathParticle;
    public ParticleGroup landImpactParticle;
    public ParticleGroup hitParticle;
    public ParticleGroup chargeParticles;

    [Space]
    [Header("point particles")]
    public ParticleGroup spawnPointParticle;
    public ParticleGroup collectPointParticle;

    [Space]
    [Header("projjectile particles")]
    public ParticleGroup projectileHitFire;
    public ParticleGroup projectileHitIce;
    public ParticleGroup projectileSpawn;

    public static ParticleManager instance;
    void Awake()
    {
        instance = this;

        InitiatePool(deathParticle);
        InitiatePool(landImpactParticle);
        InitiatePool(hitParticle);
        InitiatePool(chargeParticles);

        InitiatePool(spawnPointParticle);
        InitiatePool(collectPointParticle);

        InitiatePool(projectileHitFire);
        InitiatePool(projectileHitIce);
        InitiatePool(projectileSpawn);



    }
    public void InitiatePool(ParticleGroup particle)
    {
        PoolManager.instance.CreatePool(particle.particlePrefab, particle.amount);
    }

    public GameObject SpawnParticle(ParticleGroup particle, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject obj = PoolManager.instance.ReuseObject(particle.particlePrefab, spawnPosition, spawnRotation);
        Debug.Log(obj);
        Debug.Log(obj.GetComponent<ParticleExplosion>());
        if (obj.GetComponent<ParticleExplosion>() != null)
        {
            obj.GetComponent<ParticleExplosion>().Explode();
        }
        return obj;
    }
}
[System.Serializable]
public class ParticleGroup: System.Object
{
    public GameObject particlePrefab;
    public int amount;
}
