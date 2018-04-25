using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchUIView : MatchUIViewBase {

    public FightUnitInfoView fightUnitInfoView = null;
    public FightHeroView fightHeroView = null;
    public GameObject TreasureEffect = null;
    public HeroHeadView heroHeadView = null;
    public DataAnalyzeView DataAnalyzeViewLeft = null;
    public DataAnalyzeView DataAnalyzeViewRight = null;
    public Dictionary<uint, FightHeroView> Heads = new Dictionary<uint, FightHeroView>();
    private Dictionary<uint, FightUnitInfoView> _infos = new Dictionary<uint, FightUnitInfoView>();
    private int _timeleft = 0;
    public readonly float FIGHTMASKSPEED = 1.0f;
    public readonly float SETTLE_HEAD_INTERVAL = 0.2f;
    private Camera uiCamera = null;
    public static readonly float TreasureEffectSpeed = 15;

    public GameObject Spoils;
    private enum FightMaskState
    {
        None = 0,
        Forward = 1,
        Inverse = 2,
    }

    private class TreasureEffectObj
    {
        public GameObject TreasureEffect = null;
        public Vector3 StartPos = Vector3.zero;
        public Vector3 EndPos = Vector3.zero;
    }

    private List<TreasureEffectObj> _treasureEffect = new List<TreasureEffectObj>();

    private FightMaskState _maskState = FightMaskState.None;
    private FightMaskState MaskState
    {
        get { return _maskState; }
        set {
            if (_maskState != value)
            {
                _maskState = value;
            }
        }
    }
    private float _fightMaskLerp = 0;

    public void InitFightUnitInfo(List<FightUnit> allUnit)
    {
        List<FightUnit> _heros = new List<FightUnit>();
        for(int idx = 0; idx < allUnit.Count; ++idx)
        {
            FightUnit unit = allUnit[idx];
            AddFightUnitInfo(unit);

            if (!unit.IsEnemy)
                _heros.Add(unit);
            
        }

        _heros.Sort((FightUnit a, FightUnit b) => { return a.StubPos.CompareTo(b.StubPos); });
        
        for(int idx = 0; idx < _heros.Count; ++idx)
        {
            FightUnit unit = _heros[idx];
            GameObject fightHero = GameObject.Instantiate(fightHeroView.gameObject);
            fightHero.name = string.Format("FightHero_{0}", unit.UID);
            fightHero.transform.SetParent(this.hero_obj.transform, false);
            FightHeroView _fightHeroView = fightHero.GetComponent<FightHeroView>();
            _fightHeroView.Init();
            _fightHeroView.InitHero(unit);

            RectTransform fightHeroRect = fightHero.GetComponent<RectTransform>();
            fightHeroRect.anchoredPosition = new Vector2((idx - _heros.Count / 2.0f + 0.5f) * fightHeroRect.sizeDelta.x, 0);
            Heads.Add(unit.UID, _fightHeroView);
        }
    }

    public void AddFightUnitInfo(FightUnit unit)
    {
        GameObject unitinfo = GameObject.Instantiate(fightUnitInfoView.gameObject);
        unitinfo.name = string.Format("UnitInfo_{0}", unit.UID);
        unitinfo.transform.SetParent(this.Info_obj.transform, false);
        FightUnitInfoView infoView = unitinfo.GetComponent<FightUnitInfoView>();
        infoView.SetFightUnit(unit);
        _infos.Add(unit.UID, infoView);
    }

    public void OpenPause(bool open)
    {
        this.Pause_obj.SetActive(open);
        this.musicforbid_img.gameObject.SetActive(SoundMgr.Instance.MuteMusic);
        this.soundforbid_img.gameObject.SetActive(SoundMgr.Instance.MuteSound);
    }

    public void OpenSettlement(bool win, bool pvp, List<FightUnit> fighters)
    {
        this.Top_obj.SetActive(false);
        this.Bottom_obj.SetActive(false);
        if (!win)
            this.Fail_obj.SetActive(true);
        else
        {
            if (pvp)
            {
                this.PVPSuccess_obj.SetActive(true);
            }
            else
            {
                this.PVESuccess_obj.SetActive(true);

                ZEventSystem.Dispatch(EventConst.DropOutItem, ItemMgr.Instance.itemList);
            }
            if (fighters != null && heroHeadView != null)
            {
                GameObject heroHeadParent = pvp ? pvpheros_obj : pveheros_obj;
                for (int idx = 0; idx < fighters.Count; ++idx)
                {
                    FightUnit unit = fighters[idx];
                    if (unit == null)
                        continue;
                    GameObject heroHead = GameObject.Instantiate(heroHeadView.gameObject);
                    heroHead.transform.SetParent(heroHeadParent.transform, false);
                    RectTransform heroHeadRect = heroHead.GetComponent<RectTransform>();
                    heroHeadRect.anchoredPosition = new Vector2(heroHeadRect.sizeDelta.x * (idx * (1 + SETTLE_HEAD_INTERVAL) - (5 + SETTLE_HEAD_INTERVAL * 4) / 2 + 0.5f), 0);
                    HeroHeadView view = heroHead.GetComponent<HeroHeadView>();
                    view.Init();
                    int headId = JsonMgr.GetSingleton().GetHeroByID(unit.HeroId).headid;
                    if (pvp)
                        view.SetHeroInfo(headId, unit.Rare, unit.Star, unit.Level);
                    else
                    {
                        HeroData heroData = HeroMgr.GetSingleton().GetHeroData(unit.HeroId);
                        if (heroData != null)
                        {
                            view.SetHeroInfo(headId, unit.Rare, unit.Star, unit.Level, 10, heroData.Exp, heroData.NeedHero); //临时
                        }
                    }
                }
            }
        }
    }

    public void OpenDataAnalyze(bool open)
    {
        this.DataAnalyze_obj.SetActive(open);
    }

    public void SetDataAnalyze(List<FightUnit> fighters, List<FightUnit> enemies)
    {
        if (DataAnalyzeViewLeft == null || DataAnalyzeViewRight == null)
        {
            EDebug.LogError("DataAnalyzeNode missing!");
            return;
        }
        int maxHarm = 0;
        for (int idx = 0; idx < fighters.Count; ++idx)
        {
            FightUnit unit = fighters[idx];
            if (unit == null)
                continue;
            if (unit.HarmCount > maxHarm)
                maxHarm = unit.HarmCount;
        }
        for (int idx = 0; idx < enemies.Count; ++idx)
        {
            FightUnit unit = enemies[idx];
            if (unit == null)
                continue;
            if (unit.HarmCount > maxHarm)
                maxHarm = unit.HarmCount;
        }

        float nodeHeight = DataAnalyzeViewLeft.gameObject.GetComponent<RectTransform>().sizeDelta.y;

        for (int idx = 0; idx < fighters.Count; ++idx)
        {
            FightUnit unit = fighters[idx];
            if (unit == null)
                continue;
            GameObject node = GameObject.Instantiate(DataAnalyzeViewLeft.gameObject);
            node.transform.SetParent(Datas_obj.transform, false);
            RectTransform nodeRect = node.GetComponent<RectTransform>();
            nodeRect.anchoredPosition = new Vector2(nodeRect.sizeDelta.x / 2, -(idx + 0.5f) * nodeRect.sizeDelta.y);
            DataAnalyzeView nodeView = node.GetComponent<DataAnalyzeView>();
            nodeView.Init();
            nodeView.SetInfo(JsonMgr.GetSingleton().GetHeroByID(unit.HeroId).headid, unit.Rare, unit.Star, unit.Level, unit.HarmCount, maxHarm);
        }

        for (int idx = 0; idx < enemies.Count; ++idx)
        {
            FightUnit unit = enemies[idx];
            if (unit == null)
                continue;
            GameObject node = GameObject.Instantiate(DataAnalyzeViewRight.gameObject);
            node.transform.SetParent(Datas_obj.transform, false);
            RectTransform nodeRect = node.GetComponent<RectTransform>();
            nodeRect.anchoredPosition = new Vector2(-nodeRect.sizeDelta.x / 2, -(idx + 0.5f) * nodeRect.sizeDelta.y);
            DataAnalyzeView nodeView = node.GetComponent<DataAnalyzeView>();
            nodeView.Init();
            int headId = 0;
            if (unit.IsMonster)
            {
                headId = JsonMgr.GetSingleton().GetMonsterByID(unit.HeroId).headid;
            }
            else
                headId = JsonMgr.GetSingleton().GetHeroByID(unit.HeroId).headid;

            nodeView.SetInfo(headId, unit.Rare, unit.Star, unit.Level, unit.HarmCount, maxHarm);
        }
        RectTransform datasRect = Datas_obj.GetComponent<RectTransform>();
        datasRect.sizeDelta = new Vector2(datasRect.sizeDelta.x, Mathf.Max(fighters.Count, enemies.Count) * nodeHeight);
    }

    public void CreateTreasureEffect(Vector3 worldPos)
    {
        Vector3 followPoint = FightUnitInfoView.OUT_SCREEN;
        {
            Vector2 targetScreenPosition = Camera.main.WorldToScreenPoint(worldPos);
            if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(this.Treasure_rect, targetScreenPosition, 
                CanvasView.Instance.uicamera, out followPoint))
            {
                Debug.LogErrorFormat("计算位置失败：{0}", worldPos);
            }
        }
        TreasureEffectObj obj = new TreasureEffectObj();
        obj.TreasureEffect = GameObject.Instantiate(TreasureEffect);
        obj.TreasureEffect.transform.SetParent(this.Treasure_rect.transform, false);
        obj.TreasureEffect.transform.position = followPoint;
        obj.StartPos = followPoint;
        obj.EndPos = this.treasureicon_rect.transform.position;
        _treasureEffect.Add(obj);
    }

    /// <summary>
    /// 回合间过度效果(天黑天亮)，天黑时机，战斗状态Continue，摄像机移动完毕
    /// 天亮时机，进入下回合Prepare(不能是下次摄像机移动完成，因为那是在Fight的时候)
    /// </summary>
    /// <param name="forward">天黑or天亮</param>
    public void PlayFightMask(bool forward)
    {
        MaskState = forward ? FightMaskState.Forward : FightMaskState.Inverse;
        _fightMaskLerp = forward ? 0f : 1f;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.F10))
        {
            Buff_obj.SetActive(true);
            ZEventSystem.Dispatch(EventConst.OnGamePause, true);
            Time.timeScale = true ? 0 : 1;
            GetData(0,false);
        }
        if (Input.GetKey(KeyCode.F11))
        {
            Buff_obj.SetActive(false);
            ZEventSystem.Dispatch(EventConst.OnGamePause, false);
            Time.timeScale = false ? 0 : 1;
            fighter = null;
        }
