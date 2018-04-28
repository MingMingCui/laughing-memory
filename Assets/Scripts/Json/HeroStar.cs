
[System.Serializable]
public class HeroStarArray
{
    public HeroStar[] Array;
    private HeroStarArray() { }
}

[System.Serializable]
public class HeroStar
{
    public int ID; // ID
    public int matid; // 材料id
    public int[] matnum; // 材料数量
    public int[] sellnum; // 分解数量
    public float[] starhp; // 生命值成长
    public float[] staratk; // 物理攻击成长
    public float[] stardef; // 物理防御成长
    public float[] starmatk; // 策略攻击成长
    public float[] starmdef; // 策略防御成长
    private HeroStar() { }
}
