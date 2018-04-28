using UnityEngine;
using UnityEngine.UI;

public class TipsViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public RectTransform simpletips_rect;
    [HideInInspector]
    public Text simpletipst_txt;
    [HideInInspector]
    public RectTransform itemtips_rect;
    [HideInInspector]
    public Button ui_itemlevelbg_btn;
    [HideInInspector]
    public Image item_img;
    [HideInInspector]
    public Text num_txt;
    [HideInInspector]
    public GameObject itemselectbg_obj;
    [HideInInspector]
    public Image itemselectbg_img;
    [HideInInspector]
    public GameObject shade_obj;
    [HideInInspector]
    public Text ItemName_txt;
    [HideInInspector]
    public Text ItemNum_txt;
    [HideInInspector]
    public Text ItemPrice_txt;
    [HideInInspector]
    public Text ItemIntroduce_txt;
    [HideInInspector]
    public RectTransform monstertips_rect;
    [HideInInspector]
    public Text monstername_txt;
    [HideInInspector]
    public Text monsterlevel_txt;
    [HideInInspector]
    public GameObject monsterisboss_obj;
    [HideInInspector]
    public Text monsterintroduce_txt;
    [HideInInspector]
    public Image monsterlevel_img;
    [HideInInspector]
    public Image monsterhead_img;
    [HideInInspector]
    public RectTransform treasuretips_rect;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       simpletips_rect = mTransform.Find("simpletips_rect").GetComponent<RectTransform>();
       simpletipst_txt = mTransform.Find("simpletips_rect/simpletipst_txt").GetComponent<Text>();
       itemtips_rect = mTransform.Find("itemtips_rect").GetComponent<RectTransform>();
       ui_itemlevelbg_btn = mTransform.Find("itemtips_rect/ui_itemlevelbg_btn").GetComponent<Button>();
       item_img = mTransform.Find("itemtips_rect/ui_itemlevelbg_btn/item_mask/item_img").GetComponent<Image>();
       num_txt = mTransform.Find("itemtips_rect/ui_itemlevelbg_btn/num_txt").GetComponent<Text>();
       itemselectbg_obj = mTransform.Find("itemtips_rect/ui_itemlevelbg_btn/itemselectbg_obj").gameObject;
       itemselectbg_img = mTransform.Find("itemtips_rect/ui_itemlevelbg_btn/itemselectbg_obj/itemselectbg_img").GetComponent<Image>();
       shade_obj = mTransform.Find("itemtips_rect/ui_itemlevelbg_btn/shade_obj").gameObject;
       ItemName_txt = mTransform.Find("itemtips_rect/ItemName_txt").GetComponent<Text>();
       ItemNum_txt = mTransform.Find("itemtips_rect/ItemNum_txt").GetComponent<Text>();
       ItemPrice_txt = mTransform.Find("itemtips_rect/ItemPrice_txt").GetComponent<Text>();
       ItemIntroduce_txt = mTransform.Find("itemtips_rect/ItemIntroduce_txt").GetComponent<Text>();
       monstertips_rect = mTransform.Find("monstertips_rect").GetComponent<RectTransform>();
       monstername_txt = mTransform.Find("monstertips_rect/monstername_txt").GetComponent<Text>();
       monsterlevel_txt = mTransform.Find("monstertips_rect/monsterlevel_txt").GetComponent<Text>();
       monsterisboss_obj = mTransform.Find("monstertips_rect/monsterisboss_obj").gameObject;
       monsterintroduce_txt = mTransform.Find("monstertips_rect/monsterintroduce_txt").GetComponent<Text>();
       monsterlevel_img = mTransform.Find("monstertips_rect/monsterlevel_img").GetComponent<Image>();
       monsterhead_img = mTransform.Find("monstertips_rect/monsterhead_img").GetComponent<Image>();
       treasuretips_rect = mTransform.Find("treasuretips_rect").GetComponent<RectTransform>();
    }
}