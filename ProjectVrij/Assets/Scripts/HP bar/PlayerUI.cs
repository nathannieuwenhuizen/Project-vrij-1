using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    Material material;

    private Stats player;
    private void Start()
    {
        material = GetComponent<Image>().material;// GetComponent<Material>();
    }
   
    public void SetHealthAmount(float _amount)
    {
        material.SetFloat("_TimeAmmount", _amount);
    }
}
