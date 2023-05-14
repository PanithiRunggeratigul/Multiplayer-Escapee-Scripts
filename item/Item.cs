using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/items")]
public class Item : ScriptableObject
{
    public Sprite image;
    public ItemType type;
    
    public enum ItemType
    {
        potion,
        bomb,
    }
}
