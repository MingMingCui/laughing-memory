using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Msg.ArenaMsg
{
    [System.Serializable]
    public class ArenaResponse
    {
        public int rank;                //我的排名
        public int times;               //今日竞技次数
        public ArenaEnemy[] enemies;    //敌人
    }

    [System.Serializable]
    public class ArenaEnemy
    {
        public uint roleid;     //角色id
        public int headid;      //头像id
        public int level;       //等级
        public string rolename; //角色名
        public int fightpower;  //战斗力
    }
}
