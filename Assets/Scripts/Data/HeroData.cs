using JsonData;
using Msg.HeroMsg;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武将数据
/// </summary>
public class HeroData
{
    //武将id
    private int _heroID;
    public int HeroId { get { return _heroID; } private set { } }

    public HeroData(int heroid)
    {
        _heroID = heroid;
        int[] skills = JsonData.skills;
        SkillLevel = new Dictionary<int, int>();
        for (int i = 0; i < skills.Length; ++i)
        {
            SkillLevel.Add(skills[i], 1);
        }

        Totems = new string[0];
        int[] equip = JsonData.equip;
        Equips = new string[equip.Length];

        for (int i = 0, length = equip.Length; i < length; ++i)
        {
            EquipData data = new EquipData(equip[i])
            {
                md5 = equip[i] + heroid.ToString(),
                HeroId = heroid
            };
            EquipMgr.GetSingleton().AddEquip(data);
            Equips[i] = data.md5;
        }

        Piece = 500;
        UnLock = true;
    }
    public HeroData(HeroMsg heroMsg)
    {
        HeroId = heroMsg.heroID;
        Star = heroMsg.star;
        Officer = heroMsg.officer;
        Rare = heroMsg.rare;
        Level = heroMsg.level;
        Exp = heroMsg.exp;
        Piece = heroMsg.piece;
        Equips = heroMsg.equips;
        Totems = heroMsg.totems;
        SkillLevel = new Dictionary<int, int>();
        for (int i = 0; i < heroMsg.skillInfo.Length; ++i)
        {
            SkillInfo info = heroMsg.skillInfo[i];
            SkillLevel.Add(info.skillID, info.lv);
        }
        UnLock = true;
    }

    private bool _unLock = false;
    public bool UnLock { get { return _unLock; } private set { _unLock = value; } }


    private Hero data;

    public Hero JsonData
    {
        get
        {
            if (data == null)
                data = JsonMgr.GetSingleton().GetHeroByID(HeroId);
            return data;
        }
    }
    private HeroStar starGrow;
    public HeroStar StarGrow
    {
        get
        {
            if (starGrow == null)
                starGrow = JsonMgr.GetSingleton().GetHeroStarByID(HeroId);
            return starGrow;
        }
    }

    #region Get Max
    //等级
    private int level;
    public int Level
    {
        get
        {
            return Mathf.Max(1, level);
        }
        set
        {
            level = value;
            int unLockNum = HeroMgr.GetSingleton().GetUnLockTotemGrooveNum(level);
            if (unLockNum == Totems.Length)
                return;
            string[] totems = new string[unLockNum];
            Totems.CopyTo(totems, 0);
            for (int i = Totems.Length; i < unLockNum; ++i)
            {
                totems[i] = "";
            }
            Totems = totems;
            ZEventSystem.Dispatch(EventConst.HEROINFOCHANGE);
        }
    }
    //稀有度
    private int rare;
    public int Rare { get { return (int)Mathf.Clamp(rare, 1f, JsonMgr.GetSingleton().GetGlobalIntArrayByID(1023).value); } set { rare = value; nextRare = rareAttr = null; ZEventSystem.Dispatch(EventConst.HEROINFOCHANGE); } }

    //星级
    private int star;
    public int Star { get { return Mathf.Max(JsonData.initstar, star); } set { star = value; ZEventSystem.Dispatch(EventConst.HEROINFOCHANGE); } }

    //官职
    public int Officer { get; set; }
    //经验
    public int Exp { get; private set; }

    private int piece;
    //拥有碎片数量
    public int Piece { get { return piece; } set { piece = value; ZEventSystem.Dispatch(EventConst.HEROINFOCHANGE); } }
    #endregion

    //升级所需经验
    public int NeedHero { get { return JsonMgr.GetSingleton().GetExpByID(Level).NeedHero; } }

    //装备管理器  1、头盔Helmet 2、身体Body 3、战靴Shoes 4、武器Weapon 5、虎符Hufu 6、宝物Treasure 7、坐骑Mount 8、经典Book
    public string[] Equips { get; set; }
    public EquipData[] GetEquip()
    {
        return EquipMgr.GetSingleton().GetHeroEquip(this);
    }

    public string[] Totems { get; set; }
    public TotemData[] GetTotem()
    {
        return TotemMgr.GetSingleton().GetHeroTotem(Totems);
    } 
    
