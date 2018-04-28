using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using JsonData;

public enum FightUnitState
{
	Idle = 0,       //空闲状态
	Move = 1,       //正在移动状态
	Skill = 2,      //正在使用技能状态
}

public enum HeroCamp
{
    Wei = 0,        //魏
    Shu = 1,        //蜀
    Wu = 2,         //吴
    Qun = 3,        //群
}

public enum HeroType
{
    None = 0,           //无
    Tank = 1,           //坦克
    Warrior = 2,        //战士
    Shooter = 3,        //射手
    Adviser = 4,        //谋士
    Assist = 5,         //辅助
}

public enum SkillTarget
{
    SELF = 1,           //自己
    CUR = 2,            //当前目标
    PARTNER = 3,        //指定友军
    ENEMY = 4,          //指定敌军
    RAND_PARTNER = 5,   //随机友军
    RAND_ENEMY = 6,     //随机敌军
    ALL_UNIT = 7,       //所有单位
    ENEMY_UNIT = 8,     //最后伤害自己的人
}

public enum SkillType
{
    NORMAL = 1,     //普攻
    PASSIVE = 2,    //被动
    AUTO = 3,       //自动
    ACTIVE = 4,     //主动
    PROMPT = 5,     //瞬发
    SPECIAL = 6,    //特殊
}

public enum EffectType
{
    HARM_HP = 1,            //造成伤害
    HARM_VIGOUR = 2,        //伤害士气
    HEAL_HP = 3,            //造成治疗
    HEAL_VIGOUR = 4,        //治疗士气
    BULLET = 5,             //发射子弹
    BUFF = 6,               //添加buff
    HEAL_OVER_VIGOUR = 7,   //治疗士气，可突破上限
    Summon = 8,             //召唤
    Dispel = 9,             //驱散
    Purge = 10,             //净化
    Booster = 11,           //增加复活次数
}

public enum ChooseType
{
    LEAD_MAX = 1,       //统率最高者
    LEAD_MIN = 2,       //统率最低者
    POWER_MAX = 3,      //武力最高者
    POWER_MIN = 4,      //武力最低者
    STRATEGY_MAX = 5,   //策略最高者
    STRATEGY_MIN = 6,   //策略最低者
    HP_MAX = 7,         //血量最高者(%)
    HP_Min = 8,         //血量最低者(%)
    DISTANCE_MAX = 9,   //距离最远者
}

public enum SkillState
{
    Cradle = 0,     //前摇
    Effect = 1,     //作用
    Post = 2,       //后摇
}

public enum RangeType
{
    Line = 1,           //直线
    Circle = 2,         //圆形
    Fan = 3,            //扇形
    Rectangle = 4,      //矩形
}

public enum RangeEnemy
{
    Both = 0,       //不考虑
    Enemy = 1,      //只选敌方
    Partner = 2,    //只选友方
}

public enum EffectState
{
    None = 0,       //无
    Crit = 1,       //暴击
    Block = 2,      //格挡
    Dodge = 3,      //闪避
    Miss = 4,       //未命中
    Scared = 5,     //胆裂
    Sow = 6,        //离间
    Faint = 7,      //击晕
    Sneer = 8,      //嘲讽
    Spoof = 9,      //伪报
    SpeedUp = 10,   //加速
    SpeedDown = 11, //减速
    Heal = 12,      //回复血量
    HealCrit = 13,  //回复血量暴击
    HealVigour = 14,//回复士气
    AtkUp = 15,     //增加攻击
    AtkDown = 16,   //降低攻击
    AspUp = 17,     //增加攻速
    AspDown = 18,   //降低攻速
    DefUp = 19,     //增加护甲
    DefDown = 20,   //降低护甲
    DefImmu = 21,   //物理免疫
    MdefImmu = 22,  //策略免疫
    AddGold = 23,   //增加铜钱
}

public enum ControlState
{
    None = 0,       //无
    Faint = 1,      //眩晕
    Betray = 2,     //离间
    Sow = 3,        //混乱
    Spoof = 4,      //伪报
    Sneer = 5,      //嘲讽
}

public enum SpecialState
{
    None = 0,       //无
    Scared = 1,     //胆裂
    Slient = 2,     //沉默
    //XXX = 4,
}

/// <summary>
/// 战斗的基本单位，包含所有数据、状态
/// </summary>
public class FightUnit{
    #region 战斗相关属性
    //UID，CreateFight的时候指定，用于区分战斗单位的唯一ID
    public uint UID = 0;
    //战斗单位的模板id
    public int HeroId = 0;
    //是否是怪物
    public bool IsMonster = false;
    //是否是召唤物
    public bool IsSummon = false;
    //战斗单位名字
    public string HeroName = "";
    //性别
    public bool Sex = false;
    //阵营
    public int Camp = 0;
    //英雄类型(职业)
    public int Type = 0;
    //等级
    public int Level = 0;
    //星级
    public int Star = 0;
    //品级
    public int Rare = 1;
    //攻击范围
    public float Arange = 0;
    //技能范围
    public float Sklrange = 0;
    //Hp成长系数
    public float HpGrowRate = 0;
    //物理攻击成长系数
    public float AtkGrowRate = 0;
    //物理防御成长系数
    public float DefGrowRate = 0;
    //策略攻击成长系数
    public float MatkGrowRate = 0;
    //策略防御成长系数
    public float MdefGrowRate = 0;
    //先手值
    public int Order = 0;
    //是敌是友
    public bool IsEnemy = false;
    //是否死亡
    private bool _isDead = false;
    public bool IsDead
    {
        get { return _isDead; }
        set
        {
            if (_isDead != value)
            {
                if (value && Booster > 0)
                {
                    --Booster;
                    ZEventSystem.Dispatch(EventConst.OnFightUnitBooster, this);
                    return;
                }
                _isDead = value;
                if (_isDead)
                {
                    this.BuffMgrObj.ClearBuff(ClearBuffType.Both);
                    if (this.State == FightUnitState.Move)
                        this.PathFinderObj.StopFind();
                    if (IsUsingActiveSkill)
                    {
                        //有可能请求没有取消，取消了再发一次也没事
                        ZEventSystem.Dispatch(EventConst.OnRequestUnitPause, this, false);
                    }
                    ZEventSystem.Dispatch(EventConst.OnFightUnitDie, this);
                    this.Vigour = 0;
                    if (this.EnemyUnit != null)
                        this.EnemyUnit.OnKillUnit(this);
                }
            }
        }
    }
    //统率力
    private int _leadPower = 0;
    public int LeadPower
    {
        get { return _leadPower; }
        set
        {
            if (_leadPower != value)
            {
                _leadPower = value;
                ////统率力变化，重新计算最大生命值
                //if (!IsMonster)
                //{
                //    BaseMaxHp = AttrUtil.CalMaxHp(_leadPower, Level, HpGrowRate);
                //    MaxHP = BaseMaxHp;
                //}
            }
        }
    }
    //勇武力
    private int _power = 0;
    public int Power
    {
        get { return _power; }
        set
        {
            if (_power != value)
            {
                _power = value;
            }
        }
    }
    //谋略
    private int _strategy = 0;
    public int Strategy
    {
        get { return _strategy; }
        set
        {
            if (_strategy != value)
            {
                _strategy = value;
            }
        }
    }
    //当前血量
    private int _curHp = 0;
    public int CurHP
    {
        get { return _curHp; }
        set
        {
            if (_curHp != value)
            {
                _curHp = value;
                _curHp = UnityEngine.Mathf.Clamp(_curHp, 0, MaxHP);
                if (_curHp == 0)
                {
                    //角色死亡、重伤
                    IsDead = true;
                }
                ZEventSystem.Dispatch(EventConst.OnFightUnitHpChange, this);
            }
        }
    }
    //最大血量
    public int BaseMaxHp = 0;
    private int _maxHp = 0;
    public int MaxHP
    {
        get { return _maxHp; }
        set
        {
            if (_maxHp != value)
            {
                int diff = value - _maxHp;
                _maxHp = value;
                ZEventSystem.Dispatch(EventConst.OnFightUnitHpChange, this);
                if (diff > 0)
                {
                    CurHP += diff;
                }
                else
                {
                    CurHP = Mathf.Clamp(CurHP, 0, _maxHp);
                }
            }
        }
    }
    //士气
    private int _vigour = 0;
    public int Vigour
    {
        get { return _vigour; }
        set
        {
            if (_vigour != value)
            {
                _vigour = value;
                _vigour = Mathf.Clamp(_vigour, 0, RealMaxVigour);
                ZEventSystem.Dispatch(EventConst.OnFightUnitVigourChange, this);
            }
        }
    }

