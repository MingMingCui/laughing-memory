namespace JsonData
{
    [System.Serializable]
    public class HeroShowArray
    {
        public HeroShow[] Array;
        private HeroShowArray() { }
    }
    [System.Serializable]
    public class HeroShow
    {
        public int ID;
        public float RotX;//摄像机X
        public float RotY;//武将Y
        public float Fov;//摄像机FOV
        private HeroShow() { }
    }

}

