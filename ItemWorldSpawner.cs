using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ItemWorldSpawner : MonoBehaviour
{
    public Items item;
    [SerializeField] PhotonView PV;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {


        if (PhotonNetwork.IsMasterClient)
        {
            GameObject obj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "pfItemWorld"), transform.position, Quaternion.identity);

            int itemID = obj.GetComponent<PhotonView>().ViewID;

            PV.RPC("Change_Item", RpcTarget.All, itemID);
        }
    }

    [PunRPC]
    void Change_Item(int itemID)
    {
        GameObject itemWorld = PhotonView.Find(itemID).gameObject;

        ItemWorld itemW = itemWorld.GetComponent<ItemWorld>();
        itemW.item = this.item;

        SpriteRenderer spriteRenderer = itemW.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.GetSprite();
    }
}
