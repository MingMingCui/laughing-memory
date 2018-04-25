
public class StarUpCtrl : UICtrlBase<StarUpView>
{
    int heroId = 0;

    public override void SetParameters(object[] arg)
    {
        if (arg != null)
            heroId = (int)arg[0];
    }

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        mView.Open(heroId);
    }
    public override bool OnClose()
    {
        base.OnClose();
        mView.Close();
        return true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
