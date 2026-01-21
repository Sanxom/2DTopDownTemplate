using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public string ChestID { get; private set; }
    public bool IsOpened { get; private set; }

    [SerializeField] private GameObject _droppedItemPrefab;
    [SerializeField] private SpriteRenderer _chestSR;
    [SerializeField] private Sprite _openedSprite;

    private const string CHEST_OPEN_SFX_NAME = "ChestOpen";

    private void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }

    public bool CanInteract() => !IsOpened;

    public void Interact()
    {
        if (!CanInteract()) return;

        OpenChest();
    }

    public void SetOpened(bool isOpened)
    {
        if (IsOpened = isOpened)
            _chestSR.sprite = _openedSprite;
    }

    private void OpenChest()
    {
        SetOpened(true);
        SoundEffectManager.Play(CHEST_OPEN_SFX_NAME);

        if (_droppedItemPrefab)
        {
            GameObject droppedItem = Instantiate(_droppedItemPrefab, transform.position + Vector3.down, Quaternion.identity);
            droppedItem.GetComponent<BounceEffect>().StartBounce();
        }
    }
}