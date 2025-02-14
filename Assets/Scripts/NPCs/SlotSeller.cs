using UnityEngine;

public class SlotSeller : Interactable
{
    [SerializeField] int price;

    private void Awake()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    public override void Interact()
    {
        if (inventoryController.coins > price)
        {
            inventoryController.SetCoins(-price);
            inventoryController.UpgradeInventory();
        }
    }
}
