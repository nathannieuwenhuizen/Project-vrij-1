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
    private GameObject CharacterPrefab;

    [SerializeField]
    private PlayerUI[] playerUis;
    [SerializeField]
    private GameObject pointerGroup;

    [SerializeField]
    private GameObject resultScreen;
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private Text winningText;

    private List<Character> characters;
    public void Start()
    {
        instance = this; 


        InitializePlayers(GameInformation.PLAYER_COUNT);
        Pause(false);
    }

    public static RoundManager instance;
    

    void InitializePlayers(int amount)
    {
        GameObject group = new GameObject("Players");
        //find existing players
        Character[] chars = FindObjectsOfType<Character>();
        characters = new List<Character>
        {
            chars[0],
            chars[1]
        };
        
        //spawn optional two additional players
        for (int i = 2; i < amount; i++)
        {
            playerUis[i].gameObject.SetActive(true);

            Character newCharacter = GameObject.Instantiate(CharacterPrefab, group.transform).GetComponent<Character>();

            characters.Add(newCharacter);
            Debug.Log("player: " + i);
        }


        //Setup controller setup and camera position
        for (int i = 0; i < characters.Count; i++)
        {
            //setting up name and grouping it
            characters[i].name = "player " + (1 + i);
            characters[i].transform.parent = group.transform;

            //ui setting
            characters[i].ui = playerUis[i];

            //setting up the controller for the player with the camera.
            characters[i].ApplyPlayerSetting(i + 1);

            pointerGroup.transform.GetChild(i).gameObject.SetActive(true);
            Debug.Log("player index: " + i);
            Rect camrect = characters[i].camera.rect;
            pointerGroup.transform.GetChild(i).GetComponent<RectTransform>().position =
                new Vector2(
                    (camrect.x + camrect.width / 2) * Screen.width,
                    (camrect.y + camrect.height / 1.5f) * Screen.height);
            //new Vector3(characters[i].camera.rect.width / 2f, characters[i].camera.rect.height / 2f);

        }
    }


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
    }

    /// <summary>
    /// Reloads the active scene, will maybe be put in a seperate scene manager class...
    /// </summary>
    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    /// <summary>
    /// Goes back to menu
    /// </summary>
    public void GoToMenu()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Pauses the game or resumes it.
    /// </summary>
    /// <param name="paused"></param>
    public void Pause(bool paused)
    {
        Time.timeScale = paused ? 0f : 1f;
        pauseScreen.SetActive(paused);
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
            if (winningChar.SavedPoints < chars[i].SavedPoints)
            {
                winningChar = chars[i];
            }
        }
        return winningChar;
    }

}
