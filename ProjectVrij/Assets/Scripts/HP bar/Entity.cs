using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class that has a health variable and can die... sort of
/// </summary>
public class Entity : MonoBehaviour
{   
    [Header("Health")]
    [SerializeField]
    private int maxHealth = 100;

    private int health;

    protected virtual void Start()
    {
        //sets the health as maxHealth
        Health = MaxHealth;
    }

    /// <summary>
    /// The value of health that the entity has, if it reaches 0, the entity dies.
    /// </summary>
    public virtual int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (health <= 0)
            {
                health = 0;
                Death();
            }
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }
    public int MaxHealth
    {
        get { return maxHealth; }
    }

    /// <summary>
    /// Called hwne health reaches 0.
    /// </summary>
    public virtual void Death()
    {
        //Debug.Log("I am dead");
    }



}
