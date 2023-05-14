using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IUsable
{
    public void UseItem();
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public InventorySlot[] inventorySlots;

    public GameObject inventoryItemPrefab;

    int selectedSlot = -1;

    private void Awake()
    {
        instance = this;
        ChangeSelectedSlot(0);
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    public void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public bool AddItem(Item item)
    {
        for (int i=0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnItem(item, slot);
                return true;
            }
        }
        return false;
    }

    public void SpawnItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public Item GetSelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;

            IUsable usable = itemInSlot.GetComponent<IUsable>();

            usable.UseItem();

            Destroy(itemInSlot.gameObject);

            return item;
        }
        return null;
    }
}
