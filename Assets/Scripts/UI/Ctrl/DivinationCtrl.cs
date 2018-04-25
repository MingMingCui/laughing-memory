
public class DivinationCtrl : UICtrlBase<DivinationView>
{
    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        mView.Open();
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
