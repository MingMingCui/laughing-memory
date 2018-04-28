using Msg;
using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostBoxCtrl: UICtrlBase<PostBoxView> 
{

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnOpen()
    {
        base.OnOpen();
       // this.mView.Close();
        Init(true);
        this.mView.Init(PostBoxMgr.Instance.MailData);
    }

    public void Init(bool open)
    {
        if (open)
        {
           
            ZEventSystem.Register(EventConst.Incident, this, "Incident");
            ZEventSystem.Register(EventConst.OnMailItemIncident, this, "OnMailItemIncident");
            this.mView.Close_btn.onClick.AddListener(delegate () { this.Close(); });
            this.mView.totala_tog.onValueChanged.AddListener((bool value) => this.mView.GetMail(MailTyper.zero));
            this.mView.noticea_tog.onValueChanged.AddListener((bool value) => this.mView.GetMail(MailTyper.one));
            this.mView.awarda_tog.onValueChanged.AddListener((bool value) => this.mView.GetMail(MailTyper.two));
            this.mView.lettera_tog.onValueChanged.AddListener((bool value) => this.mView.GetMail(MailTyper.three));
            
        }
        else
        {
             ZEventSystem.DeRegister(EventConst.Incident, this);
            ZEventSystem.DeRegister(EventConst.OnMailItemIncident, this);
            this.mView.Close_btn.onClick.RemoveAllListeners();
            
            this.mView.totala_tog.onValueChanged.RemoveAllListeners();
            this.mView.noticea_tog.onValueChanged.RemoveAllListeners();
            this.mView.awarda_tog.onValueChanged.RemoveAllListeners();
            this.mView.lettera_tog.onValueChanged.RemoveAllListeners();
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
    public void Close()
    {
        UIFace.GetSingleton().Close(UIID.Mail);
    }

    public void OnMailItemIncident(bool _open, MailDatas _mailvoew)
    {
        if (_open)
        {
                if (_mailvoew.mailitem.Count == 0)
                {
                    this.mView.Draw_btn.onClick.AddListener(delegate () { this.mView.Destroy(false,_mailvoew); });
                    this.mView.deletea_btn.onClick.AddListener(delegate () { this.mView.Destroy(true,_mailvoew); });
                }
                else
                this.mView.Get_btn.onClick.AddListener(delegate () { this.mView.Destroy(true, _mailvoew); });
            this.mView.MailaWard_btn.onClick.AddListener(delegate () { this.mView.Close(false, _mailvoew); });
        }
        else
        {
            this.mView.MailaWard_btn.onClick.RemoveAllListeners();
            this.mView.deletea_btn.onClick.RemoveAllListeners();
            this.mView.Get_btn.onClick.RemoveAllListeners();
            this.mView.Draw_btn.onClick.RemoveAllListeners();
        }
    }
    public void Incident(bool _open)
    {
        if (_open)
        {
                foreach (var item in this.mView.MailList)
                {
                  if(0 == item.isUse)
                  {
                    item.Mail.onClick.AddListener(delegate () { this.mView.OpenMail(item); });
                    item.isUse = 1;
                  }
                   
                }
        }
        else
        {
            foreach (var item in this.mView.MailList)
            {
                item.Mail.onClick.RemoveAllListeners();
            }
        }
    }


}
