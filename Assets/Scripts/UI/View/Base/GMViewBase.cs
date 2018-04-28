using UnityEngine;
using UnityEngine.UI;

public class GMViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public GameObject Function_obj;
    [HideInInspector]
    public Button Close_btn;
    [HideInInspector]
    public InputField GMOrder_input;
    [HideInInspector]
    public Button Send_btn;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       Function_obj = mTransform.Find("bj/Function/Function_obj").gameObject;
       Close_btn = mTransform.Find("Close_btn").GetComponent<Button>();
       GMOrder_input = mTransform.Find("GMOrder_input").GetComponent<InputField>();
       Send_btn = mTransform.Find("Send_btn").GetComponent<Button>();
    }
}