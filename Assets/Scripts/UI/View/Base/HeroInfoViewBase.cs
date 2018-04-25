using UnityEngine;
using UnityEngine.UI;

public class HeroInfoViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public GameObject Equip_obj;
    [HideInInspector]
    public Image Head_img;
    [HideInInspector]
    public Image Worker_img;
    [HideInInspector]
    public Slider Hp_slider;
    [HideInInspector]
    public Text HeroName_txt;
    void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       Equip_obj = mTransform.Find("Equip_obj").gameObject;
       Head_img = mTransform.Find("Head_img").GetComponent<Image>();
       Worker_img = mTransform.Find("Worker_img").GetComponent<Image>();
       Hp_slider = mTransform.Find("Hp_slider").GetComponent<Slider>();
       HeroName_txt = mTransform.Find("HeroName_txt").GetComponent<Text>();
    }
}