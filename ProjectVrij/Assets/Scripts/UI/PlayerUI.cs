using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The playerUI class shows the relevant data of the character on the screen.
/// </summary>
public class PlayerUI : MonoBehaviour
{

    [SerializeField]
    Image healthImageValue;

    Material material;

    [SerializeField]
    private Image ability1;
    [SerializeField]
    private Image ability2;


    [SerializeField]
    Text pointText;
    [SerializeField]
    private Text savedPointText;

    [Space]
    [Header("abilities")]
    [SerializeField]
    private AiblityImages meleeAbilitieSprites;

    [SerializeField]
    private AiblityImages rangeAbilitieSprites;


    private void Start()
    {
        pointText = transform.GetComponentInChildren<Text>();
    }
   
    /// <summary>
    /// Changes the healthbar to fit the health precentage
    /// </summary>
    /// <param name="_amount"> varies between 0 and 1.</param>
    public void SetHealthAmount(float _amount)
    {
        if (healthImageValue != null)
        {
            healthImageValue.fillAmount = _amount;
        }
    }
    /// <summary>
    /// Set the point text with a parameter val.
    /// </summary>
    /// <param name="val">the string</param>
    public void SetPointText(string val)
    {
        pointText.text = val;

        pointText.fontSize = 25 + 10;
        StartCoroutine(DecreaseNumber(pointText, 25));
    }
    IEnumerator DecreaseNumber(Text text, int number)
    {
        while (text.fontSize > number)
        {
            text.fontSize -= 1;
            yield return new WaitForSeconds(.01f);
        }
    }

    /// <summary>
    /// Set the saved point text with a parameter val.
    /// </summary>
    /// <param name="val">the string</param>
    public void SetSavedPointText(string val)
    {
        savedPointText.text = val;
        savedPointText.fontSize = 25 + 10;
        StartCoroutine(DecreaseNumber(savedPointText, 25));

    }

    //changes the ability icons to match the abilities of the player.
    public void SetCharacterType(int playerType)
    {
        if (playerType == 0)
        {
            ability1.sprite = meleeAbilitieSprites.ability1;
            ability2.sprite = meleeAbilitieSprites.ability2;
        }
        else
        {
            ability1.sprite = rangeAbilitieSprites.ability1;
            ability2.sprite = rangeAbilitieSprites.ability2;
        }
    }

    public void CoolDownAttack1(float duration)
    {
        SetReloadAbilityOn(ability1, duration);
    }
    public void CoolDownAttack2(float duration)
    {
        SetReloadAbilityOn(ability2, duration);
    }

    private void SetReloadAbilityOn(Image img, float duration)
    {
        StartCoroutine(ReloadingAbility(img, duration));
    }
    private IEnumerator ReloadingAbility(Image img, float duration)
    {
        float index = 0;
        while (index < duration)
        {
            index += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);

            img.fillAmount = index / duration;
        }
        img.fillAmount = 1;

    }
}
[System.Serializable]
public class AiblityImages : System.Object
{
    public Sprite ability1;
    public Sprite ability2;
}
