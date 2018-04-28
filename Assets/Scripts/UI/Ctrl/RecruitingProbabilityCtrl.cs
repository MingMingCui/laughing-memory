using Msg;
using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitingProbabilityCtrl : UICtrlBase<RecruitingProbabilityView>
{

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        Init(true);
        this.mView.ShowHero();
    }

    public void Init(bool open)
    {
        if (open)
        {
            InitEvent(true);
        }
        else
        {
            InitEvent(false);
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

    public void ShowAllHeros()
    {
        this.mView.ShowHero();
    }

    public void InitEvent(bool open)
    {
        if (open)
        {

            ZEventSystem.Register(EventConst.ShowAllHeros, this, "ShowAllHeros");
        }
        else
        {
            ZEventSystem.DeRegister(EventConst.ShowAllHeros);
        }
    }
}
