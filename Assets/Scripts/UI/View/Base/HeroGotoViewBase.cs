using UnityEngine;
using UnityEngine.UI;

public class HeroGotoViewBase : UIViewBase
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Text HeroName_txt;
    [HideInInspector]
    public Image piece_img;
    [HideInInspector]
    public Image broder_img;
    [HideInInspector]
    public Text Piece_txt;
    [HideInInspector]
    public Button close_btn;
    [HideInInspector]
    public GameObject parent_obj;
    public virtual void Awake()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
        HeroName_txt = mTransform.Find("HeroName_txt").GetComponent<Text>();
        piece_img = mTransform.Find("mask/piece_img").GetComponent<Image>();
        broder_img = mTransform.Find("broder_img").GetComponent<Image>();
        Piece_txt = mTransform.Find("Piece_txt").GetComponent<Text>();
        close_btn = mTransform.Find("close_btn").GetComponent<Button>();
        parent_obj = mTransform.Find("ScrollView/Viewport/parent_obj").gameObject;
    }
}
