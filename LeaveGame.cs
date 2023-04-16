using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LeaveGame : MonoBehaviour
{
    public void LeaveGameRoom()
    {
        PhotonNetwork.LoadLevel(0);
        PhotonNetwork.LeaveRoom();
        Destroy(GameObject.Find("RoomManager"));

        // MenuManager.Instance.OpenMenu("Loading");
    }
}
