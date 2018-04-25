using System.Collections;
using System.Collections.Generic;
using JsonData;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUIView : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [HideInInspector]
    public Text num_txt = null;
    [HideInInspector]
    public Button itemlevelbg_btn = null;
    [HideInInspector]
    public int itemUIid;
    [HideInInspector]
    public GameObject itemselectbg_obj;
    [HideInInspector]
    public bool isRegister = false;
    [HideInInspector]
    public Image itemLevelbg_img;
    [HideInInspector]
    public Image itemImg_img;
    [HideInInspector]
    public GameObject shade_obj;
    [HideInInspector]
    public Mask item_mask;
    [HideInInspector]
    public bool isShow =false;
    [HideInInspector]
    public RectTransform tran;
    public void Init()
    {
        Transform mTransform = this.transform;
        tran = mTransform.GetComponent<RectTransform>();
        itemselectbg_obj = mTransform.Find("itemselectbg_obj").gameObject;
        itemLevelbg_img = mTransform.GetComponent<Image>();
        item_mask = mTransform.Find("item_mask").GetComponent<Mask>();
        itemImg_img = mTransform.Find("item_mask/item_img").GetComponent<Image>();
        num_txt = mTransform.GetComponentInChildren<Text>();
        itemlevelbg_btn = mTransform.GetComponent<Button>();
        shade_obj = mTransform.Find("shade_obj").gameObject;
    }


    /// <summary>
    /// 初始化单个物品
    /// </summary>
    /// <param name="items"></param>
    public void SetInfo(int itemid, int itemnum)
    {
        this.itemUIid = itemid;
        ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(itemid);
        item_mask.enabled = ic.type == FuncType.PIECES;
        itemLevelbg_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.rare - 1]);
        itemImg_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
        num_txt.text = itemnum > 0 ? itemnum.ToString() : "";
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        TipsMgr.CloseTip();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isShow)
        {
            TipsMgr.OpenItemTip(tran.localPosition, itemUIid, Alignment.M, new Vector2(0, -100));
        }
    }
}