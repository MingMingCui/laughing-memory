using Msg;
using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStateCtrl : UICtrlBase<SelectStateView> 
{

    public override void OnInit()
    {
        base.OnInit();
        this.mView.Init();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        Init(true);
    }

    public void Init(bool open)
    {
        if (open)
        {

          //  ZEventSystem.Register(EventConst.OnMsgOnMain, this, "OnMain");
          //  this.mView.GoGame_btn.onClick.AddListener(delegate () { OnSelectPer(); });
            this.mView.Wei_tog.onValueChanged.AddListener((bool value) => this.mView.StateNum(States.wei));
            this.mView.Shu_tog.onValueChanged.AddListener((bool value) => this.mView.StateNum(States.shu));
            this.mView.Wu_tog.onValueChanged.AddListener((bool value) => this.mView.StateNum(States.wu));
            this.mView.Start_btn.onClick.AddListener(delegate () { this.OnCreateRole((int)this.mView.State); });
            this.mView.Random_btn.onClick.AddListener(delegate () { this.mView.RandomState(); });
        }
        else
        {
           // ZEventSystem.DeRegister(EventConst.OnMsgOnMain, this);
            this.mView.Wei_tog.onValueChanged.RemoveAllListeners();
            this.mView.Shu_tog.onValueChanged.RemoveAllListeners();
            this.mView.Wu_tog.onValueChanged.RemoveAllListeners();
            this.mView.Start_btn.onClick.RemoveAllListeners();
            this.mView.Random_btn.onClick.RemoveAllListeners();
            this.mView.zhezhao_obj.SetActive(false);
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

    public void OnCreateRole(int _state)
    {
        Client.Instance.Send(ServerMsgId.CCMD_ROLE_STATE, _state, 0, Role.Instance.RoleId);
        this.mView.Hint();
    }
    
}
