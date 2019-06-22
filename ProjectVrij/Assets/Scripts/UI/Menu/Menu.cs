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

    private CameraFade camFade;
    private bool starting = false;

    [SerializeField]
    private Text playText;

    [SerializeField]
    private LoadingScreenController loadScreen;

    private void Start()
    {
        Time.timeScale = 1;
        UpdatePlayerAmount();
        camFade = FindObjectOfType<CameraFade>();
    }
    public void UpdatePlayerAmount()
    {
        GameInformation.PLAYER_COUNT = (int)slider.value;
        text.text = slider.value + " players";
    }
    public void StartGame()
    {
        if (starting)
        {
            return;
        }
        playText.text = "loading";
        starting = true;
        if (camFade != null)
        {
            //camFade.fadingOut = true;
        }
        loadScreen.LoadScene(1);
        //StartCoroutine(FadingOut());
    }
    IEnumerator FadingOut()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(1);
        starting = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
