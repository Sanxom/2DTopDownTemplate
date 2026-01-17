using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryManager _inventoryManager;

    private void Start()
    {
        _inventoryManager = FindFirstObjectByType<InventoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Item item))
        {
            // Add item to inventory
            bool itemAdded = _inventoryManager.AddItem(item.gameObject);

            if (itemAdded)
                Destroy(collision.gameObject);
        }
    }
}