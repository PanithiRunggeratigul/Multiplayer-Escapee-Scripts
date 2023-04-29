using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item
{
    public enum ItemType
    {
        dude,
        white,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.dude:
                return itemAssets.Instance.dudeSprite;
            case ItemType.white:
                return itemAssets.Instance.whiteSprite;
        }
    }
}
