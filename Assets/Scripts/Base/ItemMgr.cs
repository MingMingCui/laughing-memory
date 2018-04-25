using System.Collections.Generic;
using JsonData;

public class ItemMgr : Singleton<ItemMgr>
{
    public List<Item> itemList = new List<Item>();

    /// <summary>
    /// 添加或减少背包物品
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    public bool UpdateItem(int itemId, int itemNum)
    {
        int num = GetItemNum(itemId);
        if (num == 0)
        {
            Item _item = new Item();
            _item.itemId = itemId;
            _item.itemNum = itemNum;
            itemList.Insert(0, _item);
        }
        else
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                Item _item = itemList[i];
                if (_item.itemId == itemId)
                {
                    _item.itemNum = itemNum;
                    if (itemNum == 0)
                        itemList.RemoveAt(i);
                    if (itemNum > 9999)
                        _item.itemNum = 9999;
                }
            }
        }
        return (num == 0 || itemNum == 0);
    }


    /// <summary>
    /// 通过id获取数量
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetItemNum(int itemId)
    {
        int num = 0;
        if (itemList == null) num = 0;
        else
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].itemId == itemId)
                {
                    EDebug.Log(itemList[i].itemNum);
                    num = itemList[i].itemNum;
                    break;
                }
            }
        }
        return 999;
    }


    /// <summary>
    /// 接收服务器传过来的数据
    /// </summary>
    /// <param name="items"></param>
    public void ServerUpdateItemList(List<Item> items)
    {
        bool updateAllItem = false;
        for (int i = 0; i < items.Count; i++)
        {
            Item _item = items[i];
            if (UpdateItem(_item.itemId, _item.itemNum)) //添加或者删除了物品
            {
                updateAllItem = true;
            }
        }
        if (updateAllItem)
        {
            ZEventSystem.Dispatch(EventConst.UpdateItemList, itemList);
        }
        else
        {
            ZEventSystem.Dispatch(EventConst.UpdateItemParts, items);
        }
    }

    public void ItemSort()
    {
        itemList.Sort((Item item1, Item item2) =>
        {
            ItemConfig ic1 = JsonMgr.GetSingleton().GetItemConfigByID(item1.itemId);
            ItemConfig ic2 = JsonMgr.GetSingleton().GetItemConfigByID(item2.itemId);
            //道具物品类型不同时
            if (ic1.type < ic2.type)
            {
                return -1;   //左值小于右值,返回-1，为升序，如果返回1，就是降序
            }
            else if (ic1.type > ic2.type)
            {
                return 1;
            }
            else
            {
                //道具物品类型相同时，则按物品的稀有度进行排序
                if (ic1.rare  < ic2.rare)
                {
                    return 1;
                }
                else if (ic1.rare > ic2.rare)
                {
                    return -1;
                }
                else if (ic1.rare == ic2.rare)
                {
                    if (ic1.ID < ic2.ID)
                        return 1;
                    else if (ic1.ID > ic2.ID)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        });
    }
}