
using JsonData;

public class NavigationCtrl : UICtrlBase<NavigationView> 
{

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        mView.Open();
        this.mView.Head.onClick.AddListener(delegate() { OpenMajesty(); });
        ZEventSystem.Register(EventConst.UpdateData, this, "UpdateData");
    }
    public void Refresh(UIConfig config)
    {
        mView.gameObject.SetLayer("UI");
        mView.ShowView(config);
    }
    public void Hide()
    {
        if(mView != null && mView.gameObject != null)
            mView.gameObject.SetLayer("HideUI");
    }

    public override bool OnClose()
    {
        base.OnClose();
        this.mView.Head.onClick.RemoveAllListeners();
        ZEventSystem.DeRegister(EventConst.UpdateData, this);
        return true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
    public void UpdateData()
    {
        this.mView.Open();
    }
    public void OpenMajesty()
    {
        UIFace.GetSingleton().Open(UIID.Majesty);
    }

}
