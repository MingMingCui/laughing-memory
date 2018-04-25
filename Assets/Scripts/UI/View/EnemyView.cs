using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [HideInInspector]
    public Button Enemy;
    [HideInInspector]
    public Image EnemylevelIco;
    [HideInInspector]
    public GameObject EnemySign;
    [HideInInspector]
    public Image EnemyIco;
    [HideInInspector]
    public int EnemyID;
    [HideInInspector]
    public bool isShow =false;
    [HideInInspector]
    public RectTransform tran;

    public void Init()
    {
        tran = transform.GetComponent<RectTransform>();
        Enemy = this.GetComponent<Button>();
        EnemylevelIco = transform.Find("levelico").GetComponent<Image>();
        EnemySign = transform.Find("sign").gameObject;
        EnemyIco = this.GetComponent<Image>();
    }
    public void Endow(int icoid, bool isboss)
    {
        EnemyID = icoid;
        Monster monster = JsonMgr.GetSingleton().GetMonsterByID(icoid);
        EnemySign.SetActive(isboss);
        EnemyIco.sprite = ResourceMgr.Instance.LoadSprite(monster.headid);
        EnemylevelIco.sprite = ResourceMgr.Instance.LoadSprite(JsonMgr.GetSingleton().GetHeroRareByID(monster.rare).HeadBorder);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TipsMgr.CloseTip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isShow)
        {
            TipsMgr.OpenMonsterTip(tran.localPosition, EnemyID, EnemySign.activeSelf, Alignment.RM, new Vector2(0, 181.14f));
        }
    }
}
