using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.CorpsMsg;

public enum CorpsSortType
{
    None = 0,
    Job = 1,
    Level = 2,
    Donate = 3,
    Vigour = 4,
    LastOl = 5,
}

public class CorpsMainCtrl : UICtrlBase<CorpsMainView> {

    //默认降序排列
    private bool isDescend = true;
    private CorpsSortType lastSortType = CorpsSortType.None;
    public CorpsSortType LastSortType
    {
        get { return lastSortType; }
        set
        {
            lastSortType = value;
            this.mView.UpdateSortUI(lastSortType, isDescend);
        }
    }

    public override void OnOpen()
    {
        base.OnOpen();
        initEvent(true);
        resort(CorpsSortType.LastOl);
        this.mView.InitLogList();
        this.mView.InitRequestList();
    }

    public override bool OnClose()
    {
        initEvent(false);
        return base.OnClose();
    }

    private void initEvent(bool open)
    {
        if (open)
        {
            //----------------------信息界面-------------------------------
            this.mView.mail_btn.onClick.AddListener(openMail);
            this.mView.corpexit_btn.onClick.AddListener(quitCorp);
            this.mView.declaremod_btn.onClick.AddListener(delegate () {
                this.openEdit();
            });
            this.mView.noticemod_btn.onClick.AddListener(delegate () {
                this.openEdit();
            });
            this.mView.corpsbattle_btn.onClick.AddListener(openBattle);
            this.mView.corpspass_btn.onClick.AddListener(openPass);
            this.mView.corpboss_btn.onClick.AddListener(openBoss);
            this.mView.corpsworship_btn.onClick.AddListener(openWorship);
            this.mView.corpsshop_btn.onClick.AddListener(openShop);
            this.mView.corpdonate_btn.onClick.AddListener(openDonate);
            //----------------------信息界面-------------------------------
            //----------------------成员界面-------------------------------
            this.mView.memberlevel_btn.onClick.AddListener(delegate() { this.resort(CorpsSortType.Level); });
            this.mView.memberjob_btn.onClick.AddListener(delegate () { this.resort(CorpsSortType.Job); });
            this.mView.membervigour_btn.onClick.AddListener(delegate () { this.resort(CorpsSortType.Vigour); });
            this.mView.memberdonate_btn.onClick.AddListener(delegate () { this.resort(CorpsSortType.Donate); });
            this.mView.memberlastol_btn.onClick.AddListener(delegate () { this.resort(CorpsSortType.LastOl); });
            //----------------------成员界面-------------------------------
        }
        else
        {
            this.mView.mail_btn.onClick.RemoveAllListeners();
            this.mView.corpexit_btn.onClick.RemoveAllListeners();
            this.mView.declaremod_btn.onClick.RemoveAllListeners();
            this.mView.noticemod_btn.onClick.RemoveAllListeners();
            this.mView.corpsbattle_btn.onClick.RemoveAllListeners();
            this.mView.corpspass_btn.onClick.RemoveAllListeners();
            this.mView.corpboss_btn.onClick.RemoveAllListeners();
            this.mView.corpsworship_btn.onClick.RemoveAllListeners();
            this.mView.corpsshop_btn.onClick.RemoveAllListeners();
            this.mView.corpdonate_btn.onClick.RemoveAllListeners();
            this.mView.memberlevel_btn.onClick.RemoveAllListeners();
            this.mView.memberjob_btn.onClick.RemoveAllListeners();
            this.mView.membervigour_btn.onClick.RemoveAllListeners();
            this.mView.memberdonate_btn.onClick.RemoveAllListeners();
            this.mView.memberlastol_btn.onClick.RemoveAllListeners();
        }
    }

    private void openMail()
    {
    }

    private void quitCorp()
    {
    }

    private void openEdit()
    {
    }

    private void openBattle()
    {
    }

    private void openPass()
    {
    }

    private void openBoss()
    {
    }

    private void openWorship()
    {
    }

    private void openShop()
    {
    }

    private void openDonate()
    {
    }

    /// <summary>
    /// 重新排序成员列表
    /// </summary>
    /// <param name="type"></param>
    private void resort(CorpsSortType type)
    {
        if (type == CorpsSortType.None)
            return;
        if (LastSortType == type)
            isDescend = !isDescend; //先修改这个否则箭头会错
        LastSortType = type;

        List<CorpsMember> memberList = CorpsMgr.Instance.CorpsMemberList;
        memberList.Sort((CorpsMember a, CorpsMember b) => {
            int mainRet = sortByType(type, a, b);
            if (mainRet != 0)
                return mainRet;
            else
            {
                foreach (int sortType in System.Enum.GetValues(typeof(CorpsSortType)))
                {
                    CorpsSortType t = (CorpsSortType)sortType;
                    if (t == CorpsSortType.None || t == type)
                        continue;
                    int sortRet = sortByType(t, a, b);
                    if (sortRet != 0)
                        return sortRet;
                }
                return 0;
            }
        });
        this.mView.InitMemberList();
    }

    private int sortByType(CorpsSortType type, CorpsMember a, CorpsMember b)
    {
        switch (type)
        {
            case CorpsSortType.Level:
                return (a.Level < b.Level ? (isDescend ? 1 : -1) : ((a.Level > b.Level) ? (isDescend ? -1 : 1) : 0));
            case CorpsSortType.Job:
                return (a.Job < b.Job ? (isDescend ? 1 : -1) : ((a.Job > b.Job) ? (isDescend ? -1 : 1) : 0));
            case CorpsSortType.Vigour:
                return (a.Vigour < b.Vigour ? (isDescend ? 1 : -1) : ((a.Vigour > b.Vigour) ? (isDescend ? -1 : 1) : 0));
            case CorpsSortType.Donate:
                return (a.Donate < b.Donate ? (isDescend ? 1 : -1) : ((a.Donate > b.Donate) ? (isDescend ? -1 : 1) : 0));
            case CorpsSortType.LastOl:
                return (lastOl2SortType(a.LastOL) < lastOl2SortType(b.LastOL) ? (isDescend ? 1 : -1) : 
                    ((lastOl2SortType(a.LastOL) > lastOl2SortType(b.LastOL)) ? (isDescend ? -1 : 1) : 0));
            default:
                return 0;
        }
    }

    private long lastOl2SortType(long lastol)
    {
        return lastol != 0 ? lastol : long.MaxValue;
    }
}
