using UnityEngine;
using UnityEngine.UI;

public class RecruitingAwardViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Image buyitemlevel_img;
    [HideInInspector]
    public Image buyitem_img;
    [HideInInspector]
    public Text buyitemnum_txt;
    [HideInInspector]
    public Text buyitemname_txt;
    [HideInInspector]
    public GameObject sendoneitem_obj;
    [HideInInspector]
    public GameObject sendtenitem_obj;
    [HideInInspector]
    public GameObject hundred_obj;
    [HideInInspector]
    public GameObject hundreditem_obj;
    [HideInInspector]
    public GameObject copperbutton_obj;
    [HideInInspector]
    public Button morehundred_btn;
    [HideInInspector]
    public Image moneyhundred_img;
    [HideInInspector]
    public Text hundredcostprice_txt;
    [HideInInspector]
    public Button moreone_btn;
    [HideInInspector]
    public Image money_img;
    [HideInInspector]
    public Text costprice_txt;
    [HideInInspector]
    public Button coppersure_btn;
    [HideInInspector]
    public GameObject goldbutton_obj;
    [HideInInspector]
    public Button goldmoreone_btn;
    [HideInInspector]
    public Image goldmoney_img;
    [HideInInspector]
    public Text goldcostprice_txt;
    [HideInInspector]
    public Button goldsure_btn;
    [HideInInspector]
    public GameObject unitcopperbutton_obj;
    [HideInInspector]
    public Button unitcoppersure_btn;
    [HideInInspector]
    public Button unitmoreone_btn;
    [HideInInspector]
    public Image unitmoney_img;
    [HideInInspector]
    public Text unitcostprice_txt;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       buyitemlevel_img = mTransform.Find("buyitemlevel_img").GetComponent<Image>();
       buyitem_img = mTransform.Find("buyitemlevel_img/buyitem_img").GetComponent<Image>();
       buyitemnum_txt = mTransform.Find("buyitemlevel_img/buyitemnum_txt").GetComponent<Text>();
       buyitemname_txt = mTransform.Find("buyitemlevel_img/buyitemname_txt").GetComponent<Text>();
       sendoneitem_obj = mTransform.Find("sendoneitem_obj").gameObject;
       sendtenitem_obj = mTransform.Find("sendtenitem_obj").gameObject;
       hundred_obj = mTransform.Find("hundred_obj").gameObject;
       hundreditem_obj = mTransform.Find("hundred_obj/hundreditem_obj").gameObject;
       copperbutton_obj = mTransform.Find("copperbutton_obj").gameObject;
       morehundred_btn = mTransform.Find("copperbutton_obj/morehundred_btn").GetComponent<Button>();
       moneyhundred_img = mTransform.Find("copperbutton_obj/Image (1)/moneyhundred_img").GetComponent<Image>();
       hundredcostprice_txt = mTransform.Find("copperbutton_obj/Image (1)/moneyhundred_img/hundredcostprice_txt").GetComponent<Text>();
       moreone_btn = mTransform.Find("copperbutton_obj/moreone_btn").GetComponent<Button>();
       money_img = mTransform.Find("copperbutton_obj/Image/money_img").GetComponent<Image>();
       costprice_txt = mTransform.Find("copperbutton_obj/Image/money_img/costprice_txt").GetComponent<Text>();
       coppersure_btn = mTransform.Find("copperbutton_obj/coppersure_btn").GetComponent<Button>();
       goldbutton_obj = mTransform.Find("goldbutton_obj").gameObject;
       goldmoreone_btn = mTransform.Find("goldbutton_obj/goldmoreone_btn").GetComponent<Button>();
       goldmoney_img = mTransform.Find("goldbutton_obj/Image/goldmoney_img").GetComponent<Image>();
       goldcostprice_txt = mTransform.Find("goldbutton_obj/Image/goldmoney_img/goldcostprice_txt").GetComponent<Text>();
       goldsure_btn = mTransform.Find("goldbutton_obj/goldsure_btn").GetComponent<Button>();
       unitcopperbutton_obj = mTransform.Find("unitcopperbutton_obj").gameObject;
       unitcoppersure_btn = mTransform.Find("unitcopperbutton_obj/unitcoppersure_btn").GetComponent<Button>();
       unitmoreone_btn = mTransform.Find("unitcopperbutton_obj/unitmoreone_btn").GetComponent<Button>();
       unitmoney_img = mTransform.Find("unitcopperbutton_obj/Image/unitmoney_img").GetComponent<Image>();
       unitcostprice_txt = mTransform.Find("unitcopperbutton_obj/Image/unitmoney_img/unitcostprice_txt").GetComponent<Text>();
    }
}