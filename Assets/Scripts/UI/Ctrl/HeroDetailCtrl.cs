
public class HeroDetail : UICtrlBase<HeroDetailView>
{
    int heroID;
    public EHEROSHOWTYPE type;
    public override void OnInit()
    {
        base.OnInit();
    }
    public override void SetParameters(object[] arg)
    {
        if (arg.Length > 0)
            heroID = (int)arg[0];
        if (arg.Length > 1)
            type = (EHEROSHOWTYPE)arg[1];
    }
    public override void OnOpen()
    {
        base.OnOpen();
        mView.Open(heroID, type);
    }
    public override bool OnClose()
    {
        base.OnClose();
        mView.Close();
        type = EHEROSHOWTYPE.Card;
        return true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

}
