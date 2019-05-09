using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The playerUI class shows the relevant data of the character on the screen.
/// </summary>
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
   
    /// <summary>
    /// Changes the healthbar to fit the health precentage
    /// </summary>
    /// <param name="_amount"> varies between 0 and 1.</param>
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
    /// <summary>
    /// Set the point text with a parameter val.
    /// </summary>
    /// <param name="val">the string</param>
    public void SetPointText(string val)
    {
        pointText.text = val;
    }
}
