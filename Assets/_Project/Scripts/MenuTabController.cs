using UnityEngine;
using UnityEngine.UI;

public class MenuTabController : MonoBehaviour
{
    [SerializeField] private Image[] _tabImages;
    [SerializeField] private Button[] _tabButtons;
    [SerializeField] private GameObject[] _pages;

    private void Awake()
    {
        ActivateTab(0); // TODO: Remove this if you want to return to the same page you left on instead of the first one every time
    }

    public void ActivateTab(int tabNumber)
    {
        for (int i = 0; i < _pages.Length; i++)
        {
            _pages[i].SetActive(false);
            _tabButtons[i].enabled = true;
            _tabImages[i].color = Color.grey;
        }

        _pages[tabNumber].SetActive(true);
        _tabButtons[tabNumber].enabled = false;
        _tabImages[tabNumber].color = Color.white;
    }
}