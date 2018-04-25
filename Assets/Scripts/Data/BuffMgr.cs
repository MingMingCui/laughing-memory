using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public enum BuffType
{
    //时效buff
    Time = 1,
    //次数buff
    Times = 2,
    //触发buff
    Trigger = 3,
    //护盾型Buff
    Shield = 4,
}

public enum BuffTrigger
{
    None = 0,
    HpPro = 1,          //生命上限低于某个值
    Kill = 2,           //击杀敌军触发
    PartnerDie = 3,     //友军死亡
    SelfDie = 4,        //自己死亡
    GetHit = 5,         //受到攻击
    UseSkill = 6,       //使用技能
    ShieldBreak = 7,    //护盾爆掉
    Booster = 8,        //濒死，复活
}

public enum ClearBuffType
{
    Both = 0,       //既不可驱散又不可净化
    Dispel = 1,     //可驱散不可净化
    Purge = 2,      //不可驱散可净化
    All = 3,        //所有
}

public enum BuffEffect
{
    None = 0,           //无效果
    AddHPMax = 1,       //增加最大生命值
    AddVigour = 2,      //增加士气
    AddAtk = 3,         //增加物理攻击力
    AddMatk = 4,        //增加策略攻击力
    AddDefRate = 5,     //增加物理抗性
    AddMdefRate = 6,    //增加策略抗性
    AddDefBreak = 7,    //增加物理穿透
    AddMdefBreak = 8,   //增加策略穿透
    AddDeSowRate = 9,   //增加混乱抗性
    AddDeFaintRate = 10,//增加眩晕抗性
    AddDeSpoofRate = 11,//增加伪报抗性
    AddHealRate = 12,   //增加治疗承受
    AddHarmRate = 13,   //增加伤害承受
    AddAsp = 14,        //增加攻击速度
    AddDodgeRate = 15,  //增加闪避率
    AddHitRate = 16,    //增加命中率
    AddBlockRate = 17,  //增加格挡率
    AddRoutRate = 18,   //增加破击率
    AddCritRate = 19,   //增加暴击率
    AddFirmRate = 20,   //增加韧性率
    AddCritInc = 21,    //增加暴击伤害
    LastHarm = 22,      //持续伤害
    LastHeal = 23,      //持续治疗
    Sow = 24,           //混乱
    Faint = 25,         //眩晕
    Spoof = 26,         //伪报
    Sneer = 27,         //嘲讽
    Scared = 28,        //胆裂
    Slient = 29,        //沉默
    AddBuff = 30,       //增加Buff
    UsePromotSkill = 31,//发动瞬发技能
    Invincible = 32,        //免疫
    DeInvincible = 33,   //破免疫
    HpSuck = 34,        //吸血
    UseSkill = 35,      //使用技能
    Betray = 36,        //离间
}

/// <summary>
/// Buff数据
/// </summary>
public class Buff
{
    //uid
    public uint UID = 0;
    //buff id
    public int BuffId = 0;
    //Buff控制器
    public BuffMgr Mgr = null;
    //buff类型
    public BuffType Type = BuffType.Time;
    //持续时间
    public float BuffTime = 0;
    //累积时间
    public float AccTime = 0;
    //剩余次数
    public int UseTimes = 0;
    //次数型buff的CD
    public float BuffCD = 0;
    //次数型buff的累积CD
    public float AccBuffCD = 0;
    //触发类型
    public int Trigger = 0;
    //触发参数
    public float TriggerParam = 0;
    //护盾血量
    public int ShieldHP = 0;
    //buff等级
    public int BuffLevel = 0;
    //效果参数
    public Vector2 EffectParam = Vector2.zero;
    //优先级
    public int Priority = 0;
    //Buff施加者
    public FightUnit Caster = null;
    //技能id
    public int SklId = 0;
    //Buff是否可用
    public bool IsUsing = true;
    //Buff是否已经触发
    public bool IsTriggered = false;
    //Buff描述
    public string Desc = null;

