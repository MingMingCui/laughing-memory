using UnityEngine;
using UnityEngine.UI;

public class MajestyViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Image Frame_img;
    [HideInInspector]
    public Image Portrait_img;
    [HideInInspector]
    public Text Name_txt;
    [HideInInspector]
    public Button ChangeName_btn;
    [HideInInspector]
    public Button ChangePortrait_btn;
    [HideInInspector]
    public Button ChangeCircle_btn;
    [HideInInspector]
    public Button Logout_btn;
    [HideInInspector]
    public Button Settings_btn;
    [HideInInspector]
    public Text Level_txt;
    [HideInInspector]
    public Text Exp_txt;
    [HideInInspector]
    public Text AccountID_txt;
    [HideInInspector]
    public Text Servers_txt;
    [HideInInspector]
    public Text Corps_txt;
    [HideInInspector]
    public Text CorpsID_txt;
    [HideInInspector]
    public GameObject RawImage_obj;
    [HideInInspector]
    public GameObject ChangeName_obj;
    [HideInInspector]
    public InputField Name_input;
    [HideInInspector]
    public Text Placeholder_txt;
    [HideInInspector]
    public Button Random_btn;
    [HideInInspector]
    public Button Cancel_btn;
    [HideInInspector]
    public Button Alter_btn;
    [HideInInspector]
    public GameObject Portrait_obj;
    [HideInInspector]
    public Button zhezhao_btn;
    [HideInInspector]
    public Transform Mask_trf;
    [HideInInspector]
    public Transform Clear_trf;
    [HideInInspector]
    public Toggle Message_tog;
    [HideInInspector]
    public Image expect_img;
    [HideInInspector]
    public Toggle Reloading_tog;
    [HideInInspector]
    public GameObject Frame_obj;
    [HideInInspector]
    public Button Frame_btn;
    [HideInInspector]
    public GameObject Control_obj;
    [HideInInspector]
    public Transform Have_trf;
    [HideInInspector]
    public Transform HaveClear_trf;
    [HideInInspector]
    public Transform NotClear_trf;
    [HideInInspector]
    public Transform NotClears_trf;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       Frame_img = mTransform.Find("Image (1)/Frame_img").GetComponent<Image>();
       Portrait_img = mTransform.Find("Image (1)/Frame_img/Portrait_img").GetComponent<Image>();
       Name_txt = mTransform.Find("Image (1)/Name_txt").GetComponent<Text>();
       ChangeName_btn = mTransform.Find("Image (1)/ChangeName_btn").GetComponent<Button>();
       ChangePortrait_btn = mTransform.Find("Image (1)/ChangePortrait_btn").GetComponent<Button>();
       ChangeCircle_btn = mTransform.Find("Image (1)/ChangeCircle_btn").GetComponent<Button>();
       Logout_btn = mTransform.Find("Image (1)/Logout_btn").GetComponent<Button>();
       Settings_btn = mTransform.Find("Image (1)/Settings_btn").GetComponent<Button>();
       Level_txt = mTransform.Find("Image (1)/Image (1)/Level_txt").GetComponent<Text>();
       Exp_txt = mTransform.Find("Image (1)/Image (2)/Exp_txt").GetComponent<Text>();
       AccountID_txt = mTransform.Find("Image (1)/Image (3)/AccountID_txt").GetComponent<Text>();
       Servers_txt = mTransform.Find("Image (1)/Image (4)/Servers_txt").GetComponent<Text>();
       Corps_txt = mTransform.Find("Image (1)/Image (5)/Corps_txt").GetComponent<Text>();
       CorpsID_txt = mTransform.Find("Image (1)/Image (6)/CorpsID_txt").GetComponent<Text>();
       RawImage_obj = mTransform.Find("RawImage_obj").gameObject;
       ChangeName_obj = mTransform.Find("ChangeName_obj").gameObject;
       Name_input = mTransform.Find("ChangeName_obj/bj/Name_input").GetComponent<InputField>();
       Placeholder_txt = mTransform.Find("ChangeName_obj/bj/Name_input/Placeholder_txt").GetComponent<Text>();
       Random_btn = mTransform.Find("ChangeName_obj/bj/Random_btn").GetComponent<Button>();
       Cancel_btn = mTransform.Find("ChangeName_obj/bj/Cancel_btn").GetComponent<Button>();
       Alter_btn = mTransform.Find("ChangeName_obj/bj/Alter_btn").GetComponent<Button>();
       Portrait_obj = mTransform.Find("Portrait_obj").gameObject;
       zhezhao_btn = mTransform.Find("Portrait_obj/zhezhao_btn").GetComponent<Button>();
       Mask_trf = mTransform.Find("Portrait_obj/Slide/Mask_trf").GetComponent<Transform>();
       Clear_trf = mTransform.Find("Portrait_obj/Slide/Mask_trf/Clear_trf").GetComponent<Transform>();
       Message_tog = mTransform.Find("Message_tog").GetComponent<Toggle>();
       expect_img = mTransform.Find("expect_img").GetComponent<Image>();
       Reloading_tog = mTransform.Find("Reloading_tog").GetComponent<Toggle>();
       Frame_obj = mTransform.Find("Frame_obj").gameObject;
       Frame_btn = mTransform.Find("Frame_obj/Frame_btn").GetComponent<Button>();
       Control_obj = mTransform.Find("Frame_obj/Slide/Control_obj").gameObject;
       Have_trf = mTransform.Find("Frame_obj/Slide/Control_obj/Have_trf").GetComponent<Transform>();
       HaveClear_trf = mTransform.Find("Frame_obj/Slide/Control_obj/Have_trf/HaveClear_trf").GetComponent<Transform>();
       NotClear_trf = mTransform.Find("Frame_obj/Slide/Control_obj/Have_trf/NotClear_trf").GetComponent<Transform>();
       NotClears_trf = mTransform.Find("Frame_obj/Slide/Control_obj/Have_trf/NotClear_trf/NotClears_trf").GetComponent<Transform>();
    }
}