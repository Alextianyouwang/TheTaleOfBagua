using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PersistenceDataManager : MonoBehaviour
{
    private IDataPersistence[] _dataPersistenceObjects;
    private GameData _gameData;
    public bool EnableSaveLoad = true; 
    public static Func<SceneInfo[]> OnRequestSceneInfo;
    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneLoaded;
    }
 
    public void Start()
    {
        LoadGame();
    }

    void OnSceneLoaded(Scene scene, Scene current)
    {
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    private IDataPersistence[] FindAllDataPersistenceObjects()
    {
        IDataPersistence[] dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(false)
            .OfType<IDataPersistence>().ToArray();
        return dataPersistenceObjects;
    }

    public void NewGame()
    {
        if (!EnableSaveLoad)
            return;
        _gameData = null;
        LoadGame();
    }

    public void SaveGame()
    {
        if (!EnableSaveLoad)
            return;
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref _gameData);
        }
    }

    public void LoadGame()
    {
        if (!EnableSaveLoad)
            return;

        if (_gameData == null) 
        {
            _gameData = new GameData();
            _gameData.SetSceneInfos(OnRequestSceneInfo.Invoke());
        }
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(_gameData);
        }
      
    }

}
