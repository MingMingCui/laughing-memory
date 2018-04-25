using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMgr {

    private GameObject _treasureBox = null;
    public float TreasureSize = 1.3f;
    public float TreasureRate = 0.2f;
    public float TreasureLife = 1f;
    private List<DropObj> _dropList = new List<DropObj>();

    private class DropObj
    {
        public GameObject TreasureObj = null;
        public float AccTime = 0;
    }

    public DropMgr()
    {
        _treasureBox = ResourceMgr.Instance.LoadResource(1048) as GameObject;
        TreasureSize = JsonMgr.GetSingleton().GetGlobalIntArrayByID(1008).value;
        TreasureRate = JsonMgr.GetSingleton().GetGlobalIntArrayByID(1009).value;
        TreasureLife = JsonMgr.GetSingleton().GetGlobalIntArrayByID(1010).value;
    }

    public void Update()
    {
        for (int idx = 0; idx < _dropList.Count; ++idx)
        {
            _dropList[idx].AccTime += Time.deltaTime;
        }

        for (int idx = _dropList.Count - 1; idx >= 0; --idx)
        {
            DropObj obj = _dropList[idx];
            if (obj.AccTime >= TreasureLife)
            {
                ZEventSystem.Dispatch(EventConst.OnTreasureFly, obj.TreasureObj.transform.position);
                GameObject.Destroy(obj.TreasureObj);
                _dropList.RemoveAt(idx);
            }
        }
    }

    public void CreateTreasure(Vector2 pos, int cnt)
    {
        if (cnt <= 0)
            return;
        var drops = JsonMgr.GetSingleton().GetDropOrderArray();
        for (int i = 0, length = drops.Length; i < length &&  i < cnt; i++)
        {
            var offset = drops[i];
            float offsetX = offset.x * TreasureSize;
            float offsetY = offset.y * TreasureSize;
            DropObj obj = new DropObj
            {
                TreasureObj = GameObject.Instantiate(_treasureBox)
            };
            obj.TreasureObj.transform.localPosition = new Vector3(pos.x + offsetX, 0.1f, pos.y + offsetY);
            _dropList.Add(obj);
        }
    }
}
