using System.Collections.Generic;
using UnityEngine.UI;

public class TotemMgr
{
    private static TotemMgr _totem;
    public static TotemMgr GetSingleton()
    {
        return _totem ?? (_totem = new TotemMgr());
    }

    private Dictionary<string, TotemData> _totemDic;
    public Dictionary<string,TotemData> TotemDic
    {
        get
        {
            if(_totemDic ==null)
                _totemDic = new Dictionary<string, TotemData>();
            return _totemDic;
        }
    }
    public TotemData[] Totems
    {
        get
        {
            List<TotemData> datas = ListPool<TotemData>.Get();
            datas.AddRange(TotemDic.Values);
            SortTotem(datas);
            TotemData[] ret = new TotemData[datas.Count];
            for (int i = 0; i < ret.Length; ++i)
            {
                ret[i] = datas[i];
            }
            ListPool<TotemData>.Release(datas);
            return ret;
        }     
    }

    private void SortTotem(List<TotemData> data)
    {
        data.Sort((TotemData a, TotemData b) =>
        {
            if (a.Level > b.Level)
                return 1;
            else if (a.Level < b.Level)
                return -1;
            else
                return 0;
        });
    }
    public TotemData GetTotemByID(string id)
    {
        TotemData ret;
        TotemDic.TryGetValue(id, out ret);
        return ret;
    }
    public TotemData[] GetUnDressTotem()
    {

        List<TotemData> ret = new List<TotemData>();
        foreach (var item in TotemDic)
        {
            if (item.Value.HeroID != 0)
                continue;
            ret.Add(item.Value);
        }
        return ret.ToArray();
    }

    public TotemData[] GetHeroTotem(string[] totemArray)
    {
        TotemData[] ret = new TotemData[totemArray.Length];
      
        for (int i = 0; i < totemArray.Length; ++i)
        {
            if (TotemDic.ContainsKey(totemArray[i]))
                ret[i] = TotemDic[totemArray[i]];
        }
        return ret;
    }

    public void AddTotem(TotemData data)
    {
        if (TotemDic.ContainsKey(data.md5))
            return;
        TotemDic.Add(data.md5, data);
        ZEventSystem.Dispatch(EventConst.TOTEMDATACHANGE);
    }
    public void RemoveTotem(TotemData data)
    {
        if (TotemDic.ContainsKey(data.md5))
            TotemDic.Remove(data.md5);
        ZEventSystem.Dispatch(EventConst.TOTEMDATACHANGE);
    }
    public void TakeTotem(TotemData td,HeroData hd)
    {
        if (!TotemDic.ContainsKey(td.md5))
            return;

        int res = HeroMgr.GetSingleton().TakeHeroTotem(hd,td);
        if(res == int.MaxValue)
            return;
        //服务器处理 这里模拟
        td.HeroID = hd.HeroId;
        td.Groove = res;
        hd.ClearTotemAttr();
    }

    /// <summary>
    /// 脱下气运
    /// </summary>
    /// <param name="totem"></param>
    public void TakeOffTotem(TotemData totem)
    {
        HeroData hero = HeroMgr.GetSingleton().GetHeroData(totem.HeroID);
        if (hero == null)
            return;

        hero.Totems[totem.Groove] = "";
        totem.HeroID = 0;
        totem.Groove = 0;
        hero.ClearTotemAttr();
    }
}
