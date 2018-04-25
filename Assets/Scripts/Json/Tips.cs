namespace JsonData
{
    [System.Serializable]
    public class TipsArray
    {
        public Tips[] Array;
    }
    [System.Serializable]
    public class Tips
    {
        public int ID;//id
        public float sizex;//大小x
        public float sizey;//大小y
        public float border;//边框大小
        public string content;//内容
    }
}


