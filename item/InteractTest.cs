using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InteractTest : MonoBehaviourPun, IInteractable
{
    [SerializeField] PhotonView PV;

    // Networked object can be destroy only if:
    //  1. the object destroyed by the owner
    //  2. the object destroyed by the master

    // use with IInteractable interface
    public void Interact(int viewID)
    {
        PV.RPC("Remove_Item", RpcTarget.MasterClient, viewID);  // ask MasterClient to remove the item
        
        // another way to handle this is use request ownership of the object before destroy the object
    }

    [PunRPC]
    void Remove_Item(int viewID)
    {
        PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
    }
}
