using JsonData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitingAwardView : RecruitingAwardViewBase
{
    public ItemUIView itemUI = null;
    ItemUIView unititem = null;
    List<ItemUIView> itemUIList = new List<ItemUIView>();
    Skill ic;

    /// <summary>
    /// 显示单抽界面
    /// </summary>
    /// <param name="items"></param>
    public void ShowOneItem(Item items, int type)
    {
        if (items == null) return;
        RecruitingMgr.Instance.type = type;
        if ((int)RecruitingType.OrdinaryOne == type)
        {
            money_img.sprite = ResourceMgr.Instance.LoadSprite((int)CurrencyType.Copper);
            costprice_txt.text = ((int)LuckyDrawResoults.OUNITITEM).ToString();
            unitcostprice_txt.text = ((int)LuckyDrawResoults.OUNITITEM).ToString();
            unitmoney_img.sprite = ResourceMgr.Instance.LoadSprite((int)CurrencyType.Copper);
            moneyhundred_img.sprite = money_img.sprite;
            hundredcostprice_txt.text = ((int)LuckyDrawResoults.OHUNDRED).ToString();
            ic = JsonMgr.GetSingleton().GetSkillByID(11474);
            buyitemname_txt.text = "<color=#00FF00>" + "小经验丹" + "</color>";
            //    buyitemname_txt.text = JsonMgr.GetSingleton().GetSkillByID(11474).name;
            buyitemlevel_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.type - 1]);
        }
        else if ((int)RecruitingType.HighOne == type)
        {
            goldmoney_img.sprite = ResourceMgr.Instance.LoadSprite((int)CurrencyType.Gold);
            goldcostprice_txt.text = ((int)LuckyDrawResoults.HUNITITEM).ToString();
            ic = JsonMgr.GetSingleton().GetSkillByID(11473);
            buyitemlevel_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.type - 2]);
            buyitemname_txt.text = "<color=#00FFFF>" + "大经验丹" + "</color>";
        }
        buyitem_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);       
        buyitemnum_txt.text = "";
        if (unititem == null)
        {
            unititem = ItemInfo().GetComponent<ItemUIView>();
            unititem.Init();
            unititem.itemname_obj.SetActive(true);
        }
        unititem.gameObject.SetActive(true);
        unititem.isShow = true;
        unititem.SetInfo(items.itemId, items.itemNum);
        sendoneitem_obj.SetActive(true);
        sendtenitem_obj.SetActive(false);
        hundred_obj.SetActive(false);
        if (type == (int)RecruitingType.OrdinaryOne)
        {
            unitcopperbutton_obj.SetActive(true);
            copperbutton_obj.SetActive(false);
            goldbutton_obj.SetActive(false);
        }
        else if (type == (int)RecruitingType.HighOne)
        {
            unitcopperbutton_obj.SetActive(false);
            copperbutton_obj.SetActive(false);
            goldbutton_obj.SetActive(true);
        }
    }

    /// <summary>
    /// 显示十连抽界面
    /// </summary>
    public void ShowTenItem(List<Item> items,int type)
    {
        if (items == null) return;
        RecruitingMgr.Instance.type = type;
        if (type == (int)RecruitingType.OrdinaryHundred)
        {
            if (Role.Instance.Cash <= (int)LuckyDrawResoults.HUNDREDNUM)
                return;
        }
        if (type == (int)RecruitingType.OrdinaryTen || type == (int)RecruitingType.OrdinaryHundred)
        {
            money_img.sprite = ResourceMgr.Instance.LoadSprite((int)CurrencyType.Copper);
            costprice_txt.text = ((int)LuckyDrawResoults.OTENITEM).ToString();
            hundredcostprice_txt.text = ((int)LuckyDrawResoults.OHUNDRED).ToString();
            ic = JsonMgr.GetSingleton().GetSkillByID(11474);
            buyitemname_txt.text = "<color=#00FF00>" + "小经验丹" + "</color>";
            buyitemlevel_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.type - 1]);
        }
        else if(type == (int)RecruitingType.HighTen)
        {
            goldmoney_img.sprite = ResourceMgr.Instance.LoadSprite((int)CurrencyType.Gold);
            goldcostprice_txt.text = ((int)LuckyDrawResoults.HTENITEM).ToString();
            ic = JsonMgr.GetSingleton().GetSkillByID(11473);
            buyitemname_txt.text = "<color=#00FFFF>" + "大经验丹" + "</color>";
            buyitemlevel_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.type - 2]);
        }
        buyitem_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
        if (unititem != null)
            unititem.gameObject.SetActive(false);
        if (type == (int)RecruitingType.OrdinaryTen || type == (int)RecruitingType.HighTen)
        {
            sendoneitem_obj.SetActive(false);
            sendtenitem_obj.SetActive(true);
            hundred_obj.SetActive(false);
            buyitemnum_txt.text = ((int)LuckyDrawResoults.NUM).ToString();
        }
        else if (type == (int)RecruitingType.OrdinaryHundred)
        {
            sendoneitem_obj.SetActive(false);
            sendtenitem_obj.SetActive(false);
            hundred_obj.SetActive(true);
            buyitemnum_txt.text = ((int)LuckyDrawResoults.HUNDREDNUM).ToString();
        }
        CloseItem();
        ProcessCtrl.Instance.GoCoroutine("GetItem", GetItem(items, type));
    }

    IEnumerator GetItem(List<Item> items,int type)
    {
        float timer = 0.5f;
        if (type == (int)RecruitingType.OrdinaryHundred)
        {
            timer = 0.01f;
        }
        if (copperbutton_obj.activeSelf == true || goldbutton_obj.activeSelf == true || unitcopperbutton_obj.activeSelf == true)
        {
            unitcopperbutton_obj.SetActive(false);
            copperbutton_obj.SetActive(false);
            goldbutton_obj.SetActive(false);
        } 
        if (itemUIList.Count == items.Count)
        {
            for (int idx = 0; idx < itemUIList.Count; idx++)
            {
                yield return new WaitForSeconds(timer);
                itemUIList[idx].gameObject.SetActive(true);
                itemUIList[idx].isShow = true;
                itemUIList[idx].SetInfo(items[idx].itemId, items[idx].itemNum);
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                yield return new WaitForSeconds(timer);
                GameObject item = ItemInfo();
                ItemUIView itemsUIView = item.GetComponent<ItemUIView>();
                itemsUIView.Init();
                itemsUIView.itemname_obj.SetActive(true);
                itemsUIView.isShow = true;
                itemsUIView.SetInfo(items[i].itemId, items[i].itemNum);
                itemUIList.Add(itemsUIView);
            }
        }
        yield return new WaitForSeconds(0.2f);
        if (type == (int)RecruitingType.OrdinaryTen || type == (int)RecruitingType.OrdinaryHundred)
        {
            unitcopperbutton_obj.SetActive(false);
            copperbutton_obj.SetActive(true);
            goldbutton_obj.SetActive(false);
        }
        else if (type == (int)RecruitingType.HighTen)
        {
            unitcopperbutton_obj.SetActive(false);
            copperbutton_obj.SetActive(false);
            goldbutton_obj.SetActive(true);
        }
    }

    /// <summary>
    /// 实例化物品
    /// </summary>
    /// <returns></returns>
    public GameObject ItemInfo()
    {
        GameObject _items = Instantiate(itemUI.gameObject);
        if (RecruitingMgr.Instance.type == (int)RecruitingType.OrdinaryOne || RecruitingMgr.Instance.type == (int)RecruitingType.HighOne)
            _items.transform.SetParent(sendoneitem_obj.transform);
        else if (RecruitingMgr.Instance.type == (int)RecruitingType.OrdinaryTen || RecruitingMgr.Instance.type == (int)RecruitingType.HighTen)
            _items.transform.SetParent(sendtenitem_obj.transform);
        else if (RecruitingMgr.Instance.type == (int)RecruitingType.OrdinaryHundred)
            _items.transform.SetParent(hundreditem_obj.transform);
        _items.transform.localScale = Vector3.one;
        _items.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _items;
    }

    /// <summary>
    /// 再一次招募
    /// </summary>
    public void ClickMoreOne()
    {
        if (copperbutton_obj.activeSelf == true && costprice_txt.text == ((int)LuckyDrawResoults.OUNITITEM).ToString())
        {
            if (Role.Instance.Cash >= (int)LuckyDrawResoults.OUNITITEM)
            {
                ZEventSystem.Dispatch(EventConst.ShowOneLuckyDrawResults, RecruitingMgr.Instance._item, (int)RecruitingType.OrdinaryOne);
                Role.Instance.Cash -= (int)LuckyDrawResoults.OUNITITEM;
                Debug.Log(Role.Instance.Cash);
            }
        }
        else if (copperbutton_obj.activeSelf == true && costprice_txt.text == ((int)LuckyDrawResoults.OTENITEM).ToString())
        {
            if (Role.Instance.Cash >= (int)LuckyDrawResoults.OTENITEM)
            {
                ZEventSystem.Dispatch(EventConst.ShowTenLuckyDrawResults, RecruitingMgr.Instance.itemList, (int)RecruitingType.OrdinaryTen);
                Role.Instance.Cash -= (int)LuckyDrawResoults.OTENITEM;
                Debug.Log(Role.Instance.Cash);
            }
        }
        else if (unitcopperbutton_obj.activeSelf == true && unitcostprice_txt.text == ((int)LuckyDrawResoults.OUNITITEM).ToString())
        {
            if (Role.Instance.Cash >= (int)LuckyDrawResoults.OUNITITEM)
            {
                ZEventSystem.Dispatch(EventConst.ShowOneLuckyDrawResults, RecruitingMgr.Instance._item, (int)RecruitingType.OrdinaryOne);
                Role.Instance.Cash -= (int)LuckyDrawResoults.OUNITITEM;
                Debug.Log(Role.Instance.Cash);
            }
        }
        else if (goldbutton_obj.activeSelf == true && goldcostprice_txt.text == ((int)LuckyDrawResoults.HUNITITEM).ToString())
        {
            if (Role.Instance.Gold >= (int)LuckyDrawResoults.HUNITITEM)
            {
                ZEventSystem.Dispatch(EventConst.ShowOneLuckyDrawResults, RecruitingMgr.Instance._item, (int)RecruitingType.HighOne);
                Role.Instance.Gold -= (int)LuckyDrawResoults.HUNITITEM;
                Debug.Log(Role.Instance.Gold);
            }
        }
        else if (goldbutton_obj.activeSelf == true && goldcostprice_txt.text == ((int)LuckyDrawResoults.HTENITEM).ToString())
        {
            if (Role.Instance.Gold >= (int)LuckyDrawResoults.HTENITEM)
            {
                ZEventSystem.Dispatch(EventConst.ShowTenLuckyDrawResults, RecruitingMgr.Instance.itemList, (int)RecruitingType.HighTen);
                Role.Instance.Gold -= (int)LuckyDrawResoults.HTENITEM;
                Debug.Log(Role.Instance.Gold);
            }
        }
    }
    
    /// <summary>
    /// 关闭之前显示物品
    /// </summary>
    public void CloseItem()
    {
        if (itemUIList.Count == 0) return;
        for (int idx = 0; idx < itemUIList.Count; idx++)
        {
            itemUIList[idx].gameObject.SetActive(false);
        }
    }
}
