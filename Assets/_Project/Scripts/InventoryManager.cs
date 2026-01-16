using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private int _slotCount;
    [SerializeField] private GameObject[] _itemPrefabs;
    [SerializeField] private ItemDictionary _itemDictionary;

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> inventorySaveData = new();

        foreach (Transform slotTransform in _inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                inventorySaveData.Add(new InventorySaveData
                {
                    itemID = item.itemID,
                    slotIndex = slotTransform.GetSiblingIndex(),
                });
            }
        }
        return inventorySaveData;
    }

    public void SetInventoryItems(List<InventorySaveData> inventorySaveDataList)
    {
        // Clear inventory panel first
        foreach (Transform child in _inventoryPanel.transform)
            Destroy(child.gameObject);

        // Create new slots
        for (int i = 0; i < _slotCount; i++)
        {
            Instantiate(_slotPrefab, _inventoryPanel.transform);
        }

        // Populate slots with saved items
        foreach (InventorySaveData data in inventorySaveDataList)
        {
            if (data.slotIndex < _slotCount)
            {
                Slot slot = _inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = _itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }
}