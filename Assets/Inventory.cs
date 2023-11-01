using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventoryItems;

    // Like a list but with Key/Value pair 
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    // Prevent duplicate between scenes
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();    
    }

    public void AddItem(ItemData _item)
    {
        // If key already exist in dictionnary, add one more else, add it to inventoryItems list and dictionnary (ItemData, InventoryItem)
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            // Only one stack of item left or less
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);   // ItemData is the key, so it will delete InventoryItem as well
            }
            // More than one item left, remove 1
            else
                value.RemoveStack();
            
        }

    }


    #region TEST ITEM REMOVAL
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ItemData newItem = inventoryItems[inventoryItems.Count - 1].data;
            RemoveItem(newItem);
            Debug.Log("Item ejected");
        }
    }*/

    #endregion
}