    public string this[int eot,int index]
    {
        get
        {
            switch(eot)
            {
                case 0:
                    return Equips[index];
                case 1:
                    return Totems[index];
                default:
                    throw new System.Exception("eot输入不正确。 0取equip 1取totem");
            }
        }
        set
        {
            switch (eot)
            {
                case 0:
                    Equips[index] = value;
                    ClearEquipAttr();
                    ZEventSystem.Dispatch(EventConst.REFRESHRIGHT);
                    break;
                case 1:
                    Totems[index] = value;
                    ClearTotemAttr();
                    break;
                default:
                    throw new System.Exception("eot输入不正确。 0取equip 1取totem");
            }

        }
    }
    
    //技能等级
    public Dictionary<int, int> SkillLevel { get; private set; }

    private Dictionary<Attr, float> plusDic = new Dictionary<Attr, float>();
    private Dictionary<Attr, float> mulDic = new Dictionary<Attr, float>();

    public Dictionary<Attr, float> AttrDic
    {
        get { return ComputerAttr(); }
        private set { }
    }
    private Dictionary<Attr, float> ComputerAttr()
    {
        plusDic.Clear();
        mulDic.Clear();
         //只为调整显示顺序
        Dictionary<Attr, float> ret = new Dictionary<Attr, float>
        {
            {Attr.Power,0 },
            {Attr.Strategy,0 },
            {Attr.LeadPower,0 },
            {Attr.MaxHp,0 },
            {Attr.Asp,0 },
            {Attr.Atk,0},
            {Attr.DefBreak,0},
            {Attr.Matk,0},
            {Attr.MdefBreak,0},
            {Attr.Def,0},
            {Attr.DefRate,0},
            { Attr.MDef,0},
            {Attr.MdefRate,0},
            {Attr.DodgeRate,0},
            {Attr.HitRate,0},
            {Attr.BlockRate,0},
            {Attr.RoutRate,0} ,
            {Attr.CritRate,0},
            {Attr.CritInc,0},
            {Attr.FirmRate,0},
            {Attr.HealRate,0},
            {Attr.HpRec,0},
            {Attr.VigourRec,0},
            {Attr.HpSuck,0},
            {Attr.VigourSuck,0}
        };

        List<Pro> ep = EquipAttr();
        Dictionary<Attr, float> oa = OfficerAttr();
        Dictionary<Attr, float> ha = HeroAttr();
        Dictionary<Attr, float> ra = RareAttr(false);

        int power = 0;
        int strategy = 0;
        int leadPower = 0;
        //计算武力
        if (ha.ContainsKey(Attr.Power))
            power += (int)ha[Attr.Power];
        if (ra.ContainsKey(Attr.Power))
            power += (int)ra[Attr.Power];
        if (oa.ContainsKey(Attr.Power))
            power += (int)oa[Attr.Power];
        //计算策略
        if (ha.ContainsKey(Attr.Strategy))
            strategy += (int)ha[Attr.Strategy];
        if (ra.ContainsKey(Attr.Strategy))
            strategy += (int)ra[Attr.Strategy];
        if (oa.ContainsKey(Attr.Strategy))
            strategy += (int)oa[Attr.Strategy];
        //计算统率
        if (ha.ContainsKey(Attr.LeadPower))
            leadPower += (int)ha[Attr.LeadPower];
        if (ra.ContainsKey(Attr.Strategy))
            leadPower += (int)ra[Attr.LeadPower];
        if (oa.ContainsKey(Attr.LeadPower))
            leadPower += (int)oa[Attr.LeadPower];

        for (int i = 0; i < ep.Count; ++i)
        {
            if (ep[i].attr == Attr.Power)
            {
                power += (int)ep[i].num;
                power = Mathf.RoundToInt(power * (1f + ep[i].per));
            }
            else if (ep[i].attr == Attr.Strategy)
            {
                strategy += (int)ep[i].num;
                strategy = Mathf.RoundToInt(strategy * (1f + ep[i].per));
            }
            else if (ep[i].attr == Attr.LeadPower)
            {
                leadPower += (int)ep[i].num;
                leadPower = Mathf.RoundToInt(leadPower * (1f + ep[i].per));
            }
            else
            {
                if (plusDic.ContainsKey(ep[i].attr))
                {
                    plusDic[ep[i].attr] += ep[i].num;
                }
                else
                {
                    plusDic.Add(ep[i].attr, ep[i].num);
                }
                if (mulDic.ContainsKey(ep[i].attr))
                {
                    mulDic[ep[i].attr] += ep[i].per;
                }
                else
                {
                    mulDic.Add(ep[i].attr, ep[i].per);
                }
            }
        }
        List<Pro> tp = TotemAttr();
        for (int i = 0; i < tp.Count; ++i)
        {
            if (plusDic.ContainsKey(tp[i].attr))
            {
                plusDic[tp[i].attr] += tp[i].num;
            }
            else
            {
                plusDic.Add(tp[i].attr, tp[i].num);
            }
            if (mulDic.ContainsKey(tp[i].attr))
            {
                mulDic[tp[i].attr] += tp[i].per;
            }
            else
            {
                mulDic.Add(tp[i].attr, tp[i].per);
            }
        }


        float Def = AttrUtil.CalDef((int)power, Level, StarGrow.stardef[Star - 1]);
        float MdefRate = AttrUtil.CalMdef((int)strategy, Level, StarGrow.starmdef[Star - 1]);

        int maxHp = AttrUtil.CalMaxHp((int)leadPower, Level, StarGrow.starhp[Star - 1]);    //最大生命值
        float atk = AttrUtil.CalAtk((int)power, Level, StarGrow.staratk[Star - 1]); //物理攻击力
        float matk = AttrUtil.CalMatk((int)strategy, Level, StarGrow.starmatk[Star - 1]);  //策略攻击力

        float mdefRatr = AttrUtil.CalMdefRate(MdefRate, Level);
        float defRate = AttrUtil.CalDefRate(Def, Level);  //物理抗性
        float dodgeRate = AttrUtil.CalDodgeRate((int)leadPower, Level),  //闪避率       
            blockRate = AttrUtil.CalBlockRate((int)leadPower, Level), //格挡率    
            firmRate = AttrUtil.CalFirmRate((int)leadPower, Level), //韧性率
            hitRate = AttrUtil.CalHitRate((int)leadPower, Level), //命中率 
            routRate = AttrUtil.CalRoutRate((int)leadPower, Level),  //破击率   
            critRate = AttrUtil.CalCritRate((int)leadPower, Level);   //暴击率


        foreach (var item in ha)
        {
            if (ret.ContainsKey(item.Key))
                ret[item.Key] += item.Value;
            else
                ret.Add(item.Key, item.Value);

        }
        foreach (var item in ra)
        {
            if (ret.ContainsKey(item.Key))
                ret[item.Key] += item.Value;
            else
                ret.Add(item.Key, item.Value);
        }
        foreach (var item in oa)
        {
            if (ret.ContainsKey(item.Key))
                ret[item.Key] += item.Value;
            else
                ret.Add(item.Key, item.Value);
        }
        foreach (var item in plusDic)
        {
            if (ret.ContainsKey(item.Key))
                ret[item.Key] += item.Value;
            else
                ret.Add(item.Key, item.Value);
        }
        foreach (var item in mulDic)
        {
            if (ret.ContainsKey(item.Key))
                ret[item.Key] = ret[item.Key] * (1 + item.Value);
        }

        //增加最大生命值
        if (ret.ContainsKey(Attr.MaxHp))
            ret[Attr.MaxHp] += maxHp;
        //物理攻击计算
        if (ret.ContainsKey(Attr.Atk))
            ret[Attr.Atk] += atk;
        //策略攻击
        if (ret.ContainsKey(Attr.Matk))
            ret[Attr.Matk] += matk;
        //物理抗性
        if (ret.ContainsKey(Attr.DefRate))
            ret[Attr.DefRate] += defRate;
        //策略抗性
        if (ret.ContainsKey(Attr.MdefRate))
            ret[Attr.MdefRate] += mdefRatr;
        //闪避率 
        if (ret.ContainsKey(Attr.DodgeRate))
            ret[Attr.DodgeRate] += dodgeRate;
        //格挡率 
        if (ret.ContainsKey(Attr.BlockRate))
            ret[Attr.BlockRate] += blockRate;
        //韧性率 
        if (ret.ContainsKey(Attr.FirmRate))
            ret[Attr.FirmRate] += firmRate;
        //命中率 
        if (ret.ContainsKey(Attr.HitRate))
            ret[Attr.HitRate] += hitRate;
        //破击率 
        if (ret.ContainsKey(Attr.RoutRate))
            ret[Attr.RoutRate] += routRate;
        //暴击率 
        if (ret.ContainsKey(Attr.CritRate))
            ret[Attr.CritRate] += critRate;
        if (ret.ContainsKey(Attr.AspRate))
            ret[Attr.Asp] *= (1 + ret[Attr.AspRate]);
        return ret;
    }
    //******************************更改装备需要置空字典*************************************//
    //************武将自身 --  heroDic ******** 不需要置空*********************************//
    //************官职更改时 --  officerAttr ******** 需要置空*********************************//
    //************官阶更改时 --  rarerAttr ******** 需要置空*********************************//
    //************装备更改时 --  equipAttr ******** 需要置空*********************************//
    #region 武将自身属性 HeroAttr()
    private Dictionary<Attr, float> heroAttr;
    public Dictionary<Attr, float> HeroAttr()
    {
        if (heroAttr == null)
        {
            float Def = AttrUtil.CalDef(JsonData.power, Level, StarGrow.stardef[Star - 1]);
            float MdefRate = AttrUtil.CalMdef(JsonData.strategy, Level, StarGrow.starmdef[Star - 1]);
            heroAttr = new Dictionary<Attr, float>
            {
                { Attr.LeadPower, JsonData.leadpower }, //统率
                { Attr.Power, JsonData.power }, //武力         
                { Attr.Strategy, JsonData.strategy },   //智力
                { Attr.VigourSuck,JsonData.vigoursuck},// 士气吸取(激励)
                { Attr.DefBreak,JsonData.defbreak},//物理穿透
                { Attr.MdefBreak, JsonData.mdefbreak }, //策略穿透
                { Attr.VigourRec,JsonData.vigourrec },//士气恢复
                { Attr.CritInc, JsonData.critinc + AttrUtil.BASE_CRITINC},//暴击伤害
                { Attr.HpRec,JsonData.hprec }, //生命恢复
                { Attr.HealRate, JsonData.healrate }, //治疗承受
                { Attr.HpSuck,JsonData.hpsuck },//吸血
                { Attr.Asp, JsonData.asp }, //攻击速度
                { Attr.HarmRate, JsonData.harmrate },//伤害承受
                { Attr.DodgeRate,JsonData.dodge},
                { Attr.HitRate,JsonData.hit},
                { Attr.BlockRate,JsonData.block},
                { Attr.RoutRate,JsonData.rout} ,
                { Attr.CritRate,JsonData.crit},
                { Attr.FirmRate,JsonData.firm},
            };
        }
        return heroAttr;
    }
    private float GetHeroProperty(Attr attr)
    {
        if (HeroAttr().ContainsKey(attr))
            return HeroAttr()[attr];
        return 0;
    }
    public void ClearHeroAttr()
    {
        heroAttr.Clear();
        heroAttr = null;
    }
    #endregion
    #region 官职属性 OfficerAttr()
    /// <summary>
    /// 获取官职属性(有且只有一个武将装备)
    /// </summary>
    /// <returns></returns>
    private Dictionary<Attr, float> officerAttr;
    public Dictionary<Attr, float> OfficerAttr()
    {
        if (officerAttr == null)
        {
            do
            {
                officerAttr = new Dictionary<Attr, float>();
                Officer o = JsonMgr.GetSingleton().GetOfficerByID(Officer);
                if (o == null)
                    break;
                for (int i = 0, length = o.attrerty.Length; i < length; ++i)
                {
                    Pro pro = o.attrerty[i];
                    officerAttr.Add(pro.attr, pro.num);
                }
            } while (false);
        }
        return officerAttr;
    }
    public void ClearOfficerAttr()
    {
        if (officerAttr != null)
        {
            officerAttr.Clear();
            officerAttr = null;
        }
    }
    #endregion
    #region 官阶属性 RareAttr(bool next)
    private Dictionary<Attr, float> rareAttr;
    private Dictionary<Attr, float> nextRare;
    /// <summary>
    /// 获取官阶属性（每个武将不同）
    /// 增加值都为绝对值 
    /// </summary>
    /// <returns>返回结果用于右侧显示</returns>
    public Dictionary<Attr, float> RareAttr(bool next)
    {
        if (rareAttr == null || nextRare == null)
        {
            rareAttr = CalculateAttr();
            nextRare = CalculateAttr(1);
        }
        return next ? nextRare : rareAttr;
    }

