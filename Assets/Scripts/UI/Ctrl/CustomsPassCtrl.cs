using Msg;
using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomsPassCtrl: UICtrlBase<CustomsPassView> 
{

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        Init(true);
        this.mView.Init();
    }

    public void Init(bool open)
    {
        if (open)
        {

          //  ZEventSystem.Instance.Register(EventConst.OnMsgOnMain, this, "OnMain");
            this.mView.buy_btn.onClick.AddListener(delegate () {  });
            this.mView.operation_10_btn.onClick.AddListener(delegate () { });
            this.mView.operation_btn.onClick.AddListener(delegate () { });
            this.mView.embattle_btn.onClick.AddListener(delegate() { UIFace.GetSingleton().Open(UIID.Stub, 0); });
            this.mView.begin_btn.onClick.AddListener(delegate() { BattleMgr.Instance.BeginCombat(BattleMgr.Instance.LevelID); });
            //  this.mView.Woman1_tog.onValueChanged.AddListener((bool value) => this.mView.CutRole(false, this.mView.Woman1_tog.gameObject));
        }
        else
        {
            if (false == BattleMgr.Instance.isOff)
            {
                ZEventSystem.Dispatch(EventConst.OnClose);
            }
          
            this.mView.buy_btn.onClick.RemoveAllListeners();
            this.mView.operation_10_btn.onClick.RemoveAllListeners(); ;
            this.mView.operation_btn.onClick.RemoveAllListeners(); ;
            this.mView.embattle_btn.onClick.RemoveAllListeners(); ;
            this.mView.begin_btn.onClick.RemoveAllListeners();
            // this.mView.Woman_tog.onValueChanged.RemoveAllListeners();
        }
    }

    public override bool OnClose()
    {
        base.OnClose();
        Init(false);
        return true;

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

}
