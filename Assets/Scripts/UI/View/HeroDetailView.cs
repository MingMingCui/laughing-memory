using System.Collections.Generic;
using System.Text;
using JsonData;
using UnityEngine;
using UnityEngine.UI;

public enum EHEROSHOWTYPE
{
    Card = 0,
    Officer,
    Divination,
    Skill,
    Relation,
    Detail
}

public class HeroDetailView : HeroDetailViewBase
{

    public GameObject headGo;
    public GameObject officerGo;
    public GameObject totemGo;

    Dictionary<GameObject, EHEROSHOWTYPE> imageType;
    Dictionary<EHEROSHOWTYPE, Transform> typeTr;

    private EHEROSHOWTYPE showType;

    private Transform showGo;
    private int showID;

    private HeroData heroData;

    private CanvasRenderer[] crArray;

    Text[] officerText;
    private bool ddActive = true;

    private HeroData[] unLockData;
    private Dictionary<HeroData, GameObject> instantiate;

    private GridLayoutGroup glg;
    private RectTransform rect;
    private ScrollRect sr;

    private Image[] skillImage;

    private RectTransform officerrt;

    private Transform Herotf;
    private AnimatorPlayer player;

    private ScrollRect scrollRect;

    private UIBillboard billboard;
    public override void Awake()
    {
        base.Awake();
        instantiate = new Dictionary<HeroData, GameObject>();

        typeTr = new Dictionary<EHEROSHOWTYPE, Transform>
        {
            { EHEROSHOWTYPE.Card, Card_trf },
            { EHEROSHOWTYPE.Officer, Officer_trf },
            { EHEROSHOWTYPE.Divination, Divination_trf },
            { EHEROSHOWTYPE.Skill, Skill_trf },
            { EHEROSHOWTYPE.Relation, Relation_trf },
            { EHEROSHOWTYPE.Detail, Detail_trf }
        };
        foreach (var item in typeTr)
        {
            item.Value.SetParent(Hide_trf);
        }

        //图片分类
        imageType = new Dictionary<GameObject, EHEROSHOWTYPE>
        {
            { card_img.gameObject, EHEROSHOWTYPE.Card },
            { officer_img.gameObject, EHEROSHOWTYPE.Officer },
            { divination_img.gameObject, EHEROSHOWTYPE.Divination },
            { skill_img.gameObject, EHEROSHOWTYPE.Skill },
            { relation_img.gameObject, EHEROSHOWTYPE.Relation },
            { detail_img.gameObject, EHEROSHOWTYPE.Detail }
        };

        crArray = Star_obj.GetComponentsInChildren<CanvasRenderer>();
        glg = LeftContent_obj.GetComponent<GridLayoutGroup>();
        rect = LeftContent_obj.GetComponent<RectTransform>();
        sr = Scroll_obj.GetComponent<ScrollRect>();
        scrollRect = Template_obj.GetComponent<ScrollRect>();
        officerrt = OfficerList_obj.GetComponent<RectTransform>();
        billboard = hero_obj.GetComponent<UIBillboard>();

        TriggerListener();

        officerText = DownContent_obj.GetComponentsInChildren<Text>();

        for (int i = 0; i < officerText.Length; i++)
        {
            Text text = officerText[i];
            EventListener.Get(text.gameObject).BegineDragEvent = e =>
            {
                scrollRect.OnBeginDrag(e);
            };
            EventListener.Get(text.gameObject).DragEvent = e =>
            {
                scrollRect.OnDrag(e);
            };
            EventListener.Get(text.gameObject).EndDragEvent = e =>
            {
                scrollRect.OnEndDrag(e);
            };
        }

        skillImage = new Image[4];
        skillImage[0] = skill1_img;
        skillImage[1] = skill2_img;
        skillImage[2] = skill3_img;
        skillImage[3] = skill4_img;
    }
    private void TriggerListener()
    {
        EventListener.Get(card_img.gameObject).OnClick =
        EventListener.Get(officer_img.gameObject).OnClick =
        EventListener.Get(divination_img.gameObject).OnClick =
        EventListener.Get(skill_img.gameObject).OnClick =
        EventListener.Get(relation_img.gameObject).OnClick =
        EventListener.Get(detail_img.gameObject).OnClick = Listener;


        EventListener.Get(ddBG_img.gameObject).OnClick =
        EventListener.Get(ddArrow_btn.gameObject).OnClick = ShowPopList;

        EventListener.Get(Add_btn.gameObject).OnClick = e =>
        {
            UIFace.GetSingleton().Open(UIID.ExpPool, heroData.HeroId);
        };
    }
    private void Listener(GameObject go)
    {
        if (!imageType.ContainsKey(go))
            return;
        ShowType(imageType[go]);
    }

