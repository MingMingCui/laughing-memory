
using JsonData;

public class TipView : TipViewBase
{
    public void SetView(int tipId, string[] format)
    {
        Tip tip = JsonMgr.GetSingleton().GetTipByID(tipId);
        left_txt.supportRichText = right_txt.supportRichText = tip.Rich;
        left_txt.text = tip.Left;
        right_txt.text = tip.Right;
        content_txt.text = string.Format(tip.Content, format);
    }
}
