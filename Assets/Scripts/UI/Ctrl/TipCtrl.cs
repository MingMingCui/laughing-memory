
using System.Collections.Generic;

public delegate void ClickHandler();

public class TipCtrl : UICtrlBase<TipView>
{
    int tipID;
    List<string> format = new List<string>();
    public ClickHandler cancel;
    public ClickHandler ok;

    public override void SetParameters(object[] arg)
    {
        if (arg.Length > 0)
            tipID = (int)arg[0];
        if (arg.Length > 1)
        {
            format.Clear();
            for (int idx = 1; idx < arg.Length; ++idx)
                format.Add((string)arg[idx]);
        }
    }

    public void SetHandler(ClickHandler cancel,ClickHandler ok)
    {
        this.cancel = cancel;
        this.ok = ok;
    }

    public override void OnOpen()
    {
        base.OnOpen();
        mView.SetView(tipID, format.ToArray());
        EventListener.Get(mView.left_btn.gameObject).OnClick = e =>
        {
            if (cancel != null)
                cancel();
        };
        EventListener.Get(mView.right_btn.gameObject).OnClick = e =>
        {
            if (ok != null)
                ok();
        };
    }
    public override bool OnClose()
    {
        base.OnClose();
        tipID = 0;
        return true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

}
