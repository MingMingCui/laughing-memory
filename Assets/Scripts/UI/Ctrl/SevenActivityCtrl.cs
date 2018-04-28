using Msg;
using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SevenActivityCtrl : UICtrlBase<SevenActivityView>
{

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        Init(true);
        this.mView.AwardID = 1001;
        this.mView.SkyNum(this.mView.AwardID);
    }

    public void Init(bool open)
    {
        if (open)
        {

            ZEventSystem.Register(EventConst.OnGetRegisterSky, this, "OnGetRegisterSky");
            this.mView.one_btn.onClick.AddListener(delegate () { this.mView.SkyNum(1001); });
            this.mView.tow_btn.onClick.AddListener(delegate () { this.mView.SkyNum(1002); });
            this.mView.three_btn.onClick.AddListener(delegate () { this.mView.SkyNum(1003); });
            this.mView.four_btn.onClick.AddListener(delegate () { this.mView.SkyNum(1004); });
            this.mView.five_btn.onClick.AddListener(delegate () { this.mView.SkyNum(1005); });
            this.mView.six_btn.onClick.AddListener(delegate () { this.mView.SkyNum(1006); });
            this.mView.seven_btn.onClick.AddListener(delegate () { this.mView.SkyNum(1007); });
            this.mView.Affirm_btn.onClick.AddListener(delegate () { this.mView.GetAward(); });
            //  this.mView.Woman1_tog.onValueChanged.AddListener((bool value) => this.mView.CutRole(false, this.mView.Woman1_tog.gameObject));
        }
        else
        {
            ZEventSystem.DeRegister(EventConst.OnGetRegisterSky, this);
            //  this.mView.Select1_btn.onClick.RemoveAllListeners();
            // this.mView.Woman_tog.onValueChanged.RemoveAllListeners();
            this.mView.one_btn.onClick.RemoveAllListeners();
            this.mView.tow_btn.onClick.RemoveAllListeners();
            this.mView.three_btn.onClick.RemoveAllListeners();
            this.mView.four_btn.onClick.RemoveAllListeners();
            this.mView.five_btn.onClick.RemoveAllListeners();
            this.mView.six_btn.onClick.RemoveAllListeners();
            this.mView.seven_btn.onClick.RemoveAllListeners();
            this.mView.Affirm_btn.onClick.RemoveAllListeners();
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

    public void OnGetRegisterSky(int _sky)
    {
        this.mView.AwardID = _sky;
    }
}
