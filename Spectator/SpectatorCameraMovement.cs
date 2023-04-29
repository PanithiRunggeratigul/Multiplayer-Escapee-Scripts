using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpectatorCameraMovement : MonoBehaviour
{
    public GameObject[] Otherplayers;

    public int player_number = 0;
    public string player_name;

    public int prev_player_number = 0;

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        Otherplayers = GameObject.FindGameObjectsWithTag("Player");

        if (Otherplayers.Length == 0)
        {
            // game over
        }
        else if (Otherplayers.Length > 0)
        {
            transform.position = Otherplayers[player_number].gameObject.transform.Find("orientation").transform.position;
            transform.rotation = Otherplayers[player_number].gameObject.transform.Find("CameraHolder").transform.rotation;


            if (PV.IsMine)
            {
                Otherplayers[player_number].gameObject.transform.Find("NameCanvas").gameObject.SetActive(false);
                Otherplayers[prev_player_number].gameObject.transform.Find("NameCanvas").gameObject.SetActive(true);
            }

            player_name = Otherplayers[player_number].gameObject.GetComponent<PhotonView>().Owner.NickName;
        }
    }
}
