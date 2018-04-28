using JsonData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StubView : StubViewBase {
    //武将头像
    public HeroHeadView HeroHead = null;
    //布阵Node
    public RectTransform StubNode = null;

    private ScrollRect _herosScroll = null;
    private GridLayoutGroup _heroGrid = null;
    private RectTransform _heroGridRect = null;

    private bool _isDraging = false;
    private int _dragHero = 0;
    private int _dragPos = 0;
    private int _selectNode = 0;

    [HideInInspector]
    public Camera UICamera = null;

    [HideInInspector]
    public Dictionary<int, int> StubData = new Dictionary<int, int>();      //[stubpos:heroid]
    [HideInInspector]
    public Dictionary<int, bool> StubOpen = new Dictionary<int, bool>();

    private List<HeroHeadView> _herosBuffer = new List<HeroHeadView>();
    private Dictionary<int, StubNodeView> _stubPosDic = new Dictionary<int, StubNodeView>();
    private Dictionary<int, GameObject> _stubHeroDic = new Dictionary<int, GameObject>();
    private GameObject _stubShowHero = null;
    private RectTransform _stubDragRect = null;
    private float _headDragTime = 0;

    public const int StubRange = 5;

    private const float StubXSpace = 213;

    private const float StubYSpace = 127;

    private const float LeanOffset = 60;

    private const float StubXSpace3D = 5.3f;

    private const float StubYSpace3D = 3.1f;

    private const float LeanOffset3D = 1.53f;

    private const int StubMax = 5;

    private const float ShowBBSize = 11;

    private const float DragBBSize = 2.2f;

    public override void Awake()
    {
        base.Awake();
        _herosScroll = heros_obj.GetComponent<ScrollRect>();
        _heroGrid = herogrid_obj.GetComponent<GridLayoutGroup>();
        _heroGridRect = herogrid_obj.GetComponent<RectTransform>();
        _stubDragRect = StubDrag_bb.gameObject.GetComponent<RectTransform>();
        _initStubNode();

        //Test，此处应当是阵法
        for (int stubx = 0; stubx < StubRange; ++stubx)
        {
            for (int stuby = 0; stuby < StubRange; ++stuby)
                StubOpen.Add((stubx + 1) * 10 + stuby + 1, true);
        }
    }

    public void Start()
    {
        StubShow_bb.OrthoSize = ShowBBSize;
        StubDrag_bb.OrthoSize = DragBBSize;
    }

    public void InitHero()
    {
        _updateHeadList();

        _resetStubNode();

        foreach (var p in StubData)
        {
            _setStubHero(p.Key, true, p.Value);
        }

        #region 先保留
        //EventListener.Get(StubShow_obj).DragEvent = e =>
        //{
        //    Vector2 mousePos = e.position;

        //    float mousePosX = mousePos.x / Screen.width;
        //    float mousePosY = mousePos.y / Screen.height;

        //    RectTransform stubShowRect = StubShow_obj.GetComponent<RectTransform>();

        //    if (UICamera == null)
        //    {
        //        UICamera = GameObject.Find("Canvas/Camera").GetComponent<Camera>();
        //    }
        //    Vector2 localPos = new Vector2(9999, 9999);
        //    if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(stubShowRect, mousePos, UICamera, out localPos))
        //    {
        //        return;
        //    }

        //    Ray ray = StubShow_bb.GetCam().ScreenPointToRay(new Vector3(localPos.x + stubShowRect.sizeDelta.x * 0.5f,
        //        localPos.y + stubShowRect.sizeDelta.y * 0.5f, 0));

        //    Debug.DrawLine(ray.origin, ray.origin + ray.direction * 500.0f);

        //    RaycastHit hitinfo;
        //    if (Physics.Raycast(ray, out hitinfo))
        //    {
        //        Debug.Log(hitinfo.collider.gameObject);
        //    }
        //};
        //EventListener.Get(StubShow_obj).OnClick = e =>
        //{
        //    //Debug.Log("DragBoard OnClick");
        //};
        //EventListener.Get(StubShow_obj).PointerEnterEvent = e =>
        //{
        //    Debug.Log("Pointer Enter ");
        //};

        //EventListener.Get(StubShow_obj).PointerExitEvent = e =>
        //{
        //    Debug.Log("Pointer Exit ");
        //};

        //_setStubHero(11, true, 11100);
        //_setStubHero(12, true, 11100);
        //_setStubHero(13, true, 11100);
        //_setStubHero(14, true, 11100);
        //_setStubHero(15, true, 11100);
        #endregion

    }

    public void InitStubData(StubType type)
    {
        List<Vector2Int> RoleStubData = Role.Instance.GetStubData(type);
        StubData.Clear();
        for (int idx = 0; idx < RoleStubData.Count; ++idx)
        {
            StubData.Add(RoleStubData[idx].x, RoleStubData[idx].y);
        }
    }

    public void Clear()
    {
        foreach (var p in _stubHeroDic)
            DestroyImmediate(p.Value);
        _stubHeroDic.Clear();
    }

    private void _initStubNode()
    {
        RectTransform StubObjRect = StubPos_obj.GetComponent<RectTransform>();
        for (int stubx = 0; stubx < StubRange; ++stubx)
        {
            for (int stuby = 0; stuby < StubRange; ++stuby)
            {
                int stubPos = (stubx + 1) * 10 + (stuby + 1);
                GameObject stubnode = GameObject.Instantiate(StubNode.gameObject, this.StubShow_obj.transform);
                stubnode.name = string.Format("stubnode_{0}", stubPos);
                stubnode.transform.SetParent(StubPos_obj.transform);
                RectTransform stubNodeRect = stubnode.GetComponent<RectTransform>();
                stubNodeRect.anchoredPosition = new Vector2((StubRange - stuby) * LeanOffset + stubx * StubXSpace - StubObjRect.sizeDelta.x / 2 + 60,
                    ((StubRange - stuby) * StubYSpace - StubObjRect.sizeDelta.y / 2 - 20));

                StubNodeView nodeView = stubnode.GetComponent<StubNodeView>();
                nodeView.SetStubPos(stubPos);
                nodeView.SetStubState(StubState.Open);

                _stubPosDic.Add(stubPos, nodeView);

                EventListener.Get(stubnode).BegineDragEvent = e =>
                {
                    if (StubData.ContainsKey(stubPos))
                    {
                        _startDrag(StubData[stubPos], stubPos);
                        nodeView.SetStubState(StubState.Open);
                    }
                };

                EventListener.Get(stubnode).DragEvent = e =>
                {
                    _onDrag(e);
                };

                EventListener.Get(stubnode).EndDragEvent = e =>
                {
                    _endDrag();
                };

                EventListener.Get(stubnode).PointerEnter = e =>
                {
                    if (_isDraging && StubOpen[stubPos])
                    {
                        StubNodeView nView = stubnode.GetComponent<StubNodeView>();
                        nView.SetStubState(StubState.Select);
                        _selectNode = nView.StubPos;
                    }
                };

                EventListener.Get(stubnode).PointerExit = e =>
                {
                    if (_isDraging && StubOpen[stubPos])
                    {
                        StubNodeView nView = stubnode.GetComponent<StubNodeView>();
                        nView.RevertState();
                        _selectNode = 0;
                    }
                };
            }
        }
    }

    private bool isDrag;
    private bool isMove;

    private void _createHero()
    {
        GameObject heroHead = GameObject.Instantiate(HeroHead.gameObject);
        heroHead.transform.SetParent(herogrid_obj.transform);
        RectTransform headRect = heroHead.GetComponent<RectTransform>();
        headRect.anchoredPosition3D = Vector3.zero;
        heroHead.transform.localScale = Vector3.one;

        EventListener.Get(heroHead).BegineDragEvent = e =>
        {
            isMove = true;
            _herosScroll.OnBeginDrag(e);
        };
        EventListener.Get(heroHead).DragEvent = e =>
        {
            if(isMove)
            {
                if ((e.delta.y >= 10 || e.delta.x >= 10) && isMove)
                {
                    isMove = false;
                    isDrag = Mathf.Abs(e.delta.y) > e.delta.x;
                    if (!isDrag)
                    {
                        HeroHeadView hv = heroHead.GetComponent<HeroHeadView>();
                        if (StubData.ContainsValue(hv.HeroId))  //已经上阵
                            return;
                        _startDrag(hv.HeroId);
                    }
                    return;
                }
            }

            if(!_isDraging)
                _herosScroll.OnDrag(e);
            _onDrag(e);
        };
        EventListener.Get(heroHead).EndDragEvent = e =>
        {
            _endDrag();
            _herosScroll.OnEndDrag(e);
        };

        //EventListener.Get(heroHead).PointerDown = e =>
        //{
        //    HeroHeadView hv = heroHead.GetComponent<HeroHeadView>();
        //    if (StubData.ContainsValue(hv.HeroId))  //已经上阵
        //        return;
        //    _headDragTime = Time.time;
        //    ProcessCtrl.Instance.GoCoroutine("DelayStartDrag", _delayStartDrag(hv.HeroId, e));
        //};

        //EventListener.Get(heroHead).PointerUp = e =>
        //{
        //    _headDragTime = 0;
        //    _endDrag();
        //};

        HeroHeadView headView = heroHead.GetComponent<HeroHeadView>();
        headView.Init();
        _herosBuffer.Add(headView);
    }

    private IEnumerator _delayStartDrag(int heroId, PointerEventData data)
    {
        if (ProcessCtrl.Instance.StubDragDelay == null)
            ProcessCtrl.Instance.StubDragDelay = new WaitForSeconds(JsonMgr.GetSingleton().GetGlobalIntArrayByID(1020).value);
        yield return ProcessCtrl.Instance.StubDragDelay;
        if (_headDragTime != 0)
        {
            _startDrag(heroId);
            _onDrag(data);
        }
    }

    private void _startDrag(int heroId, int stubPos = 0)
    {
        if (stubPos != 0 && !StubData.ContainsKey(stubPos))
        {
            return;
        }
        _isDraging = true;
        _dragHero = heroId;
        _dragPos = stubPos;
        if (stubPos != 0)
        {
            _setStubHero(stubPos);
            _setShowHero(true, StubData[stubPos]);
        }
        else
        {
            _setShowHero(true, heroId);
        }
    }

    private void _onDrag(PointerEventData data)
    {
        Vector3 worldPos = new Vector3(9999, 9999, 9999);
        if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(_stubDragRect, data.position, CanvasView.Instance.uicamera, out worldPos))
        {
            return;
        }
        _stubDragRect.position = worldPos;
    }

    private void _endDrag()
    {
        if (_selectNode != 0)
        {
            if (_dragPos != 0)
            {
                _stubChange(_dragPos, _selectNode);
            }
            else if (_dragHero != 0)
            {
                _stubOn(_dragHero, _selectNode);
            }
        }
        else
        {
            if (_dragPos != 0)
            {
                if (StubData.Count == 1)
                {
                    //要下阵最后一个人
                    if (StubData.ContainsKey(_dragPos))
                    {
                        int stubheroid = StubData[_dragPos];
                        _setStubHero(_dragPos, true, stubheroid);
                    }
                    CanvasView.Instance.AddNotice(JsonMgr.GetSingleton().GetGlobalStringArrayByID(2001).desc);
                }
                else
                    _stubOff(_dragPos);
            }
        }
        _setShowHero();
        _isDraging = false;
        _dragHero = 0;
        _dragPos = 0;
        _selectNode = 0;
    }

    private void _updateHeadList()
    {
        List<HeroData> unLockedHeros = new List<HeroData>(HeroMgr.GetSingleton().Heros);

        unLockedHeros.Sort((HeroData a, HeroData b) => 
        {
            if (StubData.ContainsValue(a.HeroId) && !StubData.ContainsValue(b.HeroId))
                return -1;
            else if (!StubData.ContainsValue(a.HeroId) && StubData.ContainsValue(b.HeroId))
                return 1;
            else
                return a.HeroId < b.HeroId ? -1 : (a.HeroId > b.HeroId ? 1 : 0);
        });
        for (int idx = 0; idx < unLockedHeros.Count; ++idx)
        {
            HeroData data = unLockedHeros[idx];
            if (idx >= _herosBuffer.Count)
                _createHero();
            _herosBuffer[idx].gameObject.name = string.Format("HeroHead_{0}", data.HeroId);
            _herosBuffer[idx].SetHeroInfo(data.HeroId, data.Rare, data.Star, data.Level);
            _herosBuffer[idx].SetOnStub(StubData.ContainsValue(data.HeroId));
        }
        float maxHeight = Mathf.RoundToInt(_heroGrid.transform.childCount / 2.0f) * (_heroGrid.spacing.y + _heroGrid.cellSize.y);
        float maxWidth = (_heroGrid.spacing.x + _heroGrid.cellSize.x * 2);
        _heroGridRect.sizeDelta = new Vector2(maxWidth, maxHeight);
    }

    private void _setShowHero(bool isCreate = false, int heroId = 0)
    {
        if (_stubShowHero != null)
        {
            DestroyImmediate(_stubShowHero);
            _stubShowHero = null;
        }
        if (isCreate)
        {
            Hero heroData = JsonMgr.GetSingleton().GetHeroByID(heroId);
            GameObject heroGo = GameObject.Instantiate(ResourceMgr.Instance.LoadResource(heroData.ID) as GameObject,
                    StubDrag_bb.GetCamPos() + new Vector3(0, -2, 5), Quaternion.Euler(0, 125.75f, 0));
            if (heroGo != null)
            {
                _stubShowHero = heroGo;
                if (heroData.horseid != 0)
                {
                    GameObject horseGo = GameObject.Instantiate(ResourceMgr.Instance.LoadResource(heroData.horseid) as GameObject, heroGo.transform);
                    horseGo.transform.localPosition = Vector3.zero;
                    horseGo.transform.localScale = Vector3.one;
                }
                _stubShowHero.SetLayer(StubDrag_bb.Layer);
            }
        }
    }

    private void _resetStubNode()
    {
        for (int stubx = 0; stubx < StubRange; ++stubx)
        {
            for (int stuby = 0; stuby < StubRange; ++stuby)
            {
                int stubpos = (stubx + 1) * 10 + stuby + 1;
                StubState state = StubData.ContainsKey(stubpos) ? StubState.Stubed : StubOpen[stubpos] ? StubState.Open : StubState.Locked;
                _stubPosDic[stubpos].SetStubState(state);
            }
        }
    }

    private void _setStubHero(int stubPos, bool isCreate = false, int heroId = 0)
    {
        //不管是创建还是删除，都得先删除了
        if (_stubHeroDic.ContainsKey(stubPos))
        {
            DestroyImmediate(_stubHeroDic[stubPos]);
            _stubHeroDic.Remove(stubPos);
            _stubPosDic[stubPos].SetStubState(StubState.Open);
        }
        if (isCreate)
        {
            UIBillboard _bb = null;
            if (0 == stubPos)
                _bb = StubDrag_bb;
            else
                _bb = StubShow_bb;
            if (_bb == null)
            {
                EDebug.LogErrorFormat("StubView._setStubHero error, stubPos:{0}", stubPos);
                return;
            }
            Hero heroData = JsonMgr.GetSingleton().GetHeroByID(heroId);
            GameObject heroGo = null;
            if (stubPos != 0)
            {
                int stubX = stubPos / 10;
                int stubY = stubPos % 10;
                heroGo = GameObject.Instantiate(ResourceMgr.Instance.LoadResource(heroData.ID) as GameObject,
                    _bb.GetCamPos() + new Vector3((stubX - 1) * StubXSpace3D + (6 - stubY) * LeanOffset3D - 15f, 
                    (6 - stubY) * StubYSpace3D - 11f, 5), Quaternion.Euler(0, 125.75f, 0));
                heroGo.transform.localScale = Vector3.one * 1.8f;
            }
            else
            {
                heroGo = GameObject.Instantiate(ResourceMgr.Instance.LoadResource(heroData.ID) as GameObject,
                    _bb.GetCamPos() + new Vector3(0, 0, 5),
                    Quaternion.Euler(0, 125.75f, 0));
            }
            if (heroGo != null)
            {
                _stubHeroDic.Add(stubPos, heroGo);
                if (heroData.horseid != 0)
                {
                    GameObject horseGo = GameObject.Instantiate(ResourceMgr.Instance.LoadResource(heroData.horseid) as GameObject, heroGo.transform);
                    horseGo.transform.localPosition = Vector3.zero;
                    horseGo.transform.localScale = Vector3.one;
                }
                heroGo.SetLayer(_bb.Layer);
            }
            else
            {
                EDebug.LogErrorFormat("StubView._setStubHero error, load heroGo failed, heroId:{0}", heroData.ID);
            }
            _stubPosDic[stubPos].SetStubState(StubState.Stubed);
        }
        
    }

    private bool _stubOn(int heroId, int stubPos)
    {
        if (!StubData.ContainsKey(stubPos) && StubData.Count >= StubMax)
        {
            CanvasView.Instance.AddNotice(JsonMgr.GetSingleton().GetGlobalStringArrayByID(2002).desc);
            _stubPosDic[stubPos].RevertState();
            return false;
        }
        StubData[stubPos] = heroId;
        _setStubHero(stubPos, true, heroId);
        _updateHeadList();
        return true;
    }
    private void _stubChange(int stubPos1, int stubPos2)
    {
        if (StubData.ContainsKey(stubPos1))
        {
            if (!StubData.ContainsKey(stubPos2))
            {
                StubData[stubPos2] = StubData[stubPos1];
                StubData.Remove(stubPos1);

                _setStubHero(stubPos1);
                _setStubHero(stubPos2, true, StubData[stubPos2]);
            }
            else
            {
                int tmpHero = StubData[stubPos2];
                StubData[stubPos2] = StubData[stubPos1];
                StubData[stubPos1] = tmpHero;
                _setStubHero(stubPos1, true, StubData[stubPos1]);
                _setStubHero(stubPos2, true, StubData[stubPos2]);
            }
        }
        else
        {
            EDebug.LogErrorFormat("StubView._stubChange error, stubPos1 not contains, {0} {1}", stubPos1, stubPos2);
        }
    }
    private void _stubOff(int stubPos)
    {
        if (StubData.ContainsKey(stubPos))
        {
            StubData.Remove(stubPos);
            _setStubHero(stubPos);
            _updateHeadList();
        }
    }
}
