using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsCtrl : UICtrlBase<TipsView> {

    private object[] _tipConfig = null;
    public override void SetParameters(object[] args)
    {
        base.SetParameters(args);
        _tipConfig = args;
    }

    public override void OnOpen()
    {
        base.OnOpen();
        initEvent(true);
    }

    public override bool OnClose()
    {
        initEvent(false);
        return true;
    }

    public void OnOpenTips(int type, object data, Vector2 pos, Alignment align, Vector2 offset, List<object> args)
    {
        this.mView.OpenTip(type, data, pos, (int)align, offset, args);
    }

    public void OnCloseTips()
    {
        this.mView.CloseTip();
    }
    private void initEvent(bool open)
    {
        if (open)
        {
            ZEventSystem.Register(EventConst.OnOpenTips, this, "OnOpenTips");
            ZEventSystem.Register(EventConst.OnCloseTips, this, "OnCloseTips");
        }
        else
        {
            ZEventSystem.DeRegister(EventConst.OnOpenTips, this);
            ZEventSystem.DeRegister(EventConst.OnCloseTips, this);
        }
    }
}
