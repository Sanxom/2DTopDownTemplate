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
        if (!_menuPanel.activeSelf && PauseManager.IsGamePaused) return;

        _menuPanel.SetActive(true);
        PauseManager.SetPause(_menuPanel.activeSelf);
    }

    private void PlayerInputController_OnCloseMenuPerformed()
    {
        _menuPanel.SetActive(false);
        PauseManager.SetPause(_menuPanel.activeSelf);
    }
}