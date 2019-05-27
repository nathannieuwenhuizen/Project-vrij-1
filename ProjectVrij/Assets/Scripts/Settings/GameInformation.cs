using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInformation
{
    public static int PLAYER_COUNT = 2;
    public static string[] CHARACTER_NAMES =
    {
       "swordman Bob",
       "archer Bob"
    };
    public static List<int> CHOSEN_CHARACTERS;

    public static bool FROM_MENU = false;
}
