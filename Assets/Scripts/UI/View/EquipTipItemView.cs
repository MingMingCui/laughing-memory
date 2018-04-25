using JsonData;
using UnityEngine;
using UnityEngine.UI;

public class EquipTipItemView : MonoBehaviour
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Text name_txt;
    [HideInInspector]
    public Image border_img;
    [HideInInspector]
    public Image item_img;
    [HideInInspector]
    public Text advanveattr_txt;
    public virtual void Awake()
    {
        this.mTransform = this.transform;
        name_txt = mTransform.Find("name_txt").GetComponent<Text>();
        border_img = mTransform.Find("Image/border_img").GetComponent<Image>();
        item_img = mTransform.Find("Image/item_img").GetComponent<Image>();
        advanveattr_txt = mTransform.Find("Image (1)/advanveattr_txt").GetComponent<Text>();
    }

    public void SetView(int itemID)
    {
        ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(itemID);
        if (ic == null)
            return;
        name_txt.text = ic.name;
        border_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.rare - 1]);
        item_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
        advanveattr_txt.text = ic.propertydes;
    }
}
