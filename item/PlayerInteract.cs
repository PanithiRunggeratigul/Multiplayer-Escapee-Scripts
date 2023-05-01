using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
interface IInteractable {
    public void Interact(int viewID);
}

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] Transform interactSource;
    [SerializeField] float interactDistance;
    [SerializeField] LayerMask item;
    private DisplayDescription description;
    // private Inventory inventory;
    [SerializeField] InventoryManager inventoryManager;

    private void Start()
    {
        // inventory = new Inventory();
        // uiinventory.setInventory(inventory);
    }

    // Update is called once per frame
    void Update()
    {
        Ray r = new Ray(interactSource.position, interactSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitinfo, interactDistance, item))
        {
            description = hitinfo.collider.GetComponent<DisplayDescription>();

            if (description != null)
            {
                description.IsFacing();
                if (hitinfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        ItemWorld itemWorld = hitinfo.collider.GetComponent<ItemWorld>();
                        if (itemWorld != null && inventoryManager.AddItem(itemWorld.GetItem()))
                        {
                            int viewID = hitinfo.collider.GetComponent<PhotonView>().ViewID;
                            interactObj.Interact(viewID);
                        }
                    }
                }
            }
        }
        else if (!Physics.Raycast(r, out RaycastHit nothit, interactDistance, item) && description != null)
        {
            description.IsNotFacing();
        }
    }
}
