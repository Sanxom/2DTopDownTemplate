using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float _minDropDistance = 2f;
    [SerializeField] private float _maxDropDistance = 3f;

    private Transform _originalParent;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalParent = transform.parent;
        transform.SetParent(transform.root);
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // Follows Mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();
        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }

        Slot originalSlot = _originalParent.GetComponent<Slot>();

        if (dropSlot != null)
        {
            // Dropping on a valid slot
            if (dropSlot.currentItem != null)
            {
                // Slot has an item already
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                // Slot has no item
                originalSlot.currentItem = null;
            }

            // Move item into Drop Slot
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            // Dropping off of a slot
            if (!IsWithinInventory(eventData.position))
            {
                // If not dropping on inventory
                // Drop in world
                DropItem(originalSlot);
            }
            else
            {
                // Ending drag while over inventory UI
                // Snap back to original slot
                transform.SetParent(_originalParent);
            }
        }

        _rectTransform.anchoredPosition = Vector2.zero;
    }

    private bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = _originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    private void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null;

        // Find Player
        Transform playerTransform = FindFirstObjectByType<Player>().transform;

        // Random drop position around player
        Vector2 dropOffset = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(_minDropDistance, _maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

        // Instantiate drop item
        GameObject droppedItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
        droppedItem.GetComponent<BounceEffect>().StartBounce();

        // Destroy the UI one
        Destroy(gameObject);
    }

}