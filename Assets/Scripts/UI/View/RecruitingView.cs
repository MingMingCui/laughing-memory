using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JsonData;

public enum LuckyDrawResoults
{
    NUM=10,
    OUNITITEM = 10000,
    OTENITEM = 90000,
    HUNITITEM = 288,
    HTENITEM=2590
}

public enum CurrencyType
{
    Copper=2000,
    Gold = 2001
}


public class RecruitingView : RecruitingViewBase
{
    public RecruitingItemView itemUI = null;
    List<RecruitingItemView> itemUIList = new List<RecruitingItemView>();
    RecruitingItemView unititem = null;
    Item _item; //当前物品

    public HeroHaveInfo hero = null;
    List<HeroHaveInfo> heros = new List<HeroHaveInfo>();
    List<int> herohead = null;
    const int UNITNUM = 6;
    const int UNITHEROSIZE = 185;

    int TIMES = 5; //普通免费购买次数
    int Timer = -1; //倒计时时间分钟
    float s = -0.1f; //秒
    bool isUpdate =false;//普通倒计时开关

    int HighTimes = 1;//高档免费购买次数
    TimeSpan span;//时间差
    DateTime CountDown; //目标时间
    string time="";//高档显示倒计时时间

 
    public void Init()
    {
        if (TIMES > 0)
        {
            ordinarybuyoneprice_txt.text = "免费";
            ordinaryfreetiems_txt.text = "今日免费次数: " + TIMES.ToString() + "/5";
        }
        if (HighTimes > 0)
        {
            highgradebuyoneprice_txt.text = "免费";
            highgradefreetimes_txt.text = "免费";
        }
    }

    public void ShowView()
    {
        ordinarytalent_obj.SetActive(false);
        highgradetalent_obj.SetActive(false);
        buyone_obj.SetActive(false);
    }

    /// <summary>
    /// 点击普通信鸽箱子
    /// </summary>
    public void ClickOrdinaryTalent()
    {
        if (ordinarytalent_obj.activeSelf == false)
        {
            ShowTime(false);
            ordinarytalent_obj.SetActive(true);
        }
    }

    /// <summary>
    /// 点击高档信鸽箱子
    /// </summary>
    public void ClickHighGradeTalent()
    {
        if ( highgradetalent_obj.activeSelf == false)
        {
            highgradetalent_obj.SetActive(true);
        }
    }

    /// <summary>
    /// 关闭普通招募
    /// </summary>
    public void ClickCloseOrdinary()
    {
        ordinarytalent_obj.SetActive(false);
    }

    /// <summary>
    /// 关闭高档招募
    /// </summary>
    public void ClickCloseHighgrade()
    {
        highgradetalent_obj.SetActive(false);
    }


    /// <summary>
    /// 招募普通单个
    /// </summary>
    public void ClickOrdinaryBuyOne()
    {
        if (Timer < 0 && TIMES > 0)
        {
            ShowTime(true);
            buyone_obj.SetActive(true);
            ShowOneItem(_item);
            return;
        }
            
        if (Role.Instance.Cash >= (int)LuckyDrawResoults.OUNITITEM)
        {
            buyone_obj.SetActive(true);
            ShowOneItem(_item);            
            Role.Instance.Cash -= (int)LuckyDrawResoults.OUNITITEM;
            Debug.Log(Role.Instance.Cash);
        }
        else Debug.Log("钱不够");
    }


    /// <summary>
    /// 招募普通十个
    /// </summary>
    public void ClickOrdinaryBuyTen()
    {
        if (Role.Instance.Cash >= int.Parse(ordinarybuytenprice_txt.text))
        {
            buyone_obj.SetActive(true);
            ShowTenItem(RecruitingMgr.Instance.itemList);
            Role.Instance.Cash -= int.Parse(ordinarybuytenprice_txt.text);
            Debug.Log(Role.Instance.Cash);
        }
        else Debug.Log("钱不够");
    }

