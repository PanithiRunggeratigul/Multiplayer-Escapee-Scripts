using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    public float timeLeft;

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
                GameObject[] survivedplayers = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject player in survivedplayers)
                {
                    if (player.transform.Find("Canvas") != null)
                    {
                        player.transform.Find("Canvas").Find("GameOver").gameObject.SetActive(true);
                        player.transform.Find("Canvas").Find("Settings").gameObject.SetActive(false);
                        player.transform.Find("Canvas").Find("Pause").gameObject.SetActive(false);
                        player.transform.Find("Canvas").GetComponentInChildren<InventoryInput>().enabled = false;
                        player.GetComponent<PlayerController>().enabled = false;
                        player.layer = LayerMask.NameToLayer("Item");
                        player.transform.Find("CameraHolder").Find("Camera").GetComponent<CameraMovement>().enabled = false;
                        player.transform.Find("CameraHolder").Find("Camera").GetComponent<OpenSettings>().enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                }

                GameObject[] spectators = GameObject.FindGameObjectsWithTag("Spectator");
                foreach (GameObject spectator in spectators)
                {
                    if (spectator.transform.Find("Canvas") != null)
                    {
                        spectator.transform.Find("Canvas").Find("TimeOut").gameObject.SetActive(true);
                        spectator.transform.Find("Canvas").Find("Settings").gameObject.SetActive(false);
                        spectator.transform.Find("Canvas").Find("Pause").gameObject.SetActive(false);
                        spectator.transform.GetComponent<SpectatorCameraMovement>().enabled = false;
                        spectator.transform.GetComponent<SpectatorGetInput>().enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                }
            }
        }
    }
}
