using UnityEngine;
using Newtonsoft.Json.Linq;

public enum Attr
{
    /// <summary>
    /// 统率
    /// </summary>
    LeadPower = 1,
    /// <summary>
    /// 武力
    /// </summary>
    Power = 2,
    /// <summary>
    /// 智力
    /// </summary>
    Strategy = 3,
    /// <summary>
    /// 最大生命值
    /// </summary>
    MaxHp = 4,
    /// <summary>
    /// 物理攻击力
    /// </summary>
    Atk = 5,
    /// <summary>
    /// 策略攻击力
    /// </summary>
    Matk = 6,
    /// <summary>
    /// 物理防御力
    /// </summary>
    Def = 7,
    /// <summary>
    /// 策略防御力
    /// </summary>
    MDef = 8,
    /// <summary>
    /// 物理穿透
    /// </summary>
    DefBreak = 9,
    /// <summary>
    /// 策略穿透
    /// </summary>
    MdefBreak = 10,
    /// <summary>
    /// 物理抗性
    /// </summary>
    DefRate = 11,
    /// <summary>
    /// 策略抗性
    /// </summary>
    MdefRate = 12,
    /// <summary>
    /// 士气恢复
    /// </summary>
    VigourRec = 13,
    /// <summary>
    /// 士气吸取(激励)
    /// </summary>
    VigourSuck = 14,
    /// <summary>
    /// 闪避率
    /// </summary>
    DodgeRate = 15,
    /// <summary>
    /// 格挡率
    /// </summary>
    BlockRate = 16,
    /// <summary>
    /// 韧性率
    /// </summary>
    FirmRate = 17,
    /// <summary>
    /// 命中率
    /// </summary>
    HitRate = 18,
    /// <summary>
    /// 破击率
    /// </summary>
    RoutRate = 19,
    /// <summary>
    /// 暴击率
    /// </summary>
    CritRate = 20,
    /// <summary>
    /// 暴击伤害
    /// </summary>
    CritInc = 21,
    /// <summary>
    /// 生命恢复
    /// </summary>
    HpRec = 22,
    /// <summary>
    /// 治疗承受
    /// </summary>
    HealRate = 23,
    /// <summary>
    /// 吸血
    /// </summary>
    HpSuck = 24,
    /// <summary>
    /// 离间概率
    /// </summary>
    SowRate = 25,
    /// <summary>
    /// 眩晕概率
    /// </summary>
    FaintRate = 26,
    /// <summary>
    /// 伪报概率
    /// </summary>
    SpoofRate = 27,
    /// <summary>
    /// 离间抗性
    /// </summary>
    DeSowRate = 28,
    /// <summary>
    /// 眩晕抗性
    /// </summary>
    DeFaintRate = 29,
    /// <summary>
    /// 伪报抗性
    /// </summary>
    DeSpoofRate = 30,
    /// <summary>
    /// 攻击速度
    /// </summary>
    Asp = 31,
    /// <summary>
    /// 伤害承受
    /// </summary>
    HarmRate = 32,
    /// <summary>
    /// 离间几率
    /// </summary>
    BetrayRate = 33,
    /// <summary>
    /// 抗离间几率
    /// </summary>
    DeBetrayRate = 34,
    /// <summary>
    /// 攻击速度增长
    /// </summary>
    AspRate = 35,
}

public class AttrUtil
{

    //基础暴击伤害
    public static readonly float BASE_CRITINC = 1.5f;
    //基础格挡后伤害
    public static readonly float BASE_BLOCK = 0.5f;
    //最快攻击速度 每秒攻击2.0次，即攻击1次需要0.5秒
    public static readonly float MAX_ASP = 2;
    //最慢攻击速度 每秒攻击0.4次，即攻击1次需要2.5秒
    public static readonly float MIN_ASP = 0.4f;
    //统率计算属性系数
    public static readonly int LEAD_DODGE_VALUE = 2500000;//闪避
	public static readonly int LEAD_HIT_VALUE = 3000000;//命中
	public static readonly int LEAD_BLOCK_VALUE = 2000000;//格挡
	public static readonly int LEAD_ROUT_VALUE = 2400000;//破击
	public static readonly int LEAD_FIRM_VALUE = 2200000;//抵抗
	public static readonly int LEAD_CRIT_VALUE = 1800000;//暴击

    /// <summary>
    /// 一级属性计算最大生命值
    /// </summary>
    /// <param name="leadPower"></param>
    /// <param name="level"></param>
    /// <param name="growRate"></param>
    /// <returns></returns>
    public static int CalMaxHp(int leadPower, int level, float growRate)
    {
        return Mathf.RoundToInt(leadPower * level * growRate);
    }

