using Msg;
using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPerCtrl : UICtrlBase<SelectPerView> 
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

            ZEventSystem.Register(EventConst.OnMsgOnMain, this, "OnMain");
            this.mView.Random_btn.onClick.AddListener(delegate () { this.mView.GetName(); });
            this.mView.Select_btn.onClick.AddListener(delegate () { this.mView.Switch(true); });
            this.mView.Select1_btn.onClick.AddListener(delegate () { this.mView.Switch(false); });
            this.mView.GoGame_btn.onClick.AddListener(delegate () { OnSelectPer(); });
            this.mView.Man_tog.onValueChanged.AddListener((bool value) => this.mView.CutRole(true, this.mView.Man_tog.gameObject));
            this.mView.Man1_tog.onValueChanged.AddListener((bool value) => this.mView.CutRole(true, this.mView.Man1_tog.gameObject));
            this.mView.Woman_tog.onValueChanged.AddListener((bool value) => this.mView.CutRole(false, this.mView.Woman_tog.gameObject));
            this.mView.Woman1_tog.onValueChanged.AddListener((bool value) => this.mView.CutRole(false, this.mView.Woman1_tog.gameObject));
        }
        else
        {
            ZEventSystem.DeRegister(EventConst.OnMsgOnMain, this);
            this.mView.Select1_btn.onClick.RemoveAllListeners();
            this.mView.Select_btn.onClick.RemoveAllListeners();
            this.mView.GoGame_btn.onClick.RemoveAllListeners();
            this.mView.Random_btn.onClick.RemoveAllListeners();
            this.mView.Man_tog.onValueChanged.RemoveAllListeners();
            this.mView.Woman_tog.onValueChanged.RemoveAllListeners();
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
    public void OnSelectPer()
    {

        if (this.mView.Import_input.text == "")
        {
            this.mView.Caution(this.mView.Shield1_img,true);
            Debug.Log("请输入名字");
            return;
        }
        if (this.mView.ExamineShieldWord(this.mView.Import_input.text))
        {
            OnCreateRole(this.mView.Import_input.text,this.mView.isMan,(int)this.mView.role);
        }
        else
        {
           this.mView.Caution(this.mView.Shield_img,false);
            Debug.Log("包含屏蔽或特殊字符");
        }
       
    }

    public void OnMain(ServerMsgObj msgObj)
    {
        Role.Instance.RoleName = this.mView.Import_input.text;
        Role.Instance.Sex = this.mView.isMan;
        Role.Instance.HeadId = (int)this.mView.role;
        SceneMgr.Instance.LoadScene("Main");
    }

    public void OnCreateRole(string name, int sex, int headid)
    {
        CreateRoleMsg loginMsg = new CreateRoleMsg();
        loginMsg.nickname = name;
        loginMsg.sex = sex;
        loginMsg.face_id = headid;
        Client.Instance.Send(ServerMsgId.CCMD_ROLE_REG_CHARTER, loginMsg, 0, Role.Instance.RoleId);
    }

}
