using UnityEngine;
using UnityEngine.UI;

public class PostBoxViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public ScrollRect Control_sr;
    [HideInInspector]
    public Transform show_trf;
    [HideInInspector]
    public Button Close_btn;
    [HideInInspector]
    public Toggle totala_tog;
    [HideInInspector]
    public Toggle noticea_tog;
    [HideInInspector]
    public Toggle awarda_tog;
    [HideInInspector]
    public Toggle lettera_tog;
    [HideInInspector]
    public Button MailaWard_btn;
    [HideInInspector]
    public GameObject Content_obj;
    [HideInInspector]
    public Text MailWardName_txt;
    [HideInInspector]
    public Text MailWardContent_txt;
    [HideInInspector]
    public Text Addresser_txt;
    [HideInInspector]
    public GameObject Adjunct_obj;
    [HideInInspector]
    public Transform Currency_trf;
    [HideInInspector]
    public Transform MailItem_trf;
    [HideInInspector]
    public Button Draw_btn;
    [HideInInspector]
    public Button Get_btn;
    [HideInInspector]
    public Button deletea_btn;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       Control_sr = mTransform.Find("bj/Control_sr").GetComponent<ScrollRect>();
       show_trf = mTransform.Find("bj/Control_sr/show_trf").GetComponent<Transform>();
       Close_btn = mTransform.Find("Close_btn").GetComponent<Button>();
       totala_tog = mTransform.Find("Button/totala_tog").GetComponent<Toggle>();
       noticea_tog = mTransform.Find("Button/noticea_tog").GetComponent<Toggle>();
       awarda_tog = mTransform.Find("Button/awarda_tog").GetComponent<Toggle>();
       lettera_tog = mTransform.Find("Button/lettera_tog").GetComponent<Toggle>();
       MailaWard_btn = mTransform.Find("MailaWard_btn").GetComponent<Button>();
       Content_obj = mTransform.Find("MailaWard_btn/Show/Content_obj").gameObject;
       MailWardName_txt = mTransform.Find("MailaWard_btn/Show/Content_obj/di/MailWardName_txt").GetComponent<Text>();
       MailWardContent_txt = mTransform.Find("MailaWard_btn/Show/Content_obj/Content/MailWardContent_txt").GetComponent<Text>();
       Addresser_txt = mTransform.Find("MailaWard_btn/Show/Content_obj/Content/Addresser_txt").GetComponent<Text>();
       Adjunct_obj = mTransform.Find("MailaWard_btn/Show/Content_obj/Content/Adjunct_obj").gameObject;
       Currency_trf = mTransform.Find("MailaWard_btn/Show/Content_obj/Content/Adjunct_obj/Currency_trf").GetComponent<Transform>();
       MailItem_trf = mTransform.Find("MailaWard_btn/Show/Content_obj/Content/Adjunct_obj/MailItem_trf").GetComponent<Transform>();
       Draw_btn = mTransform.Find("MailaWard_btn/Draw_btn").GetComponent<Button>();
       Get_btn = mTransform.Find("MailaWard_btn/Get_btn").GetComponent<Button>();
       deletea_btn = mTransform.Find("MailaWard_btn/deletea_btn").GetComponent<Button>();
    }
}