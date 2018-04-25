namespace JsonData
{
    [System.Serializable]
    public class MonstersArray
    {
        private MonstersArray() { }
        public Monsters[] Array;
    }
    [System.Serializable]
    public class Monsters
    {
        private Monsters() { }
        public int ID;
        public int[] monster;
        public int[] position;
    }
}



