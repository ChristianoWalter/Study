using UnityEngine;

public class Keys : CollectableItem
{
    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public override void Interact()
    {
        if (inventoryController.items.Count == inventoryController.maxInventorySpace) return;
        inventoryController.AddItem(item);
        gameManager.CollectedKey();
        Destroy(gameObject);
    }
}
