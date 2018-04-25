using UnityEngine;
using UnityEngine.UI;

public class TipViewBase : UIViewBase
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Button left_btn;
    [HideInInspector]
    public Text left_txt;
    [HideInInspector]
    public Button right_btn;
    [HideInInspector]
    public Text right_txt;
    [HideInInspector]
    public Text content_txt;
    public virtual void Awake()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
        left_btn = mTransform.Find("left_btn").GetComponent<Button>();
        left_txt = mTransform.Find("left_btn/left_txt").GetComponent<Text>();
        right_btn = mTransform.Find("right_btn").GetComponent<Button>();
        right_txt = mTransform.Find("right_btn/right_txt").GetComponent<Text>();
        content_txt = mTransform.Find("content_txt").GetComponent<Text>();
    }
}
