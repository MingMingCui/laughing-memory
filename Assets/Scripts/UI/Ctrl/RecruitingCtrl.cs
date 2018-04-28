using System.Collections.Generic;

public class RecruitingCtrl : UICtrlBase<RecruitingView>
{

    public override void OnInit()
    {
        base.OnInit();
        this.mView.Init();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        this.mView.ShowView();
        this.mView.ordinary_btn.onClick.AddListener(delegate () { this.mView.ClickOrdinaryTalent(); });
        this.mView.highgradetalent_btn.onClick.AddListener(delegate () { this.mView.ClickHighGradeTalent(); });
        this.mView.ordinaryclose_btn.onClick.AddListener(delegate () { this.mView.ClickCloseOrdinary(); });
        this.mView.ordinarybuyone_btn.onClick.AddListener(delegate () { this.mView.ClickOrdinaryBuyOne(); });
        this.mView.highgradebuyone_btn.onClick.AddListener(delegate () { this.mView.ClickHighGradeBuyOne(); });
        this.mView.highgradebuyten_btn.onClick.AddListener(delegate () { this.mView.ClickHighGradeBuyTen(); });
        this.mView.ordinarybuyten_btn.onClick.AddListener(delegate () { this.mView.ClickOrdinaryBuyTen(); });
        this.mView.probability_btn.onClick.AddListener(delegate () { this.ShowAllHeros(); });
    }

    public override bool OnClose()
    {
        base.OnClose();
        this.mView.ordinary_btn.onClick.RemoveAllListeners();
        this.mView.highgradetalent_btn.onClick.RemoveAllListeners();
        this.mView.ordinaryclose_btn.onClick.RemoveAllListeners();
        this.mView.ordinarybuyone_btn.onClick.RemoveAllListeners();
        this.mView.highgradebuyone_btn.onClick.RemoveAllListeners();
        this.mView.highgradebuyten_btn.onClick.RemoveAllListeners();
        this.mView.ordinarybuyten_btn.onClick.RemoveAllListeners();
        this.mView.probability_btn.onClick.RemoveAllListeners();
        return true;
    }


    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    void ShowAllHeros()
    {
        UIFace.GetSingleton().Open(UIID.RecruitingProbability);
    }
}
