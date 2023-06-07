using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Save File config")]
    [SerializeField] private string fileName;

    [Header("Debugging")]
    [SerializeField] private bool InitializeDataIfNull;

    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;
    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
    #if !UNITY_EDITOR
        InitializeDataIfNull = false;
    #endif

        Initialize();
        _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void NewGame()
    {
        Debug.Log("New Game");
        this._gameData = new GameData();
        _dataHandler.Save(_gameData);
    }

    public void LoadGame()
    {
        Debug.Log("Load Game");
        _gameData = _dataHandler.Load();

        _dataPersistenceObjects = FindAllDataPersistenceObjects();

        if (_gameData == null)
        {
            if (InitializeDataIfNull)
            {
                Debug.LogWarning("No saved data was found. A new save file will be created");
                NewGame();
            }
            else
            {
                Debug.LogWarning("No saved data was found. A new game needs to be started");
                return;
            }
        }

        foreach(IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        Debug.Log("Save Game");
        _dataPersistenceObjects = FindAllDataPersistenceObjects();

        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref _gameData);
        }

        _dataHandler.Save(_gameData);
    }

    public bool HasGameData()
    {
        return _gameData != null;
    }

    //private void OnApplicationQuit()
    //{
    //    SaveGame();
    //}
}
