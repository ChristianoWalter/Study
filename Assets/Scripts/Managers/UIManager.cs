using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public InventoryController inventoryController;

    public TextMeshProUGUI coinsValueTxt;

    public List<Image> inventoryImages;

    [SerializeField] GameObject inventorySlot;
    [SerializeField] GameObject visualSlot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        inventoryController = FindObjectOfType<InventoryController>();
        inventoryController.OnInventoryChange += UpdateInventoryImages;

        inventoryController.OnCoinsChange += UpdateCoinsValue;
    }



    public static void UpdateInventoryImages(List<Item> items)//Item item)
    {
        if (instance == null) return;

        for (int i = 0; i < instance.inventoryImages.Count; i++) 
        {

            if (items.Count > i)
            {
                instance.inventoryImages[i].sprite = items[i].itemImage;
                instance.inventoryImages[i].gameObject.SetActive(true);
            }
            else
            {
                instance.inventoryImages[i].gameObject.SetActive(false);
            }
            
        }

    }

    public static void AddSlot(Image slot)
    {
        Image newSlot = Instantiate(slot, instance.inventorySlot.transform);
        Instantiate(slot, instance.visualSlot.transform);
        newSlot.gameObject.SetActive(false);
        instance.inventoryImages.Add(newSlot);
    }

    public static void UpdateCoinsValue(int value)
    {
        if (instance == null) return;

        instance.coinsValueTxt.text = "Moedas: " + value.ToString();
    }
}
