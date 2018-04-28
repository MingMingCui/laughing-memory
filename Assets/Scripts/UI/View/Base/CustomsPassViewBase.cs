using UnityEngine;
using UnityEngine.UI;

public class CustomsPassViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Image name_img;
    [HideInInspector]
    public Text name_txt;
    [HideInInspector]
    public Text intro_txt;
    [HideInInspector]
    public Text power_txt;
    [HideInInspector]
    public GameObject time_obj;
    [HideInInspector]
    public Text nowTime_txt;
    [HideInInspector]
    public Text Total_txt;
    [HideInInspector]
    public Button buy_btn;
    [HideInInspector]
    public GameObject enemyShow_obj;
    [HideInInspector]
    public GameObject dropOutShow_obj;
    [HideInInspector]
    public GameObject MoppingUp_obj;
    [HideInInspector]
    public Image star_1_img;
    [HideInInspector]
    public Image star_2_img;
    [HideInInspector]
    public Image star_3_img;
    [HideInInspector]
    public Text combat_txt;
    [HideInInspector]
    public Button operation_10_btn;
    [HideInInspector]
    public Button operation_btn;
    [HideInInspector]
    public Button embattle_btn;
    [HideInInspector]
    public Button begin_btn;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       name_img = mTransform.Find("Frame/Headline/name_img").GetComponent<Image>();
       name_txt = mTransform.Find("Frame/Headline/name_txt").GetComponent<Text>();
       intro_txt = mTransform.Find("Frame/intro_txt").GetComponent<Text>();
       power_txt = mTransform.Find("Frame/Consume/power/power_txt").GetComponent<Text>();
       time_obj = mTransform.Find("Frame/Consume/time_obj").gameObject;
       nowTime_txt = mTransform.Find("Frame/Consume/time_obj/now/nowTime_txt").GetComponent<Text>();
       Total_txt = mTransform.Find("Frame/Consume/time_obj/now/Total_txt").GetComponent<Text>();
       buy_btn = mTransform.Find("Frame/Consume/time_obj/buy_btn").GetComponent<Button>();
       enemyShow_obj = mTransform.Find("Frame/Enemy/enemyShow_obj").gameObject;
       dropOutShow_obj = mTransform.Find("Frame/DropOut/GameObject/dropOutShow_obj").gameObject;
       MoppingUp_obj = mTransform.Find("Frame/MoppingUp_obj").gameObject;
       star_1_img = mTransform.Find("Frame/MoppingUp_obj/star/star_1_img").GetComponent<Image>();
       star_2_img = mTransform.Find("Frame/MoppingUp_obj/star/star_2_img").GetComponent<Image>();
       star_3_img = mTransform.Find("Frame/MoppingUp_obj/star/star_3_img").GetComponent<Image>();
       combat_txt = mTransform.Find("Frame/MoppingUp_obj/combat_txt").GetComponent<Text>();
       operation_10_btn = mTransform.Find("Frame/MoppingUp_obj/operation_10_btn").GetComponent<Button>();
       operation_btn = mTransform.Find("Frame/MoppingUp_obj/operation_btn").GetComponent<Button>();
       embattle_btn = mTransform.Find("Frame/embattle_btn").GetComponent<Button>();
       begin_btn = mTransform.Find("Frame/begin_btn").GetComponent<Button>();
    }
}