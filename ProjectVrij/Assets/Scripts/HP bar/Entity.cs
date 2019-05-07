using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{   
    [Header("Health")]
    [SerializeField]
    private int maxHealth = 100;

    private int health;

    protected virtual void Start()
    {
        health = maxHealth;
    }
    public int Health
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
    public virtual void Death()
    {
        //Debug.Log("I am dead");
    }



}
