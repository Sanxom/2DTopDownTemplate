using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerGO;
    [SerializeField] private CinemachineConfiner2D _confiner;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private HotbarManager _hotbarManager;
    [SerializeField] private Chest[] _chestArray;

    private string _saveLocation;

    private void Start()
    {
        InitializeComponents();
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new()
        {
            playerPosition = _playerGO.transform.position,
            mapBoundary = _confiner.BoundingShape2D.gameObject.name,
            inventorySaveData = _inventoryManager.GetInventoryItems(),
            hotbarSaveData = _hotbarManager.GetHotbarItems(),
            chestSaveData = GetChestStates(),
        };

        File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));

            LoadPlayerPosition(saveData);
            LoadMap(saveData);
            LoadInventoryFromSave(saveData);
            LoadChestStates(saveData.chestSaveData);
        }
        else
        {
            SaveGame();
            SetupInitialInventory();
            MapControllerDynamic.Instance?.GenerateMap();
        }
    }

    public void DeleteSaveFile()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, "saveData.json")))
            File.Delete(Path.Combine(Application.persistentDataPath, "saveData.json"));
    }

    private void InitializeComponents()
    {
        _saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        if (_playerGO == null)
            _playerGO = GameObject.FindGameObjectWithTag("Player");
        if (_confiner == null)
            _confiner = FindFirstObjectByType<CinemachineConfiner2D>();
        if (_chestArray.Length == 0)
            _chestArray = FindObjectsByType<Chest>(FindObjectsSortMode.None);
    }

    private List<ChestSaveData> GetChestStates()
    {
        List<ChestSaveData> chestStates = new();

        foreach (Chest chest in _chestArray)
        {
            ChestSaveData chestSaveData = new()
            {
                chestID = chest.ChestID,
                isOpened = chest.IsOpened,
            };

            chestStates.Add(chestSaveData);
        }

        return chestStates;
    }

    private void LoadChestStates(List<ChestSaveData> chestStates)
    {
        foreach (Chest chest in _chestArray)
        {
            ChestSaveData chestSaveData = chestStates.FirstOrDefault(c => c.chestID == chest.ChestID);

            if (chestSaveData != null)
                chest.SetOpened(chestSaveData.isOpened);
        }
    }

    private void LoadPlayerPosition(SaveData saveData)
    {
        _playerGO.transform.position = saveData.playerPosition;
    }

    private void LoadMap(SaveData saveData)
    {
        Collider2D savedMapBoundary = GameObject.Find(saveData.mapBoundary).GetComponent<Collider2D>();
        _confiner.BoundingShape2D = savedMapBoundary;

        MapControllerManual.Instance?.HighlightArea(saveData.mapBoundary);
        MapControllerDynamic.Instance?.GenerateMap(savedMapBoundary);
    }

    private void LoadInventoryFromSave(SaveData saveData)
    {
        _inventoryManager.SetInventoryItems(saveData.inventorySaveData);
        _hotbarManager.SetHotbarItems(saveData.hotbarSaveData);
    }

    private void SetupInitialInventory()
    {
        _inventoryManager.SetInventoryItems(new List<InventorySaveData>());
        _hotbarManager.SetHotbarItems(new List<InventorySaveData>());
    }
}