using UnityEngine;
using UnityEngine.UI;

public class UICorpsViewBase : UIViewBase 
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
    public GameObject create_obj;
    [HideInInspector]
    public InputField corpsname_input;
    [HideInInspector]
    public Image flag_img;
    [HideInInspector]
    public Button modifyflag_btn;
    [HideInInspector]
    public InputField levellimit_input;
    [HideInInspector]
    public Toggle allallow_tog;
    [HideInInspector]
    public Toggle adminallow_tog;
    [HideInInspector]
    public Text cost_txt;
    [HideInInspector]
    public Button createcorps_btn;
    [HideInInspector]
    public GameObject join_obj;
    [HideInInspector]
    public GameObject corpsnodes_obj;
    [HideInInspector]
    public Toggle hidefull_tog;
    [HideInInspector]
    public Button search_btn;
    [HideInInspector]
    public Button refresh_btn;
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
       create_obj = mTransform.Find("create_obj").gameObject;
       corpsname_input = mTransform.Find("create_obj/corpsname_input").GetComponent<InputField>();
       flag_img = mTransform.Find("create_obj/flag/flag_img").GetComponent<Image>();
       modifyflag_btn = mTransform.Find("create_obj/modifyflag_btn").GetComponent<Button>();
       levellimit_input = mTransform.Find("create_obj/levellimit_input").GetComponent<InputField>();
       allallow_tog = mTransform.Find("create_obj/allowtype/allallow_tog").GetComponent<Toggle>();
       adminallow_tog = mTransform.Find("create_obj/allowtype/adminallow_tog").GetComponent<Toggle>();
       cost_txt = mTransform.Find("create_obj/cost/cost_txt").GetComponent<Text>();
       createcorps_btn = mTransform.Find("create_obj/createcorps_btn").GetComponent<Button>();
       join_obj = mTransform.Find("join_obj").gameObject;
       corpsnodes_obj = mTransform.Find("join_obj/corpslist/viewrect/corpsnodes_obj").gameObject;
       hidefull_tog = mTransform.Find("join_obj/hidefull_tog").GetComponent<Toggle>();
       search_btn = mTransform.Find("join_obj/InputField/search_btn").GetComponent<Button>();
       refresh_btn = mTransform.Find("join_obj/refresh_btn").GetComponent<Button>();
    }
}