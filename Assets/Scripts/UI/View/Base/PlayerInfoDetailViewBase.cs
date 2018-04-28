using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoDetailViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Button close_btn;
    [HideInInspector]
    public GameObject grids_obj;
    [HideInInspector]
    public Image head_img;
    [HideInInspector]
    public Text level_txt;
    [HideInInspector]
    public Text name_txt;
    [HideInInspector]
    public Text power_txt;
    [HideInInspector]
    public Text herocnt_txt;
    [HideInInspector]
    public Text corpsname_txt;
    [HideInInspector]
    public GameObject arena_obj;
    [HideInInspector]
    public Text arenarank_txt;
    [HideInInspector]
    public Text arenawin_txt;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       close_btn = mTransform.Find("close_btn").GetComponent<Button>();
       grids_obj = mTransform.Find("infos/grids_obj").gameObject;
       head_img = mTransform.Find("infos/head_img").GetComponent<Image>();
       level_txt = mTransform.Find("infos/level_txt").GetComponent<Text>();
       name_txt = mTransform.Find("infos/name_txt").GetComponent<Text>();
       power_txt = mTransform.Find("infos/power_txt").GetComponent<Text>();
       herocnt_txt = mTransform.Find("infos/herocnt_txt").GetComponent<Text>();
       corpsname_txt = mTransform.Find("infos/corpsname_txt").GetComponent<Text>();
       arena_obj = mTransform.Find("infos/arena_obj").gameObject;
       arenarank_txt = mTransform.Find("infos/arena_obj/arenarank_txt").GetComponent<Text>();
       arenawin_txt = mTransform.Find("infos/arena_obj/arenawin_txt").GetComponent<Text>();
    }
}