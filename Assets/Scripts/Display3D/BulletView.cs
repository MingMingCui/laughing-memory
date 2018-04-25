using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObj
{
    public bool IsUsing = false;
    public int ResId = 0;
    public GameObject bullet = null;
}

public class BulletView : Singleton<BulletView> {

    private List<BulletObj> _allBullet = new List<BulletObj>();
    public Vector3 FAR_POS = new Vector3(9999, 9999, 9999);

    public BulletObj CreateBullet(int resid)
    {
        BulletObj _obj = null;
        for (int idx = 0; idx < _allBullet.Count; ++idx)
        {
            BulletObj obj = _allBullet[idx];
            if (!obj.IsUsing && obj.ResId == resid)
            {
                _obj = obj;
                _obj.bullet.SetActive(true);
                break;
            }
        }
        if (_obj == null)
        {
            _obj = new BulletObj();
            _obj.ResId = resid;
            _obj.bullet = GameObject.Instantiate(ResourceMgr.Instance.LoadResource(resid) as GameObject);
            _obj.bullet.transform.position = FAR_POS;
            _allBullet.Add(_obj);
        }
        _obj.IsUsing = true;
        return _obj;
    }

    public void DestroyBullet(BulletObj obj)
    {
        obj.IsUsing = false;
        obj.bullet.transform.position = FAR_POS;
        obj.bullet.SetActive(false);
    }

    public void Clear()
    {
        _allBullet.Clear();
    }
}
