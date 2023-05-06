using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpectatorOpenSettings : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject SettingMenu;
    [SerializeField] SpectatorGetInput GetInput;
    [SerializeField] PhotonView PV;

    bool pause_isopen;
    bool setting_isopen;

    private void Start()
    {
        pause_isopen = false;
        setting_isopen = false;
    }

    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PauseMenu.SetActive(true);
            GetInput.enabled = false;
        }
        else if (!pause_isopen && !setting_isopen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PauseMenu.SetActive(false);
            GetInput.enabled = true;
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
