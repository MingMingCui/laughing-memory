namespace JsonData
{
    [System.Serializable]
    public class BulletArray
    {
        private BulletArray() { }
        public Bullet[] Array;
    }
    [System.Serializable]
    public class Bullet
    {
        private Bullet() { }
        public int ID; // ID
        public string name; // 名字
        public float speed; // 飞行速度
        public float bulletheight; // 子弹高度
        public int type; // 子弹类型
        public int bounce; // 弹射次数
        public int[] effects; // 子弹效果
        public int resid; // 资源id
    }
}

