using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum SectionState
{
    Pass,
    Now,
    Not
}

public class BattleView : BattleViewBase
{

   
    [HideInInspector]
    public Dictionary<int, LevelViewbs> Genlevel = new Dictionary<int, LevelViewbs>();
    [HideInInspector]
    public List<List<GameObject>> ChapterDic = new List<List<GameObject>>();
    [HideInInspector]
    public GameObject battleName;
    [HideInInspector]
    public ButtonScale Lock_4 = null;
    [HideInInspector]
    public ButtonScale Lock_8;
    [HideInInspector]
    public ButtonScale Lock_12;

    private Vector2 EliteHistory = Vector2.zero; // 精英章节位置
    private Vector2 CommonHistory = Vector2.zero;  //普通章节位置

    public string[] KeyChapter
    {
        get
        {
            string[] key = new string[5];
            string keys = "";
            switch (BattleMgr.Instance.SectionKey)
            {
                case 0:
                    keys = BattleMgr.Instance.NowLevelId.ToString();
                    break;
                case 1:
                    keys = BattleMgr.Instance.EliteLevelId.ToString();
                    break;
                case 2:
                    keys = BattleMgr.Instance.EpicLevelId.ToString();
                    break;
            }
            for (int i = 0; i < keys.Length; i++)
            {
                key[i] = keys[i].ToString();
            }
            return key;
        }

    }
    [HideInInspector]
    public SectionState isPass
    {
        get
        {
            SectionState Pass = SectionState.Not;
            int idxs = BattleMgr.Instance.SectionChapter;
            for (int idx = 0; idx < BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][idxs].GenStage.Count; idx++)
            {

                if (BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][idxs].GenStage[idx].StarCount != 0)
                {
                    if (idx == BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][idxs].GenStage.Count - 1)
                    {
                        Pass = SectionState.Pass;
                        return Pass;
                    }
                    else
                    {
                        Pass = SectionState.Now;
                    }
                }
                if (Pass != SectionState.Now)
                {
                    if (BattleMgr.Instance.SectionChapter - 1 >= 0 && idx == BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][idxs].GenStage.Count - 1)
                    {
                        if (BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][idxs - 1].GenStage[idx].StarCount != 0)
                        {
                            Pass = SectionState.Now;
                        }
                    }
                }

            }
            if (Pass == SectionState.Not && BattleMgr.Instance.SectionChapter == 0)
            {
                Pass = SectionState.Now;
            }
            return Pass;
        }
    }

    public void _Init()
    {
        if (Lock_4==null)
        {
            Lock_4 = Lock_4_obj.GetComponent<ButtonScale>();
            Lock_8 = Lock_8_obj.GetComponent<ButtonScale>();
            Lock_12 = Lock_12_obj.GetComponent<ButtonScale>();
        }
        LevelViewbs levelView = null;
        GameObject name = null;
        string levelName = "";
        for (int idx = 0; idx < BattleMgr.Instance.Genlevel.Count; idx++)
        {
            if(idx > 0) return;
            switch (idx)
            {
                case 0:
                    name = commonBack_obj;
                    break;
                case 1:
                    name = eliteBack_obj;
                    break;
                case 2:
                    name = epicBack_obj;
                    break;
            }
            ChapterDic.Add(new List<GameObject>());
            for (int idx1 = 0; idx1 < BattleMgr.Instance.Genlevel[idx].Count; idx1++)
            {
                string chapter = string.Format("level_{0}", ((idx + 1) * 100 + (idx1 + 1)));
                ChapterDic[idx].Add(name.transform.Find(chapter).gameObject);
                for (int idx2 = 0; idx2 < BattleMgr.Instance.Genlevel[idx][idx1].GenStage.Count; idx2++)
                {
                    levelName = string.Format("stage_{0}", BattleMgr.Instance.Genlevel[idx][idx1].GenStage[idx2].LevelID);
                    string level = String.Concat(name.name, "/", chapter, "/level/", levelName);
                    levelView = commonBack_obj.transform.parent.transform.Find(level).GetComponent<LevelViewbs>();
                    levelView.isZeki = BattleMgr.Instance.Genlevel[idx][idx1].GenStage[idx2].isZeki;
                    levelView.Init();
                    Genlevel.Add(BattleMgr.Instance.Genlevel[idx][idx1].GenStage[idx2].LevelID, levelView);
                    if (BattleMgr.Instance.Genlevel[idx][idx1].GenStage[idx2].StarCount != 0 || (idx == 0 && idx1 == 0 && idx2 == 0))
                    {
                        Genlevel[BattleMgr.Instance.Genlevel[idx][idx1].GenStage[idx2].LevelID].CloseBtn.enabled = true;
                        if (idx2 + 1 < BattleMgr.Instance.Genlevel[idx][idx1].GenStage.Count)
                        {
                            switch (idx)
                            {
                                case 0:
                                    if (BattleMgr.Instance.NowLevelId >= 10301) break;
                                        BattleMgr.Instance.NowLevelId = BattleMgr.Instance.Genlevel[idx][idx1].GenStage[idx2 + 1].LevelID;
                                    elite_tog.interactable = true;
                                    break;
                                case 1:
                                    BattleMgr.Instance.EliteLevelId = BattleMgr.Instance.Genlevel[idx][idx1].GenStage[idx2 + 1].LevelID;
                                    epic_tog.interactable = true;
                                    break;
                                case 2:
                                    BattleMgr.Instance.EpicLevelId = BattleMgr.Instance.Genlevel[idx][idx1].GenStage[idx2 + 1].LevelID;
                                    break;
                            }
                        }
                        else
                        {
                            if (idx1 + 1 >= BattleMgr.Instance.Genlevel[idx].Count) break; ;
                            if (BattleMgr.Instance.Genlevel[idx][idx1 + 1].GenStage.Count == 0 ) break;
                            switch (idx)
                            {
                                case 0:
                                    for (int i = 0; i < BattleMgr.Instance.Genlevel[idx][idx1 + 1].GenStage.Count; i++)
                                    {
                                        BattleMgr.Instance.NowLevelId = BattleMgr.Instance.Genlevel[idx][idx1 + 1].GenStage[0].LevelID;
                                        if (BattleMgr.Instance.Genlevel[idx][idx1 + 1].GenStage[i].StarCount != 0)
                                         {
                                        BattleMgr.Instance.NowLevelId = BattleMgr.Instance.Genlevel[idx][idx1 + 1].GenStage[i].LevelID;
                                        }
                                   }
                                    break;
                                case 1:
                                    BattleMgr.Instance.EliteLevelId = BattleMgr.Instance.Genlevel[idx][idx1+1].GenStage[0].LevelID;
                                    break;
                                case 2:
                                    BattleMgr.Instance.EpicLevelId = BattleMgr.Instance.Genlevel[idx][idx1+1].GenStage[0].LevelID;
                                    break;
                            }
                        }
                       
                        ChapterDic[idx][idx1].SetActive(true);
                       // battleName = ChapterDic[idx][idx1].transform.Find("battleName").gameObject;

                    }
                    else
                    {
                        Genlevel[BattleMgr.Instance.Genlevel[idx][idx1].GenStage[idx2].LevelID].CloseBtn.enabled = false;
                    }

                }
            }

        }
    }
    /// <summary>
    /// 关闭战役界面
    /// </summary>
    public void BattleBtn()
    {
        UIFace.GetSingleton().Close(UIID.Battle);
        //Transcript_obj.transform.parent.gameObject.SetActive(false);
    }
    /// <summary>
    /// 点击普通副本切换按钮
    /// </summary>
    /// <param name="isOff"></param>
    public void Common()
    {
        BattleMgr.Instance.SectionKey = 0;
        commonBack_obj.SetActive(true);
        epicBack_obj.SetActive(false);
        eliteBack_obj.SetActive(false);

        ZEventSystem.Dispatch(EventConst.OpLevel, int.Parse(KeyChapter[0]) - 1, int.Parse(KeyChapter[1] + KeyChapter[2]) - 1, 0);// 执行从其他界面进去章节
    }
    /// <summary>
    /// 点击精英切换按钮
    /// </summary>
    /// <param name="isOff"></param>
    public void Elite()
    {
      //  BattleMgr.Instance.SectionKey = 1;
      //  commonBack_obj.SetActive(false);
      //  epicBack_obj.SetActive(false);
     //   eliteBack_obj.SetActive(true);
       // ZEventSystem.Dispatch(EventConst.OpLevel, int.Parse(KeyChapter[0]) - 1, int.Parse(KeyChapter[1] + KeyChapter[2]) - 1, 0);// 执行从其他界面进去章节

    }
    /// <summary>
    /// 点击史诗切换按钮
    /// </summary>
    /// <param name="isOff"></param>
    public void Epic()
    {
    //    BattleMgr.Instance.SectionKey = 2;
     //   commonBack_obj.SetActive(false);
     //   epicBack_obj.SetActive(true);
     //   eliteBack_obj.SetActive(false);
    //    ZEventSystem.Dispatch(EventConst.OpLevel, int.Parse(KeyChapter[0]) - 1, int.Parse(KeyChapter[1] + KeyChapter[2]) - 1, 0);// 执行从其他界面进去章节
    }
    /// <summary>
    /// 点击左或右箭头时
    /// </summary>
    /// <param name="isCut"></param>
    public void CutSectionName(bool isCut)
    {
        if (isCut)
        {
            BattleMgr.Instance.SectionChapter++;
            if (BattleMgr.Instance.SectionChapter >= BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey].Count - 1)
                BattleMgr.Instance.SectionChapter = BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey].Count - 1;
                ZEventSystem.Dispatch(EventConst.OpLevel, int.Parse(KeyChapter[0]) - 1, BattleMgr.Instance.SectionChapter, 0);// 执行从其他界面进去章节
        }
        else
        {
            BattleMgr.Instance.SectionChapter--;
                if (BattleMgr.Instance.SectionChapter < 0) BattleMgr.Instance.SectionChapter = 0;
                ZEventSystem.Dispatch(EventConst.OpLevel, int.Parse(KeyChapter[0]) - 1, BattleMgr.Instance.SectionChapter, 0);// 执行从其他界面进去章节
        }
    }
    /// <summary>
    /// 左箭头的开关
    /// </summary>
    /// <param name="isshow"></param>
    public void QueenShow(bool isshow)
    {
        if (isshow)
            queen_btn.gameObject.SetActive(true);
        else
            queen_btn.gameObject.SetActive(false);
    }
    /// <summary>
    /// 右箭头的开关
    /// </summary>
    /// <param name="isshow"></param>
    public void FrontShow(bool isshow)
    {
        if (isshow)
            front_btn.gameObject.SetActive(true);
        else
            front_btn.gameObject.SetActive(false);
    }

    /// <summary>
    /// 点击进入简介界面
    /// </summary>
    /// <param name="stage"></param>
    public void BattleInt(int _levelId,BattleCtrl open)
    {
        BattleMgr.Instance.LevelID = _levelId;
        battleName.SetActive(false);  //本章节名字关闭
        Starlevel_obj.SetActive(false); //奖励宝箱关闭
        arroParent_obj.SetActive(false); //交战标记关闭
        common_tog.gameObject.SetActive(false); //
        elite_tog.gameObject.SetActive(false);//三种副本的选择按钮关闭
        epic_tog.gameObject.SetActive(false);//  
        queen_btn.gameObject.SetActive(false); //关闭左箭头
        front_btn.gameObject.SetActive(false);//关闭右箭头
        open.OpenCustoms();
    }
    
    /// <summary>
    /// 关闭简介页面
    /// </summary>
    public void CloseCustoms()
    {
        battleName.SetActive(true);
        Starlevel_obj.SetActive(true);
        arroParent_obj.SetActive(true);
        common_tog.gameObject.SetActive(true);
        elite_tog.gameObject.SetActive(true);
        epic_tog.gameObject.SetActive(true);
       
       
        if (BattleMgr.Instance.SectionChapter == 0 && isPass == SectionState.Pass)
        {
            QueenShow(false);
            FrontShow(true);
        }
        else if (BattleMgr.Instance.SectionChapter == 0)
        {
            QueenShow(false);
            FrontShow(false);
        }
        else if (BattleMgr.Instance.SectionChapter != 0 && isPass != SectionState.Pass)
        {
            QueenShow(true);
            FrontShow(false);
        }
        else
        {
            QueenShow(true);
            FrontShow(true);
        }
    }
    /// <summary>
    /// 某一章节通关后执行
    /// </summary>
    /// <param name="Section">章节编号</param>  
    /// <param name="_tey">类型编号</param>
    public void SectionPass(int _tey, int Section)
    {
        if (Section > Genlevel.Count) return;
        if (Section == 1)
        {
            switch (_tey)
            {
                case 0:
                    elite_tog.interactable = true;
                    break;
                case 1:
                    epic_tog.interactable = true;
                    break;
            }
        }

        ChapterDic[_tey][Section].gameObject.SetActive(true);
        BattleMgr.Instance.SectionKey = _tey;
        BattleMgr.Instance.SectionChapter = Section;
        InitStar(_tey, Section);
    }

    /// <summary>
    /// 通关某一阶段后
    /// </summary>
    /// <param name="s"></param>
    public void PassStage(int s)
    {
        int num = 0;
        int LevelId = 0;
        LevelViews levelvise = null;
        string[] ids = new string[5];
        for (int i = 0; i < BattleMgr.Instance.LevelID.ToString().Length; i++)
        {
            ids[i] = BattleMgr.Instance.LevelID.ToString()[i].ToString();
        }
        int typr = int.Parse(ids[0]) - 1;
        int Chapter = int.Parse(ids[1] + ids[2]) - 1;

        for (int i = 0; i < BattleMgr.Instance.Genlevel[typr][Chapter].GenStage.Count; i++)
        {
            if (BattleMgr.Instance.Genlevel[typr][Chapter].GenStage[i].LevelID == BattleMgr.Instance.LevelID)
            {
                num = BattleMgr.Instance.Genlevel[typr][Chapter].GenStage[i].StarCount;
                levelvise = BattleMgr.Instance.Genlevel[typr][Chapter].GenStage[i];
                break;
            }

        }
        if (levelvise == null) return;
        if (levelvise.ResidueTime > 0)
        {
            levelvise.ResidueTime--;

        }
        if (levelvise.StarCount < 3)
        {
            if (levelvise.isZeki)
            {
                if (levelvise.StarCount < 3 && s > levelvise.StarCount)
                {
                    s = s - levelvise.StarCount;
                }
                else
                {
                  //  arroParent_obj.SetActive(true);
                   // CustomsPass_obj.SetActive(false);
                    CloseCustoms();
                    return;
                }
                levelvise.StarCount = s + levelvise.StarCount;
                if (levelvise.StarCount > 3) levelvise.StarCount = 3;
                Genlevel[BattleMgr.Instance.LevelID].transform.Find("pentagon").GetComponent<StarView>().SetStar(levelvise.StarCount);

                BattleMgr.Instance.Genlevel[int.Parse(ids[0].ToString()) - 1][int.Parse((ids[1].ToString() + ids[2].ToString())) - 1].StarCount += s;
                InitStar(int.Parse(ids[0].ToString()) - 1, int.Parse((ids[1].ToString() + ids[2].ToString())) - 1);

            }
            else
            {
                Genlevel[BattleMgr.Instance.LevelID].CloseBtn.enabled = false;
            }

            if (isPass != SectionState.Pass && num == 0)
            {
                    switch (BattleMgr.Instance.SectionKey)
                    {
                        case 0:
                            BattleMgr.Instance.NowLevelId = BattleMgr.Instance.NowLevelId + 1;
                            Indicate(Genlevel[BattleMgr.Instance.NowLevelId]);
                            Genlevel[BattleMgr.Instance.NowLevelId].CloseBtn.enabled = true;
                            break;
                        case 1:
                            BattleMgr.Instance.EliteLevelId = BattleMgr.Instance.EliteLevelId + 1;
                            Indicate(Genlevel[BattleMgr.Instance.EliteLevelId]);
                            Genlevel[BattleMgr.Instance.EliteLevelId].CloseBtn.enabled = true;
                            break;
                        case 2:
                            BattleMgr.Instance.EpicLevelId = BattleMgr.Instance.EpicLevelId + 1;
                            Indicate(Genlevel[BattleMgr.Instance.EpicLevelId]);
                            Genlevel[BattleMgr.Instance.EpicLevelId].CloseBtn.enabled = true;
                            break;
                    }
            }
            else if(num == 0)
            {
                if (int.Parse(KeyChapter[1] + KeyChapter[2]) + 1 > BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey].Count) return;

                SectionPass(int.Parse(KeyChapter[0]) - 1, int.Parse(KeyChapter[1] + KeyChapter[2]));
                ZEventSystem.Dispatch(EventConst.OpLevel, int.Parse(KeyChapter[0]) - 1, int.Parse(KeyChapter[1] + KeyChapter[2]), 0);// 执行从其他界面进去章节
                LevelId = int.Parse(String.Concat(((int.Parse(KeyChapter[0])) * 100 + (int.Parse(KeyChapter[1] + KeyChapter[2]) + 1)).ToString(), "01"));
                switch (int.Parse(KeyChapter[0]) - 1)
                {
                    case 0:
                        BattleMgr.Instance.NowLevelId = LevelId;
                        break;
                    case 1:
                        BattleMgr.Instance.EliteLevelId = LevelId;
                        break;
                    case 2:
                        BattleMgr.Instance.EpicLevelId = LevelId;
                        break;
                }
                Genlevel[LevelId].CloseBtn.enabled = true;
                Indicate(Genlevel[LevelId]);
            }
          //  arroParent_obj.SetActive(true);
            //CustomsPass_obj.SetActive(false);
            CloseCustoms();
        }
        else
        {
            CloseCustoms();
           // CustomsPass_obj.SetActive(false);
        }
    }

    /// <summary>
    /// 更新奖励星条
    /// </summary>
    /// <param name="_type">类型</param>
    /// <param name="_section">章节</param>
    public void InitStar(int _type, int _section)
    {
        if (BattleMgr.Instance.Genlevel[_type][_section].StarCount > 12)
            BattleMgr.Instance.Genlevel[_type][_section].StarCount = 12;

        double sum = (double)(1.0 / 12);
        double count = BattleMgr.Instance.Genlevel[_type][_section].StarCount * sum;
        if (Slider_slider.value <= 1)
        {
            Slider_slider.value = (float)count;
        }

        if (BattleMgr.Instance.Genlevel[_type][_section].StarCount == 0)
        {
            Slider_slider.value = 0;
            Lock_4_obj.SetActive(true);
            Clear_4_btn.gameObject.SetActive(false);
            Open_4_obj.gameObject.SetActive(false);
            Lock_8_obj.gameObject.SetActive(true);
            Clear_8_btn.gameObject.SetActive(false);
            Open_8_obj.gameObject.SetActive(false);
            Lock_12_obj.gameObject.SetActive(true);
            Clear_12_btn.gameObject.SetActive(false);
            Open_12_obj.gameObject.SetActive(false);
        }
        BattleData data = JsonMgr.GetSingleton().GetChapter((BattleMgr.Instance.SectionKey + 1) * 100 + (BattleMgr.Instance.SectionChapter + 1));
        Vector2Int[] award = {  new Vector2Int(2000,1000+ _section), new Vector2Int(2001, 10000+ _section) };
        Vector2Int[] award1 = { new Vector2Int(2001, 1000+ _section), new Vector2Int(2002, 10000+ _section) };
        Vector2Int[] award2 = { new Vector2Int(2003, 1000+ _section), new Vector2Int(2004, 10000+ _section) };


        if (Lock_4.rect == null)
        {
            Lock_4.rect = Lock_4_obj.transform.parent.GetComponent<RectTransform>();
            Lock_8.rect = Lock_8_obj.transform.parent.GetComponent<RectTransform>();
            Lock_12.rect = Lock_12_obj.transform.parent.GetComponent<RectTransform>();
        }
        Lock_4.ItemData = award;
        Lock_4.isShow = true;
        Lock_8.ItemData = award1;
        Lock_8.isShow = true;
        Lock_12.ItemData = award2;
        Lock_12.isShow = true;
        if (BattleMgr.Instance.Genlevel[_type][_section].StarCount >= 4 && BattleMgr.Instance.Genlevel[_type][_section].isGet_4 == false)
        {
            Lock_4_obj.SetActive(false);
            Clear_4_btn.gameObject.SetActive(true);
        }
        else if (BattleMgr.Instance.Genlevel[_type][_section].isGet_4 != false)
        {
            Clear_4_btn.gameObject.SetActive(false);
            Lock_4_obj.SetActive(false);
            Open_4_obj.SetActive(true);
        }
        else if (BattleMgr.Instance.Genlevel[_type][_section].isGet_4 == false)
        {
            Clear_4_btn.gameObject.SetActive(false);
            Lock_4_obj.SetActive(true);
            Open_4_obj.SetActive(false);
        }

        if (BattleMgr.Instance.Genlevel[_type][_section].StarCount >= 8 && BattleMgr.Instance.Genlevel[_type][_section].isGet_8 == false)
        {
            Lock_8_obj.gameObject.SetActive(false);
            Clear_8_btn.gameObject.SetActive(true);
        }
        else if (BattleMgr.Instance.Genlevel[_type][_section].isGet_8 != false)
        {
            Lock_8_obj.gameObject.SetActive(false);
            Clear_8_btn.gameObject.SetActive(false);
            Open_8_obj.SetActive(true);

        }
        else if (BattleMgr.Instance.Genlevel[_type][_section].isGet_8 == false)
        {
            Clear_8_btn.gameObject.SetActive(false);
            Lock_8_obj.SetActive(true);
            Open_8_obj.SetActive(false);
        }

        if (BattleMgr.Instance.Genlevel[_type][_section].StarCount == 12 && BattleMgr.Instance.Genlevel[_type][_section].isGet_12 == false)
        {
            Lock_12_obj.gameObject.SetActive(false);
            Clear_12_btn.gameObject.SetActive(true);
        }
        else if (BattleMgr.Instance.Genlevel[_type][_section].isGet_12 != false)
        {
            Lock_12_obj.gameObject.SetActive(false);
            Clear_12_btn.gameObject.SetActive(false);
            Open_12_obj.SetActive(true);
        }
        else if (BattleMgr.Instance.Genlevel[_type][_section].isGet_12 == false)
        {
            Clear_12_btn.gameObject.SetActive(false);
            Lock_12_obj.SetActive(true);
            Open_12_obj.SetActive(false);
        }

        Existing_txt.text = BattleMgr.Instance.Genlevel[_type][_section].StarCount.ToString();
    }
    /// <summary>
    /// 开启宝箱
    /// </summary>
    /// <param name="senction"></param>
    public void OpenTreasure(string name)
    {

        BattleData data = JsonMgr.GetSingleton().GetChapter((BattleMgr.Instance.SectionKey + 1) * 100 + (BattleMgr.Instance.SectionChapter + 1));
        Debug.Log(data.ChapterId);
        if (BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].StarCount >= 4 && BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].isGet_4 == false && name == "Clear_4")
        {
            Clear_4_btn.gameObject.SetActive(false);
            Open_4_obj.SetActive(true);
            BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].isGet_4 = true;
            return;
        }
        if (BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].StarCount >= 8 && BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].isGet_8 == false && name == "Clear_8")
        {
            Clear_8_btn.gameObject.SetActive(false);
            Open_8_obj.SetActive(true);

            BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].isGet_8 = true;
            return;
        }

        if (BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].StarCount >= 12 && BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].isGet_12 == false && name == "Clear_12")
        {
            Clear_12_btn.gameObject.SetActive(false);
            Open_12_obj.SetActive(true);
            BattleMgr.Instance.Genlevel[BattleMgr.Instance.SectionKey][BattleMgr.Instance.SectionChapter].isGet_12 = true;
            return;
        }

    }

    /// <summary>
    /// 交战标记
    /// </summary>
    /// <param name="section"></param>
    public void Indicate(LevelViewbs section)
    {
        if (section.isZeki)
        {
            arroParent_obj.transform.position = new Vector3(section.gameObject.transform.position.x, section.gameObject.transform.position.y + 1f, section.gameObject.transform.position.z);
        }
        else
        {
            arroParent_obj.transform.position = new Vector3(section.gameObject.transform.position.x, section.gameObject.transform.position.y + 0.8f, section.gameObject.transform.position.z);
        }

    }

    /// <summary>
    /// 期待
    /// </summary>
    public void Expect()
    {
        expect_img.color = new Color(expect_img.color.r, expect_img.color.g, expect_img.color.b, 1);
        ProcessCtrl.Instance.GoCoroutine("ScaleCtrl", ExpectShrink());

    }
    IEnumerator ExpectShrink()
    {
        float tine = 0;
        while (true)
        {
            if (expect_img.color.a >= 0)
            {
                tine = Time.deltaTime * 0.5f;
                expect_img.color -= new Color(0, 0, 0, tine);
            }
            else
            {
                ProcessCtrl.Instance.GoCoroutine("ScaleCtrl", ExpectShrink());
            }
            yield return 0.8f;
        }
    }


}
