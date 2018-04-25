using JsonData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailItemview : MonoBehaviour
{
    [HideInInspector]
    public Button MailItem;
    [HideInInspector]
    public Image Item_img;
    [HideInInspector]
    public Text Num;
    [HideInInspector]
    public Image itemLevelbg_img;
    [HideInInspector]
    public int MailId;
    [HideInInspector]
    public bool idUse;
    public void Init()
    {
        MailItem = transform.GetComponent<Button>();
        itemLevelbg_img = transform.GetComponent<Image>();
        Item_img = transform.Find("item_img").GetComponent<Image>();
        Num = transform.Find("num_txt").GetComponent<Text>();
    }
    public void Endow(int _mailis,int _id, int itemnum)
    {
        idUse = true;
        MailId = _mailis;
        ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(_id);
        itemLevelbg_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.rare - 1]);
        Item_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
        Num.text = itemnum.ToString();

    }
}
