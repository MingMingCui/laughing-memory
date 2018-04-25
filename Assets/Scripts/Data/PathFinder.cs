using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridDirection
{
    LT = 1,
    T = 2,
    RT = 3,
    L = 4,
    M = 5,
    R = 6,
    LB = 7,
    B = 8,
    RB = 9,
}

public class PathFinder{
    //格子大小，米数
    public static readonly float GRID_SIZE = 2;
    //移动速度，米/秒
    public static readonly float MOVE_SPEED = 5;
    //最大逻辑行数
    public static readonly int MAX_ROW = 5;
    //最大逻辑列数
    public static readonly int MAX_COLOMN = 5;
    //纵向最多格子数
    public static readonly int V_GRID = MAX_ROW;
    //单个区域横向最大格子数
    public static readonly int H_GRID = MAX_COLOMN;
    //区域最大格子数
    public static readonly int AREA_TOTAL = V_GRID * H_GRID;
    //周围八方向
    public static GridDirection[] adjacentDirs = { GridDirection.LT, GridDirection.T, GridDirection.RT, GridDirection.L, GridDirection.R, GridDirection.LB, GridDirection.B, GridDirection.RB };
    //八方向排序用缓存
    private List<Vector2> dirSortList = new List<Vector2>();

    private float SpoofArange = 0;

    /// <summary>
    /// 根据布阵坐标计算实际坐标
    /// </summary>
    /// <param name="round"></param>
    /// <param name="isEnemy"></param>
    /// <param name="stubPos"></param>
    /// <returns></returns>
    public static int Stub2InitPos(int round, bool isEnemy, int stubPos)
    {
        int area = (round - 1) * 3 + (isEnemy ? 3 : 0);
        int offsetX = stubPos / 10 - 1;
        int offsetY = stubPos % 10 - 1;
        //敌人站位是反向的
        if (isEnemy)
            offsetX = MAX_COLOMN - 1 - offsetX;

        return area * AREA_TOTAL + offsetX * V_GRID + offsetY;
    }

    public static int Stub2FightPos(int round, bool isEnemy, int stubPos)
    {
        int area = (round - 1) * 3 + (isEnemy ? 2 : 1);
        int offsetX = stubPos / 10 - 1;
        int offsetY = stubPos % 10 - 1;
        //敌人站位是反向的
        if (isEnemy)
            offsetX = MAX_COLOMN - 1 - offsetX;

        return area * AREA_TOTAL + offsetX * V_GRID + offsetY;
    }

    /// <summary>
    /// 获取战斗边界grid
    /// </summary>
    /// <param name="round"></param>
    /// <param name="isEnemy"></param>
    /// <returns></returns>
    public static int GetStubBorder(int round, bool isEnemy)
    {
        int area = (round - 1) * 3 + (isEnemy ? 3 : 1);
        return area * AREA_TOTAL;
    }

    /// <summary>
    /// 初始位置计算战斗位置
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="isEnemy"></param>
    /// <returns></returns>
    public static int InitPos2FightPos(int pos, bool isEnemy)
    {
        return pos + (isEnemy ? -1 : 1) * AREA_TOTAL;
    }

    /// <summary>
    /// 根据格子位置计算实际坐标
    /// </summary>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public static Vector2 Grid2Pos(int gridPos)
    {
        int gridX = gridPos / V_GRID;
        int gridY = gridPos % H_GRID;
        return new Vector2(gridX * GRID_SIZE + GRID_SIZE * 0.5f, -gridY * GRID_SIZE - 0.5f * GRID_SIZE);
    }

    public static float GridDis(int from, int to)
    {
        int xoffset = to / V_GRID - from / V_GRID;
        int yoffset = to % H_GRID - from % H_GRID;
        return Mathf.Sqrt(Mathf.Pow(xoffset, 2) + Mathf.Pow(yoffset, 2));
    }

    public static int ManhattanDis(int from, int to)
    {
        int xoffset = to / V_GRID - from / V_GRID;
        int yoffset = to % H_GRID - from % H_GRID;
        return xoffset + yoffset;
    }

