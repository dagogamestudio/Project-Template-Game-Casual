using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIItemShop : MonoBehaviour
{
    public GameObject panelLocked;
    public GameObject panelUnlocked;

    public GameObject panelUnselected;
    public GameObject panelSelected;

    public TextMeshProUGUI textName;
    public Image imageIcon;
    public TextMeshProUGUI textPrice;


    [HideInInspector] public string itemId;

    public Button button;


    public void Setup(
        ItemShop config,
        ItemShopSaveData saveData,
        UnityAction onClick
    )
    {
        itemId = config.itemId;

        textName.text = config.itemName;
        imageIcon.sprite = config.itemIcon;
        textPrice.text = config.itemPrice.ToString();

        SetUnlocked(saveData.isUnlocked);
        SetSelected(saveData.isUsed);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(onClick);
    }

    public void SetUnlocked(bool value)
    {
        panelUnlocked.SetActive(value);
        panelLocked.SetActive(!value);
    }

    public void SetSelected(bool value)
    {
        panelSelected.SetActive(value);
        panelUnselected.SetActive(!value);
    }
}
