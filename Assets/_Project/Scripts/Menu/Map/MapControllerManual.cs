using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapControllerManual : MonoBehaviour
{
    public static MapControllerManual Instance { get; private set; }

    [SerializeField] private GameObject _mapParent;
    [SerializeField] private Color _highlightColor = Color.yellow;
    [SerializeField] private Color _dimmedColor = new(1f, 1f, 1f, 0.5f);
    [SerializeField] private RectTransform _playerIconTransform;


    private List<Image> _mapImages;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _mapImages = _mapParent.GetComponentsInChildren<Image>().ToList();
    }

    public void HighlightArea(string areaName)
    {
        foreach (Image area in _mapImages)
            area.color = _dimmedColor;

        Image currentArea = _mapImages.Find(x => x.name == areaName);

        if (currentArea != null)
        {
            currentArea.color = _highlightColor;
            _playerIconTransform.position = currentArea.GetComponent<RectTransform>().position;
        }
        else
            Debug.LogWarning($"Area not found: {areaName}");
    }
}