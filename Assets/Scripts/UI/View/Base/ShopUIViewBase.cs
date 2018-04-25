using UnityEngine;
using UnityEngine.UI;

public class ShopUIViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public GameObject chatacters1_obj;
    [HideInInspector]
    public GameObject characters1_obj;
    [HideInInspector]
    public GameObject characters2_obj;
    [HideInInspector]
    public GameObject chatacters2_obj;


    [HideInInspector]
    public GameObject chatacters3_obj;


    [HideInInspector]
    public Button head_btn;
    [HideInInspector]
    public Image NPC_img;
    [HideInInspector]
    public Image name_img;
    [HideInInspector]
    public Text RefreshTime_txt;
    [HideInInspector]
    public ScrollRect goods_sr;
    [HideInInspector]
    public GameObject goodslist_obj;
    [HideInInspector]
    public Button Refresh_btn;
    [HideInInspector]
    public Toggle shopordinary_tog;
    [HideInInspector]
    public Toggle shoppassbarrier_tog;
    [HideInInspector]
    public Toggle shopcompetitive_tog;
    [HideInInspector]
    public Toggle shopguild_tog;
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
    [HideInInspector]
    public GameObject currencytips_obj;
    [HideInInspector]
    public Image curtipbg_img;
    [HideInInspector]
    public Text currencytip_txt;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       chatacters1_obj = mTransform.Find("uI_bg/chatacters1_obj").gameObject;
       characters1_obj = mTransform.Find("uI_bg/chatacters1_obj/characters1_obj").gameObject;
       characters2_obj = mTransform.Find("uI_bg/chatacters1_obj/characters2_obj").gameObject;
       chatacters2_obj = mTransform.Find("uI_bg/chatacters2_obj").gameObject;
       characters1_obj = mTransform.Find("uI_bg/chatacters2_obj/characters1_obj").gameObject;
       characters2_obj = mTransform.Find("uI_bg/chatacters2_obj/characters2_obj").gameObject;
       chatacters3_obj = mTransform.Find("uI_bg/chatacters3_obj").gameObject;
       characters1_obj = mTransform.Find("uI_bg/chatacters3_obj/characters1_obj").gameObject;
       characters2_obj = mTransform.Find("uI_bg/chatacters3_obj/characters2_obj").gameObject;
       head_btn = mTransform.Find("shopui/bg/head_btn").GetComponent<Button>();
       NPC_img = mTransform.Find("shopui/bg/NPC_img").GetComponent<Image>();
       name_img = mTransform.Find("shopui/name/name_img").GetComponent<Image>();
       RefreshTime_txt = mTransform.Find("shopui/RefreshTime/RefreshTime_txt").GetComponent<Text>();
       goods_sr = mTransform.Find("shopui/goods_sr").GetComponent<ScrollRect>();
       goodslist_obj = mTransform.Find("shopui/goods_sr/goodslist_obj").gameObject;
       Refresh_btn = mTransform.Find("shopui/Refresh_btn").GetComponent<Button>();
       shopordinary_tog = mTransform.Find("shopui/shopordinary_tog").GetComponent<Toggle>();
       shoppassbarrier_tog = mTransform.Find("shopui/shoppassbarrier_tog").GetComponent<Toggle>();
       shopcompetitive_tog = mTransform.Find("shopui/shopcompetitive_tog").GetComponent<Toggle>();
       shopguild_tog = mTransform.Find("shopui/shopguild_tog").GetComponent<Toggle>();
       Goods_obj = mTransform.Find("shopui/Goods_obj").gameObject;
       goodsbg_img = mTransform.Find("shopui/Goods_obj/goodsbg_img").GetComponent<Image>();
       goodslevel_img = mTransform.Find("shopui/Goods_obj/goodslevel_img").GetComponent<Image>();
       goods_img = mTransform.Find("shopui/Goods_obj/goodslevel_img/goods_img").GetComponent<Image>();
       goodsname_txt = mTransform.Find("shopui/Goods_obj/goodsname_txt").GetComponent<Text>();
       goodsnum_txt = mTransform.Find("shopui/Goods_obj/goodsnum/goodsnum_txt").GetComponent<Text>();
       goodsproperty_img = mTransform.Find("shopui/Goods_obj/goodsproperty_img").GetComponent<Image>();
       goodsproperty_txt = mTransform.Find("shopui/Goods_obj/goodsproperty_img/goodsproperty_txt").GetComponent<Text>();
       goodsuse_txt = mTransform.Find("shopui/Goods_obj/goodsuse_txt").GetComponent<Text>();
       buynum_txt = mTransform.Find("shopui/Goods_obj/buynum/buynum_txt").GetComponent<Text>();
       buynumbg_img = mTransform.Find("shopui/Goods_obj/buynum/buynumbg_img").GetComponent<Image>();
       buynumicon_img = mTransform.Find("shopui/Goods_obj/buynum/buynumbg_img/buynumicon_img").GetComponent<Image>();
       buyprice_txt = mTransform.Find("shopui/Goods_obj/buynum/buynumbg_img/buyprice_txt").GetComponent<Text>();
       confirmbuy_btn = mTransform.Find("shopui/Goods_obj/confirmbuy_btn").GetComponent<Button>();
       goodsclose_btn = mTransform.Find("shopui/Goods_obj/goodsclose_btn").GetComponent<Button>();
       words_obj = mTransform.Find("shopui/words_obj").gameObject;
       words_img = mTransform.Find("shopui/words_obj/words_img").GetComponent<Image>();
       words_txt = mTransform.Find("shopui/words_obj/words_img/words_txt").GetComponent<Text>();
       currencytips_obj = mTransform.Find("shopui/currencytips_obj").gameObject;
       curtipbg_img = mTransform.Find("shopui/currencytips_obj/curtipbg_img").GetComponent<Image>();
       currencytip_txt = mTransform.Find("shopui/currencytips_obj/curtipbg_img/currencytip_txt").GetComponent<Text>();
    }
}