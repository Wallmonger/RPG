using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {   // Naming gameObject automatically in unity view on load
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }
}