    /// <summary>
    /// 增加士气，逻辑上调用直接走这个接口
    /// </summary>
    /// <param name="vigour">增加多少</param>
    /// <param name="over">是否突破上限</param>
    public void AddVigour(int vigour, bool over = false)
    {
        if (this.IsDead)
            return;

        if (vigour > 0 && this.GetSpecialState(SpecialState.Scared))
            return;

        if (vigour > 0 && !over)
        {
            Vigour = Mathf.Min(Vigour + vigour, MaxVigour);
            return;
        }
        Vigour += vigour;
    }
    //最大士气
    public readonly int MaxVigour = 1000;
    //真实最大士气
    public readonly int RealMaxVigour = 2000;
    //物理攻击力，存储不考虑其他属性影响，get时候计算属性影响
    private float _atk = 0;
    public float Atk
    {
        get { return /*(!IsMonster ? AttrUtil.CalAtk(Power, Level, AtkGrowRate) : 0) +*/ _atk + BuffMgrObj.Atk; }
        set
        {
            if (_atk != value)
            {
                _atk = value;
            }
        }
    }
    //物理防御力， 存储不考虑其他属性影响，get时候计算属性影响
    private float _def = 0;
    public float Def
    {
        get { return /*(!IsMonster ? AttrUtil.CalDef(Power, Level, DefGrowRate) : 0) +*/ _def; }
        set
        {
            if (_def != value)
            {
                _def = value;
            }
        }
    }
    //物理抗性，存储不考虑其他属性影响，get时候计算属性影响
    private float _defRate = 0;
    public float DefRate
    {
        get { return /*(!IsMonster ? AttrUtil.CalDefRate(Def, Level) : 0) + */_defRate + BuffMgrObj.DefRate; }
        set
        {
            if (_defRate != value)
            {
                _defRate = value;
            }
        }
    }
    //策略攻击力，存储不考虑其他属性影响，get时候计算属性影响
    private float _matk = 0;
    public float Matk
    {
        get { return /*(!IsMonster ? AttrUtil.CalMatk(Strategy, Level, MatkGrowRate) : 0) +*/ _matk + BuffMgrObj.Matk; }
        set
        {
            if (_matk != value)
            {
                _matk = value;
            }
        }
    }
    //策略防御力，存储不考虑其他属性影响，get时候计算属性影响
    private float _mdef = 0;
    public float Mdef
    {
        get { return /*(IsMonster ? AttrUtil.CalMdef(Strategy, Level, MdefGrowRate) : 0) +*/ _mdef; }
        set
        {
            if (_mdef != value)
            {
                _mdef = value;
            }
        }
    }
    //策略抗性，存储不考虑其他属性影响，get时候计算属性影响
    private float _mDefRate = 0;
    public float MdefRate
    {
        get { return /*(IsMonster ? AttrUtil.CalMdefRate(Mdef, Level) : 0) + */_mDefRate + BuffMgrObj.MdefRate; }
        set
        {
            if (_mDefRate != value)
            {
                _mDefRate = value;
            }
        }
    }
    //闪避
    private int _dodge;
    public int Dodge
    {
        get { return _dodge; }
        set
        {
            if (_dodge != value)
            {
                _dodge = value;
            }
        }
    }
    //闪避率，存储不考虑其他属性影响，get时候计算属性影响
    private float _dodgeRate = 0;
    public float DodgeRate
    {
        get { return /*AttrUtil.CalDodgeRate(LeadPower, Level) +*/ _dodgeRate + BuffMgrObj.DodgeRate; }
        set
        {
            if (_dodgeRate != value)
            {
                _dodgeRate = value;
            }
        }
    }
    //命中
    private int _hit;
    public int Hit
    {
        get { return _hit; }
        set
        {
            if (_hit != value)
            {
                _hit = value;
            }
        }
    }
    //命中率，存储不考虑其他属性影响，get时候计算属性影响
    private float _hitRate = 0;
    public float HitRate
    {
        get { return /*AttrUtil.CalHitRate(LeadPower, Level) + */_hitRate + BuffMgrObj.HitRate; }
        set
        {
            if (_hitRate != value)
            {
                _hitRate = value;
            }
        }
    }
    //格挡
    private int _block = 0;
    public int Block
    {
        get { return _block; }
        set
        {
            if (_block != value)
            {
                _block = value;
            }
        }
    }
    //格挡率，存储不考虑其他属性影响，get时候计算属性影响
    private float _blockRate = 0;
    public float BlockRate
    {
        get { return /*AttrUtil.CalBlockRate(LeadPower, Level) +*/ _blockRate + BuffMgrObj.BlockRate; }
        set
        {
            if (_blockRate != value)
            {
                _blockRate = value;
            }
        }
    }
    //破击
    private int _rout = 0;
    public int Rout
    {
        get { return _block; }
        set
        {
            if (_block != value)
            {
                _block = value;
            }
        }
    }
    //破击率，存储不考虑其他属性影响，get时候计算属性影响
    private float _routRate = 0;
    public float RoutRate
    {
        get { return /*AttrUtil.CalRoutRate(LeadPower, Level) +*/ _routRate + BuffMgrObj.RoutRate; }
        set
        {
            if (_routRate != value)
            {
                _routRate = value;
            }
        }
    }
    //韧性
    private int _firm = 0;
    public int Firm
    {
        get { return _firm; }
        set
        {
            if (_firm != value)
            {
                _firm = value;
            }
        }
    }
    //韧性率，存储不考虑其他属性影响，get时候计算属性影响
    private float _firmRate = 0;
    public float FirmRate
    {
        get { return /*AttrUtil.CalFirmRate(LeadPower, Level) +*/ _firmRate + BuffMgrObj.FirmRate; }
        set
        {
            if (_firmRate != value)
            {
                _firmRate = value;
            }
        }
    }
    //暴击
    private int _crit = 0;
    public int Crit
    {
        get { return _crit; }
        set
        {
            if (_crit != value)
            {
                _crit = value;
            }
        }
    }
    //暴击率
    private float _critRate = 0;
    public float CritRate
    {
        get { return /*AttrUtil.CalCritRate(LeadPower, Level) + */_critRate + BuffMgrObj.CritRate; }
        set
        {
            if (_critRate != value)
            {
                _critRate = value;
            }
        }
    }
    //暴击伤害
    private float _critInc = 0;
    public float CritInc
    {
        get { return _critInc + BuffMgrObj.CritInc; }
        set
        {
            if (_critInc != value)
            {
                _critInc = value;
            }
        }
    }
    //生命回复
    public float HPRec = 0;
    //士气回复
    public int VigourRec = 0;
    //生命偷取，只记录装备、被动等
    private float _hpSuck = 0;
    public float HPSuck
    {
        get { return _hpSuck + BuffMgrObj.HpSuck; }
        set
        {
            if (_hpSuck != value)
            {
                _hpSuck = value;
            }
        }
    }
    //能量偷取，只记录装备、被动等
    private float _vigourSuck = 0;
    public float VigourSuck
    {
        get { return _vigourSuck; }
        set
        {
            if (_vigourSuck != value)
            {
                _vigourSuck = value;
            }
        }
    }
    //攻击速度，记录的是基础攻击速度
    private float _asp = 0;
    public float Asp
    {
        get { return AttrUtil.CalAsp(_asp, AspRate); }
        set
        {
            if (_asp != value)
            {
                _asp = value;
            }
        }
    }
    //攻击速度增加率，记录基础增加率，如装备等，get时计算上buff
    private float _aspRate = 0;
    public float AspRate
    {
        get { return _aspRate + BuffMgrObj.AspRate; }
        set
        {
            if (_aspRate != value)
            {
                _aspRate = value;
            }
        }
    }
    //伤害承受，记录基础伤害承受，get时计算上buff影响
    private float _harmRate = 0;
    public float HarmRate
    {
        get { return Mathf.Max(0, _harmRate + BuffMgrObj.HarmRate); }
        set
        {
            if (_harmRate != value)
            {
                _harmRate = value;
                _harmRate = Mathf.Max(0, _harmRate);
            }
        }
    }
    //治疗承受，记录基础治疗承受，get时计算上buff影响
    private float _healRate = 0;
    public float HealRate
    {
        get { return Mathf.Max(0, _healRate + BuffMgrObj.HealRate); }
        set
        {
            if (_healRate != value)
            {
                _healRate = value;
                _healRate = Mathf.Max(0, _healRate);
            }
        }
    }
    //物理穿透，只记录基础值，get时计算buff等影响
    private float _defBreak = 0;
    public float DefBreak
    {
        get { return _defBreak + BuffMgrObj.DefBreak; }
        set
        {
            if (_defBreak != value)
            {
                _defBreak = value;
            }
        }
    }
    //策略穿透，只记录基础值，get时计算buff等影响
    private float _mdefBreak = 0;
    public float MdefBreak
    {
        get { return _mdefBreak + BuffMgrObj.MdefBreak; }
        set
        {
            if (_mdefBreak != value)
            {
                _mdefBreak = value;
            }
        }
    }
    //混乱抗性，只记录基础值，get时计算buff等影响
    private float _deSowRate = 0;
    public float DeSowRate
    {
        get { return _deSowRate + BuffMgrObj.DeSowRate; }
        set
        {
            if (_deSowRate != value)
            {
                _deSowRate = value;
            }
        }
    }
    //混乱几率
    public float SowRate = 0;
    //眩晕抗性，只记录基础值，get时计算buff等影响
    private float _deFaintRate = 0;
    public float DeFaintRate
    {
        get { return _deFaintRate + BuffMgrObj.DeFaintRate; }
        set
        {
            if (_deFaintRate != value)
            {
                _deFaintRate = value;
            }
        }
    }
    //眩晕几率
    public float FaintRate = 0;
    //伪报抗性
    private float _deSpoofRate = 0;
    public float DeSpoofRate
    {
        get { return _deSpoofRate + BuffMgrObj.DeSowRate; }
        set
        {
            if (_deSpoofRate != value)
            {
                _deSpoofRate = value;
            }
        }
    }
    //伪报几率
    public float SpoofRate = 0;
    //离间抗性
    private float _deBetrayRate = 0;
    public float DeBetrayRate
    {
        get { return _deSpoofRate + BuffMgrObj.DeBetrayRate; }
        set {
            if (_deBetrayRate != value)
            {
                _deBetrayRate = value;
            }
        }
    }
    //离间几率
    public float BetrayRate = 0;

