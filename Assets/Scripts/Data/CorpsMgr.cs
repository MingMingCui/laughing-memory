using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.CorpsMsg;

public class CorpsMgr : Singleton<CorpsMgr> {

    public CorpsMgr()
    {
        for (int idx = 0; idx < 10; ++idx)
        {
            CorpsMember member = new CorpsMember();
            member.RoleId = (uint)idx;
            member.Name = string.Format("董大喵{0}", idx);
            member.Level = idx * 10;
            member.Job = idx / 4;
            member.HeadId = 1;
            member.Vigour = 1000 + idx;
            member.Donate = 9999 - idx;
            member.LastOL = 0;

            CorpsMemberList.Add(member);
        }

        for (int idx = 0; idx < 5; ++idx)
        {
            CorpsRequest request = new CorpsRequest();
            request.RoleId = (uint)idx;
            request.Name = string.Format("董大喵{0}", idx);
            request.Level = 10 + idx;
            request.HeadId = 1;
            request.FightPower = 10000 + idx;
            CorpsRequestList.Add(request);
        }

        for (int idx = 0; idx < 5; ++idx)
        {
            CorpsLogNode log = new CorpsLogNode();
            log.date = idx + 1;
            log.log = new string[idx + 1];
            for (int idx2 = 0; idx2 < log.log.Length; ++idx2)
                log.log[idx2] = string.Format("日志日志日志日志{0}", idx2);
            CorpsLogs.Add(log);
        }

        for (int idx = 0; idx < 10; ++idx)
        {
            CorpsInfo info = new CorpsInfo();
            info.UID = (uint)idx;
            info.name = string.Format("军团名称{0}", idx);
            info.level = idx;
            info.flag = 20051 + idx;
            info.camp = idx % 3;
            info.leader = string.Format("董大喵{0}", idx);
            info.limit = 0;
            info.members = 40 + idx;
            info.state = 0;
            CorpsList.Add(info);
        }
    }

    public List<CorpsInfo> CorpsList = new List<CorpsInfo>();

    public List<CorpsMember> CorpsMemberList = new List<CorpsMember>();

    public List<CorpsLogNode> CorpsLogs = new List<CorpsLogNode>();

    public List<CorpsRequest> CorpsRequestList = new List<CorpsRequest>();

    public uint CorpsId;                        //军团id
    public int Flag;                            //旗帜id
    public int Level;                           //军团等级
    public int Exp;                             //军团经验
    public int Name;                            //军团名称
    public int Rank;                            //军团排名
    public int Power;                           //军团战斗力
    public int TodayDonate;                     //今日贡献
    public long TotalDonate;                    //总贡献
    public string Declare;                      //军团宣言
    public string Notice;                       //军团公告

    public int GetCorpsMaxMemberByLevel()
    {
        return 10 + Level * 10;
    }
}
