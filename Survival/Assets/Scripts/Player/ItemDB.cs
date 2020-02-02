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

        Load();
    }

    public void Load()
    {
        Debug.Log("Loading resources");
        for (int i = 0; i < stackleItems.Count; ++i)
        {
            CreateStackleItem(stackleItems[i].StackSize, stackleItems[i].description, i, stackleItems[i].name);
        }

        items.ForEach(item => item.LoadResources());
    }

    public bool CreateItem(string description, string name)
    {

        Item newItem = new Item(name, items.Count, description);
        items.Add(newItem);

        return true;
    }

    public bool CreateStackleItem(int size, string name, string description)
    {
        if (size == 1 || size == 0)
        {
            return CreateItem(description, name);
        }
        StackleItem item = new StackleItem(size, name, StackleItems.Count + 1000, description);
        items.Add(item);
        return true;
    }

    public bool CreateStackleItem(int size, string description, int i, string name)
    {
        if (size == 1 || size == 0)
        {
            return CreateItem(description, name);
        }
        StackleItem item = new StackleItem(size, name, 1000 + i, description);
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
        if (index < 0 || index >= items.Count)
            return null;

        return items[index].Clone();
    }

    public IClonable GetItemById(int id)
    {
        if (id < 0)
            return null;

        return items.Find(item => item.Id == id).Clone();
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
    public string name;
    public int Id;
    public string description;
    public Sprite image;
    public GameObject prefab;

    public Item(string name, int id, string description)
    {
        this.name = name;
        this.Id = id;
        this.description = description;
    }

    public void LoadResources()
    {
        //Texture2D img = Resources.Load<Texture2D>("Sprites/" + name);
        //Sprite imgSprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.zero);
        this.image = Resources.Load<Sprite>("Sprites/" + name);

        this.prefab = Resources.Load<GameObject>("Items/" + name);
    }

    public Item(string name, int id, string description, Sprite sprite, GameObject prefab) : this(name, id, description)
    {
        this.image = sprite;
        this.prefab = prefab;
    }

    public virtual IClonable Clone()
    {
        return new Item(name, Id, description, image, prefab);
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

    public StackleItem(int StackSize, string name, int id, string description) : base(name, id, description)
    {
        this.stackSize = StackSize;
    }

    public StackleItem(int StackSze, string name, int id, string description, Sprite sprite, GameObject prefab) : base(name, id, description, sprite, prefab)
    {
        this.stackSize = StackSize;
    }

    public override IClonable Clone()
    {
        return new StackleItem(StackSize, name, Id, description, image, prefab);
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