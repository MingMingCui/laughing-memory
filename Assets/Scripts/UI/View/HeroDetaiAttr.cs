using UnityEngine;
using UnityEngine.UI;

public class HeroDetaiAttr : MonoBehaviour
{
    private Text[] texts;

    private void Awake()
    {
        texts = GetComponentsInChildren<Text>();
    }

    public void AttrTxet(ScrollRect rect,Attr attr, float num)
    {
        EventListener.Get(gameObject).BegineDragEvent = e =>
        {
            rect.OnBeginDrag(e);
        };
        EventListener.Get(gameObject).DragEvent = e =>
        {
            rect.OnDrag(e);
        };
        EventListener.Get(gameObject).EndDragEvent = e =>
        {
            rect.OnEndDrag(e);
        };
            
        texts[0].text = string.Format("<color=#FFFF00>{0}</color>", AttrUtil.GetAttribute(attr));
        texts[1].text = string.Format("<color=#4AE000>{0}</color>", AttrUtil.ShowText(attr, num));
    }
    public void Clear()
    {
        texts[0].text = texts[1].text = "";
    }

}
