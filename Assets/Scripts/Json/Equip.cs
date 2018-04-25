namespace JsonData
{
    [System.Serializable]
    public class EquipArray
    {
        public Equip[] Array;
        private EquipArray() { }
    }
    [System.Serializable]
    public class Equip
    {
        public int ID; // ID
        public string Name; // 名称
        public EquipPart EquiPrat; // 部位
        public int Type; // 类型
        public int Exp; // 吞噬经验
        public EquipType Intensify;
        public Pro[] Attribute; // 基础属性
        public Pro[] Innate;//固定属性
        public int[] Ran; // 随机属性
        public int AdvancedCondition;//进阶等级条件
        public int[] Advanced;   //进阶列表
        private Equip() { }
    }
}