    /// <summary>
    /// 计算八方向相邻格子
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static int BorderGrid(int gridPos, GridDirection dir)
    {
        switch (dir)
        {
            case GridDirection.LT:
                {
                    if (gridPos % V_GRID == 0)
                    {
                        return -1;
                    }
                    return gridPos - V_GRID - 1;
                }
                break;
            case GridDirection.T:
                {
                    if (gridPos % V_GRID == 0)
                    {
                        return -1;
                    }
                    return gridPos - 1;
                }
                break;
            case GridDirection.RT:
                {
                    if (gridPos % V_GRID == 0)
                    {
                        return -1;
                    }
                    return gridPos + V_GRID - 1;
                }
                break;
            case GridDirection.L:
                return gridPos - V_GRID;
                break;
            case GridDirection.R:
                return gridPos + V_GRID;
                break;
            case GridDirection.LB:
                {
                    if (gridPos % V_GRID == V_GRID - 1)
                    {
                        return -1;
                    }
                    return gridPos - V_GRID + 1;
                }
            case GridDirection.B:
                {
                    if (gridPos % V_GRID == V_GRID - 1)
                    {
                        return -1;
                    }
                    return gridPos + 1;
                }
                break;
            case GridDirection.RB:
                {
                    if (gridPos % V_GRID == V_GRID - 1)
                    {
                        return -1;
                    }
                    return gridPos + V_GRID + 1;
                }
            default:
                return -1;
                break;
        }
    }

    //寻路的主体单位
    private FightUnit _mFighter = null;
    //寻路的目标单位
    private FightUnit _mTarget = null;
    //寻路的目标格子,-1表示没有
    private int _mTargetPos = -1;

    public PathFinder(FightUnit unit)
    {
        this._mFighter = unit;
        this.SpoofArange = JsonMgr.GetSingleton().GetGlobalIntArrayByID(1024).value;
    }

    /// <summary>
    /// 开始寻路，按照战斗单位
    /// </summary>
    /// <param name="target"></param>
    //public void StartFind(FightUnit target)
    //{
    //    Debug.LogFormat("{0} StartFind", this._mFighter.HeroId);
    //    if (target == null)
    //    {
    //        EDebug.LogError("PathFinder.StartFind failed, target is null");
    //        return;
    //    }
    //    if (target.IsDead)
    //    {
    //        EDebug.LogError("PathFinder.StartFind failed, target is dead");
    //        return;
    //    }
    //    this._mTarget = target;
    //    this._mFighter.State = FightUnitState.Move;
    //}

    /// <summary>
    /// 开始寻路，按照格子位置
    /// </summary>
    /// <param name="targetPos"></param>
    //public void StartFind(int targetPos)
    //{
    //    this._mTargetPos = targetPos;
    //    this._mFighter.State = FightUnitState.Move;
    //}

    /// <summary>
    /// 开始寻路，寻找最近可攻击目标
    /// </summary>
    public void StartFind()
    {
        this._mFighter.State = FightUnitState.Move;
    }

    /// <summary>
    /// 结束寻路
    /// </summary>
    public void  StopFind()
    {
        this._mTarget = null;
        this._mTargetPos = -1;
        this._mFighter.State = FightUnitState.Idle;
        //最后发事件，因为FIghtLogic要读状态是不是Idle
        ZEventSystem.Dispatch(EventConst.OnUnitMoveOver, this._mFighter);
    }

    /// <summary>
    /// 强制设置到某个位置
    /// </summary>
    /// <param name="gridPos"></param>
    public void SetPos(int gridPos)
    {
        if(this._mFighter.State == FightUnitState.Move)
            StopFind();
        this._mFighter.GridPos = gridPos;
        this._mFighter.TargetPos = Grid2Pos(gridPos);
        this._mFighter.CurPos = this._mFighter.TargetPos;
        this._mFighter.CurRot.SetLookRotation(this._mFighter.IsEnemy ? -Vector3.right : Vector3.right);
    }

