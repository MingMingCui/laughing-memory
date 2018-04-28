using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Msg.CorpsMsg
{
    [System.Serializable]
    public class CreateCorpsRequest
    {
        public string name;         //军团名字
        public int flag;            //旗帜id
        public int limit;           //等级限制
        public int allowtype;       //允许类型，非0表示需要军团管理员同意
    }

    [System.Serializable]
    public class CorpsInfo
    {
        public uint UID = 0;            //军团id
        public int flag = 0;            //军团图标id
        public int camp = 0;            //军团阵营
        public string name = "";        //军团名字
        public int level = 0;           //军团等级
        public string leader = "";      //军团长名字
        public int members = 0;         //军团成员数
        public int limit = 0;           //军团等级限制，0表示无限制
        public int state = 0;           //军团状态，是否正在申请，0未申请，1申请
    }

    [System.Serializable]
    public class CorpsMember
    {
        public uint RoleId;             //成员的RoleId
        public int HeadId;              //头像id
        public string Name;             //成员名字
        public int Level;               //成员等级
        public int Job;                 //成员职位
        public int Vigour;              //7天活跃
        public int Donate;              //成员贡献
        public long LastOL;             //最后上线时间，如果当前在线，则是0
    }

    [System.Serializable]
    public class CorpsRequest
    {
        public uint RoleId;         //角色id
        public int HeadId;          //头像id
        public string Name;         //名字
        public int Level;           //等级
        public int FightPower;      //战斗力
    }

    [System.Serializable]
    public class CorpsLogNode
    {
        public long date;
        public string[] log;
    }

    [System.Serializable]
    public class CorpsResponse
    {
        public CorpsInfo[] corps;                   //军团列表
        public CorpsMember[] members;               //成员列表
        public CorpsResponse[] request;             //申请列表
        public CorpsLogNode[] logs;                 //日志

        public uint id;                             //军团id
        public int flag;                            //旗帜id
        public int level;                           //军团等级
        public int exp;                             //军团经验
        public int name;                            //军团名称
        public int rank;                            //军团排名
        public int power;                           //军团战斗力
        public int todaydonate;                     //今日贡献
        public long totaldonate;                    //总贡献
        public string declare;                      //军团宣言
        public string notice;                       //军团公告
    }
}
