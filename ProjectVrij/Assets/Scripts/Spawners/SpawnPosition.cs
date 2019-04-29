using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    private bool isVacant = true;
    public bool IsVacant
    {
        get { return isVacant; }
        set { isVacant = value; }
    }
    
}