    #endregion
    #region 普通属性
    //马Id
    public int HorseId = 0;
    //当前位置
    public Vector2 CurPos = Vector2.zero;
	//目标位置
	public Vector2 TargetPos = Vector2.zero;
	//格子位置
	public int GridPos = 0;
    //布阵位置
    public int StubPos = 0;
    //朝向
    public Quaternion CurRot = Quaternion.identity;
    //本回合击杀敌将数量
    public int RoundKillNum = 0;
    //伤害统计
    public int HarmCount = 0;
    //战斗间隔
    public float FightInterval = 0;
    //战斗间隔累积
    public float FightIntervalAcc = 0;
    //战斗状态
    private FightUnitState _state = FightUnitState.Idle;
    public FightUnitState State
    {
        get { return _state; }
        set
        {
            if (_state != value)
            {
                _state = value;
                if (_state == FightUnitState.Move)
                {
                    //写在这里主要考虑到发起移动可能会有多种形式
                    ZEventSystem.Dispatch(EventConst.OnFightUnitMove, this, true);
                }
                else
                {
                    ZEventSystem.Dispatch(EventConst.OnFightUnitMove, this, false);
                }
            }
        }
    }
    #region Buff相关
    //控制状态，不可叠加
    private ControlState _cstate = ControlState.None;
    public ControlState CState
    {
        get { return _cstate; }
        set
        {
            if (_cstate != value)
            {
                //TODO通知显示层状态改变
                _cstate = value;
                Interrupt();
            }
        }
    }
    //特殊状态，可叠加
    public uint SState = 0;

    public void AddSpecialState(SpecialState state)
    {
        SState |= (uint)state;
    }
    public void RemoveSpecialState(SpecialState state)
    {
        SState &= ~(uint)state;
    }
    public bool GetSpecialState(SpecialState state)
    {
        return ((SState & (uint)state) != 0);
    }

