using UnityEngine;
using UnityEngine.UI;

public class StubViewBase : UIViewBase 
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
    public GameObject format_obj;
    [HideInInspector]
    public GameObject stub_obj;
    [HideInInspector]
    public GameObject heros_obj;
    [HideInInspector]
    public GameObject herogrid_obj;
    [HideInInspector]
    public GameObject StubPos_obj;
    [HideInInspector]
    public GameObject StubShow_obj;
    [HideInInspector]
    public UIBillboard StubShow_bb;
    [HideInInspector]
    public Button StubCommit_btn;
    [HideInInspector]
    public Text fightpower_txt;
    [HideInInspector]
    public GameObject Tabs_obj;
    [HideInInspector]
    public Toggle Stub_tog;
    [HideInInspector]
    public Toggle Format_tog;
    [HideInInspector]
    public UIBillboard StubDrag_bb;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       chatacters1_obj = mTransform.Find("UI_bg/chatacters1_obj").gameObject;
       characters1_obj = mTransform.Find("UI_bg/chatacters1_obj/characters1_obj").gameObject;
       characters2_obj = mTransform.Find("UI_bg/chatacters1_obj/characters2_obj").gameObject;
       chatacters2_obj = mTransform.Find("UI_bg/chatacters2_obj").gameObject;
       characters1_obj = mTransform.Find("UI_bg/chatacters2_obj/characters1_obj").gameObject;
       characters2_obj = mTransform.Find("UI_bg/chatacters2_obj/characters2_obj").gameObject;
       chatacters3_obj = mTransform.Find("UI_bg/chatacters3_obj").gameObject;
       characters1_obj = mTransform.Find("UI_bg/chatacters3_obj/characters1_obj").gameObject;
       characters2_obj = mTransform.Find("UI_bg/chatacters3_obj/characters2_obj").gameObject;
       format_obj = mTransform.Find("stubobjs/format_obj").gameObject;
       stub_obj = mTransform.Find("stubobjs/stub_obj").gameObject;
       heros_obj = mTransform.Find("stubobjs/stub_obj/heros_obj").gameObject;
       herogrid_obj = mTransform.Find("stubobjs/stub_obj/heros_obj/viewport/herogrid_obj").gameObject;
       StubPos_obj = mTransform.Find("stubobjs/stub_obj/StubPos_obj").gameObject;
       StubShow_obj = mTransform.Find("stubobjs/stub_obj/StubShow_obj").gameObject;
       StubShow_bb = mTransform.Find("stubobjs/stub_obj/StubShow_obj/StubShow_bb").GetComponent<UIBillboard>();
       StubCommit_btn = mTransform.Find("stubobjs/stub_obj/StubCommit_btn").GetComponent<Button>();
       fightpower_txt = mTransform.Find("stubobjs/stub_obj/fightpower/fightpower_txt").GetComponent<Text>();
       Tabs_obj = mTransform.Find("Tabs_obj").gameObject;
       Stub_tog = mTransform.Find("Tabs_obj/Stub_tog").GetComponent<Toggle>();
       Format_tog = mTransform.Find("Tabs_obj/Format_tog").GetComponent<Toggle>();
       StubDrag_bb = mTransform.Find("StubDrag_bb").GetComponent<UIBillboard>();
    }
}