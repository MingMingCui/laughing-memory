using UnityEngine;
using UnityEngine.UI;

public class RecruitingViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Button ordinary_btn;
    [HideInInspector]
    public GameObject ordinarytalent_obj;
    [HideInInspector]
    public Text num_txt;
    [HideInInspector]
    public Button ordinaryclose_btn;
    [HideInInspector]
    public Text ordinarycountdown_txt;
    [HideInInspector]
    public Text ordinarybuyoneprice_txt;
    [HideInInspector]
    public Button ordinarybuyone_btn;
    [HideInInspector]
    public Text ordinarybuytenprice_txt;
    [HideInInspector]
    public Button ordinarybuyten_btn;
    [HideInInspector]
    public Button highgradetalent_btn;
    [HideInInspector]
    public GameObject highgradetalent_obj;

    [HideInInspector]
    public Text highgradebuyoneprice_txt;
    [HideInInspector]
    public Text highgradecountdown_txt;
    [HideInInspector]
    public Button highgradebuyone_btn;
    [HideInInspector]
    public Text highgradebuytenprice_txt;
    [HideInInspector]
    public Button highgradebuyten_btn;
    [HideInInspector]
    public Button  probability_btn;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       ordinary_btn = mTransform.Find("ordinary_btn").GetComponent<Button>();
       ordinarytalent_obj = mTransform.Find("ordinarytalent_obj").gameObject;
       num_txt = mTransform.Find("ordinarytalent_obj/Image (2)/ui_itemlevelbg/num_txt").GetComponent<Text>();
       ordinaryclose_btn = mTransform.Find("ordinarytalent_obj/ordinaryclose_btn").GetComponent<Button>();
       ordinarycountdown_txt = mTransform.Find("ordinarytalent_obj/ordinarycountdown_txt").GetComponent<Text>();
       ordinarybuyoneprice_txt = mTransform.Find("ordinarytalent_obj/Image/ordinarybuyoneprice_txt").GetComponent<Text>();
       ordinarybuyone_btn = mTransform.Find("ordinarytalent_obj/ordinarybuyone_btn").GetComponent<Button>();
       ordinarybuytenprice_txt = mTransform.Find("ordinarytalent_obj/Image (1)/ordinarybuytenprice_txt").GetComponent<Text>();
       ordinarybuyten_btn = mTransform.Find("ordinarytalent_obj/ordinarybuyten_btn").GetComponent<Button>();
       highgradetalent_btn = mTransform.Find("highgradetalent_btn").GetComponent<Button>();
       highgradetalent_obj = mTransform.Find("highgradetalent_obj").gameObject;
       num_txt = mTransform.Find("highgradetalent_obj/Image(2)/ui_itemlevelbg/num_txt").GetComponent<Text>();
       highgradebuyoneprice_txt = mTransform.Find("highgradetalent_obj/Image/highgradebuyoneprice_txt").GetComponent<Text>();
       highgradecountdown_txt = mTransform.Find("highgradetalent_obj/Image (3)/highgradecountdown_txt").GetComponent<Text>();
       highgradebuyone_btn = mTransform.Find("highgradetalent_obj/highgradebuyone_btn").GetComponent<Button>();
       highgradebuytenprice_txt = mTransform.Find("highgradetalent_obj/Image (1)/highgradebuytenprice_txt").GetComponent<Text>();
       highgradebuyten_btn = mTransform.Find("highgradetalent_obj/highgradebuyten_btn").GetComponent<Button>();
        probability_btn = mTransform.Find("highgradetalent_obj/ probability_btn").GetComponent<Button>();
    }
}