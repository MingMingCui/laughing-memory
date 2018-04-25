namespace JsonData
{
    [System.Serializable]
    public class AdvancedSpendArray
    {
        public AdvancedSpend[] Array;
        private AdvancedSpendArray() { }
    }

    [System.Serializable]
    public class AdvancedSpend
    {
        public int ID;
        public MaterialSpend[] material;
        private AdvancedSpend() { }
    }
    [System.Serializable]
    public class MaterialSpend
    {
        public int material;
        public int num;
        private MaterialSpend() { }
    }

}


