using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickupUIController : MonoBehaviour
{
    public static ItemPickupUIController Instance { get; private set; }

    [SerializeField] private GameObject _popupPrefab;
    [SerializeField] private float _popupDuration = 3f;
    [SerializeField] private int _maxPopups = 5;

    private readonly Queue<GameObject> _activePopupsQueue = new();
    private WaitForSeconds _popupWaitTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _popupWaitTime = new WaitForSeconds(_popupDuration);
    }

    public void ShowItemPickup(string itemName, Sprite itemIcon)
    {
        GameObject newPopup = Instantiate(_popupPrefab, transform);
        newPopup.GetComponentInChildren<TMP_Text>().text = itemName;

        Image itemImage = newPopup.transform.Find("Item Icon")?.GetComponent<Image>();
        if (itemImage)
            itemImage.sprite = itemIcon;

        _activePopupsQueue.Enqueue(newPopup);
        if (_activePopupsQueue.Count > _maxPopups)
            Destroy(_activePopupsQueue.Dequeue());

        StartCoroutine(FadeOutAndDestroyCoroutine(newPopup));
    }

    private IEnumerator FadeOutAndDestroyCoroutine(GameObject popupGO)
    {
        yield return _popupWaitTime;

        if (popupGO == null)
            yield break;

        CanvasGroup canvasGroup = popupGO.GetComponent<CanvasGroup>();
        for (float timePassed = 0f; timePassed < 1f; timePassed += Time.deltaTime)
        {
            if (popupGO == null)
                yield break;

            canvasGroup.alpha = 1f - timePassed;
            yield return null;
        }

        Destroy(popupGO);
    }
}