    /// <summary>
    /// 显示单抽界面
    /// </summary>
    /// <param name="items"></param>
    public void ShowOneItem(Item items)
    {
        if (ordinarytalent_obj.activeSelf == true)
        {
            money_img.sprite = ResourceMgr.Instance.LoadSprite((int)CurrencyType.Copper);
            costprice_txt.text = ((int)LuckyDrawResoults.OUNITITEM).ToString();
        }
        else
        {
            money_img.sprite = ResourceMgr.Instance.LoadSprite((int)CurrencyType.Gold);
            costprice_txt.text = ((int)LuckyDrawResoults.HUNITITEM).ToString();
        }
        ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(30055);
        buyitemlevel_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.rare - 1]);
        buyitem_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
        buyitemname_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(30055).name;
        buyitemnum_txt.text = "";
        if (unititem == null)
        {
            unititem = ItemInfo().GetComponent<RecruitingItemView>();
            unititem.Init();
        }
        unititem.gameObject.SetActive(true);
        unititem.SetItemInfo(items.itemId, items.itemNum);
        sendoneitem_obj.SetActive(true);
        sendtenitem_obj.SetActive(false);
        _item = items;
        button_obj.SetActive(true);
    }

    /// <summary>
    /// 显示十连抽界面
    /// </summary>
    public void ShowTenItem(List<Item> items)
    {
        if (ordinarytalent_obj.activeSelf == true)
        {
            money_img.sprite = ResourceMgr.Instance.LoadSprite((int)CurrencyType.Copper);
            costprice_txt.text = ((int)LuckyDrawResoults.OTENITEM).ToString();
        }
        else
        {
            money_img.sprite = ResourceMgr.Instance.LoadSprite((int)CurrencyType.Gold);
            costprice_txt.text = ((int)LuckyDrawResoults.HTENITEM).ToString();
        }
        ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(30055);
        buyitemlevel_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.rare - 1]);
        buyitem_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
        buyitemname_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(30055).name;
        buyitemnum_txt.text = LuckyDrawResoults.NUM.ToString();
        if (unititem != null)
            unititem.gameObject.SetActive(false);
        sendtenitem_obj.SetActive(true);
        sendoneitem_obj.SetActive(false);
        if (itemUIList.Count == 0 || itemUIList.Count == items.Count)
        {
            CloseItem();
            ProcessCtrl.Instance.GoCoroutine("GetItem", GetItem(items));
        }
    }

    IEnumerator GetItem(List<Item> items)
    {
        if (button_obj.activeSelf == true) button_obj.SetActive(false);
        if (itemUIList.Count == items.Count)
        { 
            for (int idx = 0; idx < itemUIList.Count; idx++)
            {
                yield return new WaitForSeconds(0.5f);
                itemUIList[idx].gameObject.SetActive(true);
                itemUIList[idx].SetItemInfo(items[idx].itemId, items[idx].itemNum);
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                yield return new WaitForSeconds(0.5f);
                GameObject item = ItemInfo();
                RecruitingItemView itemsUIView = item.GetComponent<RecruitingItemView>();
                itemsUIView.Init();
                itemUIList.Add(itemsUIView);
                itemUIList[i].SetItemInfo(items[i].itemId, items[i].itemNum);
            }
        }
        yield return new WaitForSeconds(0.2f);
        button_obj.SetActive(true);
    }

    /// <summary>
    /// 实例化物品
    /// </summary>
    /// <returns></returns>
    public GameObject ItemInfo()
    {
        GameObject _items = Instantiate(itemUI.gameObject);
        if (_item == null)
            _items.transform.SetParent(sendoneitem_obj.transform);
        else
            _items.transform.SetParent(sendtenitem_obj.transform);
        _items.transform.localScale = Vector3.one;
        _items.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _items;
    }


    /// <summary>
    /// 招募高档单个
    /// </summary>
    public void ClickHighGradeBuyOne()
    {
        if (HighTimes == 1)
        {
            buyone_obj.SetActive(true);
            ShowOneItem(_item);
            time = DateTime.Now.AddHours(1).ToString();
            ShowHighGradeTime(true);
            return;
        }
        if (Role.Instance.Gold >= (int)LuckyDrawResoults.HUNITITEM)
        {
            buyone_obj.SetActive(true);
            ShowOneItem(_item);
            if (HighTimes == 0 && highgradebuyoneprice_txt.text != "免费")
            {
                Role.Instance.Gold -= (int)LuckyDrawResoults.HUNITITEM;
            }
            Debug.Log(Role.Instance.Gold);
        }
        else Debug.Log("钱不够");
    }

    /// <summary>
    /// 招募高档十个
    /// </summary>
    public void ClickHighGradeBuyTen()
    {
        if (Role.Instance.Gold >= int.Parse(highgradebuytenprice_txt.text))
        {
            buyone_obj.SetActive(true);
            ShowTenItem(RecruitingMgr.Instance.itemList);
            Role.Instance.Gold -= int.Parse(highgradebuytenprice_txt.text);
            Debug.Log(Role.Instance.Gold);
        }
        else Debug.Log("钱不够");
    }

    public void CloseItem()
    {
        if (itemUIList.Count == 0) return;
        for (int idx = 0; idx < itemUIList.Count; idx++)
        {
            itemUIList[idx].gameObject.SetActive(false);
        }
        button_obj.SetActive(false);
    }


    /// <summary>
    /// 关闭成功招募界面
    /// </summary>
    public void ClickSure()
    {
        buyone_obj.SetActive(false);
        if (TIMES > 0)
            ordinaryfreetiems_txt.text = "今日免费次数: " + TIMES.ToString() + "/5";
        else ordinaryfreetiems_txt.text = "今日免费次数已用完";
        if (HighTimes > 0)
            highgradefreetimes_txt.text = "免费";
        else highgradefreetimes_txt.text = "今日免费次数已用完";
    }

    /// <summary>
    /// 再一次招募
    /// </summary>
    public void ClickMoreOne()
    {
        if (costprice_txt.text == ((int)LuckyDrawResoults.OUNITITEM).ToString() || costprice_txt.text == ((int)LuckyDrawResoults.HUNITITEM).ToString())
        {
            if (costprice_txt.text == ((int)LuckyDrawResoults.OUNITITEM).ToString())
                ClickOrdinaryBuyOne();
            else ClickHighGradeBuyOne();
        }
        else if (costprice_txt.text == ((int)LuckyDrawResoults.OTENITEM).ToString() || costprice_txt.text == ((int)LuckyDrawResoults.HTENITEM).ToString())
        {
            if (costprice_txt.text == ((int)LuckyDrawResoults.OTENITEM).ToString())
                ClickOrdinaryBuyTen();
            else ClickHighGradeBuyTen();
        }
    }

    /// <summary>
    /// 显示概率界面
    /// </summary>
    public void ShowAllHero()
    {
        probability_obj.SetActive(true);
    }

    /// <summary>
    /// 显示武将
    /// </summary>
    /// <param name="heross"></param>
    public void ShowHero(List<int> heross)
    {
        for (int i = 0; i < heross.Count; i++)
        {
            Hero heroses = JsonMgr.GetSingleton().GetHeroByID(heross[i]);
            GameObject hero = LoadHero();
            HeroHaveInfo _heros = hero.GetComponent<HeroHaveInfo>();
            _heros.Init();
            _heros.SetHeroHaveInfo(heroses);
            heros.Add(_heros);
        }
        int row = 0;
        row = heross.Count / UNITNUM;
        if (row > 2)
        {
            hero_obj.GetComponent<RectTransform>().offsetMin = new Vector2(0, -(row - 2) * UNITHEROSIZE);
            hero_obj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        }
        herohead = heross;
    }

    /// <summary>
    /// 关闭所有武将
    /// </summary>
    public void CloseAllHero()
    {
        probability_obj.SetActive(false);
    }

    GameObject LoadHero()
    {
        GameObject _hero = Instantiate(hero.gameObject);
        _hero.transform.SetParent(hero_obj.transform);
        _hero.transform.localScale = Vector3.one;
        _hero.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _hero;
    }

    /// <summary>
    /// 显示免费次数
    /// </summary>
    void ShowFreeTimes()
    {
        if (ordinarybuyoneprice_txt.text == "免费" || TIMES > 0)
            ordinaryfreetiems_txt.text = "免费次数: " + TIMES.ToString() + "/5";
        else ordinaryfreetiems_txt.text = "今日免费次数已用完";

        if (HighTimes > 0)
        {
            highgradecountdown_txt.text = "";
            highgradefreetimes_txt.text = "免费";
            highgradebuyoneprice_txt.text = "免费";
        }
        else
        {
            highgradebuyoneprice_txt.text = ((int)LuckyDrawResoults.HUNITITEM).ToString();
            highgradefreetimes_txt.text = "今日免费次数已用完";
        }
           
    }


    void Update()
    {
        //普通抽奖
        if (isUpdate)
            OrdinaryTimer();
        //高档抽奖
        if(HighTimes == 0 )
            ShowHighGradeTime(false);
    }

    /// <summary>
    /// 普通免费倒计时
    /// </summary>
    /// <param name="open"></param>
    public void ShowTime(bool open)
    {
        ordinarybuytenprice_txt.text = ((int)LuckyDrawResoults.OTENITEM).ToString();
        if (DateTime.Now.Hour > 5)
        {
            if (!open)
            {
                ordinarycountdown_txt.text = "今日免费次数:" + TIMES.ToString() + "/5";
            }
            else
            {
                TIMES--;
                ordinarycountdown_txt.text = "今日免费次数:" + TIMES.ToString() + "/5";
                isUpdate = true;
                Timer = 1;
            }
        }
    }
    /// <summary>
    /// 普通倒计时
    /// </summary>
    void OrdinaryTimer()
    {
        if (TIMES > 0)
        {
            if (Timer >= 0)
            {
                int b = (int)s;
                ordinarycountdown_txt.text = "00" + ":" + GetTimeStr(Timer) + ":" + GetTimeStr(b) + " 后免费";
                if (s < 0)
                {
                    Timer--;
                    if (Timer >= 0)
                        s = 60;
                }
                s -= Time.deltaTime;
                ordinarybuyoneprice_txt.text = ((int)LuckyDrawResoults.OUNITITEM).ToString();
            }
            else
            {
                ordinarybuyoneprice_txt.text = "免费";
                isUpdate = false;
                ShowTime(false);
            }
        }
        else
        {
            ordinarycountdown_txt.text = "今日免费次数已用完";
            ordinarybuyoneprice_txt.text = ((int)LuckyDrawResoults.OUNITITEM).ToString();
        }
    }

    /// <summary>
    /// 高档免费倒计时
    /// </summary>
    /// <param name="open"></param>
    void ShowHighGradeTime(bool open)
    {
        if (time == "") return;
        span = CountDown.Subtract(DateTime.Now);
        if (!open)
        {
            highgradecountdown_txt.text = GetTimeStr(span.Hours) + ":" + GetTimeStr(span.Minutes) + ":" + GetTimeStr(span.Seconds) + " 后免费";      
            if (span <= new TimeSpan())
            {
                HighTimes = 1;
                ShowFreeTimes();
            }
        }
        else
        {
           
            HighTimes--;
            CountDown = Convert.ToDateTime(time);
            ShowFreeTimes();
        }
    }


    /// <summary>
    /// 时间显示格式
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    string GetTimeStr(int num)
    {
        if (num >= 10)
            return "" + num;
        return "0" + num;
    }
}
