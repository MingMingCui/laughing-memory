using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaCtrl : UICtrlBase<ArenaView> {

    public override void OnOpen()
    {
        base.OnOpen();
        this.mView.InitAreanDefend();
        this.mView.InitEnemy();
        initEvent(true);
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
            this.mView.defend_btn.onClick.AddListener(delegate() {
                UIFace.GetSingleton().Open(UIID.Stub, 0, StubType.PVPDefend);
            });
            ZEventSystem.Register(EventConst.OnStubChange, this, "OnStubChange");
        }
        else
        {
            this.mView.defend_btn.onClick.RemoveAllListeners();
            ZEventSystem.DeRegister(EventConst.OnStubChange, this);
        }
    }

    public void OnStubChange()
    {
        this.mView.InitAreanDefend();
    }

    private void watchEnemyInfo(uint uid)
    {
        UIFace.GetSingleton().Open(UIID.PlayerInfoDetail, true);
    }
}
