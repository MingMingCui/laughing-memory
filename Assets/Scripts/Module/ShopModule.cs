using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.LoginMsg;

public class ShopModule : ModuleBase
{
    public override void Register()
    {
        base.Register();
        RegistEvent((int)Msg.ServerMsgId.SCMD_RESP_OPEN_SHOP, OnShop);
        RegistEvent((int)Msg.ServerMsgId.ECMD_OPEN_SHOP, OnShopFailed);
    }


    public void OnShop(ServerMsgObj msg)
    {

        ShopsData shopRes = JsonUtility.FromJson<ShopsData>(msg.Msg);
        ShopItem[] shop = null;
        int type = 0;
        if (shopRes.shop_1.Length > 0)
        {
            type = 1;
            shop = shopRes.shop_1;
        }
        else if (shopRes.shop_2.Length > 0)
        {
            type = 2;
               shop = shopRes.shop_2;
        }
        else if (shopRes.shop_3.Length > 0)
        {
            type = 3;
            shop = shopRes.shop_3;
        }
        else if (shopRes.shop_4.Length > 0)
        {
            type = 4;
            shop = shopRes.shop_4;
        }
        else if (shopRes.shop_5.Length > 0)
        {
            type = 5;
            shop = shopRes.shop_5;
        }
        Debug.Log(shop.Length);

        if(shop.Length >0)
        {
            ShopMgr.Instance.ServerGoods(type, shop);
                ZEventSystem.Dispatch(EventConst.ShowUnitShop, type);
            
        }
    }
    public void OnShopFailed(ServerMsgObj msg)
    {
        switch (msg.SubId)
        {
            case 0:
                Debug.LogFormat("OnShopFailed" + ":" + "未知错误");
                break;
            case 1:
                Debug.LogFormat("OnShopFailed" + ":" + "生成列表错误");
                break;
            default:
                Debug.LogFormat("OnShopFailed" + ":" + "未知错误"+ msg.SubId);
                break;
        }
    }
}
