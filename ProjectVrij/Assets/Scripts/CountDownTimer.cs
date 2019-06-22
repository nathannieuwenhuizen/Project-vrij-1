using UnityEngine;
using UnityEngine.UI;
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
    [FMODUnity.EventRef] public string lastTenSeconds;

    private bool started;

    void Start()
    {
        instLastMinute = FMODUnity.RuntimeManager.CreateInstance(lastMinuteMusic);
        instLastMinute = FMODUnity.RuntimeManager.CreateInstance(gameMusic);
        instLastMinute.start();
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
            stopped = true;
            //fires the event
            OnZero?.Invoke();
        }
        else
        {
            Countdown();
        }
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
            FMODUnity.RuntimeManager.PlayOneShot(lastTenSeconds);
            _countDownText.color = alarmColor;
        }
        if(timerCount < 0.1f)
        {
            instLastMinute.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);    
        }
    }
}