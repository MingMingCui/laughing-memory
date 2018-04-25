using UnityEngine;
using UnityEngine.UI;

public class RecruitingViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Button ordinary_btn;
    [HideInInspector]
    public Text ordinaryfreetiems_txt;
    [HideInInspector]
    public Button  probability_btn;
    [HideInInspector]
    public GameObject probability_obj;
    [HideInInspector]
    public GameObject allhero_obj;
    [HideInInspector]
    public GameObject hero_obj;
    [HideInInspector]
    public Button closeprobability_btn;
    [HideInInspector]
    public GameObject ordinarytalent_obj;
    [HideInInspector]
    public Button ordinaryclose_btn;
    [HideInInspector]
    public Text ordinarycountdown_txt;
    [HideInInspector]
    public Text ordinarybuyoneprice_txt;
    [HideInInspector]
    public Button ordinarybuyone_btn;
    [HideInInspector]
    public Text buy_txt;
    [HideInInspector]
    public Text ordinarybuytenprice_txt;
    [HideInInspector]
    public Button ordinarybuyten_btn;
    [HideInInspector]
    public Button highgradetalent_btn;
    [HideInInspector]
    public Text highgradefreetimes_txt;
    [HideInInspector]
    public GameObject highgradetalent_obj;
    [HideInInspector]
    public Button highgradeclose_btn;
    [HideInInspector]
    public Text highgradecountdown_txt;
    [HideInInspector]
    public Text highgradebuyoneprice_txt;
    [HideInInspector]
    public Button highgradebuyone_btn;
    [HideInInspector]
    public Text highgradebuy_txt;
    [HideInInspector]
    public Text highgradebuytenprice_txt;
    [HideInInspector]
    public Button highgradebuyten_btn;
    [HideInInspector]
    public GameObject buyone_obj;
    [HideInInspector]
    public Image buyitemlevel_img;
    [HideInInspector]
    public Image buyitem_img;
    [HideInInspector]
    public Text buyitemname_txt;
    [HideInInspector]
    public Text buyitemnum_txt;
    [HideInInspector]
    public GameObject sendoneitem_obj;
    [HideInInspector]
    public GameObject sendtenitem_obj;
    [HideInInspector]
    public GameObject button_obj;
    [HideInInspector]
    public Button sure_btn;
    [HideInInspector]
    public Button moreone_btn;
    [HideInInspector]
    public Image money_img;
    [HideInInspector]
    public Text costprice_txt;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       ordinary_btn = mTransform.Find("ordinary_btn").GetComponent<Button>();
       ordinaryfreetiems_txt = mTransform.Find("ordinary_btn/ordinaryfreetiems_txt").GetComponent<Text>();
        probability_btn = mTransform.Find(" probability_btn").GetComponent<Button>();
       probability_obj = mTransform.Find("probability_obj").gameObject;
       allhero_obj = mTransform.Find("probability_obj/Image (1)/allhero_obj").gameObject;
       hero_obj = mTransform.Find("probability_obj/Image (1)/allhero_obj/hero_obj").gameObject;
       closeprobability_btn = mTransform.Find("probability_obj/closeprobability_btn").GetComponent<Button>();
       ordinarytalent_obj = mTransform.Find("ordinarytalent_obj").gameObject;
       ordinaryclose_btn = mTransform.Find("ordinarytalent_obj/ordinaryclose_btn").GetComponent<Button>();
       ordinarycountdown_txt = mTransform.Find("ordinarytalent_obj/ordinarycountdown_txt").GetComponent<Text>();
       ordinarybuyoneprice_txt = mTransform.Find("ordinarytalent_obj/Image/ordinarybuyoneprice_txt").GetComponent<Text>();
       ordinarybuyone_btn = mTransform.Find("ordinarytalent_obj/ordinarybuyone_btn").GetComponent<Button>();
       buy_txt = mTransform.Find("ordinarytalent_obj/ordinarybuyone_btn/buy_txt").GetComponent<Text>();
       ordinarybuytenprice_txt = mTransform.Find("ordinarytalent_obj/Image (1)/ordinarybuytenprice_txt").GetComponent<Text>();
       ordinarybuyten_btn = mTransform.Find("ordinarytalent_obj/ordinarybuyten_btn").GetComponent<Button>();
       highgradetalent_btn = mTransform.Find("highgradetalent_btn").GetComponent<Button>();
       highgradefreetimes_txt = mTransform.Find("highgradetalent_btn/highgradefreetimes_txt").GetComponent<Text>();
       highgradetalent_obj = mTransform.Find("highgradetalent_obj").gameObject;
       highgradeclose_btn = mTransform.Find("highgradetalent_obj/highgradeclose_btn").GetComponent<Button>();
       highgradecountdown_txt = mTransform.Find("highgradetalent_obj/highgradecountdown_txt").GetComponent<Text>();
       highgradebuyoneprice_txt = mTransform.Find("highgradetalent_obj/Image/highgradebuyoneprice_txt").GetComponent<Text>();
       highgradebuyone_btn = mTransform.Find("highgradetalent_obj/highgradebuyone_btn").GetComponent<Button>();
       highgradebuy_txt = mTransform.Find("highgradetalent_obj/highgradebuyone_btn/highgradebuy_txt").GetComponent<Text>();
       highgradebuytenprice_txt = mTransform.Find("highgradetalent_obj/Image (1)/highgradebuytenprice_txt").GetComponent<Text>();
       highgradebuyten_btn = mTransform.Find("highgradetalent_obj/highgradebuyten_btn").GetComponent<Button>();
       buyone_obj = mTransform.Find("buyone_obj").gameObject;
       buyitemlevel_img = mTransform.Find("buyone_obj/buyitemlevel_img").GetComponent<Image>();
       buyitem_img = mTransform.Find("buyone_obj/buyitemlevel_img/buyitem_img").GetComponent<Image>();
       buyitemname_txt = mTransform.Find("buyone_obj/buyitemlevel_img/buyitemname_txt").GetComponent<Text>();
       buyitemnum_txt = mTransform.Find("buyone_obj/buyitemlevel_img/buyitemnum_txt").GetComponent<Text>();
       sendoneitem_obj = mTransform.Find("buyone_obj/sendoneitem_obj").gameObject;
       sendtenitem_obj = mTransform.Find("buyone_obj/sendtenitem_obj").gameObject;
       button_obj = mTransform.Find("buyone_obj/button_obj").gameObject;
       sure_btn = mTransform.Find("buyone_obj/button_obj/sure_btn").GetComponent<Button>();
       moreone_btn = mTransform.Find("buyone_obj/button_obj/moreone_btn").GetComponent<Button>();
       money_img = mTransform.Find("buyone_obj/button_obj/money_img").GetComponent<Image>();
       costprice_txt = mTransform.Find("buyone_obj/button_obj/costprice_txt").GetComponent<Text>();
    }
}