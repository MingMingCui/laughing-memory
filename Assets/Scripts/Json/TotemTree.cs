namespace JsonData
{
    [System.Serializable]
    public class TotemTreeArray
    {
        public TotemTree[] Array;
    }

    [System.Serializable]
    public class TotemTree
    {
        public int ID;      //ID
        public int spend;    //花费
        public float rate;  //几率
        public int dropGroup;// 掉落组
    }
}

