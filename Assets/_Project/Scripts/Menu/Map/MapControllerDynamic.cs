using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapControllerDynamic : MonoBehaviour
{
    public static MapControllerDynamic Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private RectTransform _mapParent;
    [SerializeField] private GameObject _areaPrefab;
    [SerializeField] private RectTransform _playerIcon;

    [Header("Colors")]
    [SerializeField] private Color _defaultColor = Color.grey; // Areas in map we're not currently in.
    [SerializeField] private Color _currentAreaColor = Color.green; // Active area Color

    [Header("Map Settings")]
    [SerializeField] private GameObject _mapBounds; // Parent of area colliders
    [SerializeField] private Collider2D _initialArea; // Starting area
    [SerializeField] private float _mapScale = 10f;

    private Collider2D[] _mapAreasArray;
    private Dictionary<string, RectTransform> _uiAreasDictionary = new(); // Map each collider to their corresponding RectTransform

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _mapAreasArray = _mapBounds.GetComponentsInChildren<Collider2D>();
    }

    // Generate Map
    public void GenerateMap(Collider2D newCurrentArea = null)
    {
        Collider2D currentArea = newCurrentArea != null ? newCurrentArea : _initialArea;

        ClearMap();

        foreach (Collider2D area in _mapAreasArray)
            CreateAreaUI(area, area == currentArea);

        MovePlayerIcon(currentArea.name);
    }

    public void UpdateCurrentArea(string newCurrentArea)
    {
        foreach (KeyValuePair<string, RectTransform> area in _uiAreasDictionary)
            area.Value.GetComponent<Image>().color = area.Key == newCurrentArea ? _currentAreaColor : _defaultColor;

        MovePlayerIcon(newCurrentArea);
    }

    private void ClearMap()
    {
        foreach (Transform child in _mapParent)
            Destroy(child.gameObject);

        _uiAreasDictionary.Clear();
    }

    private void CreateAreaUI(Collider2D area, bool isCurrentArea)
    {
        GameObject areaImage = Instantiate(_areaPrefab, _mapParent);
        RectTransform rectTransform = areaImage.GetComponent<RectTransform>();

        Bounds bounds = area.bounds;
        rectTransform.sizeDelta = new(bounds.size.x * _mapScale, bounds.size.y * _mapScale);
        rectTransform.anchoredPosition = bounds.center * _mapScale;

        areaImage.GetComponent<Image>().color = isCurrentArea ? _currentAreaColor : _defaultColor;
        _uiAreasDictionary[area.name] = rectTransform;
    }

    private void MovePlayerIcon(string newCurrentArea)
    {
        if (_uiAreasDictionary.TryGetValue(newCurrentArea, out RectTransform areaUI))
            _playerIcon.anchoredPosition = areaUI.anchoredPosition;
    }
}