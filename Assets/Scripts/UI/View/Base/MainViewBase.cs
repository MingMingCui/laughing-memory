using UnityEngine;
using UnityEngine.UI;

public class MainViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public GameObject function_obj;
    [HideInInspector]
    public GameObject fun_obj;
    [HideInInspector]
    public Button warrior_btn;
    [HideInInspector]
    public Button knapsack_btn;
    [HideInInspector]
    public Button embattle_btn;
    [HideInInspector]
    public Button task_btn;
    [HideInInspector]
    public Button daily_btn;
    [HideInInspector]
    public Toggle folding_tog;
    [HideInInspector]
    public Button Battle_btn;
    [HideInInspector]
    public Button chat_btn;
    [HideInInspector]
    public Button chat_Txt_btn;
    [HideInInspector]
    public Text chat_txt;
    [HideInInspector]
    public Image expect_img;
    [HideInInspector]
    public Button State_btn;
    [HideInInspector]
    public Button Award_btn;
    [HideInInspector]
    public Button Signin_btn;
    [HideInInspector]
    public Button Shop_btn;
    [HideInInspector]
    public Button WesternShop_btn;
    [HideInInspector]
    public Button divination_btn;
    [HideInInspector]
    public Button Recruiting_btn;
    [HideInInspector]
    public Button Mail_btn;
    [HideInInspector]
    public GameObject reddot_obj;
    [HideInInspector]
    public Button GM_btn;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       function_obj = mTransform.Find("Function/function_obj").gameObject;
       fun_obj = mTransform.Find("Function/function_obj/fun_obj").gameObject;
        warrior_btn = mTransform.Find("Function/function_obj/fun_obj/warrior_btn").GetComponent<Button>();
       knapsack_btn = mTransform.Find("Function/function_obj/fun_obj/knapsack_btn").GetComponent<Button>();
       embattle_btn = mTransform.Find("Function/function_obj/fun_obj/embattle_btn").GetComponent<Button>();
       task_btn = mTransform.Find("Function/function_obj/fun_obj/task_btn").GetComponent<Button>();
       daily_btn = mTransform.Find("Function/function_obj/fun_obj/daily_btn").GetComponent<Button>();
       folding_tog = mTransform.Find("Function/folding/folding_tog").GetComponent<Toggle>();
       Battle_btn = mTransform.Find("Battle_btn").GetComponent<Button>();
       chat_btn = mTransform.Find("chat/chat_btn").GetComponent<Button>();
       chat_Txt_btn = mTransform.Find("chat/chat_Txt_btn").GetComponent<Button>();
       chat_txt = mTransform.Find("chat/chat_Txt_btn/chat_txt").GetComponent<Text>();
       expect_img = mTransform.Find("expect_img").GetComponent<Image>();
       State_btn = mTransform.Find("State_btn").GetComponent<Button>();
       Award_btn = mTransform.Find("Award_btn").GetComponent<Button>();
       Signin_btn = mTransform.Find("Signin_btn").GetComponent<Button>();
       Shop_btn = mTransform.Find("Shop_btn").GetComponent<Button>();
       WesternShop_btn = mTransform.Find("WesternShop_btn").GetComponent<Button>();
       divination_btn = mTransform.Find("divination_btn").GetComponent<Button>();
       Recruiting_btn = mTransform.Find("Recruiting_btn").GetComponent<Button>();
       Mail_btn = mTransform.Find("Mail_btn").GetComponent<Button>();
       reddot_obj = mTransform.Find("Mail_btn/reddot_obj").gameObject;
       GM_btn = mTransform.Find("GM_btn").GetComponent<Button>();
    }
}