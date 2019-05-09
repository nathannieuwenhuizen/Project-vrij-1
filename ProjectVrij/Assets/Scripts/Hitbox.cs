using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The hitbox class will be used to get the damage data from one character to the other.
/// This can vary between a punch of the hand/sword to a projectile/bullet cast from the character.
/// </summary>
public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private int damage = 50;
    [SerializeField]
    private Character character;


    /// <summary>
    /// This is hwere the hitbox comes from. 
    /// This also means that the character cant be hitted by this hitbox.
    /// </summary>
    public Character Character
    {
        get { return character; }
        set { character = value; }
    }

    /// <summary>
    /// WHen the character is hit, the health will be reduced by this damage.
    /// </summary>
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
}