    #endregion
    //系统保护，避免战斗单位在战斗结束后被杀死
    public bool SystemProtect = false;
    //无敌状态,只有对方破无敌才能打动
    public bool Invincible = false;
    //破无敌状态
    public bool DeInvincible = false;
    //抗死次数、强心针，抵挡死亡
    public int Booster = 0;
    //攻击目标，通过寻路找目标
    private FightUnit _fightTarget = null;
    public FightUnit FightTarget
    {
        get { return _fightTarget; }
        set
        {
            if (_fightTarget != value)
            {
                _fightTarget = value;
                ZEventSystem.Dispatch(EventConst.OnFightTargetChange, this, _fightTarget);
            }
        }
    }
    //受伤来源，造成本体此次受伤的FightUnit，当本体死亡时，就是杀死本体的凶手
    public FightUnit EnemyUnit = null;
    //受伤来源(技能id)，造成本次伤害的技能
    public int EnemySkill = 0;
    //嘲讽对象
    public FightUnit SneerTarget = null;
	//Buff管理器
	public BuffMgr BuffMgrObj = null;
    //寻路管理器
    public PathFinder PathFinderObj = null;
    //死亡累积
    public float DieAccTime = 0;
    //死亡时长
    public readonly float DieLast = 1.5f;
    #endregion
    #region 技能相关
    //技能管理器
    public SkillMgr SkillMgrObj = null;
    //是否正在使用主动技能
    public bool IsUsingActiveSkill = false;
    //是否处在大招前摇阶段
    public bool IsActiveSkillCradle = false;
    //技能状态
    public SkillState SklState = SkillState.Cradle;
    //前摇时间
    private float _cradleTime = 0;
    //前摇累计
    private float _cradleCur = 0;
    //后摇时间
    private float _postTime = 0;
    //后摇累计
    private float _postCur = 0;
    //效果次数
    private int _effectTimes = 0;
    //效果间隔
    private float _effectCD = 0;
    //技能等级
    private int _skillLevel = 0;
    //技能是否是普攻
    private bool _isNormalAttack = false;
    //技能配置
    private Skill _cskill = null;
    //技能目标
    private List<FightUnit> _skillTarget = new List<FightUnit>();
    //瞬发技能的技能目标
    private List<FightUnit> _promptSkillTarget = new List<FightUnit>();
    //技能效果
    private List<int> _skillEffects = new List<int>();
    //是否已经放过被动技能
    private bool _passiveSkillInited = false;
    #endregion
    #region 方法
    /// <summary>
    /// 构造方法-武将
    /// </summary>
    /// <param name="heroData"></param>
    /// <param name="stubPos"></param>
    /// <param name="isEnemy"></param>
    /// <param name="skillData"></param>
    public FightUnit(HeroData heroData, int stubPos, bool isEnemy)
    {
        Hero jHero = JsonMgr.GetSingleton().GetHeroByID(heroData.HeroId);
        if (jHero == null)
            return;
        this.HeroId = jHero.ID;
        this.HeroName = jHero.name;
        this.Sex = (jHero.sex != 0);
        this.Camp = jHero.camp;
        this.Level = heroData.Level;
        this.Star = heroData.Star;
        this.Rare = heroData.Rare;
        this.StubPos = stubPos;
        this.IsEnemy = isEnemy;
        this.HorseId = jHero.horseid;
        //血量成长率一定要在统率力之前，因为给统率力赋值直接计算HpMax
        HeroStar heroStar = JsonMgr.GetSingleton().GetHeroStarByID(this.HeroId);
        int index = this.Star - 1;
        this.HpGrowRate = heroStar.starhp[index];
        this.AtkGrowRate = heroStar.staratk[index];
        this.DefGrowRate = heroStar.stardef[index];
        this.MatkGrowRate = heroStar.starmatk[index];
        this.MdefGrowRate = heroStar.starmdef[index];
        this.Order = jHero.order;

        //乱序
        this.Order = Random.Range(1, 100);
        var AttrDic = heroData.AttrDic;
        this.LeadPower = (int)AttrDic[Attr.LeadPower];
        this.Power = (int)AttrDic[Attr.Power];
        this.Strategy = (int)AttrDic[Attr.Strategy];
        this.MaxHP = (int)AttrDic[Attr.MaxHp];
        this.BaseMaxHp = this.MaxHP;
        this.Atk = AttrDic[Attr.Atk];
        this.Matk = AttrDic[Attr.Matk];
        this.Def = AttrDic[Attr.Def];
        this.Mdef = AttrDic[Attr.MDef];
        this.DefRate = AttrDic[Attr.DefRate];
        this.MdefRate = AttrDic[Attr.MdefRate];
        this.DefBreak = AttrDic[Attr.DefBreak];
        this.MdefBreak = AttrDic[Attr.MdefBreak];
        this.DodgeRate = AttrDic[Attr.DodgeRate];
        this.HitRate = AttrDic[Attr.HitRate];
        this.BlockRate = AttrDic[Attr.BlockRate];
        this.RoutRate = AttrDic[Attr.RoutRate];
        this.FirmRate = AttrDic[Attr.FirmRate];
        this.CritRate = AttrDic[Attr.CritRate];
        this.CritInc = AttrDic[Attr.CritInc];
        this.HPRec = AttrDic[Attr.HpRec];
        this.VigourRec = (int)AttrDic[Attr.VigourRec];
        this.HPSuck = AttrDic[Attr.HpSuck];
        this.VigourSuck = AttrDic[Attr.VigourSuck];
        this.Asp = AttrDic[Attr.Asp];
        this.Arange = jHero.arange;
        this.HarmRate = AttrDic[Attr.HarmRate];
        this.HealRate = AttrDic[Attr.HealRate];
        this.DefBreak = AttrDic[Attr.DefBreak];
        this.MdefBreak = AttrDic[Attr.MdefBreak];
        this.FaintRate = AttrDic.ContainsKey(Attr.FaintRate) ? AttrDic[Attr.FaintRate] : 0;
        this.DeFaintRate = AttrDic.ContainsKey(Attr.DeFaintRate) ? AttrDic[Attr.DeFaintRate] : 0;
        this.SowRate = AttrDic.ContainsKey(Attr.SowRate) ? AttrDic[Attr.SowRate] : 0;
        this.DeSowRate = AttrDic.ContainsKey(Attr.DeSowRate) ? AttrDic[Attr.DeSowRate] : 0;
        this.SpoofRate = AttrDic.ContainsKey(Attr.SpoofRate) ? AttrDic[Attr.SpoofRate] : 0;
        this.DeSpoofRate = AttrDic.ContainsKey(Attr.DeSpoofRate) ? AttrDic[Attr.DeSpoofRate] : 0;
        this.BetrayRate = AttrDic.ContainsKey(Attr.BetrayRate) ? AttrDic[Attr.BetrayRate] : 0;
        this.DeBetrayRate = AttrDic.ContainsKey(Attr.DeBetrayRate) ? AttrDic[Attr.DeBetrayRate] : 0;


        //技能相关
        //JArray initSkillTurn = jHero["skillturn1"].ToObject<JArray>();
        //JArray skillTurn = jHero["skillturn2"].ToObject<JArray>();
        int activeSkill = 0;
        int normalSkill = 0;
        //寻找主动技能
        for (int idx = 0; idx < jHero.skills.Length; ++idx)
        {
            int skillid = jHero.skills[idx];
            Skill cskill = JsonMgr.GetSingleton().GetSkillByID(skillid);
            int skillType = cskill.type;
            if (skillType == (int)SkillType.ACTIVE)
            {
                activeSkill = skillid;
            }
            else if (skillType == (int)SkillType.NORMAL)
            {
                normalSkill = skillid;
            }
        }
        this.SkillMgrObj = new SkillMgr(this, heroData.SkillLevel, jHero.skills, jHero.skillturn1, jHero.skillturn2, activeSkill, normalSkill);

        

        Init();
        /*EDebug.LogFormat("英雄id：{0} 名字：{1} 性别：{2} 阵营：{3} 等级：{4} 星级：{5} 品级：{6} 血量成长系数：{7} 物理攻击成长系数：{8} " +
            "物理防御成长：{9} 策略攻击成长：{10} 策略防御成长：{11} 先手值：{12} 统率力：{13} 武力：{14} 策略：{15} 闪避：{16} 命中：{17} " +
            "格挡：{18} 破击：{19} 坚韧：{20} 暴击：{21} 暴击成长：{22} hp回复：{23} 士气回复：{24} 吸血：{25} 吸士气：{26} 攻速：{27} " +
            "伤害加成：{28} 治疗加成：{29} 物理穿透：{30} 策略穿透：{31} ", this.HeroId, this.HeroName, this.Sex, this.Camp, this.Level, this.Star, this.Rare,
            this.HpGrowRate, this.AtkGrowRate, this.DefGrowRate, this.MatkGrowRate, this.MdefGrowRate, this.Order, this.LeadPower, this.Power, this.Strategy, this.Dodge, this.Hit,
            this.Block, this.Rout, this.Firm, this.Crit, this.CritInc, this.HPRec, this.VigourRec, this.HPSuck, this.VigourSuck, this.Asp, this.HarmRate, this.HealRate,
            this.DefBreak, this.MdefBreak);*/
    }

    /// <summary>
    /// 构造方法-怪物
    /// </summary>
    /// <param name="tplid"></param>
    /// <param name="stubPos"></param>
    /// <param name="isEnemy"></param>
    /// <param name="bySummon"></param>
    public FightUnit(Monster monster, int stubPos, bool isEnemy, bool bySummon = false)
    {
        this.HeroId = monster.ID;
        this.HeroName = monster.name;
        this.Level = monster.level;
        this.Star = monster.star;
        this.Rare = monster.rare;
        this.IsEnemy = isEnemy;
        this.StubPos = stubPos;
        this.IsMonster = true;
        this.IsSummon = bySummon;

        //乱序
        this.Order = UnityEngine.Random.Range(1, 100);

        this.LeadPower = monster.leadpower;
        this.Power = monster.power;
        this.Strategy = monster.strategy;
        this.BaseMaxHp = monster.maxhp;
        this.MaxHP = BaseMaxHp;
        this.Atk = monster.atk;
        this.Matk = monster.matk;
        this.DefRate = monster.defrate;
        this.MdefRate = monster.mdefrate;
        this.DefBreak = monster.defbreak;
        this.MdefBreak = monster.mdefbreak;
        this.Asp = monster.asp;
        this.Arange = monster.arange;
        this.HarmRate = monster.harmrate;
        this.HealRate = monster.healrate;
        this.DodgeRate = monster.dodgerate;
        this.HitRate = monster.hitrate;
        this.BlockRate = monster.blockrate;
        this.RoutRate = monster.routrate;
        this.FirmRate = monster.firmrate;
        this.CritRate = monster.critrate;
        this.CritInc = monster.critinc;
        this.DeSowRate = monster.desowrate;
        this.DeFaintRate = monster.defaintrate;
        this.DeSpoofRate = monster.despoofrate ;
        this.DeBetrayRate = monster.debetrayrate;

        Dictionary<int, int> skillData = new Dictionary<int, int>();
        //技能相关
        //JArray allSkills = jMonster["skills"].ToObject<JArray>();

        for (int idx = 0; idx < monster.skills.Length; ++idx)
        {
            int skillid = monster.skills[idx];
            skillData[skillid] = this.Level;
        }

        //JArray initSkillTurn = jMonster["skillturn1"].ToObject<JArray>();
        //JArray skillTurn = jMonster["skillturn2"].ToObject<JArray>();
        int activeSkill = 0;
        int normalSkill = 0;
        //寻找主动技能
        for (int idx = 0; idx < monster.skills.Length; ++idx)
        {
            int skillid = monster.skills[idx];
            Skill cskill = JsonMgr.GetSingleton().GetSkillByID(skillid);
            int skillType = cskill.type;
            if (skillType == (int)SkillType.ACTIVE)
            {
                activeSkill = skillid;
            }
            else if (skillType == (int)SkillType.NORMAL)
            {
                normalSkill = skillid;
            }
        }
        this.SkillMgrObj = new SkillMgr(this, skillData, monster.skills, monster.skillturn1, monster.skillturn2, activeSkill, normalSkill);
        Init();
    }

