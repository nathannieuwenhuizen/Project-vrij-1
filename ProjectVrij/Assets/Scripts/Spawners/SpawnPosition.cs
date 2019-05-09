using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This spawnposition only holds if its empty or not. Is helpful for gthe pointspawner.
/// </summary>
public class SpawnPosition : MonoBehaviour
{
    private bool isVacant = true;
    public bool IsVacant
    {
        get { return isVacant; }
        set { isVacant = value; }
    }
    
}
