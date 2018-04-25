[System.Serializable]
public class TotemArray
{
    public Totem[] Array;
    private TotemArray() { }
}

[System.Serializable]
public class Totem
{
    private Totem() { }
    public int ID;
    public string Name;
    public int Exp;
    public Pro[] Attribute;
}
