using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMgr
{

    public static Color32 EnoughGreen = new Color32(74, 224, 0, 255);        //表示数量足够时使用的绿色

    public static Color32 LackRed = new Color32(255, 25, 0, 255);             //表示数量不足时使用的红色

    public static Color32 YellowWish = new Color32(232, 231, 165, 255);         //文字用淡黄色

    public static Color32 SkyBlue = new Color32(0, 255, 255, 255);          //文字用天蓝色

    private static int[] _border;
    public static int[] Border { get { if (_border == null) GetConfig(); return _border; }private set { _border = value; } }
    private static string[] _colors;
    public static string[] Colors { get { if (_colors == null) GetConfig(); return _colors; } private set { _colors = value; } }

    public static void GetConfig()
    {
        string cString = JsonMgr.GetSingleton().GetGlobalStringArrayByID(10002).desc;
        string bString = JsonMgr.GetSingleton().GetGlobalStringArrayByID(10003).desc;
        string[] cArray = cString.Split('#');
        _colors = new string[cArray.Length];
        for (int i = 0; i < cArray.Length; ++i)
        {
            _colors[i] = cArray[i];
        }
        string[] bArray = bString.Split('#');
        Border = new int[bArray.Length];
        for (int i = 0; i < bArray.Length; ++i)
        {
            Border[i] = int.Parse(bArray[i]);
        }
    }
}
