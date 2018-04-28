using UnityEngine;

public static class Expand
{
    public static void SetLayer(this GameObject go, string layerName)
    {
        SetLayer(go, LayerMask.NameToLayer(layerName));
    }

    public static void SetLayer(this GameObject go, int layer)
    {
        Transform[] trs = go.GetComponentsInChildren<Transform>(true);
        for (int i = 0, length = trs.Length; i < length; ++i)
        {
            trs[i].gameObject.layer = layer;
        }
    }

    /// <summary>
    /// 给字符串添加颜色标签（HTML）
    /// </summary>
    /// <param name="str"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string AddColorLabel(this string str, string color)
    {
        return string.Format("<color=#{0}>{1}</color>", color, str);
    }
}