    ///// <summary>
    ///// 判断是否有可以攻击目标，如果有返回最近的
    ///// </summary>
    ///// <returns></returns>
    //public FightUnit CanAttack()
    //{
    //    FightUnit _target = null;
    //    float _targetDis = 0;
    //    for (int idx = 0; idx < FightLogic.Instance.AllFighters.Count; ++idx)
    //    {
    //        FightUnit unit = FightLogic.Instance.AllFighters[idx];
    //        if (unit.IsDead || (unit.IsEnemy == this._mFighter.IsEnemy && _mFighter.CState != ControlState.Sow) || 
    //            (unit.Invincible && !_mFighter.DeInvincible))
    //            continue;

    //        int curGrid = this._mFighter.GridPos;
    //        int targetPos = unit.GridPos;
    //        int xoffset = (targetPos / V_GRID) - (curGrid / V_GRID);
    //        int yoffset = (targetPos % H_GRID) - (curGrid % H_GRID);
    //        float dis = Mathf.Pow(xoffset, 2) + Mathf.Pow(yoffset, 2);
    //        if (Mathf.Pow(this._mFighter.Arange, 2) >= dis)
    //        {
    //            if (_target == null || dis < _targetDis)
    //            {
    //                _target = unit;
    //                _targetDis = dis;
    //            }
    //        }
    //    }
    //    return _target;
       
    //}

    /// <summary>
    /// 判断目标是否可以被攻击
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool CanAttackTarget(FightUnit target)
    {
        if (target == null || target.IsDead)
            return false;
        int curGrid = this._mFighter.GridPos;
        int targetPos = target.GridPos;
        int xoffset = (targetPos / V_GRID) - (curGrid / V_GRID);
        int yoffset = (targetPos % H_GRID) - (curGrid % H_GRID);
        return Mathf.Pow(this._mFighter.Arange, 2) >= Mathf.Pow(xoffset, 2) + Mathf.Pow(yoffset, 2);
    }

    public void Update(float delta)
    {
        //更新CurPos
        if (this._mFighter.CurPos != this._mFighter.TargetPos)
        {
            float deltaDis = MOVE_SPEED * delta;
            if ((this._mFighter.TargetPos - this._mFighter.CurPos).magnitude <= deltaDis)
            {
                this._mFighter.CurPos = this._mFighter.TargetPos;
                ZEventSystem.Dispatch(EventConst.OnUnitMoveOver, this._mFighter);
            }
            else
            {
                this._mFighter.CurPos += ((this._mFighter.TargetPos - this._mFighter.CurPos).normalized * deltaDis);
            }
        }
        else
        {
            //到达当前格子位置
            if (this._mFighter.State == FightUnitState.Move)
            {
                if (this._mFighter.CState != ControlState.Spoof)
                {
                    FightUnit target = FightLogic.Instance.SelectFightTarget(this._mFighter, true);
                    if (target != null)
                    {
                        this._mFighter.FightTarget = target;
                        StopFind();
                        return;
                    }
                    else
                    {
                        target = FightLogic.Instance.SelectFightTarget(this._mFighter, false);
                        if (target == null)
                        {
                            //没有可以攻击的目标
                            StopFind();
                            return;
                        }
                        else
                        {
                            //寻找下一个目标点
                            int nextGridPos = NextGrid(target.GridPos);
                            if (nextGridPos > 0)
                            {
                                move(nextGridPos);
                            }
                        }
                    }
                }
                else
                {
                    int curRound = FightLogic.Instance.CurRound;
                    if (!checkGridPosIn(this._mFighter.GridPos, curRound))
                    {
                        StopFind();
                        return;
                    }
                    int nextGridPos = this._mFighter.GridPos;

                    int spoofDir = (this._mFighter.Arange <= SpoofArange ? 1 : -1) * (this._mFighter.IsEnemy ? 1 : -1);

                    do
                    {
                        nextGridPos = nextGridPos + spoofDir * V_GRID * 2;
                    } while (FightLogic.Instance.CheckGridPosOccupy(nextGridPos));
                    if (nextGridPos < 0 || nextGridPos == this._mFighter.GridPos)
                    {
                        StopFind();
                        return;
                    }
                    int nextStep = NextGrid(nextGridPos);
                    if (!checkGridPosIn(nextStep, curRound))
                    {
                        StopFind();
                        return;
                    }

                    move(nextStep);
                }
            }
        }
    }

