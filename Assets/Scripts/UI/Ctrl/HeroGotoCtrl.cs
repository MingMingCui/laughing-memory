
public class HeroGotoCtrl : UICtrlBase<HeroGotoView>
{

    int heroId = 0;

    public override void OnInit()
    {
        base.OnInit();
    }
    public override void SetParameters(object[] arg)
    {
        if (arg != null)
            heroId = (int)arg[0];
    }
    public override void OnOpen()
    {
        base.OnOpen();
        mView.Open(heroId);
    }
    public override bool OnClose()
    {
        base.OnClose();
        heroId = 0;
        return true;
    }
}
