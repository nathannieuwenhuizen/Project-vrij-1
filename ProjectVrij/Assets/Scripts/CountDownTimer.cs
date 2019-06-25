using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Counts down a certain ammount and shows it via a UI text object. When it reaches 0, it fires an event.
/// </summary>
public class CountDownTimer : MonoBehaviour
{
    public delegate void Countaction();
    public static event Countaction OnZero;

    private Color alarmColor = new Color(100, 0, 0);

    [SerializeField] private Text _countDownText;
    public float timerCount;

    public bool paused = false;
    private bool stopped = false;

    FMOD.Studio.EventInstance instLastMinute;
    FMOD.Studio.EventInstance instGameMusic;
    [FMODUnity.EventRef] public string lastMinuteMusic;
    [FMODUnity.EventRef] public string gameMusic;

    public GameObject FirstObject;

    private bool started;
    private bool started1;

    void Start()
    {
        instLastMinute = FMODUnity.RuntimeManager.CreateInstance(lastMinuteMusic);
        instGameMusic = FMODUnity.RuntimeManager.CreateInstance(gameMusic);
        instGameMusic.start();
        instGameMusic.setParameterValue("MusicOn", 1.0f);
        Debug.Log("start music");
        Countdown();
    }

    void Update ()
    {
        if (paused || stopped) { return; }

        //When timer reaches 0.
        if (timerCount <= 0)
        {
            //Hier stopt de game.
            _countDownText.text = "00:00";
            GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(FirstObject, null);
            stopped = true;
            //fires the event
            OnZero?.Invoke();
        }
        else
        {
            Countdown();
        }
 }
    public void StopMusic()
    {
        instGameMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    /// <summary>
    /// Updates the timer and shows it on the UI object.
    /// </summary>
    private void Countdown()
    {
        timerCount -= Time.deltaTime;

        var minutes = timerCount / 60;
        var seconds = timerCount % 60;
        //var fraction = (timerCount * 100) % 100;

        _countDownText.text = string.Format("{0:0}:{1:00}", Mathf.Floor(minutes), seconds); 
        if(timerCount < 53)
        {
            if (!started)
            {
                instLastMinute.start();
                started = true;
                instGameMusic.setParameterValue("MusicOn", 0f);
                instGameMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                instLastMinute.setParameterValue("MusicOn", 1.0f);
            }
            
        }
        if (timerCount < 10)
        {
            _countDownText.color = alarmColor;
            started1 = true;


        }
        if(timerCount < 0.1f)
        {
            instLastMinute.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);    
        }

    }
}