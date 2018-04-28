using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using JsonData;

public enum FightState
{
    None = -1,
	Init = 0,
	Prepare = 1,
	Fight = 2,
    Continue = 3,
	Over = 4,
}

public class FightLogic : Singleton<FightLogic>, IUpdate {
    public FightLogic()
    {
        this.RoundTime = JsonMgr.GetSingleton().GetGlobalIntArrayByID(1001).value;
    }

    //指定UID
    uint UID = 0;
    //战斗状态
    private FightState _fightState = FightState.None;
    public FightState State
    {
        get { return _fightState; }
        set
        {
            if (_fightState != value)
            {
                _fightState = value;
                ZEventSystem.Dispatch(EventConst.OnFightStateChange, _fightState);
                if (_fightState == FightState.Over)
                {
                    for (int idx = 0; idx < AllFighters.Count; ++idx)
                    {
                        FightUnit unit = AllFighters[idx];
                        if (unit.IsDead)
                            continue;
                        unit.PathFinderObj.StopFind();
                    }
                    ZEventSystem.Dispatch(EventConst.OnFightOver, HasWin);
                }
            }
        }
    }
    //己方战斗单位
    public List<FightUnit> Fighters = new List<FightUnit>();
    //敌方战斗单位
    public List<List<FightUnit>> EnemyFighters = new List<List<FightUnit>>();
    //当前所有战斗单位
    public List<FightUnit> AllFighters = new List<FightUnit>();
    //战斗总回合数
    public int TotalRound = 0;
    //当前回合数
    public int CurRound = 0;
    //战斗是pvp还是pve
    public bool IsPvp = false;
    //宝箱数量
    public int Treasures = 0;
    //是否已经胜利
    public bool HasWin = false;
    //A Star
    public AStarFinder aStarFinder = null;
    //子弹管理
    public BulletMgr BulletMgrObj = new BulletMgr();
    //CamMgr
    public CamMgr CamMgrObj = null;
    //DropMgr
    public DropMgr DropMgrObj = null;
    //是否自动战斗
    private bool _isAutoFight = false;
    public bool IsAutoFight
    {
        get { return _isAutoFight; }
        set {
            if (AllowChangeAutoFight && this._isAutoFight != value)
            {
                this._isAutoFight = value;
                ZEventSystem.Dispatch(EventConst.OnAutoFightStateChange, value);
            }
        }
    }
    //是否允许切换自动战斗状态
    private bool _allowChangeAutoFight = true;
    public bool AllowChangeAutoFight
    {
        get { return _allowChangeAutoFight; }
        set {
            if (this._allowChangeAutoFight != value)
            {
                this._allowChangeAutoFight = value;
            }
        }
    }

    //加速功能
    public bool IsSpeedUp
    {
        get { return Time.timeScale != 1; }
        set
        {
            if (IsSpeedUp != value)
            {
                Time.timeScale = value ? 2f : 1;
                ZEventSystem.Dispatch(EventConst.OnGameSpeedChange, value);
            }
        }
    }

    //FightUnit暂停
    private bool _unitPause = false;
    public bool UnitPause
    {
        get { return _unitPause; }
        set
        {
            if (_unitPause != value)
            {
                _unitPause = value;
                ZEventSystem.Dispatch(EventConst.OnUnitPause, _unitPause);
            }
        }
    }

    //战斗暂停，影响战斗时间、FightUnit用技能以及Buff生效
    public bool FightPause = false;

    //比赛回合总时间
    public float RoundTime = 0;
    //比赛回合累计时间
    public float AccRoundTime = 0;

