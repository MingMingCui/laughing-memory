using UnityEngine;
using UnityEngine.UI;

public class CorpsMainViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public GameObject chatacters1_obj;
    [HideInInspector]
    public GameObject characters1_obj;
    [HideInInspector]
    public GameObject characters2_obj;
    [HideInInspector]
    public GameObject chatacters2_obj;


    [HideInInspector]
    public GameObject chatacters3_obj;


    [HideInInspector]
    public GameObject member_obj;
    [HideInInspector]
    public Button memberlevel_btn;
    [HideInInspector]
    public Button memberjob_btn;
    [HideInInspector]
    public Button membervigour_btn;
    [HideInInspector]
    public Button memberdonate_btn;
    [HideInInspector]
    public Button memberlastol_btn;
    [HideInInspector]
    public RectTransform level_sort_rect;
    [HideInInspector]
    public RectTransform job_sort_rect;
    [HideInInspector]
    public RectTransform vigour_sort_rect;
    [HideInInspector]
    public RectTransform donate_sort_rect;
    [HideInInspector]
    public RectTransform lastol_sort_rect;
    [HideInInspector]
    public ScrollRect memberscroll_sr;
    [HideInInspector]
    public GameObject memberlist_obj;
    [HideInInspector]
    public GameObject situation_obj;
    [HideInInspector]
    public ScrollRect logs_sr;
    [HideInInspector]
    public GameObject logs_obj;
    [HideInInspector]
    public GameObject request_obj;
    [HideInInspector]
    public Text totalrequest_txt;
    [HideInInspector]
    public Text reqeust_members_txt;
    [HideInInspector]
    public ScrollRect requesters_sr;
    [HideInInspector]
    public GameObject requesters_obj;
    [HideInInspector]
    public GameObject info_obj;
    [HideInInspector]
    public GameObject baseinfo_obj;
    [HideInInspector]
    public Image flag_img;
    [HideInInspector]
    public Text level_txt;
    [HideInInspector]
    public Button mail_btn;
    [HideInInspector]
    public Image exp_img;
    [HideInInspector]
    public Text corpname_txt;
    [HideInInspector]
    public Text corprank_txt;
    [HideInInspector]
    public Text corpmember_txt;
    [HideInInspector]
    public Text corpid_txt;
    [HideInInspector]
    public Text corppower_txt;
    [HideInInspector]
    public Text corpleader_txt;
    [HideInInspector]
    public Text job_txt;
    [HideInInspector]
    public Text todayoffer_txt;
    [HideInInspector]
    public Text totaloffer_txt;
    [HideInInspector]
    public Button corpexit_btn;
    [HideInInspector]
    public Text declare_txt;
    [HideInInspector]
    public Button declaremod_btn;
    [HideInInspector]
    public Text notice_txt;
    [HideInInspector]
    public Button noticemod_btn;
    [HideInInspector]
    public Button corpsbattle_btn;
    [HideInInspector]
    public Button corpspass_btn;
    [HideInInspector]
    public Button corpboss_btn;
    [HideInInspector]
    public Button corpsworship_btn;
    [HideInInspector]
    public Button corpsshop_btn;
    [HideInInspector]
    public Button corpdonate_btn;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       chatacters1_obj = mTransform.Find("uI_bg/chatacters1_obj").gameObject;
       characters1_obj = mTransform.Find("uI_bg/chatacters1_obj/characters1_obj").gameObject;
       characters2_obj = mTransform.Find("uI_bg/chatacters1_obj/characters2_obj").gameObject;
       chatacters2_obj = mTransform.Find("uI_bg/chatacters2_obj").gameObject;
       characters1_obj = mTransform.Find("uI_bg/chatacters2_obj/characters1_obj").gameObject;
       characters2_obj = mTransform.Find("uI_bg/chatacters2_obj/characters2_obj").gameObject;
       chatacters3_obj = mTransform.Find("uI_bg/chatacters3_obj").gameObject;
       characters1_obj = mTransform.Find("uI_bg/chatacters3_obj/characters1_obj").gameObject;
       characters2_obj = mTransform.Find("uI_bg/chatacters3_obj/characters2_obj").gameObject;
       member_obj = mTransform.Find("tabobjs/member_obj").gameObject;
       memberlevel_btn = mTransform.Find("tabobjs/member_obj/memberlevel_btn").GetComponent<Button>();
       memberjob_btn = mTransform.Find("tabobjs/member_obj/memberjob_btn").GetComponent<Button>();
       membervigour_btn = mTransform.Find("tabobjs/member_obj/membervigour_btn").GetComponent<Button>();
       memberdonate_btn = mTransform.Find("tabobjs/member_obj/memberdonate_btn").GetComponent<Button>();
       memberlastol_btn = mTransform.Find("tabobjs/member_obj/memberlastol_btn").GetComponent<Button>();
       level_sort_rect = mTransform.Find("tabobjs/member_obj/level_sort_rect").GetComponent<RectTransform>();
       job_sort_rect = mTransform.Find("tabobjs/member_obj/job_sort_rect").GetComponent<RectTransform>();
       vigour_sort_rect = mTransform.Find("tabobjs/member_obj/vigour_sort_rect").GetComponent<RectTransform>();
       donate_sort_rect = mTransform.Find("tabobjs/member_obj/donate_sort_rect").GetComponent<RectTransform>();
       lastol_sort_rect = mTransform.Find("tabobjs/member_obj/lastol_sort_rect").GetComponent<RectTransform>();
       memberscroll_sr = mTransform.Find("tabobjs/member_obj/memberscroll_sr").GetComponent<ScrollRect>();
       memberlist_obj = mTransform.Find("tabobjs/member_obj/memberscroll_sr/viewport/memberlist_obj").gameObject;
       situation_obj = mTransform.Find("tabobjs/situation_obj").gameObject;
       logs_sr = mTransform.Find("tabobjs/situation_obj/logs_sr").GetComponent<ScrollRect>();
       logs_obj = mTransform.Find("tabobjs/situation_obj/logs_sr/viewport/logs_obj").gameObject;
       request_obj = mTransform.Find("tabobjs/request_obj").gameObject;
       totalrequest_txt = mTransform.Find("tabobjs/request_obj/totalrequest_txt").GetComponent<Text>();
       reqeust_members_txt = mTransform.Find("tabobjs/request_obj/reqeust_members_txt").GetComponent<Text>();
       requesters_sr = mTransform.Find("tabobjs/request_obj/requesters_sr").GetComponent<ScrollRect>();
       requesters_obj = mTransform.Find("tabobjs/request_obj/requesters_sr/viewport/requesters_obj").gameObject;
       info_obj = mTransform.Find("tabobjs/info_obj").gameObject;
       baseinfo_obj = mTransform.Find("tabobjs/info_obj/baseinfo_obj").gameObject;
       flag_img = mTransform.Find("tabobjs/info_obj/baseinfo_obj/flag/flag_img").GetComponent<Image>();
       level_txt = mTransform.Find("tabobjs/info_obj/baseinfo_obj/level_txt").GetComponent<Text>();
       mail_btn = mTransform.Find("tabobjs/info_obj/baseinfo_obj/mail_btn").GetComponent<Button>();
       exp_img = mTransform.Find("tabobjs/info_obj/baseinfo_obj/expbg/exp_img").GetComponent<Image>();
       corpname_txt = mTransform.Find("tabobjs/info_obj/baseinfo_obj/corpname/corpname_txt").GetComponent<Text>();
       corprank_txt = mTransform.Find("tabobjs/info_obj/baseinfo_obj/corprank/corprank_txt").GetComponent<Text>();
       corpmember_txt = mTransform.Find("tabobjs/info_obj/baseinfo_obj/corpmember/corpmember_txt").GetComponent<Text>();
       corpid_txt = mTransform.Find("tabobjs/info_obj/baseinfo_obj/corpid/corpid_txt").GetComponent<Text>();
       corppower_txt = mTransform.Find("tabobjs/info_obj/baseinfo_obj/corppower/corppower_txt").GetComponent<Text>();
       corpleader_txt = mTransform.Find("tabobjs/info_obj/baseinfo_obj/corpleader/corpleader_txt").GetComponent<Text>();
       job_txt = mTransform.Find("tabobjs/info_obj/baseinfo_obj/personal/job_txt").GetComponent<Text>();
       todayoffer_txt = mTransform.Find("tabobjs/info_obj/baseinfo_obj/personal/todayoffer_txt").GetComponent<Text>();
       totaloffer_txt = mTransform.Find("tabobjs/info_obj/baseinfo_obj/personal/totaloffer_txt").GetComponent<Text>();
       corpexit_btn = mTransform.Find("tabobjs/info_obj/baseinfo_obj/corpexit_btn").GetComponent<Button>();
       declare_txt = mTransform.Find("tabobjs/info_obj/declarebg/declare_txt").GetComponent<Text>();
       declaremod_btn = mTransform.Find("tabobjs/info_obj/declarebg/declaremod_btn").GetComponent<Button>();
       notice_txt = mTransform.Find("tabobjs/info_obj/noticebg/notice_txt").GetComponent<Text>();
       noticemod_btn = mTransform.Find("tabobjs/info_obj/noticebg/noticemod_btn").GetComponent<Button>();
       corpsbattle_btn = mTransform.Find("tabobjs/info_obj/corpsbattle_btn").GetComponent<Button>();
       corpspass_btn = mTransform.Find("tabobjs/info_obj/corpspass_btn").GetComponent<Button>();
       corpboss_btn = mTransform.Find("tabobjs/info_obj/corpboss_btn").GetComponent<Button>();
       corpsworship_btn = mTransform.Find("tabobjs/info_obj/corpsworship_btn").GetComponent<Button>();
       corpsshop_btn = mTransform.Find("tabobjs/info_obj/corpsshop_btn").GetComponent<Button>();
       corpdonate_btn = mTransform.Find("tabobjs/info_obj/corpdonate_btn").GetComponent<Button>();
    }
}