    private Dictionary<Attr, float> CalculateAttr(int moveIndex = 0)
    {
        int r = Rare + moveIndex;
        OfficerPro op = JsonMgr.GetSingleton().GetOfficerProByID(HeroId);
        float MaxHp = 0;
        float Atk = 0;
        float Matk = 0;
        float DefBreak = 0;
        float MdefBreak = 0;
        float DefRate = 0;
        float MdefRate = 0;
        float VigourRec = 0;
        float DodgeRate = 0;
        float BlockRate = 0;
        float FirmRate = 0;
        float HitRate = 0;
        float RoutRate = 0;
        float CritRate = 0;
        float CritInc = 0;
        float HpRec = 0;
        float HealRate = 0;
        float HpSuck = 0;
        float AspRate = 0;
        for (int i = 1; i <= r; ++i)
        {
            MaxHp += CalculateProperty(op.maxhp, i);
            Atk += CalculateProperty(op.atk, i);
            Matk += CalculateProperty(op.matk, i);
            DefBreak += CalculateProperty(op.defbreak, i);
            MdefBreak += CalculateProperty(op.mdefbreak, i);
            DefRate += CalculateProperty(op.defrate, i);
            MdefRate += CalculateProperty(op.mdefrate, i);
            VigourRec += CalculateProperty(op.vigourrec, i);
            DodgeRate += CalculateProperty(op.dodgerate, i);
            BlockRate += CalculateProperty(op.blockrate, i);
            FirmRate += CalculateProperty(op.firmrate, i);
            HitRate += CalculateProperty(op.hitrate, i);
            RoutRate += CalculateProperty(op.routrate, i);
            CritRate += CalculateProperty(op.critrate, i);
            CritInc += CalculateProperty(op.critinc, i);
            HpRec += CalculateProperty(op.hprec, i);
            HealRate += CalculateProperty(op.healrate, i);
            HpSuck +=  CalculateProperty(op.hpsuck, i);
            AspRate += CalculateProperty(op.asprate, i);
        }

        Dictionary<Attr, float> ret = new Dictionary<Attr, float>
        {
            { Attr.MaxHp, MaxHp },//最大生命值
            { Attr.Atk, Atk },//物理攻击力
            { Attr.Matk,Matk},  //策略攻击力
            { Attr.DefBreak, DefBreak },//物理穿透
            { Attr.MdefBreak, MdefBreak }, //策略穿透
            { Attr.DefRate, DefRate },//物理抗性
            { Attr.MdefRate, MdefRate }, //策略抗性
            { Attr.VigourRec,VigourRec },//士气恢复
            { Attr.DodgeRate,DodgeRate },//闪避率
            { Attr.BlockRate,BlockRate },//格挡率
            { Attr.FirmRate, FirmRate}, //韧性率
            { Attr.HitRate,HitRate },//命中率
            { Attr.RoutRate,RoutRate }, //破击率
            { Attr.CritRate,CritRate  },//暴击率
            { Attr.CritInc,CritInc},//暴击伤害
            { Attr.HpRec,HpRec}, //生命恢复
            { Attr.HealRate, HealRate}, //治疗承受
            { Attr.HpSuck, HpSuck },//吸血
            { Attr.AspRate,AspRate}, //攻击速度
        };
        return ret;
    }
    /// <summary>
    /// 计算官阶的属性
    /// </summary>
    /// <param name="baseNum">基础表属性</param>
    /// <returns>下一官阶的属性 </returns>
    private float CalculateProperty(float baseNum, int r)
    {
        return Mathf.Pow(baseNum, 1.1f) * (r - 1);
    }

