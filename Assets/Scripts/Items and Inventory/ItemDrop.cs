using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    // Number of objects enemy can drop
    [SerializeField] private int possibleItemDrop;

    // Loot table
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>(); 

    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private ItemData item;

    public void GenerateDrop()
    {
        // Generating the list
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
                dropList.Add(possibleDrop[i]);
        }

        // Enemy doesn't drop anything, return
        if (dropList.Count <= 0)
            return;

        for (int i = 0; i < possibleItemDrop; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
            /*dropList.Remove(randomItem);*/
            DropItem(randomItem);
        }
    }

    public void DropItem(ItemData _itemData)
    {
        // Generate item 
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}