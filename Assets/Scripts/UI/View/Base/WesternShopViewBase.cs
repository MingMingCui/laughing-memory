using UnityEngine;
using UnityEngine.UI;

public class WesternShopViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Image bg_img;
    [HideInInspector]
    public Button head_btn;
    [HideInInspector]
    public Image name_img;
    [HideInInspector]
    public Text RefreshTime_txt;
    [HideInInspector]
    public Text DisTime_txt;
    [HideInInspector]
    public GameObject goodslist_obj;
    [HideInInspector]
    public Button Refresh_btn;
    [HideInInspector]
    public GameObject Tips_obj;
    [HideInInspector]
    public Image tipsbg_img;
    [HideInInspector]
    public Image tipstitle_img;
    [HideInInspector]
    public Image tipstextbg_img;
    [HideInInspector]
    public Text wingnum_txt;
    [HideInInspector]
    public Text wingnum1_txt;
    [HideInInspector]
    public Text refreshtime_txt;
    [HideInInspector]
    public Button concel_btn;
    [HideInInspector]
    public Button ok_btn;
    [HideInInspector]
    public GameObject Goods_obj;
    [HideInInspector]
    public Image goodsbg_img;
    [HideInInspector]
    public Image goodslevel_img;
    [HideInInspector]
    public Image goods_img;
    [HideInInspector]
    public Text goodsname_txt;
    [HideInInspector]
    public Text goodsnum_txt;
    [HideInInspector]
    public Image goodsproperty_img;
    [HideInInspector]
    public Text goodsproperty_txt;
    [HideInInspector]
    public Text goodsuse_txt;
    [HideInInspector]
    public Text buynum_txt;
    [HideInInspector]
    public Image buynumbg_img;
    [HideInInspector]
    public Image buynumicon_img;
    [HideInInspector]
    public Text buyprice_txt;
    [HideInInspector]
    public Button confirmbuy_btn;
    [HideInInspector]
    public Button goodsclose_btn;
    [HideInInspector]
    public GameObject words_obj;
    [HideInInspector]
    public Image words_img;
    [HideInInspector]
    public Text words_txt;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       bg_img = mTransform.Find("WesternShop/bg_img").GetComponent<Image>();
       head_btn = mTransform.Find("WesternShop/head_btn").GetComponent<Button>();
       name_img = mTransform.Find("WesternShop/name_img").GetComponent<Image>();
       RefreshTime_txt = mTransform.Find("WesternShop/RefreshTime/RefreshTime_txt").GetComponent<Text>();
       DisTime_txt = mTransform.Find("WesternShop/RefreshTime/DisTime_txt").GetComponent<Text>();
       goodslist_obj = mTransform.Find("WesternShop/goods/goodslist_obj").gameObject;
       Refresh_btn = mTransform.Find("WesternShop/Refresh_btn").GetComponent<Button>();
       Tips_obj = mTransform.Find("WesternShop/Refresh_btn/Tips_obj").gameObject;
       tipsbg_img = mTransform.Find("WesternShop/Refresh_btn/Tips_obj/tipsbg_img").GetComponent<Image>();
       tipstitle_img = mTransform.Find("WesternShop/Refresh_btn/Tips_obj/tipstitle_img").GetComponent<Image>();
       tipstextbg_img = mTransform.Find("WesternShop/Refresh_btn/Tips_obj/tipstextbg_img").GetComponent<Image>();
       wingnum_txt = mTransform.Find("WesternShop/Refresh_btn/Tips_obj/tipstextbg_img/Text/wingnum_txt").GetComponent<Text>();
       wingnum1_txt = mTransform.Find("WesternShop/Refresh_btn/Tips_obj/tipstextbg_img/Text (1)/wingnum1_txt").GetComponent<Text>();
       refreshtime_txt = mTransform.Find("WesternShop/Refresh_btn/Tips_obj/tipstextbg_img/Text (2)/refreshtime_txt").GetComponent<Text>();
       concel_btn = mTransform.Find("WesternShop/Refresh_btn/Tips_obj/concel_btn").GetComponent<Button>();
       ok_btn = mTransform.Find("WesternShop/Refresh_btn/Tips_obj/ok_btn").GetComponent<Button>();
       Goods_obj = mTransform.Find("WesternShop/Goods_obj").gameObject;
       goodsbg_img = mTransform.Find("WesternShop/Goods_obj/goodsbg_img").GetComponent<Image>();
       goodslevel_img = mTransform.Find("WesternShop/Goods_obj/goodslevel_img").GetComponent<Image>();
       goods_img = mTransform.Find("WesternShop/Goods_obj/goodslevel_img/goods_img").GetComponent<Image>();
       goodsname_txt = mTransform.Find("WesternShop/Goods_obj/goodsname_txt").GetComponent<Text>();
       goodsnum_txt = mTransform.Find("WesternShop/Goods_obj/goodsnum/goodsnum_txt").GetComponent<Text>();
       goodsproperty_img = mTransform.Find("WesternShop/Goods_obj/goodsproperty_img").GetComponent<Image>();
       goodsproperty_txt = mTransform.Find("WesternShop/Goods_obj/goodsproperty_img/goodsproperty_txt").GetComponent<Text>();
       goodsuse_txt = mTransform.Find("WesternShop/Goods_obj/goodsuse_txt").GetComponent<Text>();
       buynum_txt = mTransform.Find("WesternShop/Goods_obj/buynum/buynum_txt").GetComponent<Text>();
       buynumbg_img = mTransform.Find("WesternShop/Goods_obj/buynum/buynumbg_img").GetComponent<Image>();
       buynumicon_img = mTransform.Find("WesternShop/Goods_obj/buynum/buynumbg_img/buynumicon_img").GetComponent<Image>();
       buyprice_txt = mTransform.Find("WesternShop/Goods_obj/buynum/buynumbg_img/buyprice_txt").GetComponent<Text>();
       confirmbuy_btn = mTransform.Find("WesternShop/Goods_obj/confirmbuy_btn").GetComponent<Button>();
       goodsclose_btn = mTransform.Find("WesternShop/Goods_obj/goodsclose_btn").GetComponent<Button>();
       words_obj = mTransform.Find("WesternShop/words_obj").gameObject;
       words_img = mTransform.Find("WesternShop/words_obj/words_img").GetComponent<Image>();
       words_txt = mTransform.Find("WesternShop/words_obj/words_img/words_txt").GetComponent<Text>();
    }
}