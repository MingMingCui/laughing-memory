using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class SkillMgr{

    //FightUnit
    private FightUnit _unit = null;
    //技能列表，技能->等级
    private Dictionary<int, int> _skillData = null;
    //所有技能
    private int[] _allSkills = null;
    //初始技能顺序
    private int[] _initSkillTurn = null;
    //技能顺序
    private int[] _skillTurn = null;
    //技能游标
    private int _skillFlag = 0;
    //初始技能是否放完
    private bool _skillInited = false;
    //主动技能，最多一个
    private int _activeSkill = 0;
    //普攻技能
    private int _normalSkill = 0;
    //主动技能目标
    private int _activeSkillType = -1;
    //等待主动技能，是否在等待主动技能
    private bool _waitActiveSkill = false;

    public SkillMgr(FightUnit unit, Dictionary<int, int> skillData, int[] skills, int[] initSkillTurn, int[] skillTurn, int activeSkill, int normalSkill)
    {
        this._unit = unit;
        this._skillData = skillData;
        this._allSkills = skills;
        this._initSkillTurn = initSkillTurn;
        this._skillTurn = skillTurn;
        this._skillFlag = 0;
        this._skillInited = false;
        this._activeSkill = activeSkill;
        this._normalSkill = normalSkill;
        if (this._activeSkill > 0)
        {
            Skill cskill = JsonMgr.GetSingleton().GetSkillByID(activeSkill);
            if (cskill != null)
                this._activeSkillType = cskill.target;
        }
        this._waitActiveSkill = false;
    }

    /// <summary>
    /// 按照技能释放顺序选择当前可以释放的技能
    /// </summary>
    /// <returns></returns>
    public Vector2Int SelectSkill()
    {
        if (_noSkill())
        {
            if (_normalSkill != 0)
                return new Vector2Int(_normalSkill, _skillData[_normalSkill]);
            else
                return Vector2Int.zero;
        }
        if (_activeSkill != 0 && _waitActiveSkill)
        {
            if (!_skillData.ContainsKey(_activeSkill))
            {
                EDebug.LogErrorFormat("SelectSkill failed, ActiveSkill:{0} not in skilldata", _activeSkill);
                return Vector2Int.zero;
            }
            return new Vector2Int(_activeSkill, _skillData[_activeSkill]);
        }
        int sel = !_skillInited ? _initSkillTurn[_skillFlag] : _skillTurn[_skillFlag];
        int skillId = _allSkills[sel];

        if (_skillData.ContainsKey(skillId))
            return new Vector2Int(skillId, _skillData[skillId]);
        else
        {
            EDebug.LogErrorFormat("SelectSkill failed, skillid:{0} not in skilldata", skillId);
            return Vector2Int.zero;
        }
    }

    /// <summary>
    /// 更新技能游标，一定要在UseSkill的时候再调用，确保已经走到释放技能逻辑了
    /// </summary>
    public void UpdateSkillFlag()
    {
        if (_noSkill())
            return;

        _skillFlag++;
        if (!_skillInited)
        {
            if (_skillFlag >= _initSkillTurn.Length)
            {
                _skillInited = true;
                _skillFlag = 0;
            }
        }
        else
        {
            if (_skillFlag >= _skillTurn.Length)
                _skillFlag = 0;
        }
    }

    public bool IsWaitActiveSkill()
    {
        return _waitActiveSkill;
    }

    public void UseActiveSkill()
    {
        _waitActiveSkill = true;
    }

    public void ClearActiveSkill()
    {
        _waitActiveSkill = false;
    }

    public bool IsActiveSkillEnable(FightUnit unit)
    {
        return (!unit.IsDead) && (FightLogic.Instance.State == FightState.Fight) && (unit.Vigour >= unit.MaxVigour) && 
            (_activeSkill != 0) && ((_activeSkillType != (int)SkillTarget.CUR) || (unit.FightTarget != null && !unit.FightTarget.IsDead)) 
            && !_noSkill() && !unit.SkillMgrObj.IsWaitActiveSkill() && !unit.IsActiveSkillCradle;
    }

    public Dictionary<int, int> GetSkillData()
    {
        return this._skillData;
    }

    public void Clear()
    {
        _skillFlag = 0;
        _skillInited = false;
    }

    private bool _noSkill()
    {
        return _unit.GetSpecialState(SpecialState.Slient) || _unit.CState != ControlState.None;
    }
}
