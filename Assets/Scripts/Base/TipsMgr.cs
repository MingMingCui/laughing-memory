using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsMgr {

    /// <summary>
    /// 打开简易Tips
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="tipId">模板Id</param>
    /// <param name="align">对齐方式</param>
    /// <param name="offset">偏移</param>
    /// <param name="args">参数</param>
    public static void OpenSimpleTip(Vector2 pos, int tipId,  Alignment align, Vector2 offset, params object[] args)
    {
        UIFace.GetSingleton().Open(UIID.Tips);
        ZEventSystem.Dispatch(EventConst.OnOpenTips, (int)TipsType.SimpleTip, tipId, pos, align, offset, new List<object>(args));
    }

    public static void OpenItemTip(Vector2 pos, int itemId, Alignment align, Vector2 offset)
    {
        UIFace.GetSingleton().Open(UIID.Tips);
        ZEventSystem.Dispatch(EventConst.OnOpenTips, (int)TipsType.ItemTip, itemId, pos, align, offset, null);
    }

    public static void OpenMonsterTip(Vector2 pos, int monsterId, bool isBoss, Alignment align, Vector2 offset)
    {
        UIFace.GetSingleton().Open(UIID.Tips);
        List<object> argWrapper = new List<object>();
        argWrapper.Add(isBoss);
        ZEventSystem.Dispatch(EventConst.OnOpenTips, (int)TipsType.MonsterTip, monsterId, pos, align, offset, argWrapper);
    }

    public static void OpenTreasureTip(Vector2 pos, Vector2Int[] data, Alignment align, Vector2 offset)
    {
        UIFace.GetSingleton().Open(UIID.Tips);
        ZEventSystem.Dispatch(EventConst.OnOpenTips, (int)TipsType.TreasureTip, new List<Vector2Int>(data), pos, align, offset, null);
    }

    public static void CloseTip()
    {
        ZEventSystem.Dispatch(EventConst.OnCloseTips);
    }
}
