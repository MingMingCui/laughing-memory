using UnityEngine;

public class HeroDivinationView : MonoBehaviour
{
    public DivinationItemView[] div;
    public CanvasRenderer[] lockImg;

    public void SetHeroTotemView(HeroData hero)
    {
        string unlock = JsonMgr.GetSingleton().GetGlobalStringArrayByID(10001).desc;

        string[] temp = unlock.Split('#');
        int[] unLockLv = new int[temp.Length];
        for (int i = 0; i < unLockLv.Length; ++i)
        {
            unLockLv[i] = int.Parse(temp[i]);
        }
        for (int i = 0,lv = hero.Level; i < unLockLv.Length; ++i)
        {
            lockImg[i].SetAlpha(unLockLv[i] <= lv ? 0 : 1);
            if (unLockLv[i] <= lv)
            {
                if (i < hero.GetTotem().Length)
                {
                    div[i].SetView(hero.GetTotem()[i], unLockLv[i]);
                }
                else
                {
                    div[i].SetView(null, unLockLv[i]);
                }
            }
            else
                div[i].Lock(unLockLv[i]);
        }
    }
}
