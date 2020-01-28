using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
#pragma warning disable 0649

    private Slot[] inventory;
    public Slot[] Inv
    {
        get { return inventory; }
    }

    [SerializeField]
    private int capacity;
    public int Capacity
    {
        get { return (capacity < 0) ? capacity = 0 : capacity; }
    }

#pragma warning restore 0649

    void Start()
    {
    }

    public void Init()
    {
        inventory = new Slot[capacity];
        for (int i = 0; i < capacity; i++)
        {
            inventory[i] = new Slot(null, 0);
        }
        AddItem(4, 2);
        AddItem(5, 2);
        AddItem(4, 6);
        AddItem(5, 3);
        DebugConsoleWrite();
    }

    public bool AddItem(int id, int count = 0)
    {
        if (id < 0 || id >= ItemDB.instance.Items.Count)
            return false;

        count = Mathf.Max(0, count);

        Item item = ItemDB.instance.GetItem(id) as Item;
        if (item is null)
            return false;

        StackleItem sItem = item as StackleItem;
        if (sItem is null)
        {
            foreach (var slot in inventory)
                if (slot.item == null)
                {
                    slot.item = item;
                    slot.count = 1;
                    return true;
                }
        }
        else
        {
            foreach (var slot in inventory)
            {
                if (slot.item == null)
                {
                    slot.item = sItem;
                    if (count > sItem.StackSize)
                    {
                        slot.count = sItem.StackSize;
                        AddItem(sItem, count - sItem.StackSize);
                        return false;
                    }
                    else if (count <= sItem.StackSize)
                    {
                        slot.count = count;
                        return true;
                    } 
                }
                else if (slot.item.Id == sItem.Id && slot.count < sItem.StackSize)
                {
                    int temp = slot.count + count;
                    slot.count = Mathf.Min(temp, sItem.StackSize);

                    if (temp > sItem.StackSize)
                    {
                        AddItem(sItem, temp - sItem.StackSize);
                        return false;
                    }

                    return true;
                }
            }
        }
        return false;
    }

    public bool AddItem(Item item, int count = 0)
    {
        if (item is null)
            return false;

        StackleItem sItem = item as StackleItem;
        if (sItem is null)
        {
            foreach (var slot in inventory)
                if (slot.item == null)
                {
                    slot.item = item;
                    slot.count = 1;
                    return true;
                }
        }
        else
        {
            foreach (var slot in inventory)
            {
                if (slot.item == null)
                {
                    slot.item = sItem;
                    if (count > sItem.StackSize)
                    {
                        slot.count = sItem.StackSize;
                        AddItem(sItem.Id, count - sItem.StackSize);
                        return false;
                    }
                    else if (count <= sItem.StackSize)
                    {
                        slot.count = count;
                        return true;
                    }
                }
                else if (slot.item.Id == sItem.Id && slot.count < sItem.StackSize)
                {
                    int temp = slot.count + count;
                    slot.count = Mathf.Min(temp, sItem.StackSize);

                    if (temp > sItem.StackSize)
                    {
                        AddItem(sItem.Id, temp - sItem.StackSize);
                        return false;
                    }

                    return true;
                }
            }
        }
        return false;
    }

    public bool DropItem(int slotID, int count = 0)
    {
        if (slotID < 0 || slotID >= inventory.Length)
            return false;

        count = Mathf.Max(0, count);

        if (slotID < inventory.Length)
        {
            inventory[slotID].count -= Mathf.Min(inventory[slotID].count, count);
            if (inventory[slotID].count <= 0)
                inventory[slotID].item = null;
        }

        return true;
    }

    public void DebugConsoleWrite()
    {
        foreach (var item in inventory)
        {
            Debug.Log((item.item == null) ? "none" : item.item.description + " " + item.count);
        }
    }
}
