using UnityEngine;
using UnityEngine.UI;

public class HeroHead : MonoBehaviour
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Image Head_img;
    [HideInInspector]
    public Image Border_img;
    [HideInInspector]
    public Transform StarParent_trf;
    [HideInInspector]
    public Text Lv_txt;
    [HideInInspector]
    public Transform HideStar_trf;
    public virtual void Awake()
    {
        this.mTransform = this.transform;
        Head_img = mTransform.Find("Head_img").GetComponent<Image>();
        Border_img = mTransform.Find("Border_img").GetComponent<Image>();
        StarParent_trf = mTransform.Find("StarParent_trf").GetComponent<Transform>();
        Lv_txt = mTransform.Find("Lv_txt").GetComponent<Text>();
        HideStar_trf = mTransform.Find("HideStar_trf").GetComponent<Transform>();
    }


    public void SetLv(int lv)
    {
        Lv_txt.text = lv.ToString();
    }

    public void Close()
    {

    }
}