    /// <summary>
    /// 一级属性计算物理攻击
    /// </summary>
    /// <param name="power"></param>
    /// <param name="level"></param>
    /// <param name="growRate"></param>
    /// <returns></returns>
    public static float CalAtk(int power, int level, float growRate)
    {
        return power * level * growRate;
    }

    /// <summary>
    /// 一级属性计算物理防御
    /// </summary>
    /// <param name="power"></param>
    /// <param name="level"></param>
    /// <param name="growRate"></param>
    /// <returns></returns>
    public static float CalDef(int power, int level, float growRate)
    {
        return power * level * growRate;
    }
    /// <summary>
    /// 一级属性计算策略攻击
    /// </summary>
    /// <param name="strategy"></param>
    /// <param name="level"></param>
    /// <param name="growRate"></param>
    /// <returns></returns>
    public static float CalMatk(int strategy, int level, float growRate)
    {
        return strategy * level * growRate;
    }
    /// <summary>
    /// 一级属性计算策略防御
    /// </summary>
    /// <param name="strategy"></param>
    /// <param name="level"></param>
    /// <param name="growRate"></param>
    /// <returns></returns>
    public static float CalMdef(int strategy, int level, float growRate)
    {
        return strategy * level * growRate;
    }
    /// <summary>
    /// 计算物理抗性
    /// </summary>
    /// <param name="def"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float CalDefRate(float def, int level)
    {
        return Mathf.Clamp(def / (def + 2000), 0, 0.7f);
    }
    /// <summary>
    /// 计算策略抗性
    /// </summary>
    /// <param name="mdef"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float CalMdefRate(float mdef, int level)
    {
        return Mathf.Clamp(mdef / (mdef + 2000), 0, 0.7f);
    }
    /// <summary>
    /// 计算闪避率
    /// </summary>
    /// <param name="dodge"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float CalDodgeRate(float leadPower, int level)
    {
        return Mathf.Pow(leadPower + 2, 2) / LEAD_DODGE_VALUE * (level + 20);
    }
    /// <summary>
    /// 计算命中率
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float CalHitRate(float leadPower, int level)
    {
        return Mathf.Pow(leadPower + 2, 2) / LEAD_HIT_VALUE * (level + 20);
    }
    /// <summary>
    /// 计算格挡率
    /// </summary>
    /// <param name="block"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float CalBlockRate(float leadPower, int level)
    {
        return Mathf.Pow(leadPower + 2, 2) / LEAD_BLOCK_VALUE * (level + 20);
    }

    /// <summary>
    /// 计算破击率
    /// </summary>
    /// <param name="rout"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float CalRoutRate(float leadPower, int level)
    {
        return Mathf.Pow(leadPower + 2, 2) / LEAD_ROUT_VALUE * (level + 20);
    }

    /// <summary>
    /// 计算韧性率
    /// </summary>
    /// <param name="firm"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float CalFirmRate(float leadPower, int level)
    {
        return Mathf.Pow(leadPower + 2, 2) / LEAD_FIRM_VALUE * (level + 20);
    }
    /// <summary>
    /// 计算暴击率
    /// </summary>
    /// <param name="crit"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float CalCritRate(float leadPower, int level)
    {
        return Mathf.Pow(leadPower + 2, 2) / LEAD_CRIT_VALUE * (level + 20);
    }

    /// <summary>
    /// 计算攻击速度
    /// </summary>
    /// <param name="asp"></param>
    /// <param name="asprate"></param>
    /// <returns></returns>
    public static float CalAsp(float asp, float asprate)
    {
        return Mathf.Clamp(asp * (1 + asprate), MIN_ASP, MAX_ASP);
    }

