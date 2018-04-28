using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JsonData;
using UnityEngine.UI;


public class RecruitingView : RecruitingViewBase
{
    int HighTimes = 1;//高档免费购买次数
    TimeSpan span;//时间差
    DateTime CountDown; //目标时间
    string time="";//高档显示倒计时时间


    public void Init()
    {
        if (HighTimes > 0)
        {
            highgradecountdown_txt.text = "本次免费";
            highgradebuyoneprice_txt.text = ((int)LuckyDrawResoults.HUNITITEM).ToString();
        }
    }

    public void ShowView()
    {
        ordinarytalent_obj.SetActive(false);
        highgradetalent_obj.SetActive(false);
    }

    /// <summary>
    /// 点击普通信鸽箱子
    /// </summary>
    public void ClickOrdinaryTalent()
    {
        if (ordinarytalent_obj.activeSelf == false)
        {
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
        RecruitingMgr.Instance.type = (int)RecruitingType.OrdinaryOne;
        if (Role.Instance.Cash >= (int)LuckyDrawResoults.OUNITITEM)
        {
            UIFace.GetSingleton().Open(UIID.RecruitingAward);
            ZEventSystem.Dispatch(EventConst.ShowOneLuckyDrawResults, RecruitingMgr.Instance._item, (int)RecruitingType.OrdinaryOne);        
            Role.Instance.Cash -= (int)LuckyDrawResoults.OUNITITEM;
            Debug.Log(Role.Instance.Cash);
        }else
    CanvasView.Instance.AddNotice("钱不够！");
    }


    /// <summary>
    /// 招募普通十个
    /// </summary>
    public void ClickOrdinaryBuyTen()
    {
        RecruitingMgr.Instance.type = (int)RecruitingType.OrdinaryTen;
        if (Role.Instance.Cash >= int.Parse(ordinarybuytenprice_txt.text))
        {
            UIFace.GetSingleton().Open(UIID.RecruitingAward);
            ZEventSystem.Dispatch(EventConst.ShowTenLuckyDrawResults, RecruitingMgr.Instance.itemList, (int)RecruitingType.OrdinaryTen);          
            Role.Instance.Cash -= int.Parse(ordinarybuytenprice_txt.text);
            Debug.Log(Role.Instance.Cash);

        }else
        CanvasView.Instance.AddNotice("钱不够！");
    }


    /// <summary>
    /// 招募高档单个
    /// </summary>
    public void ClickHighGradeBuyOne()
    {
        RecruitingMgr.Instance.type = (int)RecruitingType.HighOne;
        if (HighTimes == 1)
        {
            UIFace.GetSingleton().Open(UIID.RecruitingAward);
            ZEventSystem.Dispatch(EventConst.ShowOneLuckyDrawResults,RecruitingMgr.Instance._item,(int)RecruitingType.HighOne);       
            time = DateTime.Now.AddHours(1).ToString();
            ShowHighGradeTime(true);
            return;
        }
        if (Role.Instance.Gold >= (int)LuckyDrawResoults.HUNITITEM)
        {
            UIFace.GetSingleton().Open(UIID.RecruitingAward);
            ZEventSystem.Dispatch(EventConst.ShowOneLuckyDrawResults, RecruitingMgr.Instance._item, (int)RecruitingType.HighOne); 
            if (HighTimes == 0 && highgradebuyoneprice_txt.text != "免费")
            {
                Role.Instance.Gold -= (int)LuckyDrawResoults.HUNITITEM;
            }
            Debug.Log(Role.Instance.Gold);
        }
        else CanvasView.Instance.AddNotice("钱不够！");
    }

    /// <summary>
    /// 招募高档十个
    /// </summary>
    public void ClickHighGradeBuyTen()
    {
        RecruitingMgr.Instance.type = (int)RecruitingType.HighTen;
        if (Role.Instance.Gold >= int.Parse(highgradebuytenprice_txt.text))
        {
            UIFace.GetSingleton().Open(UIID.RecruitingAward);
            ZEventSystem.Dispatch(EventConst.ShowTenLuckyDrawResults, RecruitingMgr.Instance.itemList, (int)RecruitingType.HighTen);            
            Role.Instance.Gold -= int.Parse(highgradebuytenprice_txt.text);
            Debug.Log(Role.Instance.Gold);
        }
        else CanvasView.Instance.AddNotice("钱不够！"); 
    }
    

    /// <summary>
    /// 显示免费次数
    /// </summary>
    void ShowFreeTimes()
    {
        if (HighTimes > 0)
        {
            highgradecountdown_txt.text = "本次免费";
            highgradebuyoneprice_txt.text = ((int)LuckyDrawResoults.HUNITITEM).ToString();
        }
        else
        {
            ShowHighGradeTime(false);
        }
    }

    void Update()
    {
        //高档抽奖
        if(HighTimes == 0 )
            ShowHighGradeTime(false);
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
