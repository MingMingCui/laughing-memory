using UnityEngine;
using UnityEngine.UI;

public class DivinationViewBase : UIViewBase
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Button tip_btn;
    [HideInInspector]
    public Image lucky_img;
    [HideInInspector]
    public GameObject tip_obj;
    [HideInInspector]
    public Button take_btn;
    [HideInInspector]
    public Button sale_btn;
    [HideInInspector]
    public Button divination20_btn;
    [HideInInspector]
    public ScrollRect Item_sr;
    [HideInInspector]
    public Transform itemparent_trf;
    [HideInInspector]
    public Transform select_trf;
    public virtual void Awake()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
        tip_btn = mTransform.Find("Image (3)/tip_btn").GetComponent<Button>();
        lucky_img = mTransform.Find("Image (3)/lucky_img").GetComponent<Image>();
        tip_obj = mTransform.Find("Image (3)/tip_obj").gameObject;
        take_btn = mTransform.Find("Image (4)/take_btn").GetComponent<Button>();
        sale_btn = mTransform.Find("Image (4)/sale_btn").GetComponent<Button>();
        divination20_btn = mTransform.Find("Image (4)/divination20_btn").GetComponent<Button>();
        Item_sr = mTransform.Find("Image (2)/Item_sr").GetComponent<ScrollRect>();
        itemparent_trf = mTransform.Find("Image (2)/Item_sr/Viewport/itemparent_trf").GetComponent<Transform>();
        select_trf = mTransform.Find("Image (2)/Item_sr/Viewport/select_trf").GetComponent<Transform>();
    }
}
