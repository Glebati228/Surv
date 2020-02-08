using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppedItem : MonoBehaviour
{ 
#pragma warning disable 0649
    public int id;
    public int count;
#pragma warning restore 0649

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Item");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
