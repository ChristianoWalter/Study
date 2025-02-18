using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CraftingManager : MonoBehaviour
{
    [Header("Inventory Slots")]
    InventoryController inventoryController;
    public List<GameObject> itemsInInventory;
    public GameObject itemPrefab;


    [Header("Crafting Variables")]
    public  Item currentItem;

    public Image customCursor;

    public CraftSlot[] slots;

    public List<Item> itemList;

    public List<GameObject> ingredientObjects;

    public Recipes[] recipes;

    public GameObject resultItem;

    public CraftSlot resultSlot;

    private void Awake()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < inventoryController.items.Count; i++)
        {
            GameObject item = Instantiate(itemPrefab, slots[0].gameObject.transform);
            item.GetComponent<CraftItem>().item = inventoryController.items[i];
            itemsInInventory.Add(item);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < itemsInInventory.Count; i++)
        {
            Destroy(itemsInInventory[i]);
            itemsInInventory.RemoveAt(i);
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
                    /*nearestSlot.gameObject.SetActive(true);
                    nearestSlot.GetComponent<Image>().sprite = currentItem.itemImage;
                    nearestSlot.item = currentItem;*/
                    GameObject _ingredientObject = Instantiate(itemPrefab, slots[nearestSlot.index].gameObject.transform);
                    _ingredientObject.GetComponent<CraftItem>().SetToCraftingTable(currentItem);
                    itemList[nearestSlot.index] = currentItem;
                    for (int i = 0; itemList.Count > i; i++)
                    {
                        if (itemList[i] == currentItem)
                        {
                            ingredientObjects[i] = _ingredientObject;
                        }
                    }
                }
                else
                {
                    GameObject item = Instantiate(itemPrefab, slots[0].gameObject.transform);
                    item.GetComponent<CraftItem>().item = currentItem;
                    itemsInInventory.Add(item);
                    inventoryController.AddItem(currentItem);
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
                /*resultSlot.item = recipes[i].result;
                resultItem = recipes[i].result;
                resultSlot.gameObject.SetActive(true);
                resultSlot.GetComponent<Image>().sprite = recipes[i].result.itemImage;*/
                resultItem = Instantiate(itemPrefab, resultSlot.gameObject.transform);
                resultItem.GetComponent<CraftItem>().SetToResult(recipes[i].result);
                break;
            }
            else
            {
                /*resultItem = null;
                resultSlot.item = null;
                resultSlot.gameObject.SetActive(false);*/
                Destroy(resultItem);
            }
        } 
    }

    public void OnMouseDownItem(Item item, ItemType itemType)
    {
        if (currentItem == null) 
        {
            currentItem = item;
            customCursor.gameObject.SetActive(true);
            customCursor.sprite = currentItem.itemImage;

            switch (itemType)
            {
                case ItemType.None:
                    inventoryController.RemoveItem(item);
                    break;
                case ItemType.Ingredient:
                    for (int i = 0; i < itemList.Count; i++)
                    {
                        if (itemList[i] == item)
                        {
                            itemList[i] = null;
                            CheckForCompletedRecipes();
                            break;
                        }
                    }
                    break;
                case ItemType.Result:
                    for (int i = 0; i < itemList.Count; i++)
                    {
                        itemList[i] = null;
                        Destroy(ingredientObjects[i]);
                    }
                    //resultItem = null;
                    break;
            }
        }
    }
}
