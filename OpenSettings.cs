using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSettings : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject SettingMenu;

    public bool pause_isopen;
    public bool setting_isopen;

    private void Start()
    {
        pause_isopen = false;
        setting_isopen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (setting_isopen && pause_isopen)
            {
                OpenSetting();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        if (!setting_isopen)
        {
            pause_isopen = !pause_isopen;
        }

        if (pause_isopen)
        {
            this.GetComponent<CameraMovement>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PauseMenu.SetActive(true);
        }
        else if (!pause_isopen && !setting_isopen)
        {
            this.GetComponent<CameraMovement>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PauseMenu.SetActive(false);
        }
    }

    public void OpenSetting()
    {
        setting_isopen = !setting_isopen;
        if (pause_isopen && setting_isopen)
        {
            SettingMenu.SetActive(true);
        }
        else if (pause_isopen && !setting_isopen)
        {
            SettingMenu.SetActive(false);
        }
    }
}
