namespace JsonData
{
    [System.Serializable]
    public class CopyLevelArray
    {
        private CopyLevelArray() { }
        public CopyLevel[] Array;
    }
    [System.Serializable]
    public class CopyLevel
    {
        private CopyLevel() { }
        public int ID; // ID
        public string name; // 副本名称
        public int type; // 类型
        public int level; // 等级
        public int power; // 体力
        public int times; // 次数
        public int[] monster; // 怪物
        public int[] drop; // 掉落
        public string desc; // 描述
        public string monster_tip; // 怪物提示
        public int[] drop_tip; // 掉落提示
        public int combat_effect; // 战斗力提示
        public int script; // 剧情脚本
    }
}

