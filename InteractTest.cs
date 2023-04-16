using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InteractTest : MonoBehaviour, IInteractable
{
    [SerializeField] PhotonView PV;

    public void Interact(int viewID)
    {
        PV.RPC("Remove_Item", RpcTarget.MasterClient, viewID);
    }

    [PunRPC]
    void Remove_Item(int viewID)
    {
        PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
    }
}
