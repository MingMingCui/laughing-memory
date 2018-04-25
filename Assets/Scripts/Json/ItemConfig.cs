namespace JsonData
{

    [System.Serializable]
    public class ItemConfigArray
    {
        private ItemConfigArray() { }
        public ItemConfig[] Array;
    }

    [System.Serializable]
    public class ItemConfig
    {
        private ItemConfig() { }
        public int ID;      //id
        public string name;  //名称
        public FuncType type;    //物品类型
        public int icon;   //图标
        public int rare;    //   稀有度
        public int price;  //   售价
        public string propertydes;  //属性描述
        public string usedes;//使用描述
    }
}

