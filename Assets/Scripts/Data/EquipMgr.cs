using System.Collections.Generic;

public class EquipMgr
{
    //装备格子数
    public static readonly int EquipGridCnt = 8;

    private static EquipMgr _equip;
    public static EquipMgr GetSingleton()
    {
        return _equip ?? (_equip = new EquipMgr());
    }
    private EquipMgr()
    {
        _id = new Dictionary<string, EquipData>();
        _type = new Dictionary<EquipPart, List<string>>();
    }
    /// <summary>
    /// 主要存储结构
    /// </summary>
    private Dictionary<string, EquipData> _id;

    private EquipData[] _allData;
    private Dictionary<EquipPart, List<string>> _type;
     /// <summary>
     /// 获取指定英雄的装备
     /// </summary>
     /// <param name="hero"></param>
     /// <returns></returns>
    public EquipData[] GetHeroEquip(HeroData hero)
    {
        EquipData[] ret = new EquipData[EquipGridCnt];
        GetSpecifyEquip(hero.Equips, ref ret);
        return ret;
    }
    /// <summary>
    /// 获取指定类型的装备
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    public EquipData[] GetEquipByPart(EquipPart part)
    {
        List<string> md5s;
        _type.TryGetValue(part, out md5s);
        if(md5s == null)
            return null;
        EquipData[] ret = new EquipData[md5s.Count];
        GetSpecifyEquip(_type[part].ToArray(), ref ret);
        return ret;
    }
    /// <summary>
    /// 获取指定的装备
    /// </summary>
    /// <param name="ud5"></param>
    /// <param name="ret"></param>
    private void GetSpecifyEquip(string[] ud5,ref EquipData[] ret)
    {
        for (int i = 0; i < ud5.Length; ++i)
        {
            if (_id.ContainsKey(ud5[i]))
                ret[i] = _id[ud5[i]];
        }
    }
    /// <summary>
    /// 获取一个装备
    /// </summary>
    /// <param name="md5"></param>
    /// <returns></returns>
    public EquipData GetEquipDataByUID(string md5)
    {
        if (_id.ContainsKey(md5))
            return _id[md5];
        return null;
    }
    /// <summary>
    /// 获取所有装备
    /// </summary>
    /// <returns></returns>
    public EquipData[] AllEquip
    {
        get
        {
            if (_allData == null)
            {
                _allData = new EquipData[_id.Count];
                _id.Values.CopyTo(_allData, 0);
            }
            return _allData;
        }
    }

    /// <summary>
    /// 增加装备
    /// </summary>
    /// <param name="equip"></param>
    public void AddEquip(EquipData equip)
    {
        _allData = null;
        if (_id.ContainsKey(equip.md5))
        {
            _id[equip.md5] = equip;
        }
        else
        {
            _id.Add(equip.md5, equip);
        }
        EquipPart part = (EquipPart)equip.JsonData.EquiPrat;
        if (_type.ContainsKey(part))
        {
            if (!_type[part].Contains(equip.md5))
                _type[part].Add(equip.md5);
        }
        else
        {
            List<string> values = new List<string>()
            {
                equip.md5,
            };
            _type.Add(part, values);
        }
    }
    /// <summary>
    /// 检测是否有可以穿戴的装备
    /// </summary>
    /// <param name="part">部位</param>
    /// <param name="heroType">没有限制填0</param>
    /// <returns></returns>
    public string TestCanDress(EquipPart part, HeroData heroData)
    {
        int dress = GetType(part, heroData);
        return HasCanDress(part, dress);
    }
    private string HasCanDress(EquipPart part, int heroType)
    {
        EquipData[] equips = EquipMgr.GetSingleton().GetEquipByPart(part);
        if (equips == null)
            return "";
        for (int i = 0; i < equips.Length; ++i)
        {
            int type = equips[i].JsonData.Type;
            bool dress = part == EquipPart.Weapon ? type == heroType : type <= heroType;
            if (equips[i].HeroId == 0 && dress)
                return equips[i].md5;
        }
        return "";
    }

    private int GetType(EquipPart part, HeroData heroData)
    {
        switch (part)
        {
            case EquipPart.Body:
            case EquipPart.Shoes:
            case EquipPart.Helmet:
                return heroData.JsonData.armortpye;
            case EquipPart.Weapon:
                return heroData.JsonData.weapontype;
            default:
                return 0;
        }
    }

    /// <summary>
    /// 穿戴装备
    /// </summary>
    public void TakeEquip(EquipData equip,HeroData hero)
    {
        if (equip.HeroId != 0)
            return;
        EquipPart part = equip.JsonData.EquiPrat;
        hero[0, PartToIndex(part)] = equip.md5;
        equip.HeroId = hero.HeroId;
    }

    /// <summary>
    /// 脱下装备 调用之前判空
    /// </summary>
    /// <param name="equip"></param>
    public void TakeOffEquip(EquipData equip)
    {
        if (!_id.ContainsKey(equip.md5))
            return;
        if (equip.HeroId != 0)
        {
            HeroData hero = HeroMgr.GetSingleton().GetHeroData(equip.HeroId);
            int index = PartToIndex(equip.JsonData.EquiPrat);
            hero[0, index] = "";
            equip.HeroId = 0;
        }
    }

    private int PartToIndex(EquipPart part)
    {
        return (int)part - 1;
    }

    public void UpEquip(EquipData equip)
    {
        if (equip == null)
            return;
        if(_id.ContainsKey(equip.md5))
        {
            if(equip.StrengthenLv >= Role.Instance.Level)
            {
                CanvasView.Instance.AddNotice("升级主公等级开放更多");
                return;
            }
            equip.StrengthenLv++;

            _id[equip.md5] = equip;
        }
    }
}
