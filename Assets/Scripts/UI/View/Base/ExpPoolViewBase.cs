using UnityEngine;
using UnityEngine.UI;

public class ExpPoolViewBase : UIViewBase
{

    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Button cancel_btn;
    [HideInInspector]
    public Button levelup_btn;
    [HideInInspector]
    public Text exp_txt;
    [HideInInspector]
    public Text expend_txt;
    [HideInInspector]
    public Button add_btn;
    [HideInInspector]
    public Button minus_btn;
    [HideInInspector]
    public Text targetLv_txt;
    [HideInInspector]
    public Image Head_img;
    [HideInInspector]
    public GameObject StarParent_obj;
    [HideInInspector]
    public Image Border_img;
    [HideInInspector]
    public GameObject HideStar_obj;
    [HideInInspector]
    public Text Lv_txt;
    public virtual void Awake()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
        cancel_btn = mTransform.Find("cancel_btn").GetComponent<Button>();
        levelup_btn = mTransform.Find("levelup_btn").GetComponent<Button>();
        exp_txt = mTransform.Find("exp_txt").GetComponent<Text>();
        expend_txt = mTransform.Find("expend_txt").GetComponent<Text>();
        add_btn = mTransform.Find("add_btn").GetComponent<Button>();
        minus_btn = mTransform.Find("minus_btn").GetComponent<Button>();
        targetLv_txt = mTransform.Find("targetLv_txt").GetComponent<Text>();
        Head_img = mTransform.Find("Head_img").GetComponent<Image>();
        StarParent_obj = mTransform.Find("Head_img/StarParent_obj").gameObject;
        Border_img = mTransform.Find("Head_img/Border_img").GetComponent<Image>();
        HideStar_obj = mTransform.Find("Head_img/HideStar_obj").gameObject;
        Lv_txt = mTransform.Find("Head_img/Lv_txt").GetComponent<Text>();
    }
}
