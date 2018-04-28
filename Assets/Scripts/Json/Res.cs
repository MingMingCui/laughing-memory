namespace JsonData
{
    [System.Serializable]
    public class ResArray
    {
        public Res[] Array;
        private ResArray() {}
    }

    [System.Serializable]
    public class Res 
    {
        public int ID;//资源ID
        public string ResourcePath;//资源路径
        public string ResourceName;//bundle包名字
        private Res(){ }
    }
}


