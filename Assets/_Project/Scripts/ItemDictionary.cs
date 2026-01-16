using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;

    private Dictionary<int, GameObject> _itemDictionary;

    private void Awake()
    {
        _itemDictionary = new();

        // Auto-increment IDs
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].itemID = i + 1;
            }
        }

        foreach (Item item in itemPrefabs)
        {
            _itemDictionary[item.itemID] = item.gameObject;
        }
    }

    public GameObject GetItemPrefab(int itemID)
    {
        if (_itemDictionary.TryGetValue(itemID, out GameObject itemPrefab))
            return itemPrefab;
        else
            Debug.LogWarning($"Item with {itemID} not found in the Dictionary!");

        return null;
    }
}