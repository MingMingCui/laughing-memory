using JsonData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreeView : MonoBehaviour
{
    [HideInInspector]
    public Image ItemIco_img;
    [HideInInspector]
    public Text ItemNum_txt;
    [HideInInspector]
    public bool idUse;
    [HideInInspector]
    public int MailId;
    public void Init()
    {
        ItemIco_img = transform.Find("ItemIco_img").GetComponent<Image>();
        ItemNum_txt = transform.Find("ItemNum_txt").GetComponent<Text>();
    }

    public void Endow(int _mailid, Item _item)
    {
        idUse = true;
        MailId = _mailid;
        transform.gameObject.SetActive(true);
        ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(_item.itemId);
        ItemIco_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
        ItemNum_txt.text = _item.itemNum.ToString();
    }
}