    /// <summary>
    /// 为战斗单位创建VIew
    /// </summary>
    /// <param name="unit"></param>
    private void _createFightUnitView(FightUnit unit)
    {
        int resId = 0;
        int horseId = 0;
        if (!unit.IsMonster)
        {
            Hero chero = JsonMgr.GetSingleton().GetHeroByID(unit.HeroId);
            resId = chero.resid;
            horseId = chero.horseid;

        }
        else
        {
            Monster chero = JsonMgr.GetSingleton().GetMonsterByID(unit.HeroId);
            resId = chero.resid;
            horseId = chero.horseid;
        }
        
        GameObject fighterObj = GameObject.Instantiate(ResourceMgr.Instance.LoadResource(resId) as GameObject);
        GameObject horseObj = null;
        if (horseId > 0)
        {
            horseObj = GameObject.Instantiate(ResourceMgr.Instance.LoadResource(horseId) as GameObject);
            horseObj.transform.parent = fighterObj.transform;
            horseObj.transform.localPosition = Vector3.zero;
        }
        FightUnitView fightUnitView = fighterObj.AddComponent<FightUnitView>();
        fightUnitView.unit = unit;
        fightUnitView.Horse = horseObj;
        fightUnitView.Init(resId);
    }

    public void SetFightUnit(List<FightUnit> fighters = null, List<List<FightUnit>> enemyFighters = null)
    {
        this.Fighters = fighters;
        this.EnemyFighters = enemyFighters;
    }


    /// <summary>
    /// 开始一场战斗
    /// </summary>
    /// <param name="isPvp">比赛性质，pve还是pvp</param>
    /// <param name="treasures">宝箱数量</param>
    public void CreateFight(bool isPvp, int treasures = 0)
	{
		this.State = FightState.Init;
		if(this.Fighters == null)
		{
			EDebug.LogError("FightLogic.CreateFight failed, fighters is null");
			return;
		}
		if(this.EnemyFighters == null)
		{
			EDebug.LogError("FightLogic.CreateFight failed, fighters is null");
			return;
		}
        if (this.Fighters.Count == 0)
        {
            EDebug.LogError("FightLogic.CreateFight failed, fighters is empty");
            return;
        }
        if (this.EnemyFighters.Count == 0)
        {
            EDebug.LogError("FightLogic.CreateFight failed, enemyFighters is empty");
            return;
        }
        
        this.TotalRound = this.EnemyFighters.Count;
        this.CurRound = 0;
        this.IsPvp = isPvp;
        this.Treasures = treasures;
        this.DropMgrObj = new DropMgr();
        aStarFinder = new AStarFinder(PathFinder.V_GRID, (this.TotalRound * 3 + 1) * PathFinder.H_GRID);

        List<FightUnit> allUnit = new List<FightUnit>();
        for (int idx = 0; idx < Fighters.Count; ++idx)
        {
            allUnit.Add(Fighters[idx]);
        }
        for (int idx = 0; idx < EnemyFighters.Count; ++idx)
        {
            for (int idx2 = 0; idx2 < EnemyFighters[idx].Count; ++idx2)
            {
                allUnit.Add(EnemyFighters[idx][idx2]);
            }
        }

        for (int idx = 0; idx < allUnit.Count; ++idx)
        {
            allUnit[idx].UID = ++UID;
        }
        ZEventSystem.Dispatch(EventConst.OnCreateFight, allUnit);

        //Test~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        for (int idx = 0; idx < Fighters.Count; ++idx)
        {
            FightUnit unit = Fighters[idx];
            _createFightUnitView(unit);
        }

        MapMgr.Instance.CreateFightMap(1, TotalRound);
        CamMgrObj = GameObject.Find("Main Camera").GetComponent<CamMgr>();

        //EndTest~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //注册事件
        ZEventSystem.Register(EventConst.OnFightUnitDie, this, "OnUnitDie");
        ZEventSystem.Register(EventConst.OnFightStateChange, this, "OnFightStateChange");
        ZEventSystem.Register(EventConst.OnRequestUnitPause, this, "OnRequestUnitPause");
        ZEventSystem.Register(EventConst.OnFightMaskOver, this, "OnFightMaskOver");
        ZEventSystem.Register(EventConst.OnGamePause, this, "OnGamePause");

        //CamMgrObj.StartDissolve();
        CamMgrObj.PlayStartEffect();
        NextRound();
        ProcessCtrl.Instance.AddUpdate(this);
        
    }

