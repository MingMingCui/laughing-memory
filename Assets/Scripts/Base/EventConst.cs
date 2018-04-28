using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConst{

    //=========================底层相关==================================
    public static readonly string OnGamePreloaded = "OnGamePreloaded";

    public static readonly string OnScenePreload = "OnScenePreload";

    public static readonly string OnPreload = "OnPreload";

    public static readonly string SceneLoaded = "SceneLoaded";
    //=========================底层相关==================================

    //=========================基础功能==================================
    public static readonly string OnMusicMute = "OnMusicMute";

    public static readonly string OnSoundMute = "OnSoundMute";

    public static readonly string OnGamePause = "OnGamePause";
    //=========================基础功能==================================

    //=========================网络相关==================================
    public static readonly string OnMsgLogin = "OnMsgLogin";

    public static readonly string OnMsgLoginFailed = "OnMsgLoginFailed";

    public static readonly string OnMsgRegFailed = "OnMsgRegFailed";

    public static readonly string OnMsgOnMain = "OnMsgOnMain";
    //=========================网络相关==================================

    //=========================战斗相关==================================
    public static readonly string OnCreateFight = "OnCreateFight";

    public static readonly string OnNewRound = "OnNewRound";

    public static readonly string OnFightUnitDie = "OnFightUnitDie";

    public static readonly string OnFightStateChange = "OnFightStateChange";

    public static readonly string OnFightTargetChange = "OnFightTargetChange";

    public static readonly string OnUnitMoveOver = "OnUnitMoveOver";

    public static readonly string OnFightUnitMove = "OnFightUnitMove";

    public static readonly string OnFightUnitSkill = "OnFightUnitSkill";

    public static readonly string OnFightUnitHit = "OnFightUnitHit";

    public static readonly string OnFightUnitPop = "OnFightUnitPop";

    public static readonly string OnPlayHitAnim = "OnPlayHitAnim";

    public static readonly string OnFightOver = "OnFightOver";

    public static readonly string OnFightUnitTakenEffect = "OnFightUnitTakenEffect";

    public static readonly string OnFightUnitAddBuff = "OnFightUnitAddBuff";

    public static readonly string OnFightUnitHpChange = "OnFightUnitHpChange";

    public static readonly string OnFightUnitVigourChange = "OnFightUnitVigourChange";

    public static readonly string OnAutoFightStateChange = "OnAutoFightStateChange";

    public static readonly string OnGameSpeedChange = "OnGameSpeedChange";

    public static readonly string OnRequestUnitPause = "OnRequestUnitPause";

    public static readonly string OnUnitPause = "OnUnitPause";

    public static readonly string OnShieldBreak = "OnShieldBreak";

    public static readonly string OnCameraChangeOver = "OnCameraChangeOver";

    public static readonly string OnFightMaskOver = "OnFightMaskOver";

    public static readonly string OnTreasureFly = "OnTreasureFly";

    public static readonly string OnTreasureFlyOver = "OnTreasureFlyOver";

    public static readonly string OnCreateSummon = "OnCreateSummon";

    public static readonly string ForceDestroyView = "ForceDestroyView";

    public static readonly string OnSkillTakeEffect = "OnSkillTakeEffect";

    public static readonly string OnFightUnitBooster = "OnFightUnitBooster";

    public static readonly string DropOutItem = " DropOutItem";

    public static readonly string OnFightUnitInterrupt = " OnFightUnitInterrupt";

    public static readonly string OnFightUnitTakeEffect = "OnFightUnitTakeEffect";

    public static readonly string OnInitEvent = "OnInitEvent";
    //=========================战斗相关==================================

    //=========================基础相关==================================
    public static readonly string OnOpenTips = "OnOpenTips";

    public static readonly string OnCloseTips = "OnCloseTips";
    //=========================基础相关==================================

    //=========================背包相关==================================
    public static readonly string UpdateItemList = "UpdateItemList";

    public static readonly string UpdateItemParts = "UpdateItemParts";
    //=========================效果相关==================================
    //=========================效果相关==================================

    //=========================关卡相关==================================
    public static readonly string OpLevel = "OpLevel";
    public static readonly string OnClose = "OnClose";
    //=========================关卡相关==================================
    //=========================导航界面==================================

    public const string NAVIGATIONBACK = "NAVIGATIONBACK";
    public static readonly string UpdateData = "UpdateData";
    //  public static readonly string NavicatBack = "NavicatBack";
    //=========================导航界面==================================
    //=========================七天登录奖励界面==================================
    public static readonly string OnGetRegisterSky = "OnGetRegisterSky";
    //=========================七天登录奖励界面==================================

    //=========================签到相关==================================
    public static readonly string OnSignIc = "OnSignIc";
    //=========================签到相关==================================

    //=========================英雄界面==================================
    public const string REFRESHSIDE = "REFRESHSIDE";
    public const string REFRESHLEFT = "REFRESHLEFT";
    public const string REFRESHRIGHT = "REFRESHRIGHT";

    //=========================商店相关==================================
    public static readonly string ShowUnitShop = "ShowUnitShop";
    public static readonly string ShowNPC = "ShowNPC";
    public static readonly string RefreshTimes = "RefreshTimes";
    public static readonly string GetShopItemByType = "GetShopItemByType";
    //=========================商店相关==================================

    //=========================军团相关==================================
    public static readonly string OnSelectFlag = "OnSelectFlag";
    //=========================军团相关==================================

    //=========================招募相关==================================
    public static readonly string ShowOneLuckyDrawResults = "ShowOneLuckyDrawResults";

    public static readonly string ShowTenLuckyDrawResults = "ShowTenLuckyDrawResults";

    public static readonly string ShowAllHeros = "ShowAllHeros";
    //=========================招募相关==================================

    //=========================布阵相关==================================
    public static readonly string OnStubSaveOver = "OnStubSaveOver";

    public static readonly string OnStubChange = "OnStubChange";
    //=========================布阵相关==================================
    //=========================邮箱相关==================================
    public static readonly string Incident = "Incident";
    public static readonly string OnMailItemIncident = "OnMailItemIncident";
    //=========================邮箱相关==================================
    public const string ONOPENCOMPOSE = "ONOPENCOMPOSE";
    /// <summary>
    /// 装备气运
    /// </summary>
    public const string TAKETOTEM = "TAKETOTEM";
    /// <summary>
    /// 武将数据变化刷新
    /// </summary>
    public const string HEROINFOCHANGE = "HEROINFOCHANGE";
    /// <summary>
    /// 气运数据变化刷新
    /// </summary>
    public const string TOTEMDATACHANGE = "TOTEMDATACHANGE";
    //=========================邮箱相关==================================
    /// <summary>
    /// 刷新强化界面
    /// </summary>
    public const string UPSTRENGTHENVIEW = "UPSTRENGTHENVIEW";
}
