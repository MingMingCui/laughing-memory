
[System.Serializable]
public class OfficerArray
{
    public Officer[] Array;
    private OfficerArray() { }
}

[System.Serializable]
public class Officer
{
    public int ID;
    public int Quantity;//数量
    public Pro[] attrerty;//增加属性
    public string Post;//官职
    public int Unlock;//解锁等级
    private Officer() { }
}
[System.Serializable]
public class Pro
{
    public Attr attr;//属性
    public float num;//增加值
    public float per;   //百分比增加
}