    private void ShowPopList(GameObject go)
    {
        ddActive = !ddActive;
        Template_obj.SetActive(ddActive);
    }

    public void Open(int heroid, EHEROSHOWTYPE type = EHEROSHOWTYPE.Card, bool move = true)
    {
        heroData = HeroMgr.GetSingleton().GetHeroData(heroid);
        BuildHeroEvent(heroid);

        RegisteEvent();

        foreach (var item in imageType)
        {
            if(item.Value == type)
                item.Key.GetComponent<Toggle>().isOn = true;
        }
        ddActive = true;
        ShowPopList(null);

        ShowType(type);
        ShowLeft();
        ShowSide(move);
    }
    /// <summary>
    /// OPEN调用
    /// </summary>
    private void RegisteEvent()
    {
        ZEventSystem.Register(EventConst.REFRESHSIDE, this, "ShowSide");
        ZEventSystem.Register(EventConst.REFRESHLEFT,this, "ShowLeft");
        ZEventSystem.Register(EventConst.REFRESHRIGHT, this, "ShowRight");
        ZEventSystem.Register(EventConst.TOTEMDATACHANGE, this, "RefreshDivination");
    }
    /// <summary>
    /// CLOSE调用
    /// </summary>
    private void DRegisteEvent()
    {
        ZEventSystem.DeRegister(EventConst.REFRESHSIDE);
        ZEventSystem.DeRegister(EventConst.REFRESHLEFT);
        ZEventSystem.DeRegister(EventConst.REFRESHRIGHT);
        ZEventSystem.DeRegister(EventConst.TOTEMDATACHANGE);
    }

    public void BuildHeroEvent(int heroId)
    {
        if (showID == heroId)
            return;
        if (Herotf != null)
            Destroy(Herotf.gameObject);
        Vector3 v3 = billboard.GetCamPos();
        v3.y += -1.45f;
        v3.z += 10f;
        billboard.Layer = LayerMask.NameToLayer("Hero");
        HeroData data = HeroMgr.GetSingleton().GetHeroData(heroId);

        GameObject heroGo = ResourceMgr.Instance.LoadResource(data.JsonData.resid) as GameObject;
        if (heroGo == null)
            return;
        heroGo = Instantiate(heroGo);
        Herotf = heroGo.transform;
        Herotf.localPosition = v3;
        int horseid = data.JsonData.horseid;
        if (horseid != 0)
        {
            GameObject horseGo = ResourceMgr.Instance.LoadResource(horseid) as GameObject;
            if (horseGo == null)
            {
                EDebug.LogError("加载马匹资源失败 -- " + horseid);
                return;
            }
            horseGo = Instantiate(horseGo, heroGo.transform, false);
            horseGo.transform.localPosition = Vector3.zero;
            horseGo.transform.localScale = Vector3.one;
        }
        HeroShow hs = JsonMgr.GetSingleton().GetHeroShowByID(heroId);
        Vector3 vc = billboard.Cam.transform.eulerAngles;
        vc.x = hs.RotX;
        Quaternion qc = new Quaternion
        {
            eulerAngles = vc
        };
        billboard.Cam.transform.rotation = qc;
        Vector3 vh = heroGo.transform.localEulerAngles;
        vh.y = hs.RotY + +180f;
        Quaternion qh = new Quaternion
        {
            eulerAngles = vh
        };
        heroGo.transform.rotation = qh;
        billboard.Fov = hs.Fov;
        showID = heroId;
        heroGo.SetLayer("Hero");
        player = heroGo.AddComponent<AnimatorPlayer>();
        player.RandomPlay(true);
    }

