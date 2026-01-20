using System;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int itemID;
    public string itemName;

    public virtual void Pickup()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;
        if (ItemPickupUIController.Instance != null)
            ItemPickupUIController.Instance.ShowItemPickup(itemName, itemIcon);
    }

    public virtual void UseItem()
    {
        print($"Using item: {itemName}");
    }
}