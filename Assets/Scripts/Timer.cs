using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timeText;
    private TraceGame traceGame;
    public float MaxTime = 60;
    private float timeRemaining;
    public bool TimerIsRunning = false;
    private void Start()
    {
        timeRemaining = MaxTime;
        timeText = GetComponent<TextMeshProUGUI>();
        traceGame = GameObject.Find("TraceGameCanvas").GetComponent<TraceGame>();
    }

    public void StartTimer()
    {
        gameObject.SetActive(true);
        TimerIsRunning = true;
    }

    void Update()
    {
        if (TimerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = MaxTime;
                TimerIsRunning = false;
                traceGame.StartRandomizedExperiment();
                gameObject.SetActive(false);
            }
        }
    }
    void DisplayTime(float timeToDisplay, float blinkStartTime = 10)
    {
        if (timeRemaining < blinkStartTime && (int)timeRemaining % 2 != 1)
            timeText.color = Color.red;
        else
            timeText.color = Color.white;

        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}