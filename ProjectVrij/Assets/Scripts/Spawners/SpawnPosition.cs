using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This spawnposition only holds if its empty or not. Is helpful for gthe pointspawner.
/// </summary>
public class SpawnPosition : MonoBehaviour
{
    private bool isEmpty = true;
    public bool IsEmpty
    {
        get { return isEmpty; }
        set { isEmpty = value; }
    }
    
}
