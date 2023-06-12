using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using static GlobalParameters;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Save File config")]
    [SerializeField] private string fileName;

    [Header("Debugging")]
    [SerializeField] private bool _initializeDataIfNull;
    [SerializeField] private bool _useDataPersistence;

    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;
    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
#if !UNITY_EDITOR
        _initializeDataIfNull = false;
        _useDataPersistence = true;
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void NewGame()
    {
        if (!_useDataPersistence) return;

        Debug.Log("New Game");
        this._gameData = new GameData();
        _dataHandler.Save(_gameData);
        LoadGame();
    }

    public void LoadGame()
    {
        if (!_useDataPersistence) return;

        Debug.Log("Load Game");
        _gameData = _dataHandler.Load();

        _dataPersistenceObjects = FindAllDataPersistenceObjects();

        if (_gameData == null)
        {
            if (_initializeDataIfNull)
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
        if (!_useDataPersistence) return;

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
}
