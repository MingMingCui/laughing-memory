namespace JsonData
{
    [System.Serializable]
    public class GameManagerArray
    {
        private GameManagerArray() { }
        public GameManager[] Array;
    }
    [System.Serializable]
    public class GameManager
    {
        private GameManager() { }
        public int ID; // ID
        public string desc; // 名字
        public string command; // 命令
    }
}

