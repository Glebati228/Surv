using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemObject : ScriptableObject
{
    public int id;
    public GameObject prefab;

    public ItemType type;
    [TextArea(10, 20)]
    public string description;
}

public enum ItemType
{
    Default,
    Food,
    Weapon,
    Empty
}
