using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Inventory")]
public class InventoryObject : ScriptableObject
{
#pragma warning disable 0649
    [SerializeField] private List<InventoryItem> items;
    public int CAPACITY;
#pragma warning restore 0649

    public void Init()
    {
        items = new List<InventoryItem>();
        for (int i = 0; i < CAPACITY; i++)
        {
            items.Add(null);
        }
    }

    public bool AddItem(int index, ItemObject item)
    {
        if (index >= CAPACITY)
            return false;

        InventoryItem iitem = items[index];

        if (iitem.item.id == item.id)
        {
            iitem.count++;
            return true;
        }

         new InventoryItem(1, item);
        return true;
    }

    [System.Serializable]
    public class InventoryItem
    {
        public int count;
        public ItemObject item;

        public InventoryItem(int count, ItemObject item)
        {
            this.count = count;
            this.item = item;
        }
    } 
}

