using System.IO;
using Unity.Cinemachine;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerGO;
    [SerializeField] private CinemachineConfiner2D _confiner;
    [SerializeField] private InventoryManager _inventoryManager;

    private string _saveLocation;

    private void Start()
    {
        if (_playerGO == null)
            _playerGO = GameObject.FindGameObjectWithTag("Player");
        if (_confiner == null)
            _confiner = FindFirstObjectByType<CinemachineConfiner2D>();

        _saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new()
        {
            playerPosition = _playerGO.transform.position,
            mapBoundary = _confiner.BoundingShape2D.gameObject.name,
            inventorySaveData = _inventoryManager.GetInventoryItems()
        };

        File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
            _playerGO.transform.position = saveData.playerPosition;

            _confiner.BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<BoxCollider2D>();

            _inventoryManager.SetInventoryItems(saveData.inventorySaveData);
        }
        else
            SaveGame();
    }
}