    #endregion
    #region 装备属性 EquipAttr()
    private List<Pro> equipAttr;
    public List<Pro> EquipAttr()
    {
        if (equipAttr == null)
        {
            equipAttr = new List<Pro>();
            EquipData[] equips = GetEquip();
            for (int i = 0, length = equips.Length; i < length; ++i)
            {
                EquipData ed = equips[i];
                if (ed == null)
                    continue;
                equipAttr.AddRange(ed.Attribute);
                if(ed.Innate.Length > 0)
                {
                    equipAttr.AddRange(ed.Innate);
                }
                if (ed.wishs.Length > 0)
                {
                    for (int j = 0; j < ed.wishs.Length; j++)
                    {
                        equipAttr.Add(ed.wishs[i].wish);
                    }
                }
            }
        }
        return equipAttr;
    }
    public void ClearEquipAttr()
    {
        if (equipAttr != null)
        {
            equipAttr.Clear();
            equipAttr = null;
        }
    }
    #endregion
    #region   气运属性
    private List<Pro> totemAttr;
    public List<Pro> TotemAttr()
    {
        if (totemAttr == null)
        {
            totemAttr = new List<Pro>();
            TotemData[] totems = GetTotem();
            for (int i = 0, length = totems.Length; i < length; ++i)
            {
                TotemData ed = totems[i];
                if (ed == null)
                    continue;
                totemAttr.AddRange(ed.Attribute);
            }
        }
        return totemAttr;
    }
    public void ClearTotemAttr()
    {
        if (totemAttr != null)
        {
            totemAttr.Clear();
            totemAttr = null;
        }
    }
    #endregion
}

