using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private int damage = 50;
    [SerializeField]
    private Character character;

    public Character Character
    {
        get { return character; }
        set { character = value; }
    }
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
}
