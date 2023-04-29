using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Inventory
{
    private List<Item> itemList;
    public event EventHandler OnItemListChanged;

    public Inventory()
    {
        itemList = new List<Item>();

        //addItem(new Items { itemType = Items.ItemType.dude, amount = 1 });
        //addItem(new Items { itemType = Items.ItemType.white, amount = 1 });
    }

    public void addItem(Item item)
    {
        itemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemsList()
    {
        return itemList;
    }
}
