using JsonData;
using UnityEngine;
using UnityEngine.UI;

public class NavigationItemView : MonoBehaviour
{

    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Text Num_txt;
    [HideInInspector]
    public Image icon_img;
    [HideInInspector]
    public Button add_btn;
    public virtual void Awake()
    {
        this.mTransform = this.transform;
        Num_txt = mTransform.Find("Num_txt").GetComponent<Text>();
        icon_img = mTransform.Find("icon_img").GetComponent<Image>();
        add_btn = mTransform.Find("add_btn").GetComponent<Button>();
    }

    public void SetView(int itemID)
    {
        ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(itemID);
        if (ic == null)
            return;
        icon_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
        Num_txt.text = Role.Instance.GetItemNum((ItemID)itemID).ToString();
    }
}
