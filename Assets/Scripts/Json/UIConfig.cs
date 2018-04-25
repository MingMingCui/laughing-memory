namespace JsonData
{
    [System.Serializable]
    public class UIConfigArray
    {
        public UIConfig[] Array;
        private UIConfigArray() { }
    }


    [System.Serializable]
    public class UIConfig
    {
        private UIConfig() { }
        public int ID;   //ID
        public int Back;    // 0 --玩家头像 1 --返回按钮 2 --显示并遮挡  3 --不显示
        public int Resid; //资源ID
        public int[] Itemid; //显示道具ID
        public int Mask;    //是否有黑色遮罩 0 没有 1 有 点击不关闭 2 有 点击关闭
        public string Layer;  //层级
        public int Order;   //渲染队列
    }
}

