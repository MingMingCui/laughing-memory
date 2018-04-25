
[System.Serializable]
public class MonsterArray 
{
    public Monster[] Array;
    private MonsterArray() { }
}

[System.Serializable]
public class Monster 
{
    public int ID; // 怪物id
    public string name; // 怪物名字
    public int resid; // 资源id
    public int horseid; // 马id
    public int headid; // headid
    public int star; // 星级
    public int rare; // 品级
    public string desc; // 描述id
    public int level; // 等级
    public int leadpower; // 统率
    public int power; // 勇武
    public int strategy; // 谋略
    public int maxhp; // 最大生命值
    public float atk; // 物理攻击
    public float matk; // 策略攻击
    public float defrate; // 物理抗性
    public float mdefrate; // 策略抗性
    public float defbreak; // 物理穿透
    public float mdefbreak; // 策略穿透
    public float asp; // 攻击速度
    public float arange; // 攻击半径
    public float harmrate; // 受伤承受
    public float healrate; // 治疗承受
    public float dodgerate; // 闪避率
    public float hitrate; // 命中率
    public float blockrate; // 格挡率
    public float routrate; // 破击率
    public float firmrate; // 韧性率
    public float critrate; // 暴击率
    public float critinc; // 暴击伤害
    public float desowrate; // 混乱抗性
    public float defaintrate; // 眩晕抗性
    public float despoofrate; // 伪报抗性
    public float debetrayrate;//离间抗性
    public int[] skills; // 技能组
    public int[] skillturn1; // 初始技能顺序
    public int[] skillturn2; // 技能顺序
    private Monster() { }
}
