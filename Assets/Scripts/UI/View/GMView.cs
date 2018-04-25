using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GMView: GMViewBase
{
    private const int LEVEL = 80;
    private const int COPPER = 10000000;
    private const int POWER = 1000;
    private const int VIGOR = 100;
    private const int HONOR = 10000;
    public GameObject GmButton;
    // [HideInInspector]
    List<Button> GmButtonList = new List<Button>();


    public void Init()
    {
        var jSprite = JsonMgr.GetSingleton().GetGameManagerArray();
        for (int i = 0; i < jSprite.Length; ++i)
        {
            GameObject gmbutto = InitItemInfo();
            Text name = gmbutto.GetComponentInChildren<Text>();
            var command = jSprite[i];
            if (name.text.Length == 0)
            {
                Button gmbutt = gmbutto.GetComponent<Button>();
                gmbutt.onClick.AddListener(delegate () { this.GMorder(command.command); });
                name.text = command.desc;
                GmButtonList.Add(gmbutt);
            }
        }
    }



    public void GMorder(string _gmmo)
    {
        GMCommand _gmmos = new GMCommand();
        try { _gmmos = (GMCommand)Enum.Parse(typeof(GMCommand), _gmmo); }
        catch (Exception e)
        {
            Debug.Log("不存在的GM命令"+":"+e.ToString());
        }
       

        switch (_gmmos)
        {
            case GMCommand.modifylevel: //主公满级
                if (Role.Instance.Level == LEVEL) return;
                Role.Instance.Level = LEVEL;
                ZEventSystem.Dispatch(EventConst.UpdateData);

                break;
            case GMCommand.modifycopper: //增加铜钱
                Role.Instance.Cash += COPPER;
                if (Role.Instance.Cash > 99999999)
                {
                    Role.Instance.Cash = 99999999;
                }

                break;
            case GMCommand.clearclear:
                ClearsClose();
                break;
            case GMCommand.modifyPower: //增加体力

                if (Role.Instance.Power < POWER * 10)
                {
                    Role.Instance.Power = POWER;
                }
                break;
            case GMCommand.modifyviplevel:
                if (Role.Instance.Vip < 15)
                {
                    Role.Instance.Vip = Role.Instance.Vip + 1;
                }
                Debug.Log("VIP等级" + Role.Instance.Vip);

                break;
            case GMCommand.modifyglevel:
                Debug.Log("未实现");
                break;
            case GMCommand.modifygold:
                Role.Instance.Gold += COPPER;
                Role.Instance.LockedGold += COPPER;
                ZEventSystem.Dispatch(EventConst.UpdateData);
                if (Role.Instance.Gold > 99999999)
                {
                    Role.Instance.Gold = 99999999;
                    Role.Instance.LockedGold = 99999999;
                }

                break;
            case GMCommand.clearsection:
                Simulate();
                break;
            case GMCommand.modifydynamic:

                Debug.Log("未实现");
                break;
            case GMCommand.equip:

                Debug.Log("未实现");
                break;
            case GMCommand.modifyarena:

                Debug.Log("未实现");
                break;
            case GMCommand.modifyhonor:

                if (Role.Instance.Honor < HONOR * 10)
                {
                    Role.Instance.Honor += HONOR;
                    Debug.Log("荣誉" + Role.Instance.Honor);
                }
                break;
            case GMCommand.modifyskill:                
                Debug.Log("未实现");
                break;
            case GMCommand.modifyggzj:
                Debug.Log("未实现");
                break;
            case GMCommand.intensify:
                Debug.Log("未实现");
                break;
            case GMCommand.modifyvigor:
                Debug.Log("未实现");
                break;
            case GMCommand.divination:
                Debug.Log("未实现");
                break;
            case GMCommand.sophistication:
                Debug.Log("未实现");
                break;
        }
        GMOrder_input.text = "";
    }


    /// <summary>
    /// 实例化物品信息
    /// </summary>
    public GameObject InitItemInfo()
    {
        GameObject _item = GameObject.Instantiate(GmButton, Function_obj.transform);
        _item.transform.localScale = Vector3.one;
        _item.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _item;
    }




    Dictionary<int, int> lenelstar = new Dictionary<int, int>();
    Dictionary<int, int> lenelClose = new Dictionary<int, int>();
    int Time = 0;
    int s = 0;

    /// <summary>
    /// 解锁章节
    /// </summary>
    public void Simulate()
    {

        switch (Time)
        {
            case 0:
                s = 10101;
                for (int i = 0; i < 10; i++)
                {
                    lenelstar.Add(s + i, 3);
                }
                lenelstar.Add(10201, 3);
                BattleMgr.Instance.NowLevelId = 10202;
                break;
            case 1:
                s = 10202;
                for (int i = 0; i < 9; i++)
                {
                    lenelstar.Add(s + i, 3);
                }
                lenelstar.Add(10301, 3);
                BattleMgr.Instance.NowLevelId = 10302;
                break;
            case 2:
                return;
        }


        BattleMgr.Instance.ReceiveBattleData(lenelstar);
        Time++;
    }

    //解锁关卡
    public void ClearsClose()
    {
        if (Time >= 3) return;
        Time = 1;
        int s = 10101;
        int num = 10;
        while (true)
        {
            switch (Time)
            {
                case 1:
                    s = 10101;
                    break;
                case 2:
                    s = 10201;
                    break;
                case 3:
                    s = 10301;
                    break;
            }
            for (int i = 0; i < num; i++)
            {
                lenelClose.Add(s + i, 3);
            }
            Time++;
            if (Time == 4)
                break;
        }
        BattleMgr.Instance.NowLevelId = 10310;
        BattleMgr.Instance.ReceiveBattleData(lenelClose);
    }





    /// <summary>
    /// 主公满级
    /// </summary>
    public void RoleFullLevel()
    {
        
    }
    /// <summary>
    /// 增加铜钱
    /// </summary>
    public void AddCopper()
    {
        
        //ZEventSystem.Dispatch(EventConst.UpdateData);
        Debug.Log("铜钱" + Role.Instance.Cash);
    }
    ///// <summary>
    ///// 关卡解锁
    ///// </summary>
    //public void ClearStage()
    //{
       
    //}
    /// <summary>
    /// 增加体力
    /// </summary>
    public void AddPower()
    {
       
       
       // ZEventSystem.Dispatch(EventConst.UpdateData);
        Debug.Log("体力" + Role.Instance.Power);
    }
    /// <summary>
    /// VIP提升
    /// </summary>
    public void ElevateVIP()
    {
       
    }
    /// <summary>
    /// 武将满级
    /// </summary>
    public void GeneralFullLevel()
    {
        
    }
    /// <summary>
    /// 增加金锭
    /// </summary>
    public void AddGold()
    {
       
        

        Debug.Log("金锭" + Role.Instance.Gold);
        Debug.Log("绑金" + Role.Instance.LockedGold);
    }
    /// <summary>
    /// 章节解锁
    /// </summary>
    public void ClearLeve()
    {

    }
    /// <summary>
    /// 精力充沛
    /// </summary>
    public void AddVigor()
    {
       //暂时没有
    }
    /// <summary>
    /// 一键装备
    /// </summary>
    public void Outfit()
    {
        //暂时没有
    }
    /// <summary>
    /// 竞技场前进500
    /// </summary>
    public void Advance500()
    {
        //暂时没有
    }
    /// <summary>
    /// 增加荣誉
    /// </summary>
    public void AddHonor()
    {
        
       
    }
    /// <summary>
    /// 技能满级
    /// </summary>
    public void SkillFullLevel()
    {
        //暂时没有
    }
    /// <summary>
    /// 过关斩将前进3
    /// </summary>
    public void Advance3()
    {
        //暂时没有
    }
    /// <summary>
    /// 一键强化
    /// </summary>
    public void Intensify()
    {
        //暂时没有
    }
    /// <summary>
    /// 增加活跃
    /// </summary>
    public void AddActive()
    {
        //暂时没有
    }
    /// <summary>
    ///  一键卜卦
    /// </summary>
    public void Divination()
    {
        List<Item> itenList = new List<Item>();
        
            Item item = new Item();
            item.itemId = 1005;
            item.itemNum = 60;
            itenList.Add(item);
        ItemMgr.Instance.ServerUpdateItemList(itenList);
    }
    /// <summary>
    /// 一键洗练
    /// </summary>
    public void Sophistication()
    {
        //暂时没有
    }
}
