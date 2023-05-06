using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpectatorCameraMovement : MonoBehaviour
{
    public GameObject[] Otherplayers;
    [SerializeField] GameObject gameover;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject SettingMenu;
    [SerializeField] SpectatorGetInput GetInput;
    [SerializeField] SpectatorOpenSettings openSettings;
    [SerializeField] GameObject ui;

    public int player_number = 0;
    public string player_name;

    public int prev_player_number = -1;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {

        }
        else if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            if (GetComponent<Camera>() != null)
            {
                Destroy(GetComponent<Camera>().gameObject);
            }
            Destroy(ui);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Otherplayers = GameObject.FindGameObjectsWithTag("Player");

        if (Otherplayers.Length == 0)
        {
            if (PV.IsMine)
            {
                gameover.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SettingMenu.SetActive(false);
                PauseMenu.SetActive(false);
                GetInput.enabled = false;
                openSettings.enabled = false;
                GameObject.Find("Canvas").transform.Find("Timer").GetComponent<timer>().enabled = false;
            }
        }
        else if (Otherplayers.Length > 0)
        {
            if (PV.IsMine)
            {
                try
                {
                    transform.position = Otherplayers[player_number].gameObject.transform.Find("orientation").transform.position;
                    transform.rotation = Otherplayers[player_number].gameObject.transform.Find("CameraHolder").transform.rotation;

                    Otherplayers[player_number].gameObject.transform.Find("NameCanvas").gameObject.SetActive(false);

                    if (prev_player_number != -1 && Otherplayers.Length > 1)
                    {
                        Otherplayers[prev_player_number].gameObject.transform.Find("NameCanvas").gameObject.SetActive(true);
                    }

                    player_name = Otherplayers[player_number].gameObject.GetComponent<PhotonView>().Owner.NickName;
                }
                catch (System.IndexOutOfRangeException e)
                {
                    print(e);
                }
            }
        }
    }
}
