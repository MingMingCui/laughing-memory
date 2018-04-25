//using MessagePack;
using Msg.HeroMsg;
using Msg.ToTemMsg;
using UnityEngine;

public class HeroModule : ModuleBase
{
    private HeroModule(){}
    private static HeroModule _heroModule;

    public static HeroModule GetSiongleton()
    {
        return _heroModule ?? (_heroModule = new HeroModule());
    }

    public override void Register()
    {
        RegistEvent((int)Msg.ServerMsgId.S2C_LOADHERODATA, OnLoadHeroData);
        RegistEvent((int)Msg.ServerMsgId.S2C_LOADEQUIPDATA, OnLoadEquipData);
        RegistEvent((int)Msg.ServerMsgId.S2C_LOADTOTEMDATA, OnLoadTotemData);
    }

    public void OnHeroStarUp(int heroId)
    {
        
    } 
                      
    public void OnLoadHeroData(ServerMsgObj msg)
    {
        string server = msg.Msg;
        HeroMsgCollect hm = JsonUtility.FromJson<HeroMsgCollect>(server);
        HeroData[] dataArray = new HeroData[hm.heros.Length];
        for (int i = 0; i < dataArray.Length; i++)
        {
            dataArray[i] = new HeroData(hm.heros[i]);
            HeroMgr.GetSingleton().AddHeroData(dataArray[i]);
        }
        HeroMgr.GetSingleton().HeroDataLoaded();
    }
    public void OnLoadEquipData(ServerMsgObj msg)
    {
        string server = msg.Msg;
        EquipMsgCollect em = JsonUtility.FromJson<EquipMsgCollect>(server);
        EquipData[] data = new EquipData[em.equips.Length];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new EquipData(em.equips[i]);
            EquipMgr.GetSingleton().AddEquip(data[i]);
        }
    }
    public void OnLoadTotemData(ServerMsgObj msg)
    {
        string server = msg.Msg;
        TotemMsgCollect tc = JsonUtility.FromJson<TotemMsgCollect>(server);
        TotemData[] data = new TotemData[tc.totems.Length];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new TotemData(tc.totems[i]);
            TotemMgr.GetSingleton().AddTotem(data[i]);
        }
    }
}
