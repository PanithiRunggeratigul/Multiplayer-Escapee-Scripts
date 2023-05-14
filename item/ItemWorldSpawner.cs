using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ItemWorldSpawner : MonoBehaviour
{
    public Item item;   // use with scriptable object
    [SerializeField] PhotonView PV;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)   // to instantiate object once
        {
            GameObject obj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "pfItemWorld"), transform.position, Quaternion.identity);

            // change the item respect to the scriptable object
            int itemID = obj.GetComponent<PhotonView>().ViewID;

            PV.RPC("Change_Item", RpcTarget.All, itemID);   // use RPC to tells all the player that item is changes
                                                            // if not use RPC, other player except master will not see the changed item
        }
    }

    [PunRPC]
    void Change_Item(int itemID)
    {
        GameObject itemWorld = PhotonView.Find(itemID).gameObject;

        ItemWorld itemW = itemWorld.GetComponent<ItemWorld>();
        itemW.item = this.item;

        SpriteRenderer spriteRenderer = itemW.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.image;
    }
}
