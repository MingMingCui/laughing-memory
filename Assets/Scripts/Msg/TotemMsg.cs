namespace Msg.ToTemMsg
{
    [System.Serializable]
    public class TotemMsgCollect
    {
        public TotemMsg[] totems;
    }

    [System.Serializable]
    public class TotemMsg 
    {
        public string md5;
        public int itemID;
        public int exp;
        public int heroID;
        public int level;
        public int groove;
    }
}


