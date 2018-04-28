using UnityEngine;
using UnityEngine.UI;

public class SelectPerViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public InputField Import_input;
    [HideInInspector]
    public Button Random_btn;
    [HideInInspector]
    public Button GoGame_btn;
    [HideInInspector]
    public Button Select_btn;
    [HideInInspector]
    public Button Select1_btn;
    [HideInInspector]
    public Toggle Man_tog;
    [HideInInspector]
    public Toggle Man1_tog;
    [HideInInspector]
    public Toggle Woman_tog;
    [HideInInspector]
    public Toggle Woman1_tog;
    [HideInInspector]
    public Image Shield_img;
    [HideInInspector]
    public Image Shield1_img;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       Import_input = mTransform.Find("Import_input").GetComponent<InputField>();
       Random_btn = mTransform.Find("Random_btn").GetComponent<Button>();
       GoGame_btn = mTransform.Find("GoGame_btn").GetComponent<Button>();
       Select_btn = mTransform.Find("Select_btn").GetComponent<Button>();
       Select1_btn = mTransform.Find("Select1_btn").GetComponent<Button>();
       Man_tog = mTransform.Find("Man_tog").GetComponent<Toggle>();
       Man1_tog = mTransform.Find("Man1_tog").GetComponent<Toggle>();
       Woman_tog = mTransform.Find("Woman_tog").GetComponent<Toggle>();
       Woman1_tog = mTransform.Find("Woman1_tog").GetComponent<Toggle>();
       Shield_img = mTransform.Find("Shield_img").GetComponent<Image>();
       Shield1_img = mTransform.Find("Shield1_img").GetComponent<Image>();
    }
}