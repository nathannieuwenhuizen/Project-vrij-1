using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPanel : MonoBehaviour
{

    [SerializeField]
    private GameObject[] charactermodels;

    public int index = 0;
    private void Start()
    {
        ApplyName();
    }
    public void MoveSelection(bool right)
    {
        index += right ? 1 : -1;
        if (index < 0) { index = GameInformation.CHARACTER_NAMES.Length - 1; }
        if (index > GameInformation.CHARACTER_NAMES.Length - 1) { index = 0; }

        ApplyName();
    }
    private void ApplyName()
    {
        charactermodels[0].SetActive(false);
        charactermodels[1].SetActive(false);
        charactermodels[index].SetActive(true);
    }
    private void OnEnable()
    {
        ApplyName();
    }
    private void OnDisable()
    {
        if (charactermodels[0])
        {
            charactermodels[0].SetActive(false);
        }
        if (charactermodels[1])
        {
            charactermodels[1].SetActive(false);
        }
    }


}
