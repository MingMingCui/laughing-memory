using JsonData;
using UnityEngine;


public class NavigationView : NavigationViewBase
{
    private AlphaTween at;

    public GameObject item;

    public void Open()
    {
        Name_txt.text = Role.Instance.RoleName;
        Head_img.sprite = ResourceMgr.Instance.LoadSprite(Role.Instance.HeadId);
        grade_txt.text = Role.Instance.Level.ToString();
        Fighting_txt.text = "6666666";
        EventListener.Get(exit_btn.gameObject).OnClick = e =>
        {
            ZEventSystem.Dispatch(EventConst.NAVIGATIONBACK);
        };
    }

    public void ShowView(UIConfig config)
    {
        int[] itemList = config.Itemid;
        int count = ItemParent_trf.childCount;
        if(count > itemList.Length)
        {
            for (int i = itemList.Length; i < count; ++i)
            {
                DestroyImmediate(ItemParent_trf.GetChild(i).gameObject);
            }
        }
        for (int i = 0,child = 0; i < itemList.Length; i++,child++)
        {
            Transform tf;
            if(i < count)
                tf = ItemParent_trf.GetChild(child);

            else
                tf = Instantiate(item, ItemParent_trf, false).transform;
            NavigationItemView ni = tf.GetComponent<NavigationItemView>();
            ni.SetView(itemList[i]);
            
            EventListener.Get(ni.add_btn.gameObject).OnClick = e =>
            {
                Expect();
            };
        }
        if(config.Back < 2)
        {
            Head_obj.SetActive(config.Back == 0);
            exitUI_obj.SetActive(config.Back == 1);
        }
        if (null != base.canvas)
        {
            base.canvas.overrideSorting = true;
            base.canvas.sortingLayerName = config.Layer;
            //order最大值与最小值  -32768 和 32767
            base.canvas.sortingOrder = config.Back >= 2 ? -32768 : 32767; 
        }
    }

    /// <summary>
    /// 敬请期待
    /// </summary>
    public void Expect()
    {
        if (at == null)
            at = expect_img.GetComponent<AlphaTween>();
        at.ResetToBeginning();
        at.PlayForward();
    }
}      
