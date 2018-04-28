using UnityEngine;
using System.Collections.Generic;


public enum ItemID
{
    Cash = 2000,
    Gold,
    LockedGold,
    Power,
    Exp = 2006,
    ExpPool,
    Honor,
}

public enum StubType
{
    PVE = 1,
    PVPAttack = 2,
    PVPDefend = 3,
    March = 4,
}


public class Role : Singleton<Role>
{
    public Role()
    {
        _stubData.Add(StubType.PVE, new List<Vector2Int>());
        _stubData.Add(StubType.PVPAttack, new List<Vector2Int>());
        _stubData.Add(StubType.PVPDefend, new List<Vector2Int>());
        _stubData.Add(StubType.March, new List<Vector2Int>());
    }
    //RoleId
    private uint _roleId = 0;
    public uint RoleId
    {
        get { return _roleId; }
        set
        {
            if (_roleId != value)
            {
                _roleId = value;
            }
        }
    }
    //战队名称
    private string _roleName = "";
    public string RoleName
    {
        get { return _roleName; }
        set
        {
            if (_roleName != value)
            {
                _roleName = value;
            }
        }
    }
    //头像id
    private int _headId = 0;
    public int HeadId
    {
        get { return _headId; }
        set
        {
            if (_headId != value)
            {
                _headId = value;
            }
        }
    }
    //战队等级
    private int _level = 0;
    public int Level
    {
        get { return Mathf.Max(1, _level); }
        set
        {
            if (_level != value)
            {
                _level = value;
            }
        }
    }
    //战队经验
    private int _exp = 0;
    public int Exp
    {
        get { return Mathf.Max(0, _exp); }
        set
        {
            if (_exp != value)
            {
                _exp = value;
            }
        }
    }
    //经验池
    private long _expPool = 0;
    public long ExpPool
    {
        get { return (long)Mathf.Max(100000000,_expPool); }
        set
        {
            if (_expPool != value)
            {
                _expPool = value;
            }
        }
    }
    //VIP等级
    private int _vip = 0;
    public int Vip
    {
        get { return _vip; }
        set
        {
            if (_vip != value)
            {
                _vip = value;
            }
        }
    }
    //VIP经验
    private int _vipExp = 0;
    public int VipExp
    {
        get { return _vipExp; }
        set
        {
            if (_vipExp != value)
            {
                _vipExp = value;
            }
        }
    }
    //充值货币
    private int _gold = 0;
    public int Gold
    {
        get { return _gold; }
        set
        {
            if (_gold != value)
            {
                _gold = value;
            }
        }
    }
    //绑定充值货币
    private int _lockedGold = 0;
    public int LockedGold
    {
        get { return _lockedGold; }
        set
        {
            if (_lockedGold != value)
            {
                _lockedGold = value;
            }
        }
    }
    //货币
    private long _cash = 0;
    public long Cash
    {
        get { return (long)Mathf.Max(0, _cash); }
        set
        {
            if (_cash != value)
            {
                _cash = value;
            }
        }
    }
    //体力
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
    //荣誉
    private int _honor = 0;
    public int Honor
    {
        get { return _honor; }
        set
        {
            if (_honor != value)
            {
                _honor = value;
            }
        }
    }
    //性别
    private int _sex = 0;
    public int Sex
    {
        get { return _sex; }
        set
        {
            if (_sex != value)
            {
                _sex = value;
            }
        }
    }
    //签到ID
    private int _signInID = 0;
    public int SignInID
    {
        get { return _signInID; }
        set
        {
            if (_signInID != value)
            {
                _signInID = value;
            }
        }
    }
    //占卜进度
    private int _divinationTree = 0;
    public int DivinationTree
    {
        get { return _divinationTree; }
        set
        {
            if (_divinationTree != value)
            {
                _divinationTree = value;
            }
        }
    }
    //占卜幸运值
    private float _divinationLucky = 0;
    public float DivinationLucky
    {
        get { return _divinationLucky; }
        set
        {
            if (_divinationLucky != value)
            {
                _divinationLucky = value;
            }
        }
    }

    //布阵数据
    private Dictionary<StubType, List<Vector2Int>> _stubData = new Dictionary<StubType, List<Vector2Int>>();

    public List<Vector2Int> GetStubData(StubType type)
    {
        if (_stubData.ContainsKey(type))
            return _stubData[type];
        return null;
    }

    public Dictionary<StubType, List<Vector2Int>> GetStubDatas()
    {
        return _stubData;
    }

    public void InitStubData(Msg.StubMsg.StubMsg msg)
    {
        _initStubData(_stubData[StubType.PVE], msg.pve);
        _initStubData(_stubData[StubType.PVPAttack], msg.pvpattack);
        _initStubData(_stubData[StubType.PVPDefend], msg.pvpdefend);
        _initStubData(_stubData[StubType.March], msg.march);
    }

    private void _initStubData(List<Vector2Int> data, Msg.StubMsg.StubNode[] source)
    {
        if (data == null || source == null)
            return;
        data.Clear();
        for (int idx = 0; idx < source.Length; ++idx)
        {
            data.Add(new Vector2Int(source[idx].pos, source[idx].heroid));
        }
    }

    public long GetItemNum(ItemID itemID)
    {
        switch(itemID)
        {
            case ItemID.Cash:
                return Cash;
            case ItemID.Exp:
                return Exp;
            case ItemID.ExpPool:
                return ExpPool;
            case ItemID.Gold:
                return Gold;
            case ItemID.Honor:
                return Honor;
            case ItemID.LockedGold:
                return LockedGold;
            case ItemID.Power:
                return Power;
            default:
                return 0;
        }
    }
}
