using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class FightUnitView : MonoBehaviour {

    public FightUnit unit = null;
    public GameObject Horse = null;
    private Animator HeroAnim = null;
    private Animator HorseAnim = null;

    private int _resId = 0;

    private bool _hasInited = false;

    private bool _pause = false;

    private bool _isActiveSkillPaused = false;
    private List<GameObject> _activeSkillPausedEffect = new List<GameObject>();

    private int _lastStartEffect = 0;

    public static readonly int MOVE_EFFECT = 10513;
    public bool Pause
    {
        get { return _pause; }
        set
        {
            if (_pause != value)
            {
                _pause = value;
                if (HeroAnim != null)
                    HeroAnim.speed = _pause ? 0 : 1;
                if (HorseAnim != null)
                    HorseAnim.speed = _pause ? 0 : 1;
            }
        }
    }

    public void Init(int resid)
    {
        _hasInited = true;
        _resId = resid;
        _initEvent(true);
        HeroAnim = this.GetComponent<Animator>();
        if(Horse != null)
            HorseAnim = Horse.GetComponent<Animator>();
    }

    private void _destroy()
    {
        _initEvent(false);
        this.gameObject.SetActive(false);
        if (this.Horse != null)
            this.Horse.SetActive(false);
        this.unit = null;
        _activeSkillPausedEffect.Clear();
    }

    private void _initEvent(bool regist)
    {
        if (regist)
        {
            ZEventSystem.Register(EventConst.OnFightUnitMove, this, "OnFightUnitMove");
            ZEventSystem.Register(EventConst.OnFightUnitSkill, this, "OnFightUnitSkill");
            ZEventSystem.Register(EventConst.OnFightUnitDie, this, "OnFightUnitDie");
            ZEventSystem.Register(EventConst.OnPlayHitAnim, this, "OnPlayHitAnim");
            ZEventSystem.Register(EventConst.OnFightOver, this, "OnFightOver");
            ZEventSystem.Register(EventConst.OnFightUnitTakenEffect, this, "OnFightUnitTakenEffect");
            ZEventSystem.Register(EventConst.OnFightUnitAddBuff, this, "OnFightUnitAddBuff");
            ZEventSystem.Register(EventConst.OnRequestUnitPause, this, "OnRequestUnitPause");
            ZEventSystem.Register(EventConst.ForceDestroyView, this, "ForceDestroyView");
            ZEventSystem.Register(EventConst.OnFightUnitInterrupt, this, "OnFightUnitInterrupt");
            ZEventSystem.Register(EventConst.OnFightUnitTakeEffect, this, "OnFightUnitTakeEffect");
        }
        else
        {
            ZEventSystem.DeRegister(EventConst.OnFightUnitMove, this);
            ZEventSystem.DeRegister(EventConst.OnFightUnitSkill, this);
            ZEventSystem.DeRegister(EventConst.OnFightUnitDie, this);
            ZEventSystem.DeRegister(EventConst.OnPlayHitAnim, this);
            ZEventSystem.DeRegister(EventConst.OnFightOver, this);
            ZEventSystem.DeRegister(EventConst.OnFightUnitTakenEffect, this);
            ZEventSystem.DeRegister(EventConst.OnFightUnitAddBuff, this);
            ZEventSystem.DeRegister(EventConst.OnRequestUnitPause, this);
            ZEventSystem.DeRegister(EventConst.ForceDestroyView, this);
            ZEventSystem.DeRegister(EventConst.OnFightUnitInterrupt, this);
            ZEventSystem.DeRegister(EventConst.OnFightUnitTakeEffect, this);
        }
    }

    public void OnFightUnitMove(FightUnit unit, bool start)
    {
        if (unit != this.unit)
            return;
        this.HeroAnim.SetBool("Move", start);
        if (this.HorseAnim != null)
            this.HorseAnim.SetBool("Move", start);
        if (start)
        {
            GameObject go = EffectMgr.Instance.CreateEffect(this.unit, MOVE_EFFECT, Vector3.zero, Quaternion.identity, true, this.transform);
            _checkFightUnitEffect(go, -1);
        }
        else
            EffectMgr.Instance.RemoveEffect(this.unit, MOVE_EFFECT);
    }

    /// <summary>
    /// 技能效果被生效，包括普通技能生效和子弹生效
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="ceffect"></param>
    public void OnFightUnitTakenEffect(FightUnit unit, JObject ceffect)
    {
        if (unit != this.unit)
            return;
        int hiteffect = ceffect["hiteffect"].ToObject<int>();
        if (hiteffect != 0)
        {
            GameObject effect = EffectMgr.Instance.CreateEffect(this.unit, hiteffect, Vector3.zero, Quaternion.identity, false, this.transform);
            _checkFightUnitEffect(effect, -1);
        }
    }

    /// <summary>
    /// 技能效果生效，主动方
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="cskill"></param>
    public void OnFightUnitTakeEffect(FightUnit unit, Skill cskill)
    {
        if (this.unit != unit || cskill == null)
            return;
        int castEffect = cskill.casteffect;
        Quaternion lookRot = Quaternion.LookRotation(this.unit.IsEnemy ? -Vector3.forward : Vector3.forward);
        if (castEffect != 0)
        {
            GameObject effect = EffectMgr.Instance.CreateFlyEffect(this.unit, castEffect, lookRot, MapMgr.Instance.GetMapPos(), cskill.casttime);
            _checkFightUnitEffect(effect, cskill.type);
        }
    }

    //进出大招前摇
    public void OnRequestUnitPause(FightUnit unit, bool start)
    {
        if (this.unit != unit)
            return;
        _isActiveSkillPaused = start;
        _setActiveSkillPauseLayer(this.gameObject, start);

        if (!start)
        {
            for (int idx = 0; idx < _activeSkillPausedEffect.Count; ++idx)
            {
                _setActiveSkillPauseLayer(_activeSkillPausedEffect[idx], start);
            }
            _activeSkillPausedEffect.Clear();
        }
    }

    public void OnPlayHitAnim(FightUnit unit)
    {
        if (unit != this.unit)
            return;
        this.HeroAnim.Play("Hit", 0, 0);
        stopStartEffect();
        if (this.HorseAnim != null)
            this.HorseAnim.Play("Hit", 0, 0);
    }

    public void OnFightOver(bool win)
    {
        if (this.unit.IsDead || this.unit.IsEnemy == win)
            return;
        if(this.HeroAnim != null)
            this.HeroAnim.Play("Win", 0, 0);
        if (this.HorseAnim != null)
            this.HorseAnim.Play("Win", 0, 0);
    }

    public void OnFightUnitSkill(FightUnit unit, Skill cskill, float skillSpeed, FightUnit target)
    {
        if (unit != this.unit)
            return;
        int type = cskill.type;
        int starteffect = cskill.starteffect;
        int fullEffect = cskill.fulleffect;
        int attackEffect = cskill.attackeffect;
        int castEffect = cskill.casteffect;
        string AnimStateName = "";
        int soundoffset = 0;
        switch (type)
        {
            case (int)SkillType.ACTIVE:
                AnimStateName = "Skill2";
                soundoffset = 3;
                break;
            case (int)SkillType.AUTO:
                AnimStateName = "Skill";
                soundoffset = 2;
                break;
            case (int)SkillType.NORMAL:
                {
                    AnimStateName = "Attack";
                    soundoffset = 1;
                    HeroAnim.SetFloat("Asp", 1 / skillSpeed);
                    if (HorseAnim != null)
                        HorseAnim.SetFloat("Asp", 1 / skillSpeed);
                }
                break;
            case (int)SkillType.SPECIAL:
                AnimStateName = "Special";
                break;
            default:
                break;
        }


        if (AnimStateName != "")
        {
            HeroAnim.Play(AnimStateName, 0, 0);
            if(soundoffset > 0)
                SoundMgr.Instance.PlaySound(this._resId * 10 + soundoffset);
            if (HorseAnim != null)
            {
                HorseAnim.Play(AnimStateName, 0, 0);
            }
        }
        
        if (starteffect != 0)
        {
            GameObject effect = EffectMgr.Instance.CreateEffect(this.unit, starteffect, Vector3.zero, Quaternion.identity, false, this.transform, 1 / skillSpeed);
            _lastStartEffect = starteffect;
            _checkFightUnitEffect(effect, type);
        }

        Quaternion lookRot = Quaternion.LookRotation(this.unit.IsEnemy ? -Vector3.forward : Vector3.forward);

        //处理全屏/打击特效
        if (fullEffect != 0)
        {
            GameObject effect = EffectMgr.Instance.CreateEffect(this.unit, fullEffect, MapMgr.Instance.GetMapPos(), lookRot, false);
            _checkFightUnitEffect(effect, type);
        }
        else if (attackEffect != 0 && target != null)
        {
            //攻击特效
            GameObject effect = EffectMgr.Instance.CreateEffect(this.unit, attackEffect, new Vector3(target.CurPos.x, 0.1f, target.CurPos.y),
                lookRot, false,  null, 1 / skillSpeed);
            _checkFightUnitEffect(effect, type);
        }
    }


    public void OnFightUnitAddBuff(FightUnit unit, int resid, bool onfoot, bool addOrRemove)
    {
        if (unit != this.unit)
            return;
        if (addOrRemove)
        {
            int horseId = !unit.IsMonster ? JsonMgr.GetSingleton().GetHeroByID(unit.HeroId).horseid : JsonMgr.GetSingleton().GetMonsterByID(unit.HeroId).horseid;

            GameObject effect = EffectMgr.Instance.CreateEffect(this.unit, resid, new Vector3(0, onfoot ? 0 : (horseId != 0 ? 0.6f : -0.3f), 0), Quaternion.identity, true, this.transform);
            _checkFightUnitEffect(effect, -1);
        }
        else
            EffectMgr.Instance.RemoveEffect(this.unit, resid);
    }

    public void OnFightUnitDie(FightUnit unit)
    {
        if (unit != this.unit)
            return;
        this.HeroAnim.Play("Die", 0, 0);
        stopStartEffect();
        SoundMgr.Instance.PlaySound(this.unit.HeroId * 10 + 5);
        if (this.HorseAnim != null)
            this.HorseAnim.Play("Die", 0, 0);
    }

    public void OnFightUnitInterrupt(FightUnit unit)
    {
        if (unit != this.unit)
            return;
        stopStartEffect();
        this.HeroAnim.Play("Idle", 0, 0);
        if (this.HorseAnim != null)
            this.HorseAnim.Play("Idle", 0, 0);
    }

    public void stopStartEffect()
    {
        if (_lastStartEffect > 0)
            EffectMgr.Instance.RemoveEffect(this.unit, _lastStartEffect);
    }

    public void ForceDestroyView(FightUnit unit)
    {
        if (this.unit != unit)
            return;
        _destroy();
    }

    // Update is called once per frame
    void Update () {
        if (unit != null && _hasInited)
        {
            this.Pause = FightLogic.Instance.UnitPause && !this.unit.IsUsingActiveSkill;
            this.transform.position = new Vector3(this.unit.CurPos.x, 0.1f, unit.CurPos.y);
            this.transform.rotation = this.unit.CurRot;
            Vector2 targetPos = PathFinder.Grid2Pos(this.unit.GridPos);
            if (this.unit.DieAccTime >= this.unit.DieLast)
            {
                _destroy();
            }
        }
    }

    private void _checkFightUnitEffect(GameObject effect, int type)
    {
        if (effect != null && type == (int)SkillType.ACTIVE)
        {
            _activeSkillPausedEffect.Add(effect);
        }
        _setActiveSkillPauseLayer(effect, _isActiveSkillPaused);
    }

    private void _setActiveSkillPauseLayer(GameObject go, bool activeSkillLayer)
    {
        int layer = activeSkillLayer ? LayerMask.NameToLayer("ActiveSkill") : 0;
        go.layer = layer;
        foreach (Transform t in go.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = layer;
        }
    }
}
