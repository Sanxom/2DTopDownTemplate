using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    [SerializeField] private GameObject _hotbarPanel;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private int _slotCount = 10; // Numbers 1-0
    [SerializeField] private ItemDictionary _itemDictionary;

    private void Start()
    {
        PlayerInputController.OnHotbarSlot1Performed += PlayerInputController_OnHotbarSlot1Performed;
        PlayerInputController.OnHotbarSlot2Performed += PlayerInputController_OnHotbarSlot2Performed;
        PlayerInputController.OnHotbarSlot3Performed += PlayerInputController_OnHotbarSlot3Performed;
        PlayerInputController.OnHotbarSlot4Performed += PlayerInputController_OnHotbarSlot4Performed;
        PlayerInputController.OnHotbarSlot5Performed += PlayerInputController_OnHotbarSlot5Performed;
        PlayerInputController.OnHotbarSlot6Performed += PlayerInputController_OnHotbarSlot6Performed;
        PlayerInputController.OnHotbarSlot7Performed += PlayerInputController_OnHotbarSlot7Performed;
        PlayerInputController.OnHotbarSlot8Performed += PlayerInputController_OnHotbarSlot8Performed;
        PlayerInputController.OnHotbarSlot9Performed += PlayerInputController_OnHotbarSlot9Performed;
        PlayerInputController.OnHotbarSlot10Performed += PlayerInputController_OnHotbarSlot10Performed;
    }

    private void OnDisable()
    {
        PlayerInputController.OnHotbarSlot1Performed -= PlayerInputController_OnHotbarSlot1Performed;
        PlayerInputController.OnHotbarSlot2Performed -= PlayerInputController_OnHotbarSlot2Performed;
        PlayerInputController.OnHotbarSlot3Performed -= PlayerInputController_OnHotbarSlot3Performed;
        PlayerInputController.OnHotbarSlot4Performed -= PlayerInputController_OnHotbarSlot4Performed;
        PlayerInputController.OnHotbarSlot5Performed -= PlayerInputController_OnHotbarSlot5Performed;
        PlayerInputController.OnHotbarSlot6Performed -= PlayerInputController_OnHotbarSlot6Performed;
        PlayerInputController.OnHotbarSlot7Performed -= PlayerInputController_OnHotbarSlot7Performed;
        PlayerInputController.OnHotbarSlot8Performed -= PlayerInputController_OnHotbarSlot8Performed;
        PlayerInputController.OnHotbarSlot9Performed -= PlayerInputController_OnHotbarSlot9Performed;
        PlayerInputController.OnHotbarSlot10Performed -= PlayerInputController_OnHotbarSlot10Performed;
    }

    public List<InventorySaveData> GetHotbarItems()
    {
        List<InventorySaveData> hotbarSaveData = new();

        foreach (Transform slotTransform in _hotbarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                hotbarSaveData.Add(new InventorySaveData
                {
                    itemID = item.itemID,
                    slotIndex = slotTransform.GetSiblingIndex(),
                });
            }
        }
        return hotbarSaveData;
    }

    public void SetHotbarItems(List<InventorySaveData> hotbarSaveDataList)
    {
        // Clear inventory panel first
        foreach (Transform child in _hotbarPanel.transform)
            Destroy(child.gameObject);

        // Create new slots
        for (int i = 0; i < _slotCount; i++)
        {
            Instantiate(_slotPrefab, _hotbarPanel.transform);
        }

        // Populate slots with saved items
        foreach (InventorySaveData data in hotbarSaveDataList)
        {
            if (data.slotIndex < _slotCount)
            {
                Slot slot = _hotbarPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
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

    private void UseItemInSlot(int hotBarSlotNumber)
    {
        Slot slot = _hotbarPanel.transform.GetChild(hotBarSlotNumber).GetComponent<Slot>();
        if (slot.currentItem != null)
        {
            Item item = slot.currentItem.GetComponent<Item>();
            item.UseItem();
        }
    }

    private void PlayerInputController_OnHotbarSlot10Performed()
    {
        UseItemInSlot(9);
    }

    private void PlayerInputController_OnHotbarSlot9Performed()
    {
        UseItemInSlot(8);
    }

    private void PlayerInputController_OnHotbarSlot8Performed()
    {
        UseItemInSlot(7);
    }

    private void PlayerInputController_OnHotbarSlot7Performed()
    {
        UseItemInSlot(6);
    }

    private void PlayerInputController_OnHotbarSlot6Performed()
    {
        UseItemInSlot(5);
    }

    private void PlayerInputController_OnHotbarSlot5Performed()
    {
        UseItemInSlot(4);
    }

    private void PlayerInputController_OnHotbarSlot4Performed()
    {
        UseItemInSlot(3);
    }

    private void PlayerInputController_OnHotbarSlot3Performed()
    {
        UseItemInSlot(2);
    }

    private void PlayerInputController_OnHotbarSlot2Performed()
    {
        UseItemInSlot(1);
    }

    private void PlayerInputController_OnHotbarSlot1Performed()
    {
        UseItemInSlot(0);
    }
}