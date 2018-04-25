using System.Collections;
using UnityEngine;

public class ExpPoolView : ExpPoolViewBase
{
    private HeroData heroData;

    private Transform[] starImage;
    private int tempLv;
    private int cost;

    public override void Awake()
    {
        base.Awake();
        EventListener.Get(cancel_btn.gameObject).OnClick = e =>
        {
            UIFace.GetSingleton().Close(UIID.ExpPool);
        };
        EventListener.Get(add_btn.gameObject).PointerDown = e =>
        {
            ProcessCtrl.Instance.GoCoroutine("OnPress", OnPress(true));
        };
        EventListener.Get(add_btn.gameObject).PointerUp = e =>
        {
            ProcessCtrl.Instance.KillCoroutine("OnPress");
        };
        EventListener.Get(minus_btn.gameObject).PointerDown = e =>
        {
            ProcessCtrl.Instance.GoCoroutine("OnPress", OnPress(false));
        };
        EventListener.Get(minus_btn.gameObject).PointerUp = e =>
        {
            ProcessCtrl.Instance.KillCoroutine("OnPress");
        };

        EventListener.Get(minus_btn.gameObject).OnClick = e =>
        {
            int current = 1;

            if (int.TryParse(targetLv_txt.text, out current))
            {
                if (tempLv <= heroData.Level)
                {
                    CanvasView.Instance.AddNotice("不能再低了");
                    return;
                }
                current--;
                tempLv = current;
                cost = ComputerExpend();
                targetLv_txt.text = tempLv.ToString();
                expend_txt.text = cost.ToString("N0");
            }
        };
        EventListener.Get(add_btn.gameObject).OnClick = e =>
        {
            int current = 1;

            if (int.TryParse(targetLv_txt.text, out current))
            {
                if (current >= HeroMgr.MaxHeroLevel)
                {
                    CanvasView.Instance.AddNotice("等级已达上限");
                    return;
                }
                current++;
                tempLv = current;
                cost = ComputerExpend();
                if (Role.Instance.ExpPool < cost)
                {
                    tempLv--;
                    cost = ComputerExpend();
                    CanvasView.Instance.AddNotice("经验不够了");
                    return;
                }
                targetLv_txt.text = current.ToString();
                expend_txt.text = cost.ToString("N0");
            }
        };
    }

    public IEnumerator OnPress(bool plus)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            int current = heroData.Level;

            if (int.TryParse(targetLv_txt.text, out current))
            {
                if (!plus && tempLv <= heroData.Level)
                {
                    CanvasView.Instance.AddNotice("不能再低了");
                    yield break;
                }
                if (plus && tempLv == HeroMgr.MaxHeroLevel)
                {
                    CanvasView.Instance.AddNotice("等级已达上限");
                    yield break;
                }
                current += plus ? 1 : -1;
                tempLv = current;
                cost = ComputerExpend();

                if (Role.Instance.ExpPool < cost)
                {
                    tempLv--;
                    cost = ComputerExpend();
                    CanvasView.Instance.AddNotice("经验不够了");
                    yield break;
                }
                targetLv_txt.text = current.ToString();
                expend_txt.text = cost.ToString("N0");
            }
        }
    }

    public void Open(int heroId)
    {
        heroData = HeroMgr.GetSingleton().GetHeroData(heroId);

        SetHeadView();

        long exp = Role.Instance.ExpPool;
        exp_txt.text = exp.ToString("N0");
        tempLv = heroData.Level;

        if (exp < heroData.NeedHero || heroData.Level == HeroMgr.MaxHeroLevel)
        {
            levelup_btn.interactable = false;
            expend_txt.text = heroData.NeedHero.ToString("N0");
            targetLv_txt.text = tempLv.ToString();
            return;
        }
        levelup_btn.interactable = true;

        do
        {
            cost = ComputerExpend();
            if (cost > exp)
            {
                tempLv--;
                cost = ComputerExpend();
                break;
            }
            if(tempLv == HeroMgr.MaxHeroLevel)
            {
                break;
            }
            tempLv++;
        } while (tempLv <= HeroMgr.MaxHeroLevel);

        expend_txt.text = cost.ToString("N0");
        targetLv_txt.text = tempLv.ToString();

        EventListener.Get(levelup_btn.gameObject).OnClick = e =>
        {
            if (!levelup_btn.IsInteractable())
                return;
            Role.Instance.ExpPool -= cost;
            heroData.Level = tempLv;
            Lv_txt.text = heroData.Level.ToString();
            ZEventSystem.Dispatch(EventConst.REFRESHSIDE,false);
            exp_txt.text = Role.Instance.ExpPool.ToString("N0");
            UIFace.GetSingleton().Close(UIID.ExpPool);
        };
    }
    private int ComputerExpend()
    {
        int expend = 0;
        for (int i = heroData.Level; i < tempLv; ++i)
        {
            expend += JsonMgr.GetSingleton().GetExpByID(i).NeedHero;
        }            
        return expend;
    }

    private void SetHeadView()
    {
        HeroRare officerUp = JsonMgr.GetSingleton().GetHeroRareByID(heroData.Rare);
        Border_img.sprite = ResourceMgr.Instance.LoadSprite(officerUp.HeadBorder);
        Head_img.sprite = ResourceMgr.Instance.LoadSprite(heroData.JsonData.headid);
        Lv_txt.text = heroData.Level.ToString();

        int showStar = HeroMgr.MaxHeroStar - heroData.Star;
        starImage = HideStar_obj.transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < showStar; ++i)
        {
            starImage[i].SetParent(StarParent_obj.transform);
        }
    }

    public void Close()
    {
        for (int i = 0; i < starImage.Length; ++i)
        {
            starImage[i].SetParent(HideStar_obj.transform);
        }
    }
}
