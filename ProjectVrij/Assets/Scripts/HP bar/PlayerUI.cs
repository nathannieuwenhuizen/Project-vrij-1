using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    Slider slider;
    Material material;

    private Stats player;
    private void Start()
    {
        if(GetComponent<Image>() != null)
        {
            material = GetComponent<Image>().material;
        }
        if (GetComponent<Slider>() != null)
        {
            slider = GetComponent<Slider>();
        }
        // GetComponent<Material>();
        
    }
   
    public void SetHealthAmount(float _amount)
    {
        if(material != null)
        {
            material.SetFloat("_TimeAmmount", _amount);
        }
        if (slider != null)
        {
            slider.value = _amount;
        }
    }
}
