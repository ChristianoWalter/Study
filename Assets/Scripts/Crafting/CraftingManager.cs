using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    private Item currentItem;

    public Image customCursor;

    public CraftSlot[] slots;

    public List<Item> itemList;

    public Recipes[] recipes;

    public Item result;

    public CraftSlot resultSlot;

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
                nearestSlot.gameObject.SetActive(true);
                nearestSlot.GetComponent<Image>().sprite = currentItem.itemImage;
                nearestSlot.item = currentItem;

                itemList[nearestSlot.index] = currentItem;
                currentItem = null;

                CheckForCompletedRecipes();
            }
        }
    }

    private void CheckForCompletedRecipes()
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            var intersect = itemList.Intersect(recipes[i].ingredients);

            if (intersect.Count() == itemList.Count && intersect.Count() == recipes[i].ingredients.Count)
            {
                resultSlot.item = recipes[i].result;
                result = recipes[i].result;
                resultSlot.gameObject.SetActive(true);
                resultSlot.GetComponent<Image>().sprite = recipes[i].result.itemImage;
                break;
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
    
        }
    }
}