    public void ShowRight()
    {
        ShowType(showType);
    }
    private void ShowType(EHEROSHOWTYPE type)
    {
        if (showGo != null)
            showGo.SetParent(Hide_trf);
        if (LOfficer_obj.activeInHierarchy)
        {
            LOfficer_obj.SetActive(false);
            Left_obj.SetActive(true);
        }
        if (Divination_obj.activeInHierarchy)
        {
            Divination_obj.SetActive(false);
            Equip_obj.SetActive(true);
        }
        ShowLeft();
        switch (type)
        {
            case EHEROSHOWTYPE.Card:
                ShowCard();
                break;
            case EHEROSHOWTYPE.Officer:
                ShowOfficer();
                break;
            case EHEROSHOWTYPE.Divination:
                ShowDivination();
                break;
            case EHEROSHOWTYPE.Skill:
                ShowSkill();
                break;
            case EHEROSHOWTYPE.Relation:
                ShowRelation();
                break;
            case EHEROSHOWTYPE.Detail:
                ShowDetail();
                break;
        }
        showType = type;
    }

    #region 图鉴
    private void ShowCard()
    {
        showGo = Card_trf;
        showGo.SetParent(Right_trf);

        bigcard_img.sprite = ResourceMgr.Instance.LoadSprite(heroData.JsonData.cardid);
        Worker_img.sprite = ResourceMgr.Instance.LoadSprite(heroData.JsonData.type);

        HeroRare officerUp = JsonMgr.GetSingleton().GetHeroRareByID(heroData.Rare);
        Floor_img.sprite = ResourceMgr.Instance.LoadSprite(officerUp.CardBorder);
        Trans_img.sprite = ResourceMgr.Instance.LoadSprite(officerUp.CardTrans);

        for (int i = 0, length = crArray.Length; i < length; ++i)
        {
            crArray[i].SetAlpha(i < heroData.Star ? 1 : 0);
        }
        ///暂用 武将满星后处理待定....
        int[] matnum = JsonMgr.GetSingleton().GetHeroStarByID(heroData.HeroId).matnum;
        int needPiece = matnum[Mathf.Min(heroData.Star , matnum.Length - 1)];
        Piece_slider.value = heroData.Piece * 1f / needPiece;
        Num_txt.text = heroData.Piece + "/" + needPiece;
        //升星减碎片
        if (heroData.Star >= HeroMgr.MaxHeroStar)
        {
            StarUp_btn.interactable = false;
        }
        else
        {
            StarUp_btn.interactable = true;
            EventListener.Get(StarUp_btn.gameObject).OnClick = go =>
            {
                if(StarUp_btn.interactable)
                    UIFace.GetSingleton().Open(UIID.HeroStarUp, heroData.HeroId);
            };
        }
    }
    #endregion
    #region 左侧区域

    float previousX;
    float offset;

    [Header("武将拖动速度")]
    public float scale = 1f;

    public void ShowLeft()
    {
        Name_txt.text = heroData.JsonData.name;
      
        show_txt.text = heroData.Officer > 0 ? JsonMgr.GetSingleton().GetOfficerByID(heroData.Officer).Post : "";

        //需要获取武将官阶的官职列表并对dd赋值

        int[] unlock = JsonMgr.GetSingleton().GetHeroRareByID(heroData.Rare).UnLock;
 
        for (int i = 0; i < officerText.Length; ++i)
        {
            if (i >= unlock.Length)
            {
                officerText[i].text = "";
                continue;
            }
            Officer o = JsonMgr.GetSingleton().GetOfficerByID(unlock[i]);
            string txt = officerText[i].text = o.Post;

            EventListener.Get(officerText[i].gameObject).OnClick = e =>
            {
                show_txt.text = txt;
                ShowPopList(null);
                HeroMgr.GetSingleton().TakeOfficer(heroData, o.ID);
                if (showType == EHEROSHOWTYPE.Detail)
                    ShowDetail();
            };
        }
        Vector2 size = scrollRect.content.sizeDelta;
        size.y = unlock.Length * 53;
        scrollRect.content.sizeDelta = size;
        EventListener.Get(hero_obj).OnClick = e =>
        {
            if (player != null && !player.IsPlaying)
                player.RandomPlay(true);
        };
        EventListener.Get(hero_obj).BegineDragEvent = e =>
        {
            previousX = Input.mousePosition.x;
        };
        EventListener.Get(hero_obj).DragEvent = e =>
        {
            offset = previousX - Input.mousePosition.x;
            previousX = Input.mousePosition.x;
            Herotf.Rotate(Vector3.up, offset * scale, Space.Self);
        };
        HeroEquipView hev = Equip_obj.GetComponent<HeroEquipView>();
        hev.SetView(heroData);
        EventListener.Get(Dress_btn.gameObject).OnClick = e =>
        {
            EquipData[] equips = heroData.GetEquip();
            for (int i = 0; i < equips.Length; ++i)
            {
                EquipData data = equips[i];
                if(data == null)
                {
                    string md5 = EquipMgr.GetSingleton().TestCanDress((EquipPart)i + 1, heroData);
                    EquipData equip = EquipMgr.GetSingleton().GetEquipDataByUID(md5);
                    if(equip != null)
                    {
                        EquipMgr.GetSingleton().TakeEquip(equip, heroData);
                        hev.SetView(heroData);
                    }
                }
            }
        };
        EventListener.Get(Intensify_btn.gameObject).OnClick = e =>
        {
            EquipData[] equips = heroData.GetEquip();
            bool canUp = false;
            for (int i = 0; i < equips.Length; ++i)
            {
                EquipData data = equips[i];
                if (data == null || data.StrengthenLv == HeroMgr.MaxHeroLevel)
                    continue;
                canUp = true;
                EquipMgr.GetSingleton().UpEquip(data);
                hev.SetView(heroData);
            }
            if(!canUp)
                CanvasView.Instance.AddNotice("暂时没有可强化装备");
        };
    }

