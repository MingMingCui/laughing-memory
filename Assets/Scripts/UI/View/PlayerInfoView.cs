using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoView : PlayerInfoViewBase {

    private const float NORMAL_HEIGHT = 955;

    private const float SMALL_HEIGHT = 735;

    public void SetViewSize(bool smallSize)
    {
        RectTransform rc = this.infos_obj.GetComponent<RectTransform>();
        rc.sizeDelta = new Vector2(rc.sizeDelta.x, smallSize ? SMALL_HEIGHT : NORMAL_HEIGHT);
        rc.anchoredPosition = Vector2.zero;
        this.corps_obj.SetActive(!smallSize);
    }
}
