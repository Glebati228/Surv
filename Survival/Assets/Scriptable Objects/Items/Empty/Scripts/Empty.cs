using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Empty", menuName = "Inventory/Items/Empty")]
public class Empty : ItemObject
{
    private void Awake()
    {
        type = ItemType.Empty;
    }
}
