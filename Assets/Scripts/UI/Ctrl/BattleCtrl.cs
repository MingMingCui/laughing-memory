using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCtrl : UICtrlBase<BattleView>
{

    Dictionary<int, int> lenelstar = new Dictionary<int, int>();
    public override void OnInit()
    {

        base.OnInit();
        if (BattleMgr.Instance.Genlevel.Count == 0)
        {
            Simulate();
            BattleMgr.Instance.ReceiveBattleData(lenelstar);
        }
        this.mView._Init();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        _InitChapter(true);
        ZEventSystem.Dispatch(EventConst.OpLevel, int.Parse(this.mView.KeyChapter[0]) - 1, int.Parse(this.mView.KeyChapter[1] + this.mView.KeyChapter[2]) - 1, 0);// 执行从其他界面进去章节

    }

    public void Simulate()
    {
        int s = 10101;
        for (int i = 0; i < 10; i++)
        {
            lenelstar.Add(s + i, 2);
        }
        lenelstar.Add(10201,2);
        lenelstar.Add(10202, 0);
    }


    public override bool OnClose()
    {
        base.OnClose();
        _InitChapter(false);
        _InitNewSection(false, 0, 0);
        return true;

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }


    public void OnDeblocking(int _key, int _chapter)
    {
        _InitNewSection(true, _key, _chapter);
    }

    /// <summary>
    /// 打开章节
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_chapter"></param>
    public void OpLevel(int _type, int _chapter, int _level)
    {
       
        if (this.mView.Genlevel.Count == 0) this.mView._Init();
        BattleData battle = JsonMgr.GetSingleton().GetChapter(((_type + 1) * 100 + (_chapter + 1)));
        int LeveID = 0;
        Vector2 History = Vector2.zero;
        int idx = _chapter;
        int levelId;
        BattleMgr.Instance.SectionKey = _type;
        BattleMgr.Instance.SectionChapter = _chapter;
        this.mView.battleName = this.mView.ChapterDic[_type][_level].transform.Find("battleName").gameObject;

        
        if (this.mView.isPass == SectionState.Not)
        {
            Debug.Log("未开启章节:" + BattleMgr.Instance.Genlevel[_type][_chapter]);
            return;
        }
        else
        {
            if (!this.mView.ChapterDic[_type][_chapter].activeSelf)
            {
              this.mView.ChapterDic[_type][_chapter].SetActive(true);
            }
        }

        for (int i = 0; i < this.mView.ChapterDic[_type].Count; i++)
        {
            if (History != this.mView.ChapterDic[_type][i].GetComponent<RectTransform>().rect.size)
            {
                History = this.mView.ChapterDic[_type][i].GetComponent<RectTransform>().rect.size;
            }
            this.mView.ChapterDic[_type][i].GetComponent<RectTransform>().anchoredPosition = new Vector2(-((History.x + 83) * _chapter), 0);
            _chapter--;
        }
        switch (BattleMgr.Instance.SectionKey)
        {

            case 0:
                if (BattleMgr.Instance.NowLevelId==10310)
                {
                    this.mView.arroParent_obj.SetActive(false);
                }
                this.mView.Indicate(this.mView.Genlevel[BattleMgr.Instance.NowLevelId]);
                LeveID = BattleMgr.Instance.NowLevelId;
                break;
            case 1:
                this.mView.Indicate(this.mView.Genlevel[BattleMgr.Instance.EliteLevelId]);
                LeveID = BattleMgr.Instance.EliteLevelId;
                break;
            case 2:
                this.mView.Indicate(this.mView.Genlevel[BattleMgr.Instance.EpicLevelId]);
                LeveID = BattleMgr.Instance.EpicLevelId;
                break;
        }

        for (int idx3 = 0; idx3 < BattleMgr.Instance.Genlevel[_type][idx].GenStage.Count; idx3++)
        {
            if (BattleMgr.Instance.Genlevel[_type][idx].GenStage[idx3].StarCount != 0)
            {
                this.mView.Genlevel[BattleMgr.Instance.Genlevel[_type][idx].GenStage[idx3].LevelID].CloseBtn.enabled = true;
                this.mView.Genlevel[BattleMgr.Instance.Genlevel[_type][idx].GenStage[idx3].LevelID].transform.Find("pentagon").GetComponent<StarView>().SetStar(BattleMgr.Instance.Genlevel[_type][idx].GenStage[idx3].StarCount);
            }
            else
            {

                if (this.mView.Genlevel[BattleMgr.Instance.Genlevel[_type][idx].GenStage[idx3].LevelID].isZeki)
                {
                    this.mView.Genlevel[BattleMgr.Instance.Genlevel[_type][idx].GenStage[idx3].LevelID].CloseBtn.enabled = true;
                    break;
                }
                else if (BattleMgr.Instance.Genlevel[_type][idx].GenStage[idx3].LevelID == LeveID)
                {
                    this.mView.Genlevel[BattleMgr.Instance.Genlevel[_type][idx].GenStage[idx3].LevelID].CloseBtn.enabled = true;
                    break;
                }
                else
                    this.mView.Genlevel[BattleMgr.Instance.Genlevel[_type][idx].GenStage[idx3].LevelID].CloseBtn.enabled = false;

            }

        }
        this.mView.InitStar(_type, idx);

       _InitNewSection(true, _type, idx);
        if (idx == 0 && this.mView.isPass == SectionState.Pass)
        {
            this.mView.QueenShow(false);
            this.mView.FrontShow(true);
        }
        else if (idx == 0)
        {
            this.mView.QueenShow(false);
            this.mView.FrontShow(false);
        }
        else if (idx != 0 && this.mView.isPass != SectionState.Pass)
        {
            this.mView.QueenShow(true);
            this.mView.FrontShow(false);
        }
        else if(BattleMgr.Instance.NowLevelId == 10310 && idx == BattleMgr.Instance.Genlevel[_type].Count-1)
        {
            this.mView.QueenShow(true);
            this.mView.FrontShow(false);
        }
        else
        {
            this.mView.QueenShow(true);
            this.mView.FrontShow(true);

        }
        if (_level > 0)
        {
            if (_level < 10)
                levelId = int.Parse(String.Concat(((_type + 1) * 100 + (idx + 1)).ToString(), "0", _level));
            else
                levelId = int.Parse(String.Concat(((_type + 1) * 100 + (idx + 1)).ToString(), _level));

            switch (_type)
            {
                case 0:
                    BattleMgr.Instance.NowLevelId = levelId;
                    break;
                case 1:
                    BattleMgr.Instance.EliteLevelId = levelId;
                    break;
                case 2:
                    BattleMgr.Instance.EpicLevelId = levelId;
                    break;
            }
            OpLeve(levelId);
        }

    }
    /// <summary>
    /// 打开关卡
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_chapter"></param>
    /// <param name="_level"></param>
    public void OpLeve(int _levelId)
    {
        this.mView.BattleInt(_levelId,this);

       // this.mView.battleName.SetActive(false);

        //LevelData data = JsonMgr.GetSingleton().GetLevel(_levelId);
        //this.mView.intro_txt.text = data.desc;
        //this.mView.power_txt.text = data.power.ToString();
        //this.mView.queen_btn.gameObject.SetActive(false);
        //this.mView.front_btn.gameObject.SetActive(false);

        //if (data.times < 0)
        //{
        //    this.mView.time_obj.SetActive(false);
        //    this.mView.CustomsPass_obj.SetActive(true);
        //}

        //else
        //{
        //    for (int idx = 0; idx < BattleMgr.Instance.Genlevel.Count; idx++)
        //    {
        //        for (int idx1 = 0; idx1 < BattleMgr.Instance.Genlevel[idx].Count; idx1++)
        //        {
        //            for (int idx2 = 0; idx2 < BattleMgr.Instance.Genlevel[idx][idx1].GenStage.Count; idx2++)
        //            {
        //                if (BattleMgr.Instance.Genlevel[idx][idx1].GenStage[idx2].LevelID == _levelId)
        //                {
        //                    if (BattleMgr.Instance.Genlevel[idx][idx1].GenStage[idx2].ResidueTime == 0) //剩余次数
        //                    {
        //                        this.mView.nowTime_txt.text = "0";
        //                        Debug.Log("挑战次数不足，请购买次数");
        //                    }
        //                    else
        //                    {
        //                        this.mView.time_obj.SetActive(true);
        //                        this.mView.Total_txt.text = data.times.ToString();
        //                        this.mView.nowTime_txt.text = BattleMgr.Instance.Genlevel[idx][idx1].GenStage[idx2].ResidueTime.ToString();
        //                    }
        //                    switch (BattleMgr.Instance.Genlevel[idx][idx1].GenStage[idx2].StarCount)
        //                    {
        //                        case 1:
        //                            this.mView.star_1_img.color += new Color(0, 0, 0, 1);
        //                            this.mView.star_2_img.color += new Color(0, 0, 0, 0);
        //                            this.mView.star_3_img.color += new Color(0, 0, 0, 0);
        //                            break;
        //                        case 2:
        //                            this.mView.star_1_img.color += new Color(0, 0, 0, 1);
        //                            this.mView.star_2_img.color += new Color(0, 0, 0, 1);
        //                            this.mView.star_3_img.color += new Color(0, 0, 0, 0);
        //                            break;
        //                        case 3:
        //                            this.mView.star_1_img.color += new Color(0, 0, 0, 1);
        //                            this.mView.star_2_img.color += new Color(0, 0, 0, 1);
        //                            this.mView.star_3_img.color += new Color(0, 0, 0, 1);
        //                            break;
        //                    }
        //                    this.mView.CustomsPass_obj.SetActive(true);
        //                    return;
        //                }
        //            }
        //        }
        //    }
      //  }


    }

    private void _InitChapter(bool open)
    {
        if (open)
        {
            this.mView.epic_tog.onValueChanged.AddListener((bool value) => this.mView.Epic());
            this.mView.elite_tog.onValueChanged.AddListener((bool value) => this.mView.Elite());
            this.mView.common_tog.onValueChanged.AddListener((bool value) => this.mView.Common());
            this.mView.front_btn.onClick.AddListener(delegate () { this.mView.CutSectionName(true); });
            this.mView.queen_btn.onClick.AddListener(delegate () { this.mView.CutSectionName(false); });
           // this.mView.quit_btn.onClick.AddListener(delegate () { this.mView.BattleBtn(); });

           

            //this.mView.Quit_btn.onClick.AddListener(delegate () { this.mView.CloseCustoms(); });
            //this.mView.Star1_btn.onClick.AddListener(delegate () { this.mView.PassStage(1); });
            //this.mView.Star2_btn.onClick.AddListener(delegate () { this.mView.PassStage(2); });
            //this.mView.Star3_btn.onClick.AddListener(delegate () { this.mView.PassStage(3); });
            //this.mView.buy_btn.onClick.AddListener(delegate () { this.mView.Expect(); });
            //this.mView.operation_10_btn.onClick.AddListener(delegate () { this.mView.Expect(); });
            //this.mView.operation_btn.onClick.AddListener(delegate () { this.mView.Expect(); });
            //this.mView.embattle_btn.onClick.AddListener(delegate () {
            //    UIFace.GetSingleton().Open(UIID.Stub, 0);
            //});

            this.mView.Clear_4_btn.onClick.AddListener(delegate () { this.mView.OpenTreasure("Clear_4"); });
            this.mView.Clear_8_btn.onClick.AddListener(delegate () { this.mView.OpenTreasure("Clear_8"); });
            this.mView.Clear_12_btn.onClick.AddListener(delegate () { this.mView.OpenTreasure("Clear_12"); });
            ZEventSystem.Register(EventConst.OnClose, this.mView, "CloseCustoms");
            ZEventSystem.Register(EventConst.OpLevel, this, "OpLevel");
            
        }
        else
        {
            ZEventSystem.DeRegister(EventConst.OpLevel, this);
            ZEventSystem.DeRegister(EventConst.OnClose, this.mView);
            this.mView.elite_tog.onValueChanged.RemoveAllListeners();
            this.mView.common_tog.onValueChanged.RemoveAllListeners();
            this.mView.front_btn.onClick.RemoveAllListeners();
            this.mView.queen_btn.onClick.RemoveAllListeners();
            this.mView.quit_btn.onClick.RemoveAllListeners();
          
            //this.mView.Quit_btn.onClick.RemoveAllListeners();
            //this.mView.Star1_btn.onClick.RemoveAllListeners();
            //this.mView.Star2_btn.onClick.RemoveAllListeners();
            //this.mView.Star3_btn.onClick.RemoveAllListeners();
            //this.mView.buy_btn.onClick.RemoveAllListeners();
            //this.mView.operation_10_btn.onClick.RemoveAllListeners();
            //this.mView.operation_btn.onClick.RemoveAllListeners();
            //this.mView.embattle_btn.onClick.RemoveAllListeners();
           // this.mView.Clear_4_btn.onClick.RemoveAllListeners();
           // this.mView.Clear_8_btn.onClick.RemoveAllListeners();
            //this.mView.Clear_12_btn.onClick.RemoveAllListeners();
        }
    }



    public void _InitNewSection(bool open, int _key, int _chapter)
    {
        int idx = 0;
        if (open)
        {
            foreach (var item in this.mView.Genlevel)
            {
                if (item.Key == BattleMgr.Instance.Genlevel[_key][_chapter].GenStage[idx].LevelID)
                {
                    item.Value.CloseBtn.onClick.AddListener(delegate () { this.mView.BattleInt(item.Key,this); });
                    idx++;
                }
                if (idx >= BattleMgr.Instance.Genlevel[_key][_chapter].GenStage.Count)
                    return;
            }

        }
        else
        {

            foreach (var item in this.mView.Genlevel)
            {
                item.Value.CloseBtn.onClick.RemoveAllListeners();
            }
        }
    }
    public void OpenCustoms()
    {
        UIFace.GetSingleton().Open(UIID.Customs);
    }


}
