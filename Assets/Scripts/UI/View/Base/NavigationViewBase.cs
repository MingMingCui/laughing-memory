using UnityEngine;
using UnityEngine.UI;

public class NavigationViewBase : UIViewBase 
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Transform ItemParent_trf;
    [HideInInspector]
    public GameObject Head_obj;
    [HideInInspector]
    public Image Head_img;
    [HideInInspector]
    public Text grade_txt;
    [HideInInspector]
    public Text Name_txt;
    [HideInInspector]
    public Text Fighting_txt;
    [HideInInspector]
    public Image expect_img;
    [HideInInspector]
    public GameObject exitUI_obj;
    [HideInInspector]
    public Button exit_btn;
    public virtual void Awake()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
        ItemParent_trf = mTransform.Find("ItemParent_trf").GetComponent<Transform>();
        Head_obj = mTransform.Find("Head_obj").gameObject;
        Head_img = mTransform.Find("Head_obj/Head_img").GetComponent<Image>();
        grade_txt = mTransform.Find("Head_obj/grade_txt").GetComponent<Text>();
        Name_txt = mTransform.Find("Head_obj/Name_txt").GetComponent<Text>();
        Fighting_txt = mTransform.Find("Head_obj/Fighting_txt").GetComponent<Text>();
        expect_img = mTransform.Find("expect_img").GetComponent<Image>();
        exitUI_obj = mTransform.Find("exitUI_obj").gameObject;
        exit_btn = mTransform.Find("exitUI_obj/exit_btn").GetComponent<Button>();
    }
}