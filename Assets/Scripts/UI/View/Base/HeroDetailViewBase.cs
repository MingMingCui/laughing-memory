using UnityEngine;
using UnityEngine.UI;

public class HeroDetailViewBase : UIViewBase
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public GameObject SideBackGround_obj;
    [HideInInspector]
    public Image card_img;
    [HideInInspector]
    public Image officer_img;
    [HideInInspector]
    public Image divination_img;
    [HideInInspector]
    public Image skill_img;
    [HideInInspector]
    public Image relation_img;
    [HideInInspector]
    public Image detail_img;
    [HideInInspector]
    public Transform Right_trf;
    [HideInInspector]
    public Transform Card_trf;
    [HideInInspector]
    public Image Floor_img;
    [HideInInspector]
    public Image bigcard_img;
    [HideInInspector]
    public Image Trans_img;
    [HideInInspector]
    public GameObject Star_obj;
    [HideInInspector]
    public Image Worker_img;
    [HideInInspector]
    public Slider Piece_slider;
    [HideInInspector]
    public Button StarUp_btn;
    [HideInInspector]
    public Text Num_txt;
    [HideInInspector]
    public ScrollRect OfficerSV_sr;
    [HideInInspector]
    public Transform Officer_trf;
    [HideInInspector]
    public GameObject OfficerList_obj;
    [HideInInspector]
    public Transform Divination_trf;
    [HideInInspector]
    public Button gotoDiv_btn;
    [HideInInspector]
    public ScrollRect divination_sr;
    [HideInInspector]
    public Transform divparent_trf;
    [HideInInspector]
    public Transform Skill_trf;
    [HideInInspector]
    public Image skill1_img;
    [HideInInspector]
    public Image skill2_img;
    [HideInInspector]
    public Image skill3_img;
    [HideInInspector]
    public Image skill4_img;
    [HideInInspector]
    public Transform click_trf;
    [HideInInspector]
    public Text skillname_txt;
    [HideInInspector]
    public Text skilllv_txt;
    [HideInInspector]
    public Text condition_txt;
    [HideInInspector]
    public Text tell_txt;
    [HideInInspector]
    public Button lvup_btn;
    [HideInInspector]
    public Text expcost_txt;
    [HideInInspector]
    public Text moneycost_txt;
    [HideInInspector]
    public Transform Relation_trf;
    [HideInInspector]
    public Transform Detail_trf;
    [HideInInspector]
    public Text HeroName_txt;
    [HideInInspector]
    public Text Tongshuai_txt;
    [HideInInspector]
    public Text Zhili_txt;
    [HideInInspector]
    public Text Wuli_txt;
    [HideInInspector]
    public ScrollRect attrDetail_sr;
    [HideInInspector]
    public Transform detailtxt_trf;
    [HideInInspector]
    public GameObject Left_obj;
    [HideInInspector]
    public GameObject hero_obj;
    [HideInInspector]
    public GameObject Equip_obj;
    [HideInInspector]
    public GameObject Divination_obj;
    [HideInInspector]
    public Image ddBG_img;
    [HideInInspector]
    public Text show_txt;
    [HideInInspector]
    public Button ddArrow_btn;
    [HideInInspector]
    public GameObject Template_obj;
    [HideInInspector]
    public GameObject DownContent_obj;
    [HideInInspector]
    public Text divi_txt;
    [HideInInspector]
    public Text Power_txt;
    [HideInInspector]
    public Button Dress_btn;
    [HideInInspector]
    public Button Intensify_btn;
    [HideInInspector]
    public Button takeoff_btn;
    [HideInInspector]
    public Button take_btn;
    [HideInInspector]
    public Text Name_txt;
    [HideInInspector]
    public Slider Exp_slider;
    [HideInInspector]
    public Text Exp_txt;
    [HideInInspector]
    public Text Level_txt;
    [HideInInspector]
    public Text LeftLevel_txt;
    [HideInInspector]
    public Button Add_btn;
    [HideInInspector]
    public GameObject LOfficer_obj;
    [HideInInspector]
    public Image currentleft_img;
    [HideInInspector]
    public Image currentright_img;
    [HideInInspector]
    public Text current_txt;
    [HideInInspector]
    public Image upleft_img;
    [HideInInspector]
    public Image upright_img;
    [HideInInspector]
    public Text up_txt;
    [HideInInspector]
    public Text needhonor_txt;
    [HideInInspector]
    public Button up_btn;
    [HideInInspector]
    public Image honoradd_img;
    [HideInInspector]
    public Transform LeftHead_trf;
    [HideInInspector]
    public Transform RightHea_trf;
    [HideInInspector]
    public Text leftpro_txt;
    [HideInInspector]
    public Text rightpro_txt;
    [HideInInspector]
    public GameObject Scroll_obj;
    [HideInInspector]
    public Transform sideselect_trf;
    [HideInInspector]
    public GameObject LeftContent_obj;
    [HideInInspector]
    public Transform Hide_trf;
    public virtual void Awake()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
        SideBackGround_obj = mTransform.Find("SideBackGround_obj").gameObject;
        card_img = mTransform.Find("SideBackGround_obj/card_img").GetComponent<Image>();
        officer_img = mTransform.Find("SideBackGround_obj/officer_img").GetComponent<Image>();
        divination_img = mTransform.Find("SideBackGround_obj/divination_img").GetComponent<Image>();
        skill_img = mTransform.Find("SideBackGround_obj/skill_img").GetComponent<Image>();
        relation_img = mTransform.Find("SideBackGround_obj/relation_img").GetComponent<Image>();
        detail_img = mTransform.Find("SideBackGround_obj/detail_img").GetComponent<Image>();
        Right_trf = mTransform.Find("Right_trf").GetComponent<Transform>();
        Card_trf = mTransform.Find("Right_trf/Card_trf").GetComponent<Transform>();
        Floor_img = mTransform.Find("Right_trf/Card_trf/Floor_img").GetComponent<Image>();
        bigcard_img = mTransform.Find("Right_trf/Card_trf/bigcard_img").GetComponent<Image>();
        Trans_img = mTransform.Find("Right_trf/Card_trf/Trans_img").GetComponent<Image>();
        Star_obj = mTransform.Find("Right_trf/Card_trf/Star_obj").gameObject;
        Worker_img = mTransform.Find("Right_trf/Card_trf/Worker_img").GetComponent<Image>();
        Piece_slider = mTransform.Find("Right_trf/Card_trf/Piece_slider").GetComponent<Slider>();
        StarUp_btn = mTransform.Find("Right_trf/Card_trf/StarUp_btn").GetComponent<Button>();
        Num_txt = mTransform.Find("Right_trf/Card_trf/Num_txt").GetComponent<Text>();
        OfficerSV_sr = mTransform.Find("Right_trf/Officer_trf/OfficerSV_sr").GetComponent<ScrollRect>();
        Officer_trf = mTransform.Find("Right_trf/Officer_trf").GetComponent<Transform>();
        OfficerList_obj = mTransform.Find("Right_trf/Officer_trf/OfficerSV_sr/Viewport/OfficerList_obj").gameObject;
        Divination_trf = mTransform.Find("Right_trf/Divination_trf").GetComponent<Transform>();
        divination_sr = mTransform.Find("Right_trf/Divination_trf/divination_sr").GetComponent<ScrollRect>();
        divparent_trf = mTransform.Find("Right_trf/Divination_trf/divination_sr/Viewport/divparent_trf").GetComponent<Transform>();
        Skill_trf = mTransform.Find("Right_trf/Skill_trf").GetComponent<Transform>();
        skill1_img = mTransform.Find("Right_trf/Skill_trf/Skills/skill1_img").GetComponent<Image>();
        skill2_img = mTransform.Find("Right_trf/Skill_trf/Skills/skill2_img").GetComponent<Image>();
        skill3_img = mTransform.Find("Right_trf/Skill_trf/Skills/skill3_img").GetComponent<Image>();
        skill4_img = mTransform.Find("Right_trf/Skill_trf/Skills/skill4_img").GetComponent<Image>();
        click_trf = mTransform.Find("Right_trf/Skill_trf/Skills/click_trf").GetComponent<Transform>();
        skillname_txt = mTransform.Find("Right_trf/Skill_trf/skillname_txt").GetComponent<Text>();
        skilllv_txt = mTransform.Find("Right_trf/Skill_trf/skilllv/skilllv_txt").GetComponent<Text>();
        condition_txt = mTransform.Find("Right_trf/Skill_trf/condition/condition_txt").GetComponent<Text>();
        tell_txt = mTransform.Find("Right_trf/Skill_trf/tell_txt").GetComponent<Text>();
        lvup_btn = mTransform.Find("Right_trf/Skill_trf/lvup_btn").GetComponent<Button>();
        expcost_txt = mTransform.Find("Right_trf/Skill_trf/Txt/expcost_txt").GetComponent<Text>();
        moneycost_txt = mTransform.Find("Right_trf/Skill_trf/Txt/moneycost_txt").GetComponent<Text>();
        Relation_trf = mTransform.Find("Right_trf/Relation_trf").GetComponent<Transform>();
        Detail_trf = mTransform.Find("Right_trf/Detail_trf").GetComponent<Transform>();
        HeroName_txt = mTransform.Find("Right_trf/Detail_trf/HeroName_txt").GetComponent<Text>();
        Tongshuai_txt = mTransform.Find("Right_trf/Detail_trf/Image/Text/Tongshuai_txt").GetComponent<Text>();
        Zhili_txt = mTransform.Find("Right_trf/Detail_trf/Image/Text/Zhili_txt").GetComponent<Text>();
        Wuli_txt = mTransform.Find("Right_trf/Detail_trf/Image/Text/Wuli_txt").GetComponent<Text>();
        gotoDiv_btn = mTransform.Find("Right_trf/Divination_trf/gotoDiv_btn").GetComponent<Button>();
        attrDetail_sr = mTransform.Find("Right_trf/Detail_trf/attrDetail_sr").GetComponent<ScrollRect>();
        detailtxt_trf = mTransform.Find("Right_trf/Detail_trf/attrDetail_sr/Viewport/detailtxt_trf").transform;
        Left_obj = mTransform.Find("Left_obj").gameObject;
        hero_obj = mTransform.Find("Left_obj/hero_obj").gameObject;
        Equip_obj = mTransform.Find("Left_obj/Equip_obj").gameObject;
        Divination_obj = mTransform.Find("Left_obj/Divination_obj").gameObject;
        ddBG_img = mTransform.Find("Left_obj/ddBG_img").GetComponent<Image>();
        show_txt = mTransform.Find("Left_obj/ddBG_img/show_txt").GetComponent<Text>();
        ddArrow_btn = mTransform.Find("Left_obj/ddBG_img/ddArrow_btn").GetComponent<Button>();
        Template_obj = mTransform.Find("Left_obj/Template_obj").gameObject;
        DownContent_obj = mTransform.Find("Left_obj/Template_obj/Viewport/DownContent_obj").gameObject;
        divi_txt = mTransform.Find("Left_obj/Template_obj/Viewport/DownContent_obj/divi_txt").GetComponent<Text>();
        Power_txt = mTransform.Find("Left_obj/Power/Power_txt").GetComponent<Text>();
        Dress_btn = mTransform.Find("Left_obj/Equip_obj/Dress_btn").GetComponent<Button>();
        Intensify_btn = mTransform.Find("Left_obj/Equip_obj/Intensify_btn").GetComponent<Button>();
        takeoff_btn = mTransform.Find("Left_obj/Divination_obj/takeoff_btn").GetComponent<Button>();
        take_btn = mTransform.Find("Left_obj/Divination_obj/take_btn").GetComponent<Button>();
        Name_txt = mTransform.Find("Left_obj/Name_txt").GetComponent<Text>();
        Exp_slider = mTransform.Find("Left_obj/Exp_slider").GetComponent<Slider>();
        Exp_txt = mTransform.Find("Left_obj/Exp_slider/Exp_txt").GetComponent<Text>();
        LeftLevel_txt = mTransform.Find("Left_obj/Level/LeftLevel_txt").GetComponent<Text>();
        Add_btn = mTransform.Find("Left_obj/Add_btn").GetComponent<Button>();
        LOfficer_obj = mTransform.Find("LOfficer_obj").gameObject;
        currentleft_img = mTransform.Find("LOfficer_obj/currentleft_img").GetComponent<Image>();
        currentright_img = mTransform.Find("LOfficer_obj/currentright_img").GetComponent<Image>();
        current_txt = mTransform.Find("LOfficer_obj/current_txt").GetComponent<Text>();
        upleft_img = mTransform.Find("LOfficer_obj/upleft_img").GetComponent<Image>();
        upright_img = mTransform.Find("LOfficer_obj/upright_img").GetComponent<Image>();
        up_txt = mTransform.Find("LOfficer_obj/up_txt").GetComponent<Text>();
        needhonor_txt = mTransform.Find("LOfficer_obj/Image (4)/needhonor_txt").GetComponent<Text>();
        up_btn = mTransform.Find("LOfficer_obj/up_btn").GetComponent<Button>();
        honoradd_img = mTransform.Find("LOfficer_obj/honoradd_img").GetComponent<Image>();
        LeftHead_trf = mTransform.Find("LOfficer_obj/LeftHead_trf").GetComponent<Transform>();
        RightHea_trf = mTransform.Find("LOfficer_obj/RightHea_trf").GetComponent<Transform>();
        Level_txt = mTransform.Find("LOfficer_obj/RightHea_trf/Level_obj/Level_txt").GetComponent<Text>();
        leftpro_txt = mTransform.Find("LOfficer_obj/leftpro_txt").GetComponent<Text>();
        rightpro_txt = mTransform.Find("LOfficer_obj/rightpro_txt").GetComponent<Text>();
        Scroll_obj = mTransform.Find("Scroll_obj").gameObject;
        sideselect_trf = mTransform.Find("Scroll_obj/sideselect_trf").GetComponent<Transform>();
        LeftContent_obj = mTransform.Find("Scroll_obj/ViewPort/LeftContent_obj").gameObject;
        Hide_trf = mTransform.Find("Hide_trf").GetComponent<Transform>();
    }
}