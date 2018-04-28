using UnityEngine.UI;
using UnityEngine;
using JsonData;

public class MateralItem : MonoBehaviour
{
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Image Material_img;
    [HideInInspector]
    public Image MaterialBorder_img;
    [HideInInspector]
    public Text num_txt;
    public virtual void Awake()
    {
        this.mTransform = this.transform;
        Material_img = mTransform.Find("Material_img").GetComponent<Image>();
        MaterialBorder_img = mTransform.Find("MaterialBorder_img").GetComponent<Image>();
        num_txt = mTransform.Find("num_txt").GetComponent<Text>();
    }
    Color c;

    public void SetView(MaterialSpend material)
    {
        ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(material.material);
        if (ic == null)
            return;
        MaterialBorder_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.rare - 1]);
        Material_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
        int haveNum = ItemMgr.Instance.GetItemNum(material.material);
        if(num_txt.color != Color.red)
            c = num_txt.color;
        if (ic.type == FuncType.EQUIP)
            haveNum = 1;
        num_txt.text = haveNum + "/" + material.num;
        if (haveNum < material.num)
            num_txt.color = Color.red;
        else
            num_txt.color = c;
    }
}