    /// <summary>
    /// 初始化方法，每个构造方法末尾调用
    /// </summary>
    public void Init()
    {
        this.PathFinderObj = new PathFinder(this);
        this.BuffMgrObj = new BuffMgr(this);
    }

    public void InitPassiveSkill()
    {
        if (_passiveSkillInited)
            return;
        Dictionary<int, int> _skillData = this.SkillMgrObj.GetSkillData();
        foreach (var skldata in _skillData)
        {
            Skill cskill = JsonMgr.GetSingleton().GetSkillByID(skldata.Key);
            int type = cskill.type;
            if (type == (int)SkillType.PASSIVE)
            {
                UseSKill(cskill, skldata.Value);
            }
        }
        _passiveSkillInited = true;
    }

    /// <summary>
    /// 初始化每回合的战斗初始位置
    /// </summary>
    /// <param name="round"></param>
    public void InitFightPos(int round)
    {
        int fightPos = PathFinder.Stub2InitPos(round, this.IsEnemy, this.StubPos);
        this.PathFinderObj.SetPos(fightPos);
    }

    /// <summary>
    /// 更新当前的特殊状态
    /// </summary>
    /// <param name="state"></param>
    /// <param name="addorremove">添加还是删除</param>
    public void UpdateSpecialState(SpecialState state, bool addorremove)
    {
        if (addorremove)
        {
            if (!CheckSpecialState(state))
            {
                SState |= (uint)state;
            }
        }
        else
        {
            if (CheckSpecialState(state))
            {
                SState &= ~(uint)state;
            }
        }
    }

    /// <summary>
    /// 检查是否有某个特殊状态
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public bool CheckSpecialState(SpecialState state)
    {
        return ((this.SState & (uint)state) != 0);
    }

    public void NewRound()
    {
        this.RoundKillNum = 0;
        this.SklState = SkillState.Cradle;
        this.State = FightUnitState.Idle;
    }

    public void RoundOver()
    {
        int vigourAward = (int)JsonMgr.GetSingleton().GetGlobalIntArrayByID(1007).value * RoundKillNum + this.VigourRec;
        if (vigourAward > 0)
        {
            this.AddVigour(vigourAward);
            ZEventSystem.Dispatch(EventConst.OnFightUnitPop, this, new Vector2Int((int)EffectState.HealVigour, vigourAward));
        }
        int hpAward = (int)(this.HPRec * this.MaxHP);
        this.CurHP += hpAward;
        ZEventSystem.Dispatch(EventConst.OnFightUnitPop, this, new Vector2Int((int)EffectState.Heal, hpAward));
    }

    /// <summary>
    /// 杀人加士气
    /// </summary>
    /// <param name="unit"></param>
    public void OnKillUnit(FightUnit unit)
    {
        if (unit.IsEnemy == this.IsEnemy)
            return;
        this.RoundKillNum++;
        int vigourAward = (int)JsonMgr.GetSingleton().GetGlobalIntArrayByID(1006).value;
        this.AddVigour(vigourAward);
        ZEventSystem.Dispatch(EventConst.OnFightUnitPop, this, new Vector2Int((int)EffectState.HealVigour, vigourAward));
    }

    /// <summary>
    /// 进行攻击攻击，如果距离不够就先寻路
    /// </summary>
    public void Attack()
    {

        Vector2Int selSkill = SkillMgrObj.SelectSkill();
        if (selSkill.x == 0)
        {
            EDebug.LogErrorFormat("FightUnit {0} 选择技能失败", this.HeroId);
            return;
        }
        Skill cskill = JsonMgr.GetSingleton().GetSkillByID(selSkill.x);
        if (cskill.type == (int)SkillType.PASSIVE)
        {
            EDebug.LogErrorFormat("FightUnit {0} Attack failed, skill {1} is passive skill", this.HeroId, selSkill.x);
            return;
        }


        if (cskill.target == (int)SkillTarget.CUR)
        {
            if (this.FightTarget == null)
            {
                FightUnit target = FightLogic.Instance.SelectFightTarget(this, false);
                if (target != null)
                {
                    this.PathFinderObj.StartFind();
                }
                return;
            }
        }
        UseSKill(cskill, selSkill.y);
    }

    /// <summary>
    /// 放大招
    /// </summary>
    public void UseActiveSkill()
    {
        if (SkillMgrObj.IsActiveSkillEnable(this))
        {
            this.AddVigour(-MaxVigour);
            this.SkillMgrObj.UseActiveSkill();
        }
    }

    /// <summary>
    /// 使用技能，从这个阶段开始进入Skill阶段
    /// </summary>
    /// <param name="cskill"></param>
    /// <param name="level"></param>
    public void UseSKill(Skill cskill, int level)
    {
        int skillType = cskill.type;
        int skillTarget = cskill.target;

        if (skillType == (int)SkillType.NORMAL || skillType == (int)SkillType.AUTO)
            this.SkillMgrObj.UpdateSkillFlag();
        else if (skillType == (int)SkillType.ACTIVE)
            this.SkillMgrObj.ClearActiveSkill();
        else if (skillType == (int)SkillType.PROMPT || skillType == (int)SkillType.PASSIVE)
        {
            //瞬发技能直接选目标生效
            _promptSkillTarget.Clear();
            _selectSkillTarget(cskill, skillTarget, ref _promptSkillTarget);

            int[] effects = cskill.effects;
            ZEventSystem.Dispatch(EventConst.OnFightUnitSkill, this, cskill, 1, _promptSkillTarget.Count != 0 ? _promptSkillTarget[0] : null);
            for (int idx = 0; idx < _promptSkillTarget.Count; ++idx)
            {
                FightUnit target = _promptSkillTarget[idx];
				if(target == null)
					continue;
                for (int idx2 = 0; idx2 < effects.Length; ++idx2)
                {
                    int effectId = effects[idx2];
                    JObject ceffect = JsonMgr.GetSingleton().GetSkillEffect(effectId);
                    MakeEffect(ceffect, cskill.ID, level, target);
                }
            }
            return;
        }


        this.State = FightUnitState.Skill;
        this.SklState = SkillState.Cradle;

        _skillTarget.Clear();
        _selectSkillTarget(cskill, skillTarget, ref _skillTarget);
        //处理转向
        if (skillTarget == (int)SkillTarget.CUR || skillTarget == (int)SkillTarget.PARTNER || skillTarget == (int)SkillTarget.ENEMY)
        {
            if (_skillTarget.Count != 0)
            {
                FightUnit target = _skillTarget[0];
                int xoffset = Util.Judge(target.GridPos / PathFinder.V_GRID - this.GridPos / PathFinder.V_GRID);
                int yoffset = Util.Judge(target.GridPos % PathFinder.V_GRID - this.GridPos % PathFinder.V_GRID);
                //this.CurRot.SetLookRotation(new Vector3(xoffset, 0, -yoffset));
                Vector2 lookDir = target.CurPos - this.CurPos;
                this.CurRot.SetLookRotation(new Vector3(lookDir.x, 0, lookDir.y));
            }
        }

        //技能增加士气
        if (skillType == (int)SkillType.AUTO || skillType == (int)SkillType.ACTIVE)
        {
            int attackVigour = cskill.attackvigour;
            int gAttackVigour = (int)JsonMgr.GetSingleton().GetGlobalIntArrayByID(1005).value;
            this.AddVigour(gAttackVigour * attackVigour);
        }

        //添加技能效果
        _skillEffects.Clear();
        int[] skillEffects = cskill.effects;
        for (int idx = 0; idx < skillEffects.Length; ++idx)
            _skillEffects.Add(skillEffects[idx]);


        this._cradleTime = cskill.cradle;
        this._cradleCur = 0;
        this._postTime = cskill.post;
        this._postCur = 0;
        this._effectTimes = cskill.effecttimes;
        this._effectCD = cskill.effectcd;
        this._skillLevel = level;
        this._isNormalAttack = (cskill.type == 1);
        this._cskill = cskill;
        float skillTimeScale = 1;
        if (skillType == (int)SkillType.NORMAL)
        {
            skillTimeScale = (1 / this.Asp / (_cradleTime + _postTime));
            this._cradleTime *= skillTimeScale;
            this._postTime *= skillTimeScale;
        }

        
        if (skillType == (int)SkillType.ACTIVE)
        {
            this.IsUsingActiveSkill = true;
            this.IsActiveSkillCradle = true;
            if(!this.IsMonster)
                ZEventSystem.Dispatch(EventConst.OnRequestUnitPause, this, true);
        }
        ZEventSystem.Dispatch(EventConst.OnFightUnitSkill, this, cskill, skillTimeScale, _skillTarget.Count != 0 ? _skillTarget[0] : null);

        if (skillType != (int)SkillType.PASSIVE)
        {
            this.FightInterval = JsonMgr.GetSingleton().GetGlobalIntArrayByID(1011).value + Random.Range(0, JsonMgr.GetSingleton().GetGlobalIntArrayByID(1012).value);
            this.FightIntervalAcc = 0;
        }
    }

