using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    float timeLeft;

    bool ReadyTimerOn = false;
    bool GameTimerOn = false;

    public TextMeshProUGUI time;

    // Start is called before the first frame update
    void Start()
    {
        ReadyTimerOn = true;
        SetStartTimer();
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        time.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    void SetStartTimer()
    {
        timeLeft = 10;
    }

    void SetGameTimer()
    {
        timeLeft = 300;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (ReadyTimerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                updateTimer(timeLeft);
            }
            else if (timeLeft <= 0)
            {
                SetGameTimer();
                ReadyTimerOn = false;
                GameTimerOn = true;
            }
        }

        if (GameTimerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                updateTimer(timeLeft);
            }
            else
            {
                GameTimerOn = false;
                Application.Quit();
            }
        }
    }
}
