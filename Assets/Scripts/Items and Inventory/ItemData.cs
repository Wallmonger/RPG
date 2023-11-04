using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}
//! menuName adds a create asset menu on right click
/*[CreateAssetMenu(fileName ="default file name on creation", menuName ="path to create the asset on unity editor")]*/
[CreateAssetMenu(fileName = "New Item Data", menuName="Data/Item")]


//! Scriptable Object can be created in editor and can store data (like prefabs but any data not just GameObject)
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
}