    /// <summary>
    /// 打断
    /// </summary>
    /// <param name="cskill">被打断的技能</param>
    public void Interrupt()
    {
        if (this._cskill != null)
        {
            int startEffect = this._cskill.starteffect;
            if (startEffect != 0)
                EffectMgr.Instance.RemoveEffect(this, startEffect);
            this._cskill = null;
        }
        this.State = FightUnitState.Idle;
        this.FightTarget = null;
        if (IsUsingActiveSkill)
        {
            ZEventSystem.Dispatch(EventConst.OnRequestUnitPause, this, false);
        }
        ZEventSystem.Dispatch(EventConst.OnFightUnitInterrupt, this);
    }

    /// <summary>
    /// 造成伤害或者治疗
    /// </summary>
    /// <param name="data"></param>
    /// <param name="caster"></param>
    /// <param name="sklid"></param>
    /// <returns>是否击杀</returns>
    public bool HarmHp(Vector2Int data, FightUnit caster, int sklid, bool pop = true)
	{
        if (data.y < 0 && (this.SystemProtect || (this.Invincible && !caster.DeInvincible)))
            return false;

        int harm = (int)data.y;

        //处理普攻加士气
        if (harm < 0 && sklid != 0)
        {
            Skill cskill = JsonMgr.GetSingleton().GetSkillByID(sklid);
            if (cskill == null)
            {
                EDebug.LogErrorFormat("FightUnit.HarmHP AddVigour failed，no such skill {0}", sklid);
            }
            else
            {
                int sklType = cskill.type;
                if (sklType == (int)SkillType.NORMAL)
                {
                    int attackVigour = cskill.attackvigour;
                    int gAttackVigour = (int)JsonMgr.GetSingleton().GetGlobalIntArrayByID(1003).value;
                    caster.AddVigour(attackVigour * gAttackVigour);
                }
            }
        }

        if (harm < 0)
        {
            //伤害统计
            caster.HarmCount += -harm;

            //处理护盾逻辑
            int resist = this.BuffMgrObj.ResistHarm(-harm);
            harm += resist;
            if (0 == harm)
                return this.IsDead;

            if (SklState == SkillState.Post)
            {
                ZEventSystem.Dispatch(EventConst.OnPlayHitAnim, this);
            }
            this.EnemyUnit = caster;
            this.EnemySkill = sklid;

            //处理HPSuck
            caster.HarmHp(new Vector2Int(0, (int)(-harm * caster.HPSuck)), null, 0, false);
        }
        this.CurHP += harm;
        if(harm < 0)
            ZEventSystem.Dispatch(EventConst.OnFightUnitHit, this, data);
        if(pop)
            ZEventSystem.Dispatch(EventConst.OnFightUnitPop, this, data);

        return this.IsDead;
    }

    /// <summary>
    /// 造成士气伤害或者治疗
    /// </summary>
    /// <param name="value"></param>
    public void HarmVigour(int value, bool over = false)
    {
        AddVigour(value, over);
        if(value > 0)
            ZEventSystem.Dispatch(EventConst.OnFightUnitPop, this, new Vector2Int((int)EffectState.HealVigour, value));
    }
	
	public void Update(float delta)
	{
        if (this.IsDead)
        {
            if (this.DieAccTime < DieLast)
            {
                this.DieAccTime += delta;
            }
            return;
        }

        _checkFightTarget();

        if (this.PathFinderObj != null)
        {
            this.PathFinderObj.Update(delta);
        }
        if (!FightLogic.Instance.FightPause)
        {
            _updateSkill(delta);
            this.BuffMgrObj.Update();
        }
	}


    /// <summary>
    /// 技能、子弹效果生效
    /// </summary>
    /// <param name="ceffect"></param>
    /// <param name="skillTarget"></param>
    /// <returns></returns>
    public bool MakeEffect(JObject ceffect, int sklid, int skillLevel, FightUnit skillTarget)
    {
        if (ceffect == null)
        {
            EDebug.LogError("FightUnit._takeEffect get skilleffect failed");
            return false;
        }

        if (skillTarget == null)
            return false;

        //对敌方还是友方
        bool effectTarget = ceffect["target"].ToObject<int>() == 1;
        if (this.CState != ControlState.Sow)
        {
            if (this.CState == ControlState.Betray)
            {
                if (effectTarget == (this.IsEnemy != skillTarget.IsEnemy))
                    return false;
            }
            else
            {
                if (effectTarget != (this.IsEnemy != skillTarget.IsEnemy))
                    return false;
            }
        }
        if (skillTarget.Invincible && !this.DeInvincible)
        {
            if (effectTarget)
                return false;
        }

        //处理受击加士气逻辑
        int hitVigour = ceffect["hitvigour"].ToObject<int>();
        if (ceffect["isnormal"].ToObject<int>() != 0)
        {
            int gNormalHitVigour = (int)JsonMgr.GetSingleton().GetGlobalIntArrayByID(1002).value;
            skillTarget.AddVigour(hitVigour * gNormalHitVigour);
        }
        else
        {
            int gHitVigour = (int)JsonMgr.GetSingleton().GetGlobalIntArrayByID(1004).value;
            skillTarget.AddVigour(hitVigour * gHitVigour);
        }
        //通知技能受体的View显示受击特效
        ZEventSystem.Dispatch(EventConst.OnFightUnitTakenEffect, skillTarget, ceffect);
        

        int effectType = ceffect["effect"].ToObject<int>();
        int paramType = ceffect["paramtype"].ToObject<int>();
        JArray effectParam = ceffect["effectparam"].ToObject<JArray>();
        Vector3 param = AttrUtil.CalExpression(paramType, effectParam, this, skillTarget, skillLevel);

        switch (effectType)
        {
            case (int)EffectType.HARM_HP:
                skillTarget.HarmHp(AttrUtil.CalHarm(_isNormalAttack, param.x == 1, -param.y, this, skillTarget), this, sklid);
                break;
            case (int)EffectType.HEAL_HP:
                skillTarget.HarmHp(AttrUtil.CalHarm(_isNormalAttack, param.x == 1, param.y, this, skillTarget), this, sklid);
                break;
            case (int)EffectType.HARM_VIGOUR:
                skillTarget.HarmVigour(-(int)param.y);
                break;
            case (int)EffectType.HEAL_VIGOUR:
                skillTarget.HarmVigour((int)param.y);
                break;
            case (int)EffectType.BULLET:
                FightLogic.Instance.BulletMgrObj.CrateBullet((int)param.y, _skillLevel, this, skillTarget, this._cskill);
                break;
            case (int)EffectType.BUFF:
                {
                    float extraBuffRate = 0;
                    float buffRate = param.x;
                    int buffId = (int)param.y;
                    JObject cbuff = JsonMgr.GetSingleton().GetBuff(buffId);
                    if (cbuff != null)
                    {
                        int effect = cbuff["effect"].ToObject<int>();
                        switch (effect)
                        {
                            case (int)BuffEffect.Faint:
                                extraBuffRate = this.FaintRate - skillTarget.DeFaintRate;
                                break;
                            case (int)BuffEffect.Sow:
                                extraBuffRate = this.SowRate - skillTarget.DeSowRate;
                                break;
                            case (int)BuffEffect.Spoof:
                                extraBuffRate = this.SpoofRate - skillTarget.DeSpoofRate;
                                break;
                            case (int)BuffEffect.Betray:
                                extraBuffRate = this.BetrayRate - skillTarget.DeBetrayRate;
                                break;
                            default:
                                extraBuffRate = 0;
                                break;
                        }
                        if (Random.Range(0, 1.0f) < (param.x + extraBuffRate))
                        {
                            skillTarget.BuffMgrObj.AddBuff(this, (int)param.y, skillLevel);
                        }
                    }
                }
                break;
            case (int)EffectType.HEAL_OVER_VIGOUR:
                skillTarget.HarmVigour((int)param.y, true);
                break;
            case (int)EffectType.Summon:
                FightLogic.Instance.Summon((int)param.x, skillLevel, this.IsEnemy, (int)param.y, (param.z != 0));
                break;
            case (int)EffectType.Dispel:
                if (Random.Range(0, 1.0f) < param.y)
                {
                    skillTarget.BuffMgrObj.ClearBuff(ClearBuffType.Dispel);
                }
                break;
            case (int)EffectType.Purge:
                if (Random.Range(0, 1.0f) < param.y)
                {
                    skillTarget.BuffMgrObj.ClearBuff(ClearBuffType.Purge);
                }
                break;
            case (int)EffectType.Booster:
                skillTarget.Booster += (int)param.y;
                break;
            default:
                break;
        }
        return true;
    }

