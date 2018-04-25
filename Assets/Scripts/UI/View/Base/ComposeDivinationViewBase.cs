using UnityEngine.UI;
using UnityEngine;

public class ComposeDivinationViewBase : UIViewBase
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Transform particlemain_trf;
    [HideInInspector]
    public Button sure_btn;
    [HideInInspector]
    public Button auto_btn;
    [HideInInspector]
    public Image engulfexp_img;
    [HideInInspector]
    public Image expslider_img;
    [HideInInspector]
    public Text name_txt;
    [HideInInspector]
    public Text exp_txt;
    [HideInInspector]
    public ScrollRect divination_sr;
    [HideInInspector]
    public Text attr_txt;
    [HideInInspector]
    public Text nextattr_txt;
    [HideInInspector]
    public Button tip_btn;
    [HideInInspector]
    public GameObject tip_obj;
    [HideInInspector]
    public Image playItem_img;
    [HideInInspector]
    public Transform hexagon_trf;
    [HideInInspector]
    public Button close_btn;
    [HideInInspector]
    public Transform particle_trf;
    public virtual void Awake()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
        particlemain_trf = mTransform.Find("particlemain_trf").GetComponent<Transform>();
        sure_btn = mTransform.Find("sure_btn").GetComponent<Button>();
        auto_btn = mTransform.Find("auto_btn").GetComponent<Button>();
        engulfexp_img = mTransform.Find("Background/engulfexp_img").GetComponent<Image>();
        expslider_img = mTransform.Find("Background/expslider_img").GetComponent<Image>();
        name_txt = mTransform.Find("name_txt").GetComponent<Text>();
        exp_txt = mTransform.Find("exp_txt").GetComponent<Text>();
        divination_sr = mTransform.Find("divination_sr").GetComponent<ScrollRect>();
        attr_txt = mTransform.Find("attr_txt").GetComponent<Text>();
        nextattr_txt = mTransform.Find("attr_txt/nextattr_txt").GetComponent<Text>();
        tip_btn = mTransform.Find("tip_btn").GetComponent<Button>();
        tip_obj = mTransform.Find("tip_obj").gameObject;
        playItem_img = mTransform.Find("playItem_img").GetComponent<Image>();
        hexagon_trf = mTransform.Find("hexagon_trf").GetComponent<Transform>();
        close_btn = mTransform.Find("close_btn").GetComponent<Button>();
        particle_trf = mTransform.Find("particle_trf").GetComponent<Transform>();
    }
}
