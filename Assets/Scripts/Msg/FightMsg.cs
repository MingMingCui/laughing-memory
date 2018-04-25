using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Msg.FightMsg
{
    [System.Serializable]
    public class FightMsg
    {
        public bool IsPve;
        public int Treasures;
        public FightUnitData[] Fighters;
        public FightRoundData[] Enemies;
    }

    [System.Serializable]
    public class FightRoundData
    {
        public FightUnitData[] RoundData;
    }

    [System.Serializable]
    public class FightUnitData
    {
        public int UnitId;
        public int Level;
        public int Star;
        public int Rare;
        public int StubPos;
        public bool IsEnemy;
        public FightSkillData[] SkillList;
    }

    [System.Serializable]
    public class FightSkillData
    {
        public int SkillId;
        public int SkillLevel;
    }
}
