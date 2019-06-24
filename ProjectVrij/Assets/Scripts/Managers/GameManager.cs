using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// The round manager keeps track on the timer and who wins at the end of each round.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Character prefabs")]
    [SerializeField]
    private GameObject[] CharacterPrefabs;

    [Space]
    [Header("ui")]
    [SerializeField]
    private PlayerUI[] playerUis;
    [SerializeField]
    private GameObject pointerGroup;
    [SerializeField]
    private GameObject horizontalLine;
    [SerializeField]
    private GameObject blackCornerImage;

    [SerializeField]
    private GameObject resultScreen;
    [SerializeField]
    private GameObject pauseScreen;

    [SerializeField]
    private GameObject countDownObject;
    [SerializeField]
    private Image countDownImage;
    [SerializeField]
    private Text countDownText;
    [SerializeField]
    private float countDownDurationInSeconds = 1f;

    [SerializeField] 
    private CountDownTimer timer;
    [SerializeField]
    private GameObject timerObject;

    [SerializeField]
    private GameObject playerUIs;

    [FMODUnity.EventRef] public string beginGame;
    [FMODUnity.EventRef] public string countDownSound;
    FMOD.Studio.EventInstance instBeginGame;
    FMOD.Studio.EventInstance instCountDownSound;

    private List<Character> characters;
    public void Start()
    {
        instance = this; 

        InitializePlayers(GameInformation.PLAYER_COUNT);
        Pause(false);
        CountDowStart();
        Application.targetFrameRate = 60;

        instCountDownSound = FMODUnity.RuntimeManager.CreateInstance(countDownSound);
        instBeginGame = FMODUnity.RuntimeManager.CreateInstance(beginGame);
        instCountDownSound.start();
        instBeginGame.start();
    }

    public static GameManager instance;
    

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
        if (GameInformation.FROM_MENU)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].ui.gameObject.SetActive(false);
                Destroy(characters[i]);
                characters[i] = null;
            }
            characters = new List<Character> { };


            if (amount > 2)
            {
                horizontalLine.SetActive(true);
                if (amount < 4)
                {
                    blackCornerImage.SetActive(true);
                }
            }

            characters = new List<Character> { };
            //spawn all the players based on the settings
            for (int i = 0; i < amount; i++)
            {
                int uiIndex = 0;
                if (amount == 2)
                {
                    uiIndex = i;
                }
                else
                {
                    if (i < 2)
                    {
                        uiIndex = i + 2;
                    } else
                    {
                        uiIndex = i - 2;
                    }
                }

                playerUis[uiIndex].gameObject.SetActive(true);

                Character newCharacter = GameObject.Instantiate(CharacterPrefabs[GameInformation.CHOSEN_CHARACTERS[i]], group.transform).GetComponent<Character>();
                characters.Add(newCharacter);

                newCharacter.ui = playerUis[uiIndex];
            }

        } else
        {
            //spawn optional two additional players
            for (int i = 2; i < amount; i++)
            {
                //playerUis[i].gameObject.SetActive(true);

                //Character newCharacter = GameObject.Instantiate(CharacterPrefabs[0], group.transform).GetComponent<Character>();
                //characters.Add(newCharacter);
            }
        }

        //Setup controller setup and camera position
        for (int i = 0; i < characters.Count; i++)
        {
            //setting up name and grouping it
            characters[i].name = "player " + (1 + i);
            characters[i].transform.parent = group.transform;


            //setting up the controller for the player with the camera.
            characters[i].ApplyPlayerSetting(i + 1);

            //purely for testing
            //characters[0].ApplyPlayerSetting(0);
            pointerGroup.transform.GetChild(i).gameObject.SetActive(true);
            Debug.Log("player index: " + i);
            Rect camrect = characters[i].camera.rect;
            pointerGroup.transform.GetChild(i).GetComponent<RectTransform>().position =
                new Vector2(
                    (camrect.x + camrect.width / 2) * Screen.width,
                    (camrect.y + camrect.height / 1.5f) * Screen.height);
            //new Vector3(characters[i].camera.rect.width / 2f, characters[i].camera.rect.height / 2f);

        }
        Transform.FindObjectOfType<PlayerSpawner>().PositionAllPlayers(characters);

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
        foreach (Character character in characters)
        {
            character.GetComponent<InputHandler>().enabled = false;
            character.Walking(0, 0);
        }
        countDownObject.SetActive(true);
        countDownText.text = "Time's up!";

        playerUIs.SetActive(false);
        timerObject.SetActive(false);
        pointerGroup.SetActive(false);

        StartCoroutine(GameOvering());
    }
    IEnumerator GameOvering()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (Character character in characters)
        {
            character.CameraFadeToBlack();
        }
        yield return new WaitForSeconds(0.5f);

        foreach (Character character in characters)
        {
            character.SetupCameraForResultScreen();
        }
        countDownText.text = "Counting points...";

        int countScore = 0;
        while (countScore <= HighestScoredPlayer().SavedPoints)
        {
            foreach (Character character in characters)
            {
                if (character.AllPoints >= countScore)
                {
                    character.UpdateResultScoreText(countScore);
                }
            }
            yield return new WaitForSeconds(1f / HighestScoredPlayer().AllPoints);

            countScore++;
        }
        countDownText.text = (HighestScoredPlayer().transform.name + " wins!");
        resultScreen.SetActive(true);

        foreach (Character character in characters)
        {
            character.Result(character == HighestScoredPlayer());
        }
    }
    /// <summary>
    /// Reloads the active scene, will maybe be put in a seperate scene manager class...
    /// </summary>
    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        Time.timeScale = 1;
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
            if (winningChar.AllPoints < chars[i].AllPoints)
            {
                winningChar = chars[i];
            }
        }
        return winningChar;
    }

    public void CountDowStart()
    {
        countDownObject.SetActive(true);
        timer.paused = true;
        foreach(Character character in characters)
        {
            character.GetComponent<InputHandler>().CanOnlyMoveCamera = true;
        }
        StartCoroutine(CountDowning(3));
    }
    IEnumerator CountDowning(int number)
    {
        countDownText.text = number.ToString();
        float index = 0;
        while(index < countDownDurationInSeconds)
        {
            countDownImage.fillAmount = (countDownDurationInSeconds - index) / countDownDurationInSeconds;
            index += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        countDownImage.fillAmount = 0;
        if (number <= 1)
        {
            StartCoroutine(CountDownEnd());
        } else
        {
            StartCoroutine(CountDowning(number - 1));
        }
    }
    IEnumerator CountDownEnd()
    {
        countDownText.text = "go!";
        timer.paused = false;

        foreach (Character character in characters)
        {
            character.GetComponent<InputHandler>().CanOnlyMoveCamera = false;
        }
        yield return new WaitForSeconds(countDownDurationInSeconds);
        countDownObject.SetActive(false);
        instCountDownSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

    }
}
