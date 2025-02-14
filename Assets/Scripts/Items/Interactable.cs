using UnityEngine;

public enum PowerUps
{
    None,
    Healing,
    Speed,
    Force,
    IncrementHealth
}

public enum InteractableType
{
    Auto,
    Manual
}
public abstract class Interactable : MonoBehaviour
{
    public InventoryController inventoryController;
    public bool canInteract;
    public InteractableType type;

    public abstract void Interact();
}
