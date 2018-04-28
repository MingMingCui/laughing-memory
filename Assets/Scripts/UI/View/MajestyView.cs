using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MajestyView: MajestyViewBase
{
    [HideInInspector]
    public Transform ct;
    [HideInInspector]
    private readonly int ID = 11370;
    [HideInInspector]
    public Transform Hreo;
    [HideInInspector]
    public List<PortraitCircleView> PortraitList = new List<PortraitCircleView>();
    public PortraitCircleView Portrait;
    public HeadFrameView Frame;
    private GridLayoutGroup grid;
    private RectTransform Mask_rect ;
    private RectTransform Clear;
        HeroData[] allHero;

    GridLayoutGroup HaveClear;
    GridLayoutGroup NotClear;
    RectTransform HaveRect;
    RectTransform NotClearRect;
    RectTransform Control;
    [HideInInspector]
    public List<HeadFrameView> FrameList = new List<HeadFrameView>();

    float previousX;
    float offset;
    /// <summary>
    /// 初始化头像
    /// </summary>
    public void Init()
    {
        Level_txt.text = Role.Instance.Level.ToString();
        Exp_txt.text = Role.Instance.Exp.ToString();
        Name_txt.text = Role.Instance.RoleName;
        if (ct == null)
        {
            HaveClear = HaveClear_trf.GetComponent<GridLayoutGroup>();
            NotClear = NotClears_trf.GetComponent<GridLayoutGroup>();
            HaveRect = Have_trf as RectTransform;
            NotClearRect = NotClear_trf as RectTransform;
            Control = Control_obj.transform as RectTransform;

            ct = GameObject.Find("HeroParent").transform;
            grid = Clear_trf.GetComponent<GridLayoutGroup>();
            Mask_rect = Mask_trf.transform as RectTransform;
            Clear = Clear_trf.transform as RectTransform;

        }
        if (ct.childCount > 0)
        {
            for (int i = 0, length = ct.childCount; i < length; ++i)
            {
                Destroy(ct.GetChild(0).gameObject);
            }
        }
        GameObject heroGo = ResourceMgr.Instance.LoadResource(ID) as GameObject;
        if (heroGo == null)
            return;
        heroGo = Instantiate(heroGo, ct, false);
        Hreo = heroGo.transform;
        heroGo.transform.localPosition = new Vector3(0, -1, 0);
        heroGo.transform.localScale = new Vector3(2f, 2f, 2f);
        heroGo.SetLayer("Hero");
        EventListener.Get(RawImage_obj).BegineDragEvent = e =>
        {
            previousX = Input.mousePosition.x;
        };
        EventListener.Get(RawImage_obj).DragEvent = e =>
        {
            offset = previousX - Input.mousePosition.x;
            previousX = Input.mousePosition.x;
            heroGo.transform.Rotate(Vector3.up, offset * Time.deltaTime, Space.Self);
        };


        Hreo.transform.localRotation = new Quaternion(0,0,0,0);
       
        Portrait_img.sprite = ResourceMgr.Instance.LoadSprite(Role.Instance.HeadId);
      

        //头像
        allHero = HeroMgr.GetSingleton().Heros;
        if (allHero.Length > PortraitList.Count)
        {
            for (int idx = 0; idx < allHero.Length; idx++)
            {
                if (idx > PortraitList.Count)
                {
                    GameObject majesty = InitItemInfo(true,Clear_trf);
                    PortraitCircleView majestyview = majesty.GetComponent<PortraitCircleView>();
                    majestyview.Init();
                    if (idx > 10)
                    {
                        majestyview.Endow(allHero[idx].HeroId, false);
                    }
                    else
                    {
                        majestyview.Endow(allHero[idx].HeroId, true);
                    }
                    PortraitList.Add(majestyview);
                  
                }
               
            }
        }

        SelectSort();
    }

    /// <summary>
    /// 初始化头像框 
    /// </summary>
    /// <param name="frameid"></param>
    /// <param name="isopen"></param>
    public void FrameInit(List<int> frameid,bool isopen)
    {
        if (isopen)
        {
            if (frameid == null)
            {
                Frame_obj.SetActive(true);
                return;
            }
            if (frameid.Count > FrameList.Count)
            {
                for (int i = 0; i < frameid.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        HeadFrameView head = InitItemInfo(false, HaveClear_trf).GetComponent<HeadFrameView>();
                        head.Init();
                        head.isOwn = true;
                        head.Endow(frameid[i]);
                        FrameList.Add(head);
                    }
                    else
                    {
                        HeadFrameView head = InitItemInfo(false, NotClears_trf).GetComponent<HeadFrameView>();
                        head.Init();
                        head.isOwn = false;
                        head.Endow(frameid[i]);
                        FrameList.Add(head);
                    }

                }
                NotClearRect.anchoredPosition = new Vector2(0,-( HaveClear.cellSize.y* HaveClear_trf.childCount+ (HaveRect.offsetMax.y*2)));
              
              
            }
        }
        else
        {
            Frame_obj.SetActive(false);
        }
        float y = NotClearRect.anchoredPosition.y + -(NotClears_trf.childCount * NotClear.cellSize.y);
        Control.offsetMin = new Vector2(0, y- HaveRect.offsetMin.y);
        Control.offsetMax = Vector2.zero;

    }

    public void Rename(bool isop)
    {
        if (isop)
        {
            ChangeName_obj.SetActive(true);
            Name_input.text = Role.Instance.RoleName;
        }
        else
        {
            ChangeName_obj.SetActive(false);
        }
      
    }

    /// <summary>
    /// 随机昵称
    /// </summary>
    public void RandomName()
    {
        string name = JsonMgr.GetSingleton().RandomName(Role.Instance.Sex);
        if (name.Length >0)
            Name_input.text = name;
        else
            return;
    }
    

    public void ChangePortrait(bool isopen)
    {
        if (isopen)
        {
            Portrait_obj.SetActive(true);
          
        }
        else
        {
            Portrait_obj.SetActive(false);
        }
    }

    public void Select(PortraitCircleView item)
    {
      
        if (item != null && Role.Instance.HeadId != item.ID)
        {
            if (item.isRelieve == 1)
            {
              Role.Instance.HeadId = item.ID;
                Portrait_img.sprite = ResourceMgr.Instance.LoadSprite(item.ID);
                for (int i = 0; i < PortraitList.Count; i++)
                {
                    if (PortraitList[i].ID == item.ID)
                    {
                        PortraitList[i].Hint.SetActive(true);
                        PortraitList[i].SelectImag.SetActive(true);
                    }
                    else
                    {
                        PortraitList[i].Hint.SetActive(false);
                        PortraitList[i].SelectImag.SetActive(false);
                    }
                }
            }
            else
            {
                CanvasView.Instance.AddNotice("请解锁头像！");
            }
           

        }
    }

          