    /// <summary>
    /// 技能显示
    /// </summary>
    /// <param name="a"></param>
    /// <param name="level"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float SkillPanelShow(float a,float level,float b)
    {
        return a + level * b;  
    }
    /// <summary>
    /// 计算伤害(治疗)
    /// </summary>
    /// <param name="isNormal">是否普攻</param>
    /// <param name="isAtk">是否物理</param>
    /// <param name="baseHarm">基础数值，正数代表治疗负数代表伤害</param>
    /// <param name="user">发起者</param>
    /// <param name="target">承受者</param>
    /// <returns>返回值x表示EffectState，y表示伤害值</returns>
    public static Vector2Int CalHarm(bool isNormal, bool isAtk, float baseHarm, FightUnit user, FightUnit target)
    {
        float harmOffsetMin = JsonMgr.GetSingleton().GetGlobalIntArrayByID(1013).value;
        float harmOffsetMax = JsonMgr.GetSingleton().GetGlobalIntArrayByID(1014).value;
        float harmOffset = Random.Range(harmOffsetMin, harmOffsetMax);
        if (baseHarm > 0)
        {
            //治疗
            bool crit = false;
            if (Random.Range(0, 1.0f) < (user.CritRate / 2))
                crit = true;
            return new Vector2Int((int)(crit ? EffectState.HealCrit : EffectState.Heal), (int)(baseHarm * (target.HealRate + user.MdefBreak) * (crit ? (user.CritInc) : 1) * harmOffset));
        }
        else if (baseHarm < 0)
        {
            EffectState estate = EffectState.None;
            if (isNormal)
            {
                if (Random.Range(0, 1.0f) < Mathf.Max(0, (target.DodgeRate - user.HitRate)))
                {
                    estate = EffectState.Dodge;
                }
                else if (Random.Range(0, 1.0f) < Mathf.Max(0, (target.BlockRate - user.RoutRate)))
                {
                    estate = EffectState.Block;
                }
            }

            if (estate == EffectState.None)
            {
                if (Random.Range(0, 1.0f) < Mathf.Max(0, (user.CritRate - target.FirmRate)))
                {
                    estate = EffectState.Crit;
                }
            }

            float addition = estate == EffectState.None ? 1 : (estate == EffectState.Dodge ? 0 : (estate == EffectState.Block ? BASE_BLOCK : user.CritInc));
            float finalHarm = baseHarm * (1 - (Mathf.Min((isAtk ? target.DefRate : target.MdefRate) - (isAtk ? user.DefBreak : user.MdefBreak), 1))) * target.HarmRate * addition * harmOffset;
            return new Vector2Int((int)estate, (int)finalHarm);

        }
        else
        {
            EDebug.LogErrorFormat("AttrUtil.CalHarm, baseHarm is zero");
            return Vector2Int.zero;
        }
    }

    /// <summary>
    /// 计算公式，返回Vector3，
    /// </summary>
    /// <param name="type"></param>
    /// <param name="expression"></param>
    /// <param name="user"></param>
    /// <param name="target"></param>
    /// <param name="extra">额外参数，现在只是表示等级</param>
    /// <returns></returns>
    public static Vector3 CalExpression(int type, JArray expression, FightUnit user = null, FightUnit target = null, float extra = 0)
    {
        switch (type)
        {
            case 1:
                {
                    // attr * (a + extra * b)的形式
                    if (user == null || target == null)
                    {
                        EDebug.LogErrorFormat("CalExpression failed. type {0} need user and target", type);
                        return Vector2.zero;
                    }
                    if (expression.Count != 3)
                    {
                        EDebug.LogErrorFormat("CalExpression failed. type {0} expression {1} is invalid", type, expression);
                        return Vector2.zero;
                    }
                    Vector3 ret = expression2Attr(expression[0].ToObject<string>(), user, target);
                    float a = expression[1].ToObject<float>();
                    float b = expression[2].ToObject<float>();
                    ret.y = ret.y * (a + extra * b);
                    return ret;
                }
                break;
            case 2:
                {
                    //直接返回原值的形式
                    float v = expression[0].ToObject<float>();
                    return new Vector3(1, v, 0);
                }
                break;
            case 3:
                {
                    //a + 等级 * b形式
                    if (expression.Count != 2)
                    {
                        EDebug.LogErrorFormat("CalExpression failed. type {0} expression {1} is invalid", type, expression);
                        return Vector2.zero;
                    }
                    float a = expression[0].ToObject<float>();
                    float b = expression[1].ToObject<float>();
                    return new Vector3(0, a + extra * b, 0);
                }
                break;
            case 4:
                {
                    //返回两个参数，第一个参数为a + 等级 * b形式，第二个参数为返回原值形式
                    if (expression.Count != 3)
                    {
                        EDebug.LogErrorFormat("CalExpression failed. type {0} expression {1} is invalid", type, expression);
                        return Vector2.zero;
                    }
                    float a = expression[0].ToObject<float>();
                    float b = expression[1].ToObject<float>();
                    float c = expression[2].ToObject<float>();
                    return new Vector3(a + extra * b, c, 0);
                }
                break;
            case 5:
                {
                    //返回三个参数，(a, b, c)
                    if (expression.Count != 3)
                    {
                        EDebug.LogErrorFormat("CalExpression failed. type {0} expression {1} is invalid", type, expression);
                        return Vector2.zero;
                    }
                    float a = expression[0].ToObject<float>();
                    float b = expression[1].ToObject<float>();
                    float c = expression[2].ToObject<float>();
                    return new Vector3(a, b, c);
                }
                break;
            default:
                return Vector3.zero;
        }
    }

