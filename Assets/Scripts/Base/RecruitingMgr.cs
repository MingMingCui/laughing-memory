using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonData;

public class RecruitingMgr : Singleton<RecruitingMgr>
{
    public List<Item> itemList = new List<Item>();


    /// <summary>
    /// 接收服务器传过来的物品
    /// </summary>
    /// <param name="items"></param>
    public void ServerItem(Item items)
    {
        ZEventSystem.Dispatch(EventConst.ShowOneLuckyDrawResults, items);
    }

    public void ServerTen(List<Item> items)
    {
        itemList = items;
    }

    public void ServerHero(List<int> heros)
    {
        ZEventSystem.Dispatch(EventConst.ShowAllHeros,heros);
    }
}
