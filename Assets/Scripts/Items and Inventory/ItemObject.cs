using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{

    [SerializeField] private ItemData itemData;

    // Called when the script is loaded or value change in the inspector
    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = $"Item object - {itemData.itemName}";
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Debug.Log("Picked up item " + itemData.itemName);
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }  
    }

}
