using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public List<Item> items = new();
    public int maxInventorySpace;
    public int coins;
    public Image inventorySlot;

    public event Action<List<Item>> OnInventoryChange;
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
    }

    public void SetItem(Item item)
    {
        if (items.Count == maxInventorySpace) return;
        items.Add(item);
        OnInventoryChange.Invoke(items);
        //UIManager.UpdateInventoryImages();
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
        UIManager.AddSlot(inventorySlot);
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
}
