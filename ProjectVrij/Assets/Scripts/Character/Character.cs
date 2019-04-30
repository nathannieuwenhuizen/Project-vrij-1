using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : Entity
{
    [SerializeField]
    private PlayerUI ui;

    [SerializeField]
    private PlayerSpawner ps;
    // Start is called before the first frame update
    private void Start()
    {
        base.Start();
        //HealthSystem(health);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GetHealthPct());
        ui.SetHealthAmount(GetHealthPct());
    }

    public float GetHealthPct()
    {
        return (float)Health / (float)MaxHealth;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Hitbox")
        {
            Debug.Log("damage");
            Health -= 50;
            
        }
    }
    public override void Death()
    {
        base.Death();
        Debug.Log("I am dead");
        Respawn();
    }
    public void Respawn()
    {
        Debug.Log("respawn");
        Health = MaxHealth;
        ps.RespawnPlayer(this);
    }
}
