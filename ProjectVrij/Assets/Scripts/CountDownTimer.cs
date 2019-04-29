using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDownTimer : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction OnZero;

    [SerializeField] private Text _countDownText;
    public float timerCount;

    void Update ()
    {
        if (timerCount <= 0)
        {
            //Hier stopt de game.
            //Time.timeScale = 0;
            _countDownText.text = "0:00:00";

            OnZero?.Invoke();
        }

        else
        {
            Countdown();
        }
 }

    private void Countdown()
    {
        timerCount -= Time.deltaTime;

        var minutes = timerCount / 60;
        var seconds = timerCount % 60;
        var fraction = (timerCount * 100) % 100;

        _countDownText.text = string.Format("{0:0}:{1:00}:{2:00}", Mathf.Floor(minutes), seconds, fraction); 
    }
}