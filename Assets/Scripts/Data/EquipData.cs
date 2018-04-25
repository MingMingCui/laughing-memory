using JsonData;
using Msg.HeroMsg;
using UnityEngine;

public enum WeaponType
{
    Arrow = 1,      //弓箭
    Fan = 2,        //羽扇
    LHandle = 3,    //长柄
    SHandle = 4,    //短柄
}

public enum ArmorType
{
    Cloth = 1,      //布甲
    Leather = 2,    //皮甲
    Metal = 3,      //钢甲
}

public enum EquipType
{
    Normal = 1,             //强化洗练进阶(老四门)
    Token = 3,              //累计强化吞噬(宝物 虎符)
    Mount = 4,              //繁殖(坐骑)
    Book = 5,               //残页收集(书籍)
}

public enum EquipPart
{
    Helmet = 1,         //头盔
    Body = 2,           //身体
    Shoes = 3,          //战靴
    Weapon = 4,         //武器
    Hufu = 5,           //虎符
    Treasure = 6,       //宝物
    Mount = 7,          //坐骑
    Book = 8,           //经典
}

public struct RandomWish
{
    public Pro wish;
    public bool isLock;
}

public class EquipData
{
    //UID
    public string md5 { get;  set; }

    //装备ID
    private int equipId;                                                                                                   
    public int EquipId { get { return equipId; } set { equipId = value; itemData = null;data = null; } }

    private ItemConfig itemData;
    public ItemConfig ItemData
    {
        get
        {
            if (itemData == null)
                itemData = JsonMgr.GetSingleton().GetItemConfigByID(EquipId);
            return itemData;
        }
    }
    private Equip data;
    public Equip JsonData
    {
        get
        {
            if (data == null)
                data = JsonMgr.GetSingleton().GetEquipByID(EquipId);
            return data;
        }
    }
    /// <summary>
    /// 基础属性
    /// </summary>
    public Pro[] Attribute
    {
        get
        {
            Pro[] ret = new Pro[JsonData.Attribute.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = new Pro
                {
                    attr = JsonData.Attribute[i].attr,
                    num = JsonData.Attribute[i].num * (StrengthenLv+ 1),
                    per = JsonData.Attribute[i].per * (StrengthenLv + 1)
                };
            }
            return ret;
        }
    }
    /// <summary>
    /// 固定属性
    /// </summary>
    public Pro[] Innate
    {
        get
        {
            return JsonData.Innate;
        }
    }

    //随机属性
    public RandomWish[] wishs;
    public Pro[] tempWishs;


    //强化等级
    private int strengthenLv;
    public int StrengthenLv {
        get
        {
            return Mathf.Max(0, strengthenLv);
        }
        set
        {
            strengthenLv = value;
            HeroData hero = HeroMgr.GetSingleton().GetHeroData(HeroId);
            if (hero == null)
                return;
            hero.ClearEquipAttr();
            ZEventSystem.Dispatch(EventConst.REFRESHRIGHT);
        }
    }
    /// <summary>
    /// 谁装备着 没有人为0
    /// </summary>
    public int HeroId { get; set; }
    //强化经验
    private int exp;
    public int Exp { get { return Mathf.Max(0, exp); } private set { exp = value; } }

    public EquipData(int equipID)
    {
        EquipId = equipID;
        md5 = "1001";
        wishs = new RandomWish[0];
        tempWishs = new Pro[0];
    }
    public EquipData(EquipMsg msg)
    {
        md5 = msg.md5;
        EquipId = msg.equipID;
        Exp = msg.exp; ;
        HeroId = msg.heroID;
        StrengthenLv = msg.strengthenLv;
        int length = msg.randomWish.Length;
        wishs = new RandomWish[length];
        for (int i = 0; i < length; ++i)
        {
            wishs[i].wish = new Pro
            {
                attr = msg.randomWish[i].attr,
                num = msg.randomWish[i].num,
                per = msg.randomWish[i].per
            };
            wishs[i].isLock = msg.randomWish[i].isLock;
        }
        length = msg.tempWish.Length;
        tempWishs = new Pro[length];
        for (int i = 0; i < length; ++i)
        {
            tempWishs[i] = new Pro
            {
                attr = msg.tempWish[i].attr,
                num = msg.tempWish[i].num,
                per = msg.tempWish[i].per
            };
        }
    }
}
