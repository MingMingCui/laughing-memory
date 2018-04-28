using UnityEngine;
using UnityEngine.UI;

public class GoodsUIView : MonoBehaviour 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Button goods_btn;
    [HideInInspector]
    public Text GoodsName_txt;
    [HideInInspector]
    public Image GoodsLevel_img;
    [HideInInspector]
    public Image Goods_img;
    [HideInInspector]
    public Text goodsnum_txt;
    [HideInInspector]
    public Image GoodsCopperBg_img;
    [HideInInspector]
    public Image copper_img;
    [HideInInspector]
    public Text price_txt;
    [HideInInspector]
    public Button SoldOut_btn;
    [HideInInspector]
    public Image SoldOut_img;
    [HideInInspector]
    public int goodsUIId;//物品的位置ID
    [HideInInspector]
    public int Currency;

    public void Init()
    {
        this.mTransform = this.transform;
       goods_btn = mTransform.GetComponent<Button>();
       GoodsName_txt = mTransform.Find("goods_btn/GoodsName_txt").GetComponent<Text>();
       GoodsLevel_img = mTransform.Find("ui_itemlevelbg_btn").GetComponent<Image>();
       Goods_img = mTransform.Find("ui_itemlevelbg_btn/item_mask/item_img").GetComponent<Image>();
       goodsnum_txt = mTransform.Find("ui_itemlevelbg_btn/goodsnum_txt").GetComponent<Text>();
       GoodsCopperBg_img = mTransform.Find("goods_btn/GoodsCopperBg_img").GetComponent<Image>();
       copper_img = mTransform.Find("goods_btn/GoodsCopperBg_img/copper_img").GetComponent<Image>();
       price_txt = mTransform.Find("goods_btn/GoodsCopperBg_img/price_txt").GetComponent<Text>();
       SoldOut_btn = mTransform.Find("SoldOut_btn").GetComponent<Button>();
       SoldOut_img = mTransform.Find("SoldOut_btn/SoldOut_img").GetComponent<Image>();
    }    

}