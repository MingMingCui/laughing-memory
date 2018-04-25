namespace JsonData
{
    [System.Serializable]
    public class StrengthenSpendArray
    {
        public StrengthenSpend[] Array;
        private StrengthenSpendArray() { }
    }

    [System.Serializable]
    public class StrengthenSpend
    {
        public int ID;
        public int spend;
        private StrengthenSpend() { }
    }

}

