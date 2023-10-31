using UnityEngine;


//! menuName adds a create asset menu on right click
/*[CreateAssetMenu(fileName ="default file name on creation", menuName ="path to create the asset on unity editor")]*/
[CreateAssetMenu(fileName = "Something", menuName="Data/Item")]


//! Scriptable Object can be created in editor and can store data (like prefabs but any data not just GameObject)
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}
