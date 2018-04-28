
public abstract class IUICtrl
{
    public virtual bool isOpen
    {
        get;set;
    }

    public abstract void OnInit();

    public virtual void SetParameters(object[] arg) { }

    public abstract void OnOpen();

    public abstract bool OnClose();

    public abstract void OnDestroy();

    public abstract bool IsOpen();

    public abstract void LoadUI();

    public abstract UIViewBase GetView();

    public abstract void SetView(UIViewBase view);

}