    /// <summary>
    /// 检查格子位置是否在边界内
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="border"></param>
    /// <returns></returns>
    private bool checkGridPosIn(int gridPos, int round)
    {

        int min = ((round - 1) * 3 + 1) * AREA_TOTAL;
        int max = ((round - 1) * 3 + 3) * AREA_TOTAL;

        return gridPos >= min && gridPos < max;
    }

    /// <summary>
    /// 移动到某个格子
    /// </summary>
    /// <param name="grid"></param>
    private void move(int grid)
    {
        //计算朝向
        int xoffset = Util.Judge(grid / V_GRID - this._mFighter.GridPos / V_GRID);
        int yoffset = Util.Judge(grid % V_GRID - this._mFighter.GridPos % V_GRID);
        this._mFighter.CurRot.SetLookRotation(new Vector3(xoffset, 0, -yoffset));

        this._mFighter.GridPos = grid;
        this._mFighter.TargetPos = Grid2Pos(grid);
    }

    /// <summary>
    /// 计算下一个格子
    /// </summary>
    /// <param name="targetPos">目标格子位置</param>
    /// <returns></returns>
    public int NextGrid(int targetPos)
    {
        int curGrid = this._mFighter.GridPos;
        dirSortList.Clear();
        for (int idx = 0; idx < adjacentDirs.Length; ++idx)
        {
            int dirGrid = BorderGrid(targetPos, adjacentDirs[idx]);
            if (dirGrid > 0 && checkGrid(dirGrid))
            {
                float d = GridDis(curGrid, dirGrid);
                dirSortList.Add(new Vector2(dirGrid, d));
            }
        }
        dirSortList.Sort((Vector2 a, Vector2 b) =>
        {
            if (a.y < b.y)
                return -1;
            else if (a.y > b.y)
                return 1;
            else
                return 0;
        });
        List<int> obstacleList = new List<int>();
        for (int idx = 0; idx < FightLogic.Instance.AllFighters.Count; ++idx)
        {
            FightUnit unit = FightLogic.Instance.AllFighters[idx];
            if (unit.IsDead)
                continue;
            int gridPos = unit.GridPos;
            if (gridPos == curGrid)
                continue;
            obstacleList.Add(gridPos);
        }

        for (int idx = 0; idx < dirSortList.Count; ++idx)
        {
            List<int> path = null;
            FightLogic.Instance.aStarFinder.Clear();
            if (FightLogic.Instance.aStarFinder.Search(curGrid, (int)dirSortList[idx].x, obstacleList, out path))
            {
                return path[path.Count - 2];
            }
        }

        return -1;
    }

    private int findNearPosOfTarget(int start, int target)
    {
        float dis = -1;
        int pos = -1;

        for (int idx = 0; idx < adjacentDirs.Length; ++idx)
        {
            int dirGrid = BorderGrid(target, adjacentDirs[idx]);
            bool beTaken = false;
            for (int fid = 0; fid < FightLogic.Instance.AllFighters.Count; ++fid)
            {
                if (FightLogic.Instance.AllFighters[fid].GridPos == dirGrid)
                {
                    beTaken = true;
                    break;
                }
            }
            if (beTaken)
                continue;
            if (dirGrid > 0 && checkGrid(dirGrid))
            {
                float d = GridDis(start, dirGrid);
                if (dis < 0 || d < dis)
                {
                    dis = d;
                    pos = dirGrid;
                }
            }
        }
        return pos;
    }

    /// <summary>
    /// 检查格子是否可用
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    private bool checkGrid(int grid)
    {
        if (grid < 0)
            return false;
        List<FightUnit> allFighters = FightLogic.Instance.AllFighters;
        for (int idx = 0; idx < allFighters.Count; ++idx)
        {
            FightUnit unit = allFighters[idx];
            if (unit.IsDead)
                continue;
            if (grid == unit.GridPos)
                return false;
        }
        return true;
    }
	
}
