using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public bool isShow = false;
    [HideInInspector]
    public RectTransform rect = null;
    public Vector2Int[] ItemData;
    void Start ()
    {
       

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (false == isShow)
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
        else
        {
            Debug.Log(rect.gameObject.name);
            TipsMgr.OpenTreasureTip(rect.localPosition, ItemData, Alignment.M,new Vector2(-50,180));
        }
       
    }

   
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (false == isShow)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            TipsMgr.CloseTip();

        }
    }
}
