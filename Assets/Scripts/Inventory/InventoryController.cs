using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public List<Item> items = new();
    public int maxInventorySpace;
    public int coins;
    public Image inventorySlot;
    public Image selectedSlot;
    private int equippedItemIndex;

    public event Action<List<Item>> OnInventoryChange;
    public event Action<int> OnChangeSelectedItem;
    public event Action<int> OnCoinsChange;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Currency"))
        {
            PlayerPrefs.SetInt("Currency", 0);
        }

        
    }

    private void Start()
    {
        SetCoins(PlayerPrefs.GetInt("Currency"));
        //CreateInventory();
    }

    public void AddItem(Item item)
    {
        if (items.Count == maxInventorySpace) return;
        items.Add(item);
        UpdateEquippedItem(0);
        OnInventoryChange.Invoke(items);
        //UIManager.UpdateInventoryImages();
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        OnInventoryChange.Invoke(items);
    }

    public void SetCoins(int value)
    {
        coins = coins + value;
        OnCoinsChange.Invoke(coins);
        PlayerPrefs.SetInt("Currency", coins);
    }

    public void UpgradeInventory()
    {
        maxInventorySpace++;
        UIManager.AddSlot(inventorySlot, selectedSlot);
    }

    public void CreateInventory()
    {
        for(int i = 0; i < maxInventorySpace; i++)
        {
            UIManager.AddSlot(inventorySlot, selectedSlot);
        }
    }

    public void UpdateEquippedItem(int amount)
    {
        if (items == null) return;
        if (items.Count == 1)
        {
            equippedItemIndex = 0;
        }
        else
        {
            equippedItemIndex += amount;
            if (equippedItemIndex > items.Count - 1)
            {
                equippedItemIndex = 0;
            }
            else if (equippedItemIndex < 0)
            {
                equippedItemIndex = items.Count - 1;
            }
        }
        PlayerController.instance.UpdateEquippedItem(items[equippedItemIndex]);
        OnChangeSelectedItem.Invoke(equippedItemIndex);
    }

    public void UpdateUI()
    {

    }

    public void SellItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].canSell)
            {
                SetCoins(items[i].value);
                items.RemoveAt(i);
            }
        }
        OnInventoryChange.Invoke(items);
        //UIManager.UpdateInventoryImages();
    }

    public void ChangeSelectedItemInput(InputAction.CallbackContext context)
    {
        if (context.performed) UpdateEquippedItem(((int)context.ReadValue<float>()));
    }
}
