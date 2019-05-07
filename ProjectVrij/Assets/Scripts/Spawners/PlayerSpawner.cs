using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public delegate void SpawnAction();
    public static event SpawnAction SpawningPlayer;

    [SerializeField]
    private Transform[] spawnPoses;

    private List<Transform> playerPoses;

    void Start()
    {
        //setup list
        playerPoses = new List<Transform> { };

        //playeposes is located based on finding their script (its used only once)
        PlayerMovement[] players = Transform.FindObjectsOfType<PlayerMovement>();
        for (int i = 0; i < players.Length; i++)
        {
            playerPoses.Add(players[i].transform);
            RepositionPlayer(playerPoses[i], spawnPoses[i]);
        }
    }

    //respawns the palyer at the furthest position
    public void RespawnPlayer(Character player)
    {
        RepositionPlayer(player.transform, GetFurthestPositionFromPlayers());
    }

    //changes the position and rotation of the player towards the position
    void RepositionPlayer(Transform player, Transform position)
    {
        player.position = position.position;
        player.rotation = position.rotation;
    }

    //gets the firthest spawnpoint from the player
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
