using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseStats : MonoBehaviour
{
    private int health;
    [SerializeField]
    private int maxHealth;

    public void Start()
    {
        Debug.Log("base stats");
        health = maxHealth - 50;
    }
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (health < 0)
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
    public void Death()
    {
        Debug.Log("I am dead");
    }



}
