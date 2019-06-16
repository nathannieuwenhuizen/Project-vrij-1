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

    [SerializeField] private Text _countDownText;
    public float timerCount;

    void Update ()
    {
        //When timer reaches 0.
        if (timerCount <= 0)
        {
            //Hier stopt de game.
            _countDownText.text = "00:00";

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
    }
}