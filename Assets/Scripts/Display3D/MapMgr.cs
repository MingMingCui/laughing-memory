using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMgr : Singleton<MapMgr> {

    private GameObject _map = null;
    private int[] _mapRes = { 10100, 10101, 10102 };
    private int[] _sprintMap = { 10103, 10104, 10105 };

    public void CreateFightMap(int curRound, int totalRound)
    {
        if (_map != null)
        {
            GameObject.Destroy(_map);
        }
        int randMap = Random.Range(0, _mapRes.Length - 1);
        _map = GameObject.Instantiate(ResourceMgr.Instance.LoadResource(_mapRes[curRound != totalRound ? randMap : _mapRes.Length - 1]) as GameObject);
        _map.transform.position = new Vector3((3 * curRound - 1) * PathFinder.V_GRID * PathFinder.GRID_SIZE, 0, -PathFinder.H_GRID * PathFinder.GRID_SIZE * 0.5f);
    }

    public Vector3 GetMapPos()
    {
        return _map != null ? _map.transform.position : Vector3.zero;
    }

    public void Clear()
    {
        _map = null;
    }
}
