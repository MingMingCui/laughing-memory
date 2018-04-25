using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int idx;        //当前格子的位置
    public int pre;        //上一个格子的位置
    public int distance;   //当前格子距离起点的距离
    public int priority;   //当前格子的估值
    public bool isBlock;   //是否无法通过
}

public enum PathDir
{
    LT = 0,
    T = 1,
    RT = 2,
    L = 3,
    R = 4,
    LB = 5,
    B = 6,
    RB = 7,
}

public class AStarFinder{

    private int _row = 0;
    private int _column = 0;
    private int _total = 0;
    private PathNode[] _allNodes = null;
    private int _start = -1;
    private int _end = -1;
    private PriorityQueue<int> _openList1 = new PriorityQueue<int>();
    private PathDir[] adjacentDirs = new PathDir[4] { PathDir.T, PathDir.B, PathDir.L, PathDir.R };


    private Dictionary<int, int> came_from = new Dictionary<int, int>();
    private Dictionary<int, int> cost_so_far = new Dictionary<int, int>();
    public AStarFinder(int row, int column)
    {
        this._row = row;
        this._column = column;
        this._total = row * column;
        _allNodes = new PathNode[_total];
        for (int idx = 0; idx < _allNodes.Length; ++idx)
            _allNodes[idx] = new PathNode();
        clear();
    }

    public bool Search(int start, int end, List<int> obstacles, out List<int> path)
    {
        if (!check(start, end, obstacles))
        {
            path = null;
            return false;
        }
        initSearch(start, end, obstacles);
        if (!step())
        {
            path = null;
            return false;
        }

        //System.Text.StringBuilder sb = new System.Text.StringBuilder();
        path = new List<int>();
        int cur = _end;
        int pre = -1;
        while (came_from.TryGetValue(cur, out pre))
        {
            path.Add(cur);
            //sb.AppendFormat("{0}", cur);
            if (cur == pre)
                break;
            cur = pre;
            //sb.AppendFormat("<-", cur);
        }
        //Debug.Log("寻路完成");
        //Debug.Log(sb.ToString());
        return true;
    }
    public void Clear()
    {
        clear();
    }
    private void initSearch(int start, int end, List<int> obstacles)
    {
        _start = start;
        _end = end;
        for (int idx = 0; idx < obstacles.Count; ++idx)
        {
            _allNodes[obstacles[idx]].isBlock = true;
        }
    }
    private bool check(int start, int end, List<int> obstacles)
    {
        if (start < 0 || start >= _total)
        {
            Debug.LogErrorFormat("AStart.Search failed, start({0}) is invalid", start);
            return false;
        }
        if (end < 0 || end >= _total)
        {
            Debug.LogErrorFormat("AStart.Search failed, end({0}) is invalid", end);
            return false;
        }
        if (start == end)
        {
            Debug.LogErrorFormat("AStar.Search failed, start is same with end");
            return false;
        }
        if (obstacles == null)
            return false;
        bool obstacleInvalid = false;
        for (int idx = 0; idx < obstacles.Count; ++idx)
        {
            int obstacleIdx = obstacles[idx];
            if (obstacleIdx < 0 || obstacleIdx > _total)
            {
                obstacleInvalid = true;
                Debug.LogErrorFormat("AStart.Search failed, obstacle {0} value {1} is invalid", idx, obstacleIdx);
            }
            else if (obstacleIdx == start)
            {
                obstacleInvalid = true;
                Debug.LogErrorFormat("AStart.Search failed, obstacle {0} value {1} is same with start", idx, obstacleIdx);
            }
            else if (obstacleIdx == end)
            {
                obstacleInvalid = true;
                Debug.LogErrorFormat("AStart.Search failed, obstacle {0} value {1} is same with end", idx, obstacleIdx);
            }
        }
        if (obstacleInvalid)
            return false;
        return true;
    }
    private bool step()
    {
        _openList1.Enqueue(_start, 0);
        came_from[_start] = _start;
        cost_so_far[_start] = 0;
        bool hasFinish = false;
        while (0 < _openList1.Count)
        {
            int curr = _openList1.Dequeue();
            if (curr == _end)
            {
                hasFinish = true;
                break;
            }
            PathNode currNode = _allNodes[curr];
            for (int idx = 0; idx < adjacentDirs.Length; ++idx)
            {
                int adjacent = getAdjacent(curr, adjacentDirs[idx]);
                if (adjacent < 0 || _allNodes[adjacent].isBlock)
                {
                    continue;
                }
                int newDistance = cost_so_far[curr] + 1;
                if (!cost_so_far.ContainsKey(adjacent) || newDistance < cost_so_far[adjacent])
                {
                    cost_so_far[adjacent] = newDistance;
                    int priority = newDistance + manhattan(adjacent, _end);
                    _openList1.Enqueue(adjacent, priority);
                    came_from[adjacent] = curr;
                }
                //Debug.LogFormat("Adjacent Node {0} pre {1}", adjacent, adjacentNode.pre);
            }
        }
        return hasFinish;
    }

    private int manhattan(int from, int to)
    {
        int xoffset = Mathf.Abs(to / _row - from / _row);
        int yoffset = Mathf.Abs(to % _row - from % _row);
        return xoffset + yoffset;
    }
    private int getAdjacent(int pos, PathDir dir)
    {
        int tmpPos = -1;
        switch (dir)
        {
            case PathDir.LT:
                if (0 == pos % _row || 0 == pos / _row)
                    return -1;
                tmpPos = pos - _row - 1;
                break;
            case PathDir.T:
                if (0 == pos % _row)
                    return -1;
                tmpPos = pos - 1;
                break;
            case PathDir.RT:
                if (0 == pos % _row || _column - 1 == pos / _row)
                    return -1;
                tmpPos = pos + _row - 1;
                break;
            case PathDir.L:
                if (0 == pos / _row)
                    return -1;
                tmpPos = pos - _row;
                break;
            case PathDir.R:
                if (_column - 1 == pos / _row)
                    return -1;
                tmpPos = pos + _row;
                break;
            case PathDir.LB:
                if (_row - 1 == pos % _row || 0 == pos / _row)
                    return -1;
                tmpPos = pos - _row + 1;
                break;
            case PathDir.B:
                if (_row - 1 == pos % _row)
                    return -1;
                tmpPos = pos + 1;
                break;
            case PathDir.RB:
                if (_row - 1 == pos % _row || _column - 1 == pos / _row)
                    return -1;
                tmpPos = pos + _row + 1;
                break;
        }
        if (tmpPos < 0 || tmpPos >= _total)
            return -1;
        else
            return tmpPos;
    }
    private void clear()
    {
        if (_allNodes != null)
        {
            for (int idx = 0; idx < _allNodes.Length; ++idx)
            {
                PathNode pn = _allNodes[idx];
                pn.idx = idx;
                pn.pre = -1;
                pn.distance = 0;
                pn.isBlock = false;
                pn.priority = 0;
            }
        }
        _openList1.Clear();
        came_from.Clear();
        cost_so_far.Clear();
    }
    private bool comparePathNode(int a, int b)
    {
        if (a < 0 || b < 0 || a >= _total || b >= _total)
        {
            Debug.LogError("AStarFinder.ComparePathNode failed, param error.");
            return false;
        }
        return _allNodes[a].priority < _allNodes[b].priority;
    }
}
