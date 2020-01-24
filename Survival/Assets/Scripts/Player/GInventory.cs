using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;

public class GInventory : MonoBehaviour
{
#pragma warning disable 0649
    private List<InventoryItem> items = new List<InventoryItem>();
    [SerializeField] private int CAPACITY;

    [SerializeField] private GameObject emptySlot;
    [SerializeField] private RectTransform rect;
    [SerializeField] private int columnCount;
    [SerializeField] private int xOffset;
    [SerializeField] private int yOffset;
    private EventSystem eventSystem;

#pragma warning disable 0649

    // Start is called before the first frame update
    void Start()
    {
        Init();
        AddItem(1, ItemDB.instance.Items[0], 1);
    }

    private void Init()
    {
        for (int i = 0; i < CAPACITY; i++)
        {
            GameObject obj = Instantiate(emptySlot, this.rect) as GameObject;

            RectTransform rect = obj.GetComponent<RectTransform>();

            rect.localPosition = new Vector3()
            {
                x = 40 + (i % columnCount) * xOffset,
                y = -40 + (-(i / columnCount) * yOffset),
                z = 0f
            };
            rect.localScale = Vector3.one;

            rect.name = i.ToString();

            InventoryItem item = new InventoryItem();
            item.obj = obj;
            items.Add(item);
        }
    }

    private void AddItem(int id, ItemDB.Item item, int count)
    {
        items[id].id = item.id;
        items[id].obj.GetComponent<Image>().sprite = item.image;
        items[id].count = count;

        if (items[id].count > 1 && item.id != 0)
        {
            items[id].obj.GetComponentInChildren<TextMeshProUGUI>().text = count.ToString("n0");
        }
        else
        {
            items[id].obj.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    private void AddInventoryItem(int id, InventoryItem item)
    {
        items[id].id = item.id;
        items[id].count = item.count;
        items[id].obj.GetComponentInChildren<Image>().sprite = ItemDB.instance.Items[item.id].image;

        if (items[id].count > 1 && item.id != 0)
        {
            items[id].obj.GetComponentInChildren<TextMeshProUGUI>().text = items[id].count.ToString("n0");
        }
        else
        {
            items[id].obj.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }



    // Update is called once per frame
    void Update()
    {

    }

    [System.Serializable]
    public class InventoryItem
    {
        public int id;
        public GameObject obj;
        public int count;
    }
}

