using JsonData;
using System.Collections.Generic;
using UnityEngine.UI;

public class HeroMgr
{
    private static HeroMgr _hero;
    public static HeroMgr GetSingleton()
    {
        return _hero ?? (_hero = new HeroMgr());
    }
    public static int MaxHeroStar;
    public static int MaxHeroLevel;

    private int[] unLockLv;
    private HeroMgr()
    {
        _heroData = new Dictionary<int, HeroData>();
        takeOfficer = new Dictionary<int, int>();
        //调用获取数据 以后由服务器调用
        HeroDataLoaded();
        MaxHeroStar = (int)JsonMgr.GetSingleton().GetGlobalIntArrayByID(1019).value;
        MaxHeroLevel =(int)JsonMgr.GetSingleton().GetGlobalIntArrayByID(1022).value;

        string unlock = JsonMgr.GetSingleton().GetGlobalStringArrayByID(10001).desc;

        string[] temp = unlock.Split('#');
        unLockLv = new int[temp.Length];
        for (int i = 0; i < unLockLv.Length; ++i)
        {
            unLockLv[i] = int.Parse(temp[i]);
        }
    }
  
    private Dictionary<int, HeroData> _heroData;
    public Dictionary<int, HeroData> HerosContainer
    {
        get
        {
            if (_heroData == null)
                _heroData = new Dictionary<int, HeroData>();
            return _heroData;
        }
    }

    private Dictionary<int, int> takeOfficer;

    public HeroData[] Heros
    {
        get
        {
            List<HeroData> datas = ListPool<HeroData>.Get();
            datas.AddRange(HerosContainer.Values);
            _sortHero(datas);
            HeroData[] ret = new HeroData[datas.Count];
            for (int i = 0; i < ret.Length; ++i)
            {
                ret[i] = datas[i];
            }
            ListPool<HeroData>.Release(datas);
            return ret;
        }
    }
    private void _sortHero(List<HeroData> show)
    {
        show.Sort((HeroData a, HeroData b) =>
        {
            if (a.UnLock && b.UnLock)
            {
                if (a.Level > b.Level)
                    return -1;
                else if (a.Level < b.Level)
                    return 1;
                else
                {
                    if (a.Star > b.Star)
                        return -1;
                    else if (a.Star < b.Star)
                        return 1;
                    else
                    {
                        if (a.HeroId > b.HeroId)
                            return 1;
                        else if (a.HeroId < b.HeroId)
                            return -1;
                        else
                            return 0;
                    }
                }
            }
            else if (a.UnLock && !b.UnLock)
                return -1;
            else if (!a.UnLock && b.UnLock)
                return 1;
            else
            {
                if (a.Piece > a.Piece)
                    return -1;
                else if (a.Piece < a.Piece)
                    return 1;
                else
                {
                    if (a.HeroId > b.HeroId)
                        return 1;
                    else if (a.HeroId < b.HeroId)
                        return -1;
                    else
                        return 0;
                }
            }
        });
    }


    public int GetUnLockTotemGrooveNum(int level)
    {
        int length = 0;
        for (int i = 0, lv = level; i < unLockLv.Length; ++i)
        {
            if (unLockLv[i] <= lv)
            {
                length++;
            }
        }
        return length;
    }


    public HeroData GetHeroData(int heroID)
    {
        HeroData data;
        HerosContainer.TryGetValue(heroID, out data);
        return data;
    }

    public void AddHeroData(HeroData data)
    {
        if (HerosContainer.ContainsKey(data.HeroId))
            HerosContainer[data.HeroId] = data;
        else
            HerosContainer.Add(data.HeroId, data);
    }

    public void HeroDataLoaded()
    {
        GetHeroData();
        GetTakeOfficer();
    }
    protected void GetHeroData()
    {
        LockHeroData();
    }

    protected void LockHeroData()
    {
        Hero[] heros = JsonMgr.GetSingleton().GetHeroArray(false);
        HeroData[] heroData = new HeroData[heros.Length];

        for (int i = 0; i < heros.Length; i++)
        {

            HeroData data = new HeroData(heros[i].ID);
            if (!HerosContainer.ContainsKey(data.HeroId))
                HerosContainer.Add(data.HeroId, data);
        }
    }

    public int[] GetUnLockOfficer(int rare)
    {
        HeroRare o = JsonMgr.GetSingleton().GetHeroRareByID(rare);
        return o.UnLock;
    }

    public int IsTake(int officerID)
    {
        if (takeOfficer.ContainsKey(officerID))
            return takeOfficer[officerID];
        return 0;
    }
    private void GetTakeOfficer()
    {
        for (int i = 0; i < Heros.Length; ++i)
        {
            if (Heros[i].Officer != 0)
            {
                takeOfficer.Add(Heros[i].Officer, Heros[i].HeroId);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <tip 处理没有解锁的逻辑在UI层 此处不做判断 ></tip>
    /// <param 官衔="rare"></param>
    /// <param 要装备的官职="officer"></param>
    /// <returns></returns>
    public bool TakeOfficer(HeroData heroData, int officer)
    {
        //卸载正装备的称号并清除装备状态
        if (heroData.Officer != 0)
        {
            takeOfficer.Remove(heroData.Officer);
            heroData.Officer = 0;
        }
        if (takeOfficer.ContainsKey(officer))
        {
            int takeID = takeOfficer[officer];
            HeroData hd = GetHeroData(takeID);
            if(hd != null)
                hd.Officer = 0;
            takeOfficer[officer] = heroData.HeroId;
        }
        else
        {
            takeOfficer.Add(officer, heroData.HeroId);
        }
        heroData.Officer = officer;
        heroData.ClearOfficerAttr();
        return true;
    }

    public void UnfixOfficer(HeroData heroData)
    {
        if (takeOfficer.ContainsKey(heroData.Officer))
            takeOfficer.Remove(heroData.Officer);
        heroData.Officer = 0;
        heroData.ClearOfficerAttr();
    }
    public int GetNo1()
    {
        List<HeroData> mData = new List<HeroData>(Heros);
        mData.Sort((HeroData a, HeroData b) =>
        {
            if (a.Level > b.Level)
                return -1;
            else if (a.Level < b.Level)
                return 1;
            else
            {
                if (a.Star > b.Star)
                    return -1;
                else if (a.Star < b.Star)
                    return 1;
                else
                {
                    if (a.HeroId > b.HeroId)
                        return 1;
                    else if (a.HeroId < b.HeroId)
                        return -1;
                    else
                        return 0;
                }
            }
        });
        return mData[0].HeroId;
    }
    /// <summary>
    /// 穿带气运
    /// </summary>
    /// <param name="totem"></param>
    public int TakeHeroTotem(HeroData hero, TotemData totem)
    {
        int length = hero.Totems.Length;
        for (int i = 0; i < length; i++)
        {
            if (hero.Totems[i] == "")
            {
                hero.Totems[i] = totem.md5;
                hero.ClearTotemAttr();
                return i;
            }
        }
        CanvasView.Instance.AddNotice("没有可用的气运槽");
        return int.MaxValue;
    }
}

