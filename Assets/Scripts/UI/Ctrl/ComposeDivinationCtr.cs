public class ComposeDivinationCtr : UICtrlBase<ComposeDivinationView>
{
    TotemData data;

    public override void SetParameters(object[] arg)
    {
        if (arg.Length > 0)
            data = (TotemData)arg[0];
    }

    public override void OnOpen()
    {
        base.OnOpen();
        mView.Open(data);
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
