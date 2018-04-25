namespace JsonData
{
    [System.Serializable]
    public class DropOrderArray
    {
        private DropOrderArray() { }
        public DropOrder[] Array;
    }
    [System.Serializable]
    public class DropOrder
    {
        private DropOrder() { }
        public int ID;
        public int x;
        public int y;
    }
}

