using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.CorpsMsg;
using UnityEngine.UI;

public class CorpsMainView : CorpsMainViewBase {

    public RectTransform MemberNode = null;

    public CorpsLogNodeView LogNode = null;

    public RectTransform RequestNode = null;

    private List<CorpsMemberNodeView> uiMemberList = new List<CorpsMemberNodeView>();

    private List<CorpsRequestNodeView> uiRequestList = new List<CorpsRequestNodeView>();

    private List<CorpsLogNodeView> uiLogNodeList = new List<CorpsLogNodeView>();

    public void InitMemberList()
    {
        List<CorpsMember> memberList = CorpsMgr.Instance.CorpsMemberList;
        int needCreateNode = memberList.Count - uiMemberList.Count;
        if (needCreateNode > 0)
        {
            for (int idx = 0; idx < needCreateNode; ++idx)
            {
                GameObject node = GameObject.Instantiate(MemberNode.gameObject, this.memberlist_obj.transform);
                CorpsMemberNodeView nodeView = node.GetComponent<CorpsMemberNodeView>();
                uiMemberList.Add(nodeView);
            }
        }
        for (int idx = 0; idx < uiMemberList.Count; ++idx)
        {
            CorpsMemberNodeView nodeView = uiMemberList[idx];
            if (idx < memberList.Count)
            {
                CorpsMember info = memberList[idx];
                nodeView.SetInfo(info.RoleId, info.HeadId, info.Name, info.Level, info.Job, info.Vigour, info.Donate, info.LastOL, 
                    info.RoleId == Role.Instance.RoleId);
                nodeView.gameObject.SetActive(true);
            }
            else
                nodeView.gameObject.SetActive(false);
        }
        RectTransform nodesRect = this.memberlist_obj.GetComponent<RectTransform>();
        nodesRect.sizeDelta = new Vector2(nodesRect.sizeDelta.x, memberList.Count * (MemberNode.GetComponent<RectTransform>().sizeDelta.y + 10) + 20);
    }

    public void InitRequestList()
    {
        List<CorpsRequest> requestList = CorpsMgr.Instance.CorpsRequestList;
        this.totalrequest_txt.text = string.Format(JsonMgr.GetSingleton().GetGlobalStringArrayByID(2009).desc, requestList.Count);
        this.reqeust_members_txt.text = string.Format(JsonMgr.GetSingleton().GetGlobalStringArrayByID(2010).desc, CorpsMgr.Instance.CorpsMemberList.Count,
            CorpsMgr.Instance.GetCorpsMaxMemberByLevel());
        int needCreateNode = requestList.Count - uiRequestList.Count;
        if (needCreateNode > 0)
        {
            for (int idx = 0; idx < needCreateNode; ++idx)
            {
                GameObject node = GameObject.Instantiate(RequestNode.gameObject, this.requesters_obj.transform);
                CorpsRequestNodeView nodeView = node.GetComponent<CorpsRequestNodeView>();
                uiRequestList.Add(nodeView);
            }
        }
        for (int idx = 0; idx < uiRequestList.Count; ++idx)
        {
            CorpsRequestNodeView nodeView = uiRequestList[idx];
            if (idx < requestList.Count)
            {
                CorpsRequest info = requestList[idx];
                nodeView.SetInfo(info.RoleId, info.HeadId, info.Name, info.Level, info.FightPower, false);
                nodeView.gameObject.SetActive(true);
            }
            else
                nodeView.gameObject.SetActive(false);
        }
        RectTransform nodesRect = this.requesters_obj.GetComponent<RectTransform>();
        nodesRect.sizeDelta = new Vector2(nodesRect.sizeDelta.x, requestList.Count * 
            (RequestNode.GetComponent<RectTransform>().sizeDelta.y + 10) + 20);
    }

    public void InitLogList()
    {
        List<CorpsLogNode> logs = CorpsMgr.Instance.CorpsLogs;
        int uilogcnt = 0;
        for (int idx = 0; idx < logs.Count; ++idx)
        {
            CorpsLogNode node = logs[idx];
            if (node.log == null || node.log.Length == 0)
                continue;
            uilogcnt += (node.log.Length + 1);
        }
        int needLog = uilogcnt - uiLogNodeList.Count;
        if (needLog > 0)
        {
            for (int idx = 0; idx < needLog; ++idx)
            {
                GameObject node = GameObject.Instantiate(LogNode.gameObject, this.logs_obj.transform);
                CorpsLogNodeView nodeView = node.GetComponent<CorpsLogNodeView>();
                uiLogNodeList.Add(nodeView);
            }
        }
        int uiidx = 0;
        for (int idx = 0; idx < logs.Count; ++idx)
        {
            CorpsLogNode node = logs[idx];
            if (node.log == null || node.log.Length == 0)
                continue;
            uiLogNodeList[uiidx].SetInfo(true, node.date.ToString());      //TODO:转换格式
            uiLogNodeList[uiidx++].gameObject.SetActive(true);
            for (int logidx = 0; logidx < node.log.Length; ++logidx)
            {
                uiLogNodeList[uiidx].SetInfo(false, node.log[logidx]);
                uiLogNodeList[uiidx++].gameObject.SetActive(true);
            }
        }
        for (int idx = uilogcnt; idx < uiLogNodeList.Count; ++idx)
            uiLogNodeList[idx].gameObject.SetActive(false);

        RectTransform logs_rect = logs_obj.GetComponent<RectTransform>();
        logs_rect.sizeDelta = new Vector2(logs_rect.sizeDelta.x, uilogcnt * LogNode.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void UpdateSortUI(CorpsSortType sortType, bool isDescend)
    {
        this.level_sort_rect.gameObject.SetActive(false);
        this.job_sort_rect.gameObject.SetActive(false);
        this.vigour_sort_rect.gameObject.SetActive(false);
        this.donate_sort_rect.gameObject.SetActive(false);
        this.lastol_sort_rect.gameObject.SetActive(false);
        switch (sortType)
        {
            case CorpsSortType.Level:
                this.level_sort_rect.gameObject.SetActive(true);
                this.level_sort_rect.localScale = isDescend ? Vector3.one : new Vector3(1, -1, 1);
                break;
            case CorpsSortType.Job:
                this.job_sort_rect.gameObject.SetActive(true);
                this.job_sort_rect.localScale = isDescend ? Vector3.one : new Vector3(1, -1, 1);
                break;
            case CorpsSortType.Vigour:
                this.vigour_sort_rect.gameObject.SetActive(true);
                this.vigour_sort_rect.localScale = isDescend ? Vector3.one : new Vector3(1, -1, 1);
                break;
            case CorpsSortType.Donate:
                this.donate_sort_rect.gameObject.SetActive(true);
                this.donate_sort_rect.localScale = isDescend ? Vector3.one : new Vector3(1, -1, 1);
                break;
            case CorpsSortType.LastOl:
                this.lastol_sort_rect.gameObject.SetActive(true);
                this.lastol_sort_rect.localScale = isDescend ? Vector3.one : new Vector3(1, -1, 1);
                break;
            default:
                break;
        }
    }

}
