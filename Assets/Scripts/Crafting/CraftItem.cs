using UnityEngine;
using UnityEngine.UI;
public enum ItemType
{
    None,
    Ingredient,
    Result
}

public class CraftItem : MonoBehaviour
{
    [SerializeField] CraftingManager manager;
    public Item item;
    public Image sprite;
    public ItemType type;

    private void Awake()
    {
        manager = FindObjectOfType<CraftingManager>();
    }

    private void Start()
    {
        gameObject.GetComponent<Image>().sprite = item.itemImage;
    }

    public void TakeItem()
    {
        manager.OnMouseDownItem(item, type);
        Destroy(gameObject);
    }

    public void SetToCraftingTable(Item _item)
    {
        item = _item;
        type = ItemType.Ingredient;
    }

    public void SetToResult(Item _item)
    {
        item = _item;
        type = ItemType.Result;
    }
}
