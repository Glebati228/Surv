using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class InventoryGUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private GameObject slotPrefab;

    [SerializeField]
    private Sprite emptyImage;

    [Space(3), Range(0f, 10f)]
    private float maxRayCastDistance;
    private LayerMask layerMask;

    private RectTransform rectTransform;
    private Inventory inventory;
    private GameObject[] slotPrefabs;

    private Ray ray;
    private RaycastHit hit;
    private Camera cam;
#pragma warning disable 0649
    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        inventory.Init();
        rectTransform = inventoryPanel.GetComponent<RectTransform>();
        slotPrefabs = new GameObject[inventory.Inv.Length];

        for (int i = 0; i < inventory.Inv.Length; ++i)
        {
            slotPrefabs[i] = Instantiate(slotPrefab, rectTransform) as GameObject;
            slotPrefabs[i].GetComponent<RectTransform>().localPosition = Vector3.zero;
            slotPrefabs[i].GetComponentInChildren<TextMeshProUGUI>().text = (inventory.Inv[i].count == 1) ? "" : inventory.Inv[i].count.ToString("n0");
            slotPrefabs[i].GetComponent<Image>().sprite = (inventory.Inv[i].item == null) ? emptyImage : inventory.Inv[i].item.image;
        }

        cam = Camera.main;
    }

    private void RayCheck()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, maxRayCastDistance, layerMask.value))
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
