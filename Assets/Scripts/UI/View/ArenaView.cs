using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaView : ArenaViewBase {

    public HeroHeadView HeroHead = null;

    public List<ArenaNodeView> EnemyList = null;

    private List<HeroHeadView> _heroHeads = new List<HeroHeadView>();

    public void InitAreanDefend()
    {
        List<Vector2Int> stubData = Role.Instance.GetStubData(StubType.PVPDefend);
        int needHead = stubData.Count - _heroHeads.Count;
        if (needHead > 0)
        {
            for (int idx = 0; idx < needHead; ++idx)
            {
                GameObject HeroHeadGo = GameObject.Instantiate(HeroHead.gameObject, this.defend_obj.transform);
                HeroHeadView view = HeroHeadGo.GetComponent<HeroHeadView>();
                view.Init();
                _heroHeads.Add(view);
            }  
        }

        for (int idx = 0; idx < _heroHeads.Count; ++idx)
        {
            if (idx < stubData.Count)
            {
                int heroId = (int)stubData[idx].y;
                JsonData.Hero jHero = JsonMgr.GetSingleton().GetHeroByID(heroId);
                if (jHero == null)
                {
                    EDebug.LogErrorFormat("couldn't find hero {0} in json", heroId);
                    return;
                }
                HeroData hero = HeroMgr.GetSingleton().GetHeroData(heroId);
                if (hero == null)
                    EDebug.LogErrorFormat("couldn't find hero {0}", heroId);
                else
                    _heroHeads[idx].SetHeroInfo(jHero.headid, hero.Rare, hero.Star, hero.Level);
            }
            else
                _heroHeads[idx].gameObject.SetActive(false);
        }
    }

    public void InitEnemy()
    {
        for (int idx = 0; idx < EnemyList.Count; ++idx)
        {
            EnemyList[idx].SetInfo((uint)idx, idx, string.Format("董大喵{0}", idx), 100 - idx, idx, (int)Mathf.Pow(idx + 1, idx + 1));
        }
    }
}
