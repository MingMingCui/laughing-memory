using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpsNodeView : MonoBehaviour {

    public Image corps_img = null;

    public Text corpsname_txt = null;

    public Text corpslevel_txt = null;

    public Text corpsmember_txt = null;

    public Text corpsleader_txt = null;

    public Text corpslimit_txt = null;

    public Button request_btn = null;

    public Button join_btn = null;

    public Image requested_img = null;

    public Image country_img = null;

    public List<Sprite> countries = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag"></param>
    /// <param name="country"></param>
    /// <param name="name"></param>
    /// <param name="level"></param>
    /// <param name="member"></param>
    /// <param name="leader"></param>
    /// <param name="limit"></param>
    /// <param name="state">0 申请 1 加入 2 已申请</param>
    public void SetInfo(uint id, int flag, int country, string name, int level, int member, string leader, int limit, int state)
    {
        this.corps_img.sprite = ResourceMgr.Instance.LoadSprite(flag);
        this.corpsname_txt.text = name;
        this.corpslevel_txt.text = string.Format("{0}级", level);
        this.corpsmember_txt.text = string.Format("{0}/{1}", member, CorpsMgr.Instance.GetCorpsMaxMemberByLevel());
        this.corpsleader_txt.text = leader;
        this.corpslimit_txt.text = limit == 0 ? "无限制" : string.Format("{0}级", limit);
        this.country_img.sprite = countries[country];
        request_btn.gameObject.SetActive(state == 0);
        join_btn.gameObject.SetActive(state == 1);
        requested_img.gameObject.SetActive(state == 2);
    }
}
