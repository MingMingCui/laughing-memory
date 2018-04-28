using System;

namespace JsonData
{
    [Serializable]
    public class HeroArray
    {
        public Hero[] Array;
        private HeroArray() { }
    }

    [Serializable]
    public class Hero
    {
        public int ID; // ID
        public string name; // 名字
        public int sex; // 性别
        public int stage;//是否显示
        public int cardid;//图鉴
        public int camp; // 阵营
        public int type; // 类型
        public int armortpye;//防具类型
        public int weapontype;//武器类型
        public int resid; // 资源id
        public int horseid; // 马资源id
        public int headid; // 头像id
        public int initstar; // 初始星级
        public int order; // 先手值
        public int leadpower; // 统帅值
        public int power; // 武力值
        public int strategy; // 策略值
        public float dodge; // 闪避率
        public float hit; // 命中率
        public float block; // 格挡率
        public float rout; // 破击率
        public float firm; // 韧性率
        public float crit; // 暴击率
        public float critinc; // 暴击加成
        public float hprec; // 生命回复
        public int vigourrec; // 士气回复
        public float hpsuck; // 生命偷取
        public float vigoursuck; // 能量偷取
        public float asp; // 攻击速度
        public float arange; // 攻击半径
        public float harmrate; // 伤害承受
        public float healrate; // 治疗承受
        public float defbreak; // 物理穿透
        public float mdefbreak; // 策略穿透
        public int[] skills; // 技能组
        public int[] skillturn1; // 初始技能顺序
        public int[] skillturn2; // 技能顺序
        public string desc;//描述
        public int[] equip;
        private Hero() { }
    }
}

