using Msg;
using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUICtrl: UICtrlBase<ShopUIView> 
{
    const int refreshPrice = 50; //暂时商店刷新价格
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
            _initAdditionEvent(true);
            RegisterEvent(true);
            ShopMgr.Instance.Init();
            this.mView.ButtonSide();
            this.mView.Init();
            this.mView.Refresh_btn.onClick.AddListener(delegate () { this.ClickRefresh(); });
            this.mView.goodsclose_btn.onClick.AddListener(delegate () { this.mView.ClickGoodsClose(); });
            this.mView.confirmbuy_btn.onClick.AddListener(delegate () { this.mView.ClickConfirmBuy(); });
            this.mView.shopordinary_tog.onValueChanged.AddListener((bool value)=> this.mView.OrdinaryShopBtn());
            this.mView.shoppassbarrier_tog.onValueChanged.AddListener((bool value) => this.mView.PassBarrierShopBtn());
            this.mView.shopcompetitive_tog.onValueChanged.AddListener((bool value) => this.mView.CompetitiveShopBtn());
            this.mView.shopguild_tog.onValueChanged.AddListener((bool value)=>  this.mView.GuildShopBtn());
            this.mView.head_btn.onClick.AddListener(delegate () { this.mView.ClickHead((int)ShopMgr.Instance.shoptype); });
        }
        else
        {
            _initAdditionEvent(false);
            RegisterEvent(false);
            this.mView.Refresh_btn.onClick.RemoveAllListeners();
            this.mView.goodsclose_btn.onClick.RemoveAllListeners();
            this.mView.confirmbuy_btn.onClick.RemoveAllListeners();
            this.mView.shopordinary_tog.onValueChanged.RemoveAllListeners();
            this.mView.shoppassbarrier_tog.onValueChanged.RemoveAllListeners();
            this.mView.shopcompetitive_tog.onValueChanged.RemoveAllListeners();
            this.mView.shopguild_tog.onValueChanged.RemoveAllListeners();
            this.mView.head_btn.onClick.RemoveAllListeners();
        }
    }

    public override bool OnClose()
    {
        base.OnClose();
        Init(false);
        this.mView.Close();
        return true;

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    /// <summary>
    /// 更新单个商店物品
    /// </summary>
    /// <param name="shoptype"></param>
    public void ShowUnitShop(int shoptype)
    {
        if (shoptype == (int)ShopMgr.Instance.shoptype)
        {
            this.mView.GoodsShow(shoptype);
            this.mView.UpdateUnitShop(shoptype);
            _initAdditionEvent(true);
        }
        else
            return;
    }

    public void _initAdditionEvent(bool open)
    {
        if (open)
        {
            if (this.mView.goodsList != null)
            {
                foreach (var key in this.mView.goodsList)
                {
                    key.goods_btn.onClick.AddListener(delegate () { this.mView.ClickGoods(key); });
                    key.SoldOut_btn.onClick.AddListener(delegate () { this.mView.ClickSoldBtn(key); });
                }
            }
        }
        else
        {
            if (this.mView.goodsList != null)
            {
                foreach (var key in this.mView.goodsList)
                {
                    key.goods_btn.onClick.RemoveAllListeners();
                    key.SoldOut_btn.onClick.RemoveAllListeners();
                }
            }
        }
    }

    public void RegisterEvent(bool open)
    {
        if (open)
        {
            this.mView.ShowNPC((int)ShopMgr.Instance.shoptype);
            this.mView.ClickHead((int)ShopMgr.Instance.shoptype);
            this.mView.UpdateUnitShop((int)ShopMgr.Instance.shoptype);
            ZEventSystem.Register(EventConst.ShowUnitShop, this, "ShowUnitShop");
           
        }
        else
        {
            ZEventSystem.DeRegister(EventConst.ShowNPC);
            ZEventSystem.DeRegister(EventConst.ShowUnitShop);
        }
    }
    
    /// <summary>
    /// 点击刷新按钮
    /// </summary>
    public void ClickRefresh()
    {
        Debug.Log((int)ShopMgr.Instance.shoptype);
        int times = ShopMgr.Instance.shopRefreshTime[((int)ShopMgr.Instance.shoptype)-1];
        int allPrice;
        if (times == 0)
            allPrice = 50;
        else
            allPrice = refreshPrice * times;
        TipCtrl ctrl = (TipCtrl)UIFace.GetSingleton().Open(UIID.Tip,4,(allPrice.ToString()),"元宝",(times.ToString()));
        UIFace.GetSingleton().Open(UIID.Tip);
        ctrl.SetHandler(
            delegate ()
            {
                ClickOk(false);
            },
            delegate ()
            {
                ClickOk(true);
            }
        );
    }

    /// <summary>
    /// 点击商店刷新
    /// </summary>
    /// <param name="isOk"></param>
    public void ClickOk(bool isOk)
    {
        if (isOk)
        {
            ShopMgr.Instance.shopRefreshTime[((int)ShopMgr.Instance.shoptype)-1]++;
            ShopMgr.Instance.OpenUnitShop((int)ShopMgr.Instance.shoptype, true);
            ZEventSystem.Dispatch(EventConst.RefreshTimes, (int)ShopMgr.Instance.shoptype);
        }
        UIFace.GetSingleton().Close(UIID.Tip);
    }

}
