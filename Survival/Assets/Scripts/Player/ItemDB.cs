using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDB : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private List<Item> items;
    public List<Item> Items 
    {
        get
        {
            if (items == null)
                items = new List<Item>();

            return items;
        }
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

    [System.Serializable]
    public class Item
    {
        public int id;
        public string description;
        public Sprite image;

        public Item(int id, string description, Sprite image)
        {
            this.id = id;
            this.description = description;
            this.image = image;
        }
    }
}
