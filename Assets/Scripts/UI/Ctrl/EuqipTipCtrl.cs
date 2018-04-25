

public class EuqipTipCtrl : UICtrlBase<EquipTipView>
{
    int[] tragets;

    public override void SetParameters(object[] arg)
    {
        if (arg.Length > 0)
            tragets = (int[])arg[0];
    }
    public override void OnOpen()
    {
        base.OnOpen();
        mView.Open(tragets);
    }

    public override bool OnClose()
    {
        mView.Close();
        return base.OnClose();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

}
