using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : BaseStats
{
    [SerializeField]
    private PlayerUI ui;
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
            Health -= 50;
        }
    }
}
