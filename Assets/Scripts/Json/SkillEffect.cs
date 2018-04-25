namespace JsonData
{
    [System.Serializable]
    public class SkillEffectArray
    {
        private SkillEffectArray() { }
        public SkillEffect[] Array;
    }

    [System.Serializable]
    public class SkillEffect 
    {
        private SkillEffect() { }
        public int ID; // ID
        public string name; // 名字
        public int effect; // 技能产生的效果类型
        public int paramtype; // 公式类型
        public string effectparam; // 技能效果参数
        public int target; // 技能效果目标
        public int isnormal; // 是否是普攻效果
        public int hiteffect; // 受击特效
        public int hitvigour; // 被击增长士气
    }
}

