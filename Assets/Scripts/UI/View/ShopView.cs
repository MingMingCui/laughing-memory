using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using JsonData;
using Msg.LoginMsg;



public enum ShopType
{
    none,
    ordinaryshop =1,
    passbarriershop,
    competitiveshop,
    guildshop,
    westernshop
}

public class ShopView : ShopViewBase{ 
 //   DateTime CountDown;
 //   DateTime CurTime;
 //   private int year, mouth, day, hour, min, sec;
 //   private string timeURL = "http://cgi.im.qq.com/cgi-bin/cgi_svrtime";

    

  //  public GoodsUIView goodsUIView;
 //   public List<GoodsUIView> goodsList = new List<GoodsUIView>();

 //   private int curGoodsId;//当前点击的货物id
  //  private float timer = 0;//聊天气泡的计时器
    private const int SCROLLNUM = 2;
    private const int SCROLLSIZE = 305;


    void Start ()
    {
       // StartCoroutine(GetTime());
    }

    void Update()
    {
        //  UpdateTime();
    }
   
    /// <summary>
    /// 开放商店
    /// </summary>
    public void OpenShop()
    {
        if (Role.Instance.Level <= 20)
        {
            passbarrier_btn.gameObject.SetActive(false);
            competitive_btn.gameObject.SetActive(false);
            guildshop_btn.gameObject.SetActive(false);
        }
        else if (Role.Instance.Level > 20 && Role.Instance.Level <=40)
        {
            ordinaryshop_btn.gameObject.SetActive(true);
            passbarrier_btn.gameObject.SetActive(true);
        }
        else if (Role.Instance.Level > 40 && Role.Instance.Level <=60)
        {
            ordinaryshop_btn.gameObject.SetActive(true);
            passbarrier_btn.gameObject.SetActive(true);
            competitive_btn.gameObject.SetActive(true);
        }
        else
        {
            ordinaryshop_btn.gameObject.SetActive(true);
            passbarrier_btn.gameObject.SetActive(true);
            competitive_btn.gameObject.SetActive(true);
            guildshop_btn.gameObject.SetActive(true);
        }
    }

    public void RefreshTimes(int type)
    {
        switch (type)
        {
            case 1:
                ordinaryshoptimes_txt.text = ShopMgr.Instance.shopRefreshTime[type - 1].ToString();
                break;
            case 2:
                passbarriertimes_txt.text = ShopMgr.Instance.shopRefreshTime[type - 1].ToString();
                break;
            case 3:
                competitivetimes_txt.text = ShopMgr.Instance.shopRefreshTime[type - 1].ToString();
                break;
            case 4:
                guildshoptimes_txt.text = ShopMgr.Instance.shopRefreshTime[type - 1].ToString();
                break;
            default:
                break;
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

    /// <summary>
    /// 获取网络时间
    /// </summary>
    /// <returns></returns>
    //IEnumerator  GetTime()
    //{
    //    while (true)
    //    {
    //        WWW www = new WWW(timeURL);
    //        while (!www.isDone)
    //        {
    //            yield return www;
    //        }
    //        SplitTime(www.text);
    //    }
    //}

    /// <summary>
    /// 网络时间转换成DateTime
    /// </summary>
    /// <param name="dateTime"></param>
    //void SplitTime(string dateTime)
    //{
    //    dateTime = dateTime.Replace("-", "|");
    //    dateTime = dateTime.Replace(" ", "|");
    //    dateTime = dateTime.Replace(":", "|");
    //    string[] Times = dateTime.Split('|');
    //    year = int.Parse(Times[0]);
    //    mouth = int.Parse(Times[1]);
    //    day = int.Parse(Times[2]);
    //    hour = int.Parse(Times[3]);
    //    min = int.Parse(Times[4]);
    //    sec = int.Parse(Times[5]);
    //    CurTime = new DateTime(year, mouth, day,hour, min, sec);
    //}
   
}
