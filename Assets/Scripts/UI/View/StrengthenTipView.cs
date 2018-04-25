using System.Text;

public class StrengthenTipView : StrengthenTipViewBase
{
    private EquipData data;

    public override void Awake()
    {
        base.Awake();
        EventListener.Get(takeoff_btn.gameObject).OnClick = e =>
        {
            if (data == null)
                return;
            EquipMgr.GetSingleton().TakeOffEquip(data);
            data = null;
            UIFace.GetSingleton().Close(UIID.StrengthenTip);
            ZEventSystem.Dispatch(EventConst.REFRESHLEFT);
        };  
    }

    public void Open(EquipData equip)
    {
        this.data = equip;
        int rare = equip.ItemData.rare;
        border_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[rare - 1]);
        string color = ColorMgr.Colors[rare - 1];
        name_txt.text = string.Format("<color=#{0}>{1}</color>", color, equip.JsonData.Name);
        state_txt.text = "";
        levle_txt.text = equip.StrengthenLv > 0 ? "+" + equip.StrengthenLv: "";
        lvfloor_cr.SetAlpha(equip.StrengthenLv);
        des_txt.text = equip.ItemData.usedes;
        equip_img.sprite = ResourceMgr.Instance.LoadSprite(equip.ItemData.icon);
        EventListener.Get(strengthen_btn.gameObject).OnClick = e =>
        {
            UIFace.GetSingleton().Close(UIID.StrengthenTip);
            UIFace.GetSingleton().Open(UIID.Strengthen, equip);
        };
        StringBuilder sb = new StringBuilder();
        Pro[] p = equip.Attribute;
        for (int i = 0, length = p.Length; i < length; ++i)
        {
            sb.Append(AttrUtil.GetAttribute(p[i].attr));
            sb.Append(": +");
            sb.Append(AttrUtil.ShowText(p[i].attr, p[i].num , p[i].per));
            sb.Append("\n");
        }
        baseattr_txt.text = attr_txt.text = sb.ToString();
        //appendattr_obj.SetActive(equip.wishs.Length > 0);
        sb.Length = 0;
        for (int i = 0, length = equip.wishs.Length; i < length; ++i)
        {
            sb.Append(AttrUtil.GetAttribute(equip.wishs[i].wish.attr));
            sb.Append(": +");
            sb.Append(AttrUtil.ShowText(equip.wishs[i].wish.attr, equip.wishs[i].wish.num , equip.wishs[i].wish.per));
            sb.Append("\n");
        }
        appendattr_txt.text = equip.wishs.Length > 0 ? sb.ToString(): "<color=#FFFF00>提升装备品质\n开放更多属性</color>";
        p = equip.Innate;
        constattr_obj.SetActive(p.Length != 0);
        sb.Length = 0;
        for (int i = 0, length = p.Length; i < length; ++i)
        {
            sb.Append(AttrUtil.GetAttribute(p[i].attr));
            sb.Append(": +");
            sb.Append(AttrUtil.ShowText(p[i].attr, p[i].num, p[i].per));
            sb.Append("\n");
        }
        constattr_txt.text = sb.ToString();
        sb = null;
    }

    public void Close()
    {

    }
}
