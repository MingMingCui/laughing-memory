using UnityEngine;
using UnityEngine.UI;

public class SignInViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Text signInNum_txt;
    [HideInInspector]
    public Text GeneralName_txt;
    [HideInInspector]
    public Text Hint_txt;
    [HideInInspector]
    public Text quarry_txt;
    [HideInInspector]
    public GameObject AwardItem_obj;
    [HideInInspector]
    public Text Name_txt;
    [HideInInspector]
    public Button Specifier_btn;
    [HideInInspector]
    public GameObject Specifier_obj;
    [HideInInspector]
    public Button close_btn;
    [HideInInspector]
    public GameObject GetAward_obj;
    [HideInInspector]
    public Image itemlevelbg_img;
    [HideInInspector]
    public Image item_img;
    [HideInInspector]
    public Text num_txt;
    [HideInInspector]
    public Button Ensure_btn;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       signInNum_txt = mTransform.Find("signin/signInNum_txt").GetComponent<Text>();
       GeneralName_txt = mTransform.Find("SignInGeneral/GeneralName_txt").GetComponent<Text>();
       Hint_txt = mTransform.Find("SignInGeneral/Hint_txt").GetComponent<Text>();
        quarry_txt = mTransform.Find("SignInGeneral/quarry_txt").GetComponent<Text>();
       AwardItem_obj = mTransform.Find("SignInAward/AwardCase/AwardItem_obj").gameObject;
       Name_txt = mTransform.Find("Name_txt").GetComponent<Text>();
       Specifier_btn = mTransform.Find("Specifier_btn").GetComponent<Button>();
       Specifier_obj = mTransform.Find("Specifier_obj").gameObject;
       close_btn = mTransform.Find("Specifier_obj/close_btn").GetComponent<Button>();
       GetAward_obj = mTransform.Find("GetAward_obj").gameObject;
       itemlevelbg_img = mTransform.Find("GetAward_obj/itemlevelbg_img").GetComponent<Image>();
       item_img = mTransform.Find("GetAward_obj/itemlevelbg_img/item_img").GetComponent<Image>();
       num_txt = mTransform.Find("GetAward_obj/itemlevelbg_img/num_txt").GetComponent<Text>();
       Ensure_btn = mTransform.Find("GetAward_obj/Ensure_btn").GetComponent<Button>();
    }
}