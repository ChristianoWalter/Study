using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    [Header("Inventory Slots")]
    InventoryController inventoryController;
    public List<Item> itemsInInventory;
    public GameObject itemPrefab;


    [Header("Crafting Variables")]
    public  Item currentItem;

    public Image customCursor;

    public CraftSlot[] slots;

    public List<Item> itemList;

    public Recipes[] recipes;

    public Item resultItem;

    public CraftSlot resultSlot;

    private void Awake()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    private void OnEnable()
    {
        itemsInInventory = inventoryController.items;
        for (int i = 0; i < itemsInInventory.Count; i++)
        {
            Instantiate(itemPrefab, slots[0].gameObject.transform).GetComponent<CraftItem>().item = itemsInInventory[i];
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (currentItem != null)
            {
                customCursor.gameObject.SetActive(false);
                CraftSlot nearestSlot = null;
                float shortestDistance = float.MaxValue;
                foreach (CraftSlot slot in slots) 
                { 
                    float dist = Vector2.Distance(Input.mousePosition, (slot.transform.position));
                    if (dist < shortestDistance) 
                    { 
                        shortestDistance = dist;
                        nearestSlot = slot;
                    }
                }

                if (nearestSlot != slots[0])
                {
                    nearestSlot.gameObject.SetActive(true);
                    nearestSlot.GetComponent<Image>().sprite = currentItem.itemImage;
                    nearestSlot.item = currentItem;
                    itemList[nearestSlot.index] = currentItem;
                }
                else
                {
                    Instantiate(itemPrefab, slots[0].gameObject.transform).GetComponent<CraftItem>().item = currentItem;
                }
                currentItem = null;

                CheckForCompletedRecipes();
            }
        }
    }

    private void CheckForCompletedRecipes()
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            var currentRecipe = itemList.Distinct().Where(item => item != null).ToDictionary(item => item, item => itemList.Count(item2 => item2 == item));
            var ingredients = recipes[i].ingredients;
            var recipeDict = ingredients.Distinct().Where(item => item != null).ToDictionary(item => item, item => ingredients.Count(item2 => item2 == item));

            var countEqual = currentRecipe.Count() == recipeDict.Count();

            var result = countEqual && recipeDict.All(item =>
            {
                if (item.Key == null)
                    return true;

                if (!currentRecipe.TryGetValue(item.Key, out var count))
                {
                    return false;
                }

                return count == item.Value;
            });

            if (result)
            {
                resultSlot.item = recipes[i].result;
                resultItem = recipes[i].result;
                resultSlot.gameObject.SetActive(true);
                resultSlot.GetComponent<Image>().sprite = recipes[i].result.itemImage;
                break;
            }
            else
            {
                resultItem = null;
                resultSlot.item = null;
                resultSlot.gameObject.SetActive(false);
            }
        } 
    }

    public void OnMouseDownItem(Item item)
    {
        if (currentItem == null) 
        {
            currentItem = item;
            customCursor.gameObject.SetActive(true);
            customCursor.sprite = currentItem.itemImage;
            for (int i = 0; i < itemsInInventory.Count; i++)
            {
                if (itemsInInventory[i] == item)
                {
                    itemsInInventory.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
