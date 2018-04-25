using System.Text;

public class DivinationTipView : DivinationTipViewBase
{
    public void Open(TotemData data, SHOWBUTTON btnCtrl)
    {
        take_btn.gameObject.SetActive(btnCtrl == SHOWBUTTON.Etake);
        takeoff_btn.gameObject.SetActive(btnCtrl == SHOWBUTTON.EtakeOff);

        EventListener.Get(take_btn.gameObject).OnClick = e =>
        {
            ZEventSystem.Dispatch(EventConst.TAKETOTEM, data);
            UIFace.GetSingleton().Close(UIID.DivinationTip);
        };
        EventListener.Get(compose_btn.gameObject).OnClick = e =>
        {
            UIFace.GetSingleton().Open(UIID.CompostDivination, data);
            ZEventSystem.Dispatch(EventConst.ONOPENCOMPOSE);
            UIFace.GetSingleton().Close(UIID.DivinationTip);
        };
        EventListener.Get(takeoff_btn.gameObject).OnClick = e =>
        {
            TotemMgr.GetSingleton().TakeOffTotem(data);
            ZEventSystem.Dispatch(EventConst.REFRESHRIGHT, false);
            UIFace.GetSingleton().Close(UIID.DivinationTip);
        };
        item_img.sprite = ResourceMgr.Instance.LoadSprite(data.ItemData.icon);
        int rare = data.ItemData.rare;
        string color = ColorMgr.Colors[rare - 1];
        name_txt.text = string.Format("<color=#{0}>{1}</color>", color, data.TotemConfig.Name);
        level_txt.text = string.Format("<color=#{0}>Lv.{1}</color>", color, data.Level);
        slider_img.fillAmount = data.Exp / (float)data.LevelUpExp();
        prog_txt.text = data.Exp + "/" + data.LevelUpExp();
        Pro[] p = data.Attribute;
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < p.Length; ++i)
        {
            sb.Append(AttrUtil.GetAttribute(p[i].attr));
            sb.Append("  +");
            sb.Append(AttrUtil.ShowText(p[i].attr, p[i].num, p[i].per));
            sb.Append("\n");
        }
        attr_txt.text = sb.ToString();
    }
}
