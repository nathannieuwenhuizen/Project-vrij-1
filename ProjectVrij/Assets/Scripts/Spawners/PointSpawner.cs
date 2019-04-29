using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject pointPrefab;

    [SerializeField]
    private int amountOfPointsActive = 10;

    [SerializeField]
    private SpawnPosition[] spawnPoses;

    // Start is called before the first frame update
    void Start()
    {
        PoolManager.instance.CreatePool(pointPrefab, 30);
        for(int i = 0; i < amountOfPointsActive; i++)
        {
            SpawnPoint();
        }
    }
    public void SpawnPoint()
    {
        SpawnPosition randomPos = getRandomActivePos();
        randomPos.IsVacant = false;
        GameObject point = PoolManager.instance.ReuseObject(pointPrefab, randomPos.transform.position, Quaternion.identity);
        point.GetComponent<PointObject>().myPos = randomPos;
    }
    public SpawnPosition getRandomActivePos()
    {
        return getActivePos()[(int)Mathf.Floor(Random.value * getActivePos().Count)];
    }
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
