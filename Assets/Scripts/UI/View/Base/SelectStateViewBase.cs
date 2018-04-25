using UnityEngine;
using UnityEngine.UI;

public class SelectStateViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Toggle Wu_tog;
    [HideInInspector]
    public Image wu_b_img;
    [HideInInspector]
    public Image wu_a_img;
    [HideInInspector]
    public Toggle Shu_tog;
    [HideInInspector]
    public Image shu_b_img;
    [HideInInspector]
    public Image shu_a_img;
    [HideInInspector]
    public Toggle Wei_tog;
    [HideInInspector]
    public Image wei_b_img;
    [HideInInspector]
    public Image wei_a_img;
    [HideInInspector]
    public Button Start_btn;
    [HideInInspector]
    public Button Random_btn;
    [HideInInspector]
    public GameObject zhezhao_obj;
    public virtual void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       Wu_tog = mTransform.Find("Wu_tog").GetComponent<Toggle>();
       wu_b_img = mTransform.Find("Wu_tog/wu_b_img").GetComponent<Image>();
       wu_a_img = mTransform.Find("Wu_tog/wu_b_img/wu_a_img").GetComponent<Image>();
       Shu_tog = mTransform.Find("Shu_tog").GetComponent<Toggle>();
       shu_b_img = mTransform.Find("Shu_tog/shu_b_img").GetComponent<Image>();
       shu_a_img = mTransform.Find("Shu_tog/shu_b_img/shu_a_img").GetComponent<Image>();
       Wei_tog = mTransform.Find("Wei_tog").GetComponent<Toggle>();
       wei_b_img = mTransform.Find("Wei_tog/wei_b_img").GetComponent<Image>();
       wei_a_img = mTransform.Find("Wei_tog/wei_b_img/wei_a_img").GetComponent<Image>();
       Start_btn = mTransform.Find("Start_btn").GetComponent<Button>();
       Random_btn = mTransform.Find("Random_btn").GetComponent<Button>();
       zhezhao_obj = mTransform.Find("zhezhao_obj").gameObject;
    }
}