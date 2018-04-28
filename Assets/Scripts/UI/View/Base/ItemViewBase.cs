using UnityEngine;
using UnityEngine.UI;

public class ItemViewBase : UIViewBase 
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
    public GameObject Item_obj;
    [HideInInspector]
    public Image itembg_img;
    [HideInInspector]
    public Button itemclose_btn;
    [HideInInspector]
    public Image icon_img;
    [HideInInspector]
    public Image fun_img;
    [HideInInspector]
    public Toggle total_tog;
    [HideInInspector]
    public Image totalbg_img;
    [HideInInspector]
    public Toggle consumables_tog;
    [HideInInspector]
    public Image consumablesbg_img;
    [HideInInspector]
    public Toggle equip_tog;
    [HideInInspector]
    public Image equipbg_img;
    [HideInInspector]
    public Toggle material_tog;
    [HideInInspector]
    public Image materialbg_img;
    [HideInInspector]
    public Toggle other_tog;
    [HideInInspector]
    public Image otherbg_img;
    [HideInInspector]
    public GameObject gridlist_obj;
    [HideInInspector]
    public Button collating_btn;
    [HideInInspector]
    public GameObject inventorypop_obj;
    [HideInInspector]
    public Image inventorybg_img;
    [HideInInspector]
    public Image itemiconbg_img;
    [HideInInspector]
    public Image itemiconcolor_img;
    [HideInInspector]
    public Image itemicon_img;
    [HideInInspector]
    public Text itemname_txt;
    [HideInInspector]
    public Text quantity_txt;
    [HideInInspector]
    public Text propertydes_txt;
    [HideInInspector]
    public Text use_txt;
    [HideInInspector]
    public Text unitsaleprice_txt;
    [HideInInspector]
    public Text unitprice_txt;
    [HideInInspector]
    public Image copper_img;
    [HideInInspector]
    public Button sale_btn;
    [HideInInspector]
    public Button use_btn;
    [HideInInspector]
    public Button details_btn;
    [HideInInspector]
    public GameObject salepop_obj;
    [HideInInspector]
    public Image zhezhao_img;
    [HideInInspector]
    public Image salebg_img;
    [HideInInspector]
    public Image sale_img;
    [HideInInspector]
    public Button saleclose_btn;
    [HideInInspector]
    public Image saleitemiconbg_img;
    [HideInInspector]
    public Image saleitemiconcolor_img;
    [HideInInspector]
    public Image saleitemicon_img;
    [HideInInspector]
    public Text saleitemname_txt;
    [HideInInspector]
    public Text salequantity_txt;
    [HideInInspector]
    public Text saleunitprice_txt;
    [HideInInspector]
    public Image quantitybg_img;
    [HideInInspector]
    public Button sub_btn;
    [HideInInspector]
    public Image text_img;
    [HideInInspector]
    public Text select_txt;
    [HideInInspector]
    public Text total_txt;
    [HideInInspector]
    public Button add_btn;
    [HideInInspector]
    public Button max_btn;
    [HideInInspector]
    public Text getmoney_txt;
    [HideInInspector]
    public Text getmoneyprice_txt;
    [HideInInspector]
    public Button confirmsale_btn;
    void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       chatacters1_obj = mTransform.Find("UIbg/chatacters1_obj").gameObject;
       characters1_obj = mTransform.Find("UIbg/chatacters1_obj/characters1_obj").gameObject;
       characters2_obj = mTransform.Find("UIbg/chatacters1_obj/characters2_obj").gameObject;
       chatacters2_obj = mTransform.Find("UIbg/chatacters2_obj").gameObject;
       characters1_obj = mTransform.Find("UIbg/chatacters2_obj/characters1_obj").gameObject;
       characters2_obj = mTransform.Find("UIbg/chatacters2_obj/characters2_obj").gameObject;
       chatacters3_obj = mTransform.Find("UIbg/chatacters3_obj").gameObject;
       characters1_obj = mTransform.Find("UIbg/chatacters3_obj/characters1_obj").gameObject;
       characters2_obj = mTransform.Find("UIbg/chatacters3_obj/characters2_obj").gameObject;
       Item_obj = mTransform.Find("Item_obj").gameObject;
       itembg_img = mTransform.Find("Item_obj/itembg_img").GetComponent<Image>();
       itemclose_btn = mTransform.Find("Item_obj/itemclose_btn").GetComponent<Button>();
       icon_img = mTransform.Find("Item_obj/icon_img").GetComponent<Image>();
       fun_img = mTransform.Find("Item_obj/fun_img").GetComponent<Image>();
       total_tog = mTransform.Find("Item_obj/fun_img/featurelist/list/total_tog").GetComponent<Toggle>();
       totalbg_img = mTransform.Find("Item_obj/fun_img/featurelist/list/total_tog/totalbg_img").GetComponent<Image>();
       consumables_tog = mTransform.Find("Item_obj/fun_img/featurelist/list/consumables_tog").GetComponent<Toggle>();
       consumablesbg_img = mTransform.Find("Item_obj/fun_img/featurelist/list/consumables_tog/consumablesbg_img").GetComponent<Image>();
       equip_tog = mTransform.Find("Item_obj/fun_img/featurelist/list/equip_tog").GetComponent<Toggle>();
       equipbg_img = mTransform.Find("Item_obj/fun_img/featurelist/list/equip_tog/equipbg_img").GetComponent<Image>();
       material_tog = mTransform.Find("Item_obj/fun_img/featurelist/list/material_tog").GetComponent<Toggle>();
       materialbg_img = mTransform.Find("Item_obj/fun_img/featurelist/list/material_tog/materialbg_img").GetComponent<Image>();
       other_tog = mTransform.Find("Item_obj/fun_img/featurelist/list/other_tog").GetComponent<Toggle>();
       otherbg_img = mTransform.Find("Item_obj/fun_img/featurelist/list/other_tog/otherbg_img").GetComponent<Image>();
       gridlist_obj = mTransform.Find("Item_obj/inventory/gridlist_obj").gameObject;
       collating_btn = mTransform.Find("Item_obj/collating_btn").GetComponent<Button>();
       inventorypop_obj = mTransform.Find("Item_obj/inventorypop_obj").gameObject;
       inventorybg_img = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/inventorybg_img").GetComponent<Image>();
       itemiconbg_img = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/itemiconbg_img").GetComponent<Image>();
       itemiconcolor_img = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/itemiconbg_img/itemiconcolor_img").GetComponent<Image>();
       itemicon_img = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/itemiconbg_img/itemicon_img").GetComponent<Image>();
       itemname_txt = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/itemname_txt").GetComponent<Text>();
       quantity_txt = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/quantity/quantity_txt").GetComponent<Text>();
       propertydes_txt = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/propertydes/propertydes_txt").GetComponent<Text>();
       use_txt = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/use_txt").GetComponent<Text>();
       unitsaleprice_txt = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/unitprice/unitsaleprice_txt").GetComponent<Text>();
       unitprice_txt = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/unitprice/unitprice_txt").GetComponent<Text>();
       copper_img = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/unitprice/copper_img").GetComponent<Image>();
       sale_btn = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/sale_btn").GetComponent<Button>();
       use_btn = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/use_btn").GetComponent<Button>();
       details_btn = mTransform.Find("Item_obj/inventorypop_obj/inventorypop/details_btn").GetComponent<Button>();
       salepop_obj = mTransform.Find("Item_obj/salepop_obj").gameObject;
       zhezhao_img = mTransform.Find("Item_obj/salepop_obj/zhezhao_img").GetComponent<Image>();
       salebg_img = mTransform.Find("Item_obj/salepop_obj/salepop/salebg_img").GetComponent<Image>();
       sale_img = mTransform.Find("Item_obj/salepop_obj/salepop/salebg_img/sale_img").GetComponent<Image>();
       saleclose_btn = mTransform.Find("Item_obj/salepop_obj/salepop/saleclose_btn").GetComponent<Button>();
       saleitemiconbg_img = mTransform.Find("Item_obj/salepop_obj/salepop/saleitemiconbg_img").GetComponent<Image>();
       saleitemiconcolor_img = mTransform.Find("Item_obj/salepop_obj/salepop/saleitemiconbg_img/saleitemiconcolor_img").GetComponent<Image>();
       saleitemicon_img = mTransform.Find("Item_obj/salepop_obj/salepop/saleitemiconbg_img/saleitemicon_img").GetComponent<Image>();
       saleitemname_txt = mTransform.Find("Item_obj/salepop_obj/salepop/saleitemname_txt").GetComponent<Text>();
       salequantity_txt = mTransform.Find("Item_obj/salepop_obj/salepop/salequantity/salequantity_txt").GetComponent<Text>();
       saleunitprice_txt = mTransform.Find("Item_obj/salepop_obj/salepop/saleunitprice/saleunitprice_txt").GetComponent<Text>();
       quantitybg_img = mTransform.Find("Item_obj/salepop_obj/salepop/selectquantity/quantitybg_img").GetComponent<Image>();
       sub_btn = mTransform.Find("Item_obj/salepop_obj/salepop/selectquantity/sub_btn").GetComponent<Button>();
       text_img = mTransform.Find("Item_obj/salepop_obj/salepop/selectquantity/text_img").GetComponent<Image>();
       select_txt = mTransform.Find("Item_obj/salepop_obj/salepop/selectquantity/text_img/select_txt").GetComponent<Text>();
       total_txt = mTransform.Find("Item_obj/salepop_obj/salepop/selectquantity/text_img/total_txt").GetComponent<Text>();
       add_btn = mTransform.Find("Item_obj/salepop_obj/salepop/selectquantity/add_btn").GetComponent<Button>();
       max_btn = mTransform.Find("Item_obj/salepop_obj/salepop/selectquantity/max_btn").GetComponent<Button>();
       getmoney_txt = mTransform.Find("Item_obj/salepop_obj/salepop/getmoney/getmoney_txt").GetComponent<Text>();
       getmoneyprice_txt = mTransform.Find("Item_obj/salepop_obj/salepop/getmoney/getmoneyprice_txt").GetComponent<Text>();
       confirmsale_btn = mTransform.Find("Item_obj/salepop_obj/salepop/confirmsale_btn").GetComponent<Button>();
    }
}