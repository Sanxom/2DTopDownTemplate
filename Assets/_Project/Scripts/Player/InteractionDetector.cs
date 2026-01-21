using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private GameObject _interactionIcon;

    private IInteractable _interactableInRange = null;

    private void Awake()
    {
        _interactionIcon.SetActive(false);
    }

    private void Start()
    {
        PlayerInputController.OnInteractActionPerformed += PlayerInputController_OnInteractActionPerformed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            _interactableInRange = interactable;
            _interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == _interactableInRange)
        {
            _interactableInRange = null;
            _interactionIcon.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        PlayerInputController.OnInteractActionPerformed -= PlayerInputController_OnInteractActionPerformed;
    }

    private void PlayerInputController_OnInteractActionPerformed()
    {
        if (_interactableInRange == null) return;

        _interactableInRange.Interact();
        if (!_interactableInRange.CanInteract())
            _interactionIcon.SetActive(false);
    }
}