    #endregion
    #region 左侧滑动区域
    float maxHeight;
    float maxWidth;
    bool isOnDrag = false;

    public void ShowSide(bool move)
    {
        int index = 0;
        unLockData = HeroMgr.GetSingleton().Heros;

        if (move)
        {
            maxHeight = (unLockData.Length + 0.5f) * (glg.spacing.y + glg.cellSize.y);
            maxWidth = (glg.spacing.x + glg.cellSize.x);
            Vector2 v = rect.sizeDelta;
            v.x = maxWidth;
            v.y = maxHeight;
            rect.sizeDelta = v;
        }
        for (int i = 0; i < unLockData.Length; ++i)
        {
            GameObject go;
            HeroData data = unLockData[i];
            if (instantiate.ContainsKey(data))
            {
                go = instantiate[data];
            }
            else
            {
                go = Instantiate(headGo, LeftContent_obj.transform, false);
                instantiate.Add(data, go);
            }
            if (data.HeroId == heroData.HeroId)
            {
                index = i;
                sideselect_trf.SetParent(go.transform);
                sideselect_trf.localPosition = Vector3.zero;
                if (move)
                {
                    Vector2 v2 = sr.content.anchoredPosition;
                    v2.y = -maxHeight + index * glg.cellSize.y;
                    sr.content.anchoredPosition = v2;
                }
            }
            EventListener.Get(go).OnClick = e =>
            {
                if (isOnDrag)
                    return;
                Close();
                sideselect_trf.SetParent(e.transform);
                sideselect_trf.localPosition = Vector3.zero;
                Open(data.HeroId, showType, false);
            };
            EventListener.Get(go).BegineDragEvent = e =>
            {
                isOnDrag = true;
                sr.OnBeginDrag(e);
            };
            EventListener.Get(go).DragEvent = e =>
            {
                sr.OnDrag(e);
            };
            EventListener.Get(go).EndDragEvent = e =>
            {
                isOnDrag = false;
                sr.OnEndDrag(e);
            };

            HeroHeadView hhv = go.GetComponent<HeroHeadView>();
            hhv.Init();
            hhv.SetHeroInfo(data.HeroId, data.Rare, data.Star, data.Level);
        }
        //算法还没有
        Power_txt.text = "583138";
        LeftLevel_txt.text = heroData.Level.ToString();
        int needExp = JsonMgr.GetSingleton().GetExpByID(Mathf.Max(1, heroData.Level)).NeedHero;
        Exp_slider.value = (heroData.Exp * 100 / needExp * 100) * 0.01f;
        Exp_txt.text = heroData.Exp + "/" + needExp;
        if (showType == EHEROSHOWTYPE.Card)
            ShowCard();
        else if (showType == EHEROSHOWTYPE.Divination)
            ShowDivination();
        else if (showType == EHEROSHOWTYPE.Detail)
            ShowDetail();
    }
    #endregion
    #region 官职面板
    private void ShowOfficer()
    {
        showGo = Officer_trf;
        showGo.SetParent(Right_trf);

        LOfficer_obj.SetActive(true);
        Left_obj.SetActive(false);
        HeroRare ou = JsonMgr.GetSingleton().GetHeroRareByID(heroData.Rare);
        HeroRare up = JsonMgr.GetSingleton().GetHeroRareByID(Mathf.Min(heroData.Rare + 1));
        if (ou == null)
            return;
        if (up == null)
            return;
        currentleft_img.sprite = currentright_img.sprite = ResourceMgr.Instance.LoadSprite(ou.Officer);
        upleft_img.sprite = upright_img.sprite = ResourceMgr.Instance.LoadSprite(up.Officer);
        current_txt.text = ou.Name.AddColorLabel(ou.Color);
        up_txt.text = up.Name.AddColorLabel(up.Color);
        HeroHeadView hhvL = LeftHead_trf.GetComponent<HeroHeadView>();
        HeroHeadView hhvR = RightHea_trf.GetComponent<HeroHeadView>();
        hhvL.Init();
        hhvR.Init();
        hhvL.SetHeroInfo(heroData.JsonData.headid, heroData.Rare, heroData.Star, heroData.Level, true);
        hhvR.SetHeroInfo(heroData.JsonData.headid, heroData.Rare + 1, heroData.Star, heroData.Level, true);
        needhonor_txt.color = Role.Instance.Honor < up.NeedNum ? Color.red : Color.white;
        needhonor_txt.text = up.NeedNum.ToString("N0");
        EventListener.Get(up_btn.gameObject).OnClick = e =>
        {
            if (heroData.Rare >= JsonMgr.GetSingleton().GetGlobalIntArrayByID(1023).value)
            {
                CanvasView.Instance.AddNotice("高处不胜寒呐~~");
                return;
            }
            if (Role.Instance.Honor < up.NeedNum)
            {
                CanvasView.Instance.AddNotice("荣誉不足!!");
                return;
            }
            HeroMgr.GetSingleton().UnfixOfficer(heroData);
            heroData.Rare++;
            ShowOfficer();
            ShowSide(false);
        };
        Dictionary<Attr, float> ret = heroData.RareAttr(false);
        StringBuilder sb = new StringBuilder();
        foreach (var item in ret)
        {
            if (item.Value != 0)
            {
                sb.Append(AttrUtil.GetAttribute(item.Key));
                sb.Append(" ");
                sb.Append(AttrUtil.ShowText(item.Key,item.Value));
                sb.Append("\n");
            }
        }
        leftpro_txt.text = sb.ToString();
        Dictionary<Attr, float> next = heroData.RareAttr(true);
        sb.Length = 0;
        foreach (var item in next)
        {
            if (item.Value != 0)
            {
                sb.Append(AttrUtil.GetAttribute(item.Key));
                sb.Append(" ");
                sb.Append(AttrUtil.ShowText(item.Key, item.Value));
                sb.Append(" ");
                float befor = 0;
                if (ret.ContainsKey(item.Key))
                    befor = ret[item.Key];
                sb.Append(string.Format(@"<color=#00FF00> (+{0})</color>", AttrUtil.ShowText(item.Key, item.Value - befor)));
                sb.Append("\n");
            }
        }
        rightpro_txt.text = sb.ToString();

        int[] officers = HeroMgr.GetSingleton().GetUnLockOfficer(heroData.Rare);
        int childCount = OfficerList_obj.transform.childCount;
        List<Officer> os = SortPost(officers);
        for (int i = 0; i < os.Count; ++i)
        {
            Officer o = os[i];
            if (o == null)
                continue;
            GameObject itemgo;
            if (i < childCount)
            {
                itemgo = OfficerList_obj.transform.GetChild(i).gameObject;
            }
            else
            {
                itemgo = Instantiate(officerGo, OfficerList_obj.transform, false);
            }
            OfficerItem oi = itemgo.GetComponent<OfficerItem>();

            oi.SetView(heroData, o);
            EventListener.Get(oi.take_btn.gameObject).OnClick = e =>
            {
                int takeID = HeroMgr.GetSingleton().IsTake(o.ID);
                if (takeID == heroData.HeroId)
                    return;
                if (takeID != 0)
                {
                    string name = HeroMgr.GetSingleton().GetHeroData(takeID).JsonData.name;
                    TipCtrl ctrl = (TipCtrl)UIFace.GetSingleton().Open(UIID.Tip, 1, name);
                    ctrl.SetHandler(delegate () { UIFace.GetSingleton().Close(UIID.Tip); },
                        delegate ()
                        {
                            UIFace.GetSingleton().Close(UIID.Tip);
                            HeroMgr.GetSingleton().TakeOfficer(heroData, o.ID);
                            ShowOfficer();
                        });
                }
                else
                {
                    HeroMgr.GetSingleton().TakeOfficer(heroData, o.ID);
                    ShowOfficer();
                }
            };
            EventListener.Get(itemgo).OnClick = e =>
            {
                oi.OnClickItem();
                float minu = (oi.highlightY - oi.normalY);
                float height = oi.down ? minu : -minu;
                officerrt.sizeDelta = new Vector2(officerrt.sizeDelta.x, officerrt.sizeDelta.y + height);
            };
            if (oi.down)
                oi.OnClickItem();
            EventListener.Get(itemgo).BegineDragEvent = e =>
            {
                OfficerSV_sr.OnBeginDrag(e);
            };
            EventListener.Get(itemgo).DragEvent = e =>
            {
                OfficerSV_sr.OnDrag(e);
            };
            EventListener.Get(itemgo).EndDragEvent = e =>
            {
                OfficerSV_sr.OnEndDrag(e);
            };
        }
        float maxHeight = (officers.Length + 0.5f) * (10.6f + 101);
        officerrt.sizeDelta = new Vector2(officerrt.sizeDelta.x, maxHeight);
    }

