using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Inventory/Items/Food")]
public class Food : ItemObject
{
    public float healthRestore;

    private void Awake()
    {
        type = ItemType.Food;
    }
}