    /// <summary>
    /// 选择距离最近的地方目标
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="isEnemy"></param>
    /// <returns></returns>
    //public FightUnit SelectNearestEnemy(Vector2 pos, bool isEnemy)
    //{
    //    FightUnit ret = null;
    //    float dis = -1;
    //    for (int idx = 0; idx < AllFighters.Count; ++idx)
    //    {
    //        FightUnit unit = AllFighters[idx];
    //        if (unit.IsDead || (unit.IsEnemy == isEnemy && unit.CState != ControlState.Sow))
    //            continue;
    //        float sqrtMagnitude = (unit.CurPos - pos).sqrMagnitude;
    //        if (0 == sqrtMagnitude)
    //            continue;
    //        if (dis < 0 || sqrtMagnitude < dis)
    //        {
    //            ret = unit;
    //            dis = sqrtMagnitude;
    //        }
    //    }
    //    return ret;
    //}

    private List<FightUnit> _randomFightUnitBuffer = new List<FightUnit>();
    public FightUnit SelectRandom(Vector2 pos, bool isEnemy, float dis = 0)
    {
        _randomFightUnitBuffer.Clear();
        for (int idx = 0; idx < AllFighters.Count; ++idx)
        {
            FightUnit unit = AllFighters[idx];
            if (unit.IsDead)
                continue;
            if (unit.CState != ControlState.Sow)
            {
                if (unit.CState == ControlState.Betray)
                {
                    if (unit.IsEnemy != isEnemy)
                        continue;
                }
                else
                {
                    if (unit.IsEnemy == isEnemy)
                        continue;
                }
            }
            if (dis > 0)
            {
                float sqrtMagnitude = (unit.CurPos - pos).sqrMagnitude;
                if (0 == sqrtMagnitude || sqrtMagnitude > Mathf.Pow(dis, 2))
                    continue;
            }
            _randomFightUnitBuffer.Add(unit);
        }
        if (_randomFightUnitBuffer.Count > 0)
            return _randomFightUnitBuffer[Random.Range(0, _randomFightUnitBuffer.Count)];
        return null;
    }

    public void OnGamePause(bool pause)
    {
        CamMgrObj.OnGamePause(pause);
    }

