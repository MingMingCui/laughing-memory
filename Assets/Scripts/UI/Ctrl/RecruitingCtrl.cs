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
        InitEvent(true);
        this.mView.ShowView();
        this.mView.ordinary_btn.onClick.AddListener(delegate () { this.mView.ClickOrdinaryTalent(); });
        this.mView.highgradetalent_btn.onClick.AddListener(delegate () { this.mView.ClickHighGradeTalent(); });
        this.mView.ordinaryclose_btn.onClick.AddListener(delegate () { this.mView.ClickCloseOrdinary(); });
        this.mView.highgradeclose_btn.onClick.AddListener(delegate () { this.mView.ClickCloseHighgrade(); });
        this.mView.ordinarybuyone_btn.onClick.AddListener(delegate () { this.mView.ClickOrdinaryBuyOne(); });
        this.mView.highgradebuyone_btn.onClick.AddListener(delegate () { this.mView.ClickHighGradeBuyOne(); });
        this.mView.highgradebuyten_btn.onClick.AddListener(delegate () { this.mView.ClickHighGradeBuyTen(); });
        this.mView.sure_btn.onClick.AddListener(delegate () { this.mView.ClickSure(); });
        this.mView.moreone_btn.onClick.AddListener(delegate () { this.mView.ClickMoreOne(); });
        this.mView.ordinarybuyten_btn.onClick.AddListener(delegate () { this.mView.ClickOrdinaryBuyTen(); });
        this.mView.probability_btn.onClick.AddListener(delegate () { this.mView.ShowAllHero(); });
        this.mView.closeprobability_btn.onClick.AddListener(delegate () { this.mView.CloseAllHero(); });
    }

    public override bool OnClose()
    {
        base.OnClose();
        InitEvent(false);
        this.mView.ordinary_btn.onClick.RemoveAllListeners();
        this.mView.highgradetalent_btn.onClick.RemoveAllListeners();
        this.mView.ordinaryclose_btn.onClick.RemoveAllListeners();
        this.mView.highgradeclose_btn.onClick.RemoveAllListeners();
        this.mView.ordinarybuyone_btn.onClick.RemoveAllListeners();
        this.mView.highgradebuyone_btn.onClick.RemoveAllListeners();
        this.mView.highgradebuyten_btn.onClick.RemoveAllListeners();
        this.mView.sure_btn.onClick.RemoveAllListeners();
        this.mView.moreone_btn.onClick.RemoveAllListeners();
        this.mView.ordinarybuyten_btn.onClick.RemoveAllListeners();
        this.mView.probability_btn.onClick.RemoveAllListeners();
        this.mView.closeprobability_btn.onClick.RemoveAllListeners();
        return true;
    }


    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void ShowOneLuckyDrawResults(Item _item)
    {
        this.mView.ShowOneItem(_item);
    }

    //public void ShowTenLuckyDrawResults(List<Item> itemlist)
    //{
    //    this.mView.itemList = itemlist;
    //}

    public void ShowAllHeros(List<int> heros)
    {
        this.mView.ShowHero(heros);
    }

    public void InitEvent(bool open)
    {
        if (open)
        {
            ZEventSystem.Register(EventConst.ShowOneLuckyDrawResults ,this, "ShowOneLuckyDrawResults");
            //ZEventSystem.Register(EventConst.ShowTenLuckyDrawResults, this, "ShowTenLuckyDrawResults");
            ZEventSystem.Register(EventConst.ShowAllHeros, this, "ShowAllHeros");

        }
        else
        {
            ZEventSystem.DeRegister(EventConst.ShowOneLuckyDrawResults);
          //  ZEventSystem.DeRegister(EventConst.ShowTenLuckyDrawResults);
            ZEventSystem.DeRegister(EventConst.ShowAllHeros);
        }
    }
}
