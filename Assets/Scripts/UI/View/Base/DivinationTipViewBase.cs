using UnityEngine;
using UnityEngine.UI;

public class DivinationTipViewBase : UIViewBase
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public CanvasRenderer border_cr;
    [HideInInspector]
    public Image item_img;
    [HideInInspector]
    public Text level_txt;
    [HideInInspector]
    public Text name_txt;
    [HideInInspector]
    public Image slider_img;
    [HideInInspector]
    public Text attr_txt;
    [HideInInspector]
    public Button take_btn;
    [HideInInspector]
    public Button takeoff_btn;
    [HideInInspector]
    public Button compose_btn;
    [HideInInspector]
    public Text prog_txt;
    public virtual void Awake()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
        border_cr = mTransform.Find("border_cr").GetComponent<CanvasRenderer>();
        item_img = mTransform.Find("item_img").GetComponent<Image>();
        level_txt = mTransform.Find("level_txt").GetComponent<Text>();
        name_txt = mTransform.Find("name_txt").GetComponent<Text>();
        slider_img = mTransform.Find("slider_img").GetComponent<Image>();
        attr_txt = mTransform.Find("attr_txt").GetComponent<Text>();
        take_btn = mTransform.Find("button/take_btn").GetComponent<Button>();
        takeoff_btn = mTransform.Find("button/takeoff_btn").GetComponent<Button>();
        compose_btn = mTransform.Find("button/compose_btn").GetComponent<Button>();
        prog_txt = mTransform.Find("prog_txt").GetComponent<Text>();
    }
}
