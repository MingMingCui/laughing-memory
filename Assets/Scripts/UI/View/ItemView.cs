using JsonData;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 道具类型
/// </summary>
public enum FuncType
{
    CONSUMABLES = 1,// 消耗品
    EQUIP = 2, // 装备
    MATERIAL = 3,//材料
    ITEMS = 4,//镶嵌道具
    PIECES =5,//英雄碎片
    CURRENCY = 6,//货币
    HERO =7,//英雄
    OTHER = 8,//其他
}

/// <summary>
/// 稀有度类型
/// </summary>
public enum RareType
{
    RareWhite =1,
    RareGreen,
    RareBlue,
    RarePurple,
    RareOrange
}

public class ItemView : ItemViewBase
{
    private FuncType Ptype = 0;//当前物品栏里物品的类型
    int curId;//当前点击物品的id

    private const int UNITROWNUM = 5; //每行物品个数
    private const int UNITITEMSIZE = 170; //单个物品大小

    public ItemUIView itemUIView;
    public List<ItemUIView> itemUIList = new List<ItemUIView>();

    /// <summary>
    /// 更新物品信息
    /// </summary>
    /// <param name="itemList"></param>
    public void UpdateItemView(List<Item> itemList)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList.Count > itemUIList.Count )//创建物品
            {
                GameObject item = InitItemInfo();
                ItemUIView itemUIView = item.GetComponent<ItemUIView>();
                itemUIView.Init();
                itemUIList.Add(itemUIView);
            }
            else if(itemUIList.Count> itemList.Count)//隐藏物品
            {
                for (int idx = 0; idx < itemUIList.Count; idx++)
                {
                    if (i >= itemList.Count)
                    {
                        itemUIList[idx].gameObject.SetActive(false);
                        itemUIList[idx].isRegister = false;
                    }
                }
            }
        }
        //当UI中存在该物品时
        if (itemUIList != null)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                for (int j = 0; j < itemUIList.Count; j++)
                {
                    if (!itemUIList[j].gameObject.activeSelf)
                    {
                        itemUIList[j].gameObject.SetActive(true);
                        itemUIList[j].SetInfo(itemList[i].itemId, itemList[i].itemNum);
                    }
                }
            }
        }
        Collating(false);
    }

    public void UpdateItemParts(List<Item> items)
    {
        for (int i = 0; i < itemUIList.Count; i++)
        {
            for (int j = 0; j < items.Count; j++)
            {
                if (items[j].itemId == itemUIList[i].itemUIid)
                {
                    quantity_txt.text = items[j].itemNum.ToString();
                    itemUIList[i].num_txt.text = items[j].itemNum.ToString();
                }
            }
        }
    }

    /// <summary>
    /// 实例化物品信息
    /// </summary>
    public GameObject InitItemInfo()
    {
        GameObject _item = GameObject.Instantiate(itemUIView.gameObject);
        _item.transform.SetParent(gridlist_obj.transform);
        _item.transform.localScale = Vector3.one;
        _item.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return _item;
    }

    /// <summary>
    /// 点击全部按钮
    /// </summary>
    /// <param name="isClick"></param>
    public void ClickAll()
    {
        if (ItemMgr.Instance.itemList != null)
        {
            Ptype = 0;
            CollatingType(Ptype);
        }
    }
    /// <summary>
    /// 点击消耗品
    /// </summary>
    /// <param name="isClick"></param>
    public void ClickConsumables()
    {
        Ptype = FuncType.CONSUMABLES;
        CollatingType(Ptype);
    }
    /// <summary>
    /// 点击装备
    /// </summary>
    /// <param name="isClick"></param>
    public void ClickEquip()
    {
        Ptype = FuncType.EQUIP;
        CollatingType(Ptype);
    }

    /// <summary>
    /// 点击材料
    /// </summary>
    /// <param name="isClick"></param>
    public void ClickMaterial()
    {
        Ptype = FuncType.MATERIAL;
        CollatingType(Ptype);
    }

    /// <summary>
    /// 点击其它
    /// </summary>
    /// <param name="isClick"></param>
    public void ClickOther()
    {
        Ptype = FuncType.OTHER;
        CollatingType(Ptype);
    }


    /// <summary>
    /// 整理背包物品
    /// </summary>
    public void Collating(bool isSort)
    {
        if (itemUIList.Count == 0 ) return;
        int row = 0;//行数
        if (isSort)
        {
            ItemMgr.Instance.ItemSort();  //排序
        }

        //物品更新
        for (int i = 0; i < itemUIList.Count; i++)
        {
            if (i >= ItemMgr.Instance.itemList.Count)
            {
                itemUIList[i].gameObject.SetActive(false);
            }
            else
            {
                itemUIList[i].itemUIid = ItemMgr.Instance.itemList[i].itemId;
                itemUIList[i].num_txt.text = ItemMgr.Instance.itemList[i].itemNum.ToString();
                ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(ItemMgr.Instance.itemList[i].itemId);
                itemUIList[i].itemLevelbg_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.rare - 1]);
                itemUIList[i].itemImg_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
            }
            if (Ptype == 0 && i < ItemMgr.Instance.itemList.Count)
            {
                itemUIList[i].gameObject.SetActive(true);
            }
            else if (Ptype != 0 && i == itemUIList.Count - 1)
            {
                CollatingType(Ptype);
            }
        }
        if (Ptype == 0)
            ItemPop(itemUIList);
        //背包物品滑动框的大小
        row = ItemMgr.Instance.itemList.Count / UNITROWNUM;
        if (row > 3)
        {
            gridlist_obj.GetComponent<RectTransform>().offsetMin = new Vector2(0, -(row - 3) * UNITITEMSIZE);
            gridlist_obj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        }
    }

    /// <summary>
    /// 整理某一类型的物品
    /// </summary>
    public void CollatingType(FuncType type)
    {
        List<ItemUIView> items = null;
        Ptype = type;
        int row = 0;//行数 
        if (type == 0)
        {
            Collating(false);
            return;
        }
        else
        {
            items = GetItemsByType(type);
        }
        if (items.Count != 0)
        {
            ItemPop(items);
        }
        else
            inventorypop_obj.gameObject.SetActive(false);

        //背包物品滑动框的大小
        row = items.Count / UNITROWNUM;
        if (row < 4) row = 0;
        else row = row - 3;
        gridlist_obj.GetComponent<RectTransform>().offsetMin = new Vector2(0, -row * UNITITEMSIZE);
        gridlist_obj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
    }

    /// <summary>
    /// 点击物品时，对物品弹框赋值
    /// </summary>
    /// <param name="item"></param>
    public void ClickItem(ItemUIView item)
    {
        if (item == null) return;
        ItemUIView itemUI = null;
        for (int i = 0; i < itemUIList.Count; i++)
        {
            if (item.itemUIid == itemUIList[i].itemUIid)
            {
                itemUI = itemUIList[i];
            }
        }
        if (curId == itemUI.itemUIid)
        {
            quantity_txt.text = item.num_txt.text;
        }
        else
        {
            curId = itemUI.itemUIid;
            itemname_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(itemUI.itemUIid).name;
            quantity_txt.text = item.num_txt.text;
            ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(itemUI.itemUIid);
            itemiconcolor_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.rare - 1]);
            itemicon_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
            propertydes_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(itemUI.itemUIid).propertydes;
            use_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(itemUI.itemUIid).usedes;
            unitprice_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(itemUI.itemUIid).price.ToString();
        }
        inventorypop_obj.SetActive(true);
        if (JsonMgr.GetSingleton().GetItemConfigByID(itemUI.itemUIid).type == FuncType.CONSUMABLES)
        {
            use_btn.gameObject.SetActive(true);
            details_btn.gameObject.SetActive(false);
        }
        else
        {
            use_btn.gameObject.SetActive(false);
            details_btn.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 在物品弹框点击出售，弹出出售框
    /// </summary>
    public void ClickSale()
    {
        salepop_obj.SetActive(true);
        saleitemiconcolor_img.sprite = itemiconcolor_img.sprite;
        saleitemicon_img.sprite = itemicon_img.sprite;
        saleitemname_txt.text = itemname_txt.text;
        salequantity_txt.text = quantity_txt.text;
        saleunitprice_txt.text = unitprice_txt.text;
        getmoneyprice_txt.text = unitprice_txt.text;
        select_txt.text = "1";
        total_txt.text = "/ " + quantity_txt.text;
    }

    /// <summary>
    /// 关闭出售框
    /// </summary>
    public void ClickSaleColse()
    {
        salepop_obj.SetActive(false);
        inventorypop_obj.SetActive(true);
    }

    /// <summary>
    /// 点击减少和增加数量按钮
    /// </summary>
    public void ClickSubAndAdd(bool isClick)
    {
        int s = int.Parse(select_txt.text);
        int price = int.Parse(unitprice_txt.text);
        if (isClick)
        {
            s--;
            if (s <= 0)
            {
                s = 1;
                select_txt.text = s.ToString();
            }
            else select_txt.text = s.ToString();

            price = price * s;
            getmoneyprice_txt.text = price.ToString();

        }
        else
        {
            s++;
            if (s >= int.Parse(salequantity_txt.text))
            {
                s = int.Parse(salequantity_txt.text);
                select_txt.text = s.ToString();
            }
            else select_txt.text = s.ToString();
            price = price * s;
            getmoneyprice_txt.text = price.ToString();
        }
    }

    /// <summary>
    /// 点击最大值数量按钮
    /// </summary>
    public void ClickMax()
    {
        select_txt.text = quantity_txt.text;
        int s = int.Parse(select_txt.text);
        int num = int.Parse(unitprice_txt.text);
        getmoneyprice_txt.text = (s * num).ToString();
    }

    /// <summary>
    /// 点击确认出售按钮
    /// </summary>
    public void ClickConfirmsale()
    {
        //模拟服务器
        List<Item> items = new List<Item>();
        int s = int.Parse(select_txt.text);
        for (int i = 0; i < ItemMgr.Instance.itemList.Count ; i++)
        {
            if(ItemMgr.Instance.itemList[i].itemId == curId)
            {
                items.Add(new Item());
                items[0].itemId = curId;
                items[0].itemNum = ItemMgr.Instance.itemList[i].itemNum - s;
            }
        }
        //接收服务器传过来的物品
        ItemMgr.Instance.ServerUpdateItemList(items);
        //关闭出售框
        salepop_obj.SetActive(false);
    }

    /// <summary>
    /// 减少物品
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    public void SubItem(int itemId)
    {
        for (int i = 0; i < itemUIList.Count; i++)
        {
            if (ItemMgr.Instance.itemList[i].itemId == itemId)
            {
                ItemMgr.Instance.itemList.Remove(ItemMgr.Instance.itemList[i]);
                itemUIList[i].gameObject.SetActive(false);
                UpdateItemView(ItemMgr.Instance.itemList);
                break;
            }
        }
    }

    /// <summary>
    /// 点击背包按钮，背包内有物品时默认出现物品弹框
    /// </summary>
    public void ItemPop(List<ItemUIView> itemUIList)
    {
        if (itemUIList.Count != 0)
        {
            inventorypop_obj.gameObject.SetActive(true);
            curId = itemUIList[0].itemUIid;
            SelectItemEffect(itemUIList[0]);
            ItemConfig ic = JsonMgr.GetSingleton().GetItemConfigByID(itemUIList[0].itemUIid);
            itemiconcolor_img.sprite = ResourceMgr.Instance.LoadSprite(ColorMgr.Border[ic.rare - 1]);
            itemicon_img.sprite = ResourceMgr.Instance.LoadSprite(ic.icon);
            itemname_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(itemUIList[0].itemUIid).name;
            quantity_txt.text = itemUIList[0].num_txt.text;

            propertydes_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(itemUIList[0].itemUIid).propertydes;
            use_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(itemUIList[0].itemUIid).usedes;
            unitprice_txt.text = JsonMgr.GetSingleton().GetItemConfigByID(itemUIList[0].itemUIid).price.ToString();
            if (JsonMgr.GetSingleton().GetItemConfigByID(itemUIList[0].itemUIid).type == FuncType.CONSUMABLES)
            {
                use_btn.gameObject.SetActive(true);
                details_btn.gameObject.SetActive(false);
            }
            else
            {
                details_btn.gameObject.SetActive(true);
                use_btn.gameObject.SetActive(false);
            }
        }
        else return;
    }

    /// <summary>
    /// 点击物品出现选中物品光效
    /// </summary>
    /// <param name="item"></param>
    public void SelectItemEffect(ItemUIView item)
    {
        for (int i = 0; i < itemUIList.Count; i++)
        {
            if (item.itemUIid == itemUIList[i].itemUIid)
            {
                itemUIList[i].itemselectbg_obj.SetActive(true);
            }
            else
            {
                itemUIList[i].itemselectbg_obj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 点击物品，生成物品弹框
    /// </summary>
    /// <param name="id"></param>
    public void SpawnItemPop(ItemUIView item)
    {
        SelectItemEffect(item);
        ClickItem(item);
    }   

    /// <summary>
    /// 通过类型获取物品
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<ItemUIView> GetItemsByType(FuncType type)
    {
        List<ItemUIView> itemUItype = new List<ItemUIView>();
        for (int i = 0; i < itemUIList.Count; i++)
        {
            if (i >= ItemMgr.Instance.itemList.Count) return itemUItype;
            if (JsonMgr.GetSingleton().GetItemConfigByID(itemUIList[i].itemUIid).type == type)
            {
                itemUIList[i].gameObject.SetActive(true);
                itemUItype.Add(itemUIList[i]);
            }
            else
            {
                itemUIList[i].gameObject.SetActive(false);
            }
        }
        return itemUItype;
    }

    /// <summary>
    /// 打开背包默认出现类型
    /// </summary>
    public void DefaultType()
    {
        total_tog.isOn = true;
        ClickAll();
    }

    /// <summary>
    /// 打开背包默认出现物品
    /// </summary>
    public void DefaultShow()
    {
        if (ItemMgr.Instance.itemList.Count != 0)
            UpdateItemView(ItemMgr.Instance.itemList);
    }
}