    /// <summary>
    /// 字符串转换为具体属性值，返回结果x表示属性类型(0无类型1武力2策略)，y表示属性值
    /// </summary>
    /// <param name="str"></param>
    /// <param name="user"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private static Vector2 expression2Attr(string str, FightUnit user, FightUnit target)
    {
        switch (str)
        {
            case "u_atk":
                return new Vector2(1, user.Atk);
            case "t_atk":
                return new Vector2(1, target.Atk);
            case "u_matk":
                return new Vector2(2, user.Matk);
            case "t_matk":
                return new Vector2(2, target.Matk);
            case "u_hp":
                return new Vector2(0, user.MaxHP);
            case "t_hp":
                return new Vector2(0, target.MaxHP);
            case "u_chp":
                return new Vector2(0, user.CurHP);
            case "t_chp":
                return new Vector2(0, target.CurHP);
            default:
                {
                    EDebug.LogErrorFormat("expression2Attr failed, {0} is invalid", str);
                    return Vector2.zero;
                }
        }
    }
    public static string GetAttribute(Attr attr)
    {
        string ret = "";
        switch (attr)
        {
            case Attr.Asp:
                ret = "攻击速度";
                break;
            case Attr.Atk:
                ret = "物理攻击";
                break;
            case Attr.BlockRate:
                ret = "格挡";
                break;
            case Attr.CritInc:
                ret = "致命";
                break;
            case Attr.CritRate:
                ret = "暴击";
                break;
            case Attr.Def:
                ret = "物理防御";
                break;
            case Attr.DeFaintRate:
                ret = "扰乱洞察";
                break;
            case Attr.DefBreak:
                ret = "物理穿透";
                break;
            case Attr.DefRate:
                ret = "物理抗性";
                break;
            case Attr.DeSowRate:
                ret = "离间洞察";
                break;
            case Attr.DeSpoofRate:
                ret = "伪报洞察";
                break;
            case Attr.DodgeRate:
                ret = "闪避";
                break;
            case Attr.FaintRate:
                ret = "眩晕成功";
                break;
            case Attr.FirmRate:
                ret = "抵抗";
                break;
            case Attr.HealRate:
                ret = "治疗承受";
                break;
            case Attr.HitRate:
                ret = "命中";
                break;
            case Attr.HpRec:
                ret = "生命恢复";
                break;
            case Attr.HpSuck:
                ret = "生命汲取";
                break;
            case Attr.LeadPower:
                ret = "统率";
                break;
            case Attr.Matk:
                ret = "策略攻击";
                break;
            case Attr.MaxHp:
                ret = "最大生命";
                break;
            case Attr.MDef:
                ret = "策略防御";
                break;
            case Attr.MdefBreak:
                ret = "策略穿透";
                break;
            case Attr.MdefRate:
                ret = "策略抗性";
                break;
            case Attr.Power:
                ret = "勇武";
                break;
            case Attr.RoutRate:
                ret = "破击";
                break;
            case Attr.SowRate:
                ret = "离间成功";
                break;
            case Attr.SpoofRate:
                ret = "伪报成功";
                break;
            case Attr.Strategy:
                ret = "智力";
                break;
            case Attr.VigourRec:
                ret = "士气恢复";
                break;
            case Attr.VigourSuck:
                ret = "士气激励";
                break;
            case Attr.HarmRate:
                ret = "伤害承受";
                break;
            case Attr.AspRate:
                ret = "攻击速度";
                break;
            default:
                ret = attr.ToString();
                break;
        }
        return ret;
    }
    private static bool IsFloat(Attr attr)
    {
        switch (attr)
        {
            case Attr.BlockRate:
            case Attr.CritInc:
            case Attr.CritRate:
            case Attr.DeFaintRate:
            case Attr.DefBreak:
            case Attr.DefRate:
            case Attr.DeSowRate:
            case Attr.DeSpoofRate:
            case Attr.DodgeRate:
            case Attr.FaintRate:
            case Attr.FirmRate:
            case Attr.HealRate:
            case Attr.HitRate:
            case Attr.HpRec:
            case Attr.HpSuck:
            case Attr.MdefBreak:
            case Attr.MdefRate:
            case Attr.RoutRate:
            case Attr.SowRate:
            case Attr.SpoofRate:
            case Attr.HarmRate:
            case Attr.AspRate:
                return true;
        }
        return false;
    }

    public static string ShowText(Attr attr, float num,float per = 0)
    {
        string txt = "";
        if(per != 0)
        {
            txt = string.Format("{0}%", per);
        }
        else
        {
            bool isFloat = IsFloat(attr);
            if (isFloat)
                txt = string.Format("{0:0.#}%", num * 100);
            else
            {
                txt = num.ToString("0.0");
                if (attr != Attr.Asp)
                    txt = Mathf.RoundToInt(num).ToString();
            }
        }
        return txt;
    }
}
