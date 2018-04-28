using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util{

    /// <summary>
    /// 字符串切片成float数组
    /// </summary>
    /// <param name="str">要处理的字符串</param>
    /// <param name="delimiter">分隔符</param>
    /// <returns></returns>
    public static float[] Str2FloatArr(string str, char delimiter = '#')
    {
        string[] splitStr = str.Split(delimiter);
        float[] ret = new float[splitStr.Length];
        for (int idx = 0; idx < splitStr.Length; ++idx)
        {
            ret[idx] = float.Parse(splitStr[idx]);
        }
        return ret;
    }

    /// <summary>
    /// 字符串切片成int数组
    /// </summary>
    /// <param name="str">要处理的字符串</param>
    /// <param name="delimiter">分隔符</param>
    /// <returns></returns>
    public static int[] Str2IntArr(string str, char delimiter = '#')
    {
        string[] splitStr = str.Split(delimiter);
        int[] ret = new int[splitStr.Length];
        for (int idx = 0; idx < splitStr.Length; ++idx)
        {
            ret[idx] = int.Parse(splitStr[idx]);
        }
        return ret;
    }

    /// <summary>
    /// 正数返回1负数返回-1，0返回0
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static int Judge(int val)
    {
        return val > 0 ? 1 : (val < 0 ? -1 : 0);
    }

    /// <summary>
    /// 计算点到直线距离
    /// </summary>
    /// <param name="startVe2"></param>
    /// <param name="endVe2"></param>
    /// <param name="pointVe2"></param>
    /// <returns></returns>
    public static float PointToStraightlineDistance(Vector2 startVe2, Vector2 endVe2, Vector2 pointVe2)
    {
        float A = endVe2.y - startVe2.y;
        float B = startVe2.x - endVe2.x;
        float C = endVe2.x * startVe2.y - startVe2.x * endVe2.y;
        float denominator = Mathf.Sqrt(A * A + B * B);
        return Mathf.Abs((A * pointVe2.x + B * pointVe2.y + C) / denominator);
    }

    private static System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

    /// <summary>
    /// UTC时间（秒数）转DateTime
    /// </summary>
    /// <param name="utc"></param>
    /// <returns></returns>
    public static System.DateTime Utc2DateTime(long utc)
    {
        return startTime.AddSeconds(utc);
    }
}
