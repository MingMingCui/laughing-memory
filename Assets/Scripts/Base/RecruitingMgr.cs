using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonData;

public enum RecruitingType
{
    None,
    OrdinaryOne,  //普通单个
    OrdinaryTen,  //普通十连抽
    OrdinaryHundred, //普通百连抽
    HighOne,      //高档单个
    HighTen       //高档十连抽
}

public enum LuckyDrawResoults
{
    HUNDREDNUM = 100,
    NUM = 10,
    OUNITITEM = 10000,
    OTENITEM = 90000,
    OHUNDRED = 900000,
    HUNITITEM = 288,
    HTENITEM = 2590,
}

public enum CurrencyType
{
    Copper = 2000,
    Gold = 2001
}

public class RecruitingMgr : Singleton<RecruitingMgr>
{
    public List<Item> itemList = new List<Item>(); // 十连抽
    public List<Item> itemlist = new List<Item>(); //百连抽
    public Item _item; //当前物品
    public int type = (int)RecruitingType.None;
    public List<int> heroList = new List<int>();
    /// <summary>
    /// 接收服务器传过来的物品
    /// </summary>
    /// <param name="items"></param>
    public void ServerItem(Item items)
    {
        _item = items;
    }

    public void ServerTen(List<Item> items)
    {
        itemList = items;
    }

    public void ServerHundred(List<Item> items)
    {
        itemlist = items;
    }

    public void ServerHero(List<int> heros)
    {
       
        heroList = heros; 
        ZEventSystem.Dispatch(EventConst.ShowAllHeros);
    }
}