    private List<Officer> SortPost(int[] officers)
    {
        List<Officer> ret = new List<Officer>();
        for (int i = 0; i < officers.Length; ++i)
        {
            Officer o = JsonMgr.GetSingleton().GetOfficerByID(officers[i]);
            ret.Add(o);
        }
        ret.Sort((Officer a, Officer b) =>
        {
            int takeA = HeroMgr.GetSingleton().IsTake(a.ID);
            int takeB = HeroMgr.GetSingleton().IsTake(b.ID);
            if (takeA == 0 && takeB == 0)
            {
                if (a.ID < b.ID)
                    return -1;
                return 1;
            }
            else if (takeA != 0 && takeB == 0)
            {
                if (takeA == heroData.HeroId)
                    return -1;
                return 1;
            }
            else if (takeA == 0 && takeB != 0)
            {
                if (takeB == heroData.HeroId)
                    return 1;
                return -1;
            }
            else if (takeA != 0 && takeB != 0 && takeA != takeB)
            {
                if (takeA == heroData.HeroId)
                    return -1;
                if (takeB == heroData.HeroId)
                    return 1;
            }
            return 0;
        });

        return ret;
    }

    #endregion
    #region 卜卦面板
    public void RefreshDivination()
    {
        ShowDivination();
    }

    private void ShowDivination()
    {
        showGo = Divination_trf;
        showGo.SetParent(Right_trf);
        Divination_obj.SetActive(true);
        Equip_obj.SetActive(false);
        HeroDivinationView hdv = Divination_obj.GetComponent<HeroDivinationView>();
        hdv.SetHeroTotemView(heroData);
        EventListener.Get (gotoDiv_btn.gameObject).OnClick = e =>
        {
            UIFace.GetSingleton().Open(UIID.Divination);
        };
        int count = divparent_trf.childCount;
        TotemData[] mData = TotemMgr.GetSingleton().GetUnDressTotem();
        while(count > mData.Length)
        {
            DestroyImmediate(divparent_trf.GetChild(0).gameObject);
            count = divparent_trf.childCount;
        }
        for (int i = 0; i < mData.Length; ++i)
        {
            TotemData data = mData[i];
            if (data.HeroID != 0)
                continue;
            GameObject totem;
            if (i < count)
            {
                totem = divparent_trf.GetChild(i).gameObject;
            }
            else
            {
                totem = Instantiate(totemGo, divparent_trf);
            }
            DivinationItemView div = totem.GetComponent<DivinationItemView>();
            div.SetView(data);
            EventListener.Get(totem).OnClick = e =>
            {
                ZEventSystem.Register(EventConst.TAKETOTEM,this, "TakeTotem");
                UIFace.GetSingleton().Open(UIID.DivinationTip, data, SHOWBUTTON.Etake);
            };
            EventListener.Get(totem).BegineDragEvent = e =>
            {
                divination_sr.OnBeginDrag(e);
            };
            EventListener.Get(totem).DragEvent = e =>
            {
                divination_sr.OnDrag(e);
            };
            EventListener.Get(totem).EndDragEvent = e =>
            {
                divination_sr.OnEndDrag(e);
            };
        }

        Vector2 size = divination_sr.content.sizeDelta;
        size.y = Mathf.CeilToInt(mData.Length / 3f) * 187 + 65;
        divination_sr.content.sizeDelta = size;

        EventListener.Get(take_btn.gameObject).OnClick = e=>
        {
            if (mData.Length <= 0)
                return;
            int index = mData.Length - 1;
            TotemData[] totems = heroData.GetTotem();
            if(totems.Length == 0)
            {
                CanvasView.Instance.AddNotice("没有可用的气运槽");
                return;
            }
            for (int i = 0,length = totems.Length; i < length; ++i)
            {
                TotemMgr.GetSingleton().TakeTotem(mData[i], heroData);
            }
            ShowSide(false);
        };
        EventListener.Get(takeoff_btn.gameObject).OnClick = e =>
        {
            TotemData[] totems = heroData.GetTotem();
            for (int i = 0, length = totems.Length; i < length; ++i)
            {
                if (totems[i] == null)
                    continue;
                TotemMgr.GetSingleton().TakeOffTotem(totems[i]);
            }
            ShowSide(false);
        };
    }
    public void TakeTotem(TotemData data)
    {
        TotemMgr.GetSingleton().TakeTotem(data, heroData);
        ShowDivination();
    }
    #endregion
    #region  技能面板
    private void ShowSkill()
    {
        showGo = Skill_trf;
        showGo.SetParent(Right_trf);

        int[] skills = heroData.JsonData.skills;
        click_trf.localPosition = skillImage[0].transform.localPosition;
        for (int i = 1; i < skills.Length; i++)
        {
            int skillID = skills[i];
            //Skill skill = JsonMgr.GetSignleton().GetSkillByID(skillID);
            skillImage[i - 1].sprite = ResourceMgr.Instance.LoadSprite(skillID);
            EventListener.Get(skillImage[i - 1].gameObject).OnClick = e =>
             {
                 SkillChose(skillID);
                 click_trf.localPosition = e.transform.localPosition;
             };
        }
        SkillChose(skills[1]);
    }
    private void SkillChose(int skillId)
    {
        Skill skill = JsonMgr.GetSingleton().GetSkillByID(skillId);
        skillname_txt.text = skill.name;
        int skillLv = heroData.SkillLevel[skill.ID];
        skilllv_txt.text = skillLv.ToString();
        //经验花费等级+5的4次幂 * 0.01
        int expcost = Mathf.RoundToInt(Mathf.Pow(skillLv + 5, 4) * 0.01f);
        expcost_txt.text = expcost.ToString();
        int moneyconst = skillLv * 500;
        moneycost_txt.text = moneyconst.ToString();
        //2被动 3自动 4主动
        string type = "被动";
        if (skill.type == 3)
            type = "自动";
        else if (skill.type == 4)
            type = "主动";
        condition_txt.text = type;
        string desc = skill.desc;
        var o = new List<string>();
        int start = 0;
        for (int i = 0; i < desc.Length; ++i)
        {
            if (desc[i].Equals('{'))
                start = i;
            else if (desc[i].Equals('}') && start != 0)
            {
                o.Add(desc.Substring(start + 1, i - start - 1));
                start = 0;
            }
        }
        var n = new string[o.Count];
        for (int i = 0; i < o.Count; i++)
        {
            string[] sp = o[i].Split(':');
            int tostring = int.Parse(sp[0]);
            sp = sp[1].Split('|');
            float att = AttrUtil.SkillPanelShow(float.Parse(sp[0]), skillLv, float.Parse(sp[1]));
            string s = string.Format("{0:###.#}", att * (tostring == 1 ? 100 : 1));
            n[i] = s + (tostring == 1 ? "%" : "");
        }
        for (int i = 0; i < n.Length; i++)
        {
            desc = desc.Replace(o[i], n[i]);
        }
        desc = desc.Replace("{", "").Replace("}", "");
        tell_txt.supportRichText = true;
        tell_txt.text = desc;
        if (heroData.SkillLevel[skill.ID] >= HeroMgr.MaxHeroLevel)
        {
            lvup_btn.interactable = false;
            return;
        }
        lvup_btn.interactable = true;
        EventListener.Get(lvup_btn.gameObject).OnClick = e =>
        {

            if (!lvup_btn.interactable)
            {
                CanvasView.Instance.AddNotice("技能满级");
                return;
            }
            //检测技能升级条件
            long cash = Role.Instance.Cash;
            long expPool = Role.Instance.ExpPool;
            if (expcost > expPool)
            {
                CanvasView.Instance.AddNotice("经验不足，技能升级失败!");
                return;
            }
            if (moneyconst > cash)
            {
                CanvasView.Instance.AddNotice("金币不足，技能升级失败!");
                return;
            }
            //通知服务器技能升级
            heroData.SkillLevel[skill.ID]++;
            SkillChose(skillId);
        };
    }
    #endregion
    #region 羁绊面板
    private void ShowRelation()
    {
        showGo = Relation_trf;
        showGo.SetParent(Right_trf);

    }
    #endregion
    #region 详情面板
    private void ShowDetail()
    {
        showGo = Detail_trf;
        showGo.SetParent(Right_trf);
        Text[] text = HeroName_txt.GetComponentsInChildren<Text>();
        text[0].text = heroData.JsonData.name;

        string desc = heroData.JsonData.desc;
        string[] str = desc.Split('#');
        int index = 1;
        for (int i = 0, length = str[0].Length, idx; i < length; i = idx)
        {
            int subLenght = Mathf.Min(str[0].Length - i, 7);
            idx = Mathf.Min(length, i + subLenght);
            text[index].text = str[0].Substring(i, subLenght);
            index++;
            text[index].fontSize = 30;
            text[index].alignment = TextAnchor.UpperCenter;
        }
        text[index].fontSize = 26;
        text[index].text = str[1];
        text[index].alignment = TextAnchor.LowerCenter;
        for (int i = index + 1; i < text.Length; ++i)
        {
            text[i].text = "";
        }
        Dictionary<Attr, float> attr = heroData.AttrDic;

        Tongshuai_txt.text = Mathf.RoundToInt(attr[Attr.LeadPower]).ToString();
        Zhili_txt.text = Mathf.RoundToInt(attr[Attr.Strategy]).ToString();
        Wuli_txt.text = Mathf.RoundToInt(attr[Attr.Power]).ToString();
        HeroDetaiAttr[] detailTxt = detailtxt_trf.GetComponentsInChildren<HeroDetaiAttr>();
        ScrollRect scroll = detailtxt_trf.GetComponentInParent<ScrollRect>();
        StringBuilder sb = new StringBuilder();
        Attr[] attra = new Attr[attr.Count];
        float[] num = new float[attr.Count];
        attr.Keys.CopyTo(attra, 0);
        attr.Values.CopyTo(num, 0);
        int txtIdx = 0;
        for (int i = 3; i < attr.Count; ++i)
        {
            if (num[i] <= 0)
                continue;
            detailTxt[txtIdx].AttrTxet(scroll, attra[i], num[i]);
            ++txtIdx;
        }
        for (int i = txtIdx; i < detailTxt.Length; i++)
        {
            detailTxt[i].Clear();
        }
        Vector2 size = attrDetail_sr.content.sizeDelta;
        size.y = Mathf.CeilToInt(txtIdx * 0.5f) * 50 + 25;
        attrDetail_sr.content.sizeDelta = size;
    }
    #endregion

    public void Close()
    {
        for (int i = 0, length = divparent_trf.childCount; i < length; ++i)
        {
            DestroyImmediate(divparent_trf.GetChild(0).gameObject);
        }
        DRegisteEvent();
        heroData = null;
        player = null;
    }
    public void OnDestroy()
    {
        instantiate.Clear();
    }
}
