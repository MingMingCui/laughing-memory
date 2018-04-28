using UnityEngine;
using UnityEngine.UI;

public class ShopViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public GameObject SelectShop_obj;
    [HideInInspector]
    public Image Shopbg_img;
    [HideInInspector]
    public Button ordinaryshop_btn;
    [HideInInspector]
    public Image shophead_img;
    [HideInInspector]
    public Image ordinaryshoprefresh_img;
    [HideInInspector]
    public Text ordinaryshoptimes_txt;
    [HideInInspector]
    public Button passbarrier_btn;
    [HideInInspector]
    public Image passbarriershoprefresh_img;
    [HideInInspector]
    public Text passbarriertimes_txt;
    [HideInInspector]
    public Button competitive_btn;
    [HideInInspector]
    public Image competitiveshoprefresh_img;
    [HideInInspector]
    public Text competitivetimes_txt;
    [HideInInspector]
    public Button guildshop_btn;
    [HideInInspector]
    public Image guildshoprefresh_img;
    [HideInInspector]
    public Text guildshoptimes_txt;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       SelectShop_obj = mTransform.Find("SelectShop_obj").gameObject;
       Shopbg_img = mTransform.Find("SelectShop_obj/Shopbg_img").GetComponent<Image>();
       ordinaryshop_btn = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/ordinaryshop_btn").GetComponent<Button>();
       shophead_img = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/ordinaryshop_btn/shophead_img").GetComponent<Image>();
       ordinaryshoprefresh_img = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/ordinaryshop_btn/ordinaryshoprefresh_img").GetComponent<Image>();
       ordinaryshoptimes_txt = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/ordinaryshop_btn/ordinaryshoprefresh_img/ordinaryshoptimes_txt").GetComponent<Text>();
       passbarrier_btn = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/passbarrier_btn").GetComponent<Button>();
       shophead_img = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/passbarrier_btn/shophead_img").GetComponent<Image>();
       passbarriershoprefresh_img= mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/passbarrier_btn/passbarriershoprefresh_img").GetComponent<Image>();
       passbarriertimes_txt = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/passbarrier_btn/passbarriershoprefresh_img/passbarriertimes_txt").GetComponent<Text>();
       competitive_btn = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/competitive_btn").GetComponent<Button>();
       shophead_img = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/competitive_btn/shophead_img").GetComponent<Image>();
       competitiveshoprefresh_img = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/competitive_btn/competitiveshoprefresh_img").GetComponent<Image>();
       competitivetimes_txt = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/competitive_btn/competitiveshoprefresh_img/competitivetimes_txt").GetComponent<Text>();
       guildshop_btn = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/guildshop_btn").GetComponent<Button>();
       shophead_img = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/guildshop_btn/shophead_img").GetComponent<Image>();
       guildshoprefresh_img = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/guildshop_btn/guildshoprefresh_img").GetComponent<Image>();
       guildshoptimes_txt = mTransform.Find("SelectShop_obj/Shopbg_img/shopselect/shoplist/guildshop_btn/guildshoprefresh_img/guildshoptimes_txt").GetComponent<Text>();
    }
}