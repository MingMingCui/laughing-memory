using Msg;
using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajestyCtrl : UICtrlBase<MajestyView>
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
        this.mView.FrameInit(ceshi(), true);
        Incident(true);
    }

    public void Init(bool open)
    {
        if (open)
        {

            this.mView.ChangeName_btn.onClick.AddListener(delegate () { this.mView.Rename(true); });
            this.mView.Cancel_btn.onClick.AddListener(delegate () { this.mView.Rename(false); });
            this.mView.Alter_btn.onClick.AddListener(delegate () { this.IsRename(); });
            this.mView.Random_btn.onClick.AddListener(delegate () { this.mView.RandomName(); });

            this.mView.ChangePortrait_btn.onClick.AddListener(delegate () { this.mView.ChangePortrait(true); });
            this.mView.zhezhao_btn.onClick.AddListener(delegate() { this.mView.ChangePortrait(false); });
            this.mView.ChangeCircle_btn.onClick.AddListener(delegate() { this.mView.FrameInit(null,true); });
            this.mView.Frame_btn.onClick.AddListener(delegate() { this.mView.FrameInit(null, false); });
            this.mView.Logout_btn.onClick.AddListener(delegate() { Logout(); });
            this.mView.Settings_btn.onClick.AddListener(delegate() { this.mView.Expect(); });

        }
        else
        {
            Incident(false);
            this.mView.ChangeName_btn.onClick.RemoveAllListeners();
            this.mView.Cancel_btn.onClick.RemoveAllListeners();
            this.mView.Alter_btn.onClick.RemoveAllListeners();
            this.mView.Random_btn.onClick.RemoveAllListeners();
            this.mView.Frame_btn.onClick.RemoveAllListeners();
            this.mView.ChangePortrait_btn.onClick.RemoveAllListeners();
            this.mView.zhezhao_btn.onClick.RemoveAllListeners();
            this.mView.ChangeCircle_btn.onClick.RemoveAllListeners();
            this.mView.Logout_btn.onClick.RemoveAllListeners();
            this.mView.Settings_btn.onClick.RemoveAllListeners();
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

    /// <summary>
    /// 是否更改昵称
    /// </summary>
    /// <param name="isalter"></param>
    public void IsRename()
    {
            TipCtrl ctrl = (TipCtrl)UIFace.GetSingleton().Open(UIID.Tip, 5);
            UIFace.GetSingleton().Open(UIID.Tip);
            ctrl.SetHandler(
                delegate ()
                {
                    Affirm(false);
                },
                delegate ()
                {
                    Affirm(true);
                }
                );
       
    }


    public void Logout()
    {
        TipCtrl ctrl = (TipCtrl)UIFace.GetSingleton().Open(UIID.Tip, 6);
        UIFace.GetSingleton().Open(UIID.Tip);
        ctrl.SetHandler(
            delegate ()
            {
                Logout(false);
            },
            delegate ()
            {
                Logout(true);
            }
            );
    }
public void Affirm(bool rename)
{
    if (rename)
       {
       
        if (JsonMgr.GetSingleton().ExamineShieldWord(this.mView.Name_input.text))
        {
                if (Role.Instance.Gold > 100)
                {
                    Role.Instance.RoleName = this.mView.Name_input.text;
                    this.mView.Placeholder_txt.text = this.mView.Name_input.text;
                    this.mView.Name_txt.text = this.mView.Name_input.text;
                    Role.Instance.Gold -= 100;
                }
                else
                {
                    CanvasView.Instance.AddNotice("元宝不足，无法修改！");
                }
           
         }
        else
            CanvasView.Instance.AddNotice("昵称内包含屏蔽字样，请修改！");
        }
        UIFace.GetSingleton().Close(UIID.Tip);
        this.mView.ChangeName_obj.SetActive(false);
    }

    /// <summary>
    /// 为头像框添加事件
    /// </summary>
    /// <param name="isadd"></param>
    public void Incident(bool isadd)
    {
        if (isadd)
        {
            if (this.mView.PortraitList.Count > 0)
            {
                foreach (var item in this.mView.PortraitList)
                {
                    item.Select.onClick.AddListener(delegate () { this.mView.Select(item); });
                }
            }
            foreach (var item in this.mView.FrameList)
            {
                item.Frame_btn.onClick.AddListener(delegate() { this.mView.Replace(item); });
            }
        }
        else
        {
            foreach (var item in this.mView.PortraitList)
            {
                item.Select.onClick.RemoveAllListeners();
            }
            foreach (var item in this.mView.FrameList)
            {
                item.Frame_btn.onClick.RemoveAllListeners();
            }

        }
     
    }

    List<int> kuang = new List<int>();
    public List<int> ceshi()
    {

        if (kuang.Count > 0) return kuang;
        kuang.Add(20016);
        kuang.Add(20017);
        kuang.Add(20018);
        kuang.Add(20019);
        kuang.Add(20020);

        kuang.Add(20021);
        kuang.Add(20022);
        kuang.Add(20023);
        kuang.Add(20024);
        kuang.Add(20025);
        return kuang;
    }

    public void Logout(bool islogout)
    {
        if (islogout)
        {
            Client.Instance.Reset();
            SceneMgr.Instance.LoadScene("Login");
        }
        else
        {
            UIFace.GetSingleton().Close(UIID.Tip);
        }
        
    }
}
