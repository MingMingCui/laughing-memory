using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JsonData;

/// <summary>
/// 自动刷新时间
/// </summary>
public enum RefreshTime
{
    none,
    Nine,
    Moon,
    Twenty,
    TwentyFour,
    TwentyOne,
    OtherTwentyFour,
}
public class WesternShopView : WesternShopViewBase
{
    RefreshTime refresh = RefreshTime.none;
    private int year, mouth, day, hour, min, sec;
    private string timeURL = "http://cgi.im.qq.com/cgi-bin/cgi_svrtime";

    DateTime CountDown;
    DateTime CurTime;
    const int TWENTYONE = 21;

    CanvasGroup cg;
    private int curGoodsId;//当前点击的货物id
    private float timer = 0;//聊天气泡的计时器

    public GoodsUIView goodsUIView;
    public List<GoodsUIView> goodsList = new List<GoodsUIView>();

    private const int SCROLLNUM = 2;
    private const int SCROLLSIZE = 305;

    public void Init()
    {
        cg = words_img.GetComponent<CanvasGroup>();
    }

    void Start()
    {
      //  StartCoroutine(GetTime());
    }

    void Update()
    {
        WordsHidden();
     //   WesternShopRefreshTime();
    }

    /// <summary>
    /// 显示货物
    /// </summary>
    /// <param name="itemList"></param>
    public void GoodsShow()
    {
        for (int i = 0; i < ShopMgr.Instance.goodsList[(int)ShopType.westernshop].Length; i++)
        {
            GameObject shop = LoadGoodsInfo();
            GoodsUIView goodsUIView = shop.GetComponent<GoodsUIView>();
            goodsUIView.Init();
            goodsList.Add(goodsUIView);
        }
        UpdateShop();
    }

    /// <summary>
    /// 实例化货物
    /// </summary>
    /// <returns></returns>
    public GameObject LoadGoodsInfo()
    {
        GameObject _goods = GameObject.Instantiate(goodsUIView.gameObject);
        _goods.transform.SetParent(goodslist_obj.transform);
        _goods.transform.localScale = Vector3.one;
        _goods.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _goods;
    }

    /// <summary>
    /// 点击半身像弹出聊天气泡框
    /// </summary>
    /// <param name="isClick"></param>
    public void ClickHead()
    {
        words_obj.SetActive(true);
        cg.alpha = 1;
        words_txt.text = JsonMgr.GetSingleton().GetGlobalStringArrayByID(UnityEngine.Random.Range(1051, 1061)).desc;
    }

    /// <summary>
    /// 聊天气泡框的隐藏
    /// </summary>
    public void WordsHidden()
    {
        if (words_obj.activeSelf)
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                cg.alpha -= Time.deltaTime;
            }
            if (cg.alpha <= 0)
            {
                timer = 0;
                words_obj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 点击售罄货物时，半身像弹出聊天气泡框
    /// </summary>
    /// <param name="goods"></param>
    public void ClickSoldBtn(GoodsUIView goods)
    {
        words_obj.SetActive(true);
        cg.alpha = 1;
        words_txt.text = JsonMgr.GetSingleton().GetGlobalStringArrayByID(UnityEngine.Random.Range(1041, 1051)).desc;
        WordsHidden();
    }

    /// <summary>
    /// 点击货物，对货物弹框赋值
    /// </summary>
    /// <param name="goods"></param>
    public void ClickGoods(GoodsUIView goods)
    {
        Goods_obj.SetActive(true);
        GoodsUIView goodsUI = null;
        for (int i = 0; i < goodsList.Count; i++)
        {
            if (goods.goodsUIId == goodsList[i].goodsUIId)
                goodsUI = goodsList[i];
        }
        curGoodsId = goodsUI.goodsUIId;
        goodslevel_img.sprite = goodsUI.GoodsLevel_img.sprite;
        goods_img.sprite = goodsUI.Goods_img.sprite;
        goodsname_txt.text = goodsUI.GoodsName_txt.text;
        if (ItemMgr.Instance.GetItemNum(goods.goodsUIId) == 0)
        {
            goodsnum_txt.color = Color.red;
        }
        else goodsnum_txt.color = Color.yellow;
        goodsnum_txt.text = ItemMgr.Instance.GetItemNum(goods.goodsUIId).ToString();
        buyprice_txt.text = goodsUI.price_txt.text;
        if (goodsUI.goodsnum_txt.text == "")
            buynum_txt.text = "1";
        else  buynum_txt.text = goodsUI.goodsnum_txt.text;       
        buynumicon_img.sprite = goodsUI.copper_img.sprite;
        goodsproperty_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(goods.goodsUIId).propertydes;
        goodsuse_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(goods.goodsUIId).usedes;
    }

    /// <summary>
    /// 对货物赋值
    /// </summary>
    public void UpdateShop()
    {
        int CurrencyId = 20043;
        for (int i = 0; i < goodsList.Count; i++)
        {
            ItemConfig item = JsonMgr.GetSingleton().GetItemConfigByID(ShopMgr.Instance.goodsList[(int)ShopType.westernshop][i].item_id);
            if (ShopMgr.Instance.goodsList[(int)ShopType.westernshop][i].num == 1)
            {
                goodsList[i].goodsnum_txt.text = "";
                goodsList[i].GoodsName_txt.text = item.name;
            }
            else
            {
                goodsList[i].GoodsName_txt.text = item.name+ "×" + ShopMgr.Instance.goodsList[(int)ShopType.westernshop][i].num.ToString();
                goodsList[i].goodsnum_txt.text = ShopMgr.Instance.goodsList[(int)ShopType.westernshop][i].num.ToString();
            }
            goodsList[i].GoodsLevel_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[item.rare - 1]);
            goodsList[i].Goods_img.sprite = ResourceMgr.Instance.LoadSprite(item.icon);

            goodsList[i].goodsUIId = ShopMgr.Instance.goodsList[(int)ShopType.westernshop][i].item_id;
            //if (ShopMgr.Instance.goodsList[(int)ShopType.westernRegionsShop][i].saled == true)
            //    goodsList[i].SoldOut_btn.gameObject.SetActive(false);
            //else
            //    goodsList[i].SoldOut_btn.gameObject.SetActive(true);
            //if (ShopMgr.Instance.goodsList[(int)ShopType.westernRegionsShop][i].currencyType == 1)
            //    CurrencyId = 20044;
            //else
            //    CurrencyId = 20043;
            goodsList[i].copper_img.sprite = ResourceMgr.Instance.LoadSprite(CurrencyId);
            goodsList[i].price_txt.text = ShopMgr.Instance.goodsList[(int)ShopType.westernshop][i].price.ToString();
        }
        //滑动窗显示
        goodslist_obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(SCROLLNUM * SCROLLSIZE, 0);
    }

