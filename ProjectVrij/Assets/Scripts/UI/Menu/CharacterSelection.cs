using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{

    [SerializeField]
    private GameObject[] selections;


    public void ShowSelection()
    {
        for (int i = 0; i < selections.Length; i++)
        {
            selections[i].SetActive(i < GameInformation.PLAYER_COUNT);
        }
        ApplyCharacterSelection();
    }
    public void ApplyCharacterSelection()
    {
        List<int> chosen_characters = new List<int> { };
        for (int i = 0; i < GameInformation.PLAYER_COUNT; i++)
        {
            chosen_characters.Add(selections[i].GetComponent<CharacterSelectPanel>().index);
            selections[i].SetActive(i < GameInformation.PLAYER_COUNT);
        }
        GameInformation.CHOSEN_CHARACTERS = chosen_characters;
        GameInformation.FROM_MENU = true;
        Debug.Log("chosen characters: " + GameInformation.CHOSEN_CHARACTERS.Count + "chosen characters: " + GameInformation.CHOSEN_CHARACTERS[0] + "chosen characters: " + GameInformation.CHOSEN_CHARACTERS[1]);

    }
}
