using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The pointspawner handles how and when the pointObject/crystals are spawned and/or respawned.
/// </summary>

public class PointSpawner : MonoBehaviour
{
    //the prefab that will be used for spawning
    [SerializeField]
    private GameObject pointPrefab;

    //how many point objects must be activa at the same time.
    [SerializeField]
    private int amountOfPointsActive = 10;

    //all the spawn locartions
    [SerializeField]
    private SpawnPosition[] spawnPoses;

    // Start is called before the first frame update
    void Start()
    {
        //PoolManager.instance.CreatePool(pointPrefab, 30);
        for(int i = 0; i < amountOfPointsActive; i++)
        {
            SpawnPoint();
        }
    }
    /// <summary>
    /// Spawns a point object on a random spawnpoint defined in the inspector.
    /// </summary>
    public void SpawnPoint()
    {
        SpawnPosition randomPos = getRandomActivePos();
        randomPos.IsVacant = false;

        //why is this commented?
        //GameObject point = PoolManager.instance.ReuseObject(pointPrefab, randomPos.transform.position, Quaternion.identity);
        //point.GetComponent<PointObject>().myPos = randomPos;
    }
    /// <summary>
    /// Returns a random spawnpoint
    /// </summary>
    /// <returns></returns>
    public SpawnPosition getRandomActivePos()
    {
        return getActivePos()[(int)Mathf.Floor(Random.value * getActivePos().Count)];
    }
    /// <summary>
    /// Returns a list of all spawnpoints that are vacant, can probably be optimized to a single line of code.
    /// </summary>
    /// <returns></returns>
    public List<SpawnPosition> getActivePos()
    {
        List<SpawnPosition> activePoses = new List<SpawnPosition> { };
        for (int i = 0; i < spawnPoses.Length; i++)
        {
            if (spawnPoses[i].IsVacant)
            {
                activePoses.Add(spawnPoses[i]);
            }
        }
        return activePoses;
    }

}
