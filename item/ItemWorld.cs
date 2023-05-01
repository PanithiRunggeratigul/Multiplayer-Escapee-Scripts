using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ItemWorld : MonoBehaviour
{
    public Item item;
    public Item GetItem()
    {
        return item;
    }
}