    /// <summary>
    /// 有战斗单位死亡
    /// </summary>
    public void OnUnitDie(FightUnit unit)
    {
        if (unit == null)
        {
            EDebug.LogError("FightLogic.OnUnitDie, unit is null");
            return;
        }

        bool isEnemyDie = unit.IsEnemy;

        List<FightUnit> unitList = isEnemyDie ? EnemyFighters[CurRound - 1] : Fighters;
        bool allUnitDie = true;
        for (int idx = 0; idx < unitList.Count; ++idx)
        {
            if (!unitList[idx].IsDead)
            {
                allUnitDie = false;
                break;
            }
        }
        if (allUnitDie)
        {
            for (int idx = 0; idx < AllFighters.Count; ++idx)
            {
                FightUnit fightUnit = AllFighters[idx];
                if (fightUnit.IsSummon)
                {
                    ZEventSystem.Dispatch(EventConst.ForceDestroyView, fightUnit);
                }
                fightUnit.SystemProtect = true;
            }
            if (isEnemyDie)
            {
                //敌人死光了
                for (int idx = 0; idx < AllFighters.Count; ++idx)
                {
                    FightUnit u = AllFighters[idx];
                    if (u.IsDead)
                        continue;
                }
                if (CurRound < TotalRound)
                {
                    State = FightState.Continue;
                }
                else
                {
                    //战斗胜利
                    _terminate(true);
                }
            }
            else
            {
                //自己死光了
                _terminate(false);
            }
        }

        //宝箱掉落
        if (Treasures > 0)
        {
            if (isEnemyDie)
            {
                if (allUnitDie && CurRound >= TotalRound)
                {
                    this.DropMgrObj.CreateTreasure(unit.CurPos, Treasures);
                    Treasures = 0;
                }
                else
                {
                    if (Random.Range(0, 1.0f) < this.DropMgrObj.TreasureRate)
                    {
                        this.DropMgrObj.CreateTreasure(unit.CurPos, 1);
                        --Treasures;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 战斗状态改变
    /// </summary>
    /// <param name="state"></param>
    public void OnFightStateChange(FightState state)
    {
        switch (state)
        {
            case FightState.Init:
                break;
            case FightState.Prepare:
                //设置所有单位初始站位，目标站位
                for (int idx = 0; idx < AllFighters.Count; ++idx)
                {
                    AllFighters[idx].SystemProtect = false;
                    AllFighters[idx].InitPassiveSkill();
                }
                ZEventSystem.Register(EventConst.OnUnitMoveOver, this, "OnUnitMoveOver");
                EnterBattileField();
                break;
            case FightState.Fight:
                EDebug.Log("战斗开始");
                CamMgrObj.ChangeCam(true);
                //使用被动技能
                for (int idx = 0; idx < AllFighters.Count; ++idx)
                {
                    FightUnit u = AllFighters[idx];
                    if (u.IsDead)
                        continue;
                    AllFighters[idx].FightIntervalAcc = 0;
                    AllFighters[idx].FightInterval = 0;
                }
                break;
            case FightState.Continue:
                CamMgrObj.ChangeCam(false);

                clearFightState(false);

                for (int idx = 0; idx < Fighters.Count; ++idx)
                {
                    FightUnit u = Fighters[idx];
                    if (u.IsDead)
                        continue;
                    u.RoundOver();
                    u.NewRound();
                }
                ExitBattleField();
                break;
            case FightState.Over:
                EDebug.Log("战斗结束, 是否胜利:" + HasWin);
                clearFightState(true);
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// 进入战场
    /// </summary>
    public void EnterBattileField()
    {
        for (int idx = 0; idx < AllFighters.Count; ++idx)
        {
            FightUnit unit = AllFighters[idx];
            if (unit.IsDead)
                continue;
            unit.InitFightPos(CurRound);
            int fightPos = unit.GridPos + (unit.IsEnemy ? -1 : 1) * PathFinder.AREA_TOTAL;
            //unit.PathFinderObj.StartFind(fightPos);
            unit.GridPos = fightPos;
            unit.TargetPos = PathFinder.Grid2Pos(fightPos);
            unit.State = FightUnitState.Move;
        }
    }

    /// <summary>
    /// 退出战场
    /// </summary>
    public void ExitBattleField()
    {
        int lastPos = -1;
        for (int idx = 0; idx < AllFighters.Count; ++idx)
        {
            FightUnit unit = AllFighters[idx];
            if (unit.IsDead)
                continue;
            if (lastPos < 0 || unit.GridPos < lastPos)
                lastPos = unit.GridPos;
        }
        if (lastPos < 0)
        {
            EDebug.LogError("FightLogic.ExitBattleField failed, lastPos < 0");
            return;
        }
        int gridOffset = (lastPos - ((CurRound - 1) * 3 + 1) * PathFinder.AREA_TOTAL) / PathFinder.V_GRID;
        gridOffset = PathFinder.H_GRID * 2 - gridOffset;

        if (gridOffset < 0)
        {
            EDebug.LogErrorFormat("FightLogic.ExitBattleFIeld failed, lastpos:{0} gridOffset:{1}", lastPos, gridOffset);
        }

        for (int idx = 0; idx < AllFighters.Count; ++idx)
        {
            FightUnit unit = AllFighters[idx];
            if (unit.IsDead)
                continue;
            int endPos = unit.GridPos + gridOffset * PathFinder.V_GRID;
            //unit.PathFinderObj.StartFind(endPos);
            unit.GridPos = endPos;
            unit.TargetPos = PathFinder.Grid2Pos(endPos);
            unit.CurRot.SetLookRotation(Vector3.right);
            unit.State = FightUnitState.Move;
        }

    }

    /// <summary>
    /// 召唤怪物
    /// </summary>
    /// <param name="monsterId"></param>
    /// <param name="isEnemy"></param>
    /// <param name="ambush"></param>
    public void Summon(int monsterId, int level, bool isEnemy, int num, bool ambush = false)
    {
        if (num <= 0)
        {
            EDebug.LogErrorFormat("FightLogic.Summon failed, invalid monster num : {0}", num);
            return;
        }
        monsterId += (level - 1);
        for (int idx = AllFighters.Count - 1; idx > 0; --idx)
        {
            FightUnit unit = AllFighters[idx];
            if (unit.IsDead || !unit.IsSummon)
                continue;
            if (unit.HeroId == monsterId)
            {
                ZEventSystem.Dispatch(EventConst.ForceDestroyView, unit);
                AllFighters.RemoveAt(idx);
            }
        }
        for (int idx = 0; idx < num; ++idx)
        {
            int summonPos = -1;
            for (int stubX = 0; stubX < PathFinder.H_GRID; ++stubX)
            {
                for (int stubY = 0; stubY < PathFinder.V_GRID; ++stubY)
                {
                    int fightPos = PathFinder.Stub2InitPos(CurRound, (ambush ? !isEnemy : isEnemy), (stubX + 1) * 10 + (stubY + 1));
                    if (!CheckGridPosOccupy(fightPos))
                    {
                        summonPos = fightPos;
                        break;
                    }
                }
            }
            if (summonPos >= 0)
            {
                Monster monster = JsonMgr.GetSingleton().GetMonsterByID(monsterId);
                if (monster == null)
                {
                    EDebug.LogErrorFormat("Summon failed, could not find monster {0} from json", monsterId);
                    return;
                }
                FightUnit summon = new FightUnit(monster, 0, isEnemy, true);
                summon.UID = ++UID;
                _createFightUnitView(summon);
                summon.PathFinderObj.SetPos(summonPos);
                AllFighters.Add(summon);
                ZEventSystem.Dispatch(EventConst.OnCreateSummon, summon);
            }
            else
                EDebug.Log("Summon failed, could not find summonPos");
        }
    }

    /// <summary>
    /// 检查格子是否被占用
    /// </summary>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public bool CheckGridPosOccupy(int gridPos)
    {
        bool occupy = false;
        for (int idx = 0; idx < AllFighters.Count; ++idx)
        {
            FightUnit unit = AllFighters[idx];
            if (unit.IsDead)
                continue;
            if (unit.GridPos == gridPos)
            {
                occupy = true;
                break;
            }
        }
        return occupy;
    }

    /// <summary>
    /// 选择一个攻击目标
    /// </summary>
    /// <param name="self">自己</param>
    /// <param name="canAttack">是否考虑必须可以攻击到</param>
    /// <returns></returns>
    public FightUnit SelectFightTarget(FightUnit self, bool canAttack)
    {
        if (self == null || self.IsDead)
        {
            EDebug.LogError("FightLogic.SelectFightTarget failed, fightunit is null or dead");
            return null;
        }
        int selIdx = -1;
        float dis = float.MaxValue;

        //先循环一遍看看有没有可以攻击的对象，如果没有，则不考虑是否无敌
        int targetsCanBeAttackCnt = 0;
        for (int idx = 0; idx < AllFighters.Count; ++idx)
        {
            FightUnit unit = AllFighters[idx];
            if (!unit.CanBeAttack(self, false, true, true))
                continue;
            targetsCanBeAttackCnt++;
        }
        for (int idx = 0; idx < AllFighters.Count; ++idx)
        {
            FightUnit unit = AllFighters[idx];
            if (!unit.CanBeAttack(self, 0 == targetsCanBeAttackCnt, !canAttack, false))
                continue;
            //计算距离
            float gridDis = PathFinder.GridDis(self.GridPos, unit.GridPos);
            if (gridDis < dis)
            {
                selIdx = idx;
                dis = gridDis;
            }
        }
        return (selIdx >= 0) ? AllFighters[selIdx] : null;
    }

    /// <summary>
    /// 战斗单位移动到targetpos事件，主要判断是不是所有单位都到位了
    /// </summary>
    /// <param name="unit"></param>
    public void OnUnitMoveOver(FightUnit unit)
    {
        if (unit == null)
        {
            EDebug.LogError("FightLogic.OnUnitOver failed, unit is null");
            return;
        }
        unit.State = FightUnitState.Idle;
        //如果所有的单位都移动到指定位置，移除事件，进入下一状态
        if (State == FightState.Prepare)
        {
            bool allUnitIdle = true;
            for (int idx = 0; idx < AllFighters.Count; ++idx)
            {
                FightUnit fightUnit = AllFighters[idx];
                if (fightUnit.IsDead)
                    continue;
                if (fightUnit.State != FightUnitState.Idle)
                {
                    allUnitIdle = false;
                    break;
                }
            }
            //所有单位都到达了指定位置
            if (allUnitIdle)
            {
                ZEventSystem.DeRegister(EventConst.OnUnitMoveOver, this);
                State = FightState.Fight;
            }
        }
    }

    public void OnFightMaskOver()
    {
        if (State == FightState.Continue)
        {
            NextRound();
            CamMgrObj.ChangeRound(CurRound);
            MapMgr.Instance.CreateFightMap(CurRound, TotalRound);
        }
    }

    private List<uint> _unitPaused = new List<uint>();
    /// <summary>
    /// 大招暂停请求
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="pause"></param>
    public void OnRequestUnitPause(FightUnit unit, bool pause)
    {
        uint UID = unit.UID;
        for (int idx = 0; idx < _unitPaused.Count; ++idx)
        {
            if (UID == _unitPaused[idx])
            {
                if (pause)
                    return;
                else
                    _unitPaused.RemoveAt(idx);
            }
        }
        if (pause)
            _unitPaused.Add(UID);
        this.UnitPause = _unitPaused.Count > 0;
    }

    /// <summary>
    /// 开始下一回合，生成当前回合的出手顺序列表
    /// </summary>
    public void NextRound()
    {

        if (CurRound != 0)
        {
            List<FightUnit> curEnemy = EnemyFighters[CurRound - 1];
            for (int idx = 0; idx < curEnemy.Count; ++idx)
            {
                //最好判断一下死没死光
                if (!curEnemy[idx].IsDead && !curEnemy[idx].IsSummon)
                {
                    EDebug.LogErrorFormat("FightLogic.NextRound failed, round {0} has enemy alive", CurRound);
                    return;
                }
            }
        }
        CurRound++;
        if(CurRound > TotalRound)
        {
            EDebug.LogErrorFormat("FightLogic.NextRound failed, round:{0} total round:{1}", CurRound, TotalRound);
            return;
        }
        ZEventSystem.Dispatch(EventConst.OnNewRound, this.CurRound, this.TotalRound);
        this.AccRoundTime = 0;
        AllFighters.Clear();
        AllFighters.AddRange(Fighters);
        List<FightUnit> curRoundEnemy = EnemyFighters[CurRound - 1];

        //Test~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        for (int idx = 0; idx < curRoundEnemy.Count; ++idx)
        {
            FightUnit enemyUnit = curRoundEnemy[idx];
            _createFightUnitView(enemyUnit);
        }

        //EndTest~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        AllFighters.AddRange(curRoundEnemy);
        //排出手顺序
        AllFighters.Sort((FightUnit a, FightUnit b) => {
            if (a.Order > b.Order)
            {
                return 1;
            }
            else if (a.Order < b.Order)
            {
                return -1;
            }
            else
            {
                if (a.IsEnemy && !b.IsEnemy)
                {
                    return (IsPvp ? 1 : -1);
                }
                else if (!a.IsEnemy && b.IsEnemy)
                {
                    return (IsPvp ? -1 : 1);
                }
                else
                {
                    return 0;
                }
            }
        });

        State = FightState.Prepare;
        ZEventSystem.Dispatch(EventConst.OnInitEvent);
    }
	
    /// <summary>
    /// 战斗主循环
    /// </summary>
	public void Update()
	{
        float delta = Time.deltaTime;
        for (int idx = 0; idx < AllFighters.Count; ++idx)
        {
            FightUnit unit = AllFighters[idx];

            //自动放大招，写在外面防止被暂停
            if (!FightPause)
            {
                if (!unit.IsDead && (unit.IsEnemy || unit.IsSummon || (!unit.IsEnemy && FightLogic.Instance.IsAutoFight)))
                {
                    if (unit.State != FightUnitState.Skill || unit.SklState == SkillState.Post)
                        unit.UseActiveSkill();
                }
            }

            if (UnitPause && !unit.IsUsingActiveSkill && !unit.SkillMgrObj.IsWaitActiveSkill())
                continue;
            unit.Update(delta);
        }

        //子弹、特效和View都在内部判断暂停
        BulletMgrObj.UpdateBullet(delta);
        EffectMgr.Instance.Update(delta);
        DropMgrObj.Update();    //不需要暂停

        //游戏超时，判输
        if (!FightPause && (State == FightState.Fight || State == FightState.Prepare))
        {
            this.AccRoundTime += delta;
            if (AccRoundTime >= RoundTime)
            {
                //战斗超时，判输
                _terminate(false);
            }
        }
	}
	
	public void Clear()
	{
        //反注册事件
        clearFightState(true);
        ZEventSystem.DeRegister(EventConst.OnFightUnitDie, this);
        ZEventSystem.DeRegister(EventConst.OnFightStateChange, this);
        ZEventSystem.DeRegister(EventConst.OnUnitMoveOver, this);
        ZEventSystem.DeRegister(EventConst.OnRequestUnitPause, this);
        ZEventSystem.DeRegister(EventConst.OnFightMaskOver, this);
        ZEventSystem.DeRegister(EventConst.OnGamePause, this);
        ProcessCtrl.Instance.RemoveUpdate(this);
        Fighters.Clear();
        EnemyFighters.Clear();
        AllFighters.Clear();
        if(aStarFinder != null)
            aStarFinder.Clear();
        if(BulletMgrObj != null)
            BulletMgrObj.Clear();
        MapMgr.Instance.Clear();
        EffectMgr.Instance.Clear();
        DropMgrObj = null;
        CamMgrObj = null;
        Time.timeScale = 1;
        IsAutoFight = false;
        UID = 0;

    }

    /// <summary>
    /// 终止战斗
    /// </summary>
    /// <param name="result"></param>
    private void _terminate(bool result)
    {
        this.HasWin = result;
        State = FightState.Over;
    }

    /// <summary>
    /// 一场(或所有)战斗结束，清空状态
    /// </summary>
    /// <param name="overOrContinue"></param>
    private void clearFightState(bool overOrContinue)
    {
        for (int idx = 0; idx < AllFighters.Count; ++idx)
        {
            FightUnit unit = AllFighters[idx];
            unit.BuffMgrObj.ClearBuff(overOrContinue ? ClearBuffType.All : ClearBuffType.Both);
            unit.SkillMgrObj.Clear();
            UnitPause = false;
            IsSpeedUp = false;
            if(unit.IsUsingActiveSkill)
                ZEventSystem.Dispatch(EventConst.OnRequestUnitPause, unit, false);
            _unitPaused.Clear();
        }
    }
}
