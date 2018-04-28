
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ********************满星后不会打开此UI**************************
/// </summary>
public class StarUpView : StarUpViewBase
{
    private const int StarBegin = 1014;
    private Image[] starImage;

    public void Open(int heroId)
    {
        HeroData heroData = HeroMgr.GetSingleton().GetHeroData(heroId);

        int[] matnum = JsonMgr.GetSingleton().GetHeroStarByID(heroData.HeroId).matnum;
        //取下一星级需要数量 刚好为当前星级下标 
        int needPiece = matnum[heroData.Star];
        Piece_txt.text = heroData.Piece + "/" + needPiece;

        HeroRare officerUp = JsonMgr.GetSingleton().GetHeroRareByID(heroData.Rare);
        Border_img.sprite = ResourceMgr.Instance.LoadSprite(officerUp.HeadBorder);
        Head_img.sprite = ResourceMgr.Instance.LoadSprite(heroData.JsonData.headid);
        Lv_txt.text = heroData.Level.ToString();
        //碎片ID为武将ID
        piece_img.sprite = ResourceMgr.Instance.LoadSprite(heroData.HeroId);
        Fighting_txt.text = "12334325";
        Cost_txt.text = JsonMgr.GetSingleton().GetGlobalIntArrayByID(heroData.Star + StarBegin).value.ToString("N0");

        int showStar = HeroMgr.MaxHeroStar - heroData.Star;
        starImage = HideStar_obj.transform.GetComponentsInChildren<Image>();
        for (int i = 0; i < heroData.Star; ++i)
        {
            starImage[i].transform.SetParent(StarParent_obj.transform);
        }

        CanvasRenderer[] leftStar = leftstar_obj.GetComponentsInChildren<CanvasRenderer>();
        for (int i = 0; i < leftStar.Length; ++i)
        {
            leftStar[i].SetAlpha((i < showStar) ? 0 : 1);
        }
        CanvasRenderer[] rightStar = rightstar_obj.GetComponentsInChildren<CanvasRenderer>();
        for (int i = 0; i < rightStar.Length; ++i)
        {
            rightStar[i].SetAlpha((i < (showStar - 1)) ? 0 : 1);
        }

        EventListener.Get(Canel_btn.gameObject).OnClick = e =>
        {
            UIFace.GetSingleton().Close(UIID.HeroStarUp);
        };
        EventListener.Get(StarUp_btn.gameObject).OnClick = e =>
        {
            if (heroData.Piece < needPiece)
            {
                CanvasView.Instance.AddNotice("碎片不足");
                return;
            }
            Close();
            //服务器处理升星逻辑 客户端暂代
            int expend = Mathf.RoundToInt(JsonMgr.GetSingleton().GetGlobalIntArrayByID(heroData.Star + StarBegin).value);
            Role.Instance.Cash -= expend;
            heroData.Piece -= needPiece;
            heroData.Star++;
            ZEventSystem.Dispatch(EventConst.REFRESHSIDE, false);
            if (heroData.Star == HeroMgr.MaxHeroStar)
            {
                UIFace.GetSingleton().Close(UIID.HeroStarUp);
                return;
            }
            Open(heroId);
        };
        EventListener.Get(piece_img.gameObject).OnClick = e =>
        {
            UIFace.GetSingleton().Open(UIID.HeroGoto, heroId);
        };
    }
    public void Close()
    {
        for (int i = 0; i < starImage.Length; ++i)
        {
            starImage[i].transform.SetParent(HideStar_obj.transform);
        }
    }
}
