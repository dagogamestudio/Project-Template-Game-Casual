using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOptionSetting : MonoBehaviour
{
    [Header("UI")]
    public Button buttonOption;
    [SerializeField] private TextMeshProUGUI label;

    [Header("Color")]
    [SerializeField] private Color activeBg;
    [SerializeField] private Color inactiveBg;
    [SerializeField] private Color activeText;
    [SerializeField] private Color inactiveText;

    public void SetSelected(bool selected)
    {
        buttonOption.interactable = selected;
        buttonOption.image.color = selected ? activeBg : inactiveBg;
        label.color = selected ? activeText : inactiveText;
    }
}
