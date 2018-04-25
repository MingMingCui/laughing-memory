using JsonData;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SignInView: SignInViewBase
{
    public ItemUIView SignInViem;
    [HideInInspector]
    public Dictionary<int, ItemUIView> SignInItem = new Dictionary<int, ItemUIView>();
    [HideInInspector]
    public Transform ct =null;
    public void Init(int _id,bool _isUpdate,int _heroid)
    {
        if (AwardItem_obj.transform.childCount == 0 || _isUpdate)
        {
            if(_isUpdate)
            {
                foreach (var item in SignInItem)
                {
                    Destroy(item.Value.gameObject);
                    SignInItem.Remove(item.Key);
                }
            }
            Dictionary<int, Item> itemData = JsonMgr.GetSingleton().GetSignInItemData(_id);
            foreach (var item in itemData)
            {
                ItemUIView itemView = InitItemInfo().GetComponent<ItemUIView>();
                itemView.Init();
                itemView.SetInfo(item.Value.itemId, item.Value.itemNum); 
                SignInItem.Add(item.Key,itemView);
            }
        }
        Init(_heroid);
    }
    public GameObject InitItemInfo()
    {
        GameObject _item = GameObject.Instantiate(SignInViem.gameObject);
        _item.transform.SetParent(AwardItem_obj.transform);
        _item.transform.localScale = Vector3.one;
        _item.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _item;
    }
   public void Close()
    {
        if(ct.childCount > 0)
        {
            for (int i = 0, length = ct.childCount; i < length; ++i)
            {
                Destroy(ct.GetChild(0).gameObject);
            }
        }
    }

    public void SignIn(int _id)
    {
        int day = 0;   
        foreach (var item in SignInItem)
        {
            if(item.Key <= _id)
            {
                day++;
                item.Value.shade_obj.SetActive(true);
            }
        }
        signInNum_txt.text = day.ToString();
        if (Role.Instance.SignInID == _id) return;
        Role.Instance.SignInID = _id;
        GetAward_obj.SetActive(true);
        JObject singin =  JsonMgr.GetSingleton().Getactivity_sign(_id);
        ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(singin["ItemId"].ToObject<int>());
        itemlevelbg_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.rare - 1]);
        item_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
        num_txt.text = singin["ItemNum"].ToString();
      

    }

    public void Init(int _heroid)
    {
        if (ct == null)
        {
            ct = GameObject.Find("ProcessCtrl").transform;
        };
            if (ct.childCount > 0)
        {
            for (int i = 0, length = ct.childCount; i < length; ++i)
            {
                Destroy(ct.GetChild(0).gameObject);
            }
        }
        GameObject heroGo = ResourceMgr.Instance.LoadResource(_heroid) as GameObject;
        Hero hero = JsonMgr.GetSingleton().GetHeroByID(_heroid);
        string[] hint = hero.desc.Split(new char[2] { '#', ' ' });
        GeneralName_txt.text = hero.name;
        Hint_txt.text = hint[0];
        quarry_txt.text = hint[hint.Length-1];
        if (heroGo == null)
            return;
        heroGo = Instantiate(heroGo, ct, false);
        heroGo.transform.localPosition = new Vector3(0, -3, 0);
        heroGo.transform.localScale = new Vector3(2.5f,2.5f,2.5f);
        heroGo.SetLayer("Hero");
    }
}
