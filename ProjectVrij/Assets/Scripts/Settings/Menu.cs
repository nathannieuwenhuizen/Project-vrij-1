using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    [Header("Player amount")]
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Text text;
    private void Start()
    {
        UpdatePlayerAmount();
    }
    public void UpdatePlayerAmount()
    {
        GameInformation.PLAYER_COUNT = (int)slider.value;
        text.text = slider.value + " players";
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
