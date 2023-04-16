using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Inventory
{
    private List<Items> itemList;
    public event EventHandler OnItemListChanged;

    public Inventory()
    {
        itemList = new List<Items>();

        //addItem(new Items { itemType = Items.ItemType.dude, amount = 1 });
        //addItem(new Items { itemType = Items.ItemType.white, amount = 1 });
    }

    public void addItem(Items item)
    {
        itemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Items> GetItemsList()
    {
        return itemList;
    }
}
