namespace JsonData
{
    [System.Serializable]
    public class BuffArray
    {
        private BuffArray() { }
        public BuffConfig[] Array;
    }
    [System.Serializable]
    public class BuffConfig
    {
        private BuffConfig() { }
        public int ID; // BuffID
        public string desc; // 描述
        public int type; // 类型
        public int trigger; // 触发条件
        public int triggerparam; // 触发参数
        public int layers; // 叠加层数
        public int priority; // 优先级
        public int dispel; // 驱散类型
        public int needcaster; // 是否依赖施加者
        public int effect; // 效果
        public int caltype; // 公式类型
        public float[] buffparam; // buff参数
        public float bufftime; // 持续时间
        public int usetimes; // 持续次数
        public float buffcd; // 次数间隔
        public int resid; // 特效资源id
        public int onfoot; // 特效是否在脚下
    }
}

