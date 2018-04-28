using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpsMemberNodeView : MonoBehaviour {

    public GameObject selfbg_obj = null;
    public Image memberhead_img = null;
    public Text member_name_txt = null;
    public Text member_level_txt = null;
    public Text member_job_txt = null;
    public Text member_accvigour_txt = null;
    public Text member_donate_txt = null;
    public Text member_lastol_txt = null;
    public Button member_exit_btn = null;

    public static readonly int[] CorpsJob = new int[3] { 2008, 2007, 2006 };    //普通帮众、副团长、团长

    public void SetInfo(uint uid, int headid, string name, int level, int job, int vigour, int donate, long lastol, bool self = false)
    {
        this.selfbg_obj.SetActive(self);
        this.member_exit_btn.gameObject.SetActive(self);
        this.member_lastol_txt.gameObject.SetActive(!self);
        if (self)
            EventListener.Get(member_exit_btn.gameObject).OnClick = e =>
            {
                //退出公会
            };
        else
            EventListener.Get(memberhead_img.gameObject).OnClick = e =>
            {
                //查看信息
                UIFace.GetSingleton().Open(UIID.PlayerInfo, uid);
            };
        //this.memberhead_img.sprite = ResourceMgr.Instance.LoadSprite(headid);
        Color32 textColor = self ? ColorMgr.SkyBlue : ColorMgr.YellowWish;
        this.member_name_txt.color = textColor;
        this.member_name_txt.text = name;
        this.member_level_txt.color = textColor;
        this.member_level_txt.text = level.ToString();
        this.member_job_txt.color = textColor;
        this.member_job_txt.text = JsonMgr.GetSingleton().GetGlobalStringArrayByID(CorpsJob[job]).desc;
        this.member_accvigour_txt.color = textColor;
        this.member_accvigour_txt.text = vigour.ToString();
        this.member_donate_txt.color = textColor;
        this.member_donate_txt.text = donate.ToString();
        this.member_lastol_txt.text = lastol == 0 ? "在线" : lastol.ToString();

    }
}
