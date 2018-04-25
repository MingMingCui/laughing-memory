using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WesternShopCtrl : UICtrlBase<WesternShopView>
{
    public override void OnInit()
    {
        base.OnInit();
        this.mView.Init();
        this.mView.GoodsShow();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        WesternShopGoodsEvent(true);
        this.mView.ClickHead();
        this.mView.head_btn.onClick.AddListener(delegate () { this.mView.ClickHead(); });
        this.mView.goodsclose_btn.onClick.AddListener(delegate () { this.mView.CloseGoodsPop(); });
        this.mView.confirmbuy_btn.onClick.AddListener(delegate () { this.mView.ComfirmBuy(); });
        this.mView.Refresh_btn.onClick.AddListener(delegate () { this.mView.ClickWesternRefresh(); });
        this.mView.ok_btn.onClick.AddListener(delegate () { this.mView.ComfirmRefresh(); });
        this.mView.concel_btn.onClick.AddListener(delegate () { this.mView.CancelRefresh(); });
    }

    public override bool OnClose()
    {
        base.OnClose();
        WesternShopGoodsEvent(false);
        this.mView.head_btn.onClick.RemoveAllListeners();
        this.mView.goodsclose_btn.onClick.RemoveAllListeners();
        this.mView.confirmbuy_btn.onClick.RemoveAllListeners();
        this.mView.Refresh_btn.onClick.RemoveAllListeners();
        this.mView.ok_btn.onClick.RemoveAllListeners();
        this.mView.concel_btn.onClick.RemoveAllListeners();
        return true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    /// <summary>
    /// 货物点击事件
    /// </summary>
    /// <param name="open"></param>
    public void WesternShopGoodsEvent(bool open)
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
}
