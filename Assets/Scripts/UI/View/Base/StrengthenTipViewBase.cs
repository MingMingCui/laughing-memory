using UnityEngine;
using UnityEngine.UI;

public class StrengthenTipViewBase : UIViewBase
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Image border_img;
    [HideInInspector]
    public Image equip_img;
    [HideInInspector]
    public CanvasRenderer lvfloor_cr;
    [HideInInspector]
    public Text levle_txt;
    [HideInInspector]
    public Text name_txt;
    [HideInInspector]
    public Text state_txt;
    [HideInInspector]
    public Image floor_img;
    [HideInInspector]
    public Text des_txt;
    [HideInInspector]
    public Text attr_txt;
    [HideInInspector]
    public GameObject appendattr_obj;
    [HideInInspector]
    public Text appendattr_txt;
    [HideInInspector]
    public Text baseattr_txt;
    [HideInInspector]
    public GameObject constattr_obj;
    [HideInInspector]
    public Text constattr_txt;
    [HideInInspector]
    public Button strengthen_btn;
    [HideInInspector]
    public Button takeoff_btn;
    public virtual void Awake()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
        border_img = mTransform.Find("border_img").GetComponent<Image>();
        equip_img = mTransform.Find("equip_img").GetComponent<Image>();
        lvfloor_cr = mTransform.Find("equip_img/lvfloor_cr").GetComponent<CanvasRenderer>();
        levle_txt = mTransform.Find("equip_img/levle_txt").GetComponent<Text>();
        name_txt = mTransform.Find("name_txt").GetComponent<Text>();
        state_txt = mTransform.Find("state_txt").GetComponent<Text>();
        floor_img = mTransform.Find("floor_img").GetComponent<Image>();
        des_txt = mTransform.Find("floor_img/des_txt").GetComponent<Text>();
        attr_txt = mTransform.Find("attr_txt").GetComponent<Text>();
        appendattr_obj = mTransform.Find("appendattr_obj").gameObject;
        appendattr_txt = mTransform.Find("appendattr_obj/appendattr_txt").GetComponent<Text>();
        baseattr_txt = mTransform.Find("appendattr_obj/baseattr_txt").GetComponent<Text>();
        constattr_obj = mTransform.Find("constattr_obj").gameObject;
        constattr_txt = mTransform.Find("constattr_obj/constattr_txt").GetComponent<Text>();
        strengthen_btn = mTransform.Find("strengthen_btn").GetComponent<Button>();
        takeoff_btn = mTransform.Find("takeoff_btn").GetComponent<Button>();
    }
}
