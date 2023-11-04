using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UI_ItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        if (item != null)
        {
            // Setting image in itemSlot
            itemImage.sprite = item.data.icon;

            // Getting amount of stack as a text in textMeshPro if bigger than 1
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }

            else
            {
                itemText.text = "";
            }
        }
    }
}
