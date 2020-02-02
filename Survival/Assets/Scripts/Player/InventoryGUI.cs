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
    private GameObject[] slotPrefabs;

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
        slotPrefabs = new GameObject[inventory.Inv.Length];

        for (int i = 0; i < inventory.Inv.Length; ++i)
        {
            slotPrefabs[i] = Instantiate(slotPrefab, rectTransform) as GameObject;
            slotPrefabs[i].GetComponent<RectTransform>().localPosition = Vector3.zero;
            slotPrefabs[i].GetComponentInChildren<TextMeshProUGUI>().text = (inventory.Inv[i].count == 1 || inventory.Inv[i].count == 0) ? "" : inventory.Inv[i].count.ToString("n0");
            slotPrefabs[i].GetComponent<Image>().sprite = (inventory.Inv[i].item == null) ? emptyImage : inventory.Inv[i].item.image;

            slotPrefabs[i].transform.localPosition = new Vector3()
            {
                x = ((i % rowsize) * cellOffset.x) + margin.x,
                y = ((int)(i / rowsize) * cellOffset.y),
                z = 0f,
            };
        }

        cam = Camera.main;
    }

    private void RayCheck()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, maxRayCastDistance, layerMask.value))
        {
            takingText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                ObjectStates states = hit.collider.GetComponent<ObjectStates>();
                if (inventory.AddItem(states.id, states.count))
                {
                    //inventory.DebugConsoleWrite();
                    states.gameObject.SetActive(false);
                    return;
                }
            }
        }
        else
        {
            takingText.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RayCheck();
    }
}
