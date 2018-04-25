using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class OfficerItem : MonoBehaviour
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Transform view_trf;
    [HideInInspector]
    public CanvasRenderer Officerdis_cr;
    [HideInInspector]
    public Text officername_txt;
    [HideInInspector]
    public CanvasRenderer take_cr;
    [HideInInspector]
    public GameObject down_obj;
    [HideInInspector]
    public Text leftpro_txt;
    [HideInInspector]
    public Text rightpro_txt;
    [HideInInspector]
    public Button take_btn;

    private RectTransform rt;
    private VerticalLayoutGroup vlg;
    private CanvasRenderer cr;

    public bool down;

    public float normalY;
    public float highlightY;

    void Awake()
    {
        this.mTransform = this.transform;
        view_trf = mTransform.Find("view_trf").GetComponent<Transform>();
        Officerdis_cr = mTransform.Find("view_trf/Officerdis_cr").GetComponent<CanvasRenderer>();
        officername_txt = mTransform.Find("view_trf/officername_txt").GetComponent<Text>();
        take_cr = mTransform.Find("view_trf/take_cr").GetComponent<CanvasRenderer>();
        down_obj = mTransform.Find("view_trf/down_obj").gameObject;
        leftpro_txt = mTransform.Find("view_trf/down_obj/leftpro_txt").GetComponent<Text>();
        rightpro_txt = mTransform.Find("view_trf/down_obj/rightpro_txt").GetComponent<Text>();
        take_btn = mTransform.Find("view_trf/down_obj/take_btn").GetComponent<Button>();

        rt = mTransform.GetComponent<RectTransform>();
        vlg = mTransform.GetComponentInParent<VerticalLayoutGroup>();
        cr = take_btn.GetComponent<CanvasRenderer>();
    }
    public int officerID;
    private HeroData hd;
    string text;

    public void SetView(HeroData heroData, Officer o)
    {
        officerID = o.ID;
        hd = heroData;
        text = o.Post;


        take_cr.SetAlpha(TestColor(false) == heroData.HeroId ? 1 : 0);
        down_obj.SetActive(false);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < o.attrerty.Length; i++)
        {
            Pro p = o.attrerty[i];
            sb.Append(AttrUtil.GetAttribute(p.attr));
            sb.Append(string.Format("<color=#00FF00> +{0}</color>",p.num));
            if ((i + 1) % 2 != 0)
                sb.Append("              ");
            else
                sb.Append("\n");
        }
        leftpro_txt.text = sb.ToString();
    }

    private int TestColor(bool down)
    {
        int takeID = HeroMgr.GetSingleton().IsTake(officerID);
        Officerdis_cr.SetAlpha(takeID == hd.HeroId || takeID == 0 ? 0 : 1);
        string show = "";
        show = takeID != hd.HeroId && takeID != 0 ?
                      string.Format("<color=#5C5C5C>{0}  {1}</color>", text, down ? "" :JsonMgr.GetSingleton().GetHeroByID(takeID).name) :
                      down ? string.Format("<color=#FFFF30>{0}</color>", text) : string.Format("<color=#FF8A00>{0}</color>", text);
        officername_txt.text = show;
        return takeID;
    }

    public void OnClickItem()
    {
        float minu = (highlightY - normalY) * 0.5f;
        down = !down;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, down ? highlightY : normalY);
        Vector3 pos = view_trf.localPosition;
        pos.y += down ? minu : -minu;
        view_trf.localPosition = pos;
        down_obj.SetActive(down);
        vlg.SetLayoutVertical();

        TestColor(down);
        //1.已被 其它武将装备 2.没有被任何武将装备  3.自己装备
        cr.SetAlpha(take_cr.GetAlpha() == 0 ? 1 : 0);
    }

}
