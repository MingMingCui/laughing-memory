
public class HeroCtrl : UICtrlBase<HeroView>
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
        base.OnClose();
        mView.Close();
        return true;
    }

    public override void OnDestroy()
    {
        mView.Destroy();
        base.OnDestroy();
    }
}
