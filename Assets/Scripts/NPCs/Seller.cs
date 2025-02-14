using UnityEngine;
using UnityEngine.InputSystem;

public class Seller : Interactable
{

    private void Awake()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    public override void Interact()
    {
        inventoryController.SellItems();
    }
}
