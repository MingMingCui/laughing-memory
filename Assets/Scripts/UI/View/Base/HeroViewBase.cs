using UnityEngine;
using UnityEngine.UI;

public class HeroViewBase : UIViewBase
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public GameObject ScrollView_obj;
    [HideInInspector]
    public GameObject Content_obj;
    [HideInInspector]
    public GameObject SideBackGround_obj;
    [HideInInspector]
    public Image all_img;
    [HideInInspector]
    public Image shuchu_img;
    [HideInInspector]
    public Image fangyu_img;
    [HideInInspector]
    public Image gongshou_img;
    [HideInInspector]
    public Image fuzhu_img;
    [HideInInspector]
    public Image moushi_img;
    [HideInInspector]
    public GameObject Highlighted_obj;
    [HideInInspector]
    public GameObject Shuchu_obj;
    [HideInInspector]
    public GameObject Fangyu_obj;
    [HideInInspector]
    public GameObject Gongshou_obj;
    [HideInInspector]
    public GameObject Fuzhu_obj;
    [HideInInspector]
    public GameObject Moushi_obj;


    public virtual void Awake()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
        ScrollView_obj = mTransform.Find("ScrollView_obj").gameObject;
        Content_obj = mTransform.Find("ScrollView_obj/Viewport/Content_obj").gameObject;

        all_img = mTransform.Find("SideBackGround_obj/all_img").GetComponent<Image>();
        shuchu_img = mTransform.Find("SideBackGround_obj/shuchu_img").GetComponent<Image>();
        fangyu_img = mTransform.Find("SideBackGround_obj/fangyu_img").GetComponent<Image>();
        gongshou_img = mTransform.Find("SideBackGround_obj/gongshou_img").GetComponent<Image>();
        fuzhu_img = mTransform.Find("SideBackGround_obj/fuzhu_img").GetComponent<Image>();
        moushi_img = mTransform.Find("SideBackGround_obj/moushi_img").GetComponent<Image>();

        Shuchu_obj = mTransform.Find("Worker/Shuchu_obj").gameObject;
        Fangyu_obj = mTransform.Find("Worker/Fangyu_obj").gameObject;
        Gongshou_obj = mTransform.Find("Worker/Gongshou_obj").gameObject;
        Fuzhu_obj = mTransform.Find("Worker/Fuzhu_obj").gameObject;
        Moushi_obj = mTransform.Find("Worker/Moushi_obj").gameObject;
    }
}