    //创建Buff
    public Buff(BuffMgr mgr, FightUnit caster, int sklLevel, uint uid, int tplId, Vector2 effect)
    {
        JObject cbuff = JsonMgr.GetSingleton().GetBuff(tplId);
        int type = cbuff["type"].ToObject<int>();
        int priority = cbuff["priority"].ToObject<int>();
        this.UID = uid;
        this.BuffId = tplId;
        this.Mgr = mgr;
        this.Priority = priority;
        this.BuffLevel = sklLevel;
        this.AccTime = 0;
        this.BuffTime = cbuff["bufftime"].ToObject<float>();
        this.IsTriggered = false;
        this.Desc = cbuff["desc"].ToObject<string>();
        switch (type)
        {
            case (int)BuffType.Time:
                {
                    this.Type = BuffType.Time;
                }
                break;
            case (int)BuffType.Times:
                {
                    this.Type = BuffType.Times;
                    this.UseTimes = cbuff["usetimes"].ToObject<int>();
                    this.BuffCD = cbuff["buffcd"].ToObject<float>();
                    this.AccBuffCD = 0;
                }
                break;
            case (int)BuffType.Trigger:
                {
                    this.Type = BuffType.Trigger;
                    this.Trigger = cbuff["trigger"].ToObject<int>();
                    this.TriggerParam = cbuff["triggerparam"].ToObject<float>();
                }
                break;
            case (int)BuffType.Shield:
                {
                    this.Type = BuffType.Shield;
                    this.ShieldHP = (int)effect.y;
                }
                break;
            default:
                break;
        }
        this.EffectParam = effect;
        this.Caster = caster;
    }

    public void Update()
    {
        if (this.BuffTime > 0)
        {
            this.AccTime += Time.deltaTime;
            if (AccTime >= this.BuffTime)
            {
                if (this.Mgr != null)
                {
                    Mgr.RemoveBuff(this.UID);
                }
                else
                {
                    EDebug.LogErrorFormat("Buff {0} over, Could not find the mgr..", this.BuffId);
                }
            }
        }

        switch (Type)
        {
            case BuffType.Shield:
                {
                    if (this.ShieldHP <= 0)
                    {
                        
                        if (Mgr != null)
                        {
                            Mgr.OnShieldBreak(this.BuffId);
                            Mgr.RemoveBuff(this.UID);
                        }
                        else
                        {
                            EDebug.LogErrorFormat("Buff {0} over, Could not find the mgr..", this.BuffId);
                        }
                    }
                }
                break;
            case BuffType.Times:
                {
                    this.AccBuffCD += Time.deltaTime;
                    if (this.AccBuffCD >= this.BuffCD)
                    {
                        if (Mgr != null)
                        {
                            Mgr.TakeEffect(this);
                        }
                        else
                        {
                            EDebug.LogErrorFormat("Buff {0} cd over, Could not find the mgr..", this.BuffId);
                        }
                        this.UseTimes--;
                        this.AccBuffCD = 0;
                        if (this.UseTimes <= 0)
                        {
                            if (Mgr != null)
                            {
                                Mgr.RemoveBuff(this.UID);
                            }
                            else
                            {
                                EDebug.LogErrorFormat("Buff {0} over, Could not find the mgr..", this.BuffId);
                            }
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

}

public class BuffMgr{
    //为当前Mgr的每个Buff计算唯一的uid
    private uint _uid = 0;
    public List<Buff> _allBuff = new List<Buff>();
    private FightUnit _unit = null;

    #region Buff临时属性
    //最大生命值直接加在本体，按基础最大生命值
    public float Atk = 0;
    public float Matk = 0;
    public float DefRate = 0;
    public float MdefRate = 0;
    public float DefBreak = 0;
    public float MdefBreak = 0;
    public float DeSowRate = 0;
    public float DeFaintRate = 0;
    public float DeSpoofRate = 0;
    public float DeBetrayRate = 0;
    public float HealRate = 0;
    public float HarmRate = 0;
    public float AspRate = 0;
    public float DodgeRate = 0;
    public float HitRate = 0;
    public float BlockRate = 0;
    public float RoutRate = 0;
    public float CritRate = 0;
    public float FirmRate = 0;
    public float CritInc = 0;
    public float HpSuck = 0;
    //持续伤害、治疗直接处理
    //控制状态直接加在本体
    #endregion

    public BuffMgr(FightUnit unit)
    {
        this._unit = unit;
        ZEventSystem.Register(EventConst.OnFightUnitDie, this, "OnFightUnitDie");
        ZEventSystem.Register(EventConst.OnFightUnitHit, this, "OnFightUnitHit");
        ZEventSystem.Register(EventConst.OnFightUnitHpChange, this, "OnFightUnitHpChange");
        ZEventSystem.Register(EventConst.OnSkillTakeEffect, this, "OnSkillTakeEffect");
        ZEventSystem.Register(EventConst.OnFightUnitBooster, this, "OnFightUnitBooster");
    }

    public void AddBuff(FightUnit caster, int tplId, int buffLevel)
    {
        JObject buffJson = JsonMgr.GetSingleton().GetBuff(tplId);
        if (buffJson == null)
        {
            EDebug.LogErrorFormat("BuffMgr.AddBuff, failed to get buff json, id:{0}", tplId);
            return;
        }

        int type = buffJson["type"].ToObject<int>();
        int layers = buffJson["layers"].ToObject<int>();
        int resid = buffJson["resid"].ToObject<int>();
        bool onfoot = buffJson["onfoot"].ToObject<int>() != 0;

        int calType = buffJson["caltype"].ToObject<int>();
        JArray buffParam = buffJson["buffparam"].ToObject<JArray>();
        int buffPriority = buffJson["priority"].ToObject<int>();
        Vector3 effectParam = AttrUtil.CalExpression(calType, buffParam, caster, this._unit, buffLevel);

        Buff buff = new Buff(this, caster, buffLevel, ++_uid, tplId, effectParam);

        if (checkPriority(buffPriority))
        {

            int curLayer = getCurLayer(tplId);
            if (curLayer >= layers)
                removeFirstBuff(tplId);
            if (0 == getCurLayer(tplId) && resid > 0)
            {
                //通知显示层显示特效
                ZEventSystem.Dispatch(EventConst.OnFightUnitAddBuff, this._unit, resid, onfoot, true);
            }

            if (type == (int)BuffType.Time)
            {
                TakeEffect(buff);
            }
            _allBuff.Add(buff);
        }
    }

    public void RemoveBuff(uint uid)
    {
        int tplid = -1;
		for(int idx = 0; idx < _allBuff.Count; ++idx)
		{
			Buff buf = _allBuff[idx];
			if(buf.UID == uid)
			{
                if (buf.Type == BuffType.Time)
                {
                    TakeEffect(buf, true);
                }
                tplid = buf.BuffId;
                //标记为删除
                buf.IsUsing = false;
				break;
			}
		}
        if (tplid > 0 && 0 == getCurLayer(tplid))
        {
            JObject cbuff = JsonMgr.GetSingleton().GetBuff(tplid);
            int resid = cbuff["resid"].ToObject<int>();
            bool onfoot = cbuff["onfoot"].ToObject<int>() != 0;
            if (resid > 0)
            {
                //通知显示层移除特效
                ZEventSystem.Dispatch(EventConst.OnFightUnitAddBuff, this._unit, resid, onfoot, false);
            }
        }
    }

    public void Update()
    {
        for (int idx = 0; idx < _allBuff.Count; ++idx)
        {
            Buff buff = _allBuff[idx];
            if (!buff.IsUsing)
                continue;
            _allBuff[idx].Update();
        }
        //删除被标记为删除的Buff
        for (int idx = _allBuff.Count - 1; idx >= 0; --idx)
        {
            if (!_allBuff[idx].IsUsing)
            {
                _allBuff.RemoveAt(idx);
            }
        }
    }

    public void TakeEffect(Buff buff, bool inverse = false)
    {
        JObject cbuff = JsonMgr.GetSingleton().GetBuff(buff.BuffId);
        int buffeffect = cbuff["effect"].ToObject<int>();
        int calType = cbuff["caltype"].ToObject<int>();
        string buffparam = cbuff["buffparam"].ToString();

        float effectParamKey = buff.EffectParam.x;
        float effectParamValue = (inverse ? -1 : 1) * buff.EffectParam.y;

        switch (buffeffect)
        {
            case (int)BuffEffect.AddHPMax:
                this._unit.MaxHP += (int)(effectParamValue * this._unit.BaseMaxHp);
                break;
            case (int)BuffEffect.AddVigour:
                {
                    this._unit.AddVigour((int)effectParamValue);
                    if (effectParamValue > 0)
                        ZEventSystem.Dispatch(EventConst.OnFightUnitPop, this._unit, new Vector2Int((int)EffectState.HealVigour, (int)effectParamValue));
                }
                break;
            case (int)BuffEffect.AddAtk:
                this.Atk += effectParamValue;
                break;
            case (int)BuffEffect.AddMatk:
                this.Matk += effectParamValue;
                break;
            case (int)BuffEffect.AddDefRate:
                this.DefRate += effectParamValue;
                break;
            case (int)BuffEffect.AddMdefRate:
                this.MdefRate += effectParamValue;
                break;
            case (int)BuffEffect.AddDefBreak:
                this.DefBreak += effectParamValue;
                break;
            case (int)BuffEffect.AddMdefBreak:
                this.MdefBreak += effectParamValue;
                break;
            case (int)BuffEffect.AddDeSowRate:
                this.DeSowRate += effectParamValue;
                break;
            case (int)BuffEffect.AddDeFaintRate:
                this.DeFaintRate += effectParamValue;
                break;
            case (int)BuffEffect.AddDeSpoofRate:
                this.DeSpoofRate += effectParamValue;
                break;
            case (int)BuffEffect.AddHealRate:
                this.HealRate += effectParamValue;
                break;
            case (int)BuffEffect.AddHarmRate:
                this.HarmRate += effectParamValue;
                break;
            case (int)BuffEffect.AddAsp:
                this.AspRate += effectParamValue;
                break;
            case (int)BuffEffect.AddDodgeRate:
                this.DodgeRate += effectParamValue;
                break;
            case (int)BuffEffect.AddHitRate:
                this.HitRate += effectParamValue;
                break;
            case (int)BuffEffect.AddBlockRate:
                this.BlockRate += effectParamValue;
                break;
            case (int)BuffEffect.AddRoutRate:
                this.RoutRate += effectParamValue;
                break;
            case (int)BuffEffect.AddCritRate:
                this.CritRate += effectParamValue;
                break;
            case (int)BuffEffect.AddFirmRate:
                this.FirmRate += effectParamValue;
                break;
            case (int)BuffEffect.AddCritInc:
                this.CritInc += effectParamValue;
                break;
            case (int)BuffEffect.LastHeal:
                {
                    if (buff.EffectParam.x == 0)
                    {
                        this._unit.HarmHp(new Vector2Int(0, (int)buff.EffectParam.y), buff.Caster, buff.SklId);
                    }
                    else
                    {
                        this._unit.HarmHp(AttrUtil.CalHarm(false, buff.EffectParam.x == 1, buff.EffectParam.y, buff.Caster, this._unit), buff.Caster, buff.SklId);
                    }
                }
                break;
            case (int)BuffEffect.LastHarm:
                {
                    if (buff.EffectParam.x == 0)
                    {
                        this._unit.HarmHp(new Vector2Int(0, -(int)buff.EffectParam.y), buff.Caster, buff.SklId);
                    }
                    else
                    {
                        this._unit.HarmHp(AttrUtil.CalHarm(false, buff.EffectParam.x == 1, -(int)buff.EffectParam.y, buff.Caster, this._unit), buff.Caster, buff.SklId);
                    }
                }
                break;
            case (int)BuffEffect.Sow:
                {
                    this._unit.CState = (inverse ? ControlState.None : ControlState.Sow);
                }
                break;
            case (int)BuffEffect.Faint:
                {
                    this._unit.CState = (inverse ? ControlState.None : ControlState.Faint);
                }
                break;
            case (int)BuffEffect.Spoof:
                {
                    this._unit.CState = (inverse ? ControlState.None : ControlState.Spoof);
                    if (!inverse)
                        this._unit.PathFinderObj.StartFind();
                }
                break;
            case (int)BuffEffect.Sneer:
                {
                    this._unit.CState = (inverse ? ControlState.None : ControlState.Sneer);
                    this._unit.SneerTarget = !inverse ? buff.Caster : null;
                }
                break;
            case (int)BuffEffect.Scared:
                {
                    if (inverse)
                        this._unit.RemoveSpecialState(SpecialState.Scared);
                    else
                        this._unit.AddSpecialState(SpecialState.Scared);
                }
                break;
            case (int)BuffEffect.Betray:
                {
                    this._unit.CState = (inverse ? ControlState.None : ControlState.Betray);
                }
                break;
            case (int)BuffEffect.Slient:
                {
                    if (inverse)
                        this._unit.RemoveSpecialState(SpecialState.Slient);
                    else
                        this._unit.AddSpecialState(SpecialState.Slient);
                    this._unit.Interrupt();
                }
                break;
            case (int)BuffEffect.AddBuff:
                {
                    this.AddBuff(buff.Caster, (int)effectParamValue, buff.BuffLevel);
                }
                break;
            case (int)BuffEffect.UsePromotSkill:
                {
                    if (Random.Range(0, 1.0f) < effectParamKey)
                    {
                        Skill cskill = JsonMgr.GetSingleton().GetSkillByID((int)effectParamValue);
                        if (cskill != null)
                        {
                            this._unit.UseSKill(cskill, buff.BuffLevel);
                        }
                    }
                }
                break;
            case (int)BuffEffect.Invincible:
                this._unit.Invincible = !inverse;
                break;
            case (int)BuffEffect.DeInvincible:
                this._unit.DeInvincible = !inverse;
                break;
            case (int)BuffEffect.HpSuck:
                this.HpSuck += effectParamValue;
                break;
            case (int)BuffEffect.UseSkill:
                {
                    Skill cskill = JsonMgr.GetSingleton().GetSkillByID((int)effectParamValue);
                    if (cskill != null)
                    {
                        this._unit.UseSKill(cskill, buff.BuffLevel);
                    }
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 护盾抵挡伤害
    /// </summary>
    /// <param name="harm"></param>
    /// <returns>返回抵挡了多少点伤害</returns>
    public int ResistHarm(int harm)
    {
        int originalHarm = harm;
        for (int idx = 0; idx < _allBuff.Count; ++idx)
        {
            Buff buff = _allBuff[idx];
            if (!buff.IsUsing || buff.Type != BuffType.Shield)
                continue;
            int resist = Mathf.Min(harm, buff.ShieldHP);
            harm -= resist;
            buff.ShieldHP -= resist;
            if (0 <= harm)
                break;
        }
        return originalHarm - harm;
    }

    /// <summary>
    /// 自己的护盾被打爆
    /// </summary>
    /// <param name="buffid"></param>
    public void OnShieldBreak(int buffid)
    {
        for (int idx = 0; idx < _allBuff.Count; ++idx)
        {
            Buff buff = _allBuff[idx];
            if (!buff.IsUsing)
                continue;
            if (buff.Type == BuffType.Trigger && buff.Trigger == (int)BuffTrigger.ShieldBreak)
            {
                if (buffid == buff.TriggerParam)
                {
                    TakeEffect(buff);
                }
            }
        }
    }

    /// <summary>
    /// 角色死亡，处理依赖caster的buff和buff触发行为
    /// </summary>
    /// <param name="unit"></param>
    public void OnFightUnitDie(FightUnit unit)
    {
        BuffTrigger triggerType = BuffTrigger.None;
        //自己死亡，遗恨
        if (unit == this._unit)
            triggerType = BuffTrigger.SelfDie;

        //友军牺牲，血怒
        if (unit.IsEnemy == this._unit.IsEnemy && unit != this._unit)
            triggerType = BuffTrigger.PartnerDie;

        //敌人是我杀的，破敌
        if (unit.IsEnemy != this._unit.IsEnemy && unit.EnemyUnit == this._unit)
            triggerType = BuffTrigger.Kill;
        for (int idx = 0; idx < _allBuff.Count; ++idx)
        {
            Buff buff = _allBuff[idx];
            if (!buff.IsUsing)
                continue;
            
            //处理触发类型buff
            if (buff.Type == BuffType.Trigger && buff.Trigger == (int)triggerType)
            {
                //参数表示要求造成死亡的技能是指定技能
                if(buff.TriggerParam == 0 || unit.EnemySkill == buff.TriggerParam)
                    TakeEffect(buff);
            }

            JObject cbuff = JsonMgr.GetSingleton().GetBuff(buff.BuffId);
            if (0 == cbuff["needcaster"].ToObject<int>())
                continue;
            //处理依赖caster的BUff
            if (buff.Caster == unit)
                RemoveBuff(buff.UID);
        }
    }

    //自己被攻击，愤怒
    public void OnFightUnitHit(FightUnit unit, Vector2Int data)
    {
        if (unit != this._unit || data.y >= 0)
            return;

        for (int idx = 0; idx < _allBuff.Count; ++idx)
        {
            Buff buff = _allBuff[idx];
            if (!buff.IsUsing)
                continue;
            if (buff.Type == BuffType.Trigger && buff.Trigger == (int)BuffTrigger.GetHit)
            {
                TakeEffect(buff);
            }
        }
    }

    /// <summary>
    /// 当自己使用及技能时，检查有无根据技能id触发的buff
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="cskill"></param>
    public void OnSkillTakeEffect(FightUnit unit, Skill cskill)
    {
        if (cskill == null)
            return;
        if (unit != this._unit)
            return;
        int sklId = cskill.ID;
        for (int idx = 0; idx < _allBuff.Count; ++idx)
        {
            Buff buff = _allBuff[idx];
            if (!buff.IsUsing)
                continue;
            if (buff.Type == BuffType.Trigger && buff.Trigger == (int)BuffTrigger.UseSkill)
            {
                if (sklId ==  (int)buff.TriggerParam)
                {
                    TakeEffect(buff);
                }
            }
        }
    }

    //自己当前血量/最大血量变化
    public void OnFightUnitHpChange(FightUnit unit)
    {
        if (unit != this._unit)
            return;
        float hpPro = (float)unit.CurHP / unit.MaxHP;
        for (int idx = 0; idx < _allBuff.Count; ++idx)
        {
            Buff buff = _allBuff[idx];
            if (!buff.IsUsing)
                continue;
            if (buff.Type == BuffType.Trigger && buff.Trigger == (int)BuffTrigger.HpPro)
            {
                bool shouldTrigger = hpPro < buff.TriggerParam;
                if (buff.IsTriggered != shouldTrigger)
                {
                    TakeEffect(buff, !shouldTrigger);
                    buff.IsTriggered = shouldTrigger;
                }
            }
        }
    }

    public void OnFightUnitBooster(FightUnit unit)
    {
        if (unit != this._unit)
            return;
        for (int idx = 0; idx < _allBuff.Count; ++idx)
        {
            Buff buff = _allBuff[idx];
            if (!buff.IsUsing)
                continue;
            if (buff.Type == BuffType.Trigger && buff.Trigger == (int)BuffTrigger.Booster)
            {
                TakeEffect(buff);
            }
        }
    }

    public void ClearBuff(ClearBuffType type)
    {
        for (int idx = 0; idx < _allBuff.Count; ++idx)
        {
            Buff buff = _allBuff[idx];
            JObject cbuff = JsonMgr.GetSingleton().GetBuff(buff.BuffId);
            int dispelType = cbuff["dispel"].ToObject<int>();

            //所有Buff都能清空
            if (type == ClearBuffType.All)
                RemoveBuff(buff.UID);
            else
            {
                //不能驱散不能净化
                if (0 == dispelType)
                    continue;
                if (dispelType == (int)type || type == ClearBuffType.Both)
                    RemoveBuff(buff.UID);
            }
        }
    }

    public void Clear()
    {
        _allBuff.Clear();
        ZEventSystem.DeRegister(EventConst.OnFightUnitDie, this);
        ZEventSystem.DeRegister(EventConst.OnFightUnitHit, this);
        ZEventSystem.DeRegister(EventConst.OnFightUnitHpChange, this);
        ZEventSystem.DeRegister(EventConst.OnSkillTakeEffect, this);
        ZEventSystem.DeRegister(EventConst.OnFightUnitBooster, this);
    }
    /// <summary>
    /// 检查Buff优先级
    /// </summary>
    /// <param name="priority"></param>
    public bool checkPriority(int priority)
    {
        if (0 == priority)
            return true;
        for (int idx = 0; idx < _allBuff.Count; ++idx)
        {
            Buff buff = _allBuff[idx];
            if (!buff.IsUsing || 0 == buff.Priority)
                continue;
            if (priority > 0)
            {
                if (buff.Priority > 0)
                {
                    if (buff.Priority <= priority)
                    {
                        RemoveBuff(buff.UID);
                    }
                    else
                    {
                        //已存在的buff比要比较的优先级更高
                        return false;
                    }
                }
            }
            else
            {
                if (buff.Priority == priority)
                    RemoveBuff(buff.UID);
            }
        }
        return true;
    }
    private int getCurLayer(int tplid)
    {
        int layerCnt = 0;
        for (int idx = 0; idx < _allBuff.Count; ++idx)
        {
            Buff buff = _allBuff[idx];
            if (!buff.IsUsing)
                continue;
            if (_allBuff[idx].BuffId == tplid)
                layerCnt++;
        }
        return layerCnt;
    }

    private void removeFirstBuff(int tplid)
    {
        uint uid = 0;
        for (int idx = 0; idx < _allBuff.Count; ++idx)
        {
            Buff buff = _allBuff[idx];
            if (!buff.IsUsing || buff.BuffId != tplid)
                continue;
            if (0 == uid || buff.UID < uid)
                uid = buff.UID;
        }
        if (uid > 0)
            RemoveBuff(uid);
    }
}
