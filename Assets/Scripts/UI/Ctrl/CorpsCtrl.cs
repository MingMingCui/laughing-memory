using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpsCtrl : UICtrlBase<CorpsView> {

    private int defaultFlag = 0;

    public override void OnInit()
    {
        base.OnInit();
        defaultFlag = FlagSelectView.FlagStartId;
    }

    public override void OnOpen()
    {
        base.OnOpen();
        this.mView.InitCorpsList();
        this.mView.ChangeFlag(defaultFlag);
        this.mView.SetCost(1000, true);
        initEvent(true);
    }

    public override bool OnClose()
    {
        base.OnClose();
        initEvent(false);
        return true;
    }

    private void initEvent(bool open)
    {
        if (open)
        {
            this.mView.hidefull_tog.onValueChanged.AddListener(delegate (bool check) { onHideFullChanged(check); });
            this.mView.search_btn.onClick.AddListener(delegate () { search(); });
            this.mView.refresh_btn.onClick.AddListener(delegate () { refresh(); });
            this.mView.modifyflag_btn.onClick.AddListener(delegate () { modify(); });
            this.mView.createcorps_btn.onClick.AddListener(delegate () { create(); });
            ZEventSystem.Register(EventConst.OnSelectFlag, this, "OnSelectFlag");
        }
        else
        {
            this.mView.hidefull_tog.onValueChanged.RemoveAllListeners();
            this.mView.search_btn.onClick.RemoveAllListeners();
            this.mView.refresh_btn.onClick.RemoveAllListeners();
            this.mView.modifyflag_btn.onClick.RemoveAllListeners();
            this.mView.createcorps_btn.onClick.RemoveAllListeners();
            ZEventSystem.DeRegister(EventConst.OnSelectFlag, this);
        }
    }

    public void OnSelectFlag(int flagid)
    {
        defaultFlag = flagid;
        this.mView.ChangeFlag(defaultFlag);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void onHideFullChanged(bool check)
    {
    }

    private void search()
    {
    }

    private void refresh()
    {
    }

    private void modify()
    {
        UIFace.GetSingleton().Open(UIID.FlagSelect);
    }

    private void create()
    {
        string corpsName = this.mView.corpsname_input.text;
        string levelLimitText = this.mView.levellimit_input.text;
        int levellimit = string.IsNullOrEmpty(levelLimitText) ? 0 : int.Parse(this.mView.levellimit_input.text);
        int allowType = this.mView.allallow_tog.isOn ? 0 : 1;
        Debug.LogFormat("CreateCorps {0} {1} {2} {3} ", defaultFlag, corpsName, levellimit, allowType);
    }
}