    /// <summary>
    /// 是否能被攻击到
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="igonreInvincible"></param>
    /// <param name="ignoreDistance"></param>
    /// <param name="ignorePathFinder">不无视寻路可能会有效率问题，但是更准确</param>
    /// <returns></returns>
    public bool CanBeAttack(FightUnit attacker, bool igonreInvincible, bool ignoreDistance, bool ignorePathFinder)
    {
        if (this.IsDead)
            return false;
        if (this == attacker)
            return false;
        if (attacker.CState != ControlState.Sow)
        {
            bool sameCamp = attacker.IsEnemy == this.IsEnemy;
            if (attacker.CState != ControlState.Betray ? sameCamp : !sameCamp)
                return false;
        }
        if (!igonreInvincible && this.Invincible && !attacker.DeInvincible)
            return false;
        float gridDis = PathFinder.GridDis(this.GridPos, attacker.GridPos);
        if (!ignoreDistance && gridDis > attacker.Arange)
            return false;
        if (attacker.CState == ControlState.Sneer && attacker.SneerTarget != null & attacker.SneerTarget != this)
            return false;
        if (!ignorePathFinder && PathFinder.GridDis(this.GridPos, attacker.GridPos) > attacker.Arange
            && attacker.PathFinderObj.NextGrid(this.GridPos) < 0)
            return false;
        return true;
    }

    /// <summary>
    /// 选择技能目标
    /// </summary>
    /// <param name="cskill"></param>
    /// <param name="target"></param>
    /// <param name="skillTarget"></param>
    private void _selectSkillTarget(Skill cskill, int target, ref List<FightUnit> skillTarget)
    {
        switch (target)
        {
            case (int)SkillTarget.SELF:
                skillTarget.Add(this);
                break;
            case (int)SkillTarget.CUR:
                if(this.FightTarget != null)
                    skillTarget.Add(this.FightTarget);
                break;
            case (int)SkillTarget.PARTNER:
                {
                    FightUnit appoint = _selectAppointTarget(IsEnemy, cskill.choosetype);
                    if (appoint == null)
                        EDebug.LogErrorFormat("FightUnit.UseSkill failed, SelectTarget.PARTNER, select null, skill:{0}", cskill);
                    else
                        skillTarget.Add(appoint);
                }
                break;
            case (int)SkillTarget.ENEMY:
                {
                    FightUnit appoint = _selectAppointTarget(!IsEnemy, cskill.choosetype);
                    if (appoint == null)
                        EDebug.LogErrorFormat("FightUnit.UseSkill failed, SelectTarget.ENEMY, select null, skill:{0}", cskill);
                    else
                        skillTarget.Add(appoint);
                }
                break;
            case (int)SkillTarget.RAND_PARTNER:
                {
                    int randomCnt = cskill.targetnum;
                    List<FightUnit> allFighters = FightLogic.Instance.AllFighters;
                    for (int idx = 0; idx < allFighters.Count; ++idx)
                    {
                        FightUnit unit = allFighters[idx];
                        if (unit.IsDead)
                            continue;
                        if (this.CState != ControlState.Sow)
                        {
                            bool sameCamp = this.IsEnemy == unit.IsEnemy;
                            if (this.CState != ControlState.Betray ? !sameCamp : sameCamp)
                                continue;
                        }
                        skillTarget.Add(unit);
                    }
                    if (randomCnt < skillTarget.Count)
                    {
                        int tickCnt = skillTarget.Count - randomCnt;
                        for (int idx = 0; idx < tickCnt; ++idx)
                        {
                            int rdm = Random.Range(0, skillTarget.Count);
                            skillTarget.RemoveAt(rdm);
                        }
                    }
                }
                break;
            case (int)SkillTarget.RAND_ENEMY:
                {
                    int randomCnt = cskill.targetnum;
                    List<FightUnit> allFighters = FightLogic.Instance.AllFighters;
                    for (int idx = 0; idx < allFighters.Count; ++idx)
                    {
                        FightUnit unit = allFighters[idx];
                        if (unit.IsDead)
                            continue;
                        if (this.CState != ControlState.Sow)
                        {
                            bool sameCamp = this.IsEnemy == unit.IsEnemy;
                            if (this.CState != ControlState.Betray ? sameCamp : !sameCamp)
                                continue;
                        }
                        skillTarget.Add(unit);
                    }
                    if (randomCnt < skillTarget.Count)
                    {
                        int tickCnt = skillTarget.Count - randomCnt;
                        for (int idx = 0; idx < tickCnt; ++idx)
                        {
                            int rdm = Random.Range(0, skillTarget.Count);
                            skillTarget.RemoveAt(rdm);
                        }
                    }
                }
                break;
            case (int)SkillTarget.ALL_UNIT:
                {
                    List<FightUnit> allFighters = FightLogic.Instance.AllFighters;
                    for (int idx = 0; idx < allFighters.Count; ++idx)
                    {
                        FightUnit unit = allFighters[idx];
                        if (unit.IsDead)
                            continue;
                        skillTarget.Add(unit);
                    }
                }
                break;
            case (int)SkillTarget.ENEMY_UNIT:
                {
                    if(this.EnemyUnit != null)
                        skillTarget.Add(this.EnemyUnit);
                }
                break;
        }
        //根据目标范围继续加目标
        _handleSkillRange(cskill, ref skillTarget);
        //包含自己作为目标,一定要在最后加
        int selftarget = cskill.selftarget;
        if (selftarget > 0 && !_skillTarget.Contains(this))
            _skillTarget.Add(this);
        if (selftarget < 0 && skillTarget.Contains(this))
            _skillTarget.Remove(this);
    }

