using UnityEngine;

public class CollectableItem : Interactable
{
    public Item item;

    private void Awake()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    public override void Interact()
    {
        if (inventoryController.items.Count == inventoryController.maxInventorySpace) return;
        inventoryController.AddItem(item);
        Destroy(gameObject);
    }
}
