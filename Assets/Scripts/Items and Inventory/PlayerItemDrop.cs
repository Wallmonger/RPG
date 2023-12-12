using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLooseItems;
    [SerializeField] private float chanceToLooseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        // retrieving all equipments and stash

        List<InventoryItem> currentStash = inventory.GetStashList();
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList();

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToLoose = new List<InventoryItem>();

        // Drop item by chanceToLooseItems %
        foreach (InventoryItem item in currentEquipment) 
        { 
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        int unequipCount = itemsToUnequip.Count;
        for (int i = 0; i < unequipCount; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }

        foreach (InventoryItem item in currentStash)
        {
            if (Random.Range(0, 100) <= chanceToLooseMaterials)
            {
                DropItem(item.data);
                materialsToLoose.Add(item);
            }
        }

        int stashCount = materialsToLoose.Count;
        for (int i = 0; i < stashCount; i++)
        {
            inventory.RemoveItem(materialsToLoose[i].data);
        }
    }

}
