using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private InventoryObject inventory;
    public InventoryObject Inv
    {
        get { return inventory; }
        set { inventory = value; }
    }

    [SerializeField] private GameObject emptySlot;
    [SerializeField] private RectTransform rect;
    [SerializeField] private int columnCount;
    [SerializeField] private int xOffset;
    [SerializeField] private int yOffset;
    private List<ItemSlot> emptySlots = new List<ItemSlot>();

#pragma warning restore 0649

    // Start is called before the first frame update
    void Start()
    {
        Inv.Init();
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < Inv.CAPACITY; i++)
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

            ItemSlot invItem = new ItemSlot();
            invItem.id = i;
            invItem.obj = obj;

            emptySlots.Add(invItem);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    public struct ItemSlot
    {
        public GameObject obj;
        public int id;
    }
}
