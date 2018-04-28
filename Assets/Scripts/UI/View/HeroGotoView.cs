using UnityEngine;

public class HeroGotoView : HeroGotoViewBase
{
    public GameObject LevelObj;
    private HeroData data;

    public void Open(int heroId)
    {
        data = HeroMgr.GetSingleton().GetHeroData(heroId);
        HeroName_txt.text = data.JsonData.name;

        int[] matnum = JsonMgr.GetSingleton().GetHeroStarByID(heroId).matnum;
        //取下一星级需要数量 刚好为当前星级下标 
        int needPiece = matnum[data.Star];
        Piece_txt.text = data.Piece + "/" + needPiece;
        //边框为碎片稀有度 -- 固定
        //OfficerUp officerUp = JsonMgr.GetSignleton().GetOfficerUpByID(data.Rare);
        //broder_img.sprite = ResourceMgr.Instance.LoadSprite(officerUp.CardBorder);
        //碎片ID没定 
        piece_img.sprite = ResourceMgr.Instance.LoadSprite(data.JsonData.headid);

        EventListener.Get(close_btn.gameObject).OnClick = e =>
        {
            UIFace.GetSingleton().Close(UIID.HeroGoto);
        };
    }
}
