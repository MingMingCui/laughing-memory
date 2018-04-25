
namespace Msg.HeroMsg
{
    [System.Serializable]
    public class HeroMsgCollect
    {
        public HeroMsg[] heros;
    }

    [System.Serializable]
    public class HeroMsg
    {
        public int heroID;
        public int exp;
        public int officer;
        public int star;
        public int rare;
        public int piece;
        public int level;
        public string[] equips;
        public string[] totems;
        public SkillInfo[] skillInfo;
    }
    [System.Serializable]
    public class SkillInfo
    {
        public int skillID;
        public int lv;
    }

    [System.Serializable]
    public class EquipMsgCollect
    {
        public EquipMsg[] equips;
    }

    [System.Serializable]
    public class EquipMsg
    {
        public string md5;
        public int equipID;
        public int strengthenLv;
        public int exp;
        public int heroID;
        public WishInfo[] randomWish;
        public WishInfo[] tempWish;
    }
    [System.Serializable]
    public class WishInfo
    {
        public Attr attr;
        public float per;
        public float num;
        public bool isLock;
    }

    [System.Serializable]
    public class HeorPropertyUp
    {

    }
}
