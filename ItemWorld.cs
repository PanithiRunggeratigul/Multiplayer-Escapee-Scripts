using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ItemWorld : MonoBehaviour
{
    public Items item;
    //private SpriteRenderer spriteRenderer;


    //private void Awake()
    //{
    //    spriteRenderer = GetComponent<SpriteRenderer>();
    //}

    //public static ItemWorld SpawnItemWorld(Vector3 position, Items item)
    //{
    //    // Transform transform = Instantiate(itemAssets.Instance.pfItemWorld, position, Quaternion.identity);
    //    GameObject obj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "pfItemWorld"), position, Quaternion.identity);
    //    ItemWorld itemWorld = obj.transform.GetComponent<ItemWorld>();
    //    itemWorld.SetItem(item);

    //    return itemWorld;
    //}

    //public void SetItem(Items item)
    //{
    //    this.item = item;
    //    spriteRenderer.sprite = item.GetSprite();
    //}

    public Items GetItem()
    {
        return item;
    }

    //public void RemoveItem()
    //{
    //    // Destroy(gameObject);
    //    gameObject.SetActive(false);
    //}
}
