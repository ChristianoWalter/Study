using UnityEngine;
using UnityEngine.SceneManagement;

public class Coins : Interactable
{
    [SerializeField] int coinValue;


    string _key;

    private void Awake()
    {
        _key = $"{SceneManager.GetActiveScene().name}:{gameObject.name}";

        if (PlayerPrefs.HasKey(_key))
        {
            Destroy(gameObject);
        }

        inventoryController = FindObjectOfType<InventoryController>();
    }

    public override void Interact()
    {
        inventoryController.SetCoins(coinValue);

        PlayerPrefs.SetString(_key, "");

        Destroy(gameObject);
    }
}
