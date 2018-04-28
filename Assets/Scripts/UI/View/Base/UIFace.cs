using JsonData;

public enum UIID : int
{
    None = 0,
    Login = 1002,    //登录界面
    Main = 1003,    //主界面
    Match = 1004,    //战斗界面
    Bag = 1005,    //背包界面
    Battle = 1006,    //战役界面
    Navicat = 1007,    //导航界面
    CreateRole = 1012,    //创建角色界面
    SelectCountry = 1014,    //选择国家界面
    Stub = 1015,    //布阵界面
    Award = 1016,    //七天登录奖励界面
    SignIn = 1017,     //签到界面  
    Mail = 1018,       //邮箱界面
    GM = 1019,        //GM界面
    Customs =1020,   //关卡简介界面
    Majesty = 1021,   //主公系统界面
    Shop = 1201,      //商店界面
    WesternShop = 1202,      //西域商店界面
    Recruiting = 1203,      //招募界面
    ShopItem = 1204,        //商店货物界面
    RecruitingAward = 1205, //招募抽奖界面
    RecruitingProbability = 1206, //招募概率界面

    //----------------C1101-1200M 崔明明 ----------------//
    Hero = 1008,    //武将界面
    HeroDetail = 1009,    //武将详情界面
    HeroStarUp = 1010,    //武将升星界面
    HeroGoto = 1011,    //去获取界面
    ExpPool = 1013,    //经验池界面
    Tip = 1101,    //提示框
    Divination = 1102,  //卜卦界面
    DivinationTip = 1103,//护身符详情
    CompostDivination = 1104,//宝石合成
    StrengthenTip = 1105,  //强化提示
    Strengthen = 1106, //强化界面
    EuqipTipCtrl = 1107, //进阶提示


    //--------------C1301-1400 董小雨 -----------------//
    Corps = 1301,     //军团创建、加入
    FlagSelect = 1302,     //选择军旗
    CorpsMain = 1303,     //军团主界面
    PlayerInfo = 1304,     //个人信息界面
    PlayerInfoDetail = 1305,     //个人信息详情
    Tips = 1306,            //小提示
    Arena = 1307,           //竞技场
}

public class UIFace :UIMgr
{
    private static UIFace _face;
    public static UIFace GetSingleton()
    {
        return _face ?? (_face = new UIFace());
    }
    private UIFace()
    {
        Registration(UIID.Login, new LoginCtrl());
        Registration(UIID.Main, new MainCtrl());
        Registration(UIID.Bag, new ItemCtrl());
        Registration(UIID.Match, new MatchUICtrl());
        Registration(UIID.Battle, new BattleCtrl());
        Registration(UIID.Hero, new HeroCtrl());
        Registration(UIID.HeroDetail, new HeroDetail());
        Registration(UIID.HeroStarUp, new StarUpCtrl());
        Registration(UIID.HeroGoto, new HeroGotoCtrl());
        Registration(UIID.CreateRole, new SelectPerCtrl());
        Registration(UIID.ExpPool, new ExppoolCtrl());
        Registration(UIID.SelectCountry, new SelectStateCtrl());
        Registration(UIID.Stub, new StubCtrl());
        Registration(UIID.Tip, new TipCtrl());
        Registration(UIID.Award, new SevenActivityCtrl());
        Registration(UIID.SignIn, new SignInCtrl());
        Registration(UIID.Shop, new ShopCtrl());
        Registration(UIID.WesternShop, new WesternShopCtrl());
        Registration(UIID.Corps, new CorpsCtrl());
        Registration(UIID.FlagSelect, new FlagSelectCtrl());
        Registration(UIID.CorpsMain, new CorpsMainCtrl());
        Registration(UIID.Divination, new DivinationCtrl());
        Registration(UIID.DivinationTip, new DivinationTip());
        Registration(UIID.PlayerInfo, new PlayerInfoCtrl());
        Registration(UIID.PlayerInfoDetail, new PlayerInfoDetailCtrl());
        Registration(UIID.CompostDivination, new ComposeDivinationCtr());
        Registration(UIID.Recruiting, new RecruitingCtrl());
        Registration(UIID.Mail, new PostBoxCtrl());
        Registration(UIID.GM, new GMCtrl());
        Registration(UIID.Customs, new CustomsPassCtrl());
        Registration(UIID.ShopItem, new ShopUICtrl());
        Registration(UIID.Tips, new TipsCtrl());
        Registration(UIID.Strengthen,new StrengthenCtrl());
        Registration(UIID.StrengthenTip,new StrengthenTipCtrl());
        Registration(UIID.EuqipTipCtrl, new EuqipTipCtrl());
        Registration(UIID.RecruitingAward, new RecruitingAwardCtrl());
        Registration(UIID.Arena, new ArenaCtrl());
        Registration(UIID.Majesty, new MajestyCtrl());
        Registration(UIID.RecruitingProbability, new RecruitingProbabilityCtrl());
        ZEventSystem.Register(EventConst.NAVIGATIONBACK, this, "CallBack");
    }
    private NavigationCtrl navigat;

    public IUICtrl Open(UIID id,params object[] arg)
    {
        CanvasView.graphic.enabled = false;
        IUICtrl ctrl = GetCtrl(id);
        if (ctrl == null)
            throw new System.Exception("没有注册UI ---" + id);
        ctrl.SetParameters(arg);
        UIConfig uiJson = base.OpenUI(id);
        ctrl.isOpen = true;
        if (uiJson == null)
            throw new System.Exception("ui表没有ID ----" + id);
        NavigationCtrl(uiJson);
        CanvasView.graphic.enabled = true;
        return ctrl;
    }
    public void Close(UIID id)
    {
        base.CloseUI(id);
        if (openList.Contains(id))
            openList.Remove(id);
        int count = openList.Count;
        if (count > 0)
        {
            UIID uiid = openList[count - 1];
            NavigationCtrl(JsonMgr.GetSingleton().GetUIConfigByID((int)uiid));
        }
    }
    private void NavigationCtrl(UIConfig uiJson)
    {
        if (uiJson.Back < 3)
        {
            if (navigat == null)
            {
                navigat = new NavigationCtrl();
                Registration(UIID.Navicat, navigat);
                base.OpenUI(UIID.Navicat);
            }
            if(navigat.GetView() == null)
                base.OpenUI(UIID.Navicat);
            navigat.Refresh(uiJson);
        }
        else
        {
            do
            {
                if (navigat == null || navigat.GetView() == null)
                    break;
                navigat.Hide();
            } while (false);
        }
    }
    public void CallBack()
    {
        int count = openList.Count;
        if (count > 0)
        {
            Close(openList[count - 1]);
            count = openList.Count;
            if (count > 0 && !GetCtrl(openList[count - 1]).IsOpen())
                Open(openList[count - 1]);
        }
    }
}
