using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPotion : MonoBehaviour, IUsable
{
    public void UseItem()
    {
        PlayerController player = transform.parent.parent.GetComponentInParent<PlayerController>();

        player.AddBuff();
    }
}
