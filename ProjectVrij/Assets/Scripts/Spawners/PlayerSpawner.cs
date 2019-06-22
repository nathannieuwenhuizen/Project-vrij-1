using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The playspawner handles how and when the player characters are spawned and/or respawned.
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    public delegate void SpawnAction();
    public static event SpawnAction SpawningPlayer;

    //The spawnposes declaerd in gthe editor, here is where the characters can be spawned to.
    [SerializeField]
    private Transform[] spawnPoses;

    //the character transforms
    private List<Transform> playerPoses;

    void Start()
    {
        
    }

    public void PositionAllPlayers()
    {
        //setup list
        playerPoses = new List<Transform> { };

        //playeposes is located based on finding their script (its used only once)
        Character[] players = Transform.FindObjectsOfType<Character>();
        Debug.Log(players.Length);
        for (int i = 0; i < players.Length; i++)
        {
            playerPoses.Add(players[i].transform);
            RepositionPlayer(playerPoses[i], spawnPoses[i]);
        }
    }

    /// <summary>
    /// respawns the palyer at the furthest position
    /// </summary>
    public void RespawnPlayer(Character player)
    {
        RepositionPlayer(player.transform, GetFurthestPositionFromPlayers());
    }

    /// <summary>
    /// changes the position and rotation of the player towards the position
    /// </summary>
    void RepositionPlayer(Transform player, Transform position)
    {
        player.position = position.position;
        player.rotation = position.rotation;
        Debug.Log(position.rotation.y);
    }

    /// <summary>
    /// gets the furthest spawnpoint from the player
    /// </summary>
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
