using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    [SerializeField]
    private Transform[] spawnPoses;

    [SerializeField]
    private Transform[] playerPoses;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < playerPoses.Length; i++)
        {
            RepositionPlayer(playerPoses[i], spawnPoses[i]);
        }
    }
    public void RespawnPlayer(Character player)
    {
        RepositionPlayer(player.transform, GetFurthestPositionFromPlayers());
    }
    void RepositionPlayer(Transform player, Transform position)
    {
        player.position = position.position;
    }
    Transform GetFurthestPositionFromPlayers()
    {
        float furthestDistance = 0;
        Transform furthestIndex = spawnPoses[0];
        foreach(Transform pos in spawnPoses)
        {
            float minDistance = 10000f; // just a huge number...
            foreach (Transform player in playerPoses)
            {
                float cDistance = Vector3.Distance(player.position, pos.position);
                minDistance = Mathf.Min(minDistance, cDistance);
            }
            if (furthestDistance < minDistance)
            {
                furthestDistance = minDistance;
                furthestIndex = pos;
            }
        }
        return furthestIndex;
    }
    
}
