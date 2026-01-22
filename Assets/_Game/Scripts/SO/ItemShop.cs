using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Skin")]
public class ItemShop : ScriptableObject
{
    public string itemId;
    public string itemName;
    public int itemPrice;
    public Sprite itemIcon;
    public GameObject itemPrefab;

    public bool isDefaultUnlocked; // contoh: skin awal
}