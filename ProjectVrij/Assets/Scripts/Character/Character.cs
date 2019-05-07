using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : Entity
{
    [SerializeField]
    private PlayerUI ui;

    private PlayerSpawner ps;

    private int points = 0;
    private Character character;

    // Start is called before the first frame update
    private void Start()
    {
        ps = Transform.FindObjectOfType<PlayerSpawner>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
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
            character = collision.gameObject.GetComponentInParent<Character>();
            Health -= 50;

        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hitbox")
        {
            Debug.Log("damage");
            character = collision.gameObject.GetComponentInParent<Character>();
            Health -= 50;

        }
    }
    public override void Death()
    {
        base.Death();
        character.Points += 1;
        Debug.Log("I am dead");
        Respawn();
    }
    public void Respawn()
    {
        Debug.Log("respawn");
        Health = MaxHealth;
        ps.RespawnPlayer(this);
    }
    public int Points
    {
        get { return points; }
        set { points = value;
            ui.SetPointText(points.ToString());
        }
    }
}
