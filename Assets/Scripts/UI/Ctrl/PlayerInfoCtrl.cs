using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoCtrl : UICtrlBase<PlayerInfoView> {

    public override void SetParameters(object[] arg)
    {
        base.SetParameters(arg);
        if (arg.Length > 0)
        {
            isSmallSize = (bool)arg[0];
        }
    }

    private bool isSmallSize = false;

    public override void OnOpen()
    {
        base.OnOpen();
        initEvent(true);
        this.mView.SetViewSize(isSmallSize);
    }

    public override bool OnClose()
    {
        initEvent(false);
        return base.OnClose();
    }

    private void initEvent(bool open)
    {
        if (open)
        {
            this.mView.close_btn.onClick.AddListener(() => { UIFace.GetSingleton().Close(UIID.PlayerInfo);});
        }
        else
        {
            this.mView.close_btn.onClick.RemoveAllListeners();
        }
    }
}
