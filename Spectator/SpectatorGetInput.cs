using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpectatorGetInput : MonoBehaviour
{
    [SerializeField] SpectatorCameraMovement cam;
    [SerializeField] PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        cam.prev_player_number = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            cam.player_number += 1;
            cam.prev_player_number = cam.player_number - 1;

            if (cam.player_number >= cam.Otherplayers.Length)
            {
                cam.player_number = 0;
            }
            if (cam.prev_player_number < 0)
            {
                cam.prev_player_number = cam.Otherplayers.Length - 1;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            cam.player_number -= 1;
            cam.prev_player_number = cam.player_number + 1;

            if (cam.player_number < 0)
            {
                cam.player_number = cam.Otherplayers.Length-1;
            }
            if (cam.prev_player_number >= cam.Otherplayers.Length)
            {
                cam.prev_player_number = 0;
            }
        }
    }
}
