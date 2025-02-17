using UnityEngine;
using UnityEngine.UI;

public class CraftItem : MonoBehaviour
{
    [SerializeField] CraftingManager manager;
    public Item item;

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
        manager.OnMouseDownItem(item);
        Destroy(gameObject);
    }
}
