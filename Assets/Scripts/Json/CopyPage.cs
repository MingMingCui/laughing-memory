namespace JsonData
{
    [System.Serializable]
    public class CopyPageArray
    {
        private CopyPageArray() { }
        public CopyPage[] Array;
    }
    [System.Serializable]
    public class CopyPage
    {
        public int ID; // ID
        public Award[] awards; 
    }
    [System.Serializable]
    public class Award
    {
        private Award() { }
        public int condition; // 阶段1奖励要求
        public int[] item;
        public int[] num;
    }
}


