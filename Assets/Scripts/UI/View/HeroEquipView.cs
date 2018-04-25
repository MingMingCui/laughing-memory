using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroEquipView : MonoBehaviour
{
    [System.Serializable]
    public class EquipImage
    {
        public EquipPart part;
        public Image image;
        public CanvasRenderer cr;
        public CanvasRenderer floor;
        public Text level;
    }
    public EquipImage[] partArray;

    private Dictionary<EquipPart, EquipImage> partDic;
    private List<EquipPart> dontTakePart;
    private void Awake()
    {
        partDic = new Dictionary<EquipPart, EquipImage>();
        dontTakePart = new List<EquipPart>();
        for (int i = 0; i < partArray.Length; ++i)
        {
            partDic.Add(partArray[i].part, partArray[i]);
            EventListener.Get(partArray[i].cr.gameObject).OnClick = e =>
            {
                //待处理 有可装备
            };
        }
    }


    public void SetView(HeroData heroData)
    {
        dontTakePart.Clear();
        EquipData[] equips = heroData.GetEquip();
        for (int i = 0; i < partDic.Count; ++i)
        {
            EquipData data = equips[i];
            EquipPart part = (EquipPart)(i + 1);
            if (data == null)
            {
                dontTakePart.Add(part);
                partDic[part].image.gameObject.SetActive(false);
                partDic[part].level.text = "";
                continue;
            }
            partDic[part].image.gameObject.SetActive(true);
            EventListener.Get(partArray[i].image.gameObject).OnClick = e =>
            {
                if (data == null)
                    return;
                UIFace.GetSingleton().Open(UIID.StrengthenTip, data);
            };
            if (partDic.ContainsKey(part))
            {
                int id = JsonMgr.GetSingleton().GetItemConfigByID(data.EquipId).icon;
                partDic[part].image.sprite = ResourceMgr.Instance.LoadSprite(id);
                partDic[part].level.text = data.StrengthenLv > 0 ? "+" + data.StrengthenLv : "";
                if(partDic[part].floor != null)
                    partDic[part].floor.SetAlpha(data.StrengthenLv);
            }
        }
        for (int i = 0; i < dontTakePart.Count; i++)
        {
            string md5 = EquipMgr.GetSingleton().TestCanDress(dontTakePart[i], heroData);
            partDic[dontTakePart[i]].cr.SetAlpha(md5 != "" ? 1 : 0);
        }    
    }
    private void OnDestroy()
    {
        partDic = null;
    }
}