    /// <summary>
    /// 关闭货物弹框
    /// </summary>
    public void CloseGoodsPop()
    {
        Goods_obj.SetActive(false);
    }

    /// <summary>
    /// 确认购买货物
    /// </summary>
    public void ComfirmBuy()
    {
        Goods_obj.SetActive(false);
        for (int i = 0; i < goodsList.Count; i++)
        {
            if (curGoodsId == goodsList[i].goodsUIId)
            {
                goodsList[i].SoldOut_btn.gameObject.SetActive(true);
                //ShopMgr.Instance.goodsList[(int)ShopType.westernRegionsShop][i].saled = false;
            }
        }
    }

    /// <summary>
    /// 点击刷新按钮
    /// </summary>
    public void ClickWesternRefresh()
    {
        Tips_obj.SetActive(true);
    }

    /// <summary>
    /// 确认刷新
    /// </summary>
    public void ComfirmRefresh()
    {
        ShopMgr.Instance.shopRefreshTime[(int)ShopType.westernshop]++;
        refreshtime_txt.text = ShopMgr.Instance.shopRefreshTime[(int)ShopType.westernshop].ToString();
        Tips_obj.SetActive(false);
        for (int i = 0; i < goodsList.Count; i++)
        {
            goodsList[i].SoldOut_btn.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 取消刷新
    /// </summary>
    public void CancelRefresh()
    {
        Tips_obj.SetActive(false);
    }

    /// <summary>
    /// 自动刷新时间倒计时
    /// </summary>
    //public void WesternShopRefreshTime()
    //{
    //    TimeSpan span = CountDown.Subtract(CurTime);
    //    if (hour >= TWENTYONE && refresh != RefreshTime.OtherTwentyFourTime)
    //    {
    //        refresh = RefreshTime.OtherTwentyFourTime;
    //        RefreshTime_txt.text = "21:00:00";
    //        CountDown = Convert.ToDateTime("23:59:59");
    //    }
    //    else if (hour < TWENTYONE && refresh != RefreshTime.TwentyOneTime)
    //    {
    //        refresh = RefreshTime.TwentyOneTime;
    //        RefreshTime_txt.text = "21:00:00";
    //        CountDown = Convert.ToDateTime("21:00:00");
    //    }

    //    if (hour == TWENTYONE && min == 0 && sec == 0)
    //    {
    //        Debug.Log("刷新商店");
    //    }
    //    if (hour >= TWENTYONE)
    //    {
    //        DisTime_txt.text = "(" + GetTimeStr(span.Hours + 21) + ":" + GetTimeStr(span.Minutes) + ":" + GetTimeStr(span.Seconds) + ")";
    //    }
    //    else
    //        DisTime_txt.text = "(" + GetTimeStr(span.Hours) + ":" + GetTimeStr(span.Minutes) + ":" + GetTimeStr(span.Seconds) + ")";
    //}

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

    /// <summary>
    /// 获取网络时间
    /// </summary>
    /// <returns></returns>
    IEnumerator GetTime()
    {
        while (true)
        {
            WWW www = new WWW(timeURL);
            while (!www.isDone)
            {
                yield return www;
            }
            SplitTime(www.text);
        }
    }

    /// <summary>
    /// 网络时间转换成DateTime
    /// </summary>
    /// <param name="dateTime"></param>
    void SplitTime(string dateTime)
    {
        dateTime = dateTime.Replace("-", "|");
        dateTime = dateTime.Replace(" ", "|");
        dateTime = dateTime.Replace(":", "|");
        string[] Times = dateTime.Split('|');
        year = int.Parse(Times[0]);
        mouth = int.Parse(Times[1]);
        day = int.Parse(Times[2]);
        hour = int.Parse(Times[3]);
        min = int.Parse(Times[4]);
        sec = int.Parse(Times[5]);
        CurTime = new DateTime(year, mouth, day, hour, min, sec);
    }

}
