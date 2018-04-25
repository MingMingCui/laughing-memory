using UnityEngine;
using UnityEngine.UI;

public class DivinationItemView : MonoBehaviour
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Image item_img;
    [HideInInspector]
    public Text level_txt;
    [HideInInspector]
    public CanvasRenderer play_cr;
    [HideInInspector]
    public Text name_txt;
    private CanvasRenderer cr;

    public virtual void Awake()
    {
        mTransform = transform;
        item_img = mTransform.Find("item_img").GetComponent<Image>();
        cr = item_img.GetComponent<CanvasRenderer>();
        play_cr = mTransform.Find("play_cr").GetComponent<CanvasRenderer>();
        level_txt = mTransform.Find("level_txt").GetComponent<Text>();
        name_txt = mTransform.Find("name_txt").GetComponent<Text>();
    }

    public TotemData data;
    public void SetView(TotemData data,int lv = 1)
    {
        if (data == null)
        {
            Color c = item_img.color;
            c.a = 0;
            item_img.color = c;
            play_cr.SetAlpha(0);
            name_txt.text = string.Format("<color=#FFFF00>{0}级</color>", lv);
            level_txt.text = "";
            return;
        }
        this.data = data;
        cr.SetAlpha(1);
        play_cr.SetAlpha(0);

        Color co = item_img.color;
        co.a = 1;
        item_img.color = co;
        item_img.sprite = ResourceMgr.Instance.LoadSprite(data.ItemData.icon);
        EventListener.Get(item_img.gameObject).OnClick = e =>
        {
            UIFace.GetSingleton().Open(UIID.DivinationTip, data,SHOWBUTTON.EtakeOff);
        };
        int rare = data.ItemData.rare;
        string color = ColorMgr.Colors[rare - 1];
        name_txt.text = string.Format("<color=#{0}>Lv.{1}\n {2}</color>", color, data.Level, data.TotemConfig.Name);
    }

    public void Lock(int lv)
    {
        cr.SetAlpha(0);
        play_cr.SetAlpha(0);
        name_txt.supportRichText = true;
        name_txt.text = string.Format("<color=#FFFF00>{0}级</color>", lv);
        level_txt.text = "";        
    }

    public void SetAlpha(float alpha)
    {
        play_cr.SetAlpha(alpha);
    }
}
