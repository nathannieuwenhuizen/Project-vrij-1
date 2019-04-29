using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
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
        Debug.Log("test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