#endif
        float deltaLerp = Time.deltaTime * FIGHTMASKSPEED;
        deltaLerp *= (MaskState == FightMaskState.None ? 0 : (MaskState == FightMaskState.Forward ? 1 : -1));
        _fightMaskLerp += deltaLerp;
        if (_fightMaskLerp < 0 || _fightMaskLerp > 1)
        {
            _fightMaskLerp = Mathf.Clamp01(_fightMaskLerp);
            MaskState = FightMaskState.None;
            ZEventSystem.Dispatch(EventConst.OnFightMaskOver);
        }
        this.FightMask_img.color = new Color(0, 0, 0, _fightMaskLerp);

        int timeleft = (int)(FightLogic.Instance.RoundTime - FightLogic.Instance.AccRoundTime);
        if (_timeleft != timeleft)
        {
            this.time_txt.text = string.Format("{0}:{1:D2}", timeleft / 60, timeleft % 60);
            _timeleft = timeleft;
        }

        for (int idx = _treasureEffect.Count - 1; idx >= 0; --idx)
        {
            TreasureEffectObj obj = _treasureEffect[idx];
            float deltaDis = Time.deltaTime * TreasureEffectSpeed;
            Vector3 dir = obj.EndPos - obj.TreasureEffect.transform.position;
            if (dir.sqrMagnitude <= Mathf.Pow(deltaDis, 2))
            {
                GameObject.Destroy(obj.TreasureEffect);
                _treasureEffect.RemoveAt(idx);
                ZEventSystem.Dispatch(EventConst.OnTreasureFlyOver);
            }
            else
            {
                obj.TreasureEffect.transform.position += dir.normalized * deltaDis;
            }
        }
    }

    public void SetPopInfo(FightUnit unit, Vector2Int data)
    {
        if (unit == null)
            return;
        if (_infos.ContainsKey(unit.UID))
        {
            _infos[unit.UID].SetPopInfo(data);
        }
    }


   public void Dropitem(List<Item> _dropOutitem)
    {
        for (int idx = 0; idx < _dropOutitem.Count; idx++)
        {
                ItemUIView spoils = InitItemInfo().GetComponent<ItemUIView>();
                spoils.Init();
                spoils.SetInfo(_dropOutitem[idx].itemId, _dropOutitem[idx].itemNum);
                if (idx >= 3)
                {
                    spoils_obj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                    spoils_obj.GetComponent<RectTransform>().offsetMin = new Vector2(-(idx + 1 - 3) * 170, 0);
                }
        }

    }
    /// <summary>
    /// 实例化物品信息
    /// </summary>
    public GameObject InitItemInfo()
    {
       
        GameObject _item = GameObject.Instantiate(Spoils.gameObject);
        _item.transform.SetParent(spoils_obj.transform);
        _item.transform.localScale = Vector3.one;
        _item.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _item;
    }


    List<Text> objName = new List<Text>();//所加状态
   public FightUnit fighter = null;
    /// <summary>
    /// 初始化队伍
    /// </summary>
    public void Int(List<FightUnit> unitList)
    {
        if (AllFighter.Count > 0)
        {
            for (int idx = 0; idx < AllFighter.Count; idx++)
            {
                Destroy(AllFighter[idx].gameObject);
            }
            AllFighter.Clear();
        }
          
        for (int i = 0; i < unitList.Count; i++)
        {
            GameObject obj = null;
            FighterTog f = null;
            if (unitList[i].IsEnemy)
                obj = Fighters(enemybufflist_obj.transform);
            else
               obj = Fighters(herobufflist_obj.transform);
            f = obj.GetComponent<FighterTog>();
            f.Init();
            f.fighter_tog.group = herobufflist_obj.GetComponent<ToggleGroup>();
            f.isEnemy = unitList[i].IsEnemy;
            f.SetInfo(unitList[i].UID, unitList[i].HeroName);
            AllFighter.Add(f);
        }
    }
    public GameObject buffobj;
    public GameObject tog;
    public List<Toggle> fighters; //布阵武将
    public List<FighterTog> AllFighter = new List<FighterTog>();
    public GameObject BuffText()
    {
        GameObject buffText = GameObject.Instantiate(buffobj);
        buffText.transform.SetParent(objlist_obj.transform);
        buffText.transform.localScale = Vector3.one;
        buffText.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return buffText;
    }
    
    public void GetData(int id,bool isEnemy)
    {
        List<FightUnit> unitList =   FightLogic.Instance.AllFighters;
        if (unitList.Count == 0) return;
        if (AllFighter.Count == 0) return;
        for (int i = 0; i < unitList.Count; i++)
        {
            if (unitList[i].UID == id || id ==0)
            {
                if (isEnemy == unitList[i].IsEnemy)
                {
                    if (id == 0)
                    {
                        AllFighter[i].fighter_tog.isOn = true;
                        fighter = unitList[i];
                        break;
                    }
                    else
                    {
                        fighter = unitList[i];
                    }
                }
            }
        }
        if (fighter == null) return;
        BuffMgr buff = fighter.BuffMgrObj;

        //所加状态
        #region    
        if (objName.Count > buff._allBuff.Count)
        {
            for (int i = 0; i < objName.Count; i++)
            {
                if (i >= buff._allBuff.Count)
                {
                    objName[i].gameObject.SetActive(false);
                }
                else
                {
                    objName[i].gameObject.SetActive(true);
                    string objDesc = buff._allBuff[i].Desc;
                    objName[i].text = buff._allBuff[i].BuffId.ToString() + ":" + objDesc;
                }
            }
        }
        else
        {
            for (int i = 0; i < buff._allBuff.Count; i++)
            {
                string objDesc = buff._allBuff[i].Desc;
                if (i == objName.Count)
                {
                    Text o = BuffText().GetComponent<Text>();
                    o.text = buff._allBuff[i].BuffId.ToString() + ":" + objDesc;
                    objName.Add(o);
                }
                else
                {

                    objName[i].gameObject.SetActive(true);
                    objName[i].text= buff._allBuff[i].BuffId.ToString() + ":" + objDesc;
                }
            }
        }
        #endregion

        //buff数据
        #region 
        MaxHealth_txt.text = fighter.BaseMaxHp.ToString();
        StealHealth_txt.text = (fighter.HPSuck).ToString();
        CurHealth_txt.text = fighter.CurHP.ToString();
        VigourStimulate_txt.text = fighter.VigourSuck.ToString();
        HealthRegen_txt.text = string.Format("{0:P2}", fighter.HPRec);
        VigourRegen_txt.text = fighter.VigourRec.ToString();
        AttackSpeed_txt.text = fighter.Asp.ToString();
        PhysicalAttacks_txt.text = fighter.Atk.ToString();
        StrategicAttack_txt.text = fighter.Matk.ToString("f2");
        Damage_txt.text = (fighter.HarmRate).ToString();
        Treatment_txt.text = (fighter.HealRate).ToString();
        PhysicalRes_txt.text =string.Format("{0:P2}", fighter.DefRate);
        StrategicRes_txt.text = string.Format("{0:P2}", fighter.MdefRate);
        AlienationRes_txt.text = (fighter.DeBetrayRate).ToString();
        DizzinessRes_txt.text = (fighter.DeFaintRate).ToString();
        MisrepresentingRes_txt.text = (fighter.DeSpoofRate).ToString();
        PhysicalPenetration_txt.text = string.Format("{0:P2}", fighter.DefBreak);
        StrategicPenetration_txt.text = string.Format("{0:P2}", fighter.MdefBreak);
        AlienationRate_txt.text = fighter.BetrayRate.ToString();
        DizzinessRate_txt.text = fighter.FaintRate.ToString();
        DodgeRate_txt.text = string.Format("{0:P2}", fighter.DodgeRate);
        MisrepresentingRate_txt.text = fighter.SpoofRate.ToString();
        HitRate_txt.text = string.Format("{0:P2}", fighter.HitRate);
        BlockRate_txt.text = string.Format("{0:P2}", fighter.BlockRate);
        RoutRate_txt.text = string.Format("{0:P2}", fighter.RoutRate);
        FirmRate_txt.text = string.Format("{0:P2}", fighter.FirmRate);
        CritRate_txt.text = string.Format("{0:P2}", fighter.CritRate);
        CritInc_txt.text = string.Format("{0:P2}", fighter.CritInc);
        #endregion

    }

    /// <summary>
    /// 实例化上阵武将按钮
    /// </summary>
    /// <returns></returns>
    public GameObject Fighters(Transform parent)
    {
        GameObject fighter = Instantiate(tog.gameObject);
        fighter.transform.SetParent(parent);
        fighter.transform.localScale = Vector3.one;
        fighter.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return fighter;
    }
}
