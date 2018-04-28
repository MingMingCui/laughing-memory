using Msg;
using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitingAwardCtrl : UICtrlBase<RecruitingAwardView>
{

    public override void OnInit()
    {
        base.OnInit();
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
            InitEvent(true);
            this.mView.coppersure_btn.onClick.AddListener(delegate () { this.ClickSure(); });
            this.mView.goldsure_btn.onClick.AddListener(delegate () { this.ClickSure(); });
            this.mView.unitcoppersure_btn.onClick.AddListener(delegate () { this.ClickSure(); });
            this.mView.moreone_btn.onClick.AddListener(delegate () { this.mView.ClickMoreOne(); });
            this.mView.goldmoreone_btn.onClick.AddListener(delegate (){ this.mView.ClickMoreOne(); });
            this.mView.unitmoreone_btn.onClick.AddListener(delegate () { this.mView.ClickMoreOne(); });
            this.mView.morehundred_btn.onClick.AddListener(delegate () { this.mView.ShowTenItem(RecruitingMgr.Instance.itemlist, (int)RecruitingType.OrdinaryHundred); });
        }
        else
        {
            InitEvent(false);
            this.mView.coppersure_btn.onClick.RemoveAllListeners();
            this.mView.goldsure_btn.onClick.RemoveAllListeners(); 
            this.mView.moreone_btn.onClick.RemoveAllListeners();
            this.mView.goldmoreone_btn.onClick.RemoveAllListeners();
            this.mView.morehundred_btn.onClick.RemoveAllListeners();
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


    public void ShowOneLuckyDrawResults(Item _item,int type)
    {
         this.mView.ShowOneItem(_item, type);
    }

    public void ShowTenLuckyDrawResults(List<Item> itemlist,int type)
    {
        this.mView.ShowTenItem(itemlist,type);
    }


    public void InitEvent(bool open)
    {
        if (open)
        {
            ZEventSystem.Register(EventConst.ShowOneLuckyDrawResults, this, "ShowOneLuckyDrawResults");
            ZEventSystem.Register(EventConst.ShowTenLuckyDrawResults, this, "ShowTenLuckyDrawResults");
        }
        else
        {
            ZEventSystem.DeRegister(EventConst.ShowOneLuckyDrawResults);
            ZEventSystem.DeRegister(EventConst.ShowTenLuckyDrawResults);
        }
    }

    void ClickSure()
    {
        UIFace.GetSingleton().Close(UIID.RecruitingAward);
    }
}
