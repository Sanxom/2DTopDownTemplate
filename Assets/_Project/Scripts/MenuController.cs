using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;

    private void Awake()
    {
        _menuPanel.SetActive(false);
    }

    private void Start()
    {
        PlayerInputController.OnOpenMenuPerformed += PlayerInputController_OnOpenMenuPerformed;
        PlayerInputController.OnCloseMenuPerformed += PlayerInputController_OnCloseMenuPerformed;
    }

    private void OnDestroy()
    {
        PlayerInputController.OnOpenMenuPerformed -= PlayerInputController_OnOpenMenuPerformed;
        PlayerInputController.OnCloseMenuPerformed -= PlayerInputController_OnCloseMenuPerformed;
    }

    private void PlayerInputController_OnOpenMenuPerformed()
    {
        _menuPanel.SetActive(true);
    }

    private void PlayerInputController_OnCloseMenuPerformed()
    {
        _menuPanel.SetActive(false);
    }
}