using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagSelectView : UIViewBase {
    public RectTransform flagnode = null;
    [HideInInspector]
    public ScrollRect flagscroll_obj;
    [HideInInspector]
    public RectTransform flaglist_obj;

    private List<GameObject> flaglist = new List<GameObject>();

    public const int FlagStartId = 20051;
    private const int FlagCnt = 17;
    void Awake()
    {
        this.go = this.gameObject;
        this.flagscroll_obj = this.transform.Find("flagscroll_obj").GetComponent<ScrollRect>();
        this.flaglist_obj = this.transform.Find("flagscroll_obj/viewport/flaglist_obj").GetComponent<RectTransform>();
    }

    public void CreateFlagNode()
    {
        for (int flagidx = 0; flagidx < FlagCnt; ++flagidx)
        {
            if (flagidx >= flaglist.Count)
            {
                GameObject flagnodego = GameObject.Instantiate(flagnode.gameObject, flaglist_obj);
                int flagid = FlagStartId + flagidx;
                flagnodego.name = string.Format("flagnode_{0}", flagid);
                flagnodego.GetComponent<Image>().sprite = ResourceMgr.Instance.LoadSprite(flagid);
                flaglist.Add(flagnodego);
                EventListener.Get(flagnodego).OnClick = e =>
                {
                    ZEventSystem.Dispatch(EventConst.OnSelectFlag, flagid);
                    UIFace.GetSingleton().Close(UIID.FlagSelect);
                };
                EventListener.Get(flagnodego).BegineDragEvent = e =>
                {
                    flagscroll_obj.OnBeginDrag(e);
                };
                EventListener.Get(flagnodego).DragEvent = e =>
                {
                    flagscroll_obj.OnDrag(e);
                };
                EventListener.Get(flagnodego).EndDragEvent = e =>
                {
                    flagscroll_obj.OnEndDrag(e);
                };
            }
        }
        flaglist_obj.sizeDelta = new Vector2(FlagCnt * flagnode.sizeDelta.x, flaglist_obj.sizeDelta.y);

    }
}
