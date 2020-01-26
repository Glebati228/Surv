using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        inventory = new Slot[capacity];
        for (int i = 0; i < capacity; i++)
        {
            inventory[i] = new Slot(null, 0);
        }

        ItemDB.instance.CreateItem(true, "rock");
        ItemDB.instance.CreateItem(false, "grass");
        ItemDB.instance.CreateItem(true, "stick");

        AddItem(0);
        AddItem(0);

        AddItem(1);
        AddItem(1);

        AddItem(50);

        DebugConsoleWrite();
    }

    public bool AddItem(int id)
    {
        if (id < 0 || id >= ItemDB.instance.Items.Count)
            return false;

        Item item = ItemDB.instance.GetItem(id) as Item;
        if (item is null)
            return false;

        foreach (var slot in inventory)
        {
            if (slot.item == null)
            {
                slot.item = item;
                slot.count = 1;
                return true;
            }
            else if (slot.item.Id == item.Id && item.Stackable)
            {
                slot.count++;
                return true;
            }
            //Debug.Log($"slot.item hash: {slot.item.GetHashCode()}   item hash: {item.GetHashCode()}");
        }
        return false;
    }

    public bool AddItem(Item item)
    {
        if (item is null)
            return false;

        foreach (var slot in inventory)
        {
            if (slot.item == null)
            {
                slot.item = item;
                slot.count = 1;
                return true;
            }
            else if (slot.item == item && slot.item.Stackable)
            {
                slot.count++;
                return true;
            }
        }

        return false;
    }

    public bool DropItem(int slotID)
    {
        if (slotID < 0 || slotID >= inventory.Length)
            return false;

        if(slotID < inventory.Length)
        {
            inventory[slotID].item = null;
            inventory[slotID].count = 0;
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
