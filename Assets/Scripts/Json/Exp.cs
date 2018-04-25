
[System.Serializable]
public class ExpArray
{
    public Exp[] Array;
    private ExpArray() {  }
}

[System.Serializable]
public class Exp
{
    public int ID;
    public int NeedRole;//人物升级所需经验
    public int NeedHero;//英雄升级所需经验
    public int GivePower;//人物升级赠送体力
    private Exp() { }
}
