using UnityEngine;
using UnityEngine.UI;

public class SevenActivityViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Text hint_txt;
    [HideInInspector]
    public GameObject GoodsBar_obj;
    [HideInInspector]
    public Button Affirm_btn;
    [HideInInspector]
    public GameObject Get_obj;
    [HideInInspector]
    public Button one_btn;
    [HideInInspector]
    public Button tow_btn;
    [HideInInspector]
    public Button three_btn;
    [HideInInspector]
    public Button four_btn;
    [HideInInspector]
    public Button five_btn;
    [HideInInspector]
    public Button six_btn;
    [HideInInspector]
    public Button seven_btn;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       hint_txt = mTransform.Find("GetGoods/HintBj/hint_txt").GetComponent<Text>();
       GoodsBar_obj = mTransform.Find("GetGoods/GoodsBar/ge/GoodsBar_obj").gameObject;
       Affirm_btn = mTransform.Find("GetGoods/Affirm_btn").GetComponent<Button>();
       Get_obj = mTransform.Find("GetGoods/Get_obj").gameObject;
       one_btn = mTransform.Find("Fate/one_btn").GetComponent<Button>();
       tow_btn = mTransform.Find("Fate/tow_btn").GetComponent<Button>();
       three_btn = mTransform.Find("Fate/three_btn").GetComponent<Button>();
       four_btn = mTransform.Find("Fate/four_btn").GetComponent<Button>();
       five_btn = mTransform.Find("Fate/five_btn").GetComponent<Button>();
       six_btn = mTransform.Find("Fate/six_btn").GetComponent<Button>();
       seven_btn = mTransform.Find("Fate/seven_btn").GetComponent<Button>();
    }
}