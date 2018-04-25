using UnityEngine.UI;
using UnityEngine;

public class StrengthenViewBase : UIViewBase
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Toggle str_tog;
    [HideInInspector]
    public Toggle adv_tog;
    [HideInInspector]
    public Toggle ran_tog;
    [HideInInspector]
    public GameObject Strengthen_obj;
    [HideInInspector]
    public Text attr_txt;
    [HideInInspector]
    public Text thenattr_txt;
    [HideInInspector]
    public Image border_img;
    [HideInInspector]
    public Image equip_img;
    [HideInInspector]
    public CanvasRenderer lvfloor_cr;
    [HideInInspector]
    public Text lv_txt;
    [HideInInspector]
    public Image thenborder_img;
    [HideInInspector]
    public Image thenequip_img;
    [HideInInspector]
    public CanvasRenderer thenlvfloor_cr;
    [HideInInspector]
    public Text thenlv_txt;
    [HideInInspector]
    public Text spend_txt;
    [HideInInspector]
    public Button strengthen_btn;
    [HideInInspector]
    public Button strengthenonekey_btn;
    [HideInInspector]
    public GameObject Advanced_obj;
    [HideInInspector]
    public Transform parent_trf;
    [HideInInspector]
    public GameObject attr_obj;
    [HideInInspector]
    public Text name_txt;
    [HideInInspector]
    public Text advanced_txt;
    [HideInInspector]
    public Text baseattr_txt;
    [HideInInspector]
    public Text advancedattr_txt;
    [HideInInspector]
    public GameObject null_obj;
    [HideInInspector]
    public Text advancedspend_txt;
    [HideInInspector]
    public Button advanced_btn;
    [HideInInspector]
    public Image advancedborder_img;
    [HideInInspector]
    public Image advancedequip_img;
    [HideInInspector]
    public Image target_img;
    [HideInInspector]
    public GameObject Random_obj;
    [HideInInspector]
    public Image randomborder_img;
    [HideInInspector]
    public Image randomequip_img;
    [HideInInspector]
    public CanvasRenderer randomlvfloor_cr;
    [HideInInspector]
    public Text randomlv_txt;
    [HideInInspector]
    public Text equipname_txt;
    [HideInInspector]
    public Text oldattr_txt;
    [HideInInspector]
    public Text newattr_txt;
    [HideInInspector]
    public Text randomspend_txt;
    [HideInInspector]
    public Button random_btn;
    [HideInInspector]
    public Button finerandom_btn;
    [HideInInspector]
    public Button replace_btn;
    [HideInInspector]
    public Transform lock_trf;
    public virtual void Awake()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
        str_tog = mTransform.Find("Image/SideGround/str_tog").GetComponent<Toggle>();
        adv_tog = mTransform.Find("Image/SideGround/adv_tog").GetComponent<Toggle>();
        ran_tog = mTransform.Find("Image/SideGround/ran_tog").GetComponent<Toggle>();
        Strengthen_obj = mTransform.Find("Strengthen_obj").gameObject;
        attr_txt = mTransform.Find("Strengthen_obj/Image (5)/attr_txt").GetComponent<Text>();
        thenattr_txt = mTransform.Find("Strengthen_obj/Image (6)/thenattr_txt").GetComponent<Text>();
        border_img = mTransform.Find("Strengthen_obj/border_img").GetComponent<Image>();
        equip_img = mTransform.Find("Strengthen_obj/equip_img").GetComponent<Image>();
        lvfloor_cr = mTransform.Find("Strengthen_obj/equip_img/lvfloor_cr").GetComponent<CanvasRenderer>();
        lv_txt = mTransform.Find("Strengthen_obj/equip_img/lv_txt").GetComponent<Text>();
        thenborder_img = mTransform.Find("Strengthen_obj/thenborder_img").GetComponent<Image>();
        thenequip_img = mTransform.Find("Strengthen_obj/thenequip_img").GetComponent<Image>();
        thenlvfloor_cr = mTransform.Find("Strengthen_obj/thenequip_img/thenlvfloor_cr").GetComponent<CanvasRenderer>();
        thenlv_txt = mTransform.Find("Strengthen_obj/thenequip_img/thenlv_txt").GetComponent<Text>();
        spend_txt = mTransform.Find("Strengthen_obj/Text/spend_txt").GetComponent<Text>();
        strengthen_btn = mTransform.Find("Strengthen_obj/strengthen_btn").GetComponent<Button>();
        strengthenonekey_btn = mTransform.Find("Strengthen_obj/strengthenonekey_btn").GetComponent<Button>();
        Advanced_obj = mTransform.Find("Advanced_obj").gameObject;
        parent_trf = mTransform.Find("Advanced_obj/parent_trf").GetComponent<Transform>();
        attr_obj = mTransform.Find("Advanced_obj/Image/attr_obj").gameObject;
        name_txt = mTransform.Find("Advanced_obj/Image/attr_obj/name_txt").GetComponent<Text>();
        advanced_txt = mTransform.Find("Advanced_obj/Image/attr_obj/advanced_txt").GetComponent<Text>();
        baseattr_txt = mTransform.Find("Advanced_obj/Image/attr_obj/baseattr_txt").GetComponent<Text>();
        advancedattr_txt = mTransform.Find("Advanced_obj/Image/attr_obj/advancedattr_txt").GetComponent<Text>();
        null_obj = mTransform.Find("Advanced_obj/Image/null_obj").gameObject;
        advancedspend_txt = mTransform.Find("Advanced_obj/Text/advancedspend_txt").GetComponent<Text>();
        advanced_btn = mTransform.Find("Advanced_obj/advanced_btn").GetComponent<Button>();
        advancedborder_img = mTransform.Find("Advanced_obj/advancedborder_img").GetComponent<Image>();
        advancedequip_img = mTransform.Find("Advanced_obj/advancedequip_img").GetComponent<Image>();
        target_img = mTransform.Find("Advanced_obj/target_img").GetComponent<Image>();
        Random_obj = mTransform.Find("Random_obj").gameObject;
        randomborder_img = mTransform.Find("Random_obj/randomborder_img").GetComponent<Image>();
        randomequip_img = mTransform.Find("Random_obj/randomequip_img").GetComponent<Image>();
        randomlvfloor_cr = mTransform.Find("Random_obj/randomequip_img/randomlvfloor_cr").GetComponent<CanvasRenderer>();
        randomlv_txt = mTransform.Find("Random_obj/randomequip_img/randomlv_txt").GetComponent<Text>();
        equipname_txt = mTransform.Find("Random_obj/equipname_txt").GetComponent<Text>();
        oldattr_txt = mTransform.Find("Random_obj/Image/oldattr_txt").GetComponent<Text>();
        newattr_txt = mTransform.Find("Random_obj/Image (1)/newattr_txt").GetComponent<Text>();
        randomspend_txt = mTransform.Find("Random_obj/Text (1)/randomspend_txt").GetComponent<Text>();
        random_btn = mTransform.Find("Random_obj/random_btn").GetComponent<Button>();
        finerandom_btn = mTransform.Find("Random_obj/finerandom_btn").GetComponent<Button>();
        replace_btn = mTransform.Find("Random_obj/replace_btn").GetComponent<Button>();
        lock_trf = mTransform.Find("Random_obj/lock_trf").GetComponent<Transform>();
    }
}
