using Msg.LoginMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMgr : Singleton<ShopMgr> {
    
    public Dictionary<int,ShopItem[]> goodsList = new Dictionary<int, ShopItem[]>();
   // public List<Goods> wgoodsList = new List<Goods>();
    const int goodsListLength = 12; //每个商店货物种类数量
    const int shopNum = 5; //商店数量

    public  RefreshTime refresh = RefreshTime.none;
    public ShopType shoptype = ShopType.none;

    public List<int> shopRefreshTime = new List<int>(); //刷新次数

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Init()
    {
        if (shopRefreshTime.Count > 0) return;
        for (int idx = 0; idx < shopNum; idx++)
        {
            shopRefreshTime.Add(new int());
        }
    }
  

    /// <summary>
    /// 打开某一个商店
    /// </summary>
    /// <param name="type"></param>
    public void OpenUnitShop(int type,bool isRefresh)
    {
        ZEventSystem.Dispatch(EventConst.GetShopItemByType,type,isRefresh);
    }
   

    /// <summary>
    /// 接收服务器传过来的数据
    /// </summary>
    /// <param name="goods"></param>
    public void ServerGoods(int type,ShopItem[] goods)
    {
        if (goodsList.ContainsKey(type) == false)
            goodsList.Add(type, goods);
        else
        {
            goodsList.Remove(type);
            goodsList.Add(type, goods);
        }
    }

    public int GetItemID(int type,int id)
    {
        if (false == goodsList.ContainsKey(type))
        {
            Debug.Log("不包含此商店类型"+type);
                return 0; }
        int itemID = 0;
        for (int i = 0; i < goodsList[type].Length; i++)
        {
            if (id == goodsList[type][i].item)
            {
                itemID = goodsList[type][i].item_id;
                break;
            } 
            else if(i == goodsList[type].Length-1)
            {
                Debug.Log("不包含此物品" + id);
                return 0;
        }
        }
        return itemID;
    }

    /// <summary>
    /// 通过顺序id获取物品id
    /// </summary>
    /// <param name="type">商店类型</param>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetItemIDByUIID(int type,int id)
    {
        if (false == goodsList.ContainsKey(type))
        {
            Debug.Log("不包含此商店类型" + type);
            return -1;
        }
        int itemID = 0;
        for (int i = 0; i < goodsList[type].Length; i++)
        {
            if (id == goodsList[type][i].item)
            {
                itemID = goodsList[type][i].item_id;
                break;
            }
            else if (i == goodsList[type].Length - 1)
            {
                Debug.Log("不包含此物品" + id);
                return 0;
            }
        }
        return itemID;
    }
}
