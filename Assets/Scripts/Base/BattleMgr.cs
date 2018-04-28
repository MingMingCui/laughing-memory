using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System;

public class SectionView
{
    public int SectionId = 0;//章节ID
    public int SumStarCount = 12; //章奖励总星数
    public int StarCount;  //章奖励现有星数
    public bool isGet_4 = false;
    public bool isGet_8 = false;
    public bool isGet_12 = false;
    public List<LevelViews> GenStage = new List<LevelViews>(); //普通关卡数
}

public class LevelViews
{
    public bool isZeki = false;
    public int LevelID = 0;//关卡ID
    public int ResidueTime = 0;//剩余次数
    public int StarCount = 0; //获得星星数量 
}



public class BattleMgr : Singleton<BattleMgr>
{
    public List<List<SectionView>> Genlevel = new List<List<SectionView>>(); //所有章节数
    public int NowLevelId = 10101;
    public int EliteLevelId = 20101;
    public int EpicLevelId = 30101;
    public int SectionChapter = 0; //当前章节
    public int SectionKey = 0;   //当前类型
    public int LevelID = 0;     //当前章节
    public bool isOff = false;
    /// <summary>
    /// 初始化副本
    /// </summary>
    public void _init()
    {
        if (Genlevel.Count > 0) return;
        int levelId = 0;
        List<List<List<LevelData>>> levelData = JsonMgr.GetSingleton().GetLevelData();
        for (int idx = 0; idx < levelData.Count; idx++)
        {
            Genlevel.Add(new List<SectionView>());
            for (int idx1 = 0; idx1 < levelData[idx].Count; idx1++)
            {
                Genlevel[idx].Add(new SectionView());
                Genlevel[idx][idx1].SectionId = (idx + 1) * 100 + (idx1 + 1);
                for (int idx2 = 0; idx2 < levelData[idx][idx1].Count; idx2++)
                {
                    if (idx2 < 9)
                        levelId = int.Parse(String.Concat(((idx + 1) * 100 + (idx1 + 1)).ToString(), "0", (idx2 + 1)));
                    else
                        levelId = int.Parse(String.Concat(((idx + 1) * 100 + (idx1 + 1)).ToString(), (idx2 + 1)));
                    if (levelData[idx][idx1].Count > Genlevel[idx][idx1].GenStage.Count)
                    {
                        Genlevel[idx][idx1].GenStage.Add(new LevelViews());
                        Genlevel[idx][idx1].GenStage[idx2].LevelID = levelId;
                        Genlevel[idx][idx1].GenStage[idx2].isZeki = levelData[idx][idx1][idx2].type;
                        Genlevel[idx][idx1].GenStage[idx2].ResidueTime = levelData[idx][idx1][idx2].times;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 通过ID获取某一关卡星数
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public int GetStar(int _id)
    {
        int star = 0;
        for (int idx = 0; idx < Genlevel.Count; idx++)
        {
            for (int idx1 = 0; idx1 < Genlevel[idx].Count; idx1++)
            {
                for (int idx2 = 0; idx2 < Genlevel[idx][idx1].GenStage.Count; idx2++)
                {
                    if (Genlevel[idx][idx1].GenStage[idx2].LevelID == _id)
                    {
                        star = Genlevel[idx][idx1].GenStage[idx2].StarCount;
                        return star;
                    }

                }
            }

        }
        EDebug.LogErrorFormat("Can't find itemid:{0} in Battlemgr.GetStar", _id);
        return 0;
    }


    public void ReceiveBattleData(Dictionary<int, int> _leveData)
    {
        _init();
        string[] key = new string[5];
        foreach (var star in _leveData)
        {
            for (int i = 0; i < star.Key.ToString().Length; i++)
            {
                key[i] = star.Key.ToString()[i].ToString();
            }
            for (int idx = 0; idx < Genlevel[int.Parse(key[0]) - 1][int.Parse(key[1] + key[2]) - 1].GenStage.Count; idx++)
            {
                if (Genlevel[int.Parse(key[0]) - 1][int.Parse(key[1] + key[2]) - 1].GenStage[idx].LevelID == star.Key)
                {
                    if (Genlevel[int.Parse(key[0]) - 1][int.Parse(key[1] + key[2]) - 1].GenStage[idx].isZeki)
                    {
                        Genlevel[int.Parse(key[0]) - 1][int.Parse(key[1] + key[2]) - 1].StarCount += star.Value;
                        if (Genlevel[int.Parse(key[0]) - 1][int.Parse(key[1] + key[2]) - 1].StarCount > 12)
                        {
                            Genlevel[int.Parse(key[0]) - 1][int.Parse(key[1] + key[2]) - 1].StarCount = 12;
                        }
                        Genlevel[int.Parse(key[0]) - 1][int.Parse(key[1] + key[2]) - 1].GenStage[idx].StarCount = star.Value;
                    }

                }
            }
        }
    }

    /// <summary>
    /// 进去战斗
    /// </summary>
    public void BeginCombat(int id)
    {

        List<Vector2Int> stubData = Role.Instance.GetStubData(StubType.PVE);

        if (stubData.Count < StubView.StubRange)  //是否布阵
        {
            TipCtrl ctrl = (TipCtrl)UIFace.GetSingleton().Open(UIID.Tip, 3);
            ctrl.SetHandler(
                delegate () {
                    UIFace.GetSingleton().Close(UIID.Tip);
                    UIFace.GetSingleton().Open(UIID.Stub);
                },
                delegate ()
                {
                    UIFace.GetSingleton().Close(UIID.Tip);
                    StartFight(id);
                });
            return;
        }
        StartFight(id);
    }

    public void StartFight(int id)
    {
        Client.Instance.Send(Msg.ServerMsgId.CCMD_BATTLE_BEGIN, null);

        List<FightUnit> ownFighter = new List<FightUnit>();
        List<Vector2Int> stubData = Role.Instance.GetStubData(StubType.PVE);

        List<FightUnit> enemy = new List<FightUnit>();
        List<FightUnit> enemy1 = new List<FightUnit>();
        List<FightUnit> enemy2 = new List<FightUnit>();
        for (int idx = 0; idx < stubData.Count; ++idx)
        {
            JsonData.Hero heroJson = JsonMgr.GetSingleton().GetHeroByID(stubData[idx].y);
            FightUnit fighter = new FightUnit(HeroMgr.GetSingleton().GetHeroData(heroJson.ID), stubData[idx].x, false);
            ownFighter.Add(fighter);
        }

        LevelData leveldata = JsonMgr.GetSingleton().GetLevel(id);
        var monsters = JsonMgr.GetSingleton().GetMonstersArray();
        for (int idx1 = 0; idx1 < monsters.Length; idx1++)
        {
            for (int idx2 = 0; idx2 < monsters[idx1].monster.Length; idx2++)
            {
                switch (idx1)
                {
                    case 0:
                        FightUnit fighter = new FightUnit(JsonMgr.GetSingleton().GetMonsterByID(monsters[idx1].monster[idx2]), monsters[idx1].position[idx2], true);
                        enemy.Add(fighter);
                        break;
                    case 1:
                        FightUnit fighter1 = new FightUnit(JsonMgr.GetSingleton().GetMonsterByID(monsters[idx1].monster[idx2]), monsters[idx1].position[idx2], true);
                        enemy1.Add(fighter1);
                        break;
                    case 2:
                        FightUnit fighter2 = new FightUnit(JsonMgr.GetSingleton().GetMonsterByID(monsters[idx1].monster[idx2]), monsters[idx1].position[idx2], true);
                        enemy2.Add(fighter2);
                        break;
                }
            }
        }
        List<List<FightUnit>> enemyFighter = new List<List<FightUnit>>()
        {
           enemy,enemy1,enemy2
        };
        FightLogic.Instance.Clear();
        FightLogic.Instance.SetFightUnit(ownFighter, enemyFighter);

        switch (SectionKey)
        {
            case 0:
                for (int i = 0; i < Genlevel[SectionKey][SectionChapter].GenStage.Count; i++)
                {
                    if (Genlevel[SectionKey][SectionChapter].GenStage[i].LevelID == id)
                    {
                        if (Genlevel[SectionKey][SectionChapter].GenStage[i].ResidueTime > 0)
                        {
                            Genlevel[SectionKey][SectionChapter].GenStage[i].ResidueTime--;
                            isOff = true;
                            SceneMgr.Instance.LoadScene("Game");
                        }
                        else
                        {
                            Debug.Log("次数不足");
                            break;
                        }
                    }
                }

                break;
            case 1:
                for (int i = 0; i < Genlevel[SectionKey][SectionChapter].GenStage.Count; i++)
                {
                    if (Genlevel[SectionKey][SectionChapter].GenStage[i].LevelID == id)
                    {
                        if (Genlevel[SectionKey][SectionChapter].GenStage[i].ResidueTime > 0)
                        {
                            Genlevel[SectionKey][SectionChapter].GenStage[i].ResidueTime--;
                            isOff = true;
                            SceneMgr.Instance.LoadScene("Game");
                        }
                        else
                        {
                            Debug.Log("次数不足");
                            break;
                        }
                    }
                }
                break;
            case 2:
                for (int i = 0; i < Genlevel[SectionKey][SectionChapter].GenStage.Count; i++)
                {
                    if (Genlevel[SectionKey][SectionChapter].GenStage[i].LevelID == id)
                    {
                        if (Genlevel[SectionKey][SectionChapter].GenStage[i].ResidueTime > 0)
                        {
                            Genlevel[SectionKey][SectionChapter].GenStage[i].ResidueTime--;
                            isOff = true;
                            SceneMgr.Instance.LoadScene("Game");
                        }
                        else
                        {
                            Debug.Log("次数不足");
                            break;
                        }
                    }

                }
                break;
        }
    }

    public void StartPvp(uint roleid)
    {
        //向服务器请求开始PVP
    }

    public void OnStartPvp()
    {
    }

}
