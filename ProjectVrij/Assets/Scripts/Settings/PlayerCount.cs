using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCount : MonoBehaviour
{
    public int playerCount;

    public void AmountOfPlayers(int playerID)
    {
        playerCount = playerID;
    }
}
