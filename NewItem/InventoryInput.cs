using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] InventoryManager inventoryManager;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // InventoryManager.instance.GetSelectedItem();
            inventoryManager.GetSelectedItem();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // InventoryManager.instance.ChangeSelectedSlot(0);
            inventoryManager.ChangeSelectedSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // InventoryManager.instance.ChangeSelectedSlot(1);
            inventoryManager.ChangeSelectedSlot(1);
        }
    }
}
