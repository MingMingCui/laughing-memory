using UnityEngine;
using UnityEngine.UI;

public class ArenaViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Text rank_txt;
    [HideInInspector]
    public GameObject defend_obj;
    [HideInInspector]
    public Text fightpower_txt;
    [HideInInspector]
    public Button defend_btn;
    [HideInInspector]
    public Button introduce_btn;
    [HideInInspector]
    public Button fightlog_btn;
    [HideInInspector]
    public Button rank_btn;
    [HideInInspector]
    public Button award_btn;
    [HideInInspector]
    public Button changeenemy_btn;
    [HideInInspector]
    public Text refresh_txt;
    [HideInInspector]
    public Text lefttime_txt;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       rank_txt = mTransform.Find("personal/rank_txt").GetComponent<Text>();
       defend_obj = mTransform.Find("personal/defend_obj").gameObject;
       fightpower_txt = mTransform.Find("personal/fightpower_txt").GetComponent<Text>();
       defend_btn = mTransform.Find("personal/defend_btn").GetComponent<Button>();
       introduce_btn = mTransform.Find("personal/introduce_btn").GetComponent<Button>();
       fightlog_btn = mTransform.Find("personal/fightlog_btn").GetComponent<Button>();
       rank_btn = mTransform.Find("personal/rank_btn").GetComponent<Button>();
       award_btn = mTransform.Find("personal/award_btn").GetComponent<Button>();
       changeenemy_btn = mTransform.Find("changeenemy_btn").GetComponent<Button>();
       refresh_txt = mTransform.Find("refresh_txt").GetComponent<Text>();
       lefttime_txt = mTransform.Find("lefttime_txt").GetComponent<Text>();
    }
}