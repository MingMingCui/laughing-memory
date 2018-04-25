using JsonData;
using UnityEngine;
using UnityEngine.UI;

public class RecruitingItemView : MonoBehaviour
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Image itemlevel_img;
    [HideInInspector]
    public Image item_img;
    [HideInInspector]
    public Text itemname_txt;
    [HideInInspector]
    public Text itemnum_txt;
    [HideInInspector]
    public int itemid;


    public void Init()
    {
       this.mTransform = this.transform;
       itemlevel_img = mTransform.GetComponent<Image>();
       item_img = mTransform.Find("item_img").GetComponent<Image>();
       itemname_txt = mTransform.Find("itemname_txt").GetComponent<Text>();
       itemnum_txt = mTransform.Find("itemnum_txt").GetComponent<Text>();
    }


    /// <summary>
    /// 初始化单个物品
    /// </summary>
    /// <param name="items"></param>
    public void SetItemInfo(int itemid, int itemnum)
    {
        this.itemid = itemid;
        ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(itemid);
        itemlevel_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.rare - 1]);
        item_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
        if (itemnum == 1)
            itemnum_txt.text = "";
        else
            itemnum_txt.text = itemnum.ToString();
        itemname_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(itemid).name;
    }
}