using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroView : HeroViewBase
{
    public enum EWORKER
    {
        ALL = 20000,
        Worker_shuchu,
        Worker_fangyu,
        Worker_gongshou,
        Worker_fuzhu,
        Worker__moushi
    }


    public GameObject HeadInfo;
    public GameObject Lock;

    private Transform lockobj;
    Dictionary<GameObject, EWORKER> imageWorker;
    Dictionary<HeroInfo, Transform> heroGo;
    Dictionary<EWORKER, GameObject> workerGo;

    GridLayerExpand gle;
    ScrollRect sr;
    RectTransform rt;

    private bool isDrag;
    public override void Awake()
    {
        base.Awake();
        workerGo = new Dictionary<EWORKER, GameObject>
        {
            { EWORKER.Worker_fangyu, Fangyu_obj },
            { EWORKER.Worker_fuzhu, Fuzhu_obj },
            { EWORKER.Worker_gongshou, Gongshou_obj },
            { EWORKER.Worker_shuchu, Shuchu_obj },
            { EWORKER.Worker__moushi, Moushi_obj }
        };

        gle = Content_obj.GetComponent<GridLayerExpand>();
        
        sr = ScrollView_obj.GetComponent<ScrollRect>();
        rt = Content_obj.GetComponent<RectTransform>();
        lockobj = Instantiate(Lock, transform, false).transform;

        HeroData[] allHero = HeroMgr.GetSingleton().Heros;
        //英雄分类       
        heroGo = new Dictionary<HeroInfo, Transform>();
        for (int i = allHero.Length - 1; i >= 0; --i)
        {
            EWORKER worker = (EWORKER)allHero[i].JsonData.type;
            GameObject info = Instantiate(HeadInfo, workerGo[worker].transform, false);
            HeroInfo heroInfo = info.GetComponent<HeroInfo>();
            heroInfo.SetData(allHero[i]);
            EventListener.Get(info).OnClick = e =>
            {
                if (isDrag)
                    return;
                UIFace.GetSingleton().Open(UIID.HeroDetail, heroInfo.heroData.HeroId);
            };
            EventListener.Get(info).BegineDragEvent = e =>
            {
                isDrag = true;
                sr.OnBeginDrag(e);
            };
            EventListener.Get(info).DragEvent = e =>
            {
                sr.OnDrag(e);
            };
            EventListener.Get(info).EndDragEvent = e =>
            {
                isDrag = false;
                sr.OnEndDrag(e);
            };
            heroGo.Add(heroInfo, info.transform);
        }
        foreach (var item in workerGo)
        {
            item.Value.SetActive(false);
        }
        //图片分类
        imageWorker = new Dictionary<GameObject, EWORKER>
        {
            { all_img.gameObject, EWORKER.ALL },
            { shuchu_img.gameObject, EWORKER.Worker_shuchu },
            { fangyu_img.gameObject, EWORKER.Worker_fangyu },
            { gongshou_img.gameObject, EWORKER.Worker_gongshou },
            { fuzhu_img.gameObject, EWORKER.Worker_fuzhu },
            { moushi_img.gameObject, EWORKER.Worker__moushi }
        };

        TriggerListenr();
    }
    private void TriggerListenr()
    {
        EventListener.Get(all_img.gameObject).OnClick = 
        EventListener.Get(shuchu_img.gameObject).OnClick = 
        EventListener.Get(fangyu_img.gameObject).OnClick = 
        EventListener.Get(gongshou_img.gameObject).OnClick = 
        EventListener.Get(fuzhu_img.gameObject).OnClick = 
        EventListener.Get(moushi_img.gameObject).OnClick = Listenr;
    }

    private void Listenr(GameObject go)
    {
        if (!imageWorker.ContainsKey(go))
            return;
        EWORKER worker = imageWorker[go];
        ProcessCtrl.Instance.GoCoroutine("ShowWorker", ShowWorker(worker));
    }
    public void Open()
    {
        all_img.GetComponent<Toggle>().isOn = true;
        ProcessCtrl.Instance.GoCoroutine("ShowWorker", ShowWorker(EWORKER.ALL));
        ZEventSystem.Register(EventConst.HEROINFOCHANGE, this, "RefreshHeroInfo");
    }
    private IEnumerator ShowWorker(EWORKER worker)
    {
        List<HeroInfo> show = ListPool<HeroInfo>.Get();
        HeroInfo[] showList = Content_obj.GetComponentsInChildren<HeroInfo>();
        for (int i = 0, count = showList.Length; i < count; ++i)
        {
            showList[i].transform.SetParent(workerGo[(EWORKER)showList[i].Type].transform, false);
        }
        if (workerGo.ContainsKey(worker))
        {
            HeroInfo[] showList2 = workerGo[worker].GetComponentsInChildren<HeroInfo>();
            for (int i = 0, count = showList2.Length; i < count; ++i)
            {
                if (showList2[i].Type == (int)worker)
                    show.Add(showList2[i]);
            }
        }
        else
        {
            foreach (var item in workerGo)
            {
                show.AddRange(item.Value.GetComponentsInChildren<HeroInfo>());
            }
        }
        _sortHero(show);
        int minus = 0;
        for (int i = 0, length = show.Count; i < length; ++i)
        {
            if (!show[i].heroData.UnLock)
            {
                if (!lockobj.gameObject.activeInHierarchy)
                    lockobj.gameObject.SetActive(true);
                lockobj.SetParent(Content_obj.transform, false);
                minus = 1;
            }
            show[i].transform.SetParent(Content_obj.transform, false);
            if (i == length - 1 && lockobj.gameObject.activeInHierarchy && lockobj.parent != Content_obj.transform)
            {
                lockobj.gameObject.SetActive(false);
                minus = 0;
            }
            Show(show[i]);
        }
        ListPool<HeroInfo>.Release(show);
        yield return new WaitForEndOfFrame();
        int plus = lockobj.gameObject.activeInHierarchy ? 1 : 0;
        float maxHeight = (Mathf.CeilToInt((Content_obj.transform.childCount - minus) * 0.5f)) * (gle.spacing.y + gle.cellSize.y) + gle.ignorerSize.y * plus;
        float maxWidth = (gle.spacing.x + gle.cellSize.x) * 2;
        rt.sizeDelta = new Vector2(maxWidth, maxHeight);
        gle.Execute();
    }

    public void RefreshHeroInfo()
    {
        foreach (var item in heroGo)
        {
            item.Key.SetView();
        }
    }

    public void Close()
    {
        HeroInfo[] showList = Content_obj.GetComponentsInChildren<HeroInfo>();
        for (int i = 0, count = showList.Length; i < count; ++i)
        {
            showList[i].Close();
            showList[i].transform.SetParent(workerGo[(EWORKER)showList[i].Type].transform, false);
        }
        ZEventSystem.DeRegister(EventConst.HEROINFOCHANGE);
    }
    public void Destroy()
    {
        ProcessCtrl.Instance.KillCoroutine("ShowWorker");
    }

    private void Show(HeroInfo info)
    {
        info.SetView();
    }
    private void _sortHero(List<HeroInfo> show)
    {
        show.Sort((HeroInfo a, HeroInfo b) =>
        {
            if (a.heroData.UnLock && b.heroData.UnLock)
            {
                if (a.heroData.Level > b.heroData.Level)
                    return -1;
                else if (a.heroData.Level < b.heroData.Level)
                    return 1;
                else
                {
                    if (a.heroData.Star > b.heroData.Star)
                        return -1;
                    else if (a.heroData.Star < b.heroData.Star)
                        return 1;
                    else
                    {
                        if (a.heroData.HeroId > b.heroData.HeroId)
                            return 1;
                        else if (a.heroData.HeroId < b.heroData.HeroId)
                            return -1;
                        else
                            return 0;
                    }
                }
            }
            else if (a.heroData.UnLock && !b.heroData.UnLock)
                return -1;
            else if (!a.heroData.UnLock && b.heroData.UnLock)
                return 1;
            else
            {
                if (a.heroData.Piece > a.heroData.Piece)
                    return -1;
                else if (a.heroData.Piece < a.heroData.Piece)
                    return 1;
                else
                {
                    if (a.heroData.HeroId > b.heroData.HeroId)
                        return 1;
                    else if (a.heroData.HeroId < b.heroData.HeroId)
                        return -1;
                    else
                        return 0;
                }
            }
        });
    }
}
