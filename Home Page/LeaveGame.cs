using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LeaveGame : MonoBehaviour
{
    [SerializeField] PhotonView PV;

    public void LeaveGameRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("DestroyRoomManager", RpcTarget.All);
        }
        else
        {
            PhotonNetwork.LoadLevel(0);
            PhotonNetwork.LeaveRoom();
            Destroy(GameObject.Find("RoomManager"));
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        // MenuManager.Instance.OpenMenu("Loading");
    }

    [PunRPC]
    void DestroyRoomManager()
    {
        PhotonNetwork.LoadLevel(0);
        PhotonNetwork.LeaveRoom();
        Destroy(GameObject.Find("RoomManager"));
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
