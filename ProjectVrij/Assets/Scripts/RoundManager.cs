using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private GameObject resultScreen;
    [SerializeField]
    private Text winningText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        CountDownTimer.OnZero += GameOver;
    }

    void OnDisable()
    {
        CountDownTimer.OnZero -= GameOver;
    }
    void GameOver()
    {

        resultScreen.SetActive(true);
        winningText.text = (HighestScoredPlayer().transform.name + " wins!");
        //Time.timeScale = 0;
    }
    public void ReloadScene()
    {
        //Time.timeScale = 1;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
