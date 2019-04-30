using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    Slider slider;
    Material material;

    Text pointText;
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

        pointText = transform.GetComponentInChildren<Text>();
        
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
    public void SetPointText(string val)
    {
        pointText.text = val;
    }
}
