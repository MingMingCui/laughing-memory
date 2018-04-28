namespace JsonData
{
    [System.Serializable]
    public class GlobalValueArray
    {
        private GlobalValueArray() { }
        public GlobalValue[] Array;
    }
    [System.Serializable]
    public class GlobalValue
    {
        private GlobalValue() { }
        public int ID; // ID
        public string desc; // 描述
        public float value; // 数值
    }
}

