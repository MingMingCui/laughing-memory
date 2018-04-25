using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchUICtrl : UICtrlBase<MatchUIView> {

    //这两个数都是假的，所以直接定义到这里了
    public int TreasureCnt = 0;
    public int GoldCnt = 0;

    public bool IsPvp = false;

    public override void OnOpen()
    {
        base.OnOpen();
        TreasureCnt = 0;
        GoldCnt = 0;
        _initEvent(true);
        ProcessCtrl.Instance.GoCoroutine("CreateFight", _createFight());
    }

    public override bool OnClose()
    {
        base.OnClose();
        _initEvent(false);
        return true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void OnCreateFight(List<FightUnit> allUnit)
    {
        this.mView.InitFightUnitInfo(allUnit);

        //追加注册事件
        _initAdditionEvent(true);
    }

    
    public void OnCreateSummon(FightUnit summon)
    {
        this.mView.AddFightUnitInfo(summon);
    }

    public void OnAutoFightClick()
    {
        FightLogic.Instance.IsAutoFight = !FightLogic.Instance.IsAutoFight;
    }

    public void OnSpeedClick()
    {
        FightLogic.Instance.IsSpeedUp = !FightLogic.Instance.IsSpeedUp;
    }

    public void GamePause(bool pause)
    {
        this.mView.OpenPause(pause);
        ZEventSystem.Dispatch(EventConst.OnGamePause, pause);
        Time.timeScale = pause ? 0 : 1;
    }

    public void ExitFight()
    {
        FightLogic.Instance.Clear();
        SceneMgr.Instance.LoadScene("Main");
    }

    public void OpenLevel()
    {
        UIFace.GetSingleton().Open(UIID.Battle);
    }

    public void Retry()
    {
        FightLogic.Instance.Clear();
        if (IsPvp)
        {
        }
        else
        {
            BattleMgr.Instance.StartFight(BattleMgr.Instance.LevelID);
        }
    }



    public void OnCameraChangeOver(bool forward)
    {
        if (FightLogic.Instance.State == FightState.Continue)
        {
            this.mView.PlayFightMask(true);
        }
    }

    public void OnFightStateChange(FightState fightState)
    {
        if (FightLogic.Instance.State == FightState.Prepare && FightLogic.Instance.CurRound > 1)
        {
            this.mView.PlayFightMask(false);
        }
        if (fightState == FightState.Over)
        {
            this.mView.SetDataAnalyze(FightLogic.Instance.Fighters, FightLogic.Instance.EnemyFighters[FightLogic.Instance.CurRound - 1]);
            ProcessCtrl.Instance.GoCoroutine("OpenSettlement", _openSettlement(FightLogic.Instance.HasWin, FightLogic.Instance.IsPvp));
        }
    }

    public void OnAutoFightStateChange(bool open)
    {
        this.mView.auto_fight_img.gameObject.SetActive(open);
    }

    public void OnGameSpeedChange(bool open)
    {
        this.mView.speed2_img.gameObject.SetActive(open);
    }

    public void OnFightUnitPop(FightUnit unit, Vector2Int data)
    {
        this.mView.SetPopInfo(unit, data);
    }

    public void OnNewRound(int cur, int total)
    {
        this.mView.round_txt.text = string.Format("{0}/{1}", cur, total);
    }

    public void OnUnitPause(bool pause)
    {
        this.mView.ActiveSkill_img.gameObject.SetActive(pause);
    }

    public void OnMusicMute(bool mute)
    {
        this.mView.musicforbid_img.gameObject.SetActive(mute);
    }

    public void OnSoundMute(bool mute)
    {
        this.mView.soundforbid_img.gameObject.SetActive(mute);
    }

    public void OnTreasureFly(Vector3 pos)
    {
        this.mView.CreateTreasureEffect(pos);
    }

    public void OnTreasureFlyOver()
    {
        TreasureCnt++;
        this.mView.treasure_txt.text = TreasureCnt.ToString();
    }

    public void OnHeadClick(FightUnit unit)
    {
        if (unit != null)
        {
            unit.UseActiveSkill();
        }
    }

    public void OnMusicClick()
    {
        SoundMgr.Instance.MuteMusic = !SoundMgr.Instance.MuteMusic;
    }

    public void OnSoundClick()
    {
        SoundMgr.Instance.MuteSound = !SoundMgr.Instance.MuteSound;
    }

    private IEnumerator _openSettlement(bool win, bool pvp)
    {
        yield return ProcessCtrl.Instance.WatiForOneSecond;
        this.IsPvp = pvp;
        this.mView.OpenSettlement(win, pvp, FightLogic.Instance.Fighters);
        for (int idx = 0; idx < FightLogic.Instance.Fighters.Count; ++idx)
        {
            FightUnit unit = FightLogic.Instance.Fighters[idx];
            Debug.LogFormat("UID {0} 统计伤害：{1}", unit.UID, unit.HarmCount);
        }
        SoundMgr.Instance.PlaySound(win ? 100100 : 100101);
    }

    private IEnumerator _createFight()
    {
        yield return ProcessCtrl.Instance.WaitForMoment;
        FightLogic.Instance.CreateFight(false, 20);
    }


    /// <summary>
    /// 为掉落物品赋值
    /// </summary>
    /// <param name="_dropOutitem"></param>
    public void DropOutItem(List<Item> _dropOutitem)
    {
        this.mView.Dropitem(_dropOutitem);
    }
   
    private void _initAdditionEvent(bool open)
    {
        if (open)
        {
            foreach (var h in this.mView.Heads)
            {
                ZEventSystem.Register(EventConst.OnFightUnitHpChange, h.Value, "OnFightUnitHpChange");
                ZEventSystem.Register(EventConst.OnFightUnitVigourChange, h.Value, "OnFightUnitVigourChange");
                ZEventSystem.Register(EventConst.OnFightStateChange, h.Value, "OnFightStateChange");
                ZEventSystem.Register(EventConst.OnFightTargetChange, h.Value, "OnFightTargetChange");
                ZEventSystem.Register(EventConst.OnFightUnitDie, h.Value, "OnFightUnitDie");
                h.Value.fightHero_btn.onClick.AddListener(delegate () { this.OnHeadClick(h.Value.Unit); });
            }

        }
        else
        {
            foreach (var h in this.mView.Heads)
            {
                ZEventSystem.DeRegister(EventConst.OnFightUnitHpChange, h.Value);
                ZEventSystem.DeRegister(EventConst.OnFightUnitVigourChange, h.Value);
                ZEventSystem.DeRegister(EventConst.OnFightStateChange, h.Value);
                ZEventSystem.DeRegister(EventConst.OnFightTargetChange, h.Value);
                ZEventSystem.DeRegister(EventConst.OnFightUnitDie, h.Value);
                h.Value.fightHero_btn.onClick.RemoveAllListeners();
            }
        }
    }

    private void _initEvent(bool open)
    {
        if (open)
        {
            ZEventSystem.Register(EventConst.OnCreateFight, this, "OnCreateFight");
            ZEventSystem.Register(EventConst.OnFightUnitPop, this, "OnFightUnitPop");
            ZEventSystem.Register(EventConst.OnAutoFightStateChange, this, "OnAutoFightStateChange");
            ZEventSystem.Register(EventConst.OnGameSpeedChange, this, "OnGameSpeedChange");
            ZEventSystem.Register(EventConst.OnNewRound, this, "OnNewRound");
            ZEventSystem.Register(EventConst.OnUnitPause, this, "OnUnitPause");
            ZEventSystem.Register(EventConst.OnMusicMute, this, "OnMusicMute");
            ZEventSystem.Register(EventConst.OnSoundMute, this, "OnSoundMute");
            ZEventSystem.Register(EventConst.OnCameraChangeOver, this, "OnCameraChangeOver");
            ZEventSystem.Register(EventConst.OnFightStateChange, this, "OnFightStateChange");
            ZEventSystem.Register(EventConst.OnTreasureFly, this, "OnTreasureFly");
            ZEventSystem.Register(EventConst.OnTreasureFlyOver, this, "OnTreasureFlyOver");
            ZEventSystem.Register(EventConst.OnCreateSummon, this, "OnCreateSummon");
            ZEventSystem.Register(EventConst.DropOutItem, this, "DropOutItem");
            ZEventSystem.Register(EventConst.OnInitEvent,this, "OnInitEvent");
            this.mView.auto_fight_btn.onClick.AddListener(delegate () { this.OnAutoFightClick(); });
            this.mView.speed_btn.onClick.AddListener(delegate () { this.OnSpeedClick(); });
            this.mView.pause_btn.onClick.AddListener(delegate () { this.GamePause(true); });
            this.mView.continue_btn.onClick.AddListener(delegate () { this.GamePause(false); });
            this.mView.exit_btn.onClick.AddListener(delegate () { this.ExitFight(); });
            this.mView.music_btn.onClick.AddListener(delegate () { this.OnMusicClick(); });
            this.mView.sound_btn.onClick.AddListener(delegate () { this.OnSoundClick(); });
            this.mView.FailExit_btn.onClick.AddListener(delegate () { this.ExitFight(); });
            this.mView.FailTryAgain_btn.onClick.AddListener(delegate () { this.Retry(); });
            this.mView.PVERetry_btn.onClick.AddListener(delegate () { this.Retry(); });
            this.mView.PVENext_btn.onClick.AddListener(delegate () { this.OpenLevel(); });
            this.mView.PVEBack_btn.onClick.AddListener(delegate () { this.ExitFight(); });
            this.mView.PVPRetry_btn.onClick.AddListener(delegate () { this.Retry(); });
            this.mView.DataAnalyzeClose_btn.onClick.AddListener(delegate () { this.mView.OpenDataAnalyze(false); });
            this.mView.PVEDataAnalyze_btn.onClick.AddListener(delegate () { this.mView.OpenDataAnalyze(true); });
            this.mView.PVPDataAnalyze_btn.onClick.AddListener(delegate () { this.mView.OpenDataAnalyze(true); });
            this.mView.FailDataAnalyze_btn.onClick.AddListener(delegate () { this.mView.OpenDataAnalyze(true); });           
        }
        else
        {
            ZEventSystem.DeRegister(EventConst.OnInitEvent,this);
            ZEventSystem.DeRegister(EventConst.OnCreateFight, this);
            ZEventSystem.DeRegister(EventConst.OnFightUnitPop, this);
            ZEventSystem.DeRegister(EventConst.OnAutoFightStateChange, this);
            ZEventSystem.DeRegister(EventConst.OnGameSpeedChange, this);
            ZEventSystem.DeRegister(EventConst.OnNewRound, this);
            ZEventSystem.DeRegister(EventConst.OnUnitPause, this);
            ZEventSystem.DeRegister(EventConst.OnMusicMute, this);
            ZEventSystem.DeRegister(EventConst.OnSoundMute, this);
            ZEventSystem.DeRegister(EventConst.OnCameraChangeOver, this);
            ZEventSystem.DeRegister(EventConst.OnFightStateChange, this);
            ZEventSystem.DeRegister(EventConst.OnTreasureFly, this);
            ZEventSystem.DeRegister(EventConst.OnTreasureFlyOver, this);
            ZEventSystem.DeRegister(EventConst.OnCreateSummon, this);
            ZEventSystem.DeRegister(EventConst.DropOutItem, this);
            this.mView.auto_fight_btn.onClick.RemoveAllListeners();
            this.mView.speed_btn.onClick.RemoveAllListeners();
            this.mView.pause_btn.onClick.RemoveAllListeners();
            this.mView.continue_btn.onClick.RemoveAllListeners();
            this.mView.exit_btn.onClick.RemoveAllListeners();
            this.mView.music_btn.onClick.RemoveAllListeners();
            this.mView.sound_btn.onClick.RemoveAllListeners();
            this.mView.FailExit_btn.onClick.RemoveAllListeners();
            this.mView.FailTryAgain_btn.onClick.RemoveAllListeners();
            this.mView.PVERetry_btn.onClick.RemoveAllListeners();
            this.mView.PVENext_btn.onClick.RemoveAllListeners();
            this.mView.PVEBack_btn.onClick.RemoveAllListeners();
            this.mView.PVPRetry_btn.onClick.RemoveAllListeners();
            this.mView.DataAnalyzeClose_btn.onClick.RemoveAllListeners();
            this.mView.PVEDataAnalyze_btn.onClick.RemoveAllListeners();
            this.mView.PVPDataAnalyze_btn.onClick.RemoveAllListeners();
            this.mView.FailDataAnalyze_btn.onClick.RemoveAllListeners();
            _initAdditionEvent(false);
            HeroTog(false);
        }
    }

    public void OnInitEvent()
    {
        this.mView.Int(FightLogic.Instance.AllFighters);
        HeroTog(true);
    }
    public void HeroTog(bool isopen)
    {

        if (isopen)
        {
            foreach (var key in this.mView.AllFighter)
            {
                key.fighter_tog.onValueChanged.AddListener((bool value) => this.mView.GetData((int)key.UID, key.isEnemy));
            }
        }
        else
        {
            foreach (var key in this.mView.AllFighter)
            {
                key.fighter_tog.onValueChanged.RemoveAllListeners();

            }
        }
    }
}
