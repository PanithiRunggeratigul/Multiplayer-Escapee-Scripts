using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Item item;

    public Image image;

    private void Start()
    {
        InitializeItem(item);
    }

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;

        // add script according to the item that picked up
        if (item.type == Item.ItemType.potion)
        {
            gameObject.AddComponent<ItemPotion>();
        }
        else
        {
            gameObject.AddComponent<ItemBomb>();
        }
    }
}
