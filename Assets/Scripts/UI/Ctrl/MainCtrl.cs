using Msg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCtrl : UICtrlBase<MainView> {

    public override void OnInit()
    {
        base.OnInit();
        //  Init();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        if (System.DateTime.Now.Month*100+ System.DateTime.Now.Day != Role.Instance.SignInID)
        {
           // OpenSignin();
        }
       
        Init(true);
        
        
    }

    public void Init(bool open)
    {
        if (open)
        {
            this.mView.Battle_btn.onClick.AddListener(delegate () { this.OpenBattle(); });
            this.mView.folding_tog.onValueChanged.AddListener((bool value) => this.mView.Folding(value));
            this.mView.knapsack_btn.onClick.AddListener(delegate () { this.ClickKnapsack(); });
            EventListener.Get(this.mView.warrior_btn.gameObject).OnClick = o =>
            {
                UIFace.GetSingleton().Open(UIID.Hero);
            };
            this.mView.embattle_btn.onClick.AddListener(delegate () {
                UIFace.GetSingleton().Open(UIID.Stub);
            });
            this.mView.task_btn.onClick.AddListener(delegate () { this.mView.Expect(); });
            this.mView.daily_btn.onClick.AddListener(delegate () {
                //UIFace.GetSingleton().Open((int)UIID.CorpsMain);
                //UIFace.GetSingleton().Open(UIID.Corps);
                //UIFace.GetSingleton().Open(UIID.CorpsMain);
                //UIMgr.Instance.OpenUI((int)UIID.PlayerInfo, false);
                //UIFace.GetSingleton().Open(UIID.PlayerInfoDetail, false);
                this.mView.Expect();
                //UIFace.GetSingleton().Open(UIID.Arena);
            });
            this.mView.GM_btn.onClick.AddListener(delegate() { this.OpenGM(); });
            this.mView.chat_btn.onClick.AddListener(delegate () {
                //UIFace.GetSingleton().Open(UIID.PlayerInfo, true);
                this.mView.Expect();
                //Vector2Int[] data = new Vector2Int[2];
                //data[0] = new Vector2Int(11000, 99);
                //data[1] = new Vector2Int(2000, 15000000);
                //TipsMgr.OpenSimpleTip(new Vector2(0, 0), 1001, Alignment.LB, Vector2.zero, "test");
                //TipsMgr.OpenTreasureTip(new Vector2(0, 0), data, Alignment.LB, new Vector2(0, 0));
                //TipsMgr.OpenMonsterTip(new Vector2(0, 0), 20001, false, Alignment.LB, new Vector2(0, 0));
                //TipsMgr.OpenItemTip(new Vector2(0, 0), 30050, Alignment.LB, new Vector2(0, 0));
                //TipsMgr.OpenItemTip(new Vector2(0, 0), 11000, Alignment.LB, new Vector2(0, 0));
            });
            this.mView.chat_Txt_btn.onClick.AddListener(delegate () { this.mView.Expect(); });
            this.mView.State_btn.onClick.AddListener(delegate () { this.Open(); });
            this.mView.Award_btn.onClick.AddListener(delegate () { this.OpenSevenActivity(); });
            this.mView.Signin_btn.onClick.AddListener(delegate () { this.OpenSignin(); });
            this.mView.Shop_btn.onClick.AddListener(delegate () { this.OpenShop(); });
            this.mView.WesternShop_btn.onClick.AddListener(delegate () { this.OpenWesternShop(); });
            this.mView.Recruiting_btn.onClick.AddListener(delegate () { this.OpenRecruiting(); });
            this.mView.Mail_btn.onClick.AddListener(delegate () { this.OpenMail(); });
            EventListener.Get(mView.divination_btn.gameObject).OnClick = e =>
            {
                UIFace.GetSingleton().Open(UIID.Divination);
            };
        }
        else
        {
            this.mView.folding_tog.onValueChanged.RemoveAllListeners();
            this.mView.knapsack_btn.onClick.RemoveAllListeners();
            this.mView.Battle_btn.onClick.RemoveAllListeners();
            this.mView.warrior_btn.onClick.RemoveAllListeners();
            this.mView.embattle_btn.onClick.RemoveAllListeners();
            this.mView.task_btn.onClick.RemoveAllListeners();
            this.mView.daily_btn.onClick.RemoveAllListeners();
            this.mView.chat_btn.onClick.RemoveAllListeners();
            this.mView.chat_Txt_btn.onClick.RemoveAllListeners();
            this.mView.Award_btn.onClick.RemoveAllListeners();
            this.mView.Signin_btn.onClick.RemoveAllListeners();
            this.mView.Shop_btn.onClick.RemoveAllListeners();
            this.mView.WesternShop_btn.onClick.RemoveAllListeners();
            this.mView.Recruiting_btn.onClick.RemoveAllListeners();
            this.mView.GM_btn.onClick.RemoveAllListeners();
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
    /// 开启背包界面
    /// </summary>
    public void ClickKnapsack()
    {
        UIFace.GetSingleton().Open(UIID.Bag);
    }
    public void Open()
    {

        UIFace.GetSingleton().Open(UIID.SelectCountry);
    }
    //void Init()
    //{
    //    if (ItemMgr.Instance.itemList.Count != 0) return;
    //    List<Item> item1 = new List<Item>();
    //    if (item1.Count == 0)
    //    {
    //        for (int i = 0; i < 5; i++)
    //        {
    //            Item item2 = new Item();
    //            item2.itemId = 1001 + i;
    //            item2.itemNum = 35 + i;
    //            item1.Add(item2);
    //        }
    //    }
    //    ItemMgr.Instance.ServerUpdateItemList(item1);
    //}
    void OpenSevenActivity()
    {
        UIFace.GetSingleton().Open(UIID.Award);
    }
    void OpenSignin()
    {
        UIFace.GetSingleton().Open(UIID.SignIn);
    }


    void OpenShop()
    {
        UIFace.GetSingleton().Open(UIID.Shop);
    }

    void OpenWesternShop()
    {
        ShopMgr.Instance.shoptype = ShopType.westernshop;
        UIFace.GetSingleton().Open(UIID.ShopItem);
        if (!ShopMgr.Instance.goodsList.ContainsKey((int)ShopMgr.Instance.shoptype))
            WesternEvent((int)ShopMgr.Instance.shoptype,true);
        else
           WesternEvent((int)ShopMgr.Instance.shoptype, false);
    }

    void WesternEvent(int type,bool isRefresh)
    {
        ZEventSystem.Dispatch(EventConst.ShowNPC, type);
        if (false == ShopMgr.Instance.goodsList.ContainsKey(type) || isRefresh)
        {
            Client.Instance.Send(ServerMsgId.CCMD_OPEN_SHOP, null, (short)type, Role.Instance.RoleId);
        }
        else
        {
            UIFace.GetSingleton().Open(UIID.ShopItem);
        }
    }

    void OpenRecruiting()
    {
        UIFace.GetSingleton().Open(UIID.Recruiting);
    }
    void OpenMail()
    {
        UIFace.GetSingleton().Open(UIID.Mail);
    }
    void OpenGM()
    {
        UIFace.GetSingleton().Open(UIID.GM);
    }
    /// <summary>
    /// 进入战役
    /// </summary>
    public void OpenBattle()
    {
        UIFace.GetSingleton().Open(UIID.Battle);
    }
}
