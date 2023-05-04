using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeSurvived : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    timer time;

    // Start is called before the first frame update
    void Start()
    {
        time = GameObject.Find("Timer").GetComponent<timer>();

        float time_survived = 300 - time.timeLeft;

        float minutes = Mathf.FloorToInt(time_survived / 60);
        float seconds = Mathf.FloorToInt(time_survived % 60);

        text.text = "Time Survived: " + string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
