using JsonData;
using UnityEngine;
using UnityEngine.UI;

public class UIViewBase : MonoBehaviour
{
    protected GameObject go = null;
    public Canvas canvas { get; private set; }
    public GraphicRaycaster gr { get; private set; }
    public RectTransform rt { get; private set; }
    public FullMask mask { get; private set; }
    public UIConfig config { get; private set; }

    public void AddComponent(UIConfig uiJson)
    {
        config = uiJson;
        transform.SetParent(CanvasView.CanvasRoot, false);
        rt = GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;

        canvas = go.AddComponent<Canvas>();
        if (null != canvas)
        {
            canvas.overrideSorting = true;
            canvas.sortingLayerName = config.Layer;
        }
        gr = go.AddComponent<GraphicRaycaster>();
        if(gr !=null)
        {
            gr.ignoreReversedGraphics = true;
            gr.blockingObjects = GraphicRaycaster.BlockingObjects.TwoD;
        }
        if (config.Mask > 0)
        {
            if(mask == null)
            {
                Object obj = Resources.Load("Prefab/UI/ui_public/ui_mask");
                GameObject goMask = Instantiate(obj, transform) as GameObject;
                goMask.transform.SetSiblingIndex(0);
                mask = goMask.GetComponent<FullMask>();
            }
            if (config.Mask == 2)
            {
                mask.SetClick(config.ID);
            }
        }
    }
    public void SetOrder(int order)
    {
        canvas.sortingOrder = order;
    }

    public void SetView(bool isOpen)
    {
        go.SetLayer(isOpen ? "UI" : "HideUI");
        gr.enabled = isOpen ? true : false;
    }
}
