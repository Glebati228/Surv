using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;


public class InventoryGUI : MonoBehaviour
{ 
#pragma warning disable 0649
    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private GameObject slotPrefab;

    [SerializeField]
    private Sprite emptyImage;
    [SerializeField]
    private GameObject takingText;

    [Space(3), Range(0f, 10f), SerializeField]
    private float maxRayCastDistance;

    [SerializeField]
    private LayerMask layerMask;

    [Space(3), Header("Inventory Grid")]
    [SerializeField] private float rowsize;
    [SerializeField] private Vector2 cellOffset;
    [SerializeField] private Vector2 margin;
    [SerializeField] private Vector2 border;

    private RectTransform rectTransform;
    private Inventory inventory;

    private Ray ray;
    private RaycastHit hit;
    private Camera cam;
    private Vector3 mousePosition;

#pragma warning disable 0649
    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        inventory.Init();
        rectTransform = inventoryPanel.GetComponent<RectTransform>();

        for (int i = 0; i < inventory.Inv.Length; ++i)
        {
            inventory.Inv[i].slotPrefab = Instantiate(slotPrefab, rectTransform) as GameObject;
            RectTransform rectTransform1 = inventory.Inv[i].slotPrefab.GetComponent<RectTransform>();
            rectTransform1.localPosition = Vector3.zero;
            inventory.Inv[i].slotPrefab.GetComponentInChildren<TextMeshProUGUI>().text = (inventory.Inv[i].count == 1 || inventory.Inv[i].count == 0) ? "" : inventory.Inv[i].count.ToString("n0");
            inventory.Inv[i].slotPrefab.GetComponent<Image>().sprite = (inventory.Inv[i].item == null) ? emptyImage : inventory.Inv[i].item.image;

            rectTransform1.localPosition = new Vector3()
            {
                x = ((i % rowsize) * cellOffset.x) + margin.x,
                y = -((int)(i / rowsize) * cellOffset.y) + margin.y,
                z = 0f,
            };
        }

        cam = Camera.main;
    }

    private void RayCheck()
    {
        ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        if (Physics.Raycast(ray, out hit, maxRayCastDistance, layerMask.value))
        {
            takingText.SetActive(true);
            DroppedItem states = hit.collider.GetComponent<DroppedItem>();
            if (Input.GetAxis("Use") == 1)
            {
                inventory.AddItem(states.id, states.count);
                Debug.Log("--------------------");
                inventory.DebugConsoleWrite();
                UpdateInventory();
                states.gameObject.SetActive(false);
            }
        }
        else
        {
            takingText.SetActive(false);
        }
    }

    private void UpdateInventory()
    {
        foreach (var item in inventory.Inv)
        {
            if(item.item != null)
            {
                item.slotPrefab.GetComponentInChildren<TextMeshProUGUI>().text = (item.count == 1 || item.count == 0) ? "" :item.count.ToString("n0");
                item.slotPrefab.GetComponent<Image>().sprite = item.item.image;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RayCheck();
    }
}
