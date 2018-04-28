public enum SHOWBUTTON
{
    Ecompose,
    EtakeOff,
    Etake
}

public class DivinationTip : UICtrlBase<DivinationTipView>
{
    TotemData td;
    SHOWBUTTON e = SHOWBUTTON.Ecompose;
    public override void SetParameters(object[] arg)
    {
        if (arg.Length > 0)
            td = (TotemData)arg[0];
        if (arg.Length > 1)
            e = (SHOWBUTTON)arg[1];
              
    }
    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        mView.Open(td, e);
    }

    public override bool OnClose()
    {
        return base.OnClose();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
