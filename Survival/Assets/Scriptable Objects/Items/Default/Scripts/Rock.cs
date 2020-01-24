using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default", menuName = "Inventory/Items/Default")]
public class Rock : ItemObject
{
    private void Awake()
    {
        type = ItemType.Default;
    }
}
