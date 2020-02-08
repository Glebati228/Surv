using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemActions : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerDownHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin");
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEnd");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        RectTransform rectTransform = transform as RectTransform;
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void Start()
    {

    }

    public void Update()
    {

    }
}

