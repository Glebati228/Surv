using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//singleton
public class ItemDB : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private List<Item> items = new List<Item>();
    public List<Item> Items
    {
        get { return items; }
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

    public bool CreateItem(bool canStack, string description)
    {

        Item newItem = new Item(canStack, items.Count, description);
        items.Add(newItem);

        return true;
    }

    public bool DeleteItem(int id)
    {
        if (id < 0)
            return false;

        return items.Remove(items.FirstOrDefault(item => item.Id == id));
        //items.RemoveAll(item => item.Id == id);
    }

    public IClonable GetItem(int id)
    {
        if (id < 0)
            return null;

        return items[id].Clone();
    }
}

public interface IClonable
{
    IClonable Clone();
}

[System.Serializable]
public class Item : IClonable
{
    public bool Stackable { get; set; }
    public int Id { get; set; }
    public string description { get; set; }

    public Item(bool canStack, int id, string description)
    {
        this.Stackable = canStack;
        this.Id = id;
        this.description = description;
    }

    public IClonable Clone()
    {
        return new Item(Stackable, Id, description);
    }
}

[System.Serializable]
public class Slot
{
    public Item item { get; set; }
    public int count { get; set; }

    public Slot(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }
}