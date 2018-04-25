using UnityEngine;
using UnityEngine.UI;

public class BattleViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public GameObject Transcript_obj;
    [HideInInspector]
    public GameObject commonBack_obj;
    [HideInInspector]
    public GameObject eliteBack_obj;
    [HideInInspector]
    public GameObject epicBack_obj;
    [HideInInspector]
    public GameObject arroParent_obj;
    [HideInInspector]
    public GameObject Starlevel_obj;
    [HideInInspector]
    public Slider Slider_slider;
    [HideInInspector]
    public GameObject Open_4_obj;
    [HideInInspector]
    public GameObject Lock_4_obj;
    [HideInInspector]
    public Button Clear_4_btn;
    [HideInInspector]
    public GameObject Open_8_obj;
    [HideInInspector]
    public GameObject Lock_8_obj;
    [HideInInspector]
    public Button Clear_8_btn;
    [HideInInspector]
    public GameObject Open_12_obj;
    [HideInInspector]
    public GameObject Lock_12_obj;
    [HideInInspector]
    public Button Clear_12_btn;
    [HideInInspector]
    public Text Existing_txt;
    [HideInInspector]
    public Text Sum_txt;
    [HideInInspector]
    public Toggle epic_tog;
    [HideInInspector]
    public Toggle elite_tog;
    [HideInInspector]
    public Toggle common_tog;
    [HideInInspector]
    public Button quit_btn;
    [HideInInspector]
    public Button front_btn;
    [HideInInspector]
    public Button queen_btn;
    [HideInInspector]
    public GameObject StarLevelAward_obj;
    [HideInInspector]
    public GameObject MoppingUp_obj;
    [HideInInspector]
    public Text MoppingUpTime_txt;
    [HideInInspector]
    public Text expCount_txt;
    [HideInInspector]
    public Text goldCout_txt;
    [HideInInspector]
    public GameObject MoppingUpdropOut_obj;
    [HideInInspector]
    public Image icon_img;
    [HideInInspector]
    public Button frame_btn;
    [HideInInspector]
    public GameObject Accomplish_obj;
    [HideInInspector]
    public GameObject Premiums_obj;


    [HideInInspector]
    public Image expect_img;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       Transcript_obj = mTransform.Find("Transcript_obj").gameObject;
       commonBack_obj = mTransform.Find("Transcript_obj/battleMap/commonBack_obj").gameObject;
       eliteBack_obj = mTransform.Find("Transcript_obj/battleMap/eliteBack_obj").gameObject;
       epicBack_obj = mTransform.Find("Transcript_obj/battleMap/epicBack_obj").gameObject;
        arroParent_obj = mTransform.Find("Transcript_obj/battleMap/arroParent_obj").gameObject;
       Starlevel_obj = mTransform.Find("Transcript_obj/battleMap/Starlevel_obj").gameObject;
       Slider_slider = mTransform.Find("Transcript_obj/battleMap/Starlevel_obj/Slider_slider").GetComponent<Slider>();
       Open_4_obj = mTransform.Find("Transcript_obj/battleMap/Starlevel_obj/Star_4/Open_4_obj").gameObject;
       Lock_4_obj = mTransform.Find("Transcript_obj/battleMap/Starlevel_obj/Star_4/Lock_4_obj").gameObject;
       Clear_4_btn = mTransform.Find("Transcript_obj/battleMap/Starlevel_obj/Star_4/Clear_4_btn").GetComponent<Button>();
       Open_8_obj = mTransform.Find("Transcript_obj/battleMap/Starlevel_obj/Star_8/Open_8_obj").gameObject;
       Lock_8_obj = mTransform.Find("Transcript_obj/battleMap/Starlevel_obj/Star_8/Lock_8_obj").gameObject;
       Clear_8_btn = mTransform.Find("Transcript_obj/battleMap/Starlevel_obj/Star_8/Clear_8_btn").GetComponent<Button>();
       Open_12_obj = mTransform.Find("Transcript_obj/battleMap/Starlevel_obj/Star_12/Open_12_obj").gameObject;
       Lock_12_obj = mTransform.Find("Transcript_obj/battleMap/Starlevel_obj/Star_12/Lock_12_obj").gameObject;
       Clear_12_btn = mTransform.Find("Transcript_obj/battleMap/Starlevel_obj/Star_12/Clear_12_btn").GetComponent<Button>();
       Existing_txt = mTransform.Find("Transcript_obj/battleMap/Starlevel_obj/Quantity/Existing_txt").GetComponent<Text>();
       Sum_txt = mTransform.Find("Transcript_obj/battleMap/Starlevel_obj/Quantity/Sum_txt").GetComponent<Text>();
       epic_tog = mTransform.Find("Transcript_obj/epic_tog").GetComponent<Toggle>();
       elite_tog = mTransform.Find("Transcript_obj/elite_tog").GetComponent<Toggle>();
       common_tog = mTransform.Find("Transcript_obj/common_tog").GetComponent<Toggle>();
       quit_btn = mTransform.Find("Transcript_obj/quit_btn").GetComponent<Button>();
       front_btn = mTransform.Find("Transcript_obj/front_btn").GetComponent<Button>();
       queen_btn = mTransform.Find("Transcript_obj/queen_btn").GetComponent<Button>();
       StarLevelAward_obj = mTransform.Find("StarLevelAward_obj").gameObject;
       MoppingUp_obj = mTransform.Find("MoppingUp_obj").gameObject;
       MoppingUpTime_txt = mTransform.Find("MoppingUp_obj/back/headBack/MoppingUpTime_txt").GetComponent<Text>();
       expCount_txt = mTransform.Find("MoppingUp_obj/back/dropOutExp/expCount_txt").GetComponent<Text>();
       goldCout_txt = mTransform.Find("MoppingUp_obj/back/dropOutExp/goldCout_txt").GetComponent<Text>();
       MoppingUpdropOut_obj = mTransform.Find("MoppingUp_obj/back/MoppingUpdropOut_obj").gameObject;
       icon_img = mTransform.Find("MoppingUp_obj/back/MoppingUpdropOut_obj/DropOutItem/icon_img").GetComponent<Image>();
       frame_btn = mTransform.Find("MoppingUp_obj/back/MoppingUpdropOut_obj/DropOutItem/frame_btn").GetComponent<Button>();
       Accomplish_obj = mTransform.Find("MoppingUp_obj/back/Accomplish_obj").gameObject;
       Premiums_obj = mTransform.Find("MoppingUp_obj/back/Premiums_obj").gameObject;
       icon_img = mTransform.Find("MoppingUp_obj/back/Premiums_obj/DropOutItem/icon_img").GetComponent<Image>();
       frame_btn = mTransform.Find("MoppingUp_obj/back/Premiums_obj/DropOutItem/frame_btn").GetComponent<Button>();
       expect_img = mTransform.Find("expect_img").GetComponent<Image>();
    }
}