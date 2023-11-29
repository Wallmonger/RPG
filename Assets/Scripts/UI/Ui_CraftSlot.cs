using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ui_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // Transform type for CanCraft function
        ItemData_Equipment craftData = item.data as ItemData_Equipment;

        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
    }
}
