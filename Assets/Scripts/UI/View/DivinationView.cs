using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DivinationView : DivinationViewBase
{
    public DivinationTree[] mt;
    public GameObject item;
    private ScaleTween st;
    private GridLayoutGroup glg;
    private float maxLucky;

    public static int id = 0;

    public int progress;
    public override void Awake()
    {
        base.Awake();
		glg = itemparent_trf.GetComponent<GridLayoutGroup> ();
        maxLucky = JsonMgr.GetSingleton().GetGlobalIntArrayByID(1021).value;
    }
	void Start()
	{
		for (int i = 0; i < mt.Length; ++i)
		{
			DivinationTree dt = mt[i];
			dt.progress = progress;
            dt.SetView(i);
            ScaleTween st = dt.get_btn.GetComponent<ScaleTween>();

            EventListener.Get(dt.get_btn.gameObject).OnClick = e =>
			{
				if (!dt.get_btn.interactable)
					return;
				int pro = dt.RandomTree();
                Instantiate("totem" +  ++id);
                st.ResetToBeginning();
                st.PlayForward();
                SetProgress(pro);
			};
		}
		st = tip_obj.GetComponent<ScaleTween>();
		st.AddOnFinished(delegate () { tip_obj.SetActive(false); });
		EventListener.Get(tip_btn.gameObject).OnClick = e =>
		{
			tip_obj.SetActive(true);
			st.ResetToBeginning();
			st.PlayForward();
		};

		EventListener.Get(divination20_btn.gameObject).OnClick = e =>
		{
            ProcessCtrl.Instance.GoCoroutine("OneKeyinstantiate", OneKeyinstantiate());   
        };
	}
    private IEnumerator OneKeyinstantiate()
    {
        int pro = mt[progress].RandomTree();
        for (uint i = 0; i < 20; i++)
        {
            Instantiate("totem" + ++id);
            yield return new WaitForSeconds(0.1f);
        }
        SetProgress(pro);
    }

    private void Instantiate(string id)
    {
        GameObject go = Instantiate(item, itemparent_trf, false);
        DivinationItemView div = go.GetComponent<DivinationItemView>();
        TotemData td = TotemMgr.GetSingleton().GetTotemByID(id);
        if (td == null)
        {
            td = new TotemData(40007)
            {
                md5 = id
            };
            TotemMgr.GetSingleton().AddTotem(td);
        }
        div.SetView(td);
        EventListener.Get(go).OnClick = e =>
        {
            ZEventSystem.Register(EventConst.ONOPENCOMPOSE, this, "OnOpenCompose");
            UIFace.GetSingleton().Open(UIID.DivinationTip, td, SHOWBUTTON.Ecompose);
            if (!select_trf.gameObject.activeInHierarchy)
                select_trf.gameObject.SetActive(true);
            select_trf.SetParent(go.transform);
            select_trf.transform.localPosition = Vector3.zero;
        };
        EventListener.Get(go).BegineDragEvent = e =>
        {
            Item_sr.OnBeginDrag(e);
        };
        EventListener.Get(go).DragEvent = e =>
        {
            Item_sr.OnDrag(e);
        };
        EventListener.Get(go).EndDragEvent = e =>
        {
            Item_sr.OnEndDrag(e);
        };
        Role.Instance.DivinationLucky += JsonMgr.GetSingleton().GetTotemTreeByID(progress + 1).spend;
        lucky_img.fillAmount = Role.Instance.DivinationLucky / maxLucky;
    }
    public void OnOpenCompose()
    {
        Close();
        if (select_trf.gameObject.activeInHierarchy)
            select_trf.gameObject.SetActive(false);
        SetProgress(progress);
        ZEventSystem.DeRegister(EventConst.ONOPENCOMPOSE);
    }
    public void Open()
    {
        select_trf.gameObject.SetActive(false);
        progress = Role.Instance.DivinationTree;
        SetProgress(progress);
        lucky_img.fillAmount = Role.Instance.DivinationLucky / maxLucky;
        EventListener.Get(take_btn.gameObject).OnClick = e =>
        {
            ZEventSystem.Register(EventConst.ONOPENCOMPOSE, this, "OnOpenCompose");
            UIFace.GetSingleton().Open(UIID.HeroDetail, HeroMgr.GetSingleton().GetNo1(), EHEROSHOWTYPE.Divination);
            Close();
            if (select_trf.gameObject.activeInHierarchy)
                select_trf.gameObject.SetActive(false);
            ZEventSystem.DeRegister(EventConst.ONOPENCOMPOSE);
        };
    }

    private void SetProgress(int progress)
    {
        for (int i = 0; i < mt.Length; ++i)
        {
            mt[i].get_btn.interactable = i == progress;
            mt[i].enabled = i == progress;
        }
        Vector2 size = Item_sr.content.sizeDelta;
        int count = itemparent_trf.childCount;
        float y = Mathf.CeilToInt(count * 0.1f) * (glg.spacing.y + glg.cellSize.y);
        size.y = y;
        if(Item_sr.content.childCount >=20)
        {
            Vector2 pos = Item_sr.content.anchoredPosition;
            pos.y = size.y - 144f;
            Item_sr.content.anchoredPosition = pos;
        }
        Item_sr.content.sizeDelta = size;
    }
    public void Close()
    {
        select_trf.SetParent(Item_sr.viewport);
        for (int i = 0,count = itemparent_trf.childCount; i < count; i++)
        {
            DestroyImmediate(itemparent_trf.GetChild(0).gameObject);
        }
        ProcessCtrl.Instance.KillCoroutine("OneKeyinstantiate");
        id = 0;
    }
}