    /// <summary>
    /// 选择特定目标
    /// </summary>
    /// <param name="isEnemy"></param>
    /// <param name="chooseType"></param>
    /// <returns></returns>
    private FightUnit _selectAppointTarget(bool isEnemy, int chooseType)
    {
        FightUnit sel = null;
        float value = -1;
        List<FightUnit> allFighter = FightLogic.Instance.AllFighters;
        for (int idx = 0; idx < allFighter.Count; ++idx)
        {
            FightUnit unit = allFighter[idx];
            if (unit.IsDead)
                continue;
            if (this.CState != ControlState.Sow)
            {
                bool sameCamp = isEnemy == unit.IsEnemy;
                if (this.CState != ControlState.Betray ? !sameCamp : sameCamp)
                    continue;
            }
            switch (chooseType)
            {
                case (int)ChooseType.LEAD_MAX:
                    {
                        int lead = unit.LeadPower;
                        if (sel == null || lead > value)
                        {
                            sel = unit;
                            value = lead;
                        }
                    }
                    break;
                case (int)ChooseType.LEAD_MIN:
                    {
                        int lead = unit.LeadPower;
                        if (sel == null || lead < value)
                        {
                            sel = unit;
                            value = lead;
                        }
                    }
                    break;
                case (int)ChooseType.POWER_MAX:
                    {
                        int power = unit.Power;
                        if (sel == null || power > value)
                        {
                            sel = unit;
                            value = power;
                        }
                    }
                    break;
                case (int)ChooseType.POWER_MIN:
                    {
                        int power = unit.Power;
                        if (sel == null || power < value)
                        {
                            sel = unit;
                            value = power;
                        }
                    }
                    break;
                case (int)ChooseType.STRATEGY_MAX:
                    {
                        int strategy = unit.Strategy;
                        if (sel == null || strategy > value)
                        {
                            sel = unit;
                            value = strategy;
                        }
                    }
                    break;
                case (int)ChooseType.STRATEGY_MIN:
                    {
                        int strategy = unit.Strategy;
                        if (sel == null || strategy < value)
                        {
                            sel = unit;
                            value = strategy;
                        }
                    }
                    break;
                case (int)ChooseType.HP_MAX:
                    {
                        float hp = ((float)unit.CurHP / unit.MaxHP);
                        if (sel == null || hp > value)
                        {
                            sel = unit;
                            value = hp;
                        }
                    }
                    break;
                case (int)ChooseType.HP_Min:
                    {
                        float hp = ((float)unit.CurHP / unit.MaxHP);
                        if (sel == null || hp < value)
                        {
                            sel = unit;
                            value = hp;
                        }
                    }
                    break;
                case (int)ChooseType.DISTANCE_MAX:
                    {
                        float dis = (unit.CurPos - this.CurPos).magnitude;
                        if (sel == null || dis > value)
                        {
                            sel = unit;
                            value = dis;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        return sel;
    }

    private List<FightUnit> _handleSkillRangeTmpBuffer = new List<FightUnit>();
    /// <summary>
    /// 根据选择的技能目标以及技能范围扩展目标列表
    /// </summary>
    private void _handleSkillRange(Skill cskill, ref List<FightUnit> skilltarget)
    {
        int rangeType = cskill.rangetype;
        float[] range = cskill.range;
        int rangeenemy = cskill.rangeenemy;
        List<FightUnit> allFightUnit = FightLogic.Instance.AllFighters;

        for (int idx = 0; idx < skilltarget.Count; ++idx)
        {
            FightUnit target = skilltarget[idx];
			if(target == null)
				continue;
            for (int idx2 = 0; idx2 < allFightUnit.Count; ++idx2)
            {
                FightUnit unit = allFightUnit[idx2];
                if (unit.IsDead)
                    continue;
                if (rangeenemy == (int)RangeEnemy.Partner && this.IsEnemy != unit.IsEnemy ||
                    rangeenemy == (int)RangeEnemy.Enemy && this.IsEnemy == unit.IsEnemy)
                    continue;
                switch (rangeType)
                {
                    case (int)RangeType.Line:
                        {
                            Vector2 dir = (target.CurPos - this.CurPos).normalized;
                            Vector2 curDir = (unit.CurPos - this.CurPos).normalized;
                            if (range.Length >= 1 && Vector2.Dot(dir, curDir) >= 0 && Util.PointToStraightlineDistance(this.CurPos, target.CurPos, unit.CurPos) <= range[0])
                            {
                                _handleSkillRangeTmpBuffer.Add(unit);
                            }
                        }
                        break;
                    case (int)RangeType.Circle:
                        {
                            if (range.Length >= 1 && (unit.CurPos - target.CurPos).sqrMagnitude <= Mathf.Pow(range[0], 2))
                                _handleSkillRangeTmpBuffer.Add(unit);
                        }
                        break;
                    case (int)RangeType.Fan:
                        {
                            Vector3 targetForward = target.CurRot * Vector3.forward;
                            Vector2 forwardDir = new Vector2(targetForward.x, targetForward.z);
                            Vector2 curDir = (unit.CurPos - this.CurPos).normalized;
                            if (range.Length >= 2 && Vector2.Dot(forwardDir, curDir) <= Mathf.Cos(range[0] * Mathf.Deg2Rad) && 
                                (unit.CurPos - target.CurPos).sqrMagnitude <= Mathf.Pow(range[1], 2))
                                _handleSkillRangeTmpBuffer.Add(unit);
                        }
                        break;
                    case (int)RangeType.Rectangle:
                        {
                            Vector3 targetForward = target.CurRot * Vector3.forward;
                            Vector2 forwardDir = new Vector2(targetForward.x, targetForward.z);
                            Vector2 unitDir = unit.CurPos - this.CurPos;
                            float dirOffset = Vector2.Dot(forwardDir, unitDir);
                            if (range.Length >= 2 && Util.PointToStraightlineDistance(this.CurPos, this.CurPos + forwardDir, unit.CurPos) <= range[0] 
                                && dirOffset >= 0 && dirOffset <= range[1])
                                _handleSkillRangeTmpBuffer.Add(unit);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        for (int idx = 0; idx < _handleSkillRangeTmpBuffer.Count; ++idx)
        {
            FightUnit unit = _handleSkillRangeTmpBuffer[idx];
            if (!skilltarget.Contains(unit))
                skilltarget.Add(unit);
        }

        _handleSkillRangeTmpBuffer.Clear();
    }

    //处理技能效果
    private bool _takeEffect()
    {
        int validTargetCnt = 0;

        for (int eid = 0; eid < _skillEffects.Count; ++eid)
        {
            int effectId = _skillEffects[eid];
            JObject ceffect = JsonMgr.GetSingleton().GetSkillEffect(effectId);
            for (int idx = 0; idx < _skillTarget.Count; ++idx)
            {
                FightUnit skillTarget = _skillTarget[idx];
                if (skillTarget == null || skillTarget.IsDead)
                    continue;
                validTargetCnt++;
                MakeEffect(ceffect, this._cskill.ID, _skillLevel, skillTarget);
            }

        }
        return validTargetCnt > 0;
    }

    //检查FightTarget
    private void _checkFightTarget()
    {
        if (this.FightTarget != null)
        {
            //选到的都是活着的，但是可能是null
            FightUnit nearestTarget = FightLogic.Instance.SelectFightTarget(this, true);

            if (nearestTarget == null)  //说明攻击距离内已经没有了
            {
                this.FightTarget = null;
            }
            else
            {
                //选到了一个最近的可攻击目标，并且在攻击距离内
                if (this.FightTarget.IsDead ||  
                    PathFinder.GridDis(this.GridPos, nearestTarget.GridPos) < PathFinder.GridDis(this.GridPos, this.FightTarget.GridPos))
                {
                    this.FightTarget = nearestTarget;
                }
            }
        }
    }

    //技能循环
    private void _updateSkill(float delta)
    {
        if (FightLogic.Instance.State == FightState.Fight)
        {
            if (State == FightUnitState.Idle)
            {
                this.FightIntervalAcc += delta;
                if(this.SkillMgrObj.IsWaitActiveSkill() || (this.FightIntervalAcc >= FightInterval) && 
                    CState != ControlState.Faint && CState != ControlState.Spoof)
                    Attack();
            }
            else if (State == FightUnitState.Skill)
            {
                //大招打断一切
                if (SkillMgrObj.IsWaitActiveSkill())
                {
                    Interrupt();
                }
                switch (SklState)
                {
                    case SkillState.Cradle:
                        {

                            {
                                _cradleCur += delta;
                                if (_cradleCur >= _cradleTime)
                                {
                                    SklState = SkillState.Effect;
                                    ZEventSystem.Dispatch(EventConst.OnSkillTakeEffect, this, this._cskill);
                                }
                            }
                        }
                        break;
                    case SkillState.Effect:
                        {
                            if (this._cskill == null)
                                return;
                            if (!_takeEffect())
                            {
                                Interrupt();
                                return;
                            }
                            if (--this._effectTimes <= 0)
                            {
                                SklState = SkillState.Post;
                                if (IsUsingActiveSkill)
                                {
                                    this.IsActiveSkillCradle = false;
                                    if(!this.IsMonster)
                                        ZEventSystem.Dispatch(EventConst.OnRequestUnitPause, this, false);
                                }
                                //技能释放完成，_cskill置空，这样根据_cskill就知道是否正在放技能
                                this._cskill = null;
                            }
                            else
                            {
                                _cradleTime = _effectCD;
                                SklState = SkillState.Cradle;
                                _cradleCur = 0;
                            }
                        }
                        break;
                    case SkillState.Post:
                        {
                            if (SkillMgrObj.IsWaitActiveSkill())
                            {
                                Interrupt();
                            }
                            else
                            {
                                _postCur += delta;
                                if (_postCur >= _postTime)
                                {
                                    IsUsingActiveSkill = false;
                                    State = FightUnitState.Idle;
                                }
                            }
                        }
                        break;
                }
            }
        }
    }

#endregion
}
