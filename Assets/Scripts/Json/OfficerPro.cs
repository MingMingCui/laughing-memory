
[System.Serializable]
public class OfficerProArray
{
    public OfficerPro[] Array;
    private OfficerProArray() { }
}

[System.Serializable]
public class OfficerPro
{
    public int ID;
    public int maxhp;                  //最大生命值
    public int atk;                 //物理攻击力
    public int matk;                   //策略攻击力
    public float defbreak;              //物理穿透
    public float mdefbreak;             //策略穿透
    public float defrate;            //物理抗性
    public float mdefrate;              //策略抗性
    public int vigourrec;        //士气恢复
    public float dodgerate;            //闪避率
    public float blockrate;             //格挡率
    public float firmrate;             //韧性率
    public float hitrate;              //命中率
    public float routrate;             //破击率
    public float critrate;             //暴击率
    public float critinc;              //暴击伤害
    public float hprec;                 //生命恢复
    public float healrate;             //治疗承受
    public float hpsuck;               //吸血
    public float asprate;               //攻击速度增长
    private OfficerPro() { }
}


