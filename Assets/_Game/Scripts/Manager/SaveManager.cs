using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public DataGame dataNewGame;
    private string savePath;

    //C:\Users\DagoEng Game Studio\AppData\LocalLow\DefaultCompany\Project Template Casual Game
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            savePath = Path.Combine(Application.persistentDataPath, "save.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize()
    {
        if (File.Exists(savePath))
            LoadGame();
        else
            CreateNewGame();
    }

    public void SaveGame()
    {
        if (GameManager.Instance == null) return;

        DataGame data = new DataGame
        {
            playerMoney = GameManager.Instance.PlayerMoney,
            playerLevel = GameManager.Instance.playerLevel,
            shopItems = ShopManager.Instance.GetSaveData()
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log(savePath);
    }

    private void LoadGame()
    {
        string json = File.ReadAllText(savePath);
        DataGame data = JsonUtility.FromJson<DataGame>(json);

        ApplyData(data);
    }
    private void ApplyData(DataGame data)
    {
        GameManager.Instance.PlayerMoney = data.playerMoney;
        GameManager.Instance.playerLevel = data.playerLevel;

        ShopManager.Instance.ApplySaveData(data.shopItems);


        ShopManager.Instance.EnsureRuntimeData();
    }

    private void CreateNewGame()
    {
        DataGame newData = new DataGame
        {
            playerMoney = dataNewGame.playerMoney,
            playerLevel = dataNewGame.playerLevel,
            shopItems = ShopManager.Instance.CreateDefaultSaveData()
        };

        string json = JsonUtility.ToJson(newData, true);
        File.WriteAllText(savePath, json);

        ApplyData(newData);
    }

    public void ResetData()
    {
        File.Delete(savePath);
        CreateNewGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveGame();
    }

    private void OnApplicationQuit() => SaveGame();
}


[Serializable]
public class DataGame
{
    public int playerMoney;
    public int playerLevel;

    public List<ItemShopSaveData> shopItems;
}

[Serializable]
public class ItemDataSave
{
    public string itemId;
    public bool isUnlocked;
    public bool isUsed;
}