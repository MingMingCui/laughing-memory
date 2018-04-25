using UnityEngine;
using UnityEngine.UI;

public class FullMask : MonoBehaviour
{

    private Image maskSp;
    private void Awake()
    {
        maskSp = GetComponent<Image>();
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        RectTransform rt = (RectTransform)transform;
        rt.sizeDelta = new Vector2(1920f,1080f);
    }

    public void SetClick(int uiID)
    {
        if (maskSp != null)
        {
            EventListener.Get(maskSp.gameObject).OnClick = e =>
            {
                UIFace.GetSingleton().Close((UIID)uiID);
            };
        }
    }
}
