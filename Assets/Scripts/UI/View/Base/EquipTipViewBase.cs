using UnityEngine.UI;
using UnityEngine;

public class EquipTipViewBase : UIViewBase
{

    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public ScrollRect equip_sr;
    public virtual void Awake()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
        equip_sr = mTransform.Find("equip_sr").GetComponent<ScrollRect>();
    }
}
