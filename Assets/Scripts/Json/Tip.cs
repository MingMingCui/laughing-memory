namespace JsonData
{
    [System.Serializable]
    public class TipArray
    {
        public Tip[] Array;
    }
    [System.Serializable]
    public class Tip
    {
        public int ID;//id
        public string Content;//内容
        public string Left;//左按钮
        public string Right;//右按钮
        public bool Rich;//是否支持富文本
    }
}


