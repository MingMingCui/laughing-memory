public class MainView : MainViewBase
{
    PositionTween pt;
    AlphaTween at;

    /// <summary>
    /// 功能菜单收缩
    /// </summary>
    /// <param name="scale"></param>
    public void Folding(bool scale)
    {
        if (pt == null)
            pt = function_obj.GetComponent<PositionTween>();
        if (folding_tog.isOn)
            pt.PlayForward();
        else
            pt.PlayReverse();
    }

    /// <summary>
    /// 未实现功能
    /// </summary>
    public void Expect()
    {
        if (at == null)
            at = expect_img.GetComponent<AlphaTween>();
        at.ResetToBeginning();
        at.PlayForward();
    }


    void Update()
    {
        if (PostBoxMgr.Instance.MailData.Count > 0 && PostBoxMgr.Instance.isUpd)
        {
            for (int idx = 0; idx < PostBoxMgr.Instance.MailData.Count; idx++)
            {
                if (PostBoxMgr.Instance.MailData[idx].isRead == 0)
                {
                    reddot_obj.SetActive(true);
                    break;
                }
                else if (idx == PostBoxMgr.Instance.MailData.Count-1)
                {
                    reddot_obj.SetActive(false);
                    PostBoxMgr.Instance.isUpd = false;
                } 
            }
        }

    }
}