/// <summary>
/// 实例化物品信息
/// </summary>
public GameObject InitItemInfo(bool isthat,Transform parent)
    {
        GameObject _item = null;
        if (isthat)
            _item = GameObject.Instantiate(Portrait.gameObject);
        else
            _item = GameObject.Instantiate(Frame.gameObject);

       
        _item.transform.SetParent(parent);
        _item.transform.localScale = Vector3.one;
        _item.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _item;
    }

    public void SelectSort()
    {

        int num = (int)(Mask_rect.sizeDelta.x / grid.cellSize.x);

        Clear.offsetMin = Vector2.zero;
        Clear.offsetMax = new Vector2((PortraitList.Count / num) * grid.cellSize.y, 0); 
        PortraitList.Sort((PortraitCircleView item1, PortraitCircleView item2) =>
            {
                if (item1.isRelieve > item2.isRelieve)
                {
                    return 1;
                }
                else if (item1.isRelieve < item2.isRelieve)
                {
                    return -1;
                }
                else
                {
                    if (item1.ID > item2.ID)
                    {
                        return 1;
                    }
                    else if ((item1.ID < item2.ID))
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            });
      }

    public void Replace(HeadFrameView item)
    {
       
        if (item.isOwn)
        {
            Frame_img.sprite = ResourceMgr.Instance.LoadSprite(item.id);
            item.isUse.SetActive(true);
            for (int i = 0; i < FrameList.Count; i++)
            {
                if (FrameList[i].id != item.id)
                {
                    FrameList[i].isUse.SetActive(false);
                }
            }
        }
        else
        {
            CanvasView.Instance.AddNotice("请解锁头像框！");
        }
       
       
    }


    AlphaTween at;
    /// <summary>
    /// 未实现功能
    /// </summary>
    public void Expect()
    {
        if (at == null)
            at = expect_img.GetComponent<AlphaTween>();
        at.ResetToBeginning();
        at.PlayForward();
    }
}
