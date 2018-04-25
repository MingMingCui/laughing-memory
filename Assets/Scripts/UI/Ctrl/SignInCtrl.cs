using Msg;
using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SignInCtrl: UICtrlBase<SignInView> 
{
    public override void OnInit()
    {
        base.OnInit();
        OnSignIc(System.DateTime.Now.Month * 100 + System.DateTime.Now.Day, false, 11270);


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
            OnSignIc(System.DateTime.Now.Month * 100 + System.DateTime.Now.Day, true, 11270);
            ZEventSystem.Register(EventConst.OnSignIc, this, "OnSignIc");
            this.mView.Specifier_btn.onClick.AddListener(delegate () { this.mView.Specifier_obj.SetActive(true); });
            this.mView.close_btn.onClick.AddListener(delegate () { this.mView.Specifier_obj.SetActive(false); });
            this.mView.Ensure_btn.onClick.AddListener(delegate () { this.mView.GetAward_obj.SetActive(false); });
            //  this.mView.GoGame_btn.onClick.AddListener(delegate () { OnSelectPer(); });
            //  this.mView.Woman1_tog.onValueChanged.AddListener((bool value) => this.mView.CutRole(false, this.mView.Woman1_tog.gameObject));
        }
        else
        {
             ZEventSystem.DeRegister(EventConst.OnSignIc, this);
            this.mView.Specifier_btn.onClick.RemoveAllListeners();
            this.mView.close_btn.onClick.RemoveAllListeners();
            this.mView.Ensure_btn.onClick.RemoveAllListeners();
            //  this.mView.Select1_btn.onClick.RemoveAllListeners();
            // this.mView.Woman_tog.onValueChanged.RemoveAllListeners();
        }
    }

    public override bool OnClose()
    {
        base.OnClose();
        Init(false);
      this.mView.Close();
        return true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void OnSignIc(int _id,bool _isUpdate,int _heroid)
    {
        if (_isUpdate)
            this.mView.SignIn(_id);
        else
            this.mView.Init(_id, _isUpdate, _heroid);

    }

}
