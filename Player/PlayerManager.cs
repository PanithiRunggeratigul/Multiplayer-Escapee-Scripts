using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    GameObject controller;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    // instantiate player
    void CreateController()
    {
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity, 0, new object[] { PV.ViewID });
    }

    // destroy player and instantiate spectator camera
    public void Die()
    {
        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(controller);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerSpectatorCamera"), Vector3.zero, Quaternion.identity, 0, new object[] { PV.ViewID });
        }
    }
}
