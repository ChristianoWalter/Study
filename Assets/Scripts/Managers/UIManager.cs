using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public InventoryController inventoryController;

    public GameManager gameManager;

    public TextMeshProUGUI coinsValueTxt;

    public TextMeshProUGUI keysQuantityTxt;

    public List<Image> inventoryImages;

    public List<Image> selectedImages;

    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject inventorySlot;
    [SerializeField] GameObject selectedSlot;
    [SerializeField] GameObject visualSlot;

    [SerializeField] private Color selectedColor;

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

        gameManager = FindFirstObjectByType<GameManager>();

        inventoryController = FindFirstObjectByType<InventoryController>();
        inventoryController.OnInventoryChange += UpdateInventoryImages;

        inventoryController.OnCoinsChange += UpdateCoinsValue;

        inventoryController.OnChangeSelectedItem += UpdateSelectedSlot;

        gameManager.UpdateKeysQuantity += UpdateKeysQuantity;
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

    public void UpdateSelectedSlot(int _index)
    {
        for (int i = 0; i < selectedImages.Count; i++)
        {

            if (_index != i)
            {
                selectedImages[i].color = Color.clear;
            }
            else
            {
                selectedImages[i].color = selectedColor;
            }

        }
    }

    public static void AddSlot(Image slot, Image _selectedSlot)
    {
        Image newSlot = Instantiate(slot, instance.inventorySlot.transform);
        Instantiate(slot, instance.visualSlot.transform);
        newSlot.gameObject.SetActive(false);
        instance.inventoryImages.Add(newSlot);

        Image newSelectedSlot = Instantiate(_selectedSlot, instance.selectedSlot.transform);
        newSelectedSlot.color = Color.clear;
        instance.selectedImages.Add(newSelectedSlot);
    }

    public static void UpdateCoinsValue(int value)
    {
        if (instance == null) return;

        instance.coinsValueTxt.text = "Moedas: " + value.ToString();
    }

    public void UpdateKeysQuantity(int quantity)
    {
        keysQuantityTxt.text = "Chaves adquiridas: " + quantity.ToString();

        if (quantity == gameManager.keysNeeded)
        {
            victoryScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
