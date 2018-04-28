
[System.Serializable]
public class HeroRareArray
{
    public HeroRare[] Array;
    private HeroRareArray() { }
}

[System.Serializable]
public class HeroRare
{
    public int ID;
    public string Name;//官阶显示
    public string Color;//官阶颜色
    public int NeedNum;//所需功勋
    public int CardBorder;//图鉴边框
    public int CardTrans;//图鉴过渡
    public int Officer;//官职左侧标头
    public int HeadBorder;//头像边框
    public int FightHeadBorder;//战斗头像边框
    public int[] UnLock; //解锁官职
    private HeroRare() { }
}
