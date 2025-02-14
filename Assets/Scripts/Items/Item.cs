using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("Basic Infos")]
    public string itemName;
    public Sprite itemImage;
    public string description;

    [Header("Sell Infos")]
    public bool canSell;
    public int value;

    public Interactable interactable;
}
