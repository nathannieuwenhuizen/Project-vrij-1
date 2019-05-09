using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// The round manager keeps track on the timer and who wins at the end of each round.
/// </summary>
public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private GameObject resultScreen;
    [SerializeField]
    private Text winningText;


    //The enable/disable are needed for events, if a scene changes, the event will still be active, this prevents it from being active removing potential bugs.
    void OnEnable()
    {
        //Adds the event when the timer reaches zero
        CountDownTimer.OnZero += GameOver;
    }
    void OnDisable()
    {
        //Removes the event when the timer reaches zero
        CountDownTimer.OnZero -= GameOver;
    }

    /// <summary>
    /// The game stops and the resultscreen becomes active.
    /// </summary>
    void GameOver()
    {

        resultScreen.SetActive(true);
        winningText.text = (HighestScoredPlayer().transform.name + " wins!");
        //Time.timeScale = 0;
    }

    /// <summary>
    /// Reloads the active scene, will maybe be put in a seperate scene manager class...
    /// </summary>
    public void ReloadScene()
    {
        //Time.timeScale = 1;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    /// <summary>
    /// Returns the player that has the highest score.
    /// </summary>
    /// <returns></returns>
    public Character HighestScoredPlayer()
    {
        Character[] chars = FindObjectsOfType<Character>();
        Character winningChar = chars[0];
        for (int i = 0; i < chars.Length; i++)
        {
            if (winningChar.Points < chars[i].Points)
            {
                winningChar = chars[i];
            }
        }
        return winningChar;
    }

}
