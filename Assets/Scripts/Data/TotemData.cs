using JsonData;
using Msg.ToTemMsg;
using UnityEngine;

public class TotemData
{
    public string md5 { get; set; }
    public int ItemID { get; private set; }

    public TotemData(int id)
    {
        ItemID = id;
    }
    public TotemData(TotemMsg msg)
    {
        md5 = msg.md5;
        ItemID = msg.itemID;
        Exp = msg.exp;
        HeroID = msg.heroID;
        Level = msg.level;
        Groove = msg.groove;
    }
    public ItemConfig ItemData
    {
        get
        {
            return JsonMgr.GetSingleton().GetItemConfigByID(ItemID);
        }
    }
    public Totem TotemConfig
    {
        get { return JsonMgr.GetSingleton().GetTotemByID(ItemID); }
    }

    public Pro[] Attribute
    {
        get
        {
            Pro[] ret = new Pro[TotemConfig.Attribute.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = new Pro
                {
                    attr = TotemConfig.Attribute[i].attr,
                    num = TotemConfig.Attribute[i].num * Level,
                    per = TotemConfig.Attribute[i].per * Level
                };
            }
            return ret;
        }
    }


    /// <summary>
    /// 等级
    /// </summary>
    private int level;
    public int Level { get { return Mathf.Max(1, level); } set { level = value; } }
     /// <summary>
     /// 稀有度
     /// </summary>
    public int Rare { get { return ItemData.rare; } }

    /// <summary>
    /// 当前等级的经验
    /// </summary>
    public int Exp { get; set; }
    /// <summary>
    /// 装备的武将ID
    /// </summary>
    public int HeroID { get; set; }
    /// <summary>
    /// 镶嵌位置
    /// </summary>
    public int Groove { get; set; }

    /// <summary>
    /// 升级所需经验  100×（2^等级）×(2^（颜色等级-1)）
    ///等级[1, 10]
    ///颜色等级[1, 5] 
    /// </summary>
    public int LevelUpExp(int grade = 0)
    {
        return Mathf.RoundToInt(10 * Mathf.Pow(2, grade == 0 ? Level : grade) * Mathf.Pow(2, Rare - 1)); 
    }
}
