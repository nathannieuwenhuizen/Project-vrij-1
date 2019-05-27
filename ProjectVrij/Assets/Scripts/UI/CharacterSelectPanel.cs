using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPanel : MonoBehaviour
{
    [SerializeField]
    private Text characterText; //can be a mesh/image or something else

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
        characterText.text = GameInformation.CHARACTER_NAMES[index];
    }

}
