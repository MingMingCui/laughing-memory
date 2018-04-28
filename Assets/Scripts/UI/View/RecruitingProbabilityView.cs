using JsonData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsHeroHead
{
    public int isHave;//拥有:1,未拥有:2
    public Hero _heros;
}

public class RecruitingProbabilityView : RecruitingProbabilityViewBase
{
    int curId;
    public HeroHaveInfo hero = null;
    List<HeroHaveInfo> heros = new List<HeroHaveInfo>();
    List<int> herohead = null; //武将头像
    List<IsHeroHead> herolist = new List<IsHeroHead>();    

    RectTransform rect = null;
    GridLayoutGroup grid = null;
    RectTransform srrect = null;
    

    /// <summary>
    /// 显示武将
    /// </summary>
    /// <param name="heross"></param>
    public void ShowHero()
    {
        if(RecruitingMgr.Instance.heroList.Count> heros.Count)
        {

       
        for (int i = 0; i < RecruitingMgr.Instance.heroList.Count; i++)
        {
            Hero heroses = JsonMgr.GetSingleton().GetHeroByID(RecruitingMgr.Instance.heroList[i]);
            if (i == heros.Count)
            {
                GameObject hero = LoadHero();
                HeroHaveInfo _heros = hero.GetComponent<HeroHaveInfo>();
                _heros.Init();
                _heros.heroid = RecruitingMgr.Instance.heroList[i];
                heros.Add(_heros);
            }
            IsHeroHead herohead =new IsHeroHead ();
            herohead._heros = heroses;
            if (HeroMgr.GetSingleton().HerosContainer.ContainsKey(heroses.ID))
            {
                herohead.isHave = 1;
            }
            else
            {
                herohead.isHave = 2;
            }
            herolist.Add(herohead);
        }
        UpdateHero();
        }
        //滑动窗大小
        if (grid == null)
        {
            grid = hero_obj.GetComponent<GridLayoutGroup>();
            rect = hero_obj.GetComponent<RectTransform>();
            srrect = allhero_obj.GetComponent<RectTransform>();
        }
        int num = (int)(srrect.sizeDelta.y / grid.cellSize.y); //行数
        int xnum = (int)(srrect.sizeDelta.x / grid.cellSize.x);//每行个数
        rect.offsetMin = new Vector2(0, -((grid.cellSize.y + grid.spacing.y) * ((heros.Count / xnum) - num)));
        rect.offsetMax = Vector2.zero;
        herohead = RecruitingMgr.Instance.heroList;
    }
    public void UpdateHero()
    {
        HeadSort();
        for (int i = 0; i < heros.Count; i++)
        {
            if (herolist[i].isHave == 1)
                heros[i].nothave_obj.SetActive(false);
            else
                heros[i].nothave_obj.SetActive(true);
            heros[i].SetHeroHaveInfo(JsonMgr.GetSingleton().GetHeroByID(herolist[i]._heros.ID));
        }
    }

    /// <summary>
    /// 实例化武将头像
    /// </summary>
    /// <returns></returns>
    GameObject LoadHero()
    {
        GameObject _hero = Instantiate(hero.gameObject);
        _hero.transform.SetParent(hero_obj.transform);
        _hero.transform.localScale = Vector3.one;
        _hero.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _hero;
    }

    /// <summary>
    /// 武将头像排序
    /// </summary>
    /// <param name="herolist"></param>
    void HeadSort()
    {
        herolist.Sort((IsHeroHead hero1, IsHeroHead hero2) =>
            {
                if (hero1.isHave < hero2.isHave)
                    return 1;
                else if (hero1.isHave > hero2.isHave)
                    return -1;
                else 
                {
                    if (hero1._heros.ID < hero2._heros.ID)
                        return -1;
                    else if (hero1._heros.ID > hero2._heros.ID)
                        return 1;
                    else 
                    {
                        return 0;
                    }
                }
            });
    }
}
