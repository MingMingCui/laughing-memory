using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class LevelViewbs : MonoBehaviour
{
   
    [HideInInspector]
    public Button CloseBtn;
    [HideInInspector]
    public Image LockIcon;  //图标
    [HideInInspector]
    public Image PassIcon;  //图标
    [HideInInspector]
    public Image NowIcon;  //图标
    [HideInInspector]
    public GameObject Pentagon;//星星生成的父物体
    [HideInInspector]
    public bool isZeki = false; //是否大关

    public void Init( )
    {
        if (isZeki)
        {
            CloseBtn = transform.Find("Btn").GetComponent<Button>();
            LockIcon = transform.Find("Lock").GetComponent<Image>();
            PassIcon = transform.Find("Icon").GetComponent<Image>();
            Pentagon = transform.Find("pentagon").gameObject;
        }
        else
        {
            CloseBtn = transform.Find("Btn") .GetComponent< Button>();
            LockIcon = transform.Find("Lock").GetComponent<Image>();
            NowIcon = transform.Find("Now").GetComponent<Image>();
        }
		
	}
}
