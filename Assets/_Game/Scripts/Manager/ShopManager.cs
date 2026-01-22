using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("Reference")]
    public GameObject panelShop;
    public Transform parentItem;
    public UIItemShop prefabItemUI;

    [Header("Data")]
    [SerializeField] private List<ItemShop> itemConfigs;

    private Dictionary<string, ItemShopSaveData> runtimeData =
        new Dictionary<string, ItemShopSaveData>();

    public event Action<string> OnSkinEquipped;

    private void InitRuntimeData()
    {
        foreach (var item in itemConfigs)
        {
            runtimeData[item.itemId] = new ItemShopSaveData
            {
                itemId = item.itemId,
                isUnlocked = item.isDefaultUnlocked,
                isUsed = false
            };
        }
        Debug.Log("Init");
    }

    public void SetUI()
    {
        if (runtimeData.Count == 0)
        {
            Debug.LogError("Shop runtime data not initialized!");
            return;
        }

        foreach (var config in itemConfigs)
        {
            var ui = Instantiate(prefabItemUI, parentItem);
            var saveData = runtimeData[config.itemId];

            ui.Setup(
                config,
                saveData,
                () => OnButtonItemClick(config)
            );
        }
    }

    private void RefreshUI()
    {
        foreach (Transform child in parentItem)
        {
            var ui = child.GetComponent<UIItemShop>();
            var saveData = runtimeData[ui.itemId];

            ui.SetUnlocked(saveData.isUnlocked);
            ui.SetSelected(saveData.isUsed);
        }
    }

    public void OnButtonItemClick(ItemShop itemShop)
    {

        //Jika sudah unlock coba pakai
        if (IsUnlocked(itemShop))
        {
            //Coba pakai, unUsed item lainnya
            Use(itemShop);
        }
        else
        {
            //Beli itemnya

            //Check jika uang cukup
            int playerMoney = GameManager.Instance.PlayerMoney;
            int itemPrice = itemShop.itemPrice;

            if (playerMoney >= itemPrice)
            {
                GameManager.Instance.PlayerMoney -= itemPrice;
                Unlock(itemShop);

                CoreManager.Instance.ShowNotif($"Buy Success, {itemShop.itemName} Unlocked");
            }
            else
            {
                CoreManager.Instance.ShowNotif("You don't have enough money");
            }
        }


        RefreshUI();
    }

    public bool IsUnlocked(ItemShop itemShop)
        => runtimeData[itemShop.itemId].isUnlocked;

    public bool IsUsed(ItemShop itemShop)
        => runtimeData[itemShop.itemId].isUsed;

    public void Unlock(ItemShop itemShop)
        => runtimeData[itemShop.itemId].isUnlocked = true;

    public void Use(ItemShop itemShop)
    {
        foreach (var data in runtimeData.Values)
            data.isUsed = false;

        runtimeData[itemShop.itemId].isUsed = true;


        OnSkinEquipped?.Invoke(itemShop.itemId);
    }

    public List<ItemShopSaveData> GetSaveData()
        => new List<ItemShopSaveData>(runtimeData.Values);

    public void LoadFromSave(List<ItemShopSaveData> save)
    {
        foreach (var data in save)
            if (runtimeData.ContainsKey(data.itemId))
                runtimeData[data.itemId] = data;
    }

    public void ApplySaveData(List<ItemShopSaveData> saveData)
    {
        runtimeData = new Dictionary<string, ItemShopSaveData>();

        foreach (var data in saveData)
        {
            runtimeData[data.itemId] = data;
        }

        if (parentItem.childCount > 0)
            RefreshUI();
    }

    public List<ItemShopSaveData> CreateDefaultSaveData()
    {
        List<ItemShopSaveData> list = new();

        foreach (var config in itemConfigs)
        {
            list.Add(new ItemShopSaveData
            {
                itemId = config.itemId,
                isUnlocked = config.isDefaultUnlocked,
                isUsed = config.isDefaultUnlocked
            });
        }

        return list;
    }
    public void EnsureRuntimeData()
    {
        if (runtimeData != null && runtimeData.Count > 0)
            return;

        runtimeData = new Dictionary<string, ItemShopSaveData>();

        var defaultList = CreateDefaultSaveData();

        foreach (var data in defaultList)
        {
            runtimeData[data.itemId] = data;
        }

        Debug.LogWarning("RuntimeData fallback created from default save");
    }

}

[System.Serializable]
public class ItemShopSaveData
{
    public string itemId;
    public bool isUnlocked;
    public bool isUsed;
}