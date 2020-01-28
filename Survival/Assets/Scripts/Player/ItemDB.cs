using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

//singleton
public class ItemDB : MonoBehaviour
{
#pragma warning disable 0649

    //all items
    [SerializeField] private List<Item> items = new List<Item>();
    public List<Item> Items
    {
        get { return items; }
    }

    //stackle type items
    [Space]
    [SerializeField] private List<StackleItem> stackleItems = new List<StackleItem>();
    public List<StackleItem> StackleItems
    {
        get { return stackleItems; }
    }

    public static ItemDB instance;
#pragma warning restore 0649

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance == this)
            Destroy(gameObject);
    }

    void Start()
    {
        //ItemDB.instance.CreateItem("rock", image);
        //ItemDB.instance.CreateStackleItem(5, "grass", image);
        //ItemDB.instance.CreateStackleItem(2, "stick", image);
        //items.AddRange(stackleItems);
        for (int i = 0; i < stackleItems.Count; ++i)
        {
            CreateStackleItem(stackleItems[i].StackSize, stackleItems[i].description, stackleItems[i].image, stackleItems[i].prefab, i);
        }
    }

    public bool CreateItem(string description, Sprite image, GameObject prefab)
    {

        Item newItem = new Item(items.Count, description, image, prefab);
        items.Add(newItem);

        return true;
    }

    public bool CreateStackleItem(int size, string description, Sprite image, GameObject prefab)
    {
        if (size == 1 || size == 0)
        {
            return CreateItem(description, image, prefab);
        }
        StackleItem item = new StackleItem(size, StackleItems.Count + 1000, description, image, prefab);
        items.Add(item);
        return true;
    }

    public bool CreateStackleItem(int size, string description, Sprite image, GameObject prefab, int id)
    {
        if (size == 1 || size == 0)
        {
            return CreateItem(description, image, prefab);
        }
        StackleItem item = new StackleItem(size, id + 1000, description, image, prefab);
        items.Add(item);
        return true;
    }

    public bool DeleteItem(int id)
    {
        if (id < 0)
            return false;

        return items.Remove(items.FirstOrDefault(item => item.Id == id));
        //items.RemoveAll(item => item.Id == id);
    }

    public IClonable GetItem(int index)
    {
        if (index < 0)
            return null;

        return items[index].Clone();
    }
}

public interface IClonable
{
    IClonable Clone();
}

public interface IStackable
{
    int StackSize { get; }
}

[System.Serializable]
public class Item : IClonable
{
    public int Id;
    public string description;
    public Sprite image;
    public GameObject prefab;

    public Item(int id, string description)
    {
        this.Id = id;
        this.description = description;
    }

    public Item(int id, string description, Sprite image, GameObject prefab) : this(id, description)
    {
        this.image = image;
        this.prefab = prefab;
    }

    public virtual IClonable Clone()
    {
        return new Item(Id, description, image, prefab);
    }
}

[System.Serializable]
public class StackleItem : Item, IStackable
{
    [SerializeField] private int stackSize;
    public int StackSize
    {
        get { return stackSize; }
    }

    public StackleItem(int StackSize, int id, string description, Sprite image, GameObject prefab) : base(id, description, image, prefab)
    {
        this.stackSize = StackSize;
    }

    public override IClonable Clone()
    {
        return new StackleItem(StackSize, Id, description, image, prefab);
    }
}

[System.Serializable]
public class Slot
{
    public Item item;
    public int count;

    public Slot(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }
}