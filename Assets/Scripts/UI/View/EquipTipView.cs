using UnityEngine;

public class EquipTipView : EquipTipViewBase
{
    public GameObject item;
    private const float width = 249;

    public void Open(int[] targets)
    {
        for (int i = 0,length = targets.Length; i < length; ++i)
        {
            GameObject list = Instantiate(item, equip_sr.content);
            EquipTipItemView etv = list.GetComponent<EquipTipItemView>();
            etv.SetView(targets[i]);
            int id = targets[i];
            EventListener.Get(etv.item_img.gameObject).OnClick = e =>
            {
                PlayerPrefs.SetInt("advancedtarget", id);
                UIFace.GetSingleton().Close(UIID.EuqipTipCtrl);
            };
            EventListener.Get(etv.item_img.gameObject).BegineDragEvent = e =>
            {
                equip_sr.OnBeginDrag(e);
            };
            EventListener.Get(etv.item_img.gameObject).EndDragEvent = e =>
            {
                equip_sr.OnEndDrag(e);
            };
            EventListener.Get(etv.item_img.gameObject).DragEvent = e =>
            {
                equip_sr.OnDrag(e);
            };
        }
        Vector2 size = equip_sr.content.sizeDelta;
        size.x = width * targets.Length + 15f;
        equip_sr.content.sizeDelta = size;
    }
    public void Close()
    {
        for (int i = 0,length = equip_sr.content.childCount; i < length; i++)
        {
            DestroyImmediate(equip_sr.content.GetChild(0).gameObject);
        }
        ZEventSystem.Dispatch(EventConst.UPSTRENGTHENVIEW);
    }
}
