using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    [SerializeField] private Transform skinRoot;
    [SerializeField] private List<ItemShop> skinConfigs;

    private GameObject currentSkin;

    private Dictionary<string, ItemShop> skinMap;

    private void Awake()
    {
        Instance = this;

        skinMap = new Dictionary<string, ItemShop>();
        foreach (var skin in skinConfigs)
            skinMap[skin.itemId] = skin;
    }

    public void Initialize()
    {
        ShopManager.Instance.OnSkinEquipped += ApplySkin;
    }

    private void OnDestroy()
    {
        ShopManager.Instance.OnSkinEquipped -= ApplySkin;
    }

    public void ApplySkin(string itemId)
    {
        if (!skinMap.ContainsKey(itemId))
        {
            Debug.LogError($"Skin {itemId} not found");
            return;
        }

        if (currentSkin != null)
            Destroy(currentSkin);

        currentSkin = Instantiate(
            skinMap[itemId].itemPrefab,
            skinRoot
        );
    }
}
