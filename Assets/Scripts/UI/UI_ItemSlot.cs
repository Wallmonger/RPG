using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Interface must be implemented
public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        // If item is created, remove transparency
        itemImage.color = Color.white;

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

    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }

    // Handle click events on ItemSlot objects
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        // If holding ctrl, delete item
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        // On click, equip
        if (item.data.itemType != ItemType.Material)
        {
            Debug.Log($"Equiped new item {item.data.itemName}");
            Inventory.instance.EquipItem(item.data);
        }
    }
}
