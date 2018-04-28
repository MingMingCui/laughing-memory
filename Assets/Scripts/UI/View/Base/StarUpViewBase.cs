using UnityEngine;
using UnityEngine.UI;

public class StarUpViewBase : UIViewBase
{
    [HideInInspector]
    public Transform mTransform;
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
    [HideInInspector]
    public Text Fighting_txt;
    [HideInInspector]
    public GameObject leftstar_obj;
    [HideInInspector]
    public GameObject rightstar_obj;
    [HideInInspector]
    public Image piece_img;
    [HideInInspector]
    public Image broder_img;
    [HideInInspector]
    public Text Piece_txt;
    [HideInInspector]
    public Text Cost_txt;
    [HideInInspector]
    public Button Canel_btn;
    [HideInInspector]
    public Button StarUp_btn;
    public virtual void Awake()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
        Head_img = mTransform.Find("Head_img").GetComponent<Image>();
        StarParent_obj = mTransform.Find("Head_img/StarParent_obj").gameObject;
        Border_img = mTransform.Find("Head_img/Border_img").GetComponent<Image>();
        HideStar_obj = mTransform.Find("Head_img/HideStar_obj").gameObject;
        Lv_txt = mTransform.Find("Head_img/Lv_txt").GetComponent<Text>();
        Fighting_txt = mTransform.Find("Image (1)/Fighting_txt").GetComponent<Text>();
        leftstar_obj = mTransform.Find("leftstar_obj").gameObject;
        rightstar_obj = mTransform.Find("rightstar_obj").gameObject;
        piece_img = mTransform.Find("mask/piece_img").GetComponent<Image>();
        broder_img = mTransform.Find("broder_img").GetComponent<Image>();
        Piece_txt = mTransform.Find("Piece_txt").GetComponent<Text>();
        Cost_txt = mTransform.Find("Cost_txt").GetComponent<Text>();
        Canel_btn = mTransform.Find("Canel_btn").GetComponent<Button>();
        StarUp_btn = mTransform.Find("StarUp_btn").GetComponent<Button>();
    }
}