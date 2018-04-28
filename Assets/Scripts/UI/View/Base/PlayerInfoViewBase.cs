using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Button close_btn;
    [HideInInspector]
    public GameObject infos_obj;
    [HideInInspector]
    public Image head_img;
    [HideInInspector]
    public Text player_name_txt;
    [HideInInspector]
    public Text player_level_txt;
    [HideInInspector]
    public Text hascard_txt;
    [HideInInspector]
    public Text donate_txt;
    [HideInInspector]
    public Text oltime_txt;
    [HideInInspector]
    public Button watchinfo_btn;
    [HideInInspector]
    public Button startchat_btn;
    [HideInInspector]
    public Button makefriends_btn;
    [HideInInspector]
    public Button sendcard_btn;
    [HideInInspector]
    public Button playkick_btn;
    [HideInInspector]
    public Button worship_btn;
    [HideInInspector]
    public GameObject corps_obj;
    [HideInInspector]
    public Button revalue_btn;
    [HideInInspector]
    public Button relegate_btn;
    [HideInInspector]
    public Button setleader_btn;
    [HideInInspector]
    public Button kickoff_btn;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       close_btn = mTransform.Find("close_btn").GetComponent<Button>();
       infos_obj = mTransform.Find("infos_obj").gameObject;
       head_img = mTransform.Find("infos_obj/head_img").GetComponent<Image>();
       player_name_txt = mTransform.Find("infos_obj/player_name_txt").GetComponent<Text>();
       player_level_txt = mTransform.Find("infos_obj/player_level_txt").GetComponent<Text>();
       hascard_txt = mTransform.Find("infos_obj/hascard_txt").GetComponent<Text>();
       donate_txt = mTransform.Find("infos_obj/donate_txt").GetComponent<Text>();
       oltime_txt = mTransform.Find("infos_obj/oltime_txt").GetComponent<Text>();
       watchinfo_btn = mTransform.Find("infos_obj/watchinfo_btn").GetComponent<Button>();
       startchat_btn = mTransform.Find("infos_obj/startchat_btn").GetComponent<Button>();
       makefriends_btn = mTransform.Find("infos_obj/makefriends_btn").GetComponent<Button>();
       sendcard_btn = mTransform.Find("infos_obj/sendcard_btn").GetComponent<Button>();
       playkick_btn = mTransform.Find("infos_obj/playkick_btn").GetComponent<Button>();
       worship_btn = mTransform.Find("infos_obj/worship_btn").GetComponent<Button>();
       corps_obj = mTransform.Find("infos_obj/corps_obj").gameObject;
       revalue_btn = mTransform.Find("infos_obj/corps_obj/revalue_btn").GetComponent<Button>();
       relegate_btn = mTransform.Find("infos_obj/corps_obj/relegate_btn").GetComponent<Button>();
       setleader_btn = mTransform.Find("infos_obj/corps_obj/setleader_btn").GetComponent<Button>();
       kickoff_btn = mTransform.Find("infos_obj/corps_obj/kickoff_btn").GetComponent<Button>();
    }
}