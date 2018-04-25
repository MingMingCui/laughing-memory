using Msg;
using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCtrl : UICtrlBase<ShopView> {

    public override void OnInit()
    {
        base.OnInit();
      ZEventSystem.Register(EventConst.GetShopItemByType, this, "GetShopItemByType");
    }

    public override void OnOpen()
    {
        base.OnOpen();
        this.mView.OpenShop();
      
        ZEventSystem.Register(EventConst.RefreshTimes, this.mView, "RefreshTimes");
        this.mView.ordinaryshop_btn.onClick.AddListener(delegate () { this.OpenShopItem(ShopType.ordinaryshop); });
        this.mView.passbarrier_btn.onClick.AddListener(delegate () { this.OpenShopItem(ShopType.passbarriershop); });
        this.mView.competitive_btn.interactable = false;//暂时
        this.mView.guildshop_btn.interactable = false;//暂时
        this.mView.competitive_btn.onClick.AddListener(delegate () { this.OpenShopItem(ShopType.competitiveshop); });
        this.mView.guildshop_btn.onClick.AddListener(delegate () { this.OpenShopItem(ShopType.guildshop); });
    
    }

    public override bool OnClose()
    {
        base.OnClose();
        ZEventSystem.DeRegister(EventConst.RefreshTimes);
        this.mView.ordinaryshop_btn.onClick.RemoveAllListeners();
        this.mView.passbarrier_btn.onClick.RemoveAllListeners();
        this.mView.competitive_btn.onClick.RemoveAllListeners();
        this.mView.guildshop_btn.onClick.RemoveAllListeners();
        return true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void OpenShopItem(ShopType type)
    {
        ShopMgr.Instance.shoptype = type;
        UIFace.GetSingleton().Open(UIID.ShopItem);
        ShopMgr.Instance.OpenUnitShop((int)type,false);
    }

    /// <summary>
    /// 通过商店类型获取商店数据
    /// </summary>
    /// <param name="type"></param>
    public void GetShopItemByType(int type, bool isRefresh)
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
}
