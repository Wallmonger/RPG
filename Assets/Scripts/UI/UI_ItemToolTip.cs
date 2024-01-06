using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontSize = 32;

    void Start()
    {
        
    }

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null)
            return;

        itemNameText.fontSize = defaultFontSize;

        itemNameText.text = item.name;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();

        if (itemNameText.text.Length > 12)
            itemNameText.fontSize *= .7f;
    
        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);
}
