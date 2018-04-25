
[System.Serializable]
public class SkillArray
{
    public Skill[] Array;
    private SkillArray() { }
}

[System.Serializable]
public class Skill
{
    public int ID; // ID
    public string name; // 技能名字
    public int icon; // 技能图标
    public string desc; // 技能描述
    public int type; // 技能类型
    public int target; // 技能目标
    public int selftarget; // 自己作为目标
    public int choosetype; // 选择类型
    public int targetnum; // 技能目标数量
    public int rangetype; // 技能范围类型
    public float[] range; // 技能范围
    public int rangeenemy;//范围敌我
    public float cradle; // 技能1前摇时间
    public float post; // 技能后摇时间
    public int effecttimes; // 技能效果次数
    public float effectcd; // 效果间隔
    public int[] effects; // 技能效果
    public int starteffect; // 发动特效
    public int fulleffect; // 全屏特效
    public int attackeffect; // 攻击特效
    public int casteffect; //投掷特效
    public float casttime; //投掷时长
    public int attackvigour; // 攻击增长士气
    private Skill